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
using OfficeOpenXml.Style;
using System.Drawing;

namespace XT.Web.Controllers
{
    [XTAuthorizeAcademic]
    public partial class ManageAcademicController : AdminBaseController
    {
        //CH, AH
        #region Course
        public ActionResult ManageAcademic_Course(int? page)
        {
            return ManageModel(new Course(), page);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddCourse(Course model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditCourse(Course model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteCourse(int id)
        {
            return DeleteModel(id);
        }
        #endregion Course

        //CH, AH
        #region CourseFamily
        public ActionResult ManageAcademic_CourseFamily(int? page, int id = 0)
        {
            //_partial_AddEdit_Course
            //_partial_Search_Course
            //return ManageModel(new CourseFamily(), null);

            IEnumerable<CourseFamily> list = null;
            if (id == 0)
            {
                list = IoCConfig.Service<ICourseFamilyService>().FindAllValid();
            }
            else
            {
                var course = IoCConfig.Service<ICourseService>().FindById(id);
                if (course == null || !course.IsValid())
                    return RedirectToError();
                list = course.CourseFamilies.Valid();
            }

            //return ManageModel(new Faculty_Module { Faculty_Id = id }, null,
            //    hasFilter: false,
            //    list: faculty.Faculty_Modules, entityName: "Skill");
            return ManageModel(new CourseFamily { Course_Id = id }, page,
                list: list, entityName: "Course Family", 
                currentParentId: id);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddCourseFamily(CourseFamily model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditCourseFamily(CourseFamily model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteCourseFamily(int id)
        {
            return DeleteModel(id);
        }
        #endregion

        //CH, AH
        #region Module
        public ActionResult ManageAcademic_Module(int? page)
        {
            //_partial_AddEdit_Course
            //_partial_Search_Course
            //return ManageModel(new Module(), page, "_partial_Filter_Module");
            return ManageModel(new Module(), page, 
            filterSearch: SearchModelEnum.ByOthers);
        }

        [HttpPost]
        public ActionResult FilterModule(
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int Course_Id = 0,
            int CourseFamily_Id = 0,
            int Semester = 0,
            int Module_Portal_Type = 0,
            string Model_Name = "",
            string sort_target = "Module_Name",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var items = IoCConfig.Service<IModuleService>().FindAllValid();
            if (Model_Name != "")
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();
                items = items.Where(c => c.Module_Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name)
                    || c.Module_Code.ToLower().Contains(name));
            }
            if (Course_Id != 0)
            {
                items = items.Where(d => d.CourseFamily.Course_Id == Course_Id);
            }
            if (CourseFamily_Id != 0)
            {
                items = items.Where(d => d.CourseFamily_Id == CourseFamily_Id);
            }
            if (Semester != 0)
            {
                items = items.Where(d => d.Semester == Semester);
            }
            if (Module_Portal_Type != 0)
            {
                items = items.Where(d => d.Module_Portal_Type == Module_Portal_Type);
            }

            switch (sort_target)
            {
                case "Id":
                    {
                        if (sort_rank == true)
                        {
                            items = items.OrderBy(d => d.Id);
                        }
                        else
                        {
                            items = items.OrderByDescending(d => d.Id);
                        }
                        break;
                    }
                case "Module_Name":
                    {
                        if (sort_rank == true)
                        {
                            items = items.OrderBy(d => d.Module_Name);
                        }
                        else
                        {
                            items = items.OrderByDescending(d => d.Module_Name);
                        }
                        break;
                    }
            }
            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            //return ReturnPartialView("Module", items, pageNumber, pageSize);
            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddModule(ModuleModel model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditModule(ModuleModel model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteModule(int id)
        {
            return DeleteModel(id);
        }

        
        #endregion

        //CH, AH, AAE, SRO, CS
        #region Class
        #region [XTAuthorizeAcademic/XTAuthorizeAcademicExecute] Class
        private IEnumerable<Class> GetClasses()
        {
            return IoCConfig.Service<IClassService>()
                .FindAllValid().OrderByDescending(c => c.Class_Admission_Date);
        }

        public ActionResult ManageAcademic_Class(int? page)
        {
            return ManageModel(new Class { Class_Admission_Date = DateTime.Today }, 
                page, "_partial_Filter_Class",
                list: GetClasses(),
                script: "~/Scripts/Admin/ManageSchedule_ClassModule",
                breadcrumbpartial_name: "_partial_BreadCrumb_Class", 
                filterSearch: SearchModelEnum.ByOthers);
        }

        [HttpPost]
        public ActionResult FilterClass(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int Class_Studying_Status = 0,
            int Company_Id = 0,
            int Class_Day = 0,

            string Model_Name = "",
            string sort_target = "Class_Name",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;
            var items = GetClasses();//IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (Model_Name != "")
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();
                items = items.Where(c => c.Class_Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }
            else//sửa
            {
                if (Company_Id != 0)
                {
                    items = items.Where(d => d.Company_Id == Company_Id);
                }
                if (Class_Studying_Status != 0)
                {
                    items = items.Where(d => d.Class_Studying_Status == Class_Studying_Status);
                }
                if (Class_Day != 0)
                {
                    items = items.Where(d => d.Class_Day == Class_Day);
                }
            }

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddClass(Class model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditClass(Class model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteClass(int id)
        {
            return DeleteModel(id);
        }

        
        #endregion Class

        #region [XTAuthorizeMod] ImportClass
        [HttpPost]
        [XTAuthorizeMod]
        public ActionResult ImportClass(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                //////////////////////////////////////////////////////
                //////////////////////////////////////////////////////
                //Constant Variables for importing
                var COURSE_FAMILY = 3;//ADIM 6824
                //////////////////////////////////////////////////////
                //////////////////////////////////////////////////////

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int row = 1; row <= noOfRow; row++)
                    {
                        try
                        {
                            var service = IoCConfig.Service<IClassService>();
                            var item = new Class();
                            item.Class_Name = GetCellValue_String(workSheet, row, 1);
                            item.Class_Admission_Date = DateTime.Today;
                            item.Company_Id = CURRENT_COMPANY;//AMMHCM1
                            item.CourseFamily_Id = COURSE_FAMILY;//HCM ADIM 6824

                            //check exist
                            var existed = service.FindValidByCriteria(old => old.Class_Name.ToLower() == item.Class_Name.ToLower()
                                && old.Company_Id == CURRENT_COMPANY);
                            if (!IsValidModel(existed))
                            {
                                service.Add(item);
                            }
                            else
                            {
                                //service.Update(existed);
                            }
                        }
                        catch (Exception ex)
                        {
                            return Error(ex.Message + " - " + row);
                        }
                    }

                    return MyContent("Upload successfully!");
                }
            }

            return Error();
        }
        #endregion

        #endregion Class

        //CH, AH, AAE, SRO, CS
        #region [XTAuthorizeAcademic] Student

        #region [XTAuthorizeMod] Import Student
        private string GetCellValue(ExcelWorksheet workSheet, int row, AMM col, bool isText = true)
        {
            return GetCellValue(workSheet, row, (int)col, isText);
        }

        private DateTime GetCellValue_DateTime(ExcelWorksheet workSheet, int row, AMM col, bool isText = true)
        {
            return GetCellValue_DateTime_Full(workSheet, row, (int)col, isText);
        }

        private StudentAcademicStatusEnum GetCellValue_StudentStatus(ExcelWorksheet workSheet, int row)
        {
            var status = GetCellValue(workSheet, row, AMM.Status).ToLower().Trim();
            switch (status)
            {
                case "studying":
                    return StudentAcademicStatusEnum.Studying;
                case "delay":
                case "reserve":
                    return StudentAcademicStatusEnum.Delay;
                case "dropout":
                case "drop-out":
                    return StudentAcademicStatusEnum.Dropout;
                case "transfer":
                    return StudentAcademicStatusEnum.Transfer;
                case "finish":
                    return StudentAcademicStatusEnum.Finished;
            }

            return StudentAcademicStatusEnum.Studying;
        }

        private enum AMM
        {
            No = 1,
            EnrollNumber = 2,
            FirstName = 3,
            LastName = 4,
            FullName = 5,
            FirstClass = 6,
            CurrentClass = 7,
            Status = 8,
            DateOfDoing = 9,
            Sex = 10,
            Birthday = 11,
            M = 12,
            MobilePhone = 13,
            HomePhone = 14,
            ContactPhone = 15,
            Email = 16,
            Address = 17,
            ContactAddress = 18,
            District = 19,
            City = 20,
            Parent = 21,
            Relative = 22,
            PhoneContact = 23,
            ApplicationDate = 24,
            HoSo = 25,
            CS = 26,
            Course = 27,
            CourseFamily = 28,
            HighSchool = 32,
            University = 33,
            Company = 34,
            Company_Address = 35,
            Company_Postion = 36,
            Company_Salary = 37,
        }

        private Student CheckClassForStudent(Student u, string className, int CourseFamily_Id)
        {
            var lower_className = className.ToLower();
            var current_class = IoCConfig.Service<IClassService>()
                .FindByCriteria(cl => cl.Company_Id == CURRENT_COMPANY && cl.Class_Name.ToLower() == lower_className);
            if (current_class == null)
            {
                current_class = IoCConfig.Service<IClassService>().Add(new Class
                {
                    Class_Name = className,
                    Company_Id = CURRENT_COMPANY,
                    CourseFamily_Id = CourseFamily_Id,
                    Class_Studying_Status = (int)ClassStatusEnum.Studying,
                    Class_Admission_Date = DateTime.Today,
                    Status = (int)EntityStatus.Visible,
                    Created_Date = DateTime.Now
                });
            }

            u.Student_ClassHistories.Add(new Student_ClassHistory
            {
                Class_Id = current_class.Id,
                StartDate = DateTime.Now,
                Status = (int)EntityStatus.Visible,
                Created_Date = DateTime.Now,
            });

            u.Class_Id = current_class.Id;

            return u;
        }

        private string ImportOneStudent(ExcelWorksheet workSheet, int rowIterator)
        {
            var u = new Student { 
                Status = (int)EntityStatus.Visible,
                Created_Date = DateTime.Now
            };

            //////////////////////////////////////////////////////
            //////////////////////////////////////////////////////
            //Constant Variables for importing
            //var CURRENT_COMPANY = 3;
            //////////////////////////////////////////////////////
            //////////////////////////////////////////////////////

            Student existed = null;

            u.Student_EnrollNumber = GetCellValue(workSheet, rowIterator, AMM.EnrollNumber);
            if (string.IsNullOrEmpty(u.Student_EnrollNumber))
            {
                return "";
                //update
                //u.Student_FirstName = GetCellValue(workSheet, rowIterator, AMM.FirstName);
                //u.Student_LastName = GetCellValue(workSheet, rowIterator, AMM.LastName);
                //existed = IoCConfig.Service<IStudentService>().FindByCriteria(s =>
                //    s.Student_FirstName == u.Student_FirstName && s.Student_LastName == u.Student_LastName);

                //if (!IsValidModel(existed))
                //{
                //    //new
                //    var r = new Random();
                //    var id = r.Next(1000000);
                //    u.Student_EnrollNumber = PREFIX_NONSTUDENT + id.ToString();
                //}
            }
            else
            {
                existed = IoCConfig.Service<IStudentService>().FindByCriteria(s => s.Student_EnrollNumber == u.Student_EnrollNumber);
            }

            if (IsValidModel(existed))
            {
                //update for existed students
                //u = existed;
                //u.Student_Status = (int)GetCellValue_StudentStatus(workSheet, rowIterator);
                //u.Student_Status_Date = GetCellValue_DateTime(workSheet, rowIterator, AMM.DateOfDoing);

                //IoCConfig.Service<IStudentService>().Update(u);

                return "Existed";
                //Lỗi exsited có thể có trường hợp bị trùng MSSV
            }

            u.Student_FirstName = GetCellValue(workSheet, rowIterator, AMM.FirstName);
            u.Student_LastName = GetCellValue(workSheet, rowIterator, AMM.LastName);

            //course
            var Center_Id = CURRENT_COMPANY;//AuthenticationManager.Company_Id == 0 ? 1 : AuthenticationManager.Company_Id;
            var CourseName = GetCellValue(workSheet, rowIterator, AMM.Course);
            if (string.IsNullOrEmpty(CourseName))
            {
                CourseName = "HDSE";
            }
            //else if (CourseName.Contains(" "))
            //{
            //    CourseName = (CourseName.Split(" ".ToCharArray()))[1];
            //}
            //else if (CourseName.Contains("-"))
            //{
            //    CourseName = (CourseName.Split("-".ToCharArray()))[1];
            //}
            var CourseFamilyName = GetCellValue(workSheet, rowIterator, AMM.CourseFamily);
            if (string.IsNullOrEmpty(CourseFamilyName))
            {
                CourseFamilyName = "6629";
            }
            var course = IoCConfig.Service<ICourseFamilyService>()
                .FindByCriteria(c => c.CourseFamily_Name == CourseFamilyName &&
                    c.Course.Course_Code == CourseName);
            if (IsValidModel(course))
            {
                u.CourseFamily_Id = course.Id;

                //first class by center
                var firstClass = GetCellValue(workSheet, rowIterator, AMM.FirstClass);
                u = CheckClassForStudent(u, firstClass, course.Id);

                var currentClass = GetCellValue(workSheet, rowIterator, AMM.CurrentClass);
                if (!string.IsNullOrWhiteSpace(currentClass) && currentClass.ToLower() != firstClass.ToLower())
                {
                    //class 2 by center
                    u = CheckClassForStudent(u, currentClass, course.Id);
                }

                u.Student_Status = (int)GetCellValue_StudentStatus(workSheet, rowIterator);
                u.Student_Status_Date = GetCellValue_DateTime(workSheet, rowIterator, AMM.DateOfDoing);
                var gioitinh = GetCellValue(workSheet, rowIterator, AMM.Sex);
                if (!string.IsNullOrEmpty(gioitinh))
                {
                    u.Student_Gender = gioitinh == "M" ? (int)GenderEnum.Male : (int)GenderEnum.Female;
                }

                u.Student_Birthday = GetCellValue_DateTime(workSheet, rowIterator, AMM.Birthday);
                u.Student_MobilePhone = GetCellValue(workSheet, rowIterator, AMM.MobilePhone);
                u.Student_HomePhone = GetCellValue(workSheet, rowIterator, AMM.HomePhone);
                u.Student_ContactPhone = GetCellValue(workSheet, rowIterator, AMM.PhoneContact);
                u.Student_Email = GetCellValue(workSheet, rowIterator, AMM.Email);
                u.Student_Address = GetCellValue(workSheet, rowIterator, AMM.Address);
                u.Student_ContactAddress = GetCellValue(workSheet, rowIterator, AMM.ContactAddress);
                u.Student_District = GetCellValue(workSheet, rowIterator, AMM.District);
                u.Student_City = GetCellValue(workSheet, rowIterator, AMM.City);
                u.Student_Sponsor = GetCellValue(workSheet, rowIterator, AMM.Parent);
                u.Student_Sponsor_Relation = GetCellValue(workSheet, rowIterator, AMM.Relative);
                u.Student_Application_Date = GetCellValue_DateTime(workSheet, rowIterator, AMM.ApplicationDate);
                u.Student_Application_Documents = GetCellValue(workSheet, rowIterator, AMM.HoSo);
                u.Student_Application_CS = GetCellValue(workSheet, rowIterator, AMM.CS);
                u.HighSchool = GetCellValue(workSheet, rowIterator, AMM.HighSchool);
                u.University = GetCellValue(workSheet, rowIterator, AMM.University);
                u.Company = GetCellValue(workSheet, rowIterator, AMM.Company);
                u.Company_Address = GetCellValue(workSheet, rowIterator, AMM.Company_Address);
                u.Company_Position = GetCellValue(workSheet, rowIterator, AMM.Company_Postion);
                u.Company_Salary = GetCellValue(workSheet, rowIterator, AMM.Company_Salary);

                IoCConfig.Service<IStudentService>().Add(u);
                return "OK";
            }

            return CourseFamilyName;
        }

        [HttpPost]
        [XTAuthorizeCenterHead]
        public ActionResult ImportStudent(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                var usersList = new List<Student>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    //export invalid student
                    StreamWriter ws = new StreamWriter("C:\\Users\\Duong\\Google Drive\\_RedTeam_Aptech\\ImportStudent_invalid.txt");
                    var invalid_count = 0;
                    var total_count = 0;
                    for (int rowIterator = 3; rowIterator <= noOfRow; rowIterator++)
                    {
                        total_count++;
                        var u = ImportOneStudent(workSheet, rowIterator);
                        if (u != "OK")
                        {
                            invalid_count++;
                            ws.WriteLine(rowIterator + " - " + u);
                        }
                    }

                    ws.WriteLine("Invalid: " + invalid_count);
                    ws.WriteLine("Total: " + total_count);
                    ws.Close();

                    return MyContent("Upload successfully!");
                }
            }

            return Error();
        }
        #endregion

        #region [XTAuthorizeMod] Import Marks
        private Class_Module_StudentExam GetModuleMark(ExcelWorksheet ws, int row, int col)
        {
            var ROW_MODULE_NAME = 2;
            var ROW_MODULE_TYPE = ROW_MODULE_NAME + 1;

            //get mark
            var mark = 0.0f;
            var txt = GetCellValue(ws, row, col);
            if (!string.IsNullOrWhiteSpace(txt))
            {
                mark = float.Parse(txt);
            }

            //get module code
            var module_code = "";
            txt = GetCellValue(ws, ROW_MODULE_NAME, col);//TGP
            if (!string.IsNullOrWhiteSpace(txt))
            {
                module_code = txt;
            }
            else
            {
                module_code = GetCellValue(ws, ROW_MODULE_NAME, col - 1);//TGP
            }
            if (string.IsNullOrEmpty(module_code))
                return null;

            //get module type
            txt = GetCellValue(ws, ROW_MODULE_TYPE, col);//ME
            var type = txt.Contains("ME") ? "LT" : "TH";

            return null;
        }

        private Student ImportOneStudent_Mark_ByModule(
            Student student, Module module, float module_mark, string module_type,
            Faculty faculty, Resource resource)
        {
            var student_class_module = IoCConfig.Service<IClass_Module_StudentExamService>()
                                    .FindByCriteria(m => m.Student_Id == student.Id && m.Class_Module.Module_Id == module.Id);
            if (student_class_module == null)
            {
                //var class_module = u.Class.Class_Modules.FirstOrDefault(cm => cm.Module_Id == module.Id);
                var class_module = IoCConfig.Service<IClass_ModuleService>()
                            .FindByCriteria(m => m.Class_Id == student.Class_Id && m.Module_Id == module.Id);
                if (class_module == null)
                {
                    class_module = IoCConfig.Service<IClass_ModuleService>().Add(
                        new Class_Module
                        {
                            Module_Id = module.Id,
                            Class_Id = student.Class_Id,
                            Faculty_Id = faculty.Id,
                            Resource_LT_Id = resource.Id,
                            Resource_TH_Id = resource.Id,
                            Resource_Exam_Id = resource.Id,
                            Class_Module_Date_Start = DateTime.Today,
                            Class_Module_Date_End = DateTime.Today,
                            Class_Module_Date_Exam = DateTime.Today,
                            Class_Module_Status = (int)ClassModuleStatusEnum.Studying,
                            Status = (int)EntityStatus.Visible,
                            Created_Date = DateTime.Now
                        }
                        );
                }

                IoCConfig.Service<IClass_Module_StudentExamService>().Add(
                    new Class_Module_StudentExam
                    {
                        Class_Module_Id = class_module.Id,
                        Student_Id = student.Id,
                        Student_Status = (int)StudentClassModuleStatusEnum.Official,
                        Mark_LT = module_type == "LT" ? module_mark : 0,
                        Mark_TH = module_type == "TH" ? module_mark : 0,
                        Exam_Count = 1,
                        Status = (int)EntityStatus.Visible,
                        Created_Date = DateTime.Now
                    }
                    );
            }
            else
            {
                if (module_type == "LT")
                    student_class_module.Mark_LT = module_mark;
                else
                    student_class_module.Mark_TH = module_mark;

                IoCConfig.Service<IClass_Module_StudentExamService>().Update(student_class_module);
            }

            return student;
        }

        private Student ImportOneStudent_Mark(ExcelWorksheet workSheet, int rowIterator, Faculty faculty, Resource resource)
        {
            //if (rowIterator == 53)
            //{
            //    int i = 0;
            //}
            var enroll = GetCellValue(workSheet, rowIterator, 4).ToLower();
            var student_enroll = "Student" + enroll;

            var student = IoCConfig.Service<IStudentService>().FindByCriteria(s => s.Student_EnrollNumber.ToLower() == enroll || s.Student_EnrollNumber.ToLower() == student_enroll);
            if (student != null)
            {
                //clear existed marks
                var marks = IoCConfig.Service<IClass_Module_StudentExamService>()
                                .FindAllValidByCriteria(m => m.Student_Id == student.Id);
                foreach (var pre_mark in marks)
                {
                    pre_mark.Mark_LT = 0;
                    pre_mark.Mark_TH = 0;
                    IoCConfig.Service<IClass_Module_StudentExamService>().Update(pre_mark);
                }

                var modules = student.Portal_Modules_List;
                var module_name = "";
                var module_type = "LT";

                //constants
                var MODULE_TYPE_LT = "E";//AMM ME, AA E
                var ROW_MODULE_NAME = 2;
                var ROW_MODULE_TYPE = ROW_MODULE_NAME + 1;
                var COL_MARK_START = 7;//G
                var COL_MARK_END = 57;//AMM 47, AA 57
                //end constants
                
                for (int col = COL_MARK_START; col <= COL_MARK_END; col++)
                {
                    //module name
                    var current_module = GetCellValue(workSheet, ROW_MODULE_NAME, col);//TGP
                    if (!string.IsNullOrWhiteSpace(current_module))
                    {
                        module_name = current_module;
                    }
                    //module type
                    var current_type = GetCellValue(workSheet, ROW_MODULE_TYPE, col);//ME
                    module_type = current_type.Contains(MODULE_TYPE_LT) ? "LT" : "TH";

                    //module mark
                    var module_mark = 0.0f;
                    var mark = GetCellValue(workSheet, rowIterator, col);
                    if (!string.IsNullOrWhiteSpace(mark))
                    {
                        module_mark = GetCellValue_Float(workSheet, rowIterator, col);
                        if (module_mark > 0)
                        {
                            var module = modules.FirstOrDefault(m => m.Module_Code.ToLower() == module_name.ToLower());
                            if (module != null)
                            {
                                student = ImportOneStudent_Mark_ByModule(student, module, module_mark, module_type, faculty, resource);
                            }
                        }
                    }
                }
            }

            return student;
        }

        [HttpPost]
        [XTAuthorizeMod]
        public ActionResult ImportMark(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    var count = 0;
                    var faculty = IoCConfig.Service<IFacultyService>().FindAllValid().First();
                    var resource = IoCConfig.Service<IResourceService>().FindAllValid().First();

                    for (int rowIterator = 5; rowIterator <= noOfRow; rowIterator++)
                    {
                        var u = ImportOneStudent_Mark(workSheet, rowIterator, faculty, resource);
                        if (u != null) count++;
                    }

                    return MyContent("Upload successfully! - " + count);
                }
            }

            return Error();
        }

        private Student ImportOneStudent_PortalMark(ExcelWorksheet workSheet, int rowIterator, Faculty faculty, Resource resource)
        {
            var enroll = GetCellValue(workSheet, rowIterator, 4).ToLower();
            var student_enroll = "Student" + enroll;

            var student = IoCConfig.Service<IStudentService>().FindByCriteria(s => s.Student_EnrollNumber.ToLower() == enroll || s.Student_EnrollNumber.ToLower() == student_enroll);
            if (student != null)
            {
                var modules = student.Portal_Modules_List;

                //module
                var SEMESTER_NAME_COL = 10;
                var EXAM_NAME_COL = 12;
                var MARK_OBTAINED_COL = 17;
                var semester_name = GetCellValue(workSheet, rowIterator, SEMESTER_NAME_COL);
                var exam_name = GetCellValue(workSheet, rowIterator, EXAM_NAME_COL);

                var module = modules.FirstOrDefault(m => m.Semester_Name_Portal.ToLower() == semester_name.ToLower()
                                                        && m.Module_Name_Portal.ToLower() == exam_name.ToLower());
                if (module != null)
                {
                    var module_mark = GetCellValue_Float(workSheet, rowIterator, MARK_OBTAINED_COL);
                    var module_type = "LT";

                    student = ImportOneStudent_Mark_ByModule(student, module, module_mark, module_type, faculty, resource);
                }
            }

            return student;
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult ImportPortalMark(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    var count = 0;
                    var faculty = IoCConfig.Service<IFacultyService>().FindAllValid().First();
                    var resource = IoCConfig.Service<IResourceService>().FindAllValid().First();

                    for (int rowIterator = 4; rowIterator <= noOfRow; rowIterator++)
                    {
                        var u = ImportOneStudent_PortalMark(workSheet, rowIterator, faculty, resource);
                        if (u != null) count++;
                    }

                    return MyContent("Upload successfully! - " + count);
                }
            }

            return Error();
        }
        #endregion

        #region [XTAuthorizeAcademic/XTAuthorizeAcademicExecute] Manage Student
        private IEnumerable<Student> GetStudentsByClass(ref Class currentParent, string class_id = "")
        {
            IEnumerable<Student> list = null;
            if (class_id != "")
            {
                currentParent = IoCConfig.Service<IClassService>().FindByClassName(class_id);
                if (currentParent != null && currentParent.IsValid())
                {
                    list = currentParent.Students.Valid();
                }
            }
            else
            {
                list = IoCConfig.Service<IStudentService>().FindAllValid();
            }

            return list;
        }

        public ActionResult ManageAcademic_Student(int? page, string class_id = "", int id = 0, int status = 0)
        {
            IEnumerable<Student> list = null;
            var currentParent = new Class { Status = (int)EntityStatus.Visible };
            if (id > 0)
            {
                var student = IoCConfig.Service<IStudentService>().FindById(id);
                if (!IsValidModel(student))
                    return RedirectToError("Student is not valid!");

                list = new List<Student>() { student };
            }
            else
            {
                list = GetStudentsByClass(ref currentParent, class_id);
                if (currentParent == null || !currentParent.IsValid())
                    return RedirectToError("Class is not valid!");
            }
            if (status > 0)
            {
                list = list.Where(s => s.Student_Status == status);
            }

            ViewBag.Students = list;

            return ManageModel(new Student
            {
                Student_Application_Date = DateTime.Today,
                Student_Status_Date = DateTime.Today,
                Student_Birthday = DateTime.Today,
                Class_Id = currentParent.Id
            }, page, list: list, modal_size: "modal-lg",
            currentParentId: currentParent.Id, currentParentName: currentParent.Class_Name,
            breadcrumbpartial_name: "_partial_BreadCrumb_Student",
            filterSearch: SearchModelEnum.ByOthers);
        }

        [HttpPost]
        public ActionResult FilterStudent(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int Company_Id = 0,
            int Student_Status = 0,
            int Due_Status = 0,
            string Class_Id = "",
            DateTime? Birthday_Start_Date = null,
            DateTime? Birthday_End_Date = null,
            DateTime? Status_Start_Date = null,
            DateTime? Status_End_Date = null,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var currentClass = new Class { Status = (int)EntityStatus.Visible };
            var items = GetStudentsByClass(ref currentClass, Class_Id);
            if (currentClass == null || !currentClass.IsValid())
                return RedirectToError("Class is not valid!");

            //var items = IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (!string.IsNullOrEmpty(Model_Name))
            {
                var name = Model_Name;
                name = name.Trim().ToLower();

                if (name.IsNumber())
                {
                    items = items.Where(c => c.GetPhone().Contains(name) || c.GetEnrollNumber().Contains(name));
                }
                else
                {
                    if (name.StartsWith(PREFIX_STUDENT.ToLower()) || name.StartsWith(PREFIX_NONSTUDENT.ToLower()))
                    {
                        items = items.Where(c => c.GetEnrollNumber().ToLower().Contains(name));
                    }
                    else
                    {
                        name = name.Replace(PREFIX_STUDENT.ToLower(), "").Replace(PREFIX_NONSTUDENT.ToLower(), "");
                        name = name.Convert_Chuoi_Khong_Dau();
                        items = items.Where(c => c.Student_FullName.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
                    }
                }
            }
            if (Student_Status != 0)
            {
                items = items.Where(d => d.Student_Status == Student_Status);
            }
            if (Due_Status != 0)
            {
                if (Due_Status == (int)DueStatusEnum.NoDue)
                {
                    items = items.Where(d => d.Student_FeePlan_List.Count() == 0);
                }
                else
                {
                    items = items.Where(d => d.Student_FeePlan_List.Count() > 0);
                }
            }
            if (Company_Id != 0)
            {
                items = items.Where(d => d.Class.Company_Id == Company_Id);
            }
            if (Birthday_Start_Date.HasValue && Birthday_End_Date.HasValue)
            {
                var birthday_start = Birthday_Start_Date.ToBirthDay();
                var birthday_end = Birthday_End_Date.ToBirthDay();
                items = items.Where(u => birthday_start <= u.Student_Birthday.ToBirthDay()
                                                    && u.Student_Birthday.ToBirthDay() <= birthday_end);
            }
            if (Status_Start_Date.HasValue && Status_End_Date.HasValue)
            {
                items = items.Where(d =>
                    d.Student_Status_Date >= Status_Start_Date.Value &&
                    d.Student_Status_Date <= Status_End_Date.Value);
            }

            //sorting
            switch (sort_target)
            {
                case "Student_FullName":
                    {
                        if (sort_rank == true)
                        {
                            items = items.OrderBy(d => d.Student_FullName);
                        }
                        else
                        {
                            items = items.OrderByDescending(d => d.Student_FullName);
                        }
                        break;
                    }
                case "Remain_Fee":
                    {
                        if (sort_rank == true)
                        {
                            items = items.OrderBy(d => d.Remain_Fee);
                        }
                        else
                        {
                            items = items.OrderByDescending(d => d.Remain_Fee);
                        }
                        break;
                    }
            }

            ViewBag.Students = items;
            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";
            ViewBag.ModalSize = "modal-lg";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddStudent(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Student_Id > 0)
                {
                    var existed = IoCConfig.Service<IStudentService>().FindById(model.Student_Id);
                    if (!IsValidModel(existed))
                    {
                        return Error("Student is not existed");
                    }

                    existed = IoCConfig.Service<IStudentService>().UpdateClass(existed, model.Class_Id);
                    if (existed == null)
                    {
                        return Error("This student is a member of class already!");
                    }

                    return Success();
                }
                return AddModel(model);
            }

            return Error();
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditStudent(StudentModel model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteStudent(int id)
        {
            return DeleteModel(id);
        }

        #endregion Student > Students

        #region Find Student (Ajax: in select2)
        private object GetStudentInfo(Student s)
        {
            return new
            {
                id = s.Id,
                Student_FullName = s.Student_FullName,
                Student_Avatar = s.Student_Avatar != null ? s.Student_Avatar : AppSettings.DefaultAccountAvatar,
                Student_EnrollNumber = s.Student_EnrollNumber,
            };
        }

        [HttpPost]
        public ActionResult FilterStudent_Ajax(//dùng model để generic
            string q = "")
        {
            var currentClass = new Class { Status = (int)EntityStatus.Visible };
            var items = GetStudentsByClass(ref currentClass);
            if (currentClass == null || !currentClass.IsValid())
                return RedirectToError("Class is not valid!");

            //var items = IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (q != "")
            {
                var name = q;
                name = name.ToLower();
                name = name.Replace("student", "");
                name = name.Convert_Chuoi_Khong_Dau();

                if (name.IsNumber())
                {
                    items = items.Where(c => c.GetPhone().Contains(name) || c.GetEnrollNumber().Contains(name));
                }
                else
                {
                    items = items.Where(c => c.Student_FullName.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
                }

                //var name = q;
                //name = name.ToLower().Convert_Chuoi_Khong_Dau();
                //items = items.Where(c => c.Student_FullName.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }

            return Json(items.Select(s => GetStudentInfo(s)));
        }
        #endregion Find Student (Ajax: in select2)

        #region [XTAuthorizeAcademic] Student > Student Data

        #region [XTAuthorizeAcademic] Student Attendance (Student > Quản lý điểm danh của HV)
        public ActionResult ManageAcademic_Student_Attendance(int? page, int id)
        {
            var current_student = IoCConfig.Service<IStudentService>().FindById(id);
            if (current_student == null || !current_student.IsValid())
                return RedirectToError();

            return ManageModel(
                new Class_Module_StudentExam(), page,
                list: current_student.Class_Module_StudentExams.Valid(),
                entityName: "Student Attendance",
                entityFilter: "Student_Attendance",
                currentParentName: current_student.Student_FullName,
                filterSearch: SearchModelEnum.None,
                canAdd: false);
        }
        #endregion Manage Student Attendance

        #region [XTAuthorizeAcademic] Student_ClassHistory (Lich su chuyen lop - Class Change)
        public ActionResult ManageAcademic_Student_ClassChange(int? page, int id)
        {
            var current_student = IoCConfig.Service<IStudentService>().FindById(id);
            if (current_student == null || !current_student.IsValid())
                return RedirectToError();

            var currentParentName = current_student.Student_FullName + " - " + current_student.Class.Class_Name;

            return ManageModel(
                new Student_ClassHistory
                {
                    StartDate = DateTime.Today,
                    Student_Id = current_student.Id,
                    Student = current_student
                }, page,
                list: current_student.Student_ClassHistories.Valid(),
                currentParentId: current_student.Id,
                currentParentName: currentParentName,
                entityName: "Class Change",
                filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddStudent_ClassHistory(Student_ClassHistory model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditStudent_ClassHistory(Student_ClassHistory model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteStudent_ClassHistory(int id)
        {
            return DeleteModel(id);
        }
        #endregion Student_ClassHistory

        #region [XTAuthorizeAcademic] Student_AcademicStatus
        public ActionResult ManageAcademic_Student_AcademicStatus(int? page, int id)
        {
            var current_student = IoCConfig.Service<IStudentService>().FindById(id);
            if (current_student == null || !current_student.IsValid())
                return RedirectToError();

            var currentParentName = current_student.Student_FullName + " - " + current_student.Class.Class_Name;

            return ManageModel(
                new Student_AcademicStatus
                {
                    Student_Status_Date = DateTime.Today,
                    Student_Id = current_student.Id,
                    Student = current_student
                }, page,
                list: current_student.Student_AcademicStatuses.Valid(),
                currentParentId: current_student.Id,
                currentParentName: currentParentName,
                entityName: "Academic Status",
                filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddStudent_AcademicStatus(Student_AcademicStatus model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditStudent_AcademicStatus(Student_AcademicStatus model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteStudent_AcademicStatus(int id)
        {
            return DeleteModel(id);
        }
        #endregion Student_AcademicStatus

        #region [XTAuthorizeAcademic] Student_FeePlan
        public ActionResult ManageAcademic_Student_FeePlan(int? page, int id)
        {
            var current_student = IoCConfig.Service<IStudentService>().FindById(id);
            if (current_student == null || !current_student.IsValid())
                return RedirectToError();

            var currentParentName = current_student.Student_FullName + " - " + current_student.Class.Class_Name;

            return ManageModel(
                new Student_FeePlan
                {
                    Student_Id = current_student.Id,
                    Student = current_student,
                    FeePlan_StartDate = DateTime.Today
                }, page,
                list: current_student.Student_FeePlans.Valid(),
                currentParentId: current_student.Id,
                currentParentName: currentParentName,
                entityName: "Student FeePlan",
                filterSearch: SearchModelEnum.None,
                script: "~/Scripts/Admin/ManageFeePlan_Student");
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddStudent_FeePlan(Student_FeePlanModel model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditStudent_FeePlan(Student_FeePlan model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteStudent_FeePlan(int id)
        {
            return DeleteModel(id);
        }

        #endregion Student_FeePlan

        #region [XTAuthorizeAcademic] Student_FeePlan_Detail
        public ActionResult ManageAcademic_Student_FeePlan_Installment(int id)
        {
            var current_feeplan = IoCConfig.Service<IStudent_FeePlanService>().FindById(id);
            if (!IsValidModel(current_feeplan))
                return RedirectToError();

            //ViewBag.CurrentStudent = current_feeplan.Student;

            return View(current_feeplan);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddStudent_FeePlan_Installment(Student_FeePlan_Installment model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditStudent_FeePlan_Installment(Student_FeePlan_Installment model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteStudent_FeePlan_Installment(int id)
        {
            return DeleteModel(id);
        }

        [HttpGet]
        public ActionResult GetStudentFeePlanDetail(int id)
        {
            var current_student = IoCConfig.Service<IStudentService>().FindById(id);
            if (current_student == null)
                return ErrorNotExist();

            ViewBag.ViewOnly = true;

            return PartialView("Partial/_partial_Student_FeePlans", current_student);
            //var feeplan = IoCConfig.Service<IStudent_FeePlanService>().FindById(id);
            //if (feeplan == null)
            //    return ErrorNotExist();

            //ViewBag.ViewOnly = true;

            //return PartialView("Partial/_partial_Student_FeePlan", feeplan);
        }

        #endregion Student_FeePlan

        #region [XTAuthorizeAcademic] Student_BookOrder
        public ActionResult ManageAcademic_Student_BookOrder(int? page, int id)
        {
            var current_student = IoCConfig.Service<IStudentService>().FindById(id);
            if (!IsValidModel(current_student))
                return RedirectToError();

            return ManageModel(
                new BookOrder_Detail(), page,
                list: current_student.BookOrder_Details.Valid(),
                entityName: "Book Order",
                currentParentName: current_student.Student_FullName,
                filterSearch: SearchModelEnum.None,
                canAdd: false);

            //var current_bookorder = IoCConfig.Service<IBookOrderService>().FindById(id);
            //if (current_bookorder == null || !current_bookorder.IsValid())
            //    return RedirectToError();

            //var currentParentName = current_bookorder.Indent_Number + " (" + current_bookorder.Indent_Date.ToDateString() + ")";

            //return ManageModel(new BookOrder_Detail
            //{
            //    BookOrder_Id = id
            //}, page,
            //                list: current_bookorder.BookOrder_Details.Valid(),
            //                entityName: "BookOrder Detail",
            //                currentParentId: id, currentParentName: currentParentName,
            //                filterSearch: SearchModelEnum.None);
        }

        #region Book Order

        #region [XTAuthorizeAcademic] ManageBookOrder
        public ActionResult ManageAcademic_BookOrder(int? page)
        {
            var list = IoCConfig.Service<IBookOrderService>().FindAllValid().OrderByDescending(o => o.Indent_Date);
            return ManageModel(new BookOrder
            {
                Indent_Date = DateTime.Today
            }, page,
            list: list,
            breadcrumbpartial_name: "_partial_BreadCrumb_BookOrder",
            filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddBookOrder(BookOrder model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditBookOrder(BookOrder model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteBookOrder(int id)
        {
            return DeleteModel(id);
        }
        #endregion

        #region [XTAuthorizeAcademicExecute] Import BookOrder
        private enum BookOrderExcelEnum
        {
            SrNo = 1,
            StudentName = 2,
            EnrollmentNo = 3,
            BookCode = 4,
            BookPrice = 5,
        }

        private string GetCellValue(ExcelWorksheet workSheet, int row, BookOrderExcelEnum col, bool isText = true)
        {
            return GetCellValue(workSheet, row, (int)col, isText);
        }

        private int GetCellValue_Int(ExcelWorksheet workSheet, int row, BookOrderExcelEnum col, bool isText = true)
        {
            var val = GetCellValue(workSheet, row, (int)col, isText);
            if (!string.IsNullOrEmpty(val))
                return int.Parse(val);

            return 0;
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult ImportBookOrder(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                var companies = IoCConfig.Service<ICompanyService>().FindAllValid();

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    //BookOrder
                    var bookService = IoCConfig.Service<IBookOrderService>();
                    var row = 3;
                    var bookorder = new BookOrder();
                    bookorder.Indent_Date = GetCellValue_DateTime_Full(workSheet, row, 1); row += 2;
                    bookorder.Indent_Number = int.Parse(GetCellValue_String(workSheet, row, 1)); row += 2;
                    bookorder.Center = GetCellValue_String(workSheet, row, 1).Trim(); row += 2;
                    var com = companies.FirstOrDefault(c => c.Company_Name_Portal.ToLower() == bookorder.Center.ToLower());
                    if (com != null)
                    {
                        var existOrder = bookService.FindAllValidByCriteria(b => b.Indent_Number == bookorder.Indent_Number && b.Company_Id == com.Id).FirstOrDefault();
                        if (IsValidModel(existOrder))
                            bookorder = existOrder;

                        //Update date
                        bookorder.Company_Id = com.Id;
                        bookorder.Indent_Status = GetCellValue_String(workSheet, row, 1) == "Pending" ?
                        (int)IndentStatusEnum.Pending : (int)IndentStatusEnum.Approved; row += 2;
                        bookorder.SAP_Customer_ID = GetCellValue_String(workSheet, row, 1);

                        //BookOrder_Detail
                        row = 15;
                        for (int rowIterator = row; rowIterator <= noOfRow; rowIterator++)
                        {
                            var u = new BookOrder_Detail();

                            var EnrollmentNo = GetCellValue(workSheet, rowIterator, BookOrderExcelEnum.EnrollmentNo).Replace("Student", "");
                            var student = IoCConfig.Service<IStudentService>().FindAllValidByCriteria(s => s.Student_EnrollNumber.Replace("Student", "") == EnrollmentNo).FirstOrDefault();
                            if (student != null && student.IsValid())
                            {
                                u.Student_Id = student.Id;
                                u.BookCode = GetCellValue(workSheet, rowIterator, BookOrderExcelEnum.BookCode);
                                u.BookPrice = GetCellValue_Int(workSheet, rowIterator, BookOrderExcelEnum.BookPrice);

                                var existDetail = bookorder.BookOrder_Details_List.FirstOrDefault(d => d.BookCode == u.BookCode && d.Student_Id == u.Student_Id);
                                if (!IsValidModel(existDetail))
                                {
                                    bookorder.BookOrder_Details.Add(u);
                                }
                            }
                        }//endfor

                        try
                        {
                            if (bookorder.Id == 0)
                                bookService.Add(bookorder);
                            else
                                bookService.Update(bookorder);
                        }
                        catch (Exception ex)
                        {
                            return Error("Import Book Order failed!");
                        }

                        return Success("Import Book Order successfully!");
                    }

                    return Error("Company Name in Portal is not correct!");
                }
            }

            return Error();
        }
        #endregion Import BookOrder

        #region [XTAuthorizeAcademic] BookOrder Detail
        [XTAuthorizeAcademic]
        public ActionResult ManageAcademic_BookOrder_Detail(int? page, int id)
        {
            var current_bookorder = IoCConfig.Service<IBookOrderService>().FindById(id);
            if (current_bookorder == null || !current_bookorder.IsValid())
                return RedirectToError();

            var currentParentName = current_bookorder.Indent_Number + " (" + current_bookorder.Indent_Date.ToDateString() + ")";

            return ManageModel(new BookOrder_Detail
            {
                BookOrder_Id = id
            }, page,
                            list: current_bookorder.BookOrder_Details.Valid(),
                            entityName: "BookOrder Detail",
                            currentParentId: id, currentParentName: currentParentName,
                            filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddBookOrder_Detail(BookOrder_Detail model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditBookOrder_Detail(BookOrder_Detail model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteBookOrder_Detail(int id)
        {
            return DeleteModel(id);
        }

        #endregion BookOrder Detail
        #endregion BookOrder
        #endregion

        #region Student_Prize
        public ActionResult ManageAcademic_Student_Prize(int? page, int id = 0)
        {
            IEnumerable<Prize> list = null;
            var currentParentId = 0;
            var currentParentName = "";
            if (id != 0)
            {
                var current_student = IoCConfig.Service<IStudentService>().FindById(id);
                if (current_student == null || !current_student.IsValid())
                    return RedirectToError();

                list = current_student.Prizes.Valid();
                currentParentId = current_student.Id;
                currentParentName = current_student.Student_FullName;
            }

            return ManageModel(
                new Prize
                {
                    Prize_Date = DateTime.Now,
                    Prize_Type = (int)PrizeTypeEnum.Semester
                }, page,
                list: list,
                currentParentId: currentParentId,
                currentParentName: currentParentName,
                filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddPrize(Prize model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditPrize(Prize model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeletePrize(int id)
        {
            return DeleteModel(id);
        }
        #endregion Student_Prize
        #endregion Student > Student Data

        #endregion Student

        //CH, AH, AAE, SRO, CS
        #region Faculty

        #region [XTAuthorizeAcademic] Manage Faculty
        private IEnumerable<Faculty> GetFaculties()
        {
            return IoCConfig.Service<IFacultyService>().FindAllValid().OrderBy(fc => fc.FC_Name);
        }

        public ActionResult ManageAcademic_Faculty(int? page)
        {
            return ManageModel(new Faculty
            {
                FC_Birthday = DateTime.Today,
                FC_CMND_NgayCap = DateTime.Today
            }, page, modal_size: "modal-lg",
            list: GetFaculties(),
            breadcrumbpartial_name: "_partial_BreadCrumb_Faculty",
            filterSearch: SearchModelEnum.ByOthers);
        }

        [HttpPost]
        public ActionResult FilterFaculty(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int Company_Id = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var items = GetFaculties();
            if (!string.IsNullOrEmpty(Model_Name))
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();
                items = items.Where(c => c.FC_Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }
            //others
            if (Company_Id != 0)
            {
                items = items.Where(fc => fc.Company_Id == Company_Id);
            }

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddFaculty(Faculty model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditFaculty(Faculty model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteFaculty(int id)
        {
            var current = IoCConfig.Service<IFacultyService>().FindById(id);
            if (IsValidModel(current) && current.HasChildren())
            {
                return Error("FC này có lớp học! Vui lòng xóa lớp học trước khi xóa FC");
            }
            return DeleteModel(id);
        }
        #endregion Faculty

        #region [XTAuthorizeAcademic] Faculty Skill (Faculty_Module)
        public ActionResult ManageAcademic_Faculty_Skills(int? page, int id)
        {
            var faculty = IoCConfig.Service<IFacultyService>().FindById(id);
            if (faculty == null || !faculty.IsValid())
                return RedirectToError();

            //return ManageModel(new Faculty_Module { Faculty_Id = id }, null,
            //    hasFilter: false,
            //    list: faculty.Faculty_Modules, entityName: "Skill");
            return ManageModel(new Faculty_Module { Faculty_Id = id }, page,
                list: faculty.Modules_List, entityName: "Skill",
                currentParentName: faculty.FC_Name,
                filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult AddFaculty_Module(Faculty_Module model)
        {
            return AddModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult EditFaculty_Module(Faculty_Module model)
        {
            return EditModel(model);
        }

        [HttpPost]
        [XTAuthorizeAcademicExecute]
        public ActionResult DeleteFaculty_Module(int id)
        {
            return DeleteModel(id);
        }
        #endregion Faculty Skill (Faculty_Module)

        #region [XTAuthorizeMod] ImportFaculty
        [HttpPost]
        [XTAuthorizeMod]
        public ActionResult ImportFaculty(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                //////////////////////////////////////////////////////
                //////////////////////////////////////////////////////
                //Constant Variables for importing
                //var CURRENT_COMPANY = 3;
                //////////////////////////////////////////////////////
                //////////////////////////////////////////////////////

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int row = 2; row <= noOfRow; row++)
                    {
                        try
                        {
                            var service = IoCConfig.Service<IFacultyService>();
                            var item = new Faculty();

                            //1: Firstname
                            //2: Lastname
                            item.FC_Name = GetCellValue_String(workSheet, row, 3);//Fullname
                            item.FC_Nickname = GetCellValue_String(workSheet, row, 4);//FC ID/Nickname
                            item.FC_Birthday = GetCellValue_DateTime_Full(workSheet, row, 5, format: "dd/MM/yyyy");
                            //6: Skill
                            var skill_row = 6;
                            item.FC_Phone = GetCellValue_String(workSheet, row, skill_row + 1);
                            item.FC_Email = GetCellValue_String(workSheet, row, skill_row + 2);
                            item.Company_Id = CURRENT_COMPANY;
                            item.FC_CMND = "";
                            item.FC_CMND_NgayCap = DateTime.Now;

                            //Skils
                            //var skills = GetCellValue_String(workSheet, row, 6);
                            //if (!string.IsNullOrWhiteSpace(skills))
                            //{
                            //    foreach (var _skill in skills.Split(",".ToCharArray()))
                            //    {
                            //        var skill = _skill.Trim().ToLower();
                            //        var module = IoCConfig.Service<IModuleService>()
                            //            .FindValidByCriteria(m => m.Module_Code.ToLower() == skill);
                            //        if (IsValidModel(module))
                            //        {
                            //            item.Faculty_Modules.Add(new Faculty_Module { 
                            //                Module_Id = module.Id,
                            //                Created_Date = DateTime.Now
                            //            });
                            //        }
                            //    }
                            //}

                            //check exist
                            var existed = IoCConfig.Service<IFacultyService>()
                                .FindValidByCriteria(old => 
                                    old.Company_Id == CURRENT_COMPANY &&
                                    old.FC_Nickname != null && item.FC_Nickname != null && 
                                    old.FC_Nickname.ToLower() == item.FC_Nickname.ToLower());
                            if (!IsValidModel(existed))
                            {
                                IoCConfig.Service<IFacultyService>().Add(item);
                            }
                            else
                            {
                                existed.FC_Name = item.FC_Name;
                                existed.FC_Birthday = item.FC_Birthday;
                                existed.FC_Phone = item.FC_Phone;
                                existed.FC_Email = item.FC_Email;
                                existed.FC_CMND_NgayCap = item.FC_CMND_NgayCap;

                                IoCConfig.Service<IFacultyService>().Update(existed);
                            }
                        }
                        catch (Exception ex)
                        {
                            return Error(ex.Message + " - " + row);
                        }
                    }

                    return MyContent("Upload successfully!");
                }
            }

            return Error();
        }
        #endregion
        #endregion Faculty

        //CH, AH, AAE, SRO, CS
        #region Student Profile
        public ActionResult StudentDetail(int id, string tab = "AcademicStatus")
        {
            var current_student = IoCConfig.Service<IStudentService>().FindById(id);
            if (current_student == null || !current_student.IsValid())
                return RedirectToError();

            ViewBag.Tab = tab;
            return View(current_student);
        }

        #region Export Marks
        private void SetCellValueAndFont(ExcelRange cell, object value,
            ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left,
            bool isMerge = true, bool isBold = false, bool isItalic = false, bool isBorder = false,
            float size = 12)
        {
            cell.Merge = isMerge;
            cell.Style.HorizontalAlignment = align;
            if (isBorder)
            {
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            var dataFont = cell.Style.Font;
            dataFont.SetFromFont(new Font("Calibri", size)); //Do this first
            dataFont.Bold = isBold;
            dataFont.Italic = isItalic;

            cell.Value = value;
        }

        private void SetCellValueAndFontAndBorder(ExcelRange cell, object value,
            ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left,
            bool isMerge = true, bool isBold = false, bool isItalic = false, bool isBorder = true,
            float size = 12)
        {
            SetCellValueAndFont(cell, value, align, isMerge, isBold, isItalic, isBorder, size);
        }

        private void ExportMarks_InitSheet(ExcelWorksheet ws, Student student, int sem = 1)
        {
            var row = 5;
            SetCellValueAndFont(ws.Cells[row, 1, row, 10], "STUDY PERFORMANCE",
                ExcelHorizontalAlignment.Center, isBold: true, size: 24);
            row += 3;
            SetCellValueAndFontAndBorder(ws.Cells[row, 2, row, 3], "Student's Name", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 4, row, 8], student.Student_FullName);
            row++;
            SetCellValueAndFontAndBorder(ws.Cells[row, 2, row, 3], "Batch", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 4, row, 8], student.Class.Class_Name);
            row++;
            SetCellValueAndFontAndBorder(ws.Cells[row, 2, row, 3], "Roll Number", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 4, row, 8], student.GetEnrollNumber());
            row++;
            SetCellValueAndFontAndBorder(ws.Cells[row, 2, row, 3], "Course", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 4, row, 8], student.CourseFamily.CourseFamily_FullName);
            row++;
            SetCellValueAndFontAndBorder(ws.Cells[row, 2, row, 3], "Semester", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 4, row, 8], sem);
            row++;
        }

        public FileStreamResult ExportMarks(int id = 0, int sem = 1)
        {
            var student = IoCConfig.Service<IStudentService>().FindById(id);
            if (!IsValidModel(student))
            {
                return null;
            }

            //open file
            ExcelPackage pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("Sem " + sem);
            ExportMarks_InitSheet(ws, student, sem);

            var row = 14;

            //student info
            SetCellValueAndFontAndBorder(ws.Cells[row, 1, row, 5], "Module", ExcelHorizontalAlignment.Center, isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 6], "Mark", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 7], "Max Mark", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 8], "Rate", isBold: true);
            SetCellValueAndFontAndBorder(ws.Cells[row, 9], "Status", isBold: true);
            row++;

            //student marks
            var marks = Helper.GetStudentSemesterMarks(student, sem);
            foreach (var mark in marks)
            {
                SetCellValueAndFontAndBorder(ws.Cells[row, 1, row, 5], mark.Mark_Name);
                SetCellValueAndFontAndBorder(ws.Cells[row, 6], mark.Mark, ExcelHorizontalAlignment.Center);
                SetCellValueAndFontAndBorder(ws.Cells[row, 7], mark.Max_Mark, ExcelHorizontalAlignment.Center);
                SetCellValueAndFontAndBorder(ws.Cells[row, 8], mark.Rate, ExcelHorizontalAlignment.Center);
                SetCellValueAndFontAndBorder(ws.Cells[row, 9], mark.Rate_Status, ExcelHorizontalAlignment.Center);
                row++;
            }

            //note
            //Evaluation Details
            //1. Computation of total Marks for Practical ( A )
            //IM = (A1+A2+A3+A4)/4
            //2. Computation of total Marks for Theory ( EM )
            //EM = (ME1+ME2+ME3+ME4)/4
            //3. Computation of Overall Weighted Marks (0)
            //O = 25% of  IM + 50% of EM +  25% of eProject

            row++;
            SetCellValueAndFont(ws.Cells[row, 1, row, 5], "Evaluation Details", ExcelHorizontalAlignment.Center, isBold: true); row++;
            SetCellValueAndFont(ws.Cells[row, 1, row, 5], "1. Computation of total Marks for Practical ( A )", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1, row, 5], "IM = (A1+A2+A3+A4)/4", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1, row, 5], "2. Computation of total Marks for Theory ( EM )", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1, row, 5], "EM = (ME1+ME2+ME3+ME4)/4", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1, row, 5], "3. Computation of Overall Weighted Marks (O)", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1, row, 5], "O = 25% of  IM + 50% of EM +  25% of eProject", ExcelHorizontalAlignment.Center); row++;

            //signature
            row++;
            var left_row = row;
            SetCellValueAndFont(ws.Cells[row, 7, row, 9], DateTime.Today.ToString("dd MMMM, yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 7, row, 9], "Deputy Academic Head", ExcelHorizontalAlignment.Center); row++;

            row = left_row;
            SetCellValueAndFont(ws.Cells[row, 1], "Note", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1], "EM: Theory", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1], "IM: Practice", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1], "Fail: Chưa đạt", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1], "Pass: Đạt", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1], "Credit: Khá", ExcelHorizontalAlignment.Center); row++;
            SetCellValueAndFont(ws.Cells[row, 1], "Distinction: Giỏi", ExcelHorizontalAlignment.Center); row++;

            //close file
            var stream = new MemoryStream(pck.GetAsByteArray());
            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "StudentMarks_" + student.GetEnrollNumber() + "_Sem" + sem + ".xlsx");
        }
        #endregion Export Marks
        #endregion

        //CH, AH, AAE, SRO, CS
        #region Faculty Profile
        public ActionResult FacultyDetail(int id, string tab = "AcademicStatus")
        {
            var current_user = IoCConfig.Service<IFacultyService>().FindById(id);
            if (current_user == null || !current_user.IsValid())
                return RedirectToError();

            ViewBag.Tab = tab;
            return View(current_user);
        }
        #endregion

        //CH, AH, AAE, SRO, CS
        
    }
}