using Microsoft.Gaming.XboxGameBar;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace MurbongCrosshair
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class Widget1Settings : Page
    {
        private byte r = 0, g = 255, b = 0;

        public Widget1Settings()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is XboxGameBarWidget settings)
            {
                _ = settings.TryResizeWindowAsync(new Size(550, 600));
            }

            if (Widget1.crosshair != null)
            {
                InitializeControls(Widget1.crosshair);
            }
        }

        private void InitializeControls(Crosshair crosshair)
        {
            var color = crosshair.Colors;

            Outline.Value = crosshair.Outline;
            Gap.Value = crosshair.Gap;
            Thickness.Value = crosshair.Thickness;
            Alpha.Value = crosshair.Alpha;
            Size.Value = crosshair.Size;

            Dot_Box.IsChecked = crosshair.EnableDot;
            T_Box.IsChecked = crosshair.EnableTShape;
            DotCircle_Box.IsChecked = crosshair.DotIsCircle;

            Color_Red.Value = r = color.R;
            Color_Green.Value = g = color.G;
            Color_Blue.Value = b = color.B;
            CircleCrosshair_Box.IsChecked = crosshair.EnableCircleCrosshair;
            ColorPreview.Background = new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        private void CenterScreen_Click(object sender, RoutedEventArgs e)
        {
            Widget1.CenterScreen?.Invoke();
        }

        private void Color_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            r = (byte)Color_Red.Value;
            g = (byte)Color_Green.Value;
            b = (byte)Color_Blue.Value;

            var newColor = Color.FromArgb(255, r, g, b);
            Widget1.crosshair.Color = newColor;
            ColorPreview.Background = new SolidColorBrush(newColor);

            Widget1.SettingEvent?.Invoke();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender == Alpha) Widget1.crosshair.Alpha = (byte)e.NewValue;
            else if (sender == Thickness) Widget1.crosshair.Thickness = (float)e.NewValue;
            else if (sender == Size) Widget1.crosshair.Size = (int)e.NewValue;
            else if (sender == Gap) Widget1.crosshair.Gap = (int)e.NewValue;
            else if (sender == Outline) Widget1.crosshair.Outline = (int)e.NewValue;

            Widget1.SettingEvent?.Invoke();
        }

        private void OutlineCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Outline != null)
            {
                Outline.Value = 0;
                Widget1.crosshair.Outline = 0;
            }

            Widget1.SettingEvent?.Invoke();
        }

        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == Dot_Box)
                Widget1.crosshair.EnableDot = Dot_Box.IsChecked == true;
            else if (sender == T_Box)
                Widget1.crosshair.EnableTShape = T_Box.IsChecked == true;
            else if (sender == DotCircle_Box)
                Widget1.crosshair.DotIsCircle = DotCircle_Box.IsChecked == true;
            else if (sender == CircleCrosshair_Box)
                Widget1.crosshair.EnableCircleCrosshair = CircleCrosshair_Box.IsChecked == true;

            Widget1.SettingEvent?.Invoke();
        }
    }
}