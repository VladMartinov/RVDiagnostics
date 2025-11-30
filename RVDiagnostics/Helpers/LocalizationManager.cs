using System.Windows;

namespace RVDiagnostics.Helpers
{
    public static class LocalizationManager
    {
        public static void SetLanguage(string lang)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"/Resources/Localization/Strings.{lang}.xaml", UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries[1] = dict; // 0 — тема, 1 — язык
        }
    }
}
