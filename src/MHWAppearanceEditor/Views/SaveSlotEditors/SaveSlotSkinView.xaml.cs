using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.Controls;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotSkinView : ReactiveUserControl<SaveSlotViewModel>
    {
        private SkinColorEdit SkinColorEdit => this.FindControl<SkinColorEdit>("SkinColorEdit");

        public SaveSlotSkinView()
        {
            InitializeComponent();

            this.Bind(ViewModel, vm => vm.SkinColorX, v => v.SkinColorEdit.SkinColorX);
            this.Bind(ViewModel, vm => vm.SkinColorY, v => v.SkinColorEdit.SkinColorY);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
