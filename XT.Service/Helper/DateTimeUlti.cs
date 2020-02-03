using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace XT.BusinessService
{
    public static class DateTimeUlti
    {
        public static DateTime MinDate(DateTime s1, DateTime s2)
        {
            return s1 < s2 ? s1 : s2;
        }

        public static DateTime MaxDate(DateTime s1, DateTime s2)
        {
            return s1 > s2 ? s1 : s2;
        }

        public static DateTime StartOfWeek()
        {
            return StartOfWeek(DateTime.Today);
        }

        public static DateTime EndOfWeek()
        {
            return EndOfWeek(DateTime.Today);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek = DayOfWeek.Sunday)
        {
            int diff = endOfWeek - dt.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(1 * diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        public static DateTime Parse(string date)
        {
            //return DateTime.ParseExact(date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.AssumeUniversal);
            return DateTime.ParseExact(date, "dd/MM/yyyy", new CultureInfo("vi-VN"));
        }

        public static bool IsWeekend(this DateTime dt)
        {
            return (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
        }

        public static bool IsEndOfMonth(this DateTime dt)
        {
            return dt.AddDays(1).Day == 1;
        }

        public static string ToDateString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string ToDateString(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "";
        }

        public static string ToMonthString(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("MM/yyyy") : "";
        }

        public static DateTime? ToBirthDay(this DateTime? date)
        {
            if (date == null)
                return null;
            return date.Value.ToBirthDay();
        }

        public static DateTime? ToBirthDay(this DateTime date)
        {
            DateTime? birthday = null;
            try
            {
                birthday = new DateTime(DateTime.Today.Year, date.Month, date.Day);
            }
            catch (Exception ex)
            {
                return null;
            }

            return birthday;
        }

        public static string GetDayOfWeek(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
                return "CN";
            return ((int)date.DayOfWeek + 1).ToString();
        }

        /// <summary>
        /// Hàm tính khoảng ngày từ ngày bắt đầu đến ngày kết thúc (VD: từ 1/12/2014 đến 4/12/2014 sẽ có 4 ngày)
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int Get2DaysDuration(DateTime startDate, DateTime endDate)
        {
            //nếu date2 - date1 sẽ chỉ có 3 ngày (4-1=3), phải cộng thêm 1 đơn vị
            //date2 > date1
            if (endDate < startDate)
                return -1;
            return (endDate.Date - startDate.Date).Days + 1;
        }
    }

    public static class NumberUlti
    {
        public static string ToHourString(this float time)
        {
            var hour = Math.Floor(time);
            //var min = Math.Round((time - hour) * 100);
            var min = Math.Round((time - hour) * 60);

            if (min == 0)
                return hour.ToString();

            return hour.ToString() + ":" + min.ToString();
        }

        public static string ToUTCHourString(this float time)
        {
            time = time - 7;
            var hour = Math.Floor(time);
            //var min = Math.Round((time - hour) * 100);
            var min = Math.Round((time - hour) * 60);

            //if (min == 0)
            //    return hour.ToString();

            var hour_s = hour < 10 ? "0" + hour.ToString() : hour.ToString();
            var min_s = min < 10 ? "0" + min.ToString() : min.ToString();

            return hour_s + ":" + min_s + ":00";
        }
    }
}