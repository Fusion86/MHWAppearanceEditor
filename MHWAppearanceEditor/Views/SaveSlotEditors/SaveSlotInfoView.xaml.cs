using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Controls;
using MHWAppearanceEditor.ViewModels.Tabs;
using MHWAppearanceEditor.ViewModels;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotInfoView : ReactiveUserControl<SaveSlotViewModel>
    {
        private static bool showDangerousWarning = true;

        private TextBox HunterRank => this.FindControl<TextBox>("HunterRank");
        private TextBox HunterXp => this.FindControl<TextBox>("HunterXp");

        public SaveSlotInfoView()
        {
            InitializeComponent();

            HunterRank.GotFocus += DangerousControl_GotFocus;
            HunterXp.GotFocus += DangerousControl_GotFocus;
        }

        private void DangerousControl_GotFocus(object sender, Avalonia.Input.GotFocusEventArgs e)
        {
            if (showDangerousWarning)
            {
                Focus(); // Remove focus from dangerous control by setting focus to SaveSlotInfoView
                MainWindowViewModel.Instance.ShowPopup("Editing your Hunter Rank or XP can be very dangerous and is a untested feature.\nI am not going to stop you, but keep in mind that if you screw this up it's on you.");
                showDangerousWarning = false;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
