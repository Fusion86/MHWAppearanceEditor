using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;
using MHWAppearanceEditor.Extensions;
using MHWAppearanceEditor.Helpers;
using MHWAppearanceEditor.Models;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.ViewModels
{
    public class StartScreenViewModel : ViewModelBase
    {
        private static readonly ILogger CtxLog = Log.ForContext<StartScreenViewModel>();

        public ViewModelActivator Activator { get; }

        public ReactiveCommand<Unit, Unit> LoadSteamAccountsCommand { get; }
        public ReactiveCommand<SteamAccount, Unit> OpenSteamAccountCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

        public ObservableCollection<SteamAccount> SteamAccounts { get; } = new ObservableCollection<SteamAccount>();

        public StartScreenViewModel()
        {
            Activator = new ViewModelActivator();

            LoadSteamAccountsCommand = ReactiveCommand.CreateFromTask(LoadSteamAccounts);
            OpenSteamAccountCommand = ReactiveCommand.Create<SteamAccount>(OpenSteamAccount);
            OpenFileCommand = ReactiveCommand.CreateFromTask(ShowOpenFileDialog);

            // Initialize view
            LoadSteamAccountsCommand.Execute().Subscribe();
        }

        private async Task LoadSteamAccounts()
        {
            await Task.Run(async () =>
            {
                var users = SteamUtility.GetSteamUsersWithMhw();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SteamAccounts.Clear();
                    SteamAccounts.AddRange(users);
                });
            });
        }

        private void OpenSteamAccount(SteamAccount steamAccount)
        {
            string saveDataPath = Path.Combine(SteamUtility.GetMhwSaveDir(steamAccount)!, "SAVEDATA1000");
            MainWindowViewModel.Instance.SetActiveViewModel(new SaveDataViewModel(saveDataPath) { SteamAccount = steamAccount });
        }

        private async Task ShowOpenFileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog { AllowMultiple = false };

            string? initialPath = SteamUtility.GetMhwSaveDir();
            if (initialPath != null)
                ofd.Directory = initialPath;

            var filePaths = await ofd.ShowAsync();
            if (filePaths == null || filePaths.Length == 0)
            {
                CtxLog.Information("No file selected");
            }
            else
            {
                MainWindowViewModel.Instance.SetActiveViewModel(new SaveDataViewModel(filePaths[0]));
            }
        }
    }
}
