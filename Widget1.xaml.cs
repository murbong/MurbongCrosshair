using Microsoft.Gaming.XboxGameBar;
using Microsoft.Graphics.Canvas;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Geometry;

namespace MurbongCrosshair
{
    public sealed partial class Widget1 : Page
    {
        private static readonly Size WidgetSize = new Size(400, 400);
        private XboxGameBarWidget widget;
        public static Crosshair crosshair;
        public static Action SettingEvent;
        public static Action CenterScreen;

        public Widget1()
        {
            this.InitializeComponent();
        }

        private double GetDpiScale()
        {
            var display = DisplayInformation.GetForCurrentView();
            Debug.WriteLine(display.RawPixelsPerViewPixel);
            return display.RawPixelsPerViewPixel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;
            widget.SettingsClicked += async (s, args) => await OpenSettingsAsync();

            SettingEvent += SaveAndRedraw;
            CenterScreen += () => widget.CenterWindowAsync();

            crosshair = new Crosshair();
            crosshair.ImportSetting(Settings.Get<string>("Crosshair"));

            widget.MinWindowSize = WidgetSize;
            widget.MaxWindowSize = WidgetSize;

            // widget.CenterWindowAsync();
            widget.TryResizeWindowAsync(WidgetSize);
        }

        private async Task OpenSettingsAsync() => await widget.ActivateSettingsAsync();

        private void SaveAndRedraw()
        {
            Settings.Set("Crosshair", crosshair.ExportSetting());
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, RenderCrosshair);
        }

        private void canvasSwapChainPaenl_Loaded(object sender, RoutedEventArgs e)
        {
            RenderCrosshair();
        }

        private void RenderCrosshair()
        {
            canvasSwapChainPanel.SwapChain = new CanvasSwapChain(
                CanvasDevice.GetSharedDevice(),
                (float)WidgetSize.Width,
                (float)WidgetSize.Height,
                96 * (float)GetDpiScale());

            using (var ds = canvasSwapChainPanel.SwapChain.CreateDrawingSession(Colors.Transparent))
            {
                ds.Clear(Colors.Transparent);
                DrawCrosshair(ds);
                ds.Flush();
            }

            canvasSwapChainPanel.SwapChain.Present();
        }

        private void DrawCrosshair(CanvasDrawingSession ds)
        {
            var centerX = (float)WidgetSize.Width / 2;
            var centerY = (float)WidgetSize.Height / 2;

            float length = crosshair.Size * 2;
            float width = crosshair.Thickness * 2;
            float gap = crosshair.Gap + 4;
            float outline = crosshair.Outline;

            if (crosshair.EnableDot)
            {
                if (crosshair.DotIsCircle)
                    DrawCircle(ds, new Vector2(centerX, centerY), width / 2, crosshair.Colors);
                else
                    DrawRect(ds, centerX - width / 2, centerY - width / 2, width, width, crosshair.Colors);
            }

            
            if (crosshair.EnableCircleCrosshair)
            {
                DrawCircleCrosshair(ds, centerX, centerY);
                return;
            }

            if (crosshair.EnableOutline)
                DrawOutline(ds, centerX, centerY, length, width, gap, outline);


            DrawRect(ds, centerX + (width / 2) + gap, centerY - (width / 2), length, width, crosshair.Colors);
            DrawRect(ds, centerX - (length + (width / 2)) - gap, centerY - (width / 2), length, width, crosshair.Colors);

            if (!crosshair.EnableTShape)
                DrawRect(ds, centerX - (width / 2), centerY - (length + (width / 2)) - gap, width, length, crosshair.Colors);

            DrawRect(ds, centerX - (width / 2), centerY + (width / 2) + gap, width, length, crosshair.Colors);
        }

        private void DrawCircleCrosshair(CanvasDrawingSession ds, float centerX, float centerY)
        {
            float radius = crosshair.Size;
            float thickness = crosshair.Thickness;
            var color = crosshair.Colors;

            if (crosshair.EnableOutline)
            {
                float outlineThickness = thickness + (crosshair.Outline * 2);
                ds.DrawCircle(new Vector2(centerX, centerY), radius, Colors.Black, outlineThickness);
            }
            // ✅ 원형 외곽선(링) 그리기
            ds.DrawCircle(new Vector2(centerX, centerY), radius, color, thickness);
            

            
        }
        private void DrawOutline(CanvasDrawingSession ds, float x, float y, float len, float thick, float gap, float outline)
        {
            var color = Colors.Black;

            if (crosshair.Size > 0)
            {
                DrawRect(ds, x + ((thick / 2) + gap) - outline, y - (thick / 2) - outline, len + outline * 2, thick + outline * 2, color);
                DrawRect(ds, x - ((len + (thick / 2)) + gap) - outline, y - (thick / 2) - outline, len + outline * 2, thick + outline * 2, color);

                if (!crosshair.EnableTShape)
                    DrawRect(ds, x - (thick / 2) - outline, y - ((len + (thick / 2)) + gap) - outline, thick + outline * 2, len + outline * 2, color);

                DrawRect(ds, x - (thick / 2) - outline, y + ((thick / 2) + gap) - outline, thick + outline * 2, len + outline * 2, color);
            }

            if (crosshair.EnableDot)
            {
                if (crosshair.DotIsCircle)
                    DrawCircle(ds, new Vector2(x, y), (thick / 2) + outline, color);
                else
                    DrawRect(ds, x - (thick / 2) - outline, y - (thick / 2) - outline, thick + outline * 2, thick + outline * 2, color);
            }
        }

        private void DrawRect(CanvasDrawingSession ds, float left, float top, float width, float height, Color color)
        {
            ds.FillRectangle(left, top, width, height, color);
        }

        private void DrawCircle(CanvasDrawingSession ds, Vector2 center, float radius, Color color)
        {
            ds.FillCircle(center, radius, color);
        }
    }
}