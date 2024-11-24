using LW3.Models;
using System.Formats.Tar;

namespace LW3;

public partial class EditTeaPage : ContentPage
{
    private Tea _tea; // Чай, що редагується
    private MainPage _mainPage; // Посилання на головну сторінку

    public EditTeaPage(Tea tea, MainPage mainPage)
    {
        InitializeComponent();
        _tea = tea;
        _mainPage = mainPage;

        // Заповнення даних у форму
        NameEntry.Text = _tea.Name;
        BrandEntry.Text = _tea.Brand;
        PriceEntry.Text = _tea.Price.ToString("F2");
        YearEntry.Text = _tea.Year.ToString();
        CategoryEntry.Text = _tea.Category;
        StockEntry.Text = _tea.Stock.ToString();
    }

    private void OnSaveTeaClicked(object sender, EventArgs e)
    {
        // Оновлення даних чаю
        _tea.Name = NameEntry.Text;
        _tea.Brand = BrandEntry.Text;
        _tea.Price = double.TryParse(PriceEntry.Text, out var price) ? price : 0;
        _tea.Year = int.TryParse(YearEntry.Text, out var year) ? year : 0;
        _tea.Category = CategoryEntry.Text;
        _tea.Stock = int.TryParse(StockEntry.Text, out var stock) ? stock : 0;

        // Оновлення даних у головній сторінці
        _mainPage.RefreshTeaList();

        // Повернення на попередню сторінку
        Navigation.PopAsync();
    }
}
