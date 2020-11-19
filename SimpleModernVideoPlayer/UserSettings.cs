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
    public class UserSettings
    {
        // 设置存储
        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        static Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public static string _userName { get { return localSettings.Values["UserName"].ToString(); } }
        public static BitmapImage _userAvatar { get { return getUserAvatar().Result; } }
        public static string _userPwd { get { return localSettings.Values["UserPwd"].ToString(); } }
        public static bool _isLoggedIn { get { return bool.Parse(localSettings.Values["IsLoggedIn"].ToString()); } }
        public static string _localfolder { get { return localFolder.Path; } }

        private static async Task<BitmapImage> getUserAvatar()
        {
            try
            {
                StorageFile userAvatar = await localFolder.GetFileAsync("UserAvatar.jpg");
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

        public static void setExample()
        {
            localSettings.Values["UserName"] = "Vinnocent";
            localSettings.Values["UserPwd"] = "123456";
            localSettings.Values["IsLoggedIn"] = false;
        }

        public static async void saveThubnail(WriteableBitmap bitmapImage,string filename)
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
