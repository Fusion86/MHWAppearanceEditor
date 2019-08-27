using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.Controls;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotHairView : ReactiveUserControl<SaveSlotViewModel>
    {
        private ColorEdit HairColorEdit => this.FindControl<ColorEdit>("HairColorEdit");
        private ColorEdit FacialHairColorEdit => this.FindControl<ColorEdit>("FacialHairColorEdit");
        private readonly ComboBox[] forceResetControls;

        public SaveSlotHairView()
        {
            InitializeComponent();

            // HACK:
            // When changing the character's gender the Items in the ComboBoxes update correctly, but the SelectedItem does not.
            // For some reason calling RaisePropertyChanged also doesn't work, so we resort to manually re-setting the SelectedItem.
            forceResetControls = new[]
            {
                this.FindControl<ComboBox>("HairTypeSelect"),
                this.FindControl<ComboBox>("FacialHairTypeSelect"),
            };
            AttachedToVisualTree += SaveSlotHairView_AttachedToVisualTree;

            this.Bind(ViewModel, vm => vm.HairColor, v => v.HairColorEdit.Color);
            this.Bind(ViewModel, vm => vm.FacialHairColor, v => v.FacialHairColorEdit.Color);
            this.OneWayBind(ViewModel, vm => vm.ColorPaletteNatural, v => v.HairColorEdit.ColorPalette);
            this.OneWayBind(ViewModel, vm => vm.ColorPaletteNatural, v => v.FacialHairColorEdit.ColorPalette);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SaveSlotHairView_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            foreach (var ctrl in forceResetControls)
            {
                ctrl.SelectedItem = ctrl.SelectedItem;
            }
        }
    }
}
