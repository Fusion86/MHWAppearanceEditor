using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using MHWAppearanceEditor.Extensions;
using MHWAppearanceEditor.Helpers;
using MHWAppearanceEditor.Models;
using ReactiveUI;
using Serilog;
using System;
using System.IO;
using System.Linq;
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

        private readonly SourceList<SteamAccount> steamAccounts = new SourceList<SteamAccount>();
        public IObservableCollection<SteamAccount> SteamAccountsBinding { get; } = new ObservableCollectionExtended<SteamAccount>();

        public StartScreenViewModel()
        {
            Activator = new ViewModelActivator();

            steamAccounts.Connect()
                .Bind(SteamAccountsBinding)
                .Subscribe();

            LoadSteamAccountsCommand = ReactiveCommand.CreateFromTask(LoadSteamAccounts);
            OpenSteamAccountCommand = ReactiveCommand.Create<SteamAccount>(OpenSteamAccount);
            OpenFileCommand = ReactiveCommand.CreateFromTask(ShowOpenFileDialog);

            // Initialize view
            LoadSteamAccountsCommand.Execute().Subscribe();
        }

        private async Task LoadSteamAccounts()
        {
            await Task.Run(() =>
            {
                var users = SteamUtility.GetSteamUsersWithMhw();

                steamAccounts.Edit(lst =>
                {
                    lst.Clear();
                    lst.AddRange(users);
                });
            });
        }

        private void OpenSteamAccount(SteamAccount steamAccount)
        {
            string saveDataPath = Path.Combine(SteamUtility.GetMhwSaveDir(steamAccount), "SAVEDATA1000");
            MainWindowViewModel.Instance.SetActiveViewModel(new SaveDataViewModel(saveDataPath) { SteamAccount = steamAccount });
        }

        private async Task ShowOpenFileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog { AllowMultiple = false };

            string initialPath = SteamUtility.GetMhwSaveDir();
            if (initialPath != null)
                ofd.InitialDirectory = initialPath;

            string filePath = (await ofd.ShowAsync()).FirstOrDefault();
            if (filePath == null)
            {
                CtxLog.Information("No file selected");
            }
            else
            {
                MainWindowViewModel.Instance.SetActiveViewModel(new SaveDataViewModel(filePath));
            }
        }
    }
}
