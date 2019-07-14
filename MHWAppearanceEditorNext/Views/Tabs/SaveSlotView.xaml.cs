using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditorNext.ViewModels.Tabs;

namespace MHWAppearanceEditorNext.Views.Tabs
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
