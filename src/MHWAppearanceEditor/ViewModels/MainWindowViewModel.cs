using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace MHWAppearanceEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        internal static MainWindowViewModel Instance { get; private set; }

        public ReactiveCommand<Unit, Unit> ToggleShowLog { get; }

        /// <remarks>
        /// I originally wrote this code before I knew about the existence of RoutedViewHost, which is obviously better than whetever the hell this is.
        /// </remarks>
        [Reactive] public ViewModelBase ActiveViewModel { get; private set; }
        [Reactive] public bool ShowLog { get; set; } = false;
        [Reactive] public string MostRecentEventMessage { get; private set; }

        // Popup
        [Reactive] public string PopupText { get; private set; } = "";
        [Reactive] public bool PopupIsOpen { get; set; }
        [Reactive] public bool PopupCanClose { get; private set; }
        [Reactive] public double ContentOpacity { get; private set; } = 1;

        public ObservableCollection<LogEventViewModel> EventsBinding { get; }

        // No need to re-create this object each time
        private readonly StartScreenViewModel startScreenViewModel = new StartScreenViewModel();

        public MainWindowViewModel()
        {
            Instance = this;

            var logger = Locator.Current.GetService<LogSink>()!;
            EventsBinding = logger.Events;

            logger.Events.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems.Count > 0 && e.NewItems[0] is LogEventViewModel vm)
                    MostRecentEventMessage = $"[{vm.ShortSourceContext}] {vm.Message}";
            };

            ToggleShowLog = ReactiveCommand.Create(() => { ShowLog = !ShowLog; });

            this.WhenAnyValue(x => x.PopupIsOpen)
                .Subscribe(isOpen => ContentOpacity = isOpen ? 0.5 : 1);

            ShowStartScreen();
        }

        public void SetActiveViewModel(ViewModelBase viewModel)
        {
            ActiveViewModel = viewModel;
        }

        public void ShowStartScreen()
        {
            ActiveViewModel = startScreenViewModel;
        }

        public void ShowPopup(string text, bool canClose = true)
        {
            PopupCanClose = canClose;
            PopupText = text;
            PopupIsOpen = true;
        }
    }
}
