namespace RVDiagnostics.Models
{
    public class CarModel : LocalizableModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Years { get; set; }
        public string Engine { get; set; }
        public string Ecu { get; set; }
        public string Protocol { get; set; }
        public string Connector { get; set; }
        public List<string> Tests { get; set; }
        public string CarImagePath { get; set; }
        public string ConnectorImagePath { get; set; }

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