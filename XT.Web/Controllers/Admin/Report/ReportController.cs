using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;
using System.Web.Script.Serialization;
using System.IO;
using PagedList;
using PagedList.Mvc;
using XT.Web.Models;
using XT.Web.External.MVCAttributes;
using System.Configuration;

namespace XT.Web.Controllers
{
    public partial class ReportController : AdminBaseController
    {
        #region Report_Academic

        #region Report_Academic_FC_Salary
        private IEnumerable<IGrouping<Faculty, Class_Module_Day>> GetClassModuleReport(DateTime Start_Date, DateTime End_Date)
        {
            var items = IoCConfig.Service<IClass_Module_DayService>()
                .FindAllValidByCriteria(c => c.Class_Module_Day_Status == (int)ClassModuleDayStatusEnum.Studying 
                                            && Start_Date <= c.Class_Module_Day_Date
                                            && c.Class_Module_Day_Date <= End_Date)
                .GroupBy(c => c.Class_Module.Faculty);

            return items;
        }

        public ActionResult Report_Academic_FC_Salary(
            DateTime? Start_Date,
            DateTime? End_Date)
        {
            if (Start_Date == null)
                Start_Date = DateTime.Today.StartOfMonth();
            if (End_Date == null)
                End_Date = DateTime.Today.EndOfMonth();

            var items = GetClassModuleReport(Start_Date.Value, End_Date.Value);

            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;

            return View(items);
        }

        public ActionResult FilterReport_Academic_FC_Salary(
            DateTime Start_Date,
            DateTime End_Date)
        {
            var items = GetClassModuleReport(Start_Date, End_Date);

            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;

            return PartialView("_partial_Report_Academic_FC_Salary", items);
        }
        #endregion Report_Academic_FC_Salary

        #region Report_Academic_Exam_Plan
        private IEnumerable<Class_Module> GetExamReport(DateTime Start_Date, DateTime End_Date)
        {
            var items = IoCConfig.Service<IClass_ModuleService>()
                .FindAllValidByCriteria(c => c.Class_Module_Status == (int)ClassModuleStatusEnum.Studying 
                                            && Start_Date <= c.Class_Module_Date_Exam
                                            && c.Class_Module_Date_Exam <= End_Date);

            return items;
        }

        public ActionResult Report_Academic_Exam_Plan(
            DateTime? Start_Date,
            DateTime? End_Date)
        {
            if (Start_Date == null)
                Start_Date = DateTime.Today.StartOfMonth();
            if (End_Date == null)
                End_Date = DateTime.Today.EndOfMonth();

            var items = GetExamReport(Start_Date.Value, End_Date.Value);

            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;

            return View(items);
        }

        public ActionResult FilterReport_Academic_Exam_Plan(
            DateTime Start_Date,
            DateTime End_Date)
        {
            var items = GetExamReport(Start_Date, End_Date);

            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;

            return PartialView("_partial_Report_Academic_Exam_Plan", items);
        }
        #endregion Report_Academic_Exam_Plan

        #region Report_Academic_FC_Passrate
        private IEnumerable<IGrouping<Faculty, Class_Module>> GetExamReportGroupByFaculty(DateTime Start_Date, DateTime End_Date)
        {
            var items = GetExamReport(Start_Date, End_Date).GroupBy(c => c.Faculty);

            return items;
        }

        public ActionResult Report_Academic_FC_Passrate(
            DateTime? Start_Date,
            DateTime? End_Date)
        {
            if (Start_Date == null)
                Start_Date = DateTime.Today.StartOfMonth();
            if (End_Date == null)
                End_Date = DateTime.Today.EndOfMonth();

            var items = GetExamReportGroupByFaculty(Start_Date.Value, End_Date.Value);

            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;

            return View(items);
        }

        public ActionResult FilterReport_Academic_FC_Passrate(
            DateTime Start_Date,
            DateTime End_Date)
        {
            var items = GetExamReportGroupByFaculty(Start_Date, End_Date);

            ViewBag.Start_Date = Start_Date;
            ViewBag.End_Date = End_Date;

            return PartialView("_partial_Report_Academic_FC_Passrate", items);
        }
        #endregion Report_Academic_FC_Passrate

        #endregion Report_Academic

        public ActionResult Report_FeePlan()
        {
            return View();
        }
    }
}