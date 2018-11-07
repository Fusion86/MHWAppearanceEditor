using Serilog;
using Cirilla.Core.Models;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace MHWAppearanceEditor
{
    public class AppearanceValueDiff : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AppearanceValueDiff(string name, object currentValue, object newValue = null)
        {
            Name = name;
            CurrentValue = currentValue;
            NewValue = newValue ?? currentValue; // Set to currentValue if there is no newValue
        }

        public string Name { get; set; }
        public object CurrentValue { get; set; }
        public object NewValue { get; set; }

        public bool HasChanges => !CurrentValue.Equals(NewValue);
    }

    public class ImportExportOptions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetGender { get; set; } = true;
        public bool SetHair { get; set; } = true;
        public bool SetMakeup1 { get; set; } = true;
        public bool SetMakeup2 { get; set; } = true;
    }

    public class SaveSlotViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SaveSlot _saveSlot; // Remember, this is a reference to SaveData.SaveSlot[x]  0, 1 or 2

        public string HunterName
        {
            get => _saveSlot.HunterName;
            set
            {
                try
                {
                    // The setter will throw an exception if the name is too large (large as in: number of bytes) or invalid (when not UTF-8)
                    _saveSlot.HunterName = value;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public int HunterRank { get => _saveSlot.HunterRank; set => _saveSlot.HunterRank = value; }
        public int Zenny { get => _saveSlot.Zenny; set => _saveSlot.Zenny = value; }
        public int ResearchPoints { get => _saveSlot.ResearchPoints; set => _saveSlot.ResearchPoints = value; }
        public int HunterXp { get => _saveSlot.HunterXp; set => _saveSlot.HunterXp = value; }

        public bool IsJsonValid { get; set; }
        public string ImportJsonText { get; set; }
        public ImportExportOptions ImportOptions { get; private set; } = new ImportExportOptions();

        #region Commands

        public RelayCommand ImportJsonCommand { get; set; }
        public RelayCommand FillWithCurrentAppearanceCommand { get; set; }

        #endregion

        public SaveSlotViewModel(SaveSlot saveSlot)
        {
            _saveSlot = saveSlot;

            ImportJsonCommand = new RelayCommand(ImportJson, CanImportJson);
            FillWithCurrentAppearanceCommand = new RelayCommand(FillWithCurrentAppearance, CanFillWithCurrentAppearance);
        }

        #region Calculated properties

        public string ExportJsonText
        {
            get
            {
                return JsonConvert.SerializeObject(GetAppearance(), Formatting.Indented);
            }
        }

        [DependsOn(nameof(ImportJsonText))]
        public List<AppearanceValueDiff> ImportDiff
        {
            get
            {
                var current = GetAppearance()?.ToDictionary();
                var import = GetImportAppearance()?.ToDictionary();

                List<AppearanceValueDiff> list = new List<AppearanceValueDiff>();

                foreach (var entry in current)
                {
                    // Create entry with values     key            - currentValue      - newValue
                    // e.g                          "HairType"     - 1                 - 2

                    if (import == null)
                        list.Add(new AppearanceValueDiff(entry.Key, entry.Value)); // Show current values
                    else
                        list.Add(new AppearanceValueDiff(entry.Key, entry.Value, import[entry.Key])); // Show current and new values and compare them
                }

                return list;
            }
        }

        #endregion

        #region Commands

        public bool CanImportJson() => GetImportAppearance() != null;
        public void ImportJson()
        {
            GetImportAppearance()?.ApplyToSaveSlot(_saveSlot);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImportDiff)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ExportJsonText)));
        }

        public bool CanFillWithCurrentAppearance() => GetAppearance() != null;
        public void FillWithCurrentAppearance()
        {
            ImportJsonText = ExportJsonText;
        }

        #endregion

        #region Methods

        private SerializableAppearance GetAppearance() => new SerializableAppearance(_saveSlot);

        public SerializableAppearance GetImportAppearance()
        {
            if (ImportJsonText == null)
                return null;

            try
            {
                var obj = JsonConvert.DeserializeObject<SerializableAppearance>(ImportJsonText);
                IsJsonValid = true;
                return obj;
            }
            catch (Exception ex)
            {
                IsJsonValid = false;
                Log.Error(ex.Message);
                return null;
            }
        }

        #endregion
    }
}
