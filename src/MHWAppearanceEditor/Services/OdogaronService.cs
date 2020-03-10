using MHWAppearanceEditor.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;

namespace MHWAppearanceEditor.Services
{
    public class OdogaronService : ReactiveObject
    {
        private static readonly Serilog.ILogger log = Log.ForContext<OdogaronService>();
        private static readonly string EXE_PATH = Path.Combine(Directory.GetCurrentDirectory(), "odogaron.exe");

        [Reactive] public bool IsRunning { get; private set; }

        private Process? proc = null;
        private readonly AppSettings settings = Locator.Current.GetService<SettingsService>().Settings;

        public OdogaronService()
        {
            settings.WhenAnyValue(x => x.AutoEnableSaveCheckBypass).Where(x => x == true).Do(_ => Start()).Subscribe();
        }

        public void Start()
        {
            if (proc?.IsRunning() == true) return;
            if (proc == null) proc = CreateProcess();

            try
            {
                proc.Start();
                proc.BeginOutputReadLine();
                IsRunning = true;
                log.Information("Started odogaron.exe");
            }
            catch (Exception ex)
            {
                log.Error("Couldn't start odogaron: " + ex.Message);
            }
        }

        public void Stop()
        {
            if (proc?.IsRunning() == true)
            {
                proc.Kill();
                proc.WaitForExit();
                proc = null;
                IsRunning = false;
                log.Information("Stopped odogaron.exe");
            }
        }

        private Process CreateProcess()
        {
            var proc = new Process();
            proc.StartInfo.FileName = EXE_PATH;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.OutputDataReceived += OutputDataReceived;
            proc.Exited += OnExited; // For when it crashes I guess?
            //proc.StartInfo.Verb = "runas"; // Doesn't work when UseShellExecute = false
            proc.StartInfo.Arguments = "watch";
            return proc;
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // Ignore spam
            if (e.Data != "No MonsterHunterWorld process found.")
                log.Information(e.Data);
        }

        private void OnExited(object? sender, EventArgs e)
        {
            proc = null;
            IsRunning = false;
        }
    }
}
