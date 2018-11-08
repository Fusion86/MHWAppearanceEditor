using Serilog;
using Cirilla.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using ICSharpCode.AvalonEdit.Document;
using MHWAppearanceEditor.Models;
using System.Timers;

namespace MHWAppearanceEditor.ViewModels
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

        public SaveSlot SaveSlot { get; } // Remember, this is a reference to SaveData.SaveSlot[x]  0, 1 or 2

        public string HunterName
        {
            get => SaveSlot.HunterName;
            set
            {
                try
                {
                    // The setter will throw an exception if the name is too large (large as in: number of bytes) or invalid (when not UTF-8)
                    SaveSlot.HunterName = value;
                }
                catch (Exception ex)
                {
                    Log.Error("[{HunterName}] " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public int HunterRank { get => SaveSlot.HunterRank; set => SaveSlot.HunterRank = value; }
        public int Zenny { get => SaveSlot.Zenny; set => SaveSlot.Zenny = value; }
        public int ResearchPoints { get => SaveSlot.ResearchPoints; set => SaveSlot.ResearchPoints = value; }
        public int HunterXp { get => SaveSlot.HunterXp; set => SaveSlot.HunterXp = value; }

        public bool IsJsonValid { get; set; }
        public TextDocument ImportJsonDocument { get; set; } = new TextDocument();
        public ImportExportOptions ImportOptions { get; private set; } = new ImportExportOptions();

        public int SelectedTabIndex { get; set; }

        #region Commands

        public RelayCommand ImportJsonCommand { get; set; }
        public RelayCommand FillWithCurrentAppearanceCommand { get; set; }

        #endregion

        private int _propertyChangedDelay;

        public SaveSlotViewModel(SaveSlot saveSlot)
        {
            SaveSlot = saveSlot;

            ImportJsonCommand = new RelayCommand(ImportJson, CanImportJson);
            FillWithCurrentAppearanceCommand = new RelayCommand(FillWithCurrentAppearance, CanFillWithCurrentAppearance);

            // We put this in a timer to make sure that we don't do any intensive work while a user is typing in the json import tab
            Timer propertyChangedTimer = new Timer();
            propertyChangedTimer.Interval = 100;
            propertyChangedTimer.Elapsed += (sender, e) =>
            {
                // -1   = already updated
                //  0   = should update now
                // >0   = count down to zero

                if (_propertyChangedDelay != -1)
                {
                    if (_propertyChangedDelay == 0)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImportDiff)));
                        _propertyChangedDelay = -1;
                    }
                    else
                    {
                        _propertyChangedDelay -= Math.Min(_propertyChangedDelay, (int)propertyChangedTimer.Interval);
                    }
                }
            };
            propertyChangedTimer.Start();

            // On text changed queue PropertyChangedEvent for ImportDiff
            ImportJsonDocument.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Text")
                    _propertyChangedDelay = 1000; // (Re)set update delay to 1 second
            };
        }

        #region Calculated properties

        public string ExportJsonText
        {
            get
            {
                return JsonConvert.SerializeObject(GetAppearance(), Formatting.Indented);
            }
        }

        /// <summary>
        /// Returns list of AppearanceValueDiff comparing the current saveslots appearance and the import tab appearance
        /// 
        /// PropertyChangedEvent is called 1 second after the most recent update to ImportJsonDocument.Text
        /// </summary>
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
            GetImportAppearance()?.ApplyToSaveSlot(SaveSlot);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImportDiff)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ExportJsonText)));
        }

        public bool CanFillWithCurrentAppearance() => GetAppearance() != null;
        public void FillWithCurrentAppearance()
        {
            ImportJsonDocument.Text = ExportJsonText;
        }

        #endregion

        #region Methods

        private SerializableAppearance GetAppearance() => new SerializableAppearance(SaveSlot);

        public SerializableAppearance GetImportAppearance()
        {
            if (ImportJsonDocument.Text == null)
                return null;

            try
            {
                var obj = JsonConvert.DeserializeObject<SerializableAppearance>(ImportJsonDocument.Text);
                IsJsonValid = true;
                return obj;
            }
            catch (Exception ex)
            {
                IsJsonValid = false;
                Log.Error("[{HunterName}] " + ex.Message);
                return null;
            }
        }

        #endregion
    }
}
