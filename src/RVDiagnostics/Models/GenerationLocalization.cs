namespace RVDiagnostics.Models
{
    public class GenerationLocalization
    {
        public string Ru { get; set; }
        public string En { get; set; }

        public string GetLocalized()
            => LocalizationManager.CurrentLanguage.StartsWith("en") ? En : Ru;

        public bool Matches(string value)
            => value == Ru || value == En;
    }
}
