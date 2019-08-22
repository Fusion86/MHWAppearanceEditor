using System;
using System.Diagnostics;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
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

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Which .NET framework to use, can be either 'netcoreapp3.0' or 'net461'. Release always uses 'net461'.")]
    string Framework = "netcoreapp3.0";

    readonly AbsolutePath Project = RootDirectory / "MHWAppearanceEditor" / "MHWAppearanceEditor.csproj";
    readonly AbsolutePath StylesListGenPath = RootDirectory / "scripts" / "styles_list_gen.py";

    AbsolutePath OutputDirectory => RootDirectory / "output";

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
                .SetProjectFile(Project));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            // Force net461 for a Release
            if (ExecutingTargets.Contains(Release))
            {
                Framework = "net461";
                Configuration = Configuration.Release;
            }

            DotNetBuild(s => s
                .SetProjectFile(Project)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetOutputDirectory(OutputDirectory)
                .SetFramework(Framework));
        });

    Target GenerateCharacterAssets => _ => _
        .Executes(() =>
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "python",
                Arguments = StylesListGenPath
            });

            process.WaitForExit();
            Logger.Info("styles_list_gen exit code: " + process.ExitCode);

            if (process.ExitCode != 0)
                throw new Exception("styles_list_gen has a non-zero exit code!");
        });

    Target CopyCharacterAssets => _ => _
        .After(GenerateCharacterAssets)
        .After(Clean)
        .Executes(() =>
        {
            var characterAssetsDir = OutputDirectory / "character_assets";

            if (DirectoryExists(characterAssetsDir))
                DeleteDirectory(characterAssetsDir);

            CopyDirectoryRecursively(StylesListGenPath.Parent / "styles_list_gen", characterAssetsDir);

            if (!FileExists(characterAssetsDir / "assets.json"))
                throw new Exception("character_assets is incomplete!");
        });

    Target EnsureSecretsAreSet => _ => _
        .Before(Compile)
        .Executes(() =>
        {

        });

    Target Release => _ => _
        .Description("Create a release running on the net461 platform.")
        .DependsOn(Clean, Compile, CopyCharacterAssets, EnsureSecretsAreSet);

    Target Full => _ => _
        .Description("Same as Release, but also rebuild all character_assets.")
        .DependsOn(GenerateCharacterAssets, Release);
}
