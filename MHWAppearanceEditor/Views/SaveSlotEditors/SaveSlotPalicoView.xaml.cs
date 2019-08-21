using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.Controls;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotPalicoView : ReactiveUserControl<SaveSlotViewModel>
    {
        private ColorEdit PatternColor1Edit => this.FindControl<ColorEdit>("PatternColor1Edit");
        private ColorEdit PatternColor2Edit => this.FindControl<ColorEdit>("PatternColor2Edit");
        private ColorEdit PatternColor3Edit => this.FindControl<ColorEdit>("PatternColor3Edit");
        private ColorEdit FurColorEdit => this.FindControl<ColorEdit>("FurColorEdit");
        private ColorEdit LeftEyeColorEdit => this.FindControl<ColorEdit>("LeftEyeColorEdit");
        private ColorEdit RightEyeColorEdit => this.FindControl<ColorEdit>("RightEyeColorEdit");
        private ColorEdit ClothingColorEdit => this.FindControl<ColorEdit>("ClothingColorEdit");

        public SaveSlotPalicoView()
        {
            InitializeComponent();
            ViewModel = DataContext as SaveSlotViewModel;

            this.Bind(ViewModel, vm => vm.PalicoPatternColor1, v => v.PatternColor1Edit.Color);
            this.Bind(ViewModel, vm => vm.PalicoPatternColor2, v => v.PatternColor2Edit.Color);
            this.Bind(ViewModel, vm => vm.PalicoPatternColor3, v => v.PatternColor3Edit.Color);
            this.Bind(ViewModel, vm => vm.PalicoFurColor, v => v.FurColorEdit.Color);
            this.Bind(ViewModel, vm => vm.PalicoLeftEyeColor, v => v.LeftEyeColorEdit.Color);
            this.Bind(ViewModel, vm => vm.PalicoRightEyeColor, v => v.RightEyeColorEdit.Color);
            this.Bind(ViewModel, vm => vm.PalicoClothingColor, v => v.ClothingColorEdit.Color);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
