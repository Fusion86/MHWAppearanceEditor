using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>();

    //[GitVersion] readonly GitVersion GitVersion;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Which .NET framework to use, can be either 'netcoreapp3.0' or 'net461'. Release always uses 'net461'.")]
    string Framework = "netcoreapp3.0";

    [Parameter("Which runtime to use. Release always uses 'win7-x64'.")]
    string Runtime = "any";

    static readonly AbsolutePath AppearanceEditorProject = RootDirectory / "src" / "MHWAppearanceEditor" / "MHWAppearanceEditor.csproj";
    static readonly AbsolutePath OdogaronProject = RootDirectory / "src" / "Odogaron" / "Odogaron.csproj";
    static readonly AbsolutePath ScriptsDirectory = RootDirectory / "scripts";
    static readonly AbsolutePath CharacterAssetsGen = ScriptsDirectory / "character_assets.py";
    static readonly AbsolutePath PaletteExtractor = ScriptsDirectory / "palette_extractor.py";
    static readonly AbsolutePath InnoSetupConfig = ScriptsDirectory / "installer.iss";

    AbsolutePath OutputDirectory => RootDirectory / "output";

    private static Lazy<string> PythonPath = new Lazy<string>(() =>
    {
        try
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "--version",
                RedirectStandardError = true,
                UseShellExecute = false,
            });

            var stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (stderr.Contains("Python 2."))
                return "python3";
        }
        catch (Exception) { }

        return "python";
    });

    private static void ExecPython(string path) => Exec(PythonPath.Value, path);

    private static void Exec(string program, string args)
    {
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = program,
            Arguments = args
        });

        process.WaitForExit();
        Logger.Info("Exit code: " + process.ExitCode);

        if (process.ExitCode != 0)
            throw new Exception("Got a non-zero exit code!");
    }

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(OdogaronProject));
        });

    Target RestoreOdogaron => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(AppearanceEditorProject));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .DependsOn(CompileOdogaron)
        .Executes(() =>
        {
            // Force net461 for a Release
            if (ExecutingTargets.Contains(Release))
            {
                Framework = "net461";
                Runtime = "win7-x64";
                Configuration = Configuration.Release;
            }

            DotNetBuild(s => s
                .SetProjectFile(AppearanceEditorProject)
                .SetConfiguration(Configuration)
                // TODO: This also sets the Cirilla.Core version, which we DO NOT want
                //.SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
                //.SetFileVersion(GitVersion.GetNormalizedFileVersion())
                //.SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore()
                .SetOutputDirectory(OutputDirectory)
                .SetFramework(Framework)
                .SetRuntime(Runtime));

            // Move DLLs to lib folder and remove leftover files
            string libDir = OutputDirectory / "lib";
            EnsureExistingDirectory(libDir);

            foreach (var dll in GlobFiles(OutputDirectory, "*.dll"))
                MoveFileToDirectory(dll, libDir);

            foreach (var file in GlobFiles(OutputDirectory, "*.exe.config", "*.pdb", "*.deps.json"))
                DeleteFile(file);
        });

    Target CompileOdogaron => _ => _
        .DependsOn(RestoreOdogaron)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(OdogaronProject)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetOutputDirectory(OutputDirectory));
        });

    Target GenerateCharacterAssets => _ => _
        .Executes(() =>
        {
            ExecPython(CharacterAssetsGen);
        });

    Target GenerateColorPalette => _ => _
        .Executes(() =>
        {
            ExecPython(PaletteExtractor);
        });

    Target CopyAssets => _ => _
        .After(GenerateColorPalette)
        .After(GenerateCharacterAssets)
        .After(Clean)
        .Executes(() =>
        {
            var characterAssetsDir = OutputDirectory / "assets";

            if (DirectoryExists(characterAssetsDir))
                DeleteDirectory(characterAssetsDir);

            CopyDirectoryRecursively(ScriptsDirectory / "assets", characterAssetsDir);

            if (!FileExists(characterAssetsDir / "character_assets.json"))
                throw new Exception("character_assets.json is missing!");

            if (!FileExists(characterAssetsDir / "skin_color.png"))
                throw new Exception("skin_color.png is missing!");

            if (!FileExists(characterAssetsDir / "palette.json"))
                throw new Exception("palette.json is missing!");
        });

    Target EnsureSecretsAreSet => _ => _
        .Before(Compile)
        .Executes(() =>
        {
            var code = File.ReadAllText(RootDirectory / "src" / "MHWAppearanceEditor" / "SuperSecret.cs");
            if (code.Contains("\"\""))
                throw new Exception("Something is not set!");
        });

    Target Release => _ => _
        .Description("Create a release running on the net461 platform.")
        .DependsOn(EnsureSecretsAreSet, Clean, Compile, CopyAssets, CreateInstaller)
        .After(Compile);

    Target Full => _ => _
        .Description("Same as Release, but also rebuild all assets.")
        .DependsOn(GenerateCharacterAssets, GenerateColorPalette, Release);

    Target CreateInstaller => _ => _
        .Description("Create installer with Inno Setup")
        .After(Compile, CopyAssets)
        .Executes(() =>
        {
            Exec(@"C:\Program Files (x86)\Inno Setup 6\ISCC.exe", $"{InnoSetupConfig}");
        });
}
