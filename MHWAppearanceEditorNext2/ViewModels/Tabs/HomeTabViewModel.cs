using Cirilla.Core.Models;
using DynamicData;
using DynamicData.Binding;
using MHWAppearanceEditorNext2.Helpers;
using MHWAppearanceEditorNext2.Interfaces;
using MHWAppearanceEditorNext2.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MHWAppearanceEditorNext2.ViewModels.Tabs
{
    public class HomeTabViewModel : ViewModelBase, ITabViewModel, ISupportsActivation, IEnableLogger
    {
        public string Name => "Home";
        public bool CanClose => false;

        public ViewModelActivator Activator { get; }

        public ReactiveCommand<Unit, Unit> LoadSteamAccountsCommand { get; }
        public ReactiveCommand<SteamAccount, SaveData> LoadSaveSlotsCommand { get; }
        public ReactiveCommand<SaveSlotViewModel, Unit> OpenSlotCommand { get; }

        [Reactive] public SteamAccount SelectedAccount { get; set; }

        /// <summary>
        /// SaveData for the SelectedAccount.
        /// <para>
        /// Gets automatically updated when SelectedAccount changes (there is a delay of like 2 seconds because of the blowfish encryption).
        /// </para>
        /// </summary>
        [Reactive] public SaveData SelectedAccountSaveData { get; set; }

        [ObservableAsProperty] public List<SaveSlotViewModel> SelectedAccountSaveSlots { get; }

        private readonly SourceList<SteamAccount> steamAccounts = new SourceList<SteamAccount>();
        public IObservableCollection<SteamAccount> SteamAccountsBinding { get; } = new ObservableCollectionExtended<SteamAccount>();

        private readonly MainWindowViewModel _parent;

        public HomeTabViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            Activator = new ViewModelActivator();

            steamAccounts.Connect()
                .Bind(SteamAccountsBinding)
                .Subscribe();

            LoadSteamAccountsCommand = ReactiveCommand.CreateFromTask(LoadSteamAccounts);
            LoadSaveSlotsCommand = ReactiveCommand.CreateFromObservable<SteamAccount, SaveData>(LoadSaveData);
            LoadSaveSlotsCommand.Subscribe(x => SelectedAccountSaveData = x);
            OpenSlotCommand = ReactiveCommand.CreateFromObservable<SaveSlotViewModel, Unit>(x => _parent.OpenTabCommand.Execute(x));

            // Initialize data
            LoadSteamAccountsCommand.Execute().Subscribe();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this.WhenAnyValue(x => x.SelectedAccount)
                    .Where(x => x != null)
                    .InvokeCommand(LoadSaveSlotsCommand)
                    .DisposeWith(disposables);

                this.WhenAnyValue(x => x.SelectedAccountSaveData)
                    .Where(x => x != null)
                    .Select(x => x.SaveSlots.Select(saveSlot => new SaveSlotViewModel(saveSlot)).ToList())
                    .ToPropertyEx(this, x => x.SelectedAccountSaveSlots)
                    .DisposeWith(disposables);
            });
        }

        private async Task LoadSteamAccounts()
        {
            await Task.Run(() =>
            {
                var users = Utility.GetSteamUsersWithMhw();

                steamAccounts.Edit(lst =>
                {
                    lst.Clear();
                    lst.AddRange(users);
                });
            });
        }

        private IObservable<SaveData> LoadSaveData(SteamAccount selectedAccount)
        {
            return Observable.Create<SaveData>(async observer =>
            {
                string savePath = Path.Combine(Utility.GetMhwSaveDir(selectedAccount), "SAVEDATA1000");

                if (savePath != null)
                {
                    try
                    {
                        SaveData data = null;
                        await Task.Run(() => data = new SaveData(savePath));
                        observer.OnNext(data);
                        this.Log().Info($"Opened {savePath}");
                    }
                    catch (Exception ex)
                    {
                        this.Log().Error(ex);
                    }
                }

                observer.OnCompleted();
            });
        }
    }
}
