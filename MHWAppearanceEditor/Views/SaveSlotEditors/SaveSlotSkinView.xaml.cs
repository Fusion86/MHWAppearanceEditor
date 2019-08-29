using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.Tabs;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotSkinView : ReactiveUserControl<SaveSlotViewModel>
    {
        public SaveSlotSkinView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
