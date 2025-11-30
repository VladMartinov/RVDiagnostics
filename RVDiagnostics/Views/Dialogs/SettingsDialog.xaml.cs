using RVDiagnostics.Helpers;
using RVDiagnostics.Services;
using System.Windows;
using System.Windows.Controls;

namespace RVDiagnostics.Views.Dialogs
{
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            var lang = ((ComboBoxItem)LanguageSelector.SelectedItem).Tag?.ToString();
            var theme = ((ComboBoxItem)ThemeSelector.SelectedItem).Tag?.ToString();

            if (!string.IsNullOrEmpty(lang))
            {
                LocalizationManager.SetLanguage(lang);
                SettingsService.Current.Language = lang;
            }

            if (!string.IsNullOrEmpty(theme))
            {
                ThemeManager.ApplyTheme(theme);
                SettingsService.Current.Theme = theme;
            }

            SettingsService.Save();

            this.Close();
        }
    }
}
