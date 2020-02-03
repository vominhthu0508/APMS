namespace XT.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class MyDataContext : DbContext
    {
        public MyDataContext()
            : base("name=MyConnectionString")
        {
        }

        public MyDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        //tao cho InitCreate trong Configuration
        public virtual DbSet<User_Type> User_Types { get; set; }
        public virtual DbSet<Role_Type> Role_Types { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<User_Profile> User_Profiles { get; set; }

        //Khong tao vi luc chay dung LINQ
        //public virtual DbSet<BookOrder> BookOrders { get; set; }
        //public virtual DbSet<Class> Classes { get; set; }
        //public virtual DbSet<Class_Module> Class_Modules { get; set; }
        //public virtual DbSet<Class_Module_Day> Class_Module_Days { get; set; }
        //public virtual DbSet<Class_Module_StudentExam> Class_Module_StudentExams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region User_Type
            modelBuilder.Entity<User_Type>()
                .HasMany(e => e.User_Profiles)
                .WithRequired(e => e.User_Type)
                .HasForeignKey(e => e.User_Type_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region User_Type
            modelBuilder.Entity<Role_Type>()
                .HasMany(e => e.User_Profiles)
                .WithRequired(e => e.Role_Type)
                .HasForeignKey(e => e.Role_Type_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Account
            modelBuilder.Entity<Account>()
                .Property(e => e.Account_RecoverPasswordKey)
                .IsUnicode(false);
            #endregion

            #region User_Profile
            modelBuilder.Entity<User_Profile>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.User_Profile)
                .HasForeignKey(e => e.User_Profile_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Profile>()
                .HasMany(e => e.User_Companies)
                .WithRequired(e => e.User_Profile)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);
            #endregion User_Profile

            #region Company_Type
            modelBuilder.Entity<Company_Type>()
                .HasMany(e => e.Companies)
                .WithRequired(e => e.Company_Type)
                .HasForeignKey(e => e.Company_Type_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company_Type>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Company_Type)
                .HasForeignKey(e => e.Company_Type_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company_Type>()
                .HasMany(e => e.FeePlans)
                .WithRequired(e => e.Company_Type)
                .HasForeignKey(e => e.Company_Type_Id)
                .WillCascadeOnDelete(false);
            #endregion Company_Type

            #region Company
            modelBuilder.Entity<Company>()
                .HasMany(e => e.Faculties)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.Company_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Classes)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.Company_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.BookOrders)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.Company_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Resources)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.Company_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.User_Companies)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.Company_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Faculty
            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.Faculty_Modules)
                .WithRequired(e => e.Faculty)
                .HasForeignKey(e => e.Faculty_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Faculty>()
               .HasMany(e => e.Class_Modules)
               .WithRequired(e => e.Faculty)
               .HasForeignKey(e => e.Faculty_Id)
               .WillCascadeOnDelete(false);
            #endregion

            #region Class_Module
            modelBuilder.Entity<Class_Module>()
                .HasMany(e => e.Class_Module_Days)
                .WithRequired(e => e.Class_Module)
                .HasForeignKey(e => e.Class_Module_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class_Module>()
                .HasMany(e => e.Class_Module_StudentExams)
                .WithRequired(e => e.Class_Module)
                .HasForeignKey(e => e.Class_Module_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Class_Module_Day
            modelBuilder.Entity<Class_Module_Day>()
                .HasMany(e => e.Class_Module_Day_Students)
                .WithRequired(e => e.Class_Module_Day)
                .HasForeignKey(e => e.Class_Module_Day_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Class_Module_StudentExam
            modelBuilder.Entity<Class_Module_StudentExam>()
                .HasMany(e => e.Prizes)
                .WithOptional(e => e.Exam)//WithOptional
                .HasForeignKey(e => e.Exam_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Class
            modelBuilder.Entity<Class>()
                .HasMany(e => e.Class_Modules)
                .WithOptional(e => e.Class)
                .HasForeignKey(e => e.Class_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Class)
                .HasForeignKey(e => e.Class_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Student_ClassHistories)
                .WithRequired(e => e.Class)
                .HasForeignKey(e => e.Class_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Student
            modelBuilder.Entity<Student>()
                .HasMany(e => e.Student_AcademicStatuses)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.Student_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Student_FeePlans)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.Student_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Prizes)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.Student_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Student_ClassHistories)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.Student_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Class_Module_StudentExams)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.Student_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Class_Module_Day_Students)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.Student_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.BookOrder_Details)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.Student_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Student_FeePlan
            modelBuilder.Entity<Student_FeePlan>()
                .HasMany(e => e.Student_FeePlan_Installments)
                .WithRequired(e => e.Student_FeePlan)
                .HasForeignKey(e => e.Student_FeePlan_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region BookOrder
            modelBuilder.Entity<BookOrder>()
                .HasMany(e => e.BookOrder_Details)
                .WithRequired(e => e.BookOrder)
                .HasForeignKey(e => e.BookOrder_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Resource
            modelBuilder.Entity<Resource>()
                .HasMany(e => e.Class_Module_LTs)
                .WithRequired(e => e.Resource_LT)
                .HasForeignKey(e => e.Resource_LT_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resource>()
                .HasMany(e => e.Class_Module_THs)
                .WithRequired(e => e.Resource_TH)
                .HasForeignKey(e => e.Resource_TH_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resource>()
                .HasMany(e => e.Class_Module_Exams)
                .WithRequired(e => e.Resource_Exam)
                .HasForeignKey(e => e.Resource_Exam_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Course
            modelBuilder.Entity<Course>()
                .HasMany(e => e.CourseFamilies)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.Course_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region CourseFamily
            modelBuilder.Entity<CourseFamily>()
                .HasMany(e => e.Modules)
                .WithRequired(e => e.CourseFamily)
                .HasForeignKey(e => e.CourseFamily_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseFamily>()
               .HasMany(e => e.Classes)
               .WithRequired(e => e.CourseFamily)
               .HasForeignKey(e => e.CourseFamily_Id)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseFamily>()
               .HasMany(e => e.Students)
               .WithRequired(e => e.CourseFamily)
               .HasForeignKey(e => e.CourseFamily_Id)
               .WillCascadeOnDelete(false);
            #endregion

            #region Module
            modelBuilder.Entity<Module>()
                .HasMany(e => e.Faculty_Modules)
                .WithRequired(e => e.Module)
                .HasForeignKey(e => e.Module_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.Class_Modules)
                .WithRequired(e => e.Module)
                .HasForeignKey(e => e.Module_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region FeePlan
            modelBuilder.Entity<FeePlan>()
                .HasMany(e => e.FeePlan_Details)
                .WithRequired(e => e.FeePlan)
                .HasForeignKey(e => e.FeePlan_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FeePlan>()
                .HasMany(e => e.Student_FeePlans)
                .WithRequired(e => e.FeePlan)
                .HasForeignKey(e => e.FeePlan_Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region FeePlan_Detail
            modelBuilder.Entity<FeePlan_Detail>()
                .HasMany(e => e.Student_FeePlan_Installments)
                .WithRequired(e => e.FeePlan_Detail)
                .HasForeignKey(e => e.FeePlan_Detail_Id)
                .WillCascadeOnDelete(false);
            #endregion
        }

    }
}
