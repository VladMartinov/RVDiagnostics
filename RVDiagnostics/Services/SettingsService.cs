using System.IO;
using System.Text.Json;
using RVDiagnostics.Models;

namespace RVDiagnostics.Services
{
    public static class SettingsService
    {
        private static readonly string ConfigPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RVDiagnostics", "settings.json");

        public static SettingsModel Current { get; private set; } = new SettingsModel();

        public static void Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return;

                string json = File.ReadAllText(ConfigPath);
                Current = JsonSerializer.Deserialize<SettingsModel>(json) ?? new SettingsModel();
            }
            catch
            {
                Current = new SettingsModel(); // fallback safety
            }
        }

        public static void Save()
        {
            string dir = Path.GetDirectoryName(ConfigPath)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }
    }
}
