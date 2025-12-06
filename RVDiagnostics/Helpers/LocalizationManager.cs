using System.Windows;

namespace RVDiagnostics.Helpers
{
    public static class LocalizationManager
    {
        public static void SetLanguage(string langCode)
        {
            var dict = new ResourceDictionary();
            string path = $"Resources/Localization/Strings.{langCode}.xaml";

            dict.Source = new Uri(path, UriKind.Relative);

            for (int i = Application.Current.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                var md = Application.Current.Resources.MergedDictionaries[i];
                if (md.Source != null && md.Source.OriginalString.Contains("Localization"))
                    Application.Current.Resources.MergedDictionaries.RemoveAt(i);
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
