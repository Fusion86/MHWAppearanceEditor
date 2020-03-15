using MHWAppearanceEditor.Services;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace MHWAppearanceEditor.ViewModels
{
    public class FirstRunViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ContinueCommand { get; }

        public SettingsService SettingsService { get; } = Locator.Current.GetService<SettingsService>();
        public AppSettings Settings => SettingsService.Settings;

        public FirstRunViewModel()
        {
            ContinueCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                SettingsService.Settings.ShowFirstRunMessage = false;
                await SettingsService.Save();
                MainWindowViewModel.Instance.SetActiveViewModel(new StartScreenViewModel());
            });
        }
    }
}
