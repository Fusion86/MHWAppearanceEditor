using DynamicData;
using DynamicData.Binding;
using MHWAppearanceEditorNext.Interfaces;
using MHWAppearanceEditorNext.ViewModels.Tabs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;

namespace MHWAppearanceEditorNext.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Add new tab or open it if it already exists.
        /// </summary>
        public ReactiveCommand<ITabViewModel, Unit> OpenTabCommand { get; }

        [Reactive] public ITabViewModel SelectedTab { get; set; }

        private readonly SourceList<ITabViewModel> openTabs = new SourceList<ITabViewModel>();

        /// <summary>
        /// Collection of opened tabs.
        /// <para/>
        /// Readonly!
        /// </summary>
        public IObservableCollection<ITabViewModel> OpenTabsBinding { get; } = new ObservableCollectionExtended<ITabViewModel>();

        public MainWindowViewModel()
        {
            openTabs.Connect()
                .Bind(OpenTabsBinding)
                .Subscribe();

            openTabs.Add(new HomeViewModel(this));

            OpenTabCommand = ReactiveCommand.Create<ITabViewModel, Unit>(OpenTab);
        }

        private Unit OpenTab(ITabViewModel vm)
        {
            if (!OpenTabsBinding.Contains(vm))
            {
                openTabs.Add(vm);
            }

            SelectedTab = vm;
            return Unit.Default;
        }
    }
}
