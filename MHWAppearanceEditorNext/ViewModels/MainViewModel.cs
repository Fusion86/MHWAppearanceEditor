using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MHWAppearanceEditorNext.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand ToggleTargetCommand { get; }
        public RelayCommand ToggleEditorTypeCommand { get; }
        public RelayCommand ToggleDetailsEditorVisibilityCommand { get; }

        public MainViewModel()
        {
            ToggleTargetCommand = new RelayCommand(ToggleTarget);
            ToggleEditorTypeCommand = new RelayCommand(ToggleEditorType);
            ToggleDetailsEditorVisibilityCommand = new RelayCommand(ToggleDetailsEditorVisibility);
        }

        public string Title
        {
            get
            {
                const string title = "MHWAppearanceEditor";

                if (IsInDesignMode)
                    return title + " (Design Mode)";
                else
                    return title;
            }
        }

        public bool IsCharacterTarget { get; set; } = true;
        public bool IsEditorTypeVisual { get; set; } = true;
        public bool IsDetailsEditorVisible { get; set; } = true;

        private void ToggleTarget()
        {
            IsCharacterTarget = !IsCharacterTarget;
        }

        private void ToggleEditorType()
        {
            IsEditorTypeVisual = !IsEditorTypeVisual;
        }

        private void ToggleDetailsEditorVisibility()
        {
            IsDetailsEditorVisible = !IsDetailsEditorVisible;
        }
    }
}