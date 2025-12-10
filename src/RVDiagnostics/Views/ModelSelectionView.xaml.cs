using RVDiagnostics.Data;
using RVDiagnostics.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RVDiagnostics.Views
{
    public partial class ModelSelectionView : UserControl
    {
        // Состояние для восстановления после смены языка
        private struct FormState
        {
            public string SelectedMake { get; set; }
            public string SelectedModel { get; set; }
            public string SelectedGeneration { get; set; }
            public string SelectedEngine { get; set; }
            public string EcuSearchText { get; set; }
            public string CurrentModelKey { get; set; }
            public bool IsVehicleMode { get; set; }
        }

        private readonly Dictionary<string, GenerationLocalization> _generationLocalizations = GenerationLocalizationDatabase.Generations;
        private readonly List<EcuSuggestion> _ecuCodes = EcuSuggestionDatabase.EcuCodes;
        private readonly Dictionary<string, CarModel> _carModels = CarModelDatabase.Models;

        private CarModel _currentModel;
        private bool _isVehicleMode = true;
        private bool _isUpdatingLanguage = false;

        public ModelSelectionView()
        {
            InitializeComponent();
            InitializeView();
            LocalizationManager.LanguageChanged += OnLanguageChanged;
        }

        ~ModelSelectionView()
        {
            LocalizationManager.LanguageChanged -= OnLanguageChanged;
        }

        private void InitializeView()
        {
            SetupEventHandlers();
            SwitchToVehicleMode();
            InitializeComboBoxes();
            ResetTestsList();
        }

        private void SetupEventHandlers()
        {
            VehicleTile.MouseLeftButtonDown += (s, e) => SwitchToVehicleMode();
            EcuTile.MouseLeftButtonDown += (s, e) => SwitchToEcuMode();

            EcuSearchBox.TextChanged += EcuSearchBox_TextChanged;

            MakeComboBox.SelectionChanged += OnMakeSelected;
            ModelComboBox.SelectionChanged += OnModelSelected;
            GenerationComboBox.SelectionChanged += OnGenerationSelected;
            EngineComboBox.SelectionChanged += OnEngineSelected;

            ClearButton.Click += ClearSelection;
            StartDiagnosticButton.Click += StartDiagnostic;
        }

        #region Mode Switching
        private void SwitchToVehicleMode() => SwitchToMode(true);
        private void SwitchToEcuMode() => SwitchToMode(false);

        private void SwitchToMode(bool isVehicleMode)
        {
            _isVehicleMode = isVehicleMode;

            VehicleTile.Tag = isVehicleMode ? "active" : "inactive";
            EcuTile.Tag = isVehicleMode ? "inactive" : "active";

            VehicleForm.Visibility = isVehicleMode ? Visibility.Visible : Visibility.Collapsed;
            EcuForm.Visibility = isVehicleMode ? Visibility.Collapsed : Visibility.Visible;
            SuggestionsBox.Visibility = isVehicleMode ? Visibility.Collapsed : SuggestionsBox.Visibility;

            UpdateStartButtonState();
        }
        #endregion

        #region Language Change Support
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            _isUpdatingLanguage = true;
            var savedState = SaveFormState();

            UpdateInterfaceOnLanguageChange();
            RestoreFormState(savedState);
        }

        private FormState SaveFormState()
        {
            return new FormState
            {
                SelectedMake = MakeComboBox.SelectedItem?.ToString(),
                SelectedModel = ModelComboBox.SelectedItem?.ToString(),
                SelectedGeneration = GenerationComboBox.SelectedItem?.ToString(),
                SelectedEngine = EngineComboBox.SelectedItem?.ToString(),
                EcuSearchText = EcuSearchBox.Text,
                CurrentModelKey = _carModels.FirstOrDefault(x => x.Value == _currentModel).Key,
                IsVehicleMode = _isVehicleMode
            };
        }

        private void RestoreFormState(FormState state)
        {
            if (!_isUpdatingLanguage) return;

            UnsubscribeFromEvents();
            try
            {
                RestoreMode(state);
                RestoreComboBoxSelections(state);
                RestoreEcuSearch(state);
                RestoreCurrentModel(state);
                UpdateStartButtonState();
            }
            finally
            {
                SubscribeToEvents();
                _isUpdatingLanguage = false;
            }
        }

        private void UnsubscribeFromEvents()
        {
            MakeComboBox.SelectionChanged -= OnMakeSelected;
            ModelComboBox.SelectionChanged -= OnModelSelected;
            GenerationComboBox.SelectionChanged -= OnGenerationSelected;
            EngineComboBox.SelectionChanged -= OnEngineSelected;
            EcuSearchBox.TextChanged -= EcuSearchBox_TextChanged;
        }

        private void SubscribeToEvents()
        {
            MakeComboBox.SelectionChanged += OnMakeSelected;
            ModelComboBox.SelectionChanged += OnModelSelected;
            GenerationComboBox.SelectionChanged += OnGenerationSelected;
            EngineComboBox.SelectionChanged += OnEngineSelected;
            EcuSearchBox.TextChanged += EcuSearchBox_TextChanged;
        }

        private void RestoreMode(FormState state)
        {
            if (state.IsVehicleMode)
                SwitchToVehicleMode();
            else
                SwitchToEcuMode();
        }

        private void RestoreComboBoxSelections(FormState state)
        {
            MakeComboBox.SelectedItem = state.SelectedMake;

            if (!string.IsNullOrEmpty(state.SelectedModel))
            {
                ModelComboBox.SelectedItem = state.SelectedModel;

                if (!string.IsNullOrEmpty(state.SelectedGeneration))
                {
                    var generation = FindLocalizedGeneration(state.SelectedGeneration);
                    GenerationComboBox.SelectedItem = generation?.GetLocalized();

                    if (!string.IsNullOrEmpty(state.SelectedEngine))
                    {
                        EngineComboBox.SelectedItem = state.SelectedEngine;
                    }
                }
            }
        }

        private void RestoreEcuSearch(FormState state)
        {
            if (!string.IsNullOrEmpty(state.EcuSearchText))
            {
                EcuSearchBox.Text = state.EcuSearchText;
            }
        }

        private void RestoreCurrentModel(FormState state)
        {
            if (string.IsNullOrEmpty(state.CurrentModelKey))
            {
                _currentModel = null;
                ResetModelInfo();
            }
            else if (_carModels.TryGetValue(state.CurrentModelKey, out var model))
            {
                _currentModel = model;
                UpdateModelInfo(model);
            }
        }
        #endregion

        #region ComboBox Management
        private void InitializeComboBoxes()
        {
            InitializeMakeComboBox();
            UpdateGenerationComboBox();
        }

        private void InitializeMakeComboBox()
        {
            MakeComboBox.Items.Clear();
            MakeComboBox.Items.Add(CreateComboBoxItem("Brand.Ford", "Ford"));
        }

        private void UpdateGenerationComboBox()
        {
            if (!GenerationComboBox.IsEnabled) return;

            var selectedValue = GenerationComboBox.SelectedItem?.ToString();
            GenerationComboBox.Items.Clear();

            foreach (var generation in _generationLocalizations.Values)
            {
                GenerationComboBox.Items.Add(generation.GetLocalized());
            }

            if (!string.IsNullOrEmpty(selectedValue))
            {
                var localizedGeneration = FindLocalizedGeneration(selectedValue);
                if (localizedGeneration != null)
                {
                    GenerationComboBox.SelectedItem = localizedGeneration.GetLocalized();
                }
            }
        }

        private GenerationLocalization FindLocalizedGeneration(string value)
        {
            return _generationLocalizations.Values.FirstOrDefault(g => g.Matches(value));
        }

        private ComboBoxItem CreateComboBoxItem(string resourceKey, string defaultValue)
        {
            var content = Application.Current.TryFindResource(resourceKey) as string ?? defaultValue;
            return new ComboBoxItem { Content = content };
        }
        #endregion

        #region Selection Event Handlers
        private void OnMakeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (MakeComboBox.SelectedItem == null)
            {
                DisableDependentComboBoxes();
                return;
            }

            var selectedMake = GetSelectedItemText(MakeComboBox);

            ModelComboBox.IsEnabled = true;
            ModelComboBox.Items.Clear();

            if (selectedMake == "Ford")
            {
                ModelComboBox.Items.Add("Scorpio");
            }

            ResetDependentSelections();
            ClearCurrentModel();
            UpdateStartButtonState();
        }

        private void OnModelSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ModelComboBox.SelectedItem == null)
            {
                GenerationComboBox.IsEnabled = false;
                EngineComboBox.IsEnabled = false;
                return;
            }

            GenerationComboBox.IsEnabled = true;
            UpdateGenerationComboBox();

            GenerationComboBox.SelectedIndex = -1;
            EngineComboBox.IsEnabled = false;
            EngineComboBox.Items.Clear();

            ClearCurrentModel();
            UpdateStartButtonState();
        }

        private void OnGenerationSelected(object sender, SelectionChangedEventArgs e)
        {
            if (GenerationComboBox.SelectedItem == null)
            {
                EngineComboBox.IsEnabled = false;
                return;
            }

            EngineComboBox.IsEnabled = true;
            EngineComboBox.Items.Clear();

            var selectedGeneration = GenerationComboBox.SelectedItem.ToString();

            if (IsFirstGeneration(selectedGeneration))
            {
                var engineText = GetLocalizedString("Engine.Scorpio.N9D", "2.0i DOHC (N9D) - 88GB-12A650-AB");
                EngineComboBox.Items.Add(engineText);
            }

            ClearCurrentModel();
            UpdateStartButtonState();
        }

        private void OnEngineSelected(object sender, SelectionChangedEventArgs e)
        {
            if (EngineComboBox.SelectedItem == null)
            {
                ClearCurrentModel();
                return;
            }

            var selectedEngine = EngineComboBox.SelectedItem.ToString();

            if (selectedEngine.Contains("88GB-12A650-AB"))
            {
                LoadCarModel("88GB-12A650-AB");
            }
            else
            {
                ClearCurrentModel();
            }

            UpdateStartButtonState();
        }
        #endregion

        #region ECU Search
        private void EcuSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = EcuSearchBox.Text.ToUpper();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                HideSuggestions();
                return;
            }

            var filteredCodes = FilterEcuCodes(searchText);

            if (filteredCodes.Count > 0)
            {
                ShowSuggestions(filteredCodes);
            }
            else
            {
                HideSuggestions();
            }
        }

        private List<EcuSuggestion> FilterEcuCodes(string searchText)
        {
            return _ecuCodes
                .Where(code => code.Code.ToUpper().Contains(searchText) ||
                               code.GetLocalizedDescription().ToUpper().Contains(searchText))
                .ToList();
        }

        private void ShowSuggestions(List<EcuSuggestion> suggestions)
        {
            EcuSuggestionsList.ItemsSource = suggestions;
            SuggestionsBox.Visibility = Visibility.Visible;
        }

        private void HideSuggestions()
        {
            SuggestionsBox.Visibility = Visibility.Collapsed;
            if (_currentModel == null)
            {
                StartDiagnosticButton.IsEnabled = false;
            }
        }

        private void SuggestionItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Border border) || !(border.DataContext is EcuSuggestion selectedEcu))
                return;

            EcuSearchBox.Text = selectedEcu.Code;
            SuggestionsBox.Visibility = Visibility.Collapsed;
            LoadCarModel(selectedEcu.Code);
        }
        #endregion

        #region Model Management
        private void LoadCarModel(string ecuCode)
        {
            // Ищем точное совпадение
            if (_carModels.TryGetValue(ecuCode, out var model))
            {
                SetCurrentModel(model);
                return;
            }

            // Ищем частичное совпадение
            var partialMatch = _carModels.FirstOrDefault(x =>
                x.Key.Contains(ecuCode) || ecuCode.Contains(x.Key));

            if (!partialMatch.Equals(default(KeyValuePair<string, CarModel>)))
            {
                SetCurrentModel(partialMatch.Value);
            }
            else
            {
                ShowEcuNotFoundWarning(ecuCode);
                ResetModelInfo();
            }
        }

        private void SetCurrentModel(CarModel model)
        {
            _currentModel = model;
            UpdateModelInfo(model);
            UpdateStartButtonState();
        }

        private void ClearCurrentModel()
        {
            if (_currentModel != null)
            {
                _currentModel = null;
                ResetModelInfo();
            }
        }

        private void UpdateModelInfo(CarModel model)
        {
            // Обновляем текстовые поля
            BrandText.Text = model.Brand;
            ModelText.Text = model.Model;
            YearsText.Text = model.Years;
            EngineText.Text = model.Engine;
            EcuText.Text = model.Ecu;
            ProtocolText.Text = model.Protocol;
            ConnectorText.Text = model.Connector;

            // Обновляем список тестов
            UpdateTestsList(model.Tests);

            // Обновляем описание и изображения
            DescriptionText.Text = model.GetLocalizedDescription();
            LoadCarImage(model.CarImagePath);
            LoadConnectorImage(model.ConnectorImagePath);
        }

        private void UpdateTestsList(IEnumerable<string> tests)
        {
            var testsCollection = new ObservableCollection<string>();
            foreach (var test in tests)
            {
                testsCollection.Add(test);
            }
            TestsList.ItemsSource = testsCollection;
        }

        private void ResetModelInfo()
        {
            var notSelected = GetLocalizedString("Default.NotSelected", "--");
            var defaultDescription = GetLocalizedString("Info.DefaultDescription",
                "Выберите модель автомобиля или введите код ЭБУ для отображения информации");

            BrandText.Text = notSelected;
            ModelText.Text = notSelected;
            YearsText.Text = notSelected;
            EngineText.Text = notSelected;
            EcuText.Text = notSelected;
            ProtocolText.Text = notSelected;
            ConnectorText.Text = notSelected;

            ResetTestsList();
            DescriptionText.Text = defaultDescription;

            CarImage.Source = null;
            ConnectorImage.Source = null;

            _currentModel = null;
        }

        private void ResetTestsList()
        {
            TestsList.ItemsSource = new ObservableCollection<string>();
        }
        #endregion

        #region Image Loading
        private void LoadCarImage(string imagePath) => CarImage.Source = LoadImageResource(imagePath);
        private void LoadConnectorImage(string imagePath) => ConnectorImage.Source = LoadImageResource(imagePath);

        private BitmapImage LoadImageResource(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            try
            {
                // Убираем начальный слеш если есть
                if (path.StartsWith("/"))
                    path = path.Substring(1);

                var uri = new Uri(path, UriKind.Relative);
                var streamInfo = Application.GetResourceStream(uri);

                if (streamInfo == null) return null;

                using (var stream = streamInfo.Stream)
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки изображения {path}: {ex.Message}");
                return null;
            }
        }
        #endregion

        #region Helper Methods
        private void DisableDependentComboBoxes()
        {
            ModelComboBox.IsEnabled = false;
            GenerationComboBox.IsEnabled = false;
            EngineComboBox.IsEnabled = false;
        }

        private void ResetDependentSelections()
        {
            ModelComboBox.SelectedIndex = -1;
            GenerationComboBox.IsEnabled = false;
            GenerationComboBox.Items.Clear();
            EngineComboBox.IsEnabled = false;
            EngineComboBox.Items.Clear();
        }

        private bool IsFirstGeneration(string value)
        {
            return _generationLocalizations.Values.Any(gen => gen.Matches(value));
        }

        private string GetSelectedItemText(ComboBox comboBox)
        {
            if (comboBox.SelectedItem is ComboBoxItem comboBoxItem)
                return comboBoxItem.Content.ToString();

            return comboBox.SelectedItem?.ToString();
        }

        private string GetLocalizedString(string resourceKey, string defaultValue)
        {
            return Application.Current.TryFindResource(resourceKey) as string ?? defaultValue;
        }

        private void UpdateStartButtonState()
        {
            // Если модель уже выбрана, кнопка всегда активна
            if (_currentModel != null)
            {
                StartDiagnosticButton.IsEnabled = true;
                return;
            }

            // Проверяем условия в зависимости от режима
            StartDiagnosticButton.IsEnabled = _isVehicleMode
                ? EngineComboBox.SelectedItem != null && EngineComboBox.IsEnabled
                : _currentModel != null;
        }

        private void UpdateInterfaceOnLanguageChange()
        {
            if (_currentModel != null)
            {
                UpdateModelInfo(_currentModel);
            }

            UpdateGenerationComboBox();
            UpdateEcuSuggestions();
            UpdateDefaultDescription();
        }

        private void UpdateEcuSuggestions()
        {
            if (SuggestionsBox.Visibility != Visibility.Visible ||
                !(EcuSuggestionsList.ItemsSource is List<EcuSuggestion> suggestions))
                return;

            // Обновляем источник данных для отображения новых локализованных значений
            var updatedSuggestions = new List<EcuSuggestion>(suggestions);
            EcuSuggestionsList.ItemsSource = null;
            EcuSuggestionsList.ItemsSource = updatedSuggestions;
        }

        private void UpdateDefaultDescription()
        {
            if (_currentModel == null)
            {
                DescriptionText.Text = GetLocalizedString("Info.DefaultDescription",
                    "Выберите модель автомобиля или введите код ЭБУ для отображения информации");
            }
        }
        #endregion

        #region Button Click Handlers
        private void ClearSelection(object sender, RoutedEventArgs e)
        {
            MakeComboBox.SelectedIndex = -1;
            ModelComboBox.Items.Clear();
            ModelComboBox.IsEnabled = false;
            GenerationComboBox.Items.Clear();
            GenerationComboBox.IsEnabled = false;
            EngineComboBox.Items.Clear();
            EngineComboBox.IsEnabled = false;
            EcuSearchBox.Text = "";
            SuggestionsBox.Visibility = Visibility.Collapsed;
            ResetModelInfo();
            UpdateStartButtonState();
        }

        private void StartDiagnostic(object sender, RoutedEventArgs e)
        {
            if (_currentModel == null) return;

            var message = GetLocalizedString("Message.StartingDiagnostic",
                "Начинаем диагностику {0} {1}");
            ShowMessage(string.Format(message, _currentModel.Brand, _currentModel.Model));

            // Здесь будет навигация на следующий экран диагностики
        }

        private void ShowEcuNotFoundWarning(string ecuCode)
        {
            var message = GetLocalizedString("Message.ECUNotFound",
                "Конфигурация для кода '{0}' не найдена");
            MessageBox.Show(string.Format(message, ecuCode), "Поиск",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}