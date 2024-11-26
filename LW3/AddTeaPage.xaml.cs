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
        // �������� ����
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

        try
        {
            // ��������� ������������ ID � ������ ��� 0, ���� ������ �������
            var maxId = _teas.Count > 0 ? _teas.Max(tea => tea.Id) : 0;

            var newTea = new Tea
            {
                Id = maxId + 1, // ����� ID = ������������ ID + 1
                Name = NameEntry.Text,
                Brand = BrandEntry.Text,
                Price = price,
                Year = year,
                Category = CategoryEntry.Text,
                Stock = stock
            };

            // ������ ����� ��� � ������
            _teas.Add(newTea);

            // ��������� ������ �� ������� �������
            _mainPage.RefreshTeaList();

            // ����������� �� ��������� �������
            await Navigation.PopAsync();
        }
        catch
        {
            // ������� �� ��� ��������� ������ ���
            await DisplayAlert("�������", "�� ������� �������� ����� ���. ���� �����, �������� ������ ���.", "OK");
        }
    }
}
