using RVDiagnostics.Helpers;
using RVDiagnostics.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RVDiagnostics
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SettingsService.Load();

            LocalizationManager.SetLanguage(SettingsService.Current.Language);
            ThemeManager.ApplyTheme(SettingsService.Current.Theme);

            base.OnStartup(e);
        }
    }
}
