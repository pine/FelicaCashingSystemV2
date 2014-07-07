using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfCommonds
{
    public static class BitmapToBitmapSourceConverter
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap bitmap)
        {
            var hBitmap = bitmap.GetHbitmap();
            
            BitmapSource source = null;
            try
            {
                source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
            return source;
        }

        public static BitmapSource ToBitmapSource(this byte[] imageData)
        {
            var bitmap = imageData.ToBitmap();

            if (bitmap != null)
            {
                return bitmap.ToBitmapSource();
            }

            return null;
        }
    }
}
