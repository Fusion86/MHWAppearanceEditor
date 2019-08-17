using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.SaveSlotEditors;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotFaceView : ReactiveUserControl<SaveSlotFaceViewModel>
    {
        public SaveSlotFaceView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
