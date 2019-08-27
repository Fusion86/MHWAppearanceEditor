using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.Controls;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotEyesView : ReactiveUserControl<SaveSlotViewModel>
    {
        private ColorEdit LeftEyeColorEdit => this.FindControl<ColorEdit>("LeftEyeColorEdit");
        private ColorEdit RightEyeColorEdit => this.FindControl<ColorEdit>("RightEyeColorEdit");
        private ColorEdit EyebrowColorEdit => this.FindControl<ColorEdit>("EyebrowColorEdit");
        private ComboBox[] forceResetControls;

        public SaveSlotEyesView()
        {
            InitializeComponent();

            // HACK:
            // When changing the character's gender the Items in the ComboBoxes update correctly, but the SelectedItem does not.
            // For some reason calling RaisePropertyChanged also doesn't work, so we resort to manually re-setting the SelectedItem.
            forceResetControls = new[]
            {
                this.FindControl<ComboBox>("EyeTypeSelect"),
                this.FindControl<ComboBox>("EyebrowTypeSelect"),
            };
            AttachedToVisualTree += SaveSlotEyesView_AttachedToVisualTree;

            this.Bind(ViewModel, vm => vm.LeftEyeColor, v => v.LeftEyeColorEdit.Color);
            this.Bind(ViewModel, vm => vm.RightEyeColor, v => v.RightEyeColorEdit.Color);
            this.Bind(ViewModel, vm => vm.EyebrowColor, v => v.EyebrowColorEdit.Color);
            this.OneWayBind(ViewModel, vm => vm.ColorPaletteVibrant, v => v.LeftEyeColorEdit.ColorPalette);
            this.OneWayBind(ViewModel, vm => vm.ColorPaletteVibrant, v => v.RightEyeColorEdit.ColorPalette);
            this.OneWayBind(ViewModel, vm => vm.ColorPaletteNatural, v => v.EyebrowColorEdit.ColorPalette);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SaveSlotEyesView_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            foreach (var ctrl in forceResetControls)
            {
                ctrl.SelectedItem = ctrl.SelectedItem;
            }
        }
    }
}
