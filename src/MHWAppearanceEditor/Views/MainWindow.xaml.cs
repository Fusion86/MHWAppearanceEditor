using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace MHWAppearanceEditor.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private DataGrid LogDataGrid => this.FindControl<DataGrid>("LogDataGrid");
        private Border PopupBorder => this.FindControl<Border>("PopupBorder");

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.EventsBinding, v => v.LogDataGrid.Items).DisposeWith(disposables);
            });

            this.GetObservable(WindowStateProperty)
                .Subscribe(x =>
                {
                    PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                    PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
                });

            this.GetObservable(IsExtendedIntoWindowDecorationsProperty)
                .Subscribe(x =>
                {
                    if (!x)
                    {
                        SystemDecorations = SystemDecorations.Full;
                        TransparencyLevelHint = WindowTransparencyLevel.Blur;
                    }
                });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (ViewModel != null)
            {
                // Close popup when the user clicks outside it.
                // This used to be a built-in thing in Avalonia, but with 0.10 they borked/changed how the popup system works.
                if (ViewModel.PopupIsOpen && !PopupBorder.IsPointerOver)
                {
                    ViewModel.PopupIsOpen = false;
                    e.Handled = true;
                    return;
                }
            }

            base.OnPointerPressed(e);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            ExtendClientAreaChromeHints =
                ExtendClientAreaChromeHints.PreferSystemChrome |
                ExtendClientAreaChromeHints.OSXThickTitleBar;
        }
    }
}
