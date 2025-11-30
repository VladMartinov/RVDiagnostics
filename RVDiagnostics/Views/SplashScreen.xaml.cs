using System.Windows;
using RVDiagnostics.Helpers;

namespace RVDiagnostics
{
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
            Loaded += SplashScreen_Loaded;
        }

        private async void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            // Плавное появление
            WindowAnimations.FadeIn(this, 0.8);

            // Имитируем загрузку
            await Task.Delay(3500);

            // Плавный переход на главное окно
            WindowAnimations.FadeOutAndClose(this, () =>
            {
                var main = new MainWindow();
                main.Show();
            });
        }
    }
}
