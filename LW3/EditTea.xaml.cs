using LW3.Models;
using System;

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

    private async void OnSaveTeaClicked(object sender, EventArgs e)
    {
        // Валідація даних
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Помилка", "Назва чаю не може бути порожньою.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(BrandEntry.Text))
        {
            await DisplayAlert("Помилка", "Бренд не може бути порожнім.", "OK");
            return;
        }

        if (!double.TryParse(PriceEntry.Text, out var price) || price < 0)
        {
            await DisplayAlert("Помилка", "Ціна повинна бути числом більше або рівним 0.", "OK");
            return;
        }

        if (!int.TryParse(YearEntry.Text, out var year) || year <= 0)
        {
            await DisplayAlert("Помилка", "Рік повинен бути додатним числом.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CategoryEntry.Text))
        {
            await DisplayAlert("Помилка", "Категорія не може бути порожньою.", "OK");
            return;
        }

        if (!int.TryParse(StockEntry.Text, out var stock) || stock < 0)
        {
            await DisplayAlert("Помилка", "Кількість на складі повинна бути числом більше або рівним 0.", "OK");
            return;
        }

        // Оновлення даних чаю
        _tea.Name = NameEntry.Text;
        _tea.Brand = BrandEntry.Text;
        _tea.Price = price;
        _tea.Year = year;
        _tea.Category = CategoryEntry.Text;
        _tea.Stock = stock;

        // Оновлення даних у головній сторінці
        _mainPage.RefreshTeaList();

        // Повернення на попередню сторінку
        await Navigation.PopAsync();
    }
}
