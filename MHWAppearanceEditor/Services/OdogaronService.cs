using MHWAppearanceEditor.Extensions;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Diagnostics;
using System.Reactive.Linq;

namespace MHWAppearanceEditor.Services
{
    public class OdogaronService
    {
        private static readonly Serilog.ILogger log = Log.ForContext<OdogaronService>();
        private static readonly string EXE_PATH = "odogaron.exe";

        private Process? proc = null;
        private readonly AppSettings settings = Locator.Current.GetService<SettingsService>().Settings;

        public OdogaronService()
        {
            this.WhenAnyValue(x => x.settings.EnableOdogaronBackgroundRun)
                .Do(x =>
                {
                    if (x) Start();
                    else Stop();
                })
                .Subscribe();
        }

        public void Start()
        {
            if (proc?.IsRunning() == true) return;
            if (proc == null) proc = CreateProcess();

            proc.Start();
            proc.BeginOutputReadLine();
            log.Information("Started odogaron.exe");
        }

        public void Stop()
        {
            if (proc?.IsRunning() == true)
            {
                proc.Close();
                proc = null;
                log.Information("Stopped odogaron.exe");
            }
        }

        private Process CreateProcess()
        {
            var proc = new Process();
            proc.StartInfo.FileName = EXE_PATH;
            proc.StartInfo.UseShellExecute = false;
            proc.OutputDataReceived += OutputDataReceived;
            proc.StartInfo.Verb = "runas";
            return proc;
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            log.Information(e.Data);
        }
    }
}
