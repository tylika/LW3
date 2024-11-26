using LW3.Models;
using System;

namespace LW3;

public partial class EditTeaPage : ContentPage
{
    private Tea _tea; // ���, �� ����������
    private MainPage _mainPage; // ��������� �� ������� �������

    public EditTeaPage(Tea tea, MainPage mainPage)
    {
        InitializeComponent();
        _tea = tea;
        _mainPage = mainPage;

        // ���������� ����� � �����
        NameEntry.Text = _tea.Name;
        BrandEntry.Text = _tea.Brand;
        PriceEntry.Text = _tea.Price.ToString("F2");
        YearEntry.Text = _tea.Year.ToString();
        CategoryEntry.Text = _tea.Category;
        StockEntry.Text = _tea.Stock.ToString();
    }

    private async void OnSaveTeaClicked(object sender, EventArgs e)
    {
        // �������� �����
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("�������", "����� ��� �� ���� ���� ���������.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(BrandEntry.Text))
        {
            await DisplayAlert("�������", "����� �� ���� ���� �������.", "OK");
            return;
        }

        if (!double.TryParse(PriceEntry.Text, out var price) || price < 0)
        {
            await DisplayAlert("�������", "ֳ�� ������� ���� ������ ����� ��� ����� 0.", "OK");
            return;
        }

        if (!int.TryParse(YearEntry.Text, out var year) || year <= 0)
        {
            await DisplayAlert("�������", "г� ������� ���� �������� ������.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CategoryEntry.Text))
        {
            await DisplayAlert("�������", "�������� �� ���� ���� ���������.", "OK");
            return;
        }

        if (!int.TryParse(StockEntry.Text, out var stock) || stock < 0)
        {
            await DisplayAlert("�������", "ʳ������ �� ����� ������� ���� ������ ����� ��� ����� 0.", "OK");
            return;
        }

        // ��������� ����� ���
        _tea.Name = NameEntry.Text;
        _tea.Brand = BrandEntry.Text;
        _tea.Price = price;
        _tea.Year = year;
        _tea.Category = CategoryEntry.Text;
        _tea.Stock = stock;

        // ��������� ����� � ������� �������
        _mainPage.RefreshTeaList();

        // ���������� �� ��������� �������
        await Navigation.PopAsync();
    }
}
