using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
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
        byte r = 0, g = 0, b = 0;
        public Widget1Settings()
        {
            this.InitializeComponent();

        }

        private void CenterScreen_Click(object sender, RoutedEventArgs e)
        {
            Widget1.CenterScreen();
        }

        private void Color_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

            var slider = sender as Slider;

            if (slider == Color_Red)
                r = (byte)Color_Red.Value;
            else if (slider == Color_Green)
                g = (byte)Color_Green.Value;
            else if (slider == Color_Blue)
                b = (byte)Color_Blue.Value;

            if (ColorPreview != null)
                ColorPreview.Background = new SolidColorBrush(Color.FromArgb(255, r, g, b));

            Widget1.crosshair.Color = Color.FromArgb(255, r, g, b);

            Widget1.SettingEvent();
        }

        private void Alpha_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            Widget1.crosshair.Alpha = (byte)slider.Value;
            Widget1.SettingEvent();
        }

        private void Thickness_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            Widget1.crosshair.Thickness = (int)slider.Value;
            Widget1.SettingEvent();
        }

        private void Size_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            Widget1.crosshair.Size = (int)slider.Value;
            Widget1.SettingEvent();
        }

        private void Gap_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            Widget1.crosshair.Gap = (int)slider.Value;
            Widget1.SettingEvent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Widget1.crosshair != null)
            {
                var color = Widget1.crosshair.Colors;
                Outline.Value = Widget1.crosshair.Outline;
                Gap.Value = Widget1.crosshair.Gap;
                Dot_Box.IsChecked = Widget1.crosshair.EnableDot;
                Thickness.Value = Widget1.crosshair.Thickness;
                Alpha.Value = Widget1.crosshair.Alpha;
                Size.Value = Widget1.crosshair.Size;
                Color_Red.Value = r = color.R;
                Color_Green.Value = g = color.G;
                Color_Blue.Value = b = color.B;
            }
        }

        private void OutlineCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("hello");
            var checkbox = sender as CheckBox;
            if (checkbox.IsChecked == false)
            {
                Outline.Value = 0;
                Widget1.crosshair.Outline = 0;
            }
            Widget1.SettingEvent();

        }

        private void Outline_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            Widget1.crosshair.Outline = (int)slider.Value;
            Widget1.SettingEvent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            Widget1.crosshair.EnableDot = (bool)(checkbox.IsChecked);
            Widget1.SettingEvent();
        }
    }
}
