using System.Globalization;
using System.Windows;

public static class LocalizationManager
{
    public static event EventHandler LanguageChanged;
    public static event EventHandler LanguageChanging; // Новое событие

    private static string _currentLanguage = "ru-RU";

    public static string CurrentLanguage
    {
        get => _currentLanguage;
        private set
        {
            if (_currentLanguage != value)
            {
                // Уведомляем о начале смены языка
                LanguageChanging?.Invoke(null, EventArgs.Empty);

                _currentLanguage = value;
                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }

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

        // Устанавливаем культуру потока
        var culture = new CultureInfo(langCode);
        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;

        CurrentLanguage = langCode;
    }
}