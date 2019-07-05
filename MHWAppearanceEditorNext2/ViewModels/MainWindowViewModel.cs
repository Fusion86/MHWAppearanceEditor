using DynamicData;
using DynamicData.Binding;
using MHWAppearanceEditorNext2.Interfaces;
using MHWAppearanceEditorNext2.ViewModels.Tabs;
using ReactiveUI;
using System;

namespace MHWAppearanceEditorNext2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly SourceList<ITabViewModel> openTabs = new SourceList<ITabViewModel>();
        public IObservableCollection<ITabViewModel> OpenTabsBinding { get; } = new ObservableCollectionExtended<ITabViewModel>();

        public MainWindowViewModel()
        {
            openTabs.Connect()
                .Bind(OpenTabsBinding)
                .Subscribe();

            openTabs.Add(new HomeTabViewModel());
        }
    }
}
