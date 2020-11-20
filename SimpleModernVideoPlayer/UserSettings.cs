using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SimpleModernVideoPlayer
{
    public class UserSettings:NotificationObject
    {
        // 设置存储
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;


        //ID
        public string _userID
        {
            get
            {
                return localSettings.Values["UserID"].ToString();
            }
            set
            {
                localSettings.Values["UserID"] = value;
                this.RaisePropertyChange("_userID");
            }
        }
        //用户名
        public string _userName
        {
            get
            {
                return localSettings.Values["UserName"].ToString();

            }
            set
            {
                localSettings.Values["UserName"] = value;
                this.RaisePropertyChange("_userName");
            }
        }
        //用户头像
        public ImageSource _UserAvatar;
        public ImageSource _userAvatar { get { return _UserAvatar; } set { _UserAvatar = value; this.RaisePropertyChange("_userAvatar"); } }
        //用户密码
        public string _userPwd
        {
            get
            {
                return localSettings.Values["UserPwd"].ToString();
            }
            set
            {
                localSettings.Values["UserPwd"] = value;
                this.RaisePropertyChange("_userName");
            }
        }
        //是否已经登陆
        public bool _isLoggedIn
        {
            get
            {
                return bool.Parse(localSettings.Values["IsLoggedIn"].ToString());
            }
            set
            {
                localSettings.Values["IsLoggedIn"] = value;
                this.RaisePropertyChange("_isLoggedIn");
            }
        }
        //设置存储
        public string _localfolder { get { return localFolder.Path; } }

        //测试用
        public void setExample()
        {
            localSettings.Values["UserID"] = "123456789";
            localSettings.Values["UserName"] = "Vinnocent";
            localSettings.Values["UserPwd"] = "123456";
            localSettings.Values["IsLoggedIn"] = true;
        }

        public async void saveBitmapimage(WriteableBitmap bitmapImage, string filename)
        {
            StorageFile thumbFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            Guid bitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            using (IRandomAccessStream stream = await thumbFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(bitmapEncoderGuid, stream);
                Stream pixelStream = bitmapImage.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                          (uint)bitmapImage.PixelWidth,
                          (uint)bitmapImage.PixelHeight,
                          96.0,
                          96.0,
                          pixels);
                await encoder.FlushAsync();
            }
        }
    }
}
