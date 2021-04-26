using Avalonia.Controls;
using Cirilla.Core.Enums;
using Cirilla.Core.Models;
using MHWAppearanceEditor.Extensions;
using MHWAppearanceEditor.Helpers;
using MHWAppearanceEditor.Models;
using MHWAppearanceEditor.ViewModels.Tabs;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.ViewModels.SaveSlotEditors
{
    public class SaveSlotToolsViewModel : ViewModelBase
    {
        private static readonly ILogger CtxLog = Log.ForContext<SaveSlotToolsViewModel>();

        // This is the SaveData from where we want to import from
        [Reactive] public SaveData SourceSaveData { get; private set; }
        public List<SaveSlotViewModel> SourceSaveSlots { [ObservableAsProperty]get; }

        public ReactiveCommand<Unit, Unit> SelectSaveDataCommand { get; }
        public ReactiveCommand<SaveSlot, SerializableAppearance> ImportFromSaveSlotCommand { get; }
        public ReactiveCommand<Unit, SerializableAppearance?> ImportFromCmpCommand { get; }
        public ReactiveCommand<Unit, SerializableAppearance?> ImportFromJsonCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportToCmpCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportToJsonCommand { get; }

        private readonly SaveSlot saveSlotContext; // aka target to where we apply the imported content

        public SaveSlotToolsViewModel(SaveSlot saveSlotContext)
        {
            this.saveSlotContext = saveSlotContext;

            SelectSaveDataCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(SelectSaveData);
            ImportFromSaveSlotCommand = ReactiveCommand.Create<SaveSlot, SerializableAppearance>(ImportFromSaveSlot);
            ImportFromCmpCommand = ReactiveCommand.CreateFromTask<Unit, SerializableAppearance?>(ImportFromCmp);
            ImportFromJsonCommand = ReactiveCommand.CreateFromTask<Unit, SerializableAppearance?>(ImportFromJson);
            ExportToCmpCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(ExportToCmp);
            ExportToJsonCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(ExportToJson);

            ImportFromSaveSlotCommand.Subscribe(ImportSerializableAppearance);
            ImportFromCmpCommand.Subscribe(ImportSerializableAppearance);
            ImportFromJsonCommand.Subscribe(ImportSerializableAppearance);

            this.WhenAnyValue(x => x.SourceSaveData)
                .Where(x => x != null)
                .Select(saveData => saveData.SaveSlots.Select(x => new SaveSlotViewModel(x)).ToList())
                .ToPropertyEx(this, x => x.SourceSaveSlots);

            // Set source context to currently opened SaveData
            SourceSaveData = this.saveSlotContext.SaveData;
        }

        private void ImportSerializableAppearance(SerializableAppearance? serializableAppearance)
        {
            if (serializableAppearance == null)
                return;

            MainWindowViewModel.Instance.ShowPopup("Imported appearance.\nRemember to click on Save when you are done!");
            serializableAppearance.ApplyToSaveSlot(saveSlotContext);

            CtxLog.Information("Updating CharacterAppearance.Type to 'Zero'");
            saveSlotContext.CharacterAppearance.Type = CharacterAppearanceType.Zero.Value;

            CtxLog.Information("Applied appearance changes");
        }

        private async Task<Unit> SelectSaveData(Unit _)
        {
            OpenFileDialog ofd = new OpenFileDialog { AllowMultiple = false };

            var filePath = (await ofd.ShowAsync()).FirstOrDefault();
            if (filePath == null)
            {
                CtxLog.Information("No file selected");
            }
            else
            {
                try
                {
                    SourceSaveData = await Task.Run(() => new SaveData(filePath));
                }
                catch (Exception ex)
                {
                    MainWindowViewModel.Instance.ShowPopup(ex.Message);
                    CtxLog.Error(ex, ex.Message);
                }
            }

            return Unit.Default;
        }

        private SerializableAppearance ImportFromSaveSlot(SaveSlot saveSlot)
        {
            return new SerializableAppearance(saveSlot);
        }

        private async Task<SerializableAppearance?> ImportFromCmp(Unit _)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            var items = await ofd.ShowAsync();

            if (items?.Length > 0)
            {
                try
                {
                    CMP cmp = new CMP(items[0]);
                    return new SerializableAppearance(cmp);
                }
                catch (Exception ex)
                {
                    MainWindowViewModel.Instance.ShowPopup(ex.Message);
                    CtxLog.Error(ex, ex.Message);
                }
            }

            return null;
        }

        private async Task<SerializableAppearance?> ImportFromJson(Unit _)
        {
            // Mostly copy-pasted from MHWAppearanceEditor legacy
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filters.Add(new FileDialogFilter { Name = "Shareable Character Appearance", Extensions = new List<string> { "json", "zip" } });
            var items = await ofd.ShowAsync();

            if (items?.Length > 0)
            {
                string jsonFile = items[0];

                // If zip file
                if (new FileInfo(items[0]).Extension.ToLower() == ".zip")
                {
                    string tempFolder = "cirilla_temp";

                    ZipFile.ExtractToDirectory(items[0], tempFolder);
                    var jsonFiles = new DirectoryInfo(tempFolder).GetFiles().Where(x => x.Extension.ToLower() == ".json").ToList();

                    if (jsonFiles.Count > 0)
                    {
                        // Show selector
                        CtxLog.Warning("This tool currently doesn't support multiple JSON files in a zip. For now we'll just use the first JSON file in the zip.");
                        jsonFile = jsonFiles[0].FullName;
                    }
                    else if (jsonFiles.Count == 0)
                    {
                        CtxLog.Error("This zip file doesn't contain any JSON files!");
                        return null;
                    }
                    else
                    {
                        jsonFile = jsonFiles[0].FullName;
                    }
                }

                try
                {
                    string str = File.ReadAllText(jsonFile);
                    var obj = JsonConvert.DeserializeObject<SerializableAppearance>(str);
                    CtxLog.Information($"Imported Character JSON from {jsonFile}");
                    return obj;
                }
                catch (Exception ex)
                {
                    MainWindowViewModel.Instance.ShowPopup(ex.Message);
                    CtxLog.Error(ex, ex.Message);
                    return null;
                }
            }

            return null;
        }

        private async Task<Unit> ExportToCmp(Unit _)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialFileName = Utility.GetSafeFilename(saveSlotContext.HunterName);
            sfd.Filters.Add(new FileDialogFilter { Name = "NPC Character Preset", Extensions = new List<string> { "cmp" } });

            var fileName = await sfd.ShowAsync();

            if (fileName != null)
            {
                try
                {
                    CMP cmp = new CMP(saveSlotContext.Native.CharacterAppearance);
                    cmp.Save(fileName);

                    string str = $"Exported NPC Character Preset to {fileName}";
                    MainWindowViewModel.Instance.ShowPopup(str);
                    CtxLog.Information(str);
                }
                catch (Exception ex)
                {
                    MainWindowViewModel.Instance.ShowPopup(ex.Message);
                    CtxLog.Error(ex, ex.Message);
                }
            }

            return Unit.Default;
        }

        private async Task<Unit> ExportToJson(Unit _)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialFileName = Utility.GetSafeFilename(saveSlotContext.HunterName);
            sfd.Filters.Add(new FileDialogFilter { Name = "Shareable Character Appearance", Extensions = new List<string> { "json" } });

            var fileName = await sfd.ShowAsync();

            if (fileName != null)
            {
                try
                {
                    var appearance = new SerializableAppearance(saveSlotContext);
                    string json = JsonConvert.SerializeObject(appearance, Formatting.Indented);
                    File.WriteAllText(fileName, json);

                    string result = $"Exported Character JSON to {fileName}";
                    MainWindowViewModel.Instance.ShowPopup(result);
                    CtxLog.Information(result);
                }
                catch (Exception ex)
                {
                    MainWindowViewModel.Instance.ShowPopup(ex.Message);
                    CtxLog.Error(ex, ex.Message);
                }
            }

            return Unit.Default;
        }
    }
}
