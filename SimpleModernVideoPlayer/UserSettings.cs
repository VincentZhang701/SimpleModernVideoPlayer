using System;
using System.Collections.Generic;
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
        public BitmapImage _userAvatar
        {
            get
            {
                return getUserAvatar().Result;
            }
            set
            {
                //Implementing
                this.RaisePropertyChange("_userAvatar");
            }
        }
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
        
        /// <summary>
        /// 从设置存储获得用户头像
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户头像的Bitmap</returns>
        private async Task<BitmapImage> getUserAvatar()
        {
            try
            {
                StorageFile userAvatar = await localFolder.GetFileAsync("Uimg.jpg");
                using (var randomAccessStream = await userAvatar.OpenAsync(FileAccessMode.Read))
                {
                    var result = new BitmapImage();
                    await result.SetSourceAsync(randomAccessStream);
                    return result;
                }
            }
            catch (Exception)
            {
                return new BitmapImage(new Uri("ms-appx:///Assets/130-512.png"));
            }
        }

        //测试用
        public void setExample()
        {
            localSettings.Values["UserName"] = "Vinnocent";
            localSettings.Values["UserPwd"] = "123456";
            localSettings.Values["IsLoggedIn"] = true;
        }

        public async void saveThubnail(WriteableBitmap bitmapImage, string filename)
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
