using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels;

namespace MHWAppearanceEditor.Views
{
    public class ExceptionView : ReactiveUserControl<ExceptionViewModel>
    {
        public ExceptionView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
