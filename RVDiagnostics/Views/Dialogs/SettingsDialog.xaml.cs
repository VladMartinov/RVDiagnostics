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
            Loaded += SettingsDialog_Loaded;
        }

        private void SettingsDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var lang = AppSettingsService.Settings.Language;

            foreach (ComboBoxItem item in LanguageSelector.Items)
            {
                if (item.Tag.ToString() == lang)
                {
                    LanguageSelector.SelectedItem = item;
                    break;
                }
            }
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            var lang = (LanguageSelector.SelectedItem as ComboBoxItem)?.Tag.ToString();

            if (lang == null) lang = "en";

            AppSettingsService.Settings.Language = lang;
            AppSettingsService.Save();

            LocalizationManager.SetLanguage(lang);

            DialogResult = true;
            Close();

        }

        private void ResetToDefaults(object sender, RoutedEventArgs e)
        {
            LanguageSelector.SelectedIndex = 0;
        }
    }
}
