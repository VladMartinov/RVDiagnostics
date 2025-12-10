using RVDiagnostics.Models;

namespace RVDiagnostics.Data
{
    public static class GenerationLocalizationDatabase
    {
        public static readonly Dictionary<string, GenerationLocalization> Generations = new()
        {
            {
                "Первое поколение (1985-1992)",
                new GenerationLocalization
                {
                    Ru = "Первое поколение (1985-1992)",
                    En = "First generation (1985-1992)"
                }
            }
        };
    }
}
