using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;

namespace SimpleModernVideoPlayer
{
    public class MediaModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sf">媒体地址</param>
        public MediaModel(StorageFile sf)
        {
            MediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromStorageFile(sf));
        }

        /// <summary>
        /// 媒体标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 媒体缩略图URI
        /// </summary>
        public WriteableBitmap ArtUri { get; set; }

        /// <summary>
        /// 播放进度
        /// </summary>
        public TimeSpan PlaybackPosition { get; set; }

        /// <summary>
        /// 播放历史时间
        /// </summary>
        public DateTime PlaybackHistory { get; set; }
        public string sPlaybackHistory { get { return PlaybackHistory.Year + "/" + PlaybackHistory.Month + "/" + PlaybackHistory.Day; } }

        public bool canSync { get; set; }

        /// <summary>
        /// 播放列表项目（使用构造URI初始化）
        /// </summary>
        public MediaPlaybackItem MediaPlaybackItem { get; private set; }
    }
}
