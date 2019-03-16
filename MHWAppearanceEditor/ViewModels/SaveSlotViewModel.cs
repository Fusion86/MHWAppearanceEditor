using Serilog;
using Cirilla.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using ICSharpCode.AvalonEdit.Document;
using MHWAppearanceEditor.Models;
using System.Windows.Input;

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

    public class SaveSlotViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SaveSlot SaveSlot { get; } // Remember, this is a reference to SaveData.SaveSlot[x]  0, 1 or 2

        public PalicoAppearanceViewModel PalicoAppearance { get; }

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
                    Log.Error($"[{HunterName}] " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public string PalicoName
        {
            get => SaveSlot.PalicoName;
            set
            {
                try
                {
                    // The setter will throw an exception if the name is too large (large as in: number of bytes) or invalid (when not UTF-8)
                    SaveSlot.PalicoName = value;
                }
                catch (Exception ex)
                {
                    Log.Error($"[{PalicoName}] " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public int HunterRank { get => SaveSlot.HunterRank; set => SaveSlot.HunterRank = value; }
        public int Zenny { get => SaveSlot.Zenny; set => SaveSlot.Zenny = value; }
        public int ResearchPoints { get => SaveSlot.ResearchPoints; set => SaveSlot.ResearchPoints = value; }
        public int HunterXp { get => SaveSlot.HunterXp; set => SaveSlot.HunterXp = value; }

        public bool IsJsonValid { get; set; }
        public int LastJsonUpdate { get; private set; }
        public int LastImportDiffUpdate { get; private set; }
        public TextDocument ImportJsonDocument { get; set; } = new TextDocument();

        #region Commands

        public RelayCommand ApplyJsonCommand { get; }

        #endregion

        private MainWindowViewModel _parent;

        public SaveSlotViewModel(SaveSlot saveSlot, MainWindowViewModel mainWindowViewModel)
        {
            SaveSlot = saveSlot;

            _parent = mainWindowViewModel;

            ApplyJsonCommand = new RelayCommand(ApplyJson, CanApplyJson);

            // Update LastJsonUpdate when text is changed 
            ImportJsonDocument.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Text")
                    LastJsonUpdate = Environment.TickCount;
            };

            PalicoAppearance = new PalicoAppearanceViewModel(SaveSlot);

            // Fill text editor with current appearance
            ImportJsonDocument.Text = GetExportJsonText();
        }

        #region Calculated properties

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

                if (import != null)
                    _parent.StatusText = $"[{HunterName}] Successfully parsed JSON";

                LastImportDiffUpdate = Environment.TickCount;

                return list;
            }
        }

        #endregion

        #region Commands

        public bool CanApplyJson() => IsJsonValid;
        public void ApplyJson()
        {
            GetImportAppearance()?.ApplyToSaveSlot(SaveSlot);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImportDiff)));
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
                CommandManager.InvalidateRequerySuggested(); // Needed because otherwise the "Apply changes" button does not update
                return obj;
            }
            catch (Exception ex)
            {
                IsJsonValid = false;
                CommandManager.InvalidateRequerySuggested(); // Needed because otherwise the "Apply changes" button does not update
                Log.Error($"[{HunterName}] " + ex.Message);
                return null;
            }
        }

        public string GetExportJsonText()
        {
            return JsonConvert.SerializeObject(GetAppearance(), Formatting.Indented);
        }

        public void UpdateImportDiff()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImportDiff)));
        }

        #endregion
    }
}
