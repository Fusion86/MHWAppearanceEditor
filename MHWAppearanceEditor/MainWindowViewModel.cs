using Cirilla.Core.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace MHWAppearanceEditor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SaveData SaveData { get; set; }
        public List<SaveSlot> SaveSlots => SaveData?.SaveSlots;
        public SaveSlot SelectedSaveSlot { get; set; }

        public string ExportText { get; set; }

        #region Commands

        public RelayCommand OpenFileCommand { get; }
        public RelayCommand SaveFileCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            SaveFileCommand = new RelayCommand(SaveFile, CanSaveFile);
        }

        private bool CanOpenFile() => true;
        private void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
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

        }
    }
}
