using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.Tabs;

namespace MHWAppearanceEditor.Views.Tabs
{
    public class SaveSlotView : ReactiveUserControl<SaveSlotViewModel>
    {
        public SaveSlotView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
