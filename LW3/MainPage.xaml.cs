using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LW3.Models;

namespace LW3;

public partial class MainPage : ContentPage
{
    private List<Tea> _teas = new();
    private string? _jsonFilePath;
    private Tea? _selectedTea;

    public MainPage()
    {
        InitializeComponent();
    }

    public Tea? SelectedTea
    {
        get => _selectedTea;
        set
        {
            _selectedTea = value;
            OnPropertyChanged();
        }
    }

    // Load JSON file
    private async void OnUploadFileClicked(object sender, EventArgs e)
    {
        var fileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.iOS, new[] { "public.json" } },
            { DevicePlatform.Android, new[] { "application/json" } },
            { DevicePlatform.WinUI, new[] { ".json" } },
            { DevicePlatform.MacCatalyst, new[] { "json" } }
        });

        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select a JSON file",
            FileTypes = fileType
        });

        if (result != null)
        {
            try
            {
                var fileContent = File.ReadAllText(result.FullPath);
                _teas = JsonConvert.DeserializeObject<List<Tea>>(fileContent) ?? new List<Tea>();
                TeaCollectionView.ItemsSource = _teas;
                _jsonFilePath = result.FullPath;
            }
            catch (JsonReaderException)
            {
                await DisplayAlert("Error", "Invalid JSON format.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
            }
        }
    }

    // Add a new tea entry
    private async void OnAddTeaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTeaPage(_teas, this));
        SaveJsonToFile();
    }

    // Edit selected tea
    private async void OnEditTeaClicked(object sender, EventArgs e)
    {
        if (SelectedTea != null)
        {
            await Navigation.PushAsync(new EditTeaPage(SelectedTea, this));
        }
        else
        {
            await DisplayAlert("Error", "Please select a tea to edit.", "OK");
        }
    }

    // Remove selected tea
    private async void OnRemoveTeaClicked(object sender, EventArgs e)
    {
        if (SelectedTea != null)
        {
            bool isConfirmed = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{SelectedTea.Name}'?", "Yes", "No");
            if (isConfirmed)
            {
                _teas.Remove(SelectedTea);
                SelectedTea = null;
                RefreshTeaList();
                SaveJsonToFile();
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select a tea to delete.", "OK");
        }
    }

    // Search teas
    private void OnSearchTeaClicked(object sender, EventArgs e)
    {
        var filteredTeas = _teas.AsEnumerable();

        // Валідація та фільтрація за назвою чаю
        var teaNameFilter = FilterTeaNameEntry.Text;
        if (!string.IsNullOrWhiteSpace(teaNameFilter))
        {
            filteredTeas = filteredTeas.Where(tea =>
                tea.Name.Contains(teaNameFilter, StringComparison.OrdinalIgnoreCase));
        }

        // Валідація та фільтрація за брендом
        var brandFilter = FilterBrandEntry.Text;
        if (!string.IsNullOrWhiteSpace(brandFilter))
        {
            filteredTeas = filteredTeas.Where(tea =>
                tea.Brand.Contains(brandFilter, StringComparison.OrdinalIgnoreCase));
        }

        // Валідація та фільтрація за категорією
        var categoryFilter = FilterCategoryEntry.Text;
        if (!string.IsNullOrWhiteSpace(categoryFilter))
        {
            filteredTeas = filteredTeas.Where(tea =>
                tea.Category.Contains(categoryFilter, StringComparison.OrdinalIgnoreCase));
        }

        // Валідація та фільтрація за мінімальною ціною
        if (double.TryParse(FilterMinPriceEntry.Text, out var minPrice) && minPrice >= 0)
        {
            filteredTeas = filteredTeas.Where(tea => tea.Price >= minPrice);
        }

        // Валідація та фільтрація за максимальною ціною
        if (double.TryParse(FilterMaxPriceEntry.Text, out var maxPrice) && maxPrice >= 0)
        {
            filteredTeas = filteredTeas.Where(tea => tea.Price <= maxPrice);
        }

        // Валідація та фільтрація за роком
        if (int.TryParse(FilterYearEntry.Text, out var year) && year > 0)
        {
            filteredTeas = filteredTeas.Where(tea => tea.Year == year);
        }

        // Валідація та фільтрація за кількістю на складі
        if (int.TryParse(FilterStockEntry.Text, out var stock) && stock >= 0)
        {
            filteredTeas = filteredTeas.Where(tea => tea.Stock >= stock);
        }

        // Присвоєння відфільтрованої колекції до джерела
        TeaCollectionView.ItemsSource = filteredTeas.ToList();
    }

    // Clear filters
    private void OnClearFiltersClicked(object sender, EventArgs e)
    {
        FilterTeaNameEntry.Text = string.Empty;
        FilterBrandEntry.Text = string.Empty;
        FilterCategoryEntry.Text = string.Empty;
        FilterMinPriceEntry.Text = string.Empty;
        FilterMaxPriceEntry.Text = string.Empty;
        FilterYearEntry.Text = string.Empty;
        FilterStockEntry.Text = string.Empty;

        TeaCollectionView.ItemsSource = _teas;
    }

    // Refresh tea list
    public void RefreshTeaList()
    {
        TeaCollectionView.ItemsSource = null;
        TeaCollectionView.ItemsSource = _teas;
    }

    // Save JSON data
    private void SaveJsonToFile()
    {
        if (!string.IsNullOrEmpty(_jsonFilePath))
        {
            var jsonContent = JsonConvert.SerializeObject(_teas, Formatting.Indented);
            File.WriteAllText(_jsonFilePath, jsonContent);
        }
    }

    // Handle tea selection change
    private void OnTeaSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Tea selectedTea)
        {
            SelectedTea = selectedTea;
        }
        else
        {
            SelectedTea = null;
        }
    }

    // Navigate to info page
    private async void OnInfoPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InfoPage());
    }
}
