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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


namespace SimpleModernVideoPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // 页面列表
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("home", typeof(HomePage)),
            ("videoplayer", typeof(videoplayerpage)),
            ("musicplayer", typeof(musicplayerpage)),
            ("account",typeof(accountpage)),
        };

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            UserSettings.setExample();

            if (UserSettings._isLoggedIn == false)
            {
                string a = "未登录";
                userNameMain.SetBinding(TextBlock.TextProperty, new Binding() { Source = a });
            }
            else
            {
                userNameMain.SetBinding(TextBlock.TextProperty, new Binding() { Source = UserSettings._userName });
            }
            
            userAvatarMain.SetBinding(PersonPicture.ProfilePictureProperty, new Binding() { Source = UserSettings._userAvatar });
        }

        /// <summary>
        /// 导航失败_事件处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 导航视图加载时_事件处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // You can also add items in code.
            /*            NavView.MenuItems.Add(new NavigationViewItemSeparator());
                        NavView.MenuItems.Add(new NavigationViewItem
                        {
                            Content = "My content",
                            Icon = new SymbolIcon((Symbol)0xF1AD),
                            Tag = "content"
                        });
                        _pages.Add(("content", typeof(MyContentPage)));*/

            // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];
            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            NavView_Navigate("home", new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());

            // Add keyboard accelerators for backwards navigation.
            var goBack = new KeyboardAccelerator { Key = Windows.System.VirtualKey.GoBack };
            goBack.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var altLeft = new KeyboardAccelerator
            {
                Key = Windows.System.VirtualKey.Left,
                Modifiers = Windows.System.VirtualKeyModifiers.Menu
            };
            altLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(altLeft);
        }

        /// <summary>
        /// 导航事件发生_事件处理器_使用ItemInvoke
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavView_ItemInvoked(NavigationView sender,
                                         NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        /// <summary>
        /// 导航事件发生_事件处理器_使用SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavView_SelectionChanged(NavigationView sender,
                                              NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        /// <summary>
        /// 处理导航请求，过滤掉不需要的导航
        /// </summary>
        /// <param name="navItemTag">目标页面</param>
        /// <param name="transitionInfo">过渡动画</param>
        private void NavView_Navigate(
            string navItemTag,
            Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings")
            {
                _page = typeof(SettingsPage);
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            {
                ContentFrame.Navigate(_page, null, transitionInfo);
            }
        }

        /// <summary>
        /// 窗体返回事件发生_事件处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavView_BackRequested(NavigationView sender,
                                           NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        /// <summary>
        /// 快捷键返回事件发生_事件处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void BackInvoked(KeyboardAccelerator sender,
                                 KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        /// <summary>
        /// 返回事件处理
        /// </summary>
        /// <returns></returns>
        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        /// <summary>
        /// 设置导航到相应页面时应当同时发生的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
                NavView.Header = "Settings";
            }
            else if (ContentFrame.SourcePageType == typeof(accountpage))
            {
                //Not Implemented
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

                NavView.SelectedItem = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));

                NavView.Header =
                    ((NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
            }
        }

        private void accoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavView_Navigate(((NavigationViewItem)sender).Tag.ToString(), new EntranceNavigationTransitionInfo());
        }
    }
}
