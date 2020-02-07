using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;
using XT.Web.Models;
using XT.Web.External.MVCAttributes;

namespace XT.Web.Controllers
{
    [XTAuthorize]
    public partial class ManageTimekeeperController : AdminBaseController
    {
        #region Manage Timekeeper
        public ActionResult ManageTimeKeeper(int? page)
        {
            var Min_Date = DateTime.Now.AddDays(-30);
            var Max_Date = DateTime.Now;

            ViewBag.CanAdd = true;

            var list = GetReportByTimekeerpWithUser(Min_Date, Max_Date, Current_User_Id)
                .GroupBy(e => e.Checkin_Date.Date)
                .OrderByDescending(e => e.Key.Date);

            return View("ReportByTimeKeeperWithUser", list);
        }

        [HttpPost]
        public ActionResult AddTimekeeper(TimekeeperModel model)
        {
            return AddModel(model);
        }
        #endregion

        #region ReportByTimeKeeperWithUser (Chi tiết chấm công cá nhân)
        private IEnumerable<Timekeeper> GetTimekeeperReport(DateTime Start_Date, DateTime End_Date, int user_id = 0)
        {
            var service = IoCConfig.Service<ITimekeeperService>();
            var list = new List<Timekeeper>();
            if (user_id > 0)
            {
                list = service.FindAllValid().Where(e =>
                e.Checkin_Date.Date >= Start_Date.Date &&
                e.Checkin_Date.Date <= End_Date.Date &&
                e.User_Id == user_id)
                .ToList();
            }
            else
            {
                list = service.FindAllValid().Where(e =>
                e.Checkin_Date.Date >= Start_Date.Date &&
                e.Checkin_Date.Date <= End_Date.Date)
                .ToList();
            }
            return list;
        }

        private IEnumerable<Timekeeper> GetReportByTimekeerpWithUser(DateTime Start_Date, DateTime End_Date, int user_id = 0)
        {
            if (user_id > 0 && !AuthenticationManager.CanViewEmployeeTimekeeperData(user_id))
            {
                throw UnauthorizedAccessException();
            }
            var user = IoCConfig.Service<IUser_ProfileService>().FindById(user_id);
            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;
            ViewBag.User_Id = user_id;
            ViewBag.Current_User = user;
            ViewBag.MapApiKey = AppSettings.GGMapApiKey;
            ViewBag.Timekeepers = GetReportByTimekeeperCalendar(Start_Date, End_Date, current_User: user);

            return GetTimekeeperReport(Start_Date, End_Date, user_id);
        }

        public ActionResult ReportByTimeKeeperWithUser(int? page, DateTime? Start_Date, DateTime? End_Date, int id = 0)
        {
            if (id <= 0)
                return RedirectToError("Không tồn tại User này!");

            var Min_Date = Start_Date ?? DateTime.Now.AddDays(-30);
            var Max_Date = End_Date ?? DateTime.Now;

            ViewBag.CanAdd = false;

            var list = GetReportByTimekeerpWithUser(Min_Date, Max_Date, id)
                .GroupBy(e => e.Checkin_Date.Date)
                .OrderByDescending(e => e.Key.Date);

            return View(list);
        }

        public ActionResult FilterReportByTimekeeper(int? page,
            DateTime? Start_Date,
            DateTime? End_Date,
            int User_Id = 0)
        {
            var Min_Date = Start_Date ?? DateTime.Now;//.AddDays(-30);
            var Max_Date = End_Date ?? DateTime.Now;

            var list = GetReportByTimekeerpWithUser(Min_Date, Max_Date, User_Id)
                .GroupBy(e => e.Checkin_Date.Date)
                .OrderByDescending(e => e.Key.Date);

            return PartialView("_partial_Report_TimekeeperWithUser_Search", list);
        }
        #endregion

        #region ReportByTimeKeeperCalendar
        private IEnumerable<CalendarTimeKeeperModel> GetReportByTimekeeperCalendar(
            DateTime? Start_Date,
            DateTime? End_Date,
            int? branch = null,
            User_Profile current_User = null)
        {
            if (current_User != null && !AuthenticationManager.CanViewEmployeeTimekeeperData(current_User.Id))
                throw new UnauthorizedAccessException();

            IEnumerable<User_Profile> users = null;

            if (current_User == null && branch.HasValue)
            {
                users = IoCConfig.Service<IUserProfileService>().FindAllValidByCriteria(e => e.Role_Type_Id != (int)RoleTypeEnum.Admin);
                
                if (branch > 0)
                {
                    users = users.Where(e => e.HasCompany(branch.Value));
                }
            }
            else
            {
                users = new List<User_Profile> { current_User };
            }

            if (!Start_Date.HasValue)
                Start_Date = DateTime.Now.StartOfMonth().AddMonths(-1).AddDays(25);
            if (!End_Date.HasValue)
                End_Date = DateTime.Now.StartOfMonth().AddDays(24);

            if ((End_Date.Value - Start_Date.Value).TotalDays > 60)
            {
                Start_Date = End_Date.Value.AddDays(-60);
            }

            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;
            ViewBag.Branch_Id = branch;

            var work_Days = TimeKeeperHelper.GetWorkDays(Start_Date.Value, End_Date.Value);
            ViewBag.Work_Days = work_Days;

            return CalendarTimeKeeperModel.GetCalendarTimeKeeperReportByAllUsers(users, work_Days, Start_Date.Value, End_Date.Value).Where(r => r.Count_All > 0).OrderByDescending(r => r.User_Profile.User_Type_Id);
        }

        public ActionResult ReportByTimeKeeperCalendar(int? page,
            DateTime? Start_Date,
            DateTime? End_Date,
            int? branch = -1)
        {
            int pageSize = PAGE_SIZE;
            int pageNumber = (page ?? 1);

            var items = GetReportByTimekeeperCalendar(Start_Date, End_Date, branch);
            if (items.Count() == 0)
            {
                var tk = IoCConfig.Service<ITimekeeperService>().FindAllValid().OrderByDescending(t => t.Checkin_Date).FirstOrDefault();
                if (tk != null)
                {
                    var date = tk.Checkin_Date.Date;
                    End_Date = date;
                    Start_Date = date.AddDays(-30);
                    items = GetReportByTimekeeperCalendar(Start_Date, End_Date, branch);
                }
            }

            ViewBag.Page_Size = pageSize;
            ViewBag.Page_Number = pageNumber;

            return View(items);
        }

        public ActionResult FilterReportByTimeKeeperCalendar(int? page,
            DateTime? Start_Date,
            DateTime? End_Date,
            int? branch = null)
        {
            int pageSize = PAGE_SIZE;
            int pageNumber = (page ?? 1);

            var items = GetReportByTimekeeperCalendar(Start_Date, End_Date, branch);

            ViewBag.Page_Size = pageSize;
            ViewBag.Page_Number = pageNumber;
            ViewBag.Branch_Id = branch;

            return PartialView("_partial_Report_TimeKeeperCalendar", items);
        }
        #endregion
    }
}