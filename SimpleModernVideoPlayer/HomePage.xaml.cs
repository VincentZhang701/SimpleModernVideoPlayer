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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace SimpleModernVideoPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {


        private ParticleSystem ps;
        private Point pMouse = new Point(9999, 9999);


        public HomePage()
        {
            this.InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private double qpHeight { get { return this.Height / 6; } }
        private double qpWidth { get { return this.Width / 6; } }


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


        private void Open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Web_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
