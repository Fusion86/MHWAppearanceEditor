using Cirilla.Core.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MHWAppearanceEditor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SaveData SaveData { get; set; }
        public List<SaveSlotViewModel> SaveSlots => SaveData?.SaveSlots.Select(x => new SaveSlotViewModel(x)).ToList();
        public SaveSlotViewModel SelectedSaveSlot { get; set; }

        public string ExportText { get; set; }

        #region Commands

        public RelayCommand OpenFileCommand { get; }
        public RelayCommand SaveFileCommand { get; }
        public RelayCommand OpenSaveDataFolderCommand { get; }
        public RelayCommand ImportCharacterJsonCommand { get; }
        public RelayCommand ExportCharacterJsonCommand { get; }

        #endregion

        private bool _openFilePathSetOnce; // We only want to set it once, because maybe someone keeps their saves on their desktop?

        public MainWindowViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            SaveFileCommand = new RelayCommand(SaveFile, CanSaveFile);
            OpenSaveDataFolderCommand = new RelayCommand(OpenSaveDataFolder, CanOpenSaveDataFolder);
            ImportCharacterJsonCommand = new RelayCommand(ImportCharacterJson, CanImportCharacterJson);
            ExportCharacterJsonCommand = new RelayCommand(ExportCharacterJson, CanExportCharacterJson);
        }

        #region Commands

        private bool CanOpenFile() => true;
        private async void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;

            if (!_openFilePathSetOnce)
            {
                string savePath = Utility.GetMhwSaveDir();
                if (savePath != null)
                    ofd.InitialDirectory = savePath;

                _openFilePathSetOnce = true;
            }

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    await Task.Run(() => SaveData = new SaveData(ofd.FileName));
                    Log.Information($"Opened {ofd.FileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error while opening file");
                }
            }
        }

        private bool CanSaveFile() => true;
        private async void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                await Task.Run(() => SaveData.Save(sfd.FileName));
                Log.Information($"Saved SaveData to {sfd.FileName}");
            }
        }

        public bool CanOpenSaveDataFolder() => Utility.GetMhwSaveDir() != null;
        public void OpenSaveDataFolder()
        {
            string path = Utility.GetMhwSaveDir();

            // This should never happen because CanOpenSaveDataFolder(), but no reason not to include it (e.g for when called manually)
            if (path == null)
                return;

            Process.Start("explorer.exe", path);
            Log.Information($"Started explorer in {path}");
        }

        public bool CanImportCharacterJson() => SelectedSaveSlot != null;
        public void ImportCharacterJson()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    string str = File.ReadAllText(ofd.FileName);
                    bool isValid = true;

                    try
                    {
                        JToken.Parse(str);
                    }
                    catch
                    {
                        Log.Information("Invalid JSON file!");
                        MessageBox.Show("Invalid JSON file!", "Error");
                        isValid = false;
                    }

                    if (isValid)
                    {
                        SerializableMetadata metaData = JsonConvert.DeserializeObject<SerializableMetadata>(str);

                        if (metaData.Author != null || metaData.Description != null || metaData.PreviewImage != null || metaData.Website != null || metaData.Title != null)
                        {
                            PreviewPreset popup = new PreviewPreset(metaData);
                            if (popup.ShowDialog() != true)
                                return;
                        }

                        SelectedSaveSlot.ImportJsonText = str;
                        Log.Information($"Imported Character JSON from {ofd.FileName}");

                        // Select "Import Appearance" tab
                        SelectedSaveSlot.SelectedTabIndex = 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        public bool CanExportCharacterJson() => SelectedSaveSlot != null;
        public void ExportCharacterJson()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Utility.GetSafeFilename(SelectedSaveSlot.HunterName) + ".json";
            sfd.Filter = "Shareable Character Appearance|*.json";

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, SelectedSaveSlot.ExportJsonText);
                Log.Information($"Exported Character JSON to {sfd.FileName}");
            }
        }

        #endregion
    }
}
