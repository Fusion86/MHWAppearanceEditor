using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels;

namespace MHWAppearanceEditor.Views
{
    public class FirstRunView : ReactiveUserControl<FirstRunViewModel>
    {
        public FirstRunView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
