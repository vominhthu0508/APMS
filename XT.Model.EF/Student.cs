namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student
    {
        public Student()
        {
            Student_AcademicStatuses = new HashSet<Student_AcademicStatus>();
            Student_FeePlans = new HashSet<Student_FeePlan>();
            Prizes = new HashSet<Prize>();
            Student_ClassHistories = new HashSet<Student_ClassHistory>();
            Class_Module_StudentExams = new HashSet<Class_Module_StudentExam>();
            Class_Module_Day_Students = new HashSet<Class_Module_Day_Student>();
            BookOrder_Details = new HashSet<BookOrder_Detail>();
        }

        public int Id { get; set; }

        public int CourseFamily_Id { get; set; }//

        public int Class_Id { get; set; }//

        //Sau nay muon tim FirstClass thi tra trong Student_ClassHistory
        //public int FirstClass_Id { get; set; }

        //Profile
        public string Student_EnrollNumber { get; set; }//
        public string Student_FirstName { get; set; }//
        public string Student_LastName { get; set; }//
        public int Student_Gender { get; set; }//
        public DateTime Student_Birthday { get; set; }//
        public string Student_Avatar { get; set; }//

        //Contact
        public string Student_MobilePhone { get; set; }
        public string Student_HomePhone { get; set; }
        public string Student_ContactPhone { get; set; }
        public string Student_Email { get; set; }
        public string Student_Facebook { get; set; }
        public string Student_Address { get; set; }
        public string Student_ContactAddress { get; set; }
        public string Student_City { get; set; }
        public string Student_District { get; set; }
        public string Student_Sponsor { get; set; }
        public string Student_Sponsor_Relation { get; set; }
        public string Student_Sponsor_Address { get; set; }

        //Application Info
        public DateTime Student_Application_Date { get; set; }//
        public string Student_Application_CS { get; set; }//
        public string Student_Application_Documents { get; set; }//
        /// <summary>
        /// [StudentAcademicStatusEnum]
        /// </summary>
        public int Student_Status { get; set; }//
        public DateTime Student_Status_Date { get; set; }//
        public int Student_Promotion { get; set; }//

        //Education
        public string HighSchool { get; set; }
        public string University { get; set; }
        public string Company { get; set; }
        public string Company_Address { get; set; }
        public string Company_Position { get; set; }
        public string Company_Salary { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual CourseFamily CourseFamily { get; set; }
        public virtual Class Class { get; set; }

        public virtual ICollection<Student_AcademicStatus> Student_AcademicStatuses { get; set; }
        public virtual ICollection<Student_FeePlan> Student_FeePlans { get; set; }
        public virtual ICollection<Prize> Prizes { get; set; }
        public virtual ICollection<Student_ClassHistory> Student_ClassHistories { get; set; }
        public virtual ICollection<Class_Module_StudentExam> Class_Module_StudentExams { get; set; }
        public virtual ICollection<Class_Module_Day_Student> Class_Module_Day_Students { get; set; }
        public virtual ICollection<BookOrder_Detail> BookOrder_Details { get; set; }
    }
}
