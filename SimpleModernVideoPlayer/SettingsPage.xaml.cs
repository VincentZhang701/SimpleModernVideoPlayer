using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SimpleModernVideoPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private MainPage rootPage = MainPage.Current;
        private ParticleSystem ps;
        private Point pMouse = new Point(9999, 9999);
        public SettingsPage()
        {
            this.InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            userNameSetting.SetBinding(TextBlock.TextProperty, new Binding() { Source = MainPage.userSettings._userName,Mode=BindingMode.OneWay });
            
            logStateChangedSetting();
        }

        void logStateChangedSetting()
        {
            Thread.Sleep(1000);
            rootPage.logStateChanged();
            Thread.Sleep(1000);
            if (MainPage.userSettings._isLoggedIn == false)
            {
                userNameSetting.ClearValue(TextBlock.TextProperty);
                userNameSetting.SetBinding(TextBlock.TextProperty, new Binding() { Source = MainPage.userSettings._userName, Mode = BindingMode.OneWay });
                userNameBox.Visibility = Visibility.Visible;
                userPwdBox.Visibility = Visibility.Visible;
                loginBtn.Visibility = Visibility.Visible;
                tb1.Visibility = Visibility.Visible;
                tb2.Visibility = Visibility.Visible;
                logoutBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                userNameSetting.ClearValue(TextBlock.TextProperty);
                userNameSetting.SetBinding(TextBlock.TextProperty, new Binding() { Source = MainPage.userSettings._userName, Mode = BindingMode.OneWay });
                userNameBox.Visibility = Visibility.Collapsed;
                userPwdBox.Visibility = Visibility.Collapsed;
                loginBtn.Visibility = Visibility.Collapsed;
                tb1.Visibility = Visibility.Collapsed;
                tb2.Visibility = Visibility.Collapsed;
                logoutBtn.Visibility = Visibility.Visible;
            }
            userAvatarSetting.SetBinding(PersonPicture.ProfilePictureProperty, new Binding() { Source = MainPage.userSettings._userAvatar, Mode = BindingMode.OneWay });

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ps = new ParticleSystem(15, 260, 20, 100, 150, this.cvs_particleContainer, this.grid_lineContainer);
            //注册帧动画
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            this.SizeChanged += CompositionTarget_Rendering;
        }

        /// <summary>
        /// 帧渲染事件
        /// </summary>
        private void CompositionTarget_Rendering(object sender, object e)
        {
            ps.ParticleRoamUpdate(pMouse);
            //ps.AddOrRemoveParticleLine();
            //ps.MoveParticleLine();
        }

        private void Grid_MouseMove(object sender, PointerRoutedEventArgs e)
        {
            pMouse = e.GetCurrentPoint(this.cvs_particleContainer).Position;
        }

        private void Grid_MouseLeave(object sender, PointerRoutedEventArgs e)
        {
            pMouse = new Point(9999, 9999);
        }

        private void logOut_Clicked(object sender, RoutedEventArgs e)
        {
            MainPage.userSettings._isLoggedIn = false;
            logStateChangedSetting();
        }

        private void logIn_Clicked(object sender, RoutedEventArgs e)
        {
            MainPage.userSettings._isLoggedIn = true;
            logStateChangedSetting();
        }
    }
}
