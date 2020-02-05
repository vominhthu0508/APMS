using System;
using System.Collections.Generic;
using System.Linq;
using XT.Model;
using XT.Web.External;

namespace XT.Web.Models
{
    #region CalendarTimeKeeperModel (ReportByTimeKeeperCalendar)
    /// <summary>
    /// Ca làm việc
    /// </summary>
    public class ShiftTime
    {
        public DateTime Ngay { get; set; }
        public TimeSpan Vao { get; set; }
        public TimeSpan Ra { get; set; }
        public TimeSpan? Vao_MaxLate { get; set; }
        public bool IsWorking { get; set; }
        public TimeSpan GiuaCa
        {
            get
            {
                var diff = (Ra - Vao).TotalSeconds / 2;
                return Vao.Add(new TimeSpan(0, 0, (int)diff));
            }
        }

        public ShiftTime()
        {
            IsWorking = false;
        }

        public ShiftTime(DateTime date, TimeSpan vao, TimeSpan ra, bool isWorking = true, TimeSpan? vao_max_late = null)
        {
            Ngay = date;
            Vao = vao;
            Vao_MaxLate = vao_max_late;
            Ra = ra;
            IsWorking = isWorking;
        }
    }

    public class CalendarTimeKeeperModel
    {
        public User_Profile User_Profile { get; set; }
        public IEnumerable<CalendarData> Calendar_Datas { get; set; }
        public float Count_All { get; set; }
        public float Count_M { get; set; }
        public float Count_MX { get; set; }
        public float Count_S { get; set; }
        public float Count_MS { get; set; }
        public float Count_X { get; set; }

        public class CalendarData
        {
            public int User_Id { get; set; }
            public ShiftTime CheckinTime { get; set; }
            public CalendarData Pre_Shift { get; set; }
            public List<Timekeeper> Timekeepers { get; set; }
            public string Label_Timekeeper { get; set; }
            public bool IsWork { get; set; }

            public CalendarData()
            {
                Timekeepers = new List<Timekeeper>();
                IsWork = false;
            }

            public TimeSpan? VaoThuc
            {
                get
                {
                    var tk = Timekeepers.FirstOrDefault();
                    if (tk == null || tk.Checkin_Date.TimeOfDay > CheckinTime.GiuaCa)
                        return null;

                    return tk.Checkin_Date.TimeOfDay;
                }
            }
            public TimeSpan? RaThuc
            {
                get
                {
                    var tk = Timekeepers.LastOrDefault();
                    if (tk == null || tk.Checkin_Date.TimeOfDay < CheckinTime.GiuaCa)
                        return null;

                    return tk.Checkin_Date.TimeOfDay;
                }
            }

            public bool HasAlreadyCheckIn
            {
                get
                {
                    return Timekeepers.Count > 0;
                }
            }

            public bool HasAlreadyCheckOut
            {
                get
                {
                    return Timekeepers.Count > 1 && Timekeepers.Count(t => t.Checkin_Date.TimeOfDay > CheckinTime.GiuaCa) > 0;
                }
            }

            private string GetTimekeeperLabel_CheckIn()
            {
                var label = "";
                if (VaoThuc != null)
                {
                    if (CheckinTime.Vao < VaoThuc)
                    {
                        label = "M";
                        if (CheckinTime.Vao_MaxLate.HasValue && VaoThuc <= CheckinTime.Vao_MaxLate)
                        {
                            label += "X";
                        }


                    }
                    else
                    {
                        label = "X";
                    }
                }

                return label;
            }

            private string GetTimekeeperLabel_CheckOut()
            {
                var label = "";

                if (RaThuc != null)
                {
                    if (CheckinTime.Ra > RaThuc)
                    {
                        label = "S";
                    }
                    else
                    {
                        label = "X";
                    }
                }

                return label;
            }

            public string GetTimekeeperLabel()
            {
                var workingHours = (CheckinTime.Ra - CheckinTime.Vao).TotalHours + 1;
                var label = "";
                var label_CheckIn = GetTimekeeperLabel_CheckIn();
                var label_CheckOut = GetTimekeeperLabel_CheckOut();
                if (label_CheckIn == "X" && label_CheckOut == "X")
                {
                    label = "X";
                }
                else
                {
                    if (label_CheckOut == "" || label_CheckIn == "")
                    {
                        label = "";//Ro
                    }
                    else
                    {
                        if (label_CheckIn != "X")
                        {
                            label += label_CheckIn;
                        }
                        if (label_CheckOut != "X")
                        {
                            label += label_CheckOut;
                        }
                    }
                }

                if (label == "X")
                {
                    if (workingHours < AppSettings.MaxWorkHour / AppSettings.WeekWorkDay)
                        label += "/2";
                }

                if (label == "")
                {
                    label = "Ro";
                }

                return label;
            }

            public string ToTimekeeperHtmlLabel()
            {
                var html = "<ul>";
                foreach (var t in this.Timekeepers.Valid())
                {
                    var title = "";
                    var ip = "";
                    var location = "";
                    if (t.IP_Modem != null)
                    {
                        ip = "IP: " + t.IP + " (" + t.IP_Modem + ")";
                        ip = "<span>" + ip + "</span>";
                    }
                    if (t.Location != null)
                    {
                        //title = "Chấm công máy";
                        location = "<span class='google-location' data-address='" + t.Location + "'>" + title + "</span>";
                    }

                    var time = t.Checkin_Date.ToString("dd/MM/yyyy HH:mm:ss");
                    if (t.Photo != null)
                    {
                        time += " <img src='" + Helper.MyUrlContent_DefaultImage(t.Photo) + "' class='img-small' />";
                    }

                    html += "<li>" +
                                 time +
                                "<br/>" +
                                ip +
                                "<br/>" +
                                 location +
                            "</li>";
                }

                html += "</ul>";

                return html;
            }

            public string SetTimekeeperWorkingLabel()
            {
                Label_Timekeeper = GetTimekeeperLabel();
                IsWork = Label_Timekeeper != "" && Label_Timekeeper.StartsWith("X");

                return Label_Timekeeper;
            }
        }

        #region [private] Calculate Timekeeper by Shifts

        private static CalendarData GetWorkingShift(CalendarData shift)
        {
            return shift != null && shift.CheckinTime.IsWorking ? shift : null;
        }

        private static CalendarData GetCurrentShiftByUnshiftedTimekeeper(Timekeeper tk, CalendarData pre_shift, CalendarData post_shift)
        {
            CalendarData curr_shift = null;

            if (pre_shift == null)
            {
                if (post_shift != null)
                {
                    curr_shift = post_shift;
                }
            }
            else
            {
                if (post_shift == null)
                {
                    curr_shift = pre_shift;
                }
                else//giữa ca
                {
                    if (pre_shift.HasAlreadyCheckIn && !pre_shift.HasAlreadyCheckOut)//chưa check đủ ca
                    {
                        curr_shift = pre_shift;
                    }
                    else//ca trước đã đủ
                    {
                        curr_shift = post_shift;
                    }
                }
            }

            return curr_shift;
        }

        private static CalendarData[] GetCurrentDateShiftsForOneDate(
            User_Profile user,
            DateTime curr_date,
            IEnumerable<Timekeeper> curr_date_Timekeepers,
            IEnumerable<ShiftTime> curr_date_ShiftTimes = null)
        {
            var curr_date_Shifts = curr_date_ShiftTimes.Select(s => new CalendarData { User_Id = user.Id, CheckinTime = s, Pre_Shift = null }).ToArray();

            //add timekeeper
            foreach (var tk in curr_date_Timekeepers)
            {
                var x = tk.Checkin_Date.TimeOfDay;
                var curr_shift = curr_date_Shifts.FirstOrDefault(s => s.CheckinTime.Vao <= x && x <= s.CheckinTime.Ra);
                curr_shift = GetWorkingShift(curr_shift);

                if (curr_shift == null)
                {
                    var pre_shift = curr_date_Shifts.Where(s => s.CheckinTime.Ra < x).OrderBy(s => s.CheckinTime.Ra).LastOrDefault();
                    var post_shift = curr_date_Shifts.Where(s => s.CheckinTime.Vao > x).OrderBy(s => s.CheckinTime.Vao).FirstOrDefault();
                    pre_shift = GetWorkingShift(pre_shift);
                    post_shift = GetWorkingShift(post_shift);

                    curr_shift = GetCurrentShiftByUnshiftedTimekeeper(tk, pre_shift, post_shift);
                }

                if (curr_shift != null)
                {
                    curr_shift.Timekeepers.Add(tk);
                }
            }

            for (var idx = 1; idx < curr_date_Shifts.Length; idx++)
            {
                curr_date_Shifts[idx].Pre_Shift = curr_date_Shifts[idx - 1];
            }

            return curr_date_Shifts;
        }
        #endregion

        #region [private] CalculateReport

        private void CalculateReport()
        {
            var today = DateTime.Today;
            Count_All = 0;
            foreach (var calendar_data in Calendar_Datas)
            {
                if (calendar_data.CheckinTime.Ngay <= today && calendar_data.Timekeepers.Any())
                {
                    var label_TK = calendar_data.SetTimekeeperWorkingLabel();
                    CountReport_Timekeeper(label_TK);
                }
            }

            Count_X = Count_X / AppSettings.WeekWorkDay;
        }

        private void CalculateReport_Timekeeper(bool withCountLabel = true)
        {
            foreach (var calendar_data in Calendar_Datas)
            {
                if (calendar_data.Timekeepers.Any())
                {
                    var label_TK = calendar_data.SetTimekeeperWorkingLabel();
                    if (withCountLabel)
                        CountReport_Timekeeper(label_TK);
                }
            }

            Count_X = Count_X / AppSettings.WeekWorkDay;
        }

        private void CountReport_Timekeeper(string label)
        {
            Count_All++;
            //if (label.Contains("MX"))
            if (label == "MX")
            {
                Count_MX++;
                //Count_M++;
                //if (Count_MX <= AppSettings.MaxLateTimesPerMonth)
                //{
                //    if (label.Contains("/2"))
                //    {
                //        Count_X += 0.5f;
                //    }
                //    else
                //    {
                //        Count_X++;
                //    }
                //}
            }

            switch (label)
            {
                case "MS":
                case "MXS":
                    {
                        Count_MS++;
                        Count_M++;
                        Count_S++;
                        break;
                    }
                case "M":
                    {
                        Count_M++;
                        break;
                    }
                case "S":
                    {
                        Count_S++;
                        break;
                    }
                case "X":
                    {
                        //Count_X++;
                        break;
                    }
                case "X/2":
                    {
                        //Count_X += 0.5f;
                        break;
                    }
            }

            Count_X += CountReport_TimekeeperByLabel(label, Count_MX);
        }

        public static float CountReport_TimekeeperByLabel(string label, float Count_MX = 0)
        {
            float Count_X = 0;
            if (label == "MX")
            {
                if (Count_MX <= AppSettings.MaxLateTimesPerMonth)
                {
                    if (label.Contains("/2"))
                    {
                        Count_X += 0.5f;
                    }
                    else
                    {
                        Count_X++;
                    }
                }
            }

            switch (label)
            {
                case "MS":
                case "MXS":
                    {
                        break;
                    }
                case "M":
                    {
                        break;
                    }
                case "S":
                    {
                        break;
                    }
                case "X":
                    {
                        Count_X++;
                        break;
                    }
                case "X/2":
                    {
                        Count_X += 0.5f;
                        break;
                    }
            }

            return Count_X;
        }

        #endregion CalculateReport

        #region GetCalendarTimeKeeperReportByUser for Start_Date & End_Date
        private static CalendarTimeKeeperModel CalculateCalendarTimeKeepersByUser(User_Profile user, List<DateTime> work_Days, DateTime Start_Date, DateTime End_Date)
        {
            IEnumerable<Timekeeper> u_TimekeepersByDate = user.GetValidCheckinsByStartAndEndDate(Start_Date, End_Date);//Get all timekeepers

            var Calendar_Datas = new List<CalendarData>();

            if (u_TimekeepersByDate.Any())
            {
                for (int i = 0; i < work_Days.Count; i++)
                {
                    var curr_date = work_Days[i].Date;
                    var curr_date_ShiftTimes = TimeKeeperHelper.GetShiftTimesForOneDate(curr_date, user);

                    //var curr_date_Timekeepers = u_TimekeepersByDate.Where(t => t.Checkin_Date.Date == curr_date);

                    //Requests && TimekeeperFromRequests
                    var curr_date_Timekeepers = u_TimekeepersByDate.Where(t => t.Checkin_Date.Date == curr_date);

                    //Calculate Shifttime from Timekeepers
                    curr_date_Timekeepers = curr_date_Timekeepers.OrderBy(e => e.Checkin_Date);
                    var curr_date_Shifts = GetCurrentDateShiftsForOneDate(user, curr_date, curr_date_Timekeepers, curr_date_ShiftTimes);

                    //Set request to Shifttime
                    if (curr_date_Shifts.Length > 0)
                    {
                        foreach (var calendar_data in curr_date_Shifts)
                        {
                            var calendarTime = calendar_data.CheckinTime;
                        }
                    }


                    Calendar_Datas.AddRange(curr_date_Shifts);
                }
            }

            var user_Report = new CalendarTimeKeeperModel
            {
                User_Profile = user,
                Calendar_Datas = Calendar_Datas
            };
            user_Report.CalculateReport();

            return user_Report;
        }

        public static CalendarTimeKeeperModel GetCalendarTimeKeeperReportByUser(
            User_Profile user,
            DateTime Start_Date,
            DateTime End_Date)
        {
            var work_Days = TimeKeeperHelper.GetWorkDays(Start_Date, End_Date).ToList();
            return CalculateCalendarTimeKeepersByUser(user, work_Days, Start_Date, End_Date);
        }

        #endregion

        #region  GetCalendarTimeKeeperReportByAllUsers for Start_Date & End_Date
        public static IEnumerable<CalendarTimeKeeperModel> GetCalendarTimeKeeperReportByAllUsers(
        IEnumerable<User_Profile> users,
        IEnumerable<DateTime> work_Days,
        DateTime Start_Date,
        DateTime End_Date)
        {
            var users_Report = new List<CalendarTimeKeeperModel>();
            var days = work_Days.ToList();

            foreach (var user in users)
            {
                var user_Report = CalculateCalendarTimeKeepersByUser(user, days, Start_Date, End_Date);

                users_Report.Add(user_Report);
            }

            return users_Report;
        }
        #endregion

        #region GetCalendarTimekeeperReportByUser For OneDate
        private static CalendarTimeKeeperModel GetCalendarTimekeeperReportForOneDate(
            User_Profile user,
            DateTime curr_date,
            IEnumerable<Timekeeper> curr_date_Timekeepers,
            bool withCalculateLabel = true)
        {
            var Calendar_Datas = GetCurrentDateShiftsForOneDate(user, curr_date, curr_date_Timekeepers);
            var user_Report = new CalendarTimeKeeperModel
            {
                User_Profile = user,
                Calendar_Datas = Calendar_Datas
            };

            user_Report.CalculateReport_Timekeeper(withCalculateLabel);

            return user_Report;
        }

        private static IEnumerable<Timekeeper> GetAllUserTimekeepersForOneDate(
        User_Profile user, DateTime curr_date)
        {
            return user.GetValidCheckinsByDate(curr_date).OrderBy(e => e.Checkin_Date);
        }

        public static CalendarTimeKeeperModel GetCalendarTimekeeperReportForOneDate(
            User_Profile user,
            DateTime curr_date,
            bool withCalculateLabel = true)
        {
            var curr_date_Timekeepers = GetAllUserTimekeepersForOneDate(user, curr_date);

            return GetCalendarTimekeeperReportForOneDate(user, curr_date, curr_date_Timekeepers, withCalculateLabel);
        }

        public static bool IsWorkedCompletely(User_Profile user, DateTime date)
        {
            var report = GetCalendarTimekeeperReportForOneDate(user, date, withCalculateLabel: false);
            return report.Calendar_Datas.Any(c => c.IsWork);
        }

        #endregion

    }
    #endregion
}