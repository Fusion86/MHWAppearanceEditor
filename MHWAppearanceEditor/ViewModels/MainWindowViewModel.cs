using Cirilla.Core.Models;
using MHWAppearanceEditor.Helpers;
using MHWAppearanceEditor.Models;
using MHWAppearanceEditor.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MHWAppearanceEditor.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SaveData SaveData { get; set; }
        public List<SaveSlotViewModel> SaveSlots => SaveData?.SaveSlots.Select(x => new SaveSlotViewModel(x, this)).ToList();
        public SaveSlotViewModel SelectedSaveSlot { get; set; }

        public string ExportText { get; set; }
        public string StatusText { get; set; }

        #region Commands

        public RelayCommand OpenFileCommand { get; }
        public RelayCommand SaveFileCommand { get; }
        public RelayCommand OpenSaveDataFolderCommand { get; }
        public RelayCommand ImportCmpCommand { get; }
        public RelayCommand ExportCmpCommand { get; }
        public RelayCommand ImportCharacterJsonCommand { get; }
        public RelayCommand ExportCharacterJsonCommand { get; }
        public RelayCommand CloseWorkbenchCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            SaveFileCommand = new RelayCommand(SaveFile, CanSaveFile);
            OpenSaveDataFolderCommand = new RelayCommand(OpenSaveDataFolder, CanOpenSaveDataFolder);
            ImportCmpCommand = new RelayCommand(ImportCmp, CanImportCmp);
            ExportCmpCommand = new RelayCommand(ExportCmp, CanExportCmp);
            ImportCharacterJsonCommand = new RelayCommand(ImportCharacterJson, CanImportCharacterJson);
            ExportCharacterJsonCommand = new RelayCommand(ExportCharacterJson, CanExportCharacterJson);
            CloseWorkbenchCommand = new RelayCommand(CloseWorkbench);
        }

        #region Commands

        private bool CanOpenFile() => true;
        private async void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;

            string savePath = Utility.GetMhwSaveDir();
            if (savePath != null)
                ofd.InitialDirectory = savePath;


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

            string savePath = Utility.GetMhwSaveDir();
            if (savePath != null)
                sfd.InitialDirectory = savePath;

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

        public bool CanImportCmp() => SelectedSaveSlot != null;
        public void ImportCmp()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Character Preset|*.cmp";

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    // Bit hacky and not very performance friendly
                    CMP cmp = new CMP(ofd.FileName);
                    SerializableAppearance appearance = new SerializableAppearance(cmp);

                    SelectedSaveSlot.ImportJsonDocument.Text = JsonConvert.SerializeObject(appearance, Formatting.Indented);
                    Log.Information($"Imported Character Preset from {ofd.FileName}");

                    // Select "Import Appearance" tab
                    SelectedSaveSlot.SelectedTabIndex = 1;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public bool CanExportCmp() => SelectedSaveSlot != null;
        public void ExportCmp()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Utility.GetSafeFilename(SelectedSaveSlot.HunterName) + ".cmp";
            sfd.Filter = "NPC Character Preset|*.cmp";

            if (sfd.ShowDialog() == true)
            {
                CMP cmp = new CMP(SelectedSaveSlot.SaveSlot.Native.Appearance);
                cmp.Save(sfd.FileName);

                Log.Information($"Exported NPC Character Preset to {sfd.FileName}");
            }
        }

        public bool CanImportCharacterJson() => SelectedSaveSlot != null;
        public void ImportCharacterJson()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Shareable Character Appearance|*.json;*.zip";

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    string jsonFile = ofd.FileName;

                    // If zip file
                    if (new FileInfo(ofd.FileName).Extension.ToLower() == ".zip")
                    {
                        string tempFolder = "cirilla_temp";

                        ZipFile.ExtractToDirectory(ofd.FileName, tempFolder);
                        var jsonFiles = new DirectoryInfo(tempFolder).GetFiles().Where(x => x.Extension.ToLower() == ".json").ToList();

                        if (jsonFiles.Count > 0)
                        {
                            // Show selector
                            MessageBox.Show("This tool currently doesn't support multiple JSON files in a zip.\nFor now we'll just use the first JSON file in the zip.");
                            jsonFile = jsonFiles[0].FullName;
                        }
                        else if (jsonFiles.Count == 0)
                        {
                            MessageBox.Show("This zip file doesn't contain any JSON files!");
                            return;
                        }
                        else
                        {
                            jsonFile = jsonFiles[0].FullName;
                        }
                    }

                    string str = File.ReadAllText(jsonFile);
                    bool isValid = true;

                    try
                    {
                        JToken.Parse(str);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
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

                        SelectedSaveSlot.ImportJsonDocument.Text = str;
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

        public void CloseWorkbench()
        {
            SaveData = null;
            SelectedSaveSlot = null;
            GC.Collect();
        }

        #endregion
    }
}
