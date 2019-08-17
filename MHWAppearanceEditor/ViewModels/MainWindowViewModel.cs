using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;
using System;
using System.Linq;
using System.Reactive;

namespace MHWAppearanceEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        internal static MainWindowViewModel Instance { get; private set; }

        public ReactiveCommand<Unit, Unit> ToggleShowLog { get; }

        [Reactive] public ViewModelBase ActiveViewModel { get; private set; }
        [Reactive] public bool ShowLog { get; set; } = false;
        [Reactive] public string MostRecentEventMessage { get; private set; }

        public IObservableCollection<LogEvent> EventsBinding { get; } = new ObservableCollectionExtended<LogEvent>();

        // No need to re-create this object each time
        private readonly StartScreenViewModel StartScreenViewModel = new StartScreenViewModel();

        public MainWindowViewModel()
        {
            LogSink.AppLogger.Events.Connect()
                .Bind(EventsBinding)
                .Subscribe();

            EventsBinding.ObserveCollectionChanges().Subscribe(_ => MostRecentEventMessage = EventsBinding.Last().MessageTemplate.Text);

            ActiveViewModel = new StartScreenViewModel();
            Instance = this;

            ToggleShowLog = ReactiveCommand.Create(() => { ShowLog = !ShowLog; });
        }

        public void SetActiveViewModel(ViewModelBase viewModel)
        {
            ActiveViewModel = viewModel;
        }

        public void ShowStartScreen()
        {
            ActiveViewModel = StartScreenViewModel;
        }
    }
}
