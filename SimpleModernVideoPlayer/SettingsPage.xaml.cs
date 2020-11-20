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
using SimpleModernVideoPlayer.Dao;
using SimpleModernVideoPlayer.Domain;
using SimpleModernVideoPlayer.Service;
using SimpleModernVideoPlayer.Utils;
using Windows.UI.Xaml.Media.Imaging;

namespace SimpleModernVideoPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private double qpHeight { get { return this.Height / 6; } }
        private double qpWidth { get { return this.Width / 6; } }
        private MainPage rootPage = MainPage.Current;
        private ParticleSystem ps;
        private Point pMouse = new Point(9999, 9999);
        public SettingsPage()
        {
            this.InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            this.userAvatarSetting.DataContext = MainPage.userSettings;
            this.userNameSetting.DataContext = MainPage.userSettings;
            logStateChangedSetting();
        }

        /// <summary>
        /// 改变控件可见性
        /// </summary>
        void logStateChangedSetting()
        {
            if (MainPage.userSettings._isLoggedIn == false)
            {
                userNameBox.Visibility = Visibility.Visible;
                userPwdBox.Visibility = Visibility.Visible;
                loginBtn.Visibility = Visibility.Visible;
                tb1.Visibility = Visibility.Visible;
                tb2.Visibility = Visibility.Visible;
                logoutBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                userNameBox.Visibility = Visibility.Collapsed;
                userPwdBox.Visibility = Visibility.Collapsed;
                loginBtn.Visibility = Visibility.Collapsed;
                tb1.Visibility = Visibility.Collapsed;
                tb2.Visibility = Visibility.Collapsed;
                logoutBtn.Visibility = Visibility.Visible;
            }
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
            this.flyout1.Hide();
            MainPage.userSettings._isLoggedIn = false;
            MainPage.userSettings._userName = "未登录";
            MainPage.userSettings._userAvatar = new BitmapImage(new Uri("ms-appx:///Assets/130-512.png"));
            logStateChangedSetting();
        }

        private async void logIn_Clicked(object sender, RoutedEventArgs e)
        {
            //校验格式
            if (SimpleModernVideoPlayer.Utils.UserCheck.CheckUsername(this.userNameBox.Text) && SimpleModernVideoPlayer.Utils.UserCheck.CheckUserpswd(this.userPwdBox.Text))
            {
                User user = RESTClient.GetUserByInfo(new Info() { name = this.userNameBox.Text });
                //校验成功
                if (user.ID == 0)
                {
                    this.Conten_Create.Visibility = Visibility.Visible;
                    await this.Conten_Create.ShowAsync();
                }
                else
                {
                    if (user.password==this.userPwdBox.Text)
                    {
                        MainPage.userSettings._isLoggedIn = true;
                        MainPage.userSettings._userName = user.name;
                        MainPage.userSettings._userID = user.ID.ToString();
                        if (user.avatar == "Default")
                        {
                            MainPage.userSettings._userAvatar = new BitmapImage(new Uri("ms-appx:///Assets/130-512.png"));
                            
                        }
                        else
                        {
                            byte[] buf = Convert.FromBase64String(user.avatar);
                            ImageSource imageSource = await Utils.PhotoConverter.SaveToImageSource(buf);
                            MainPage.userSettings._userAvatar = imageSource;
                        }
                        
                       
                        logStateChangedSetting();
                        
                    }
                    else
                    {
                        this.contentDialog_passerr.Visibility = Visibility.Visible;
                        await this.contentDialog_passerr.ShowAsync();
                    }
                }
            }
            else
            {
                contentDialog.Visibility = Visibility.Visible;
                await contentDialog.ShowAsync();
            }
        }

        private void cansel_Clicked(object sender, RoutedEventArgs e)
        {
            this.flyout1.Hide();
        }

        private void create_Clicked(object sender, ContentDialogButtonClickEventArgs e)
        {
            RESTClient.CreateUser(new User()
            {
                CreateTime = DateTime.Now.ToString(),
                LastModifiedTime = DateTime.Now.ToString(),
                name = this.userNameBox.Text,
                password = this.userPwdBox.Text,
                avatar="Default"
            });
        }

        private void nocreate_Clicked(object sender, ContentDialogButtonClickEventArgs e)
        {
            //nothing
        }
    }
}
