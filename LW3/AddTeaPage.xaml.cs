using System.Formats.Tar;
using LW3.Models;

namespace LW3;

public partial class AddTeaPage : ContentPage
{
    private readonly List<Tea> _teas;
    private readonly MainPage _mainPage;

    public AddTeaPage(List<Tea> teas, MainPage mainPage)
    {
        InitializeComponent();
        _teas = teas;
        _mainPage = mainPage;
    }

    private async void OnSaveTeaClicked(object sender, EventArgs e)
    {
        try
        {
            // Знаходимо максимальний ID у списку або 0, якщо список порожній
            var maxId = _teas.Count > 0 ? _teas.Max(tea => tea.Id) : 0;

            var newTea = new Tea
            {
                Id = maxId + 1, // Новий ID = максимальний ID + 1
                Name = NameEntry.Text,
                Brand = BrandEntry.Text,
                Price = double.TryParse(PriceEntry.Text, out var price) ? price : 0,
                Year = int.TryParse(YearEntry.Text, out var year) ? year : 0,
                Category = CategoryEntry.Text,
                Stock = int.TryParse(StockEntry.Text, out var stock) ? stock : 0
            };

            // Додаємо новий чай до списку
            _teas.Add(newTea);

            // Оновлюємо список на головній сторінці
            _mainPage.RefreshTeaList();

            // Повертаємося на попередню сторінку
            await Navigation.PopAsync();
        }
        catch
        {
            // Виводимо повідомлення про помилку
            await DisplayAlert("Error", "Failed to save new tea. Please check the inputs.", "OK");
        }
    }
}
