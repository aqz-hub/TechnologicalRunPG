using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TechnologicalRunPG.CustomComponents
{
    /// <summary>
    /// Логика взаимодействия для Picture.xaml
    /// </summary>
    public partial class Picture : UserControl
    {
        public Picture()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Установить картинку
        /// </summary>
        /// <param name="_image"></param>
        public void SetImage(Bitmap _image)
        {
            if (_image != null)
            {
                BitmapImage img = Bitmap2BitmapImage(_image);
                imageBrush.ImageSource = img;
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
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
