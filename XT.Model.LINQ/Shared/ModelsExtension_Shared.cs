using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

////////////////////////////////////////////////////////////////////////////////////
//This file is for common extensions used by both EF and LINQ
//Không thể move file này ra khỏi project EF và LINQ vì sẽ conflict với dll Model
//Partial Class phải chung dll với Class gốc

namespace XT.Model
{
    public partial class Account
    {
        public bool IsInactive()
        {
            return this.Status == (int)EntityStatus.UnPublic;
        }

        public bool IsUser()
        {
            return this.GetRoleId() == (int)RoleTypeEnum.User;
        }

        public bool IsAdmin()
        {
            return this.GetRoleId() == (int)RoleTypeEnum.Admin;
        }

        public int GetRoleId()
        {
            return this.User_Profile.Role_Type_Id;
        }

        public void UpdateProfile(IRegisterEntity<Int32> profile)
        {
            this.Account_Name = profile.Name;
            this.Account_Email = profile.Email;
            this.Account_Avatar = profile.Avatar;
        }
    }

    public partial class User_Profile
    {
        #region Global Properties
        /// <summary>
        /// NV
        /// </summary>
        public static string USER_PREFIX_ID_STRING = "NV";//will be replaced in Global.asax.cs
        //User_Profile.USER_PREFIX_ID_STRING = AppSettings.Company_ID_Prefix;
        /// <summary>
        /// 00000
        /// </summary>
        public static string USER_PREFIX_ID_FORMAT = "00000";

        public string User_Full_Id
        {
            get
            {
                return USER_PREFIX_ID_STRING + this.Id.ToString(USER_PREFIX_ID_FORMAT);
            }
        }

        public string User_Profile_Full_Name_Id
        {
            get
            {
                return this.User_Profile_Name + " (" + User_Full_Id + ")";
            }
        }

        public string GetGender()
        {
            var gender = "Nam";
            if (this.User_Profile_Gender.HasValue && this.User_Profile_Gender.Value == (int)GenderEnum.Female)
            {
                gender = "Nữ";
            }

            return gender;
        }

        public string GetPhone()
        {
            return this.User_Profile_Phone != null ? this.User_Profile_Phone : "";
        }

        public string GetFacebook()
        {
            if (!string.IsNullOrEmpty(this.User_Profile_Facebook))
            {
                if (this.User_Profile_Facebook.Contains("facebook.com"))
                    return this.User_Profile_Facebook;
                return "https://www.facebook.com/" + this.User_Profile_Facebook.TrimStart('/');
            }

            return "#";
        }
        #endregion

        public void SetRole(RoleTypeEnum type)
        {
            this.Role_Type_Id = (int)type;
        }

        public IEnumerable<User_Company> User_Companies_List
        {
            get
            {
                return this.User_Companies.Valid();
            }
        }

        public IEnumerable<Company> Companies_List
        {
            get
            {
                return User_Companies_List.Select(uc => uc.Company);
            }
        }

        public bool HasCompany(int Company_Id)
        {
            return User_Companies_List.Any(uc => uc.Company_Id == Company_Id);
        }
    }

    public partial class CourseFamily
    {
        //public string CourseFamily_FullName()
        //{
        //    return this.Course.Course_Name + " " + this.CourseFamily_Name;
        //}

        public string CourseFamily_FullName
        {
            get {
                return this.Course.Course_Code + " " + this.CourseFamily_Name;
            }
        }

        public IEnumerable<Module> Modules_List
        {
            get
            {
                return this.Modules.Valid();
            }
        }

        public IEnumerable<Module> Portal_Modules_List
        {
            get
            {
                return this.Modules_List.Where(m => m.IsPortal && m.Module_Exam_Type == (int)ModuleExamTypeEnum.Test);
            }
        }

        public IEnumerable<Module> Extra_Modules_List
        {
            get
            {
                return this.Modules_List.Where(m => m.IsExtra);
            }
        }
    }

    public partial class Class
    {
        public string Class_Name_Company {
            get {
                return this.Class_Name + "(" + this.Company.Company_Name_Abbrev + ")";
            }
        }

        public IEnumerable<Student> Students_List {
            get {
                return this.Students.Valid();
            }
        }

        public IEnumerable<Module> Portal_Modules_List
        {
            get
            {
                return this.CourseFamily.Portal_Modules_List;
            }
        }

        public void GenerateScheduleForModule(Module module)
        {
            
        }
    }

    public partial class Faculty
    {
        public IEnumerable<Faculty_Module> Modules_List
        {
            get
            {
                return this.Faculty_Modules.Valid();
            }
        }

        /// <summary>
        /// Dùng cho delete - check children before delete
        /// </summary>
        /// <returns></returns>
        public override bool HasChildren()
        {
            return this.Class_Modules.Valid().Count() > 0;
        }

        public bool HasModule(Module module)
        {
            return this.Modules_List.Any(m => m.Id == module.Id);
        }
    }

    public partial class Student
    {
        public string Student_FullName
        {
            get
            {
                return Student_LastName + " " + Student_FirstName;
            }
        }

        public string FirstClass
        {
            get
            {
                var cls = this.Student_ClassHistories.Valid().FirstOrDefault();
                if (cls != null)
                    return cls.Class.Class_Name;

                return this.Class.Class_Name;
            }
        }

        public IEnumerable<Class_Module_StudentExam> Exam_Modules_List
        {
            get
            {
                //return this.GetModules();
                return this.Class_Module_StudentExams.Valid();
            }
        }

        public IEnumerable<Class_Module_StudentExam> Exam_Modules_Guest_List
        {
            get
            {
                //return this.GetModules_Guest();
                return Exam_Modules_List.Where(s => s.Student_Status == (int)StudentClassModuleStatusEnum.Guest);
            }
        }

        public IEnumerable<Module> Portal_Modules_List
        {
            get
            {
                //return this.Class.Portal_Modules_List;
                var modules = this.CourseFamily.Portal_Modules_List;
                if (modules.Count() == 0)//CIM,DIM
                {
                    if (this.CourseFamily.Course.Parent_Course != null)
                    {
                        var parent_course = this.CourseFamily.Course.Parent_Course.CourseFamilies.Valid().FirstOrDefault(cf => cf.CourseFamily_Name == this.CourseFamily.CourseFamily_Name);
                        if (parent_course != null)
                        {
                            modules = parent_course.Portal_Modules_List;
                        }
                    }
                }

                return modules;
            }
        }

        public IEnumerable<Module> Extra_Modules_List
        {
            get
            {
                return this.Exam_Modules_List.Where(ex => ex.Class_Module.Module.IsExtra).Select(ex => ex.Class_Module.Module);
            }
        }

        public IEnumerable<Prize> Prizes_List
        {
            get
            {
                return this.Prizes.Valid();
            }
        }

        public IEnumerable<Student_FeePlan> Student_FeePlan_List
        {
            get
            {
                return this.Student_FeePlans.Valid();
            }
        }

        public IEnumerable<Student_FeePlan_Installment> Student_FeePlan_Installments_List
        {
            get
            {
                return this.Student_FeePlan_List.SelectMany(f => f.Student_FeePlan_Installments).Valid();
            }
        }

        public IEnumerable<Student_AcademicStatus> Student_AcademicStatuses_List
        {
            get
            {
                return this.Student_AcademicStatuses.Valid();
            }
        }

        public IEnumerable<Student_ClassHistory> Student_ClassHistories_List
        {
            get
            {
                return this.Student_ClassHistories.Valid();
            }
        }

        public int Actual_Course_Fee
        {
            get
            {
                return this.Student_FeePlan_List.Sum(f => f.Actual_Course_Fee);
            }
        }

        public int Remain_Fee
        {
            get
            {
                return this.Student_FeePlan_List.Sum(f => f.Remain_Fee);
            }
        }

        public int Paid_Fee
        {
            get
            {
                return this.Student_FeePlan_List.Sum(f => f.Paid_Fee);
            }
        }

        public int Discount_Amount
        {
            get
            {
                return this.Student_FeePlan_List.Sum(f => f.Discount_Amount);
            }
        }

        public int Nominal_Course_Fee
        {
            get
            {
                return this.Student_FeePlan_List.Sum(f => f.Nominal_Course_Fee);
            }
        }

        public int Remain_FeeUntilNow
        {
            get
            {
                return this.Student_FeePlan_List.Sum(f => f.Remain_FeeUntilNow);
            }
        }

        public string GetPhone()
        {
            return this.Student_MobilePhone != null ? this.Student_MobilePhone : "";
        }

        public string GetEnrollNumber()
        {
            return this.Student_EnrollNumber != null ? this.Student_EnrollNumber : "";
        }

        //public IEnumerable<Class_Module_StudentExam> GetModules()
        //{
        //    return this.Class_Module_StudentExams.Valid();
        //}

        //public IEnumerable<Class_Module_StudentExam> GetModules_Guest()
        //{
        //    return this.GetModules().Where(s => s.Student_Status == (int)StudentClassModuleStatusEnum.Guest);
        //}
    }

    public partial class Student_FeePlan
    {
        public int Paid_Fee
        {
            get
            {
                return this.Student_FeePlan_Installments.Valid()
                    .Where(i => i.Installment_Status == (int)InstallmentStatusEnum.Finished)
                    .Sum(i => i.Amount_Actual);
            }
        }

        public int Paid_FeeUntilNow
        {
            get
            {
                return this.Student_FeePlan_Installments.Valid()
                    .Where(i => i.Date_Planning <= DateTime.Today && i.Installment_Status == (int)InstallmentStatusEnum.Finished)
                    .Sum(i => i.Amount_Actual);
            }
        }

        public int Remain_Fee
        {
            get
            {
                return this.Actual_Course_Fee - this.Paid_Fee;
            }
        }

        public int Remain_FeeUntilNow
        {
            get
            {
                return Remain_FeeUntilDate(DateTime.Today);
            }
        }

        public bool IsFinishAllFees
        {
            get
            {
                return this.Remain_Fee == 0;
            }
        }

        public IEnumerable<Student_FeePlan_Installment> Student_FeePlan_Installments_List
        {
            get
            {
                return this.Student_FeePlan_Installments.Valid();
            }
        }

        public IEnumerable<Student_FeePlan_Installment> Student_Due_Installments_List
        {
            get
            {
                return this.Student_FeePlan_Installments_List.Where(i => i.Installment_Status == (int)InstallmentStatusEnum.Planned);
            }
        }

        public void SetupFeePlanInstallments(FeePlan feeplan)
        {
            var feeplans = feeplan.FeePlan_Details.Valid().OrderBy(f => f.FeePlan_Index);
            var feeplans_count = feeplan.FeePlan_Count;
            var discount_amount = this.Discount_Amount;
            var discount_amount_each = discount_amount / feeplans_count;

            //thêm từng install
            var start_date = this.FeePlan_StartDate;
            var next_date = new DateTime(start_date.Year, start_date.Month, 1);
            if (start_date.Day > 15)
            {
                next_date = next_date.AddMonths(1);
            }
            var first = true;
            var months = feeplan.FeePlan_Months / feeplans_count;
            this.Student_FeePlan_Installments.Clear();
            foreach (var n in feeplans)
            {
                this.Student_FeePlan_Installments.Add(new Student_FeePlan_Installment
                {
                    FeePlan_Detail_Id = n.Id,
                    Date_Planning = first ? start_date : next_date,
                    Amount_Planning = n.FeePlan_Amount - discount_amount_each,
                    Installment_Status = (int)InstallmentStatusEnum.Planned,
                    Status = (int)EntityStatus.Visible
                });

                next_date = next_date.AddMonths(months);
                first = false;
            }
        }

        public int Remain_FeeUntilDate(DateTime date)
        {
            var installs = this.Student_Due_Installments_List//Student_FeePlan_Installments.Valid()
                    .Where(i => i.Date_Planning <= date.Date);
            var plan = installs.Sum(i => i.Amount_Planning);

            var paid = installs
                .Where(i => i.Installment_Status == (int)InstallmentStatusEnum.Finished)
                .Sum(i => i.Amount_Actual);

            return plan - paid;
        }

        public int Remain_FeeUntilDate(DateTime start_Date, DateTime end_Date)
        {
            var ins = this.Student_Due_Installments_List
                .FirstOrDefault(i => start_Date <= i.Date_Planning && i.Date_Planning <= end_Date);

            return ins != null ? ins.Amount_Planning : 0;
        }

        public bool HasDueInstallment(DateTime Start_Date, DateTime End_Date)
        {
            return this.Student_Due_Installments_List.Any(i => Start_Date <= i.Date_Planning && i.Date_Planning <= End_Date);
        }
    }

    public partial class Module
    {
        public string Module_Name_Code {
            get {
                return this.Module_Name + " - " + this.Module_Code;
            }
        }

        public IEnumerable<Faculty> Faculty_List
        {
            get
            {
                return this.Faculty_Modules.Valid().Select(f => f.Faculty).Valid();
            }
        }

        public bool IsPortal
        {
            get
            {
                return this.Module_Portal_Type == (int)ModulePortalTypeEnum.Portal;
            }
        }

        public bool IsExtra
        {
            get
            {
                return this.Module_Portal_Type == (int)ModulePortalTypeEnum.Extra;
            }
        }
    }

    public partial class Class_Module
    {
        public string Class_Name
        {
            get
            {
                return this.Class != null ? this.Class.Class_Name : this.Class_Module_Name;
            }
        }

        public string Class_Module_FullName
        {
            get
            {
                return this.Class_Name + " - " + this.Module.Module_Code;
            }
        }

        public IEnumerable<Class_Module_Day> Class_Module_Days_List
        {
            get
            {
                return this.Class_Module_Days.Valid();
            }
        }

        public float Passrate
        {
            get 
            {
                var all = this.Class_Module_StudentExams.Valid();
                var all_exams = all.Count();
                all_exams = all_exams > 0 ? all_exams : 1;
                var passed_exams = all.Count(e => e.IsExamPass);

                return (float)passed_exams * 100 / all_exams;
            }
        }

        public DateTime GetExactStartDateByClassDay(DateTime start_date, ClassDayEnum day)
        {
            var start_weekday = (int)start_date.DayOfWeek;
            switch (day)
            {
                case ClassDayEnum._2_4_6://weekday chan
                    {
                        if (start_weekday == 6)//Sat->2
                        {
                            start_date = start_date.AddDays(2);
                        }
                        else if (start_weekday % 2 == 0)//Sun->2, 3->4, 5->6
                        {
                            start_date = start_date.AddDays(1);
                        }
                        break;
                    }
                case ClassDayEnum._3_5_7://weekday le
                    {
                        if (start_weekday == 0)//Sun->3
                        {
                            start_date = start_date.AddDays(2);
                        }
                        else if (start_weekday % 2 == 1)//2,4,6
                        {
                            start_date = start_date.AddDays(1);
                        }
                        break;
                    }
            }

            return start_date;
        }

        public DateTime GetNextDateByCurrentDate(DateTime curr_date)
        {
            var next = 2;
            if (curr_date.DayOfWeek == DayOfWeek.Friday || curr_date.DayOfWeek == DayOfWeek.Saturday)
            {
                next = 3;
            }
            if (curr_date.DayOfWeek == DayOfWeek.Sunday)
            {
                next = 7;
            }
            curr_date = curr_date.AddDays(next);

            return curr_date;
        }

        private List<DateTime> GetDatesBySessionStartEnd(DateTime start_date, int duration, ClassDayEnum day)
        {
            start_date = GetExactStartDateByClassDay(start_date, day);

            var dates = new List<DateTime>();
            var curr_date = start_date;
            for (int i = 1; i <= duration; i++)
            {
                dates.Add(curr_date);

                curr_date = GetNextDateByCurrentDate(curr_date);
            }

            return dates;
        }

        private List<DateTime> GetDatesBySessionStartEnd(DateTime start_date, DateTime end_date, ClassDayEnum day)
        {
            start_date = GetExactStartDateByClassDay(start_date, day);

            var dates = new List<DateTime>();
            var curr_date = start_date;
            while (curr_date <= end_date)
            {
                dates.Add(curr_date);

                curr_date = GetNextDateByCurrentDate(curr_date);
            }

            return dates;
        }

        private List<DateTime> GetDatesBySessionStartEnd()
        {
            if (this.Class_Module_DurationByDay > 0)
            {
                return GetDatesBySessionStartEnd(
                            this.Class_Module_Date_Start,
                            this.Class_Module_DurationByDay,
                            (ClassDayEnum)this.Class_Module_Day);
            }
            else
            {
                if (this.Class_Module_Date_End >= this.Class_Module_Date_Start)
                {
                    return GetDatesBySessionStartEnd(
                            this.Class_Module_Date_Start,
                            this.Class_Module_Date_End,
                            (ClassDayEnum)this.Class_Module_Day);
                }
            }

            return null;
        }

        public void AddModuleDay(DateTime date, Class current_class, ClassModuleDayStatusEnum status)
        {
            var day = new Class_Module_Day
            {
                Class_Module_Day_Date = date,
                Class_Module_Day_Status = (int)status,
                Status = (int)EntityStatus.Visible,
                Created_Date = DateTime.Now
            };

            //foreach (var student in current_class.Students)
            //{
            //    day.Class_Module_Day_Students.Add(new Class_Module_Day_Student { 
            //        Student_Id = student.Id,
            //        Attendance_Status = (int)StudentClassModuleAttendanceEnum.P,
            //        Status = (int)EntityStatus.Visible,
            //        Created_Date = DateTime.Now });
            //}

            this.Class_Module_Days.Add(day);
        }

        public void AddModuleDays(ClassModuleDayStatusEnum status, Class current_class = null)
        {
            this.Class_Module_Days.Clear();

            current_class = current_class == null ? this.Class : current_class;
            var dates = GetDatesBySessionStartEnd();
            if (dates != null)
            {
                foreach (var date in dates)
                {
                    this.AddModuleDay(date, current_class, ClassModuleDayStatusEnum.Scheduled);
                }
            }
        }

        public void AddStudents(Class current_class)
        {
            this.Class_Module_StudentExams.Clear();
            foreach (var student in current_class.Students.Where(s => s.Student_Status == (int)StudentAcademicStatusEnum.Studying))
            {
                var exam = new Class_Module_StudentExam
                {
                    Student_Id = student.Id,
                    Student_Status = (int)StudentClassModuleStatusEnum.Official,
                    Exam_Count = 1,
                    Status = (int)EntityStatus.Visible,
                    Created_Date = DateTime.Now
                };

                this.Class_Module_StudentExams.Add(exam);
            }
        }

        public void GenerateSchedule(Class current_class)
        {
            //add students: Class_Module_StudentExam
            this.AddStudents(current_class);

            //add days: Class_Module_Day
            this.AddModuleDays(ClassModuleDayStatusEnum.Scheduled, current_class);
        }

        public DateTime GetClassModuleEndDate()
        {
            var dates = GetDatesBySessionStartEnd();
            if (dates != null)
                return dates.Last();

            return this.Class_Module_Date_Start;
        }
    }

    public partial class Class_Module_StudentExam
    {
        public bool IsExamPass_LT
        {
            get
            {
                if (Class_Module != null)
                {
                    var Module_Max_LT = Class_Module.Module.Module_Max_LT;
                    Module_Max_LT = Module_Max_LT > 0 ? Module_Max_LT : 1;
                    return Mark_LT * 100 / Module_Max_LT >= 40;
                }

                return false;
            }
        }

        public bool IsExamPass_TH
        {
            get
            {
                if (Class_Module != null)
                {
                    var Module_Max_TH = Class_Module.Module.Module_Max_TH;
                    Module_Max_TH = Module_Max_TH > 0 ? Module_Max_TH : 1;
                    return Mark_TH * 100 / Module_Max_TH >= 40;
                    //return Mark_TH * 100 / Class_Module.Module.Module_Max_TH >= 40;
                }

                return false;
            }
        }

        public bool IsExamValid
        {
            get
            {
                return TotalAttendance() >= 40;
            }
        }

        public bool IsExamPass
        {
            get
            {
                return IsExamValid && IsExamPass_LT && IsExamPass_TH;
            }
        }

        public string ExamPassStatus
        {
            get
            {
                return IsExamPass ? "PASSED" : "FAILED";
            }
        }

        public int TotalAttendance()
        {
            if (Class_Module != null)
            {
                var days = this.Class_Module.Class_Module_Days;
                float day_count = days.Count;
                if (day_count == 0)
                    return 0;
                float day_atten_total = 0;
                foreach (var day in days)
                {
                    float day_atten = 0;
                    var student = day.Class_Module_Day_Students.Valid().FirstOrDefault(s => s.Student_Id == this.Student_Id);
                    if (student != null)
                    {
                        if (student.Attendance_Status == (int)StudentClassModuleAttendanceEnum.P)
                            day_atten = 1;
                        else if (student.Attendance_Status == (int)StudentClassModuleAttendanceEnum.PA)
                            day_atten = 0.5F;
                        day_atten_total += day_atten;
                    }
                }

                return (int)Math.Round((day_atten_total * 100) / day_count);
            }

            return 0;
        }
    }

    public partial class Class_Module_Day
    {
        public IEnumerable<Class_Module_Day_Student> Class_Module_Day_Students_List
        {
            get
            {
                return this.Class_Module_Day_Students.Valid();
            }
        }
    }

    public partial class BookOrder
    {
        public IEnumerable<BookOrder_Detail> BookOrder_Details_List
        {
            get
            {
                return this.BookOrder_Details.Valid();
            }
        }
    }
}
