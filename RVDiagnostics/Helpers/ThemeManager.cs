using System.Windows;

namespace RVDiagnostics.Helpers
{
    public static class ThemeManager
    {
        public static void ApplyTheme(string name)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"/Resources/Themes/{name}.xaml", UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries[0] = dict;
        }
    }
}
