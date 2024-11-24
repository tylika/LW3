using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace LW3;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage());
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);
        if (window != null)
        {
            // Назва та розміри вікна
            window.Title = "Tea Store Manager";
            window.Width = 1000;
            window.Height = 700;
        }

#if WINDOWS
        window.Created += (s, e) =>
        {
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window.Handler.PlatformView);
            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

            // Перехоплення закриття вікна
            appWindow.Closing += async (s, e) =>
            {
                e.Cancel = true; // Скасовуємо закриття до підтвердження
                bool result = await App.Current.MainPage.DisplayAlert(
                    "Confirmation",
                    "Are you sure you want to exit the Tea Store Manager?",
                    "Yes",
                    "No");

                if (result)
                {
                    App.Current.Quit(); // Завершуємо програму, якщо користувач підтвердив
                }
            };
        };
#endif
        return window;
    }
}
