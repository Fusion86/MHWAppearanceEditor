using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.SaveSlotEditors;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotToolsView : ReactiveUserControl<SaveSlotToolsViewModel>
    {
        public SaveSlotToolsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
