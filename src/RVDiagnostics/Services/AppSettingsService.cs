using System.IO;
using System.Text.Json;

namespace RVDiagnostics.Services
{
    public class AppSettings
    {
        public string Language { get; set; } = "ru";
    }

    public static class AppSettingsService
    {
        private static readonly string ConfigPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static AppSettings Settings { get; private set; }

        /// <summary>
        /// Загружает настройки или создаёт файл по умолчанию.
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    Settings = JsonSerializer.Deserialize<AppSettings>(json);

                    if (Settings == null)
                        Settings = CreateDefault();
                }
                else
                {
                    Settings = CreateDefault();
                    Save();
                }
            }
            catch
            {
                Settings = CreateDefault();
                Save();
            }
        }

        /// <summary>
        /// Сохраняет текущие настройки в config.json.
        /// </summary>
        public static void Save()
        {
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }

        private static AppSettings CreateDefault()
        {
            return new AppSettings
            {
                Language = "ru",
            };
        }
    }
}
