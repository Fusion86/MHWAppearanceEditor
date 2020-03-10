using MHWAppearanceEditor.Services;
using Splat;

namespace MHWAppearanceEditor.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        public SettingsService SettingsService { get; } = Locator.Current.GetService<SettingsService>();
        public AppSettings Settings => SettingsService.Settings;
    }
}
