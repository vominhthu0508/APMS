using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class Company_TypeService : Service<Company_Type, Int32>, ICompany_TypeService
    {
        public Company_TypeService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Company_Type_Name";
            }
        }
    }
    public class Class_ModuleService : Service<Class_Module, Int32>, IClass_ModuleService
    {
        public Class_ModuleService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Class_Module_Name";
            }
        }
        public IEnumerable<Class_Module> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }
    }
    public class Class_Module_DayService : Service<Class_Module_Day, Int32>, IClass_Module_DayService
    {
        public Class_Module_DayService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Class_Module_Day_Name";
            }
        }

        public override string NameForParent
        {
            get
            {
                return "Class_Module_Id";
            }
        }
    }
    public class Class_Module_StudentExamService : Service<Class_Module_StudentExam, Int32>, IClass_Module_StudentExamService
    {
        public Class_Module_StudentExamService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Class_Module_StudentExam_Name";
            }
        }

        public override string NameForParent
        {
            get
            {
                return "Class_Module_Id";
            }
        }
    }
    public class BookOrder_DetailService : Service<BookOrder_Detail, Int32>, IBookOrder_DetailService
    {
        public BookOrder_DetailService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "BookOrder_Detail_Name";
            }
        }
    }
    public class Student_FeePlan_InstallmentService : Service<Student_FeePlan_Installment, Int32>, IStudent_FeePlan_InstallmentService
    {
        public Student_FeePlan_InstallmentService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Student_FeePlan_Installment_Name";
            }
        }
    }
    public class FeePlan_DetailService : Service<FeePlan_Detail, Int32>, IFeePlan_DetailService
    {
        public FeePlan_DetailService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "FeePlan_Detail_Name";
            }
        }
    }
    public class FeePlanService : Service<FeePlan, Int32>, IFeePlanService
    {
        public FeePlanService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "FeePlan_Name";
            }
        }
        public IEnumerable<FeePlan> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }
    }
    public class PrizeService : Service<Prize, Int32>, IPrizeService
    {
        public PrizeService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Prize_Name";
            }
        }
        public IEnumerable<Prize> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }
    }
    public class Class_Module_Day_StudentService : Service<Class_Module_Day_Student, Int32>, IClass_Module_Day_StudentService
    {
        public Class_Module_Day_StudentService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Class_Module_Day_Student_Name";
            }
        }
    }
    public class Student_ClassHistoryService : Service<Student_ClassHistory, Int32>, IStudent_ClassHistoryService
    {
        public Student_ClassHistoryService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Student_ClassHistory_Name";
            }
        }
    }
    public class ResourceService : Service<Resource, Int32>, IResourceService
    {
        public ResourceService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Resource_Name";
            }
        }
        public IEnumerable<Resource> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }
    }
    public class Role_TypeService : Service<Role_Type, Int32>, IRole_TypeService
    {
        public Role_TypeService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Role_Type_Name";
            }
        }
    }
    public class User_TypeService : Service<User_Type, Int32>, IUser_TypeService
    {
        public User_TypeService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "User_Type_Name";
            }
        }
    }
    public class User_ProfileService : Service<User_Profile, Int32>, IUser_ProfileService
    {
        public User_ProfileService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        //public override string NameForFinding
        //{
        //    get
        //    {
        //        return "User_Profile_Name";
        //    }
        //}
    }
    public class User_CompanyService : Service<User_Company, Int32>, IUser_CompanyService
    {
        public User_CompanyService(IUow uow) : base(uow) { }
        //used for FindByName in Manage
        public override string NameForParent
        {
            get
            {
                return "User_Id";
            }
        }
    }

    public class TimekeeperService : Service<Timekeeper, Int32>, ITimekeeperService
    {
        public TimekeeperService(IUow uow) : base(uow) { }

        public override string NameForParent
        {
            get
            {
                return "User_Id";
            }
        }

        public Timekeeper CheckExistTimekeeper(Timekeeper current, int maxWaitingMinutes)
        {
            return FindByCriteria(u =>
                u.Status == (int)EntityStatus.Visible &&
                u.Id != current.Id &&
                u.User_Id == current.User_Id &&
                (DateTime.Now - u.Checkin_Date).TotalMinutes < maxWaitingMinutes
                );
        }
    }
}
