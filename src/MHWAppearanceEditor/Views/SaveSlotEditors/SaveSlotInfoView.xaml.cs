﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels;
using MHWAppearanceEditor.ViewModels.Tabs;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotInfoView : ReactiveUserControl<SaveSlotViewModel>
    {
        private static bool showDangerousWarning = true;

        private TextBox HunterRank => this.FindControl<TextBox>("HunterRank");
        private TextBox MasterRank => this.FindControl<TextBox>("MasterRank");
        private TextBox HunterXp => this.FindControl<TextBox>("HunterXp");
        private TextBox MasterXp => this.FindControl<TextBox>("MasterXp");

        public SaveSlotInfoView()
        {
            InitializeComponent();

            HunterRank.GotFocus += DangerousControl_GotFocus;
            HunterXp.GotFocus += DangerousControl_GotFocus;

            // Not sure whether editing your master rank is dangerous?
            //MasterRank.GotFocus += DangerousControl_GotFocus;
            //MasterXp.GotFocus += DangerousControl_GotFocus;
        }

        private void DangerousControl_GotFocus(object? sender, Avalonia.Input.GotFocusEventArgs e)
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
