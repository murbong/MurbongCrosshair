﻿using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        private XboxGameBarWidget widget = null;
        public Widget1()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;
            widget.SettingsClicked += Widget_SettingsClicked;

        }
        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await widget.ActivateSettingsAsync();
        }

        private void canvasSwapChainPaenl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
