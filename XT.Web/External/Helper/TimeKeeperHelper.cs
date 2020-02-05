using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XT.BusinessService;
using XT.Utilities;
using XT.Model;
using XT.Web;
using XT.Web.Models;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace XT.Web.External
{
    public static class TimeKeeperHelper
    {
        public static string TimeStartWork_1
        {
            get
            {
                return AppSettings.TimeStartWork_1;
            }
        }
        public static string TimeEndWork_1
        {
            get
            {
                return AppSettings.TimeEndWork_1;
            }
        }
        public static string TimeStartWork_2
        {
            get
            {
                return AppSettings.TimeStartWork_2;
            }
        }
        public static string TimeEndWork_2
        {
            get
            {
                return AppSettings.TimeEndWork_2;
            }
        }
        public static string TimeEndWork_Saturday
        {
            get
            {
                return AppSettings.TimeEndWork_Saturday;
            }
        }
        //public static string MaxLateStartWork
        //{
        //    get
        //    {
        //        return AppSettings.MaxLateStartWork;
        //    }
        //}
        /// <summary>
        /// Default Setting V1
        /// </summary>
        public static TimeSpan V1
        {
            get { return ToTimeSpan(TimeStartWork_1); }
        }
        /// <summary>
        /// Default Setting R1
        /// </summary>
        public static TimeSpan R1
        {
            get { return ToTimeSpan(TimeEndWork_1); }
        }
        /// <summary>
        /// Default Setting V2
        /// </summary>
        public static TimeSpan V2
        {
            get { return ToTimeSpan(TimeStartWork_2); }
        }
        /// <summary>
        /// Default Setting R2
        /// </summary>
        public static TimeSpan R2
        {
            get { return ToTimeSpan(TimeEndWork_2); }
        }
        /// <summary>
        /// Default Setting R_Saturday
        /// </summary>
        public static TimeSpan R_Saturday
        {
            get { return ToTimeSpan(TimeEndWork_Saturday); }
        }
        /// <summary>
        /// Default Setting V1_MaxLate
        /// </summary>
        public static TimeSpan MaxLateStartWork
        {
            get { return ToTimeSpan(AppSettings.MaxLateStartWork); }
        }

        private static DateTime[] _DateOffs = null;
        public static DateTime[] DateOffs
        {
            get
            {
                if (_DateOffs != null)
                {
                    return _DateOffs;
                }
                if (!string.IsNullOrEmpty(AppSettings.DateOff))
                {
                    _DateOffs = AppSettings.DateOff.Split(';').Select(s => DateTime.Parse(s)).ToArray();
                }

                return _DateOffs;
            }
            set
            {
                _DateOffs = value;
            }
        }


        public static DateTime? GetDateOff(DateTime date)
        {
            if (DateOffs.Any(d => d.Date == date.Date))
                return DateOffs.FirstOrDefault(d => d.Date == date.Date);
            return null;
        }

        public static TimeSpan ToTimeSpan(string timeString)
        {
            var arr = timeString.Split('-').Select(e => Convert.ToInt32(e)).ToArray();
            return new TimeSpan(arr[0], arr[1], arr[2]);
        }

        public static IEnumerable<DateTime> GetWorkDays(DateTime Start_Date, DateTime End_Date)
        {
            for (int i = 0; Start_Date.AddDays(i) <= End_Date; i++)
            {
                var currDate = Start_Date.AddDays(i);
                yield return currDate.Date;
            }
        }

        public static float GetWorkingDaysCount(DateTime Start_Date, DateTime End_Date)
        {
            var count = 0f;

            for (var currDate = Start_Date.Date; currDate <= End_Date.Date; currDate = currDate.AddDays(1))
            {
                if (!currDate.IsWeekend())
                {
                    count++;
                }
                else
                {
                    if (currDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        count += AppSettings.SaturdayWorkDay * 0.5f;// * GetWorkingDayForOneCheckin();
                    }
                }
            }

            return count;
        }

        public static IEnumerable<ShiftTime> GetShiftTimesForOneDate(DateTime date, User_Profile user)
        {
            var WeekWorkDay = AppSettings.WeekWorkDay;

            var list = new List<ShiftTime>();

            if (date.IsWeekend())
            {
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    //Common
                    var R12 = AppSettings.SaturdayWorkDay == 1 ? R1 : R_Saturday;
                    list.Add(new ShiftTime(date, V1, R12, true, MaxLateStartWork));

                    //LHMT only
                    if (WeekWorkDay == 2 && AppSettings.SaturdayWorkDay == 1)
                    {
                        list.Add(new ShiftTime(date, V2, R2, false));
                    }
                }

                //For shown in calendar, not for timekeeping
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    list.Add(new ShiftTime(date, V1, R2, false));//, V1_MaxLate));
                }
            }
            else
            {
                if (WeekWorkDay == 1)
                {
                    list.Add(new ShiftTime(date, V1, R2, true, MaxLateStartWork));
                }
                if (WeekWorkDay == 2)
                {
                    list.Add(new ShiftTime(date, V1, R1, true, MaxLateStartWork));
                    list.Add(new ShiftTime(date, V2, R2));
                }
            }

            return list;
        }

        public static int GetMaxWaitingMinutes()
        {
            return AppSettings.MaxWaitingMinutesBetweenTimekeepers;
        }

        public static string CallApiGetUserAddressByLocation(string location)
        {
            string[] userLatLong = location.Split(',');
            string apiUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + userLatLong[0] + "," + userLatLong[1] + "&key=" + AppSettings.GGMapApiKey;

            HttpClient client = new HttpClient();
            var responseTask = client.GetAsync(apiUrl);
            var data = responseTask.Result.Content.ReadAsStringAsync().Result;
            JObject jObjectData = JObject.Parse(data);
            if (jObjectData["results"].Count() > 0)
            {
                return jObjectData["results"][0]["formatted_address"].ToString();
            }
            return "";
        }
    }
}