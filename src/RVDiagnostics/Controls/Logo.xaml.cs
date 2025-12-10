using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RVDiagnostics.Controls
{
    public partial class Logo : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(Logo),
                new PropertyMetadata(80.0, OnSizeChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Logo),
                new PropertyMetadata("RV", OnTextChanged));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(Logo),
                new PropertyMetadata(12.0, OnCornerRadiusChanged));

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(Logo),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7c1d2f"))));

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(Logo),
                new PropertyMetadata(32.0, OnFontSizeChanged));

        #endregion

        #region Properties

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        #endregion

        #region Constructors

        public Logo()
        {
            InitializeComponent();
            UpdateLogo();
        }

        #endregion

        #region Property Changed Handlers

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Logo logo)
            {
                logo.UpdateLogo();
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Logo logo)
            {
                logo.UpdateLogo();
            }
        }

        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Logo logo)
            {
                logo.UpdateLogo();
            }
        }

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Logo logo)
            {
                logo.UpdateLogo();
            }
        }

        #endregion

        #region Private Methods

        private void UpdateLogo()
        {
            if (LogoBorder != null && LogoText != null)
            {
                // Устанавливаем размер самого UserControl
                this.Width = Size;
                this.Height = Size;

                // Устанавливаем размер Border
                LogoBorder.Width = Size;
                LogoBorder.Height = Size;
                LogoBorder.CornerRadius = new CornerRadius(CornerRadius);
                LogoBorder.Background = BackgroundColor;

                // Устанавливаем текст
                LogoText.Text = Text;
                LogoText.FontSize = FontSize;
            }
        }

        #endregion
    }
}