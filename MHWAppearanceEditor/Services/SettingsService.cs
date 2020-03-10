using Newtonsoft.Json;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.Services
{
    public class SettingsService : ReactiveObject
    {
        private static readonly ILogger log = Log.ForContext<SettingsService>();

        [Reactive] public AppSettings Settings { get; private set; } = new AppSettings();

        private readonly string configPath;
        private readonly AsyncLock ioLock = new AsyncLock();

        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, MissingMemberHandling = MissingMemberHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate };

        public SettingsService(string configPath = "config.json")
        {
            this.configPath = configPath;

            if (File.Exists(configPath))
                Load().WaitAndUnwrapException();
            else
                Save().WaitAndUnwrapException();
        }

        public async Task Load()
        {
            using (await ioLock.LockAsync())
            {
                try
                {
                    string json = File.ReadAllText(configPath);
                    Settings = JsonConvert.DeserializeObject<AppSettings>(json);
                }
                catch (Exception ex)
                {
                    log.Error("Couldn't load settings: " + ex.Message);
                }
            }
        }

        public async Task Save()
        {
            using (await ioLock.LockAsync())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(Settings, serializerSettings);
                    File.WriteAllText(configPath, json);
                }
                catch (Exception ex)
                {
                    log.Error("Couldn't save settings: " + ex.Message);
                }
            }
        }
    }
}
