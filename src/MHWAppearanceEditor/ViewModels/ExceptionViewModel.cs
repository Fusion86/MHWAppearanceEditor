using ReactiveUI;
using System;
using System.Reactive;

namespace MHWAppearanceEditor.ViewModels
{
    public class ExceptionViewModel : ViewModelBase
    {
        public Exception Exception { get; private set; }
        public string Message => $"Something went wrong - {Exception.Message}";
        public string ExceptionBody => Exception.ToString();

        public ReactiveCommand<Unit, Unit> BackToStartScreenCommand { get; }

        public ExceptionViewModel(Exception ex)
        {
            Exception = ex;

            BackToStartScreenCommand = ReactiveCommand.Create(BackToStartScreen);
        }

        private void BackToStartScreen()
        {
            MainWindowViewModel.Instance.ShowStartScreen();
        }
    }
}
