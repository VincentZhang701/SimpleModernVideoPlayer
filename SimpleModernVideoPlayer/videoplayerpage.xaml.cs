using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace SimpleModernVideoPlayer
{
    public sealed partial class videoplayerpage : Page
    {
        //播放列表对象
        private static MediaPlaybackList playbackList = new MediaPlaybackList();

        //播放器对象
        private static MediaPlayer mp = new MediaPlayer();

        //保存变量
        private static ItemsControl pVLis = new ItemsControl();

        //构造函数
        public videoplayerpage()
        {
            this.InitializeComponent();
            InitializePlayer();
            InitializePlaybackList();
        }

        /// <summary>
        /// 初始化播放器
        /// </summary>
        void InitializePlayer()
        {
            // 同步UI状态
            playlistView.ItemClick += playlist_Clicked;

            // 关联播放器
            mp1.SetMediaPlayer(mp);

            // 关联播放列表
            mp.Source = playbackList;
        }

        /// <summary>
        /// 初始化播放列表
        /// </summary>
        void InitializePlaybackList()
        {
            playlistView.ItemsSource = pVLis.Items;
            if (playbackList.Items.Count != 0)
            {
                playlistView.SelectedIndex = (int)playbackList.CurrentItemIndex;
            }

            // Subscribe for changes
            playbackList.CurrentItemChanged += PlaybackList_CurrentItemChanged;
        }

        /// <summary>
        /// 同步正在播放的项目到UI界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                playlistView.SelectedIndex = (int)sender.CurrentItemIndex;
            });
        }

        /// <summary>
        /// 播放列表添加
        /// </summary>
        private async void openMedia()
        {
            // 文件对话框
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".mkv");
            openPicker.FileTypeFilter.Add(".avi");

            // 文件对象
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // 创建缩略图
                var thumbnail = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.VideosView);
                WriteableBitmap writeableBitmap = new WriteableBitmap(100, 64);
                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
                randomAccessStream.Seek(0);
                writeableBitmap.SetSource(randomAccessStream);
                //UserSettings.saveThubnail(writeableBitmap, file.DisplayName + ".jpg");

                // 媒体对象
                MediaModel media = new MediaModel(file)
                {
                    Title = file.Name,
                    ArtUri = writeableBitmap,
                    PlaybackPosition = TimeSpan.Parse("00:00:00"),
                    PlaybackHistory = DateTime.Now
                };
                pVLis.Items.Add(media);
                playbackList.Items.Add(media.MediaPlaybackItem);
            }
        }

        /// <summary>
        /// 选择了播放列表视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playlist_Clicked(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MediaModel;

            playbackList.MoveTo((uint)playbackList.Items.IndexOf(item.MediaPlaybackItem));

            if (MediaPlaybackState.Paused == mp.PlaybackSession.PlaybackState)
            {
                mp.Play();
            }
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPlay_Clicked(object sender, RoutedEventArgs e)
        {
            openMedia();
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removePlay_Clicked(object sender, RoutedEventArgs e)
        {
            var item = playlistView.SelectedItem as MediaModel;
            pVLis.Items.Remove(item);
            playbackList.Items.Remove(item.MediaPlaybackItem);

        }

        /// <summary>
        /// 清空播放列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delPlay_Click(object sender, RoutedEventArgs e)
        {
            pVLis.Items.Clear();
            playbackList.Items.Clear();
        }

        /// <summary>
        /// 切换播放列表开闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaylistControl_Switch(object sender, EventArgs e)
        {
            if (this.splitView.IsPaneOpen == true)
            {
                this.splitView.IsPaneOpen = false;
            }
            else
            {
                this.splitView.IsPaneOpen = true;
            }
        }
    }
}
