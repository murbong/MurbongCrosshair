using Microsoft.Gaming.XboxGameBar;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class Widget1 : Page
    {
        private Size size;
        private XboxGameBarWidget widget = null;
        public static Crosshair crosshair;
        public static Action SettingEvent;
        public static Action CenterScreen;

        public Widget1()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;
            widget.SettingsClicked += Widget_SettingsClicked;
            SettingEvent += SettingChange;
            CenterScreen += () =>  widget.CenterWindowAsync() ;
            crosshair = new Crosshair();
            crosshair.ImportSetting((string)Settings.GetSettingValue("Crosshair"));
            size = new Size(400, 400);
            widget.MinWindowSize = size;
            widget.MaxWindowSize = size;
            Task.Run(() => widget.TryResizeWindowAsync(size));
        }
        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await widget.ActivateSettingsAsync();
        }

        private void SettingChange()
        {
            Settings.SetSettingValue("Crosshair", crosshair.ExportSetting());
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                renderOnce();
            });
        }

        private void canvasSwapChainPaenl_Loaded(object sender, RoutedEventArgs e)
        {
            renderOnce();
        }

        private void renderOnce()
        {
            canvasSwapChainPanel.SwapChain = new CanvasSwapChain(CanvasDevice.GetSharedDevice(), (float)size.Width, (float)size.Height, 96);
            var swapChain = canvasSwapChainPanel.SwapChain;

            using (var ds = swapChain.CreateDrawingSession(Colors.Transparent))
            {
                var x = (float)size.Width / 2;
                var y = (float)size.Height / 2;
                var crosshairLength = crosshair.Size * 2;
                var crosshairWidth = crosshair.Thickness * 2;
                var crosshairGap = crosshair.Gap + 4;
                var outlineThickness = crosshair.Outline;
                ds.Clear(Colors.Transparent);

                if (crosshair.EnableOutline)
                {
                    ds.FillRectangle(
                        x + ((crosshairWidth / 2) + crosshairGap) - outlineThickness,
                        y - (crosshairWidth / 2) - outlineThickness, 
                        crosshairLength + outlineThickness * 2,
                        crosshairWidth + outlineThickness * 2, Colors.Black);

                    ds.FillRectangle((x - ((crosshairLength + (crosshairWidth / 2)) + crosshairGap)) - outlineThickness,
                        y - (crosshairWidth / 2) - outlineThickness, 
                        crosshairLength + outlineThickness * 2,
                        crosshairWidth + outlineThickness * 2, Colors.Black);

                    ds.FillRectangle(x - (crosshairWidth / 2) - outlineThickness,
                        y - ((crosshairLength + (crosshairWidth / 2)) + crosshairGap) - outlineThickness,
                        crosshairWidth + outlineThickness * 2, 
                        crosshairLength + outlineThickness * 2, Colors.Black);

                    ds.FillRectangle(x - (crosshairWidth / 2) - outlineThickness,
                        y + ((crosshairWidth / 2) + crosshairGap) - outlineThickness,
                        crosshairWidth  + outlineThickness * 2, 
                        crosshairLength + outlineThickness * 2, Colors.Black);

                    if (crosshair.EnableDot)
                        ds.FillRectangle(x - (crosshairWidth / 2) - outlineThickness,
                            y - (crosshairWidth / 2) - outlineThickness, 
                            crosshairWidth + outlineThickness * 2,
                            crosshairWidth + outlineThickness * 2, Colors.Black);
                }

                if (crosshair.EnableDot)
                {
                    ds.FillRectangle(x - (crosshairWidth / 2), y - (crosshairWidth / 2), crosshairWidth, crosshairWidth, crosshair.Colors);
                }
                //draw crosshair
                ds.FillRectangle(x + ((crosshairWidth / 2) + crosshairGap), y - (crosshairWidth / 2), crosshairLength, crosshairWidth, crosshair.Colors);
                ds.FillRectangle((x - ((crosshairLength + (crosshairWidth / 2)) + crosshairGap)), y - (crosshairWidth / 2), crosshairLength, crosshairWidth, crosshair.Colors);

                ds.FillRectangle(x - (crosshairWidth / 2), y - ((crosshairLength + (crosshairWidth / 2)) + crosshairGap), crosshairWidth, crosshairLength, crosshair.Colors);
                ds.FillRectangle(x - (crosshairWidth / 2), y + ((crosshairWidth / 2) + crosshairGap), crosshairWidth, crosshairLength, crosshair.Colors);






                ds.Flush();
                swapChain.Present();
            }
        }
    }
}
