using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;

namespace SimpleModernVideoPlayer.Utils
{
    static class PhotoConverter
    {
        public static async Task<ImageSource> SaveToImageSource(this byte[] imageBuffer)
        {
            ImageSource imageSource = null;
            using (MemoryStream stream = new MemoryStream(imageBuffer))
            {
                try
                {
                    var ras = stream.AsRandomAccessStream();
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.PngDecoderId, ras);
                    var provider = await decoder.GetPixelDataAsync();
                    byte[] buffer = provider.DetachPixelData();
                    WriteableBitmap bitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                    await bitmap.PixelBuffer.AsStream().WriteAsync(buffer, 0, buffer.Length);
                    imageSource = bitmap;
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return imageSource;
        }
    }
}
