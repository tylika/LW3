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

        if (!string.IsNullOrWhiteSpace(FilterTeaNameEntry.Text))
            filteredTeas = filteredTeas.Where(tea => tea.Name.Contains(FilterTeaNameEntry.Text, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(FilterBrandEntry.Text))
            filteredTeas = filteredTeas.Where(tea => tea.Brand.Contains(FilterBrandEntry.Text, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(FilterCategoryEntry.Text))
            filteredTeas = filteredTeas.Where(tea => tea.Category.Contains(FilterCategoryEntry.Text, StringComparison.OrdinalIgnoreCase));

        if (double.TryParse(FilterMinPriceEntry.Text, out var minPrice))
            filteredTeas = filteredTeas.Where(tea => tea.Price >= minPrice);

        if (double.TryParse(FilterMaxPriceEntry.Text, out var maxPrice))
            filteredTeas = filteredTeas.Where(tea => tea.Price <= maxPrice);

        if (int.TryParse(FilterYearEntry.Text, out var year))
            filteredTeas = filteredTeas.Where(tea => tea.Year == year);

        if (int.TryParse(FilterStockEntry.Text, out var stock))
            filteredTeas = filteredTeas.Where(tea => tea.Stock >= stock);

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
