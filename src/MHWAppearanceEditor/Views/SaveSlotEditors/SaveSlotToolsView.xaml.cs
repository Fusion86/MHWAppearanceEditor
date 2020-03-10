using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Cirilla.Core.Models;
using MHWAppearanceEditor.ViewModels.SaveSlotEditors;

namespace MHWAppearanceEditor.Views.SaveSlotEditors
{
    public class SaveSlotToolsView : ReactiveUserControl<SaveSlotToolsViewModel>
    {
        public SaveSlotToolsView()
        {
            InitializeComponent();
            ViewModel = (SaveSlotToolsViewModel)DataContext;
        }

        public static readonly StyledProperty<SaveSlot> SaveSlotProperty =
            AvaloniaProperty.Register<SaveSlotToolsView, SaveSlot>("SaveSlot", inherits: true);

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public SaveSlot SaveSlot
        {
            get { return GetValue(SaveSlotProperty); }
            set
            {
                SetValue(SaveSlotProperty, value);
                DataContext = ViewModel = new SaveSlotToolsViewModel(value);
            }
        }
    }
}
