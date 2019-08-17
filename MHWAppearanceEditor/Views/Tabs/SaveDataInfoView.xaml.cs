using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.Tabs;

namespace MHWAppearanceEditor.Views.Tabs
{
    public class SaveDataInfoView : ReactiveUserControl<SaveDataInfoViewModel>
    {
        public SaveDataInfoView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
