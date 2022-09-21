using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Cirilla.Core.Models;
using DynamicData;
using MHWAppearanceEditor.Extensions;
using MHWAppearanceEditor.Models;
using MHWAppearanceEditor.Services;
using MHWAppearanceEditor.ViewModels.Tabs;
using MHWAppearanceEditor.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.ViewModels
{
    public class SaveDataViewModel : ViewModelBase
    {
        private static readonly Serilog.ILogger log = Log.ForContext<SaveDataViewModel>();

        public ReactiveCommand<Unit, Unit> OpenNewCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenHelpWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }

        public SteamAccount? SteamAccount { get; set; }
        [Reactive] public bool IsLoading { get; private set; } = true;
        [Reactive] public int SelectedTabIndex { get; set; } = 0;

        public ObservableCollection<object> Tabs { get; } = new ObservableCollection<object>();

        private SaveData? saveData;
        private readonly string saveDataDirectory;
        private readonly BackupService backup = Locator.Current.GetService<BackupService>()!;
        private readonly AppSettings settings = Locator.Current.GetService<SettingsService>()!.Settings;

        public SaveDataViewModel(string saveDataPath)
        {
            OpenNewCommand = ReactiveCommand.Create(OpenNew);
            SaveCommand = ReactiveCommand.CreateFromTask(ShowSaveDialog);
            OpenHelpWindowCommand = ReactiveCommand.Create(OpenHelpWindow);
            OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);

            saveDataDirectory = Path.GetDirectoryName(saveDataPath);

            // Don't await and run on other thread because it is needed, just trust me
            Task.Run(() => LoadSaveData(saveDataPath));
        }

        private async Task LoadSaveData(string saveDataPath)
        {
            try
            {
                saveData = await Task.Run(() => new SaveData(saveDataPath));

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Tabs.Clear();
                    Tabs.Add(new SaveDataInfoViewModel(saveData));
                    Tabs.AddRange(saveData.SaveSlots.Select(x => new SaveSlotViewModel(this, x)));
                });

                IsLoading = false;
            }
            catch (Exception ex)
            {
                log.Error(ex, ex.Message);
                MainWindowViewModel.Instance.SetActiveViewModel(new ExceptionViewModel(ex));
            }
        }

        private void OpenNew()
        {
            // TODO: Check for unsaved changes
            MainWindowViewModel.Instance.ShowStartScreen();
        }

        private void OpenHelpWindow()
        {
            new HelpWindow().Show();
        }

        private void OpenSettingsWindow()
        {
            new SettingsWindow().Show();
        }

        private async Task ShowSaveDialog()
        {
            if (saveData == null) return;

            SaveFileDialog sfd = new SaveFileDialog
            {
                Directory = saveDataDirectory,
                InitialFileName = "SAVEDATA1000"
            };

            sfd.Filters.Add(new FileDialogFilter { Name = "Monster Hunter World SaveData" });

            if (settings.EnableAdvancedFeatures)
            {
                sfd.Filters.Add(new FileDialogFilter { Name = "Decrypted SaveData (files must end with .dec)", Extensions = new List<string> { "dec" } });
            }

            string fileName = await sfd.ShowAsync();

            if (fileName == null)
            {
                log.Information("Saving cancelled");
            }
            else
            {
                MainWindowViewModel.Instance.ShowPopup("Creating SaveData backup...", false);
                backup.CreateBackup(saveData);

                MainWindowViewModel.Instance.ShowPopup("Saving...", false);

                try
                {
                    bool encrypted = !fileName.EndsWith(".dec");

                    await Task.Run(() => saveData.Save(fileName, encrypted));
                    MainWindowViewModel.Instance.ShowPopup($"Saved SaveData to '{fileName}'");
                }
                catch (Exception ex)
                {
                    log.Error(ex, ex.Message);
                    MainWindowViewModel.Instance.ShowPopup(ex.Message);
                }
            }
        }
    }
}
