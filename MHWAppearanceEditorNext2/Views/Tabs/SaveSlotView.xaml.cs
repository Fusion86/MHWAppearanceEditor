using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditorNext2.ViewModels.Tabs;

namespace MHWAppearanceEditorNext2.Views.Tabs
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
