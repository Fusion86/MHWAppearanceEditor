using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.SaveSlotEditors;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotInfoView : ReactiveUserControl<SaveSlotInfoViewModel>
    {
        public SaveSlotInfoView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
