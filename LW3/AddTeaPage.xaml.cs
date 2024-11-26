using System;
using System.Linq;
using System.Collections.Generic;
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
        // Валідація полів
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

        try
        {
            // Знаходимо максимальний ID у списку або 0, якщо список порожній
            var maxId = _teas.Count > 0 ? _teas.Max(tea => tea.Id) : 0;

            var newTea = new Tea
            {
                Id = maxId + 1, // Новий ID = максимальний ID + 1
                Name = NameEntry.Text,
                Brand = BrandEntry.Text,
                Price = price,
                Year = year,
                Category = CategoryEntry.Text,
                Stock = stock
            };

            // Додаємо новий чай у список
            _teas.Add(newTea);

            // Оновлюємо список на головній сторінці
            _mainPage.RefreshTeaList();

            // Повертаємося на попередню сторінку
            await Navigation.PopAsync();
        }
        catch
        {
            // Помилка під час додавання нового чаю
            await DisplayAlert("Помилка", "Не вдалося зберегти новий чай. Будь ласка, перевірте введені дані.", "OK");
        }
    }
}
