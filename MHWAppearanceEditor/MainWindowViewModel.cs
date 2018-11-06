using Cirilla.Core.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        public RelayCommand OpenSaveDataFolderCommand { get; set; }

        #endregion

        private bool _openFilePathSetOnce; // We only want to set it once, because maybe someone keeps their saves on their desktop?

        public MainWindowViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            SaveFileCommand = new RelayCommand(SaveFile, CanSaveFile);
            OpenSaveDataFolderCommand = new RelayCommand(OpenSaveDataFolder, CanOpenSaveDataFolder);
        }

        #region Commands

        private bool CanOpenFile() => true;
        private void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;

            if (!_openFilePathSetOnce)
            {
                string savePath = Utility.GetMhwSaveDir();
                if (savePath != null)
                    ofd.FileName = savePath;

                _openFilePathSetOnce = true;
            }

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    SaveData = new SaveData(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error while opening file");
                }
            }
        }

        private bool CanSaveFile() => true;
        private void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                SaveData.Save(sfd.FileName);
            }
        }

        public bool CanOpenSaveDataFolder() => Utility.GetMhwSaveDir() != null;
        public void OpenSaveDataFolder()
        {
            Process.Start("explorer.exe", Utility.GetMhwSaveDir());
        }

        #endregion
    }
}
