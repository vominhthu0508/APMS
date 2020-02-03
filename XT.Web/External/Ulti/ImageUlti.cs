using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.Web.External
{
    public static class ImageUlti
    {
        public static Image Resize(this Image img, int srcX, int srcY, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            var bmp = new Bitmap(dstWidth, dstHeight);
            bmp.SetResolution(72, 72);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //graphics.DrawImage(img, 0, 0, dstWidth, dstHeight);

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    var destRect = new Rectangle(0, 0, dstWidth, dstHeight);
                    graphics.DrawImage(img, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return bmp;
        }

        public static Image ResizeProportional(this Image img, int width, int height, bool enlarge = false)
        {
            double ratio = Math.Max(img.Width / (double)width, img.Height / (double)height);
            if (ratio < 1 && !enlarge) return img;
            return img.Resize(0, 0, img.Width, img.Height, (int)Math.Round(img.Width / ratio), (int)Math.Round(img.Height / ratio));
        }

        public static Image ResizeCropExcess(this Image img, int dstWidth, int dstHeight)
        {
            double srcRatio = img.Width / (double)img.Height;
            double dstRatio = dstWidth / (double)dstHeight;
            int srcX, srcY, cropWidth, cropHeight;

            if (srcRatio < dstRatio) // trim top and bottom
            {
                cropHeight = dstHeight * img.Width / dstWidth;
                srcY = (img.Height - cropHeight) / 2;
                cropWidth = img.Width;
                srcX = 0;
            }
            else // trim left and right
            {
                cropWidth = dstWidth * img.Height / dstHeight;
                srcX = (img.Width - cropWidth) / 2;
                cropHeight = img.Height;
                srcY = 0;
            }

            return img.Resize(srcX, srcY, cropWidth, cropHeight, dstWidth, dstHeight);
        }

        public static Image ResizeByWidth(Bitmap img, int newWidth)
        {
            if (img.Width > newWidth)
            {
                int newHeight = img.Height * newWidth / img.Width;
                img = img.Resize(0, 0, img.Width, img.Height, newWidth, newHeight) as Bitmap;
            }
            return img as Image;
        }
    }
}