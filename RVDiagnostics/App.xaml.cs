using RVDiagnostics.Helpers;
using RVDiagnostics.Services;
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
            base.OnStartup(e);

            // START. Service
            AppSettingsService.Initialize();
            // END. Service

            // START. Helpers
            LocalizationManager.SetLanguage(AppSettingsService.Settings.Language);
            // END. Helpers
        }
    }
}
