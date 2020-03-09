using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.Tabs;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotFaceView : ReactiveUserControl<SaveSlotViewModel>
    {
        private readonly ComboBox[] forceResetControls;

        public SaveSlotFaceView()
        {
            InitializeComponent();

            // HACK:
            // When changing the character's gender the Items in the ComboBoxes update correctly, but the SelectedItem does not.
            // For some reason calling RaisePropertyChanged also doesn't work, so we resort to manually re-setting the SelectedItem.
            forceResetControls = new[]
            {
                this.FindControl<ComboBox>("BrowTypeSelect"),
                this.FindControl<ComboBox>("FaceTypeSelect"),
                this.FindControl<ComboBox>("NoseTypeSelect"),
                this.FindControl<ComboBox>("MouthTypeSelect"),
            };
            AttachedToVisualTree += SaveSlotFaceView_AttachedToVisualTree;
        }

        private void SaveSlotFaceView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            foreach (var ctrl in forceResetControls)
            {
                ctrl.SelectedItem = ctrl.SelectedItem;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
