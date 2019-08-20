using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.Controls;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotMakeupView : ReactiveUserControl<SaveSlotViewModel>
    {
        private ColorEdit Makeup1ColorEdit => this.FindControl<ColorEdit>("Makeup1ColorEdit");
        private ColorEdit Makeup2ColorEdit => this.FindControl<ColorEdit>("Makeup2ColorEdit");
        private readonly ComboBox[] forceResetControls;

        public SaveSlotMakeupView()
        {
            InitializeComponent();
            ViewModel = DataContext as SaveSlotViewModel;

            // HACK:
            // When changing the character's gender the Items in the ComboBoxes update correctly, but the SelectedItem does not.
            // For some reason calling RaisePropertyChanged also doesn't work, so we resort to manually re-setting the SelectedItem.
            forceResetControls = new[]
            {
                this.FindControl<ComboBox>("Makeup1TypeSelect"),
                this.FindControl<ComboBox>("Makeup2TypeSelect"),
            };
            AttachedToVisualTree += SaveSlotMakeupView_AttachedToVisualTree;

            this.Bind(ViewModel, vm => vm.Makeup1Color, v => v.Makeup1ColorEdit.Color);
            this.Bind(ViewModel, vm => vm.Makeup2Color, v => v.Makeup2ColorEdit.Color);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SaveSlotMakeupView_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            foreach (var ctrl in forceResetControls)
            {
                ctrl.SelectedItem = ctrl.SelectedItem;
            }
        }
    }
}
