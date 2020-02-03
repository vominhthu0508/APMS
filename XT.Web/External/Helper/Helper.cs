using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XT.BusinessService;
using XT.Model;
using XT.Web.Models;
using XT.Utilities;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Text;
using System.Security.Cryptography;

namespace XT.Web.External
{
    public class Helper
    {
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        //GENERAL

        #region General
        public static string PhoneFakeString(string phone)
        {
            var last2nums = phone;
            if (!string.IsNullOrEmpty(phone) && phone.Length > 2)
                last2nums = phone.Substring(phone.Length - 2, 2);
            return "▒▒▒▒▒▒▒▒▒" + last2nums;
        }

        public static string GetAccountPhoneNumber(string phone)
        {
            if (AuthenticationManager.IsAuthenticated)
                return phone;

            return PhoneFakeString(phone);
        }

        public static string GetAccountEmail()
        {
            if (AuthenticationManager.IsAuthenticated &&
                !AuthenticationManager.Account_Email.Contains("homely.vn"))
                return AuthenticationManager.Account_Email;

            return "";
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Tính khoảng cách theo km (double)
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="long1"></param>
        /// <param name="lat2"></param>
        /// <param name="long2"></param>
        /// <returns></returns>
        public static double CalculateDistance(double lat1, double long1, double lat2, double long2)
        {
            var R = 6371;
            var lat = lat1;
            var lng = long1;
            var mlat = lat2;
            var mlng = long2;
            var dLat = (mlat - lat) * Math.PI / 180;
            var dLong = (mlng - lng) * Math.PI / 180;
            var a = Math.Sin((double)(dLat / 2)) * Math.Sin((double)(dLat / 2)) +
                Math.Cos((double)((lat) * Math.PI / 180)) * Math.Cos((double)((lat) * Math.PI / 180)) * Math.Sin((double)(dLong / 2)) * Math.Sin((double)(dLong / 2));
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;

            return d;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static IEnumerable<string> GetCustomErrorFromModelValidation(
            ViewDataDictionary ViewData)
        {
            //Đây là những error mà không đi kèm lúc validate theo model
            //- CustomError: những error phát sinh khi xử lý
            //- Success: câu thông báo thành công khi xử lý


            //var CustomErrorToken = "CustomError";
            //return ViewData.ModelState.Values.SelectMany(m => m.Errors)
            //    .Where(e => e.ErrorMessage.StartsWith(CustomErrorToken))
            //    .Select(e => e.ErrorMessage.Replace(CustomErrorToken, ""));
            //return hasCustomError.HasValue && hasCustomError.Value ?
            //    ViewData.ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage) :
            //    new List<string>();
            //return ViewData.ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage);
            return ViewData.ModelState.Where(m => m.Key == "CustomError" || m.Key == "Success" || string.IsNullOrEmpty(m.Key)).SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage);
        }

        public static int GetQuarter(DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 3)
                return 1;
            else if (date.Month >= 4 && date.Month <= 6)
                return 2;
            else if (date.Month >= 7 && date.Month <= 9)
                return 3;
            else
                return 4;

        }

        public static string GetQuaterText(DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 3)
                return "I/" + date.Year.ToString();
            else if (date.Month >= 4 && date.Month <= 6)
                return "II/" + date.Year.ToString();
            else if (date.Month >= 7 && date.Month <= 9)
                return "III/" + date.Year.ToString();
            else
                return "IV/" + date.Year.ToString();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        //IMAGES

        #region IMAGES
        public static Image GetThumbnailImage(Image img, int new_width, int new_height = 0)
        {
            int height = img.Height * new_width / img.Width;

            if (new_height > 0)
                height = new_height;

            return img.ResizeCropExcess(new_width, height);
        }

        public static string MyUrlContent_AccountAvatar(string path = "")
        {
            return UrlContent_GetImage(path, AppSettings.DefaultAccountAvatar);
        }

        public static string MyUrlContent_AccountCover(string path = "")
        {
            return UrlContent_GetImage(path, AppSettings.DefaultAccountCover);
        }

        public static string MyUrlContent_RoomImage(string path = "")
        {
            return UrlContent_GetImage(path, AppSettings.DefaultRoomImage);
        }

        public static string MyUrlContent_DefaultIcon(string path = "")
        {
            return UrlContent_GetImage(path, AppSettings.DefaultIcon);
        }

        public static string MyUrlContent_DefaultImage(string path = "")
        {
            return UrlContent_GetImage(path, AppSettings.DefaultNoImage);
        }

        public static string MyUrlContent_DefaultDomainImage(string path = "")
        {
            return "http://homely.vn" + UrlContent_GetImage(path, AppSettings.DefaultNoImage);
        }

        private static string UrlContent_GetImage(string path, string default_image)
        {
            //1. Gốc
            return string.IsNullOrEmpty(path) ?
                VirtualPathUtility.ToAbsolute(default_image) :
                VirtualPathUtility.ToAbsolute(path);

            //2. Test
            //return string.IsNullOrEmpty(path) ?
            //    VirtualPathUtility.ToAbsolute(default_image) :
            //    "http://homely.vn" + VirtualPathUtility.ToAbsolute(path);

            //3. Của Tài
            //return string.IsNullOrEmpty(path) ?
            //    VirtualPathUtility.ToAbsolute(default_image) :
            //    HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute(path);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        //FILE UPLOAD

        #region FILE UPLOAD
        public static string SaveAs(string image_path, HttpPostedFileBase uploadFile)
        {
            var id = Guid.NewGuid().ToString();

            var StorageRoot = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(image_path));

            var ext = Path.GetExtension(uploadFile.FileName);
            //var fullPath = StorageRoot + id + "" + ext;
            var fullName = image_path + id + "" + ext;
            var fullPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(fullName));

            uploadFile.SaveAs(fullPath);

            return fullName;
        }

        public static string SaveAs(string image_path, HttpPostedFile uploadFile)
        {
            var id = Guid.NewGuid().ToString();

            var StorageRoot = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(image_path));

            var ext = Path.GetExtension(uploadFile.FileName);
            //var fullPath = StorageRoot + id + "" + ext;
            var fullName = image_path + id + "" + ext;
            var fullPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(fullName));

            uploadFile.SaveAs(fullPath);

            return fullName;
        }

        public static string SaveToThumbnailImage(string image_path, string fullName, int width = 165, int height = 95)
        {
            var fullPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(fullName));
            var smallName = "";
            if (System.IO.File.Exists(fullPath))
            {
                var id = Guid.NewGuid().ToString();
                Image org_img = Image.FromFile(fullPath);
                Image newimg = null;

                var small_width = width;
                var small_height = height;

                newimg = Helper.GetThumbnailImage(org_img, small_width, small_height);

                smallName = image_path + id + "_small.jpg";
                var smallPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(smallName));

                newimg.Save(smallPath, ImageFormat.Jpeg);
            }

            return smallName;
        }

        #endregion


        #region ENUM
        public static string GetStudentStatusColor(StudentAcademicStatusEnum status)
        {
            switch (status)
            {
                case StudentAcademicStatusEnum.Studying:
                    {
                        return "green";
                    }
                case StudentAcademicStatusEnum.Delay:
                    {
                        return "yellow";
                    }
                case StudentAcademicStatusEnum.Dropout:
                    {
                        return "red";
                    }
                case StudentAcademicStatusEnum.Note:
                    {
                        return "white";
                    }
                case StudentAcademicStatusEnum.Transfer:
                    {
                        return "black";
                    }
                case StudentAcademicStatusEnum.Upgrade:
                    {
                        return "purple";
                    }
                case StudentAcademicStatusEnum.Finished:
                    {
                        return "blue";
                    }
            }

            return "gray";
        }

        public static string GetInstallmentStatusColor(InstallmentStatusEnum status)
        {
            switch (status)
            {
                case InstallmentStatusEnum.Planned:
                    {
                        return "primary";
                    }
                case InstallmentStatusEnum.Finished:
                    {
                        return "green";
                    }
                case InstallmentStatusEnum.ExtendWait:
                    {
                        return "red";
                    }
                case InstallmentStatusEnum.ExtendOK:
                    {
                        return "orange";
                    }
            }

            return "";
        }

        public static string GetInstallmentStatus(InstallmentStatusEnum status)
        {
            switch (status)
            {
                case InstallmentStatusEnum.Planned:
                    {
                        return "CHƯA ĐÓNG";
                    }
                case InstallmentStatusEnum.Finished:
                    {
                        return "ĐÃ ĐÓNG";
                    }
                case InstallmentStatusEnum.ExtendWait:
                    {
                        return "XIN GIA HẠN";
                    }
                case InstallmentStatusEnum.ExtendOK:
                    {
                        return "ĐƯỢC GIA HẠN";
                    }
            }

            return "";
        }

        public static string GetFacultyTypeName(FacultyTypeEnum type)
        {
            switch (type)
            {
                case FacultyTypeEnum.FullTime:
                    {
                        return "FullTime";
                    }
                case FacultyTypeEnum.PartTime:
                    {
                        return "PartTime";
                    }
            }

            return "";
        }

        public static string GetGenderText(GenderEnum type)
        {
            switch (type)
            {
                case GenderEnum.Male:
                    {
                        return "Nam";
                    }
                case GenderEnum.Female:
                    {
                        return "Nữ";
                    }
            }

            return "";
        }
        #endregion

        //public static string ReturnPartialView(string action)
        //{
        //    switch (action)
        //    {
        //        case "ManageSchedule_ClassModule_Exam":
        //        case "FilterClass_Module_StudentExam":
        //            {
        //                return "_partial_Search_Class_Module_Exam";
        //            }
        //        case "ManageSchedule_ClassModule_StudentAttendance":
        //            {
        //                return "_partial_Search_Class_Module_Day_Student";
        //            }
        //        case "ManageSchedule_ExamPlan":
        //        case "FilterExamPlan":
        //            {
        //                return "_partial_Search_Class_Module_ExamPlan";
        //            }
        //        case "ManageAcademic_Student_Attendance":
        //            {
        //                return "_partial_Search_Student_Attendance";
        //            }
        //        case "ManageFeePlan_Installment":
        //            {
        //                return "_partial_Search_Installment";
        //            }
        //    }

        //    return "";
        //}

        public static float GetRating(float mark, float max_mark)
        {
            return (float)Math.Round(max_mark > 0 ? mark * 100 / max_mark : 0);
        }

        public static string GetMarkRating(float rate)
        {
            if (rate >= 75)
                return "DISTINCTION";
            if (rate >= 60)
                return "CREDIT";
            if (rate >= 40)
                return "PASS";
            return "FAIL";
        }

        public static IEnumerable<Student_MarkModel> GetStudentSemesterMarks(Student student, int sem = 0)
        {
            var list = new List<Student_MarkModel>();

            //var course = student.Class.CourseFamily;
            IEnumerable<Module> modules = null;
            if (sem > 0)
            {
                modules = student.Portal_Modules_List;
                //if (modules.Count() == 0)//CIM, DIM
                //{
                //    //bo sung them course cha
                //    var parent_course = IoCConfig.Service<ICourseFamilyService>()
                //        .FindValidByCriteria(c => 
                //            c.CourseFamily_Name == student.CourseFamily.CourseFamily_Name &&
                //            c.Modules_List.Count() > 0);
                //    modules = parent_course.Portal_Modules_List;
                //}

                modules = modules.Where(m => m.Semester == sem);
            }
            else
            {
                modules = student.Extra_Modules_List;
            }
            //IEnumerable<Module> modules = sem > 0 ? student.Portal_Modules_List.Where(m => m.Semester == sem) : student.Extra_Modules_List;

            foreach (var module in modules)
            {
                var student_module = student.Exam_Modules_List.FirstOrDefault(m => m.Class_Module.Module_Id == module.Id);
                if (module.Module_Type < (int)ModuleTypeEnum.LT_TH)
                {
                    var max_mark = module.Module_Type == (int)ModuleTypeEnum.LT ? module.Module_Max_LT : module.Module_Max_TH;
                    //var module_mark = student.Modules_List.FirstOrDefault(m => m.Class_Module.Module_Id == module.Id);
                    var mark = student_module != null ? (module.Module_Type == (int)ModuleTypeEnum.LT ? student_module.Mark_LT : student_module.Mark_TH) : 0;
                    var rate = Helper.GetRating(mark, max_mark);

                    list.Add(new Student_MarkModel
                    {
                        Module_Name = module.Module_Name,
                        Module_Type = (ModuleTypeEnum)module.Module_Type,//((ModuleTypeEnum)module.Module_Type).ToString(),
                        Mark = mark,
                        Max_Mark = max_mark,
                        Rate = rate,
                        Rate_Status = Helper.GetMarkRating(rate)
                    });
                }
                else
                {
                    //var module_mark = student.Modules_List.FirstOrDefault(m => m.Class_Module.Module_Id == module.Id);
                    var mark_LT = 0.0f;
                    var mark_TH = 0.0f;
                    var rate_LT = 0.0f;
                    var rate_TH = 0.0f;
                    if (student_module != null)
                    {
                        mark_LT = student_module.Mark_LT;
                        mark_TH = student_module.Mark_TH;
                        rate_LT = Helper.GetRating(mark_LT, module.Module_Max_LT);//module.Module_Max_LT > 0 ? mark_LT * 100 / module.Module_Max_LT : 0;
                        rate_TH = Helper.GetRating(mark_TH, module.Module_Max_TH);//module.Module_Max_TH > 0 ? mark_TH * 100 / module.Module_Max_TH : 0;
                    }
                    list.Add(new Student_MarkModel
                    {
                        Module_Name = module.Module_Name_Code,
                        Module_Type = ModuleTypeEnum.LT,
                        Mark = mark_LT,
                        Max_Mark = module.Module_Max_LT,
                        Rate = rate_LT,
                        Rate_Status = Helper.GetMarkRating(rate_LT)
                    });
                    list.Add(new Student_MarkModel
                    {
                        Module_Name = module.Module_Name_Code,
                        Module_Type = ModuleTypeEnum.TH,//"TH",
                        Mark = mark_TH,
                        Max_Mark = module.Module_Max_TH,
                        Rate = rate_TH,
                        Rate_Status = Helper.GetMarkRating(rate_TH)
                    });
                }
            }

            if (list.Count > 0)
            {
                //Overall Weighted Mark
                float IM = 0;
                var IM_Count = 0;
                float EM = 0;
                var EM_Count = 0;
                float Proj = 0;
                foreach (var mark in list)
                {
                    if (!mark.Module_Name.ToLower().Contains("project"))
                    {
                        if (mark.Module_Type == ModuleTypeEnum.TH)
                        {
                            IM += mark.Rate;
                            IM_Count++;
                        }
                        else
                        {
                            EM += mark.Rate;
                            EM_Count++;
                        }
                    }
                    else
                    {
                        Proj = mark.Rate;
                    }
                }
                IM = IM / IM_Count;
                EM = EM / EM_Count;
                int Overall = (int)((25 * IM + 50 * EM + 25 * Proj) / 100);
                list.Add(new Student_MarkModel
                {
                    Module_Name = "Overall Weighted Marks",
                    Module_Type = ModuleTypeEnum.LT_TH,
                    Mark = Overall,
                    Max_Mark = 100,
                    Rate = Overall,
                    Rate_Status = Helper.GetMarkRating(Overall)
                });
            }

            foreach (var mark in list)
            {
                mark.Mark_Name = mark.Module_Type == ModuleTypeEnum.LT_TH ? mark.Module_Name : mark.Module_Name + " (" + ((ModuleTypeEnum)mark.Module_Type).ToString() + ")";
            }

            return list;
        }

        
    }

    public static class NumberHelper
    {
        public static string ToMoneyString(this long money)
        {
            return money.ToString("##,#0");
        }
    }
}