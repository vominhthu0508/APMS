using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;

namespace XT.Web.Models
{
    public class LayoutModel
    {
        #region Global Level

        public static IEnumerable<Company_Type> Company_Types()
        {
            return IoCConfig.Service<ICompanyTypeService>().FindAllValid();
        }

        public static IEnumerable<MyDictionary> Company_Types_ToMyDictionary()
        {
            return Company_Types().Select(c => new MyDictionary { Id = c.Id, Name = c.Company_Type_Name });
        }

        public static IEnumerable<Company> Companies()
        {
            return IoCConfig.Service<ICompanyService>().FindAllValid();
        }

        public static IEnumerable<MyDictionary> Companies_ToMyDictionary()
        {
            return Companies().Select(c => new MyDictionary { Id = c.Id, Name = c.Company_Name_Abbrev });
        }

        public static IEnumerable<Company> CompaniesAll(string name = "Center")
        {
            var all = new List<Company>();
            all.Add(new Company { Id = 0, Company_Name_Abbrev = "--" + name + "--" });
            all.AddRange(Companies());
            return all.OrderBy(a => a.Id);
        }

        public static IEnumerable<MyDictionary> CompaniesAll_ToMyDictionary()
        {
            return CompaniesAll().Select(c => new MyDictionary { Id = c.Id, Name = c.Company_Name_Abbrev });
        }

        /// <summary>
        /// FC là tài sản chung
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Faculty> Faculties()
        {
            return IoCConfig.Service<IFacultyService>().FindAllValid();//.FindAllValidByCriteria(f => f.Company_Id == AuthenticationManager.Company_Id);
        }

        public static IEnumerable<MyDictionary> Faculties_ToMyDictionary()
        {
            return Faculties().Select(c => new MyDictionary { Id = c.Id, Name = c.FC_Name });
        }

        public static IEnumerable<Faculty> FacultiesAll(string name = "Faculty")
        {
            var all = new List<Faculty>();
            all.Add(new Faculty { Id = 0, FC_Name = "--" + name + "--" });
            all.AddRange(Faculties());
            return all.OrderBy(a => a.Id);
        }

        /// <summary>
        /// Resource là tài sản chung
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Resource> Resources()
        {
            return IoCConfig.Service<IResourceService>().FindAllValid();//.FindAllValidByCriteria(f => f.Company_Id == AuthenticationManager.Company_Id);
        }

        public static IEnumerable<MyDictionary> Resources_ToMyDictionary()
        {
            return Resources().Select(c => new MyDictionary { Id = c.Id, Name = c.Resource_Name });
        }

        #endregion FindAllValid

        #region Company Type Level
        public static IEnumerable<Course> Courses()
        {
            //return IoCConfig.Service<ICourseService>().FindAllValid();
            return IoCConfig.Service<ICourseService>()
                .FindAllValidByCriteria(c => c.Company_Type_Id == AuthenticationManager.Company_Type_Id);
        }

        public static IEnumerable<MyDictionary> Courses_ToMyDictionary()
        {
            return Courses().Select(c => new MyDictionary { Id = c.Id, Name = c.Course_Code });
        }

        public static IEnumerable<Course> CoursesAll(string name = "Course")
        {
            var all = new List<Course>();
            all.Add(new Course { Id = 0, Course_Code = "--" + name + "--" });
            all.AddRange(Courses());
            return all.OrderBy(a => a.Id);
        }

        public static IEnumerable<MyDictionary> CoursesAll_ToMyDictionary()
        {
            return CoursesAll().Select(c => new MyDictionary { Id = c.Id, Name = c.Course_Code });
        }

        public static IEnumerable<CourseFamily> CourseFamilies()
        {
            //return IoCConfig.Service<ICourseFamilyService>().FindAllValid();
            return IoCConfig.Service<ICourseFamilyService>()
                .FindAllValidByCriteria(c => c.Course.Company_Type_Id == AuthenticationManager.Company_Type_Id)
                .OrderBy(c => c.Course.Course_Code);
        }

        public static IEnumerable<MyDictionary> CourseFamilies_ToMyDictionary()
        {
            return CourseFamilies().Select(c => new MyDictionary { Id = c.Id, Name = c.CourseFamily_FullName });
        }

        public static IEnumerable<CourseFamily> CourseFamiliesAll(string name = "Course Family")
        {
            var all = new List<CourseFamily>();
            all.Add(new CourseFamily { Id = 0, CourseFamily_Name = "--" + name + "--" });
            all.AddRange(CourseFamilies());
            return all.OrderBy(a => a.Id);
        }

        public static IEnumerable<Module> Modules(int CourseFamily_Id = 0)
        {
            //return IoCConfig.Service<IModuleService>().FindAllValid();
            if (CourseFamily_Id != 0)
                return IoCConfig.Service<IModuleService>().FindAllValidByCriteria(m => m.CourseFamily_Id == CourseFamily_Id);
            return IoCConfig.Service<IModuleService>().FindAllValidByCriteria(m => m.CourseFamily.Course.Company_Type_Id == AuthenticationManager.Company_Type_Id);
        }

        public static IEnumerable<MyDictionary> Modules_ToMyDictionary(int CourseFamily_Id = 0)
        {
            return Modules(CourseFamily_Id).Select(c => new MyDictionary { Id = c.Id, Name = c.Module_Code });
        }

        public static IEnumerable<Module> ModulesAll(int CourseFamily_Id = 0, string name = "Module")
        {
            var all = new List<Module>();
            all.Add(new Module { Id = 0, Module_Name = "--" + name + "--" });
            all.AddRange(Modules(CourseFamily_Id));
            return all.OrderBy(a => a.Id);
        }

        public static IEnumerable<FeePlan> FeePlans()
        {
            return IoCConfig.Service<IFeePlanService>()
                .FindAllValidByCriteria(f => f.Company_Type_Id == AuthenticationManager.Company_Type_Id);
        }

        public static IEnumerable<MyDictionary> FeePlans_ToMyDictionary()
        {
            return FeePlans().Select(c => new MyDictionary { Id = c.Id, Name = c.FeePlan_Name });
        }
        #endregion

        #region Company Level
        public static IEnumerable<Class> Classes(int status = -1)
        {
            if (status > -1)
                return IoCConfig.Service<IClassService>().FindAllValidByCriteria(c => c.Class_Studying_Status == status);
            return IoCConfig.Service<IClassService>().FindAllValid();
                //.FindAllValidByCriteria(c => c.Company_Id == AuthenticationManager.Company_Id);
        }

        public static IEnumerable<MyDictionary> Classes_ToMyDictionary(int status = -1)
        {
            return Classes(status).Select(c => new MyDictionary { Id = c.Id, Name = c.Class_Name_Company });
        }

        public static IEnumerable<Class> ClassesAll(int status = -1, string name = "Class")
        {
            var all = new List<Class>();
            all.Add(new Class { Id = 0, Class_Name = "--" + name + "--", CourseFamily_Id = 0 });
            all.AddRange(Classes(status));
            return all.OrderBy(a => a.Id);
        }

        public static IEnumerable<MyDictionary> ClassesAll_ToMyDictionary(int status = -1)
        {
            return ClassesAll(status).Select(c => new MyDictionary { Id = c.Id, Name = c.Class_Name_Company });
        }

        #endregion

        #region GetName
        public static string GetText_ClassDayEnum(ClassDayEnum filter)
        {
            switch (filter)
            {
                case ClassDayEnum._2_4_6:
                    {
                        return "2,4,6";
                    }
                case ClassDayEnum._3_5_7:
                    {
                        return "3,5,7";
                    }
                case ClassDayEnum.Sunday:
                    {
                        return "Sunday";
                    }
            }

            return "";
        }

        public static string GetControllerName()
        {
            return AppSettings.RootPath + "/" + GetCurrentControllerName();
        }

        public static string GetCurrentControllerName()
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
        }

        public static string GetCurrentActionName()
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
        }
        #endregion
    }
}