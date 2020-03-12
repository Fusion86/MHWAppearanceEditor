﻿using DynamicData;
using DynamicData.Binding;
using MHWAppearanceEditor.Services;
using Nito.AsyncEx.Synchronous;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Diagnostics;
using System.Linq;
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

        public IObservableCollection<LogEventViewModel> EventsBinding { get; } = new ObservableCollectionExtended<LogEventViewModel>();

        // No need to re-create this object each time
        private readonly StartScreenViewModel startScreenViewModel = new StartScreenViewModel();
        private readonly SettingsService settingsService = Locator.Current.GetService<SettingsService>();

        public MainWindowViewModel()
        {
            Instance = this;

            var logger = Locator.Current.GetService<LogSink>();
            logger.Events.Connect()
                .Bind(EventsBinding)
                .Subscribe();

            EventsBinding.ObserveCollectionChanges().Subscribe(_ => MostRecentEventMessage = EventsBinding.Last().Message);

            if (settingsService?.Settings.ShowFirstRunMessage == true || Debugger.IsAttached)
            {
                ActiveViewModel = new FirstRunViewModel();
                settingsService.Settings.ShowFirstRunMessage = false;
                settingsService.Save().WaitAndUnwrapException();
            }
            else
            {
                ActiveViewModel = new StartScreenViewModel();
            }

            ToggleShowLog = ReactiveCommand.Create(() => { ShowLog = !ShowLog; });

            this.WhenAnyValue(x => x.PopupIsOpen)
                .Subscribe(isOpen => ContentOpacity = isOpen ? 0.5 : 1);
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