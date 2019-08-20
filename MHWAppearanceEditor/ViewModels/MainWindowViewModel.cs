using DynamicData;
using DynamicData.Binding;
using MHWAppearanceEditor.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        internal static MainWindowViewModel Instance { get; private set; }

        public ReactiveCommand<Unit, Unit> ToggleShowLog { get; }

        [Reactive] public ViewModelBase ActiveViewModel { get; private set; }
        [Reactive] public CharacterAssets CharacterAssets { get; private set; }
        [Reactive] public bool ShowLog { get; set; } = false;
        [Reactive] public string MostRecentEventMessage { get; private set; }

        public IObservableCollection<LogEventViewModel> EventsBinding { get; } = new ObservableCollectionExtended<LogEventViewModel>();

        // No need to re-create this object each time
        private readonly StartScreenViewModel StartScreenViewModel = new StartScreenViewModel();

        public MainWindowViewModel()
        {
            var logger = Locator.Current.GetService<LogSink>();

            logger.Events.Connect()
                .Bind(EventsBinding)
                .Subscribe();

            EventsBinding.ObserveCollectionChanges().Subscribe(_ => MostRecentEventMessage = EventsBinding.Last().Message);

            ActiveViewModel = new StartScreenViewModel();
            Instance = this;

            ToggleShowLog = ReactiveCommand.Create(() => { ShowLog = !ShowLog; });

            // Don't await
            Task.Run(() => LoadMoreStuff());
        }

        public void SetActiveViewModel(ViewModelBase viewModel)
        {
            ActiveViewModel = viewModel;
        }

        public void ShowStartScreen()
        {
            ActiveViewModel = StartScreenViewModel;
        }

        // Not the best way to load stuff, but I guess it works for now
        private void LoadMoreStuff()
        {
            CharacterAssets = CharacterAssets.LoadAssetsMap("character_assets");
        }
    }
}
