using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2
{
    public static class BitmapSizeResizer
    {
        /// <summary>
        /// アスペクト比を維持したまま、正方形にリサイズする
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap ResizeSquared(Bitmap image, int size)
        {
            var squaredImage = BitmapSizeResizer.TrimSquared(image);
            var resizedImage = BitmapSizeResizer.Resize(squaredImage, size, size);

            return resizedImage;
        }

        /// <summary>
        /// 画像をアスペクト比を無視しリサイズする
        /// </summary>
        /// <param name="image"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Bitmap Resize(Bitmap image, int w, int h)
        {
            var resizedImage = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(resizedImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, w, h);

            return resizedImage;
        }

        /// <summary>
        /// 四角形にトリミングする
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap TrimSquared(Bitmap image)
        {
            int imageSize;
            int x;
            int y;

            if (image.Width < image.Height)
            {
                imageSize = image.Width;
                x = 0;
                y = (image.Height - image.Width) / 2;
            }

            else
            {
                imageSize = image.Height;
                x = (image.Width - image.Height) / 2;
                y = 0;
            }

            var rect = new Rectangle(x, y, imageSize, imageSize);
            var squaredImage = image.Clone(rect, image.PixelFormat);

            return squaredImage;
        }
    }
}
