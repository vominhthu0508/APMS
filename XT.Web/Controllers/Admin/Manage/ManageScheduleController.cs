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
using OfficeOpenXml;

namespace XT.Web.Controllers
{
    [XTAuthorizeAcademic]
    public partial class ManageScheduleController : AdminBaseController
    {
        #region [XTAuthorizeAcademicExecute] Class_Module (Quan ly lop hoc ~ giong cach lam cua ManageStudent)
        private IEnumerable<Class_Module> GetClassModules(ref Class currentParent, 
            DateTime? start_date, DateTime? end_date, string class_name = "")
        {
            IEnumerable<Class_Module> list = null;
            if (class_name != "")
            {
                currentParent = IoCConfig.Service<IClassService>().FindByClassName(class_name);
                if (currentParent != null && currentParent.IsValid())
                {
                    list = currentParent.Class_Modules;
                }
            }
            else
            {
                list = IoCConfig.Service<IClass_ModuleService>().FindAllValid();
            }

            var start = DateTime.MinValue;
            var end = DateTime.MaxValue;
            if (start_date.HasValue)
                start = start_date.Value;
            if (end_date.HasValue && start < end_date.Value)
                end = end_date.Value;

            return list.Valid().Where(c => start <= c.Class_Module_Date_Start && c.Class_Module_Date_Start <= end)
                .OrderByDescending(c => c.Class_Module_Date_Start);
        }

        public ActionResult ManageSchedule_ClassModule(int? page, DateTime? start_date, DateTime? end_date, string class_id = "")
        {
            var currentParent = new Class { Status = (int)EntityStatus.Visible };
            var list = GetClassModules(ref currentParent, start_date, end_date, class_id);
            if (currentParent == null || !currentParent.IsValid())
                return RedirectToError("Class is not valid!");

            //update FC
            //foreach (var item in list)
            //{
            //    if (item.Faculty_Id == 1)
            //    {
            //        item.Faculty_Id = 2;
            //        var fcs = item.Module.Faculty_List;
            //        if (fcs.Count() > 0)
            //            item.Faculty_Id = fcs.First().Id;

            //        IoCConfig.Service<IClass_ModuleService>().Update(item);
            //    }
            //}

            return ManageModel(new Class_Module
            {
                Class_Module_Date_Start = DateTime.Today,
                Class_Module_Date_End = DateTime.Today,
                Class_Module_Date_Exam = DateTime.Today,
                Class_Module_Hour_Start = currentParent.Class_Hour_Start,
                Class_Module_Hour_End = currentParent.Class_Hour_End,
                Class_Id = currentParent.Id
            }, page, list: list,
            currentParentId: currentParent.Id, currentParentName: currentParent.Class_Name,
            filterSearch: SearchModelEnum.ByOthers,
            entityName: "Class Schedule",
            breadcrumbpartial_name: "_partial_BreadCrumb_Class_Module",
            script: "~/Scripts/Admin/ManageSchedule_ClassModule");
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddClass_Module(Class_ModuleModel model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditClass_Module(Class_Module model)
        {
            if (model.Class_Id == 0)
                model.Class_Id = null;
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteClass_Module(int id)
        {
            return DeleteModel(id);
        }

        [HttpPost]
        public ActionResult FilterClass_Module(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            DateTime? Start_Date, DateTime? End_Date,
            string class_id = "",
            int Class_Module_Status = 0,
            int Semester = 0,
            //int Module_Id = 0,
            int Id_Class = 0,
            int Company_Id = 0,
            int Class_Module_Day = 0,
            int Faculty_Id = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var currentClass = new Class { Status = (int)EntityStatus.Visible };
            var items = GetClassModules(ref currentClass, Start_Date, End_Date, class_id);
            if (currentClass == null || !currentClass.IsValid())
                return RedirectToError("Class is not valid!");

            //var items = IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (Model_Name != "")
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();
                items = items.Where(c => c.Module.Module_Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }
            if (Class_Module_Status != 0)
            {
                items = items.Where(d => d.Class_Module_Status == Class_Module_Status);
            }
            if (Semester != 0)
            {
                items = items.Where(d => d.Module.Semester == Semester);
            }
            //if (Module_Id != 0)
            //{
            //    items = items.Where(d => d.Module_Id == Module_Id);
            //}
            if (Id_Class != 0)
            {
                items = items.Where(d => d.Class_Id == Id_Class);
            }
            if (Company_Id != 0)//lop review khong search duoc vi class == null hoac dung current company from session
            {
                items = items.Where(d => d.Class != null && d.Class.Company_Id == Company_Id);
            }
            if (Class_Module_Day != 0)
            {
                items = items.Where(d => d.Class_Module_Day == Class_Module_Day);
            }
            if (Faculty_Id != 0)
            {
                items = items.Where(d => d.Faculty_Id == Faculty_Id);
            }

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }

        [HttpPost]
        public ActionResult GetModulesByCourseFamily(int id)
        {
            var course = IoCConfig.Service<ICourseFamilyService>().FindById(id);
            if (IsValidModel(course))
            {
                var modules = new List<object>();
                modules.Add(new { Id = 0, Name = "Choose Module" });
                foreach (var f in course.Modules_List)
                {
                    modules.Add(new { Id = f.Id, Name = f.Module_Name, Target_Value = f.Module_DurationByDay });
                }
                return Json(modules);
            }

            return null;
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult Generate_ClassModule(int Id, int Semester, DateTime Class_Module_Date_Start)
        {
            var current_class = IoCConfig.Service<IClassService>().FindById(Id);
            if (!IsValidModel(current_class))
                return Error("Class is not valid!");

            var modules = current_class.Portal_Modules_List.Where(m => m.Semester == Semester);
            var fcs = current_class.Company.Faculties.Valid();
            var res = current_class.Company.Resources.Valid().FirstOrDefault();

            var start_date = Class_Module_Date_Start;
            foreach (var module in modules)
            {
                var fc = fcs.Where(f => f.HasModule(module)).FirstOrDefault();
                if (fc != null && res != null)
                {
                    var class_module = new Class_Module
                    {
                        Module_Id = module.Id,
                        Faculty_Id = fc.Id,
                        Resource_LT_Id = res.Id,
                        Resource_TH_Id = res.Id,
                        Resource_Exam_Id = res.Id,
                        Class_Module_Day = current_class.Class_Day,
                        Class_Module_Date_Start = start_date,
                        Class_Module_DurationByDay = module.Module_DurationByDay,
                        Class_Module_Hour_Start = current_class.Class_Hour_Start,
                        Class_Module_Hour_End = current_class.Class_Hour_End,
                        Class_Module_Status = (int)ClassModuleStatusEnum.Scheduled,

                        Created_Date = DateTime.Now,
                        Status = (int)EntityStatus.Visible,
                    };

                    var end_date = class_module.GetClassModuleEndDate();
                    if (end_date == null)
                    {
                        end_date = start_date;
                    }
                    var exam_date = class_module.GetNextDateByCurrentDate(end_date);
                    class_module.Class_Module_Date_End = end_date;
                    class_module.Class_Module_Date_Exam = exam_date;

                    current_class.Class_Modules.Add(class_module);

                    start_date = class_module.GetNextDateByCurrentDate(exam_date);
                }
            }

            IoCConfig.Service<IClassService>().Update(current_class);

            return Success();
        }

        #region Find Class Module (Ajax: Calendar)
       
        private object GetClassModuleInfo(Class_Module s)
        {
            var start = s.Class_Module_Date_Start;
            var end = s.Class_Module_Date_End;
            return new
            {
                id = s.Id,
                resourceId = s.Resource_LT_Id,
                start_day = start.Day,
                start_month = start.Month,
                start_year = start.Year,
                start_hour = s.Class_Module_Hour_Start,
            };
        }

        [HttpPost]
        public ActionResult FindClassModule_Ajax(DateTime? Start_Date, DateTime? End_Date)
        {
            var currentClass = new Class { Status = (int)EntityStatus.Visible };
            var items = GetClassModules(ref currentClass, Start_Date, End_Date, "");
            if (currentClass == null || !currentClass.IsValid())
                return RedirectToError("Class is not valid!");

            return Json(items.Select(s => GetClassModuleInfo(s)));
        }
        #endregion Find Student (Ajax: in select2)

        #region [XTAuthorizeMod] ImportClassModule

        private enum TKBEnum
        { 
            Class = 1,
            Day = 2,
            Time = 3,
            Date = 4,
            Module = 5,
            FC = 6,
            Resource = 7,
            Course = 8
        }

        private Class_Module GetClassModuleDay(ExcelWorksheet workSheet, int rowIterator, 
            Company center, Class current_class, CourseFamily ngoaikhoa)
        {
            Class_Module item = new Class_Module();
            var Class_Module_Date_Start = GetCellValue_DateTime(workSheet, rowIterator, (int)TKBEnum.Date);
            if (Class_Module_Date_Start == null)
                return null;
            item.Class_Module_Date_Start = Class_Module_Date_Start.Value;

            var module = GetCellValue(workSheet, rowIterator, (int)TKBEnum.Module);
            if (string.IsNullOrEmpty(module))
            {
                return null;
            }
            module = module.ToLower();
            var module_names = module.Split("-".ToCharArray());
            if (module_names.Length != 2)
            {
                return null;
            }
            var module_code = module_names[0].Trim();
            var module_day = module_names[1].Trim();
            if (string.IsNullOrEmpty(module_day))
            {
                return null;
            }

            //1. Module
            var _module = current_class.CourseFamily.Modules_List
                .FirstOrDefault(m => m.Module_Code != null && m.Module_Code.ToLower().Equals(module_code));//IoCConfig.Service<IModuleService>().FindValidByCriteria(m => m.Module_Code != null && m.Module_Code.ToLower().Equals(module_code));
            if (!IsValidModel(_module) && ngoaikhoa != null)
            {
                _module = ngoaikhoa.Modules_List
                    .FirstOrDefault(m => m.Module_Code != null && m.Module_Code.ToLower().Equals(module_code));
            }

            if (!IsValidModel(_module))
            {
                return null;
            }
            item.Module_Id = _module.Id;
            
            if (module_day != "test")
            {
                item.Class_Module_Day = int.Parse(module_day);
                item.Class_Module_Status = (int)ClassModuleDayStatusEnum.Studying;
            }
            else
            {
                item.Class_Module_Day = 0;
                item.Class_Module_Status = (int)ClassModuleDayStatusEnum.Test;
            }

            //2. FC
            item.Faculty_Id = 0;
            var FC_Nickname = GetCellValue(workSheet, rowIterator, (int)TKBEnum.FC);
            if (!string.IsNullOrEmpty(FC_Nickname))
            {
                var _fc = IoCConfig.Service<IFacultyService>().FindValidByCriteria(f => 
                    f.Company_Id == center.Id &&
                    f.FC_Nickname != null && f.FC_Nickname.ToLower().Equals(FC_Nickname.ToLower()));
                    //center.Faculties.Valid().FirstOrDefault(f => f.FC_Nickname != null && f.FC_Nickname.ToLower().Equals(FC_Nickname.ToLower()));
                if (!IsValidModel(_fc))
                {
                    _fc = IoCConfig.Service<IFacultyService>().Add(new Faculty
                    {
                        Company_Id = center.Id,
                        FC_Name = FC_Nickname,
                        FC_Nickname = FC_Nickname,
                        FC_Type = (int)FacultyTypeEnum.PartTime,
                        FC_Gender = (int)GenderEnum.Male,
                        FC_Birthday = DateTime.Today,
                        FC_Salary = 0,
                        FC_WorkingHour = 0,
                        FC_CMND_NgayCap = DateTime.Today,

                        Status = (int)EntityStatus.Visible
                    });
                }
                item.Faculty_Id = _fc.Id;
            }

            //3. Resource
            var Resource_Name = GetCellValue(workSheet, rowIterator, (int)TKBEnum.Resource);
            if (string.IsNullOrEmpty(Resource_Name))
            {
                return null;
            }
            var _resource = IoCConfig.Service<IResourceService>().FindValidByCriteria(r => 
                    r.Company_Id == center.Id &&
                    r.Resource_Name != null && r.Resource_Name.ToLower().Equals(Resource_Name.ToLower()));
                //center.Resources.Valid().FirstOrDefault(r => r.Resource_Name != null && r.Resource_Name.ToLower().Equals(Resource_Name.ToLower()));
            if (!IsValidModel(_resource))
            {
                _resource = IoCConfig.Service<IResourceService>().Add(new Resource
                {
                    Company_Id = center.Id,
                    Resource_Name = Resource_Name,

                    Status = (int)EntityStatus.Visible
                });
            }
            item.Resource_LT_Id = _resource.Id;
            item.Resource_TH_Id = _resource.Id;
            item.Resource_Exam_Id = _resource.Id;

            return item;
        }

        [HttpPost]
        [XTAuthorizeMod]
        public ActionResult ImportClassModule(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                //////////////////////////////////////////////////////
                //////////////////////////////////////////////////////
                //Constant Variables for importing
                //var CURRENT_COMPANY = 3;
                var Course_NgoaiKhoa_Id = 8;
                //////////////////////////////////////////////////////
                //////////////////////////////////////////////////////

                var center = IoCConfig.Service<ICompanyService>().FindById(CURRENT_COMPANY);
                CourseFamily ngoaikhoa = IoCConfig.Service<ICourseFamilyService>().FindById(Course_NgoaiKhoa_Id);
                var reserved_fc = center.Faculties.Valid().First();

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var sheets = package.Workbook.Worksheets;
                    foreach (var workSheet in sheets)
                    {
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        //Current Class
                        //AMMHCM
                        //var class_name = workSheet.Name.Trim();
                        //var class_name = GetCellValue(workSheet, rowIterator, (int)TKBEnum.Class);
                        //var current_class = center.Classes.Valid().FirstOrDefault(c => c.Class_Name.ToLower().Equals(class_name.ToLower()));//IoCConfig.Service<IClassService>().FindValidByCriteria(c => c.Class_Name.ToLower().Equals(class_name.ToLower()));

                        //if (IsValidModel(current_class))
                        if (true)
                        {
                            //Class_Module
                            Class_Module class_module = null;//current class_module
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                try
                                {
                                    var class_name = GetCellValue(workSheet, rowIterator, (int)TKBEnum.Class);
                                    var current_class = center.Classes.Valid().FirstOrDefault(c => c.Class_Name.ToLower().Equals(class_name.ToLower()));
                                    if (current_class == null)
                                    {
                                        continue;
                                    }

                                    var class_module_day = GetClassModuleDay(workSheet, rowIterator, 
                                        center, current_class, ngoaikhoa);
                                    if (class_module_day == null)
                                    {
                                        continue;
                                    }

                                    //Check existed
                                    var existed = IoCConfig.Service<IClass_ModuleService>()
                                                .FindValidByCriteria(c => c.Class_Id == current_class.Id 
                                                    && c.Module_Id == class_module_day.Module_Id);
                                    if (existed == null)
                                    {
                                        if (class_module_day.Faculty_Id == 0)
                                        {
                                            class_module_day.Faculty_Id = reserved_fc.Id;
                                        }
                                        class_module = new Class_Module
                                        {
                                            Class_Id = current_class.Id,
                                            Class_Module_Day = current_class.Class_Day,
                                            Class_Module_Hour_Start = current_class.Class_Hour_Start,
                                            Class_Module_Hour_End = current_class.Class_Hour_End,
                                            Class_Module_Status = (int)ClassModuleStatusEnum.Studying,
                                            Class_Module_Date_Start = class_module_day.Class_Module_Date_Start,
                                            Class_Module_Date_End = class_module_day.Class_Module_Date_Start,
                                            Class_Module_Date_Exam = class_module_day.Class_Module_Date_Start,
                                            Class_Module_DurationByDay = 0,
                                            Faculty_Id = class_module_day.Faculty_Id,
                                            Module_Id = class_module_day.Module_Id,
                                            Resource_LT_Id = class_module_day.Resource_LT_Id,
                                            Resource_TH_Id = class_module_day.Resource_TH_Id,
                                            Resource_Exam_Id = class_module_day.Resource_Exam_Id,

                                            Created_Date = DateTime.Now,
                                            Status = (int)EntityStatus.Visible,
                                        };

                                        class_module.AddStudents(current_class);
                                        class_module = IoCConfig.Service<IClass_ModuleService>().Add(class_module);
                                    }
                                    else
                                    {
                                        class_module = existed;

                                        if (class_module_day.Faculty_Id != 0)
                                        {
                                            class_module.Faculty_Id = class_module_day.Faculty_Id;
                                            class_module.Resource_LT_Id = class_module_day.Resource_LT_Id;
                                            class_module.Resource_TH_Id = class_module_day.Resource_TH_Id;
                                            class_module.Resource_Exam_Id = class_module_day.Resource_Exam_Id;
                                        }
                                        else
                                        {
                                            class_module.Resource_Exam_Id = class_module_day.Resource_Exam_Id;
                                        }

                                        class_module = IoCConfig.Service<IClass_ModuleService>().Update(class_module);
                                    }

                                    //If first day
                                    if (class_module_day.Class_Module_Status == (int)ClassModuleDayStatusEnum.Studying)
                                    {
                                        var day_count = class_module_day.Class_Module_Day;
                                        if (class_module.Class_Module_Days_List.Count() >= day_count)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (class_module.Class_Module_Days_List.Any(d => d.Class_Module_Day_Status == (int)ClassModuleDayStatusEnum.Test))
                                        {
                                            continue;
                                        }
                                    }

                                    //Add new class_module_day
                                    if (class_module != null)
                                    {
                                        var date = class_module_day.Class_Module_Date_Start;
                                        var status = (ClassModuleDayStatusEnum)class_module_day.Class_Module_Status;
                                        class_module.AddModuleDay(date, current_class, status);

                                        class_module.Class_Module_Date_End = date;
                                        class_module.Class_Module_Date_Exam = date;
                                        class_module.Class_Module_DurationByDay++;

                                        IoCConfig.Service<IClass_ModuleService>().Update(class_module);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }

                    return MyContent("Upload successfully!");
                }
            }

            return Error();
        }
        #endregion

        #endregion Class_Module

        #region [XTAuthorizeAcademicExecute] Class_Module_StudentExam (Quan ly danh sach SV cua lop)

        /// <summary>
        /// Parent Text (currentParentName)
        /// </summary>
        /// <param name="current_classmodule"></param>
        /// <returns></returns>
        private string GetClassModuleSmallText(Class_Module current_classmodule)
        {
            return current_classmodule.Class.Class_Name + " - " + current_classmodule.Module.Module_Name +
                                    " (" + current_classmodule.Class_Module_Date_Start.ToDateString() + " - " + current_classmodule.Class_Module_Date_End.ToDateString() + ") " +
                                    " - FC " + current_classmodule.Faculty.FC_Name;
        }


        private IEnumerable<Class_Module_StudentExam> GetClassModuleStudentExam(Class_Module current_classmodule)
        {
            return current_classmodule.Class_Module_StudentExams.Valid().OrderBy(s => s.Student_Status);
        }

        //ManageSchedule_ClassSession_Students
        public ActionResult ManageSchedule_ClassModule_Students(int? page, int id)
        {
            var current_classmodule = IoCConfig.Service<IClass_ModuleService>().FindById(id);
            if (current_classmodule == null || !current_classmodule.IsValid())
                return RedirectToError();

            return ManageModel(
                new Class_Module_StudentExam { 
                    Class_Module_Id = id,
                    Student_Status = (int)StudentClassModuleStatusEnum.Guest,
                    Exam_Count = 1,
                    Status = (int)EntityStatus.Visible,
                }, page,
                list: GetClassModuleStudentExam(current_classmodule),
                entityName: "Student",
                currentParentName: GetClassModuleSmallText(current_classmodule),
                currentParentId: id,
                filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddClass_Module_StudentExam(Class_Module_StudentExam model)
        {
            var existed = IoCConfig.Service<IClass_Module_StudentExamService>()
                .FindAllValidByCriteria(e => e.Class_Module_Id == model.Class_Module_Id && e.Student_Id == model.Student_Id).FirstOrDefault();
            if (existed != null)
            {
                return Error("Đã tồn tại sinh viên học lớp này rồi!");
            }
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteClass_Module_StudentExam(int id)
        {
            return DeleteModel(id);
        }

        #endregion

        #region [XTAuthorizeAcademicExecute] Class_Module_StudentExam (Quan ly điểm thi)
        //ManageSchedule_ClassSession_Exam
        public ActionResult ManageSchedule_ClassModule_Exam(int? page, int id)
        {
            var current_classmodule = IoCConfig.Service<IClass_ModuleService>().FindById(id);
            if (current_classmodule == null || !current_classmodule.IsValid())
                return RedirectToError();

            var currentParentName = GetClassModuleSmallText(current_classmodule);

            return ManageModel(
                new Class_Module_StudentExam
                {
                    Class_Module_Id = id,
                    Student_Status = (int)StudentClassModuleStatusEnum.Guest,
                    Exam_Count = 1,
                    Status = (int)EntityStatus.Visible,
                }, page,
                list: GetClassModuleStudentExam(current_classmodule),
                canAdd: false,
                entityName: "Student Exam",
                entityFilter: "Class_Module_Exam",
                currentParentId: current_classmodule.Id, currentParentName: currentParentName,
                //filterSearch: SearchModelEnum.ByOthers,
                filterpartial_name: "_partial_Filter_Class_Module_StudentExam");
        }

        [HttpPost]
        public ActionResult FilterClass_Module_Exam(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,
            string entityFilter,

            int class_id = 0,
            int ExamStatus = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var current_classmodule = IoCConfig.Service<IClass_ModuleService>().FindById(class_id);
            if (current_classmodule == null || !current_classmodule.IsValid())
                return Error();

            var items = GetClassModuleStudentExam(current_classmodule);

            //var items = IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (ExamStatus != 0)
            {
                var IsExamValid = ExamStatus == (int)ClassModuleStudentExamEnum.Valid;
                items = items.Where(d => d.IsExamValid == IsExamValid);
            }

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize, entityFilter);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditClass_Module_StudentExam(Class_Module_StudentExam model)
        {
            return EditModel(model);
        }

        #endregion

        #region [XTAuthorizeAcademicExecute] Class_Module_Day (Quan ly ngày học)
        //ManageSchedule_ClassSession_Attendance
        public ActionResult ManageSchedule_ClassModule_Attendance(int? page, int id)
        {
            var current_classmodule = IoCConfig.Service<IClass_ModuleService>().FindById(id);
            if (current_classmodule == null || !current_classmodule.IsValid())
                return RedirectToError();

            var currentParentName = GetClassModuleSmallText(current_classmodule);

            return ManageModel(
                new Class_Module_Day
                {
                    Class_Module_Id = id,
                    Class_Module_Day_Date = DateTime.Today,
                    Class_Module_Day_Status = (int)ClassModuleDayStatusEnum.Scheduled,
                    Status = (int)EntityStatus.Visible,
                }, page,
                list: current_classmodule.Class_Module_Days.OrderBy(d => d.Class_Module_Day_Date).Valid(),
                entityName: "Class Day", 
                currentParentModel: current_classmodule,
                currentParentId: current_classmodule.Id, currentParentName: currentParentName,
                breadcrumbpartial_name: "_partial_BreadCrumb_Class_Module_Day",
                filterSearch: SearchModelEnum.None);
        }

        [XTAuthorizeAcademicExecute]
        public ActionResult Generate_ClassModule_StudentAttendance(int id)
        {
            var current_classmodule = IoCConfig.Service<IClass_ModuleService>().FindById(id);
            if (!IsValidModel(current_classmodule))
                return RedirectToError();

            if (current_classmodule.Class_Module_Days_List.Count() == 0)
            {
                current_classmodule.AddModuleDays(ClassModuleDayStatusEnum.Scheduled);
                IoCConfig.Service<IClass_ModuleService>().Update(current_classmodule);
            }

            return RedirectToAction("ManageSchedule_ClassModule_Attendance", new { id = id });
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddClass_Module_Day(Class_Module_Day model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditClass_Module_Day(Class_Module_Day model)
        {
            var old_iteam = IoCConfig.Service<IClass_Module_DayService>().FindById(model.Id);
            var new_item = model;
            var class_module = old_iteam.Class_Module;
            var curr_date = new_item.Class_Module_Day_Date;

            foreach (var day in class_module.Class_Module_Days_List)
            {
                if (day.Class_Module_Day_Date > old_iteam.Class_Module_Day_Date)
                {
                    day.Class_Module_Day_Date = class_module.GetNextDateByCurrentDate(curr_date);
                    IoCConfig.Service<IClass_Module_DayService>().Update(day);

                    curr_date = day.Class_Module_Day_Date;
                }
            }

            model.Class_Module_Day_Date = class_module.GetExactStartDateByClassDay(model.Class_Module_Day_Date, (ClassDayEnum)class_module.Class_Module_Day);

            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteClass_Module_Day(int id)
        {
            var day = IoCConfig.Service<IClass_Module_DayService>().FindById(id);
            if (day == null || !day.IsValid())
                return Error("Không tồn tại ngày học này");
            if (day.Class_Module_Day_Date <= DateTime.Today)
                return Error("Không thể xóa ngày học đã diễn ra");

            return DeleteModel(id);
        }

        #endregion

        #region Class_Module_Day_Student (Quan ly điểm danh của HV)
        private IEnumerable<Class_Module_StudentExam> GetStudentExams(
            ref Class_Module current_classmodule,
            int id = 0, int day_id = 0, int student_id = 0)
        {
            if (day_id <= 0 && id <= 0)
                return null;

            if (day_id > 0)
            {
                var day = IoCConfig.Service<IClass_Module_DayService>().FindById(day_id);
                if (!IsValidModel(day))
                    return null;
                id = day.Class_Module_Id;
                current_classmodule = day.Class_Module;
            }
            else
            {
                current_classmodule = IoCConfig.Service<IClass_ModuleService>().FindById(id);
                if (!IsValidModel(current_classmodule))
                    return null;
            }
            var students = current_classmodule.Class_Module_StudentExams.Valid();
            if (student_id > 0)
                students = students.Where(s => s.Student_Id == student_id);

            return students;
        }

        //ManageSchedule_ClassSession_StudentAttendance
        public ActionResult ManageSchedule_ClassModule_StudentAttendance(int? page, 
            int id = 0, int day_id = 0, int student_id = 0)
        {
            Class_Module current_classmodule = null;
            var students = GetStudentExams(ref current_classmodule, id, day_id, student_id);
            if (current_classmodule == null || students == null)
                return RedirectToError();

            ViewBag.CurrentClassModule = current_classmodule;
            ViewBag.Day = day_id;
            ViewBag.Student = student_id;

            return ManageModel(
                new Class_Module_Day_Student(), page,
                list: students,
                entityName: "Student Attendance",
                entityFilter: "Class_Module_Day_Student",
                currentParentId: current_classmodule.Id, currentParentName: GetClassModuleSmallText(current_classmodule),
                canAdd: false,
                noPaging: true,
                noSearchBox: true,
                filterSearch: SearchModelEnum.ByOthers);
        }

        [HttpPost]
        [XTAuthorizeAcademic]
        public ActionResult AddClass_Module_Day_Student(Class_Module_Day_Student model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademic]
        public ActionResult EditClass_Module_Day_Student(Class_Module_Day_Student model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademic]
        public ActionResult CheckAllAttendanceForDay(int id)
        {
            var day = IoCConfig.Service<IClass_Module_DayService>().FindById(id);
            if (IsValidModel(day))
            {
                foreach (var student in day.Class_Module_Day_Students)
                {
                    //student.Attendance_Status = (int)StudentClassModuleAttendanceEnum.P;
                    //student.Status = (int)EntityStatus.Visible;
                    student.Status = (int)EntityStatus.Invisible;
                }
                foreach (var student in day.Class_Module.Class_Module_StudentExams.Valid().Select(ex => ex.Student))
                {
                    day.Class_Module_Day_Students.Add(new Class_Module_Day_Student
                    {
                        Student_Id = student.Id,
                        Attendance_Status = (int)StudentClassModuleAttendanceEnum.P,
                        Status = (int)EntityStatus.Visible
                    });
                }

                return EditModel(day);
            }

            return Error();
        }

        [HttpPost]
        public ActionResult FilterClass_Module_Day_Student(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int class_module_id = 0,
            int day_id = 0,
            int student_id = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            Class_Module current_classmodule = null;
            var items = GetStudentExams(ref current_classmodule, class_module_id, day_id, student_id);
            if (current_classmodule == null || items == null)
                return Error();

            ViewBag.CurrentClassModule = current_classmodule;
            ViewBag.Day = day_id;
            ViewBag.Student = student_id;

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }

        #endregion

        #region ExamPlan (Quan ly lich thi)

        private IEnumerable<Class_Module> GetClassModulesByExamDate(ref Class currentParent,
            DateTime? start_date, DateTime? end_date, string class_id = "")
        {
            IEnumerable<Class_Module> list = null;
            if (class_id != "")
            {
                currentParent = IoCConfig.Service<IClassService>().FindByClassName(class_id);
                if (currentParent != null && currentParent.IsValid())
                {
                    list = currentParent.Class_Modules;
                }
            }
            else
            {
                list = IoCConfig.Service<IClass_ModuleService>().FindAllValid();
            }

            var start = DateTime.MinValue;
            var end = DateTime.MaxValue;
            if (start_date.HasValue)
                start = start_date.Value;
            if (end_date.HasValue && start <= end_date.Value)
                end = end_date.Value;

            return list.Where(c => start <= c.Class_Module_Date_Exam && c.Class_Module_Date_Exam <= end)
                .OrderByDescending(c => c.Class_Module_Date_Exam);
        }

        public ActionResult ManageSchedule_ExamPlan(int? page, DateTime? start_date, DateTime? end_date, string class_id = "")
        {
            var currentParent = new Class { Status = (int)EntityStatus.Visible };
            var list = GetClassModulesByExamDate(ref currentParent, start_date, end_date, class_id);
            if (currentParent == null || !currentParent.IsValid())
                return RedirectToError("Class is not valid!");

            return ManageModel(new Class_Module
            {
                Class_Module_Date_Start = DateTime.Today,
                Class_Module_Date_End = DateTime.Today,
                Class_Module_Date_Exam = DateTime.Today,
                Class_Module_Hour_Start = currentParent.Class_Hour_Start,
                Class_Module_Hour_End = currentParent.Class_Hour_End,
                Class_Id = currentParent.Id
            }, page, list: list,
            currentParentId: currentParent.Id, currentParentName: currentParent.Class_Name,
            //filterSearch: SearchModelEnum.ByOthers,
            filterpartial_name: "_partial_Filter_Class_Module",
            entityName: "Exam Plan",
            entityFilter: "Class_Module_ExamPlan",
            canAdd: false,
            script: "~/Scripts/Admin/ManageSchedule_ClassModule");
        }

        [HttpPost]
        public ActionResult FilterClass_Module_ExamPlan(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            string entityFilter,
            DateTime? Start_Date, DateTime? End_Date,
            string class_id = "",
            int Class_Module_Status = 0,
            int Semester = 0,
            int Module_Id = 0,
            int Company_Id = 0,
            int Class_Module_Day = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var currentClass = new Class { Status = (int)EntityStatus.Visible };
            var items = GetClassModulesByExamDate(ref currentClass, Start_Date, End_Date, class_id);
            if (currentClass == null || !currentClass.IsValid())
                return RedirectToError("Class is not valid!");

            //var items = IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (Model_Name != "")
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();
                items = items.Where(c => c.Class_Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }
            if (Class_Module_Status != 0)
            {
                items = items.Where(d => d.Class_Module_Status == Class_Module_Status);
            }
            if (Semester != 0)
            {
                items = items.Where(d => d.Module.Semester == Semester);
            }
            if (Module_Id != 0)
            {
                items = items.Where(d => d.Module_Id == Module_Id);
            }
            if (Company_Id != 0)//lop review khong search duoc vi class == null hoac dung current company from session
            {
                items = items.Where(d => d.Class != null && d.Class.Company_Id == Company_Id);
            }
            if (Class_Module_Day != 0)
            {
                items = items.Where(d => d.Class_Module_Day == Class_Module_Day);
            }

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize, entityFilter);
        }

        #endregion ExamPlan

        public ActionResult ManageSchedule_ClassSession_Calendar(string class_id, DateTime? start_date, DateTime? end_date)
        {
            //return RedirectToError();

            if (string.IsNullOrEmpty(class_id))
                class_id = "Arena HCM";//current company from Session
            if (start_date == null)
                start_date = DateTimeUlti.StartOfWeek();
            if (end_date == null)
                end_date = DateTimeUlti.EndOfWeek();
            ViewBag.ClassID = class_id;
            ViewBag.StartDate = start_date;
            ViewBag.EndDate = end_date;

            return View();
        }

        [HttpPost]
        public ActionResult GetClassSession_Data(DateTime? start_date, DateTime? end_date)
        {
            var start = DateTime.Today;
            var end = start.AddDays(7);
            if (start_date.HasValue)
                start = start_date.Value;
            if (end_date.HasValue && start < end_date.Value)
                end = end_date.Value;

            var currentParent = new Class { Status = (int)EntityStatus.Visible };
            var list = GetClassModules(ref currentParent, start, end).SelectMany(s => s.Class_Module_Days_List);

            return Json(list.Select(s => new { 
                id = s.Id,
                resourceId = s.Class_Module.Resource_LT_Id,
                start = s.Class_Module_Day_Date.ToString("yyyy-MM-dd") + "T" + s.Class_Module.Class_Module_Hour_Start.ToUTCHourString() + "Z",
                end = s.Class_Module_Day_Date.ToString("yyyy-MM-dd") + "T" + s.Class_Module.Class_Module_Hour_End.ToUTCHourString() + "Z",
                title = s.Class_Module.Class_Module_FullName,
                backgroundColor = "#00c0ef",
                borderColor = "#00c0ef"
            }));
        }
    }
}