using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.Utilities
{
    public static class GeneralUtils
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T[] EnumToArray<T>()
        {
            var array = Enum.GetValues(typeof (T));
            return array.Cast<T>().ToArray();
        }

        public static string TimeAgo(DateTime date)
        {
            TimeSpan timeSince = DateTime.Now.Subtract(date);
            if (timeSince.TotalMilliseconds < 1)
                return "Vãi cả tương lai";// "not yet";
            if (timeSince.TotalMinutes < 1)
                return "Bây giờ";// "Just now";
            if (timeSince.TotalMinutes < 2)
                return "Cách đây 1 phút";// "1 minute ago";
            if (timeSince.TotalMinutes < 60)
                return string.Format("Cách đây {0} phút", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120)
                return "Cách đây 1 giờ";
            if (timeSince.TotalHours < 6)
                return string.Format("Cách đây {0} giờ", timeSince.Hours);
            if (timeSince.TotalHours < 24)
                return "Hôm nay lúc " + date.ToString("HH:mm");
            if (timeSince.TotalDays == 1)
                return "Hôm qua lúc " + date.ToString("HH:mm");
            if (timeSince.TotalDays < 7)
                return string.Format("cách đây {0} ngày", timeSince.Days);
            if (timeSince.TotalDays < 14)
                return "Tuần trước";
            if (timeSince.TotalDays < 21)
                return "Cách đây 2 tuần";
            if (timeSince.TotalDays < 28)
                return "Cách đây 3 tuần";
            if (timeSince.TotalDays < 60)
                return "Tháng trước";
            if (timeSince.TotalDays < 365)
                return string.Format("Cách đây {0} tháng", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730)
                return "Năm trước";

            //last but not least...
            return string.Format("Cách đây {0} năm", Math.Round(timeSince.TotalDays / 365));

        }

        public static string UpperCaseFirst(string p)
        {
            if (string.IsNullOrEmpty(p))
            {
                return string.Empty;
            }

            return char.ToUpper(p[0]) + p.Substring(1);
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        //private static Image cropImage(Image img, Rectangle cropArea)
        //{
        //    Bitmap bmpImage = new Bitmap(img);
        //    Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        //    return (Image)(bmpCrop);
        //}

        //public static Image GetThumbnailImage(string url, int new_width, int new_height = 0)
        //{
        //    Image img = Image.FromFile(url);
        //    int width = new_width;
        //    int height = img.Height * new_width / img.Width;

        //    //resize
        //    Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
        //    img = img.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);

        //    //crop
        //    if (0 < new_height && new_height < height)
        //    {
        //        img = cropImage(img, new Rectangle(0, 0, new_width, new_height));
        //    }

        //    return img;
        //}

        //public static Image GetThumbnailImage(Image img, int new_width, int new_height = 0)
        //{
        //    //int width = new_width;
        //    //int height = img.Height * new_width / img.Width;

        //    ////resize
        //    //Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
        //    //img = img.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);

        //    ////crop
        //    //if (0 < new_height && new_height < height)
        //    //{
        //    //    img = cropImage(img, new Rectangle(0, 0, new_width, new_height));
        //    //}

        //    //return img;

        //    int height = img.Height * new_width / img.Width;

        //    if (new_height > 0)
        //        height = new_height;

        //    return img.ResizeCropExcess(new_width, height);
        //}

        //public static void DeleteImage(string StorageRoot, string targetType, string imageType, string id, double width, double height)
        //{
        //    var widthName = "";
        //    if (width > 0)
        //    {
        //        widthName = "_" + width.ToString();
        //    }
        //    var heightName = "";
        //    if (height > 0)
        //    {
        //        heightName = "_" + height.ToString();
        //    }
        //    var imagePathNoHeight = StorageRoot + "/" + targetType + "Upload/" + id + "_" + imageType + widthName + ".jpg";
        //    var imagePath = StorageRoot + "/" + targetType + "Upload/" + id + "_" + imageType + widthName + heightName + ".jpg";
        //    if (File.Exists(imagePathNoHeight))
        //    {
        //        File.Delete(imagePathNoHeight);
        //    }
        //    if (File.Exists(imagePath))
        //    {
        //        File.Delete(imagePath);
        //    }
        //}

        //public static double Rad(double x)
        //{
        //    return x * Math.PI / 180;
        //}
    }
}
