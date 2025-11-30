using RVDiagnostics.Views;
using RVDiagnostics.Views.Dialogs;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace RVDiagnostics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Navigate(UIElement nextView)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(250));
            fadeOut.Completed += (s, e) =>
            {
                MainContent.Content = nextView;

                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250));
                MainContent.BeginAnimation(OpacityProperty, fadeIn);
            };

            if (MainContent.Content != null)
                MainContent.BeginAnimation(OpacityProperty, fadeOut);
            else
                MainContent.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250)));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            var dlg = new SettingsDialog
            {
                Owner = this
            };
            dlg.ShowDialog();
        }
    }
}