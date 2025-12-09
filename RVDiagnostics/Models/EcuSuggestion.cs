namespace RVDiagnostics.Models
{
    public class EcuSuggestion : LocalizableModel
    {
        public string Code { get; set; }

        private string _description;
        private string _descriptionAlt;

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string DescriptionAlt
        {
            get => _descriptionAlt;
            set { _descriptionAlt = value; OnPropertyChanged(); }
        }

        public string GetLocalizedDescription()
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            return culture.StartsWith("en") && !string.IsNullOrEmpty(DescriptionAlt)
                ? DescriptionAlt
                : Description;
        }
    }
}