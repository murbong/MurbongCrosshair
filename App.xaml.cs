using Microsoft.Gaming.XboxGameBar;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MurbongCrosshair
{
    /// <summary>
    /// 기본 애플리케이션 클래스를 보완하는 애플리케이션별 동작을 제공합니다.
    /// </summary>
    sealed partial class App : Application
    {
        private XboxGameBarWidget widget1 = null;
        private XboxGameBarWidget widget1Settings = null;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind != ActivationKind.Protocol) return;

            if (args is IProtocolActivatedEventArgs protocolArgs &&
                protocolArgs.Uri.Scheme.Equals("ms-gamebarwidget") &&
                args is XboxGameBarWidgetActivatedEventArgs widgetArgs &&
                widgetArgs.IsLaunchActivation)
            {
                var frame = new Frame();
                frame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = frame;

                switch (widgetArgs.AppExtensionId)
                {
                    case "Widget1":
                        InitializeWidget(widgetArgs, frame, typeof(Widget1), ref widget1, Widget1Window_Closed);
                        break;

                    case "Widget1Settings":
                        InitializeWidget(widgetArgs, frame, typeof(Widget1Settings), ref widget1Settings, Widget1SettingsWindow_Closed);
                        break;
                }

                Window.Current.Activate();
            }
        }

        private void InitializeWidget(
            XboxGameBarWidgetActivatedEventArgs args,
            Frame frame,
            Type pageType,
            ref XboxGameBarWidget widgetInstance,
            WindowClosedEventHandler closedHandler)
        {
            widgetInstance = new XboxGameBarWidget(args, Window.Current.CoreWindow, frame);
            frame.Navigate(pageType, widgetInstance);
            Window.Current.Closed += closedHandler;
        }

        private void Widget1Window_Closed(object sender, Windows.UI.Core.CoreWindowEventArgs e)
        {
            widget1 = null;
            Window.Current.Closed -= Widget1Window_Closed;
        }

        private void Widget1SettingsWindow_Closed(object sender, Windows.UI.Core.CoreWindowEventArgs e)
        {
            widget1Settings = null;
            Window.Current.Closed -= Widget1SettingsWindow_Closed;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame && rootFrame.Content != null)
            {
                Window.Current.Activate();
                return;
            }

            var frame = new Frame();
            frame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // 이전 상태 복원 로직 (선택 사항)
            }

            frame.Navigate(typeof(MainPage), e.Arguments);
            Window.Current.Content = frame;
            Window.Current.Activate();
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Failed to load Page {e.SourcePageType.FullName}");
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            widget1 = null;
            widget1Settings = null;

            deferral.Complete();
        }
    }
}
