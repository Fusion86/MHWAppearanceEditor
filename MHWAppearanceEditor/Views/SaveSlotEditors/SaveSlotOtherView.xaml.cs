using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.Controls;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotOtherView : ReactiveUserControl<SaveSlotViewModel>
    {
        private ColorEdit ClothingColorEdit => this.FindControl<ColorEdit>("ClothingColorEdit");
        private readonly ComboBox[] forceResetControls;

        public SaveSlotOtherView()
        {
            InitializeComponent();
            ViewModel = DataContext as SaveSlotViewModel;

            // HACK:
            // When changing the character's gender the Items in the ComboBoxes update correctly, but the SelectedItem does not.
            // For some reason calling RaisePropertyChanged also doesn't work, so we resort to manually re-setting the SelectedItem.
            forceResetControls = new[]
            {
                this.FindControl<ComboBox>("ClothingTypeSelect"),
            };
            AttachedToVisualTree += SaveSlotOtherView_AttachedToVisualTree;

            this.Bind(ViewModel, vm => vm.ClothingColor, v => v.ClothingColorEdit.Color);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SaveSlotOtherView_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            foreach (var ctrl in forceResetControls)
            {
                ctrl.SelectedItem = ctrl.SelectedItem;
            }
        }
    }
}
