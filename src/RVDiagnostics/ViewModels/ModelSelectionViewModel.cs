using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RVDiagnostics.Abstractions;
using RVDiagnostics.Data;
using RVDiagnostics.Enums;
using RVDiagnostics.Models;

namespace RVDiagnostics.ViewModels;

public partial class ModelSelectionViewModel : ViewModelBase
{
    private readonly Dictionary<string, GenerationLocalization> _generationLocalizations = GenerationLocalizationDatabase.Generations;
    private readonly List<EcuSuggestion> _ecuCodes = EcuSuggestionDatabase.EcuCodes;
    private readonly Dictionary<string, CarModel> _carModels = CarModelDatabase.Models;
    
    [ObservableProperty]
    private string _selectedMake = string.Empty;
    [ObservableProperty]
    private string _selectedModel = string.Empty;
    [ObservableProperty]
    private string _selectedGeneration = string.Empty;
    [ObservableProperty]
    private string _selectedEngine = string.Empty;
    [ObservableProperty]
    private string _ecuSearchText = string.Empty;
    [ObservableProperty]
    private string _currentModelKey = string.Empty;
    [ObservableProperty] 
    private SearchVariation _searchVariation = SearchVariation.Vehicle;
    [ObservableProperty]
    private CarModel? _currentModel;



    [RelayCommand]
    public void ChangeSearchVariation()
    {
        SearchVariation = SearchVariation == SearchVariation.Vehicle ? SearchVariation.Ecu : SearchVariation.Vehicle;
    }
}