using RVDiagnostics.Models;

namespace RVDiagnostics.Data
{
    public static class EcuSuggestionDatabase
    {
        public static readonly List<EcuSuggestion> EcuCodes = new()
        {
            new EcuSuggestion
            {
                Code = "88GB-12A650-AB",
                Description = "Ford Scorpio 2.0i (N9D) 1985-1992",
                DescriptionAlt = "Ford Scorpio 2.0i (N9D) 1985-1992"
            }
        };
    }
}