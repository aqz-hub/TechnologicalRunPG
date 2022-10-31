using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace TechnologicalRunPG.HW
{
    class BitmapWork
    {
        /// <summary>
        /// Преобразовать Bitmap в BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
