using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface ICompany_TypeService : IService<Company_Type, Int32> { }
    public interface IClass_ModuleService : IService<Class_Module, Int32> { }
    public interface IClass_Module_DayService : IService<Class_Module_Day, Int32> { }
    public interface IClass_Module_StudentExamService : IService<Class_Module_StudentExam, Int32> { }
    public interface IBookOrder_DetailService : IService<BookOrder_Detail, Int32> { }
    public interface IStudent_FeePlan_InstallmentService : IService<Student_FeePlan_Installment, Int32> { }
    public interface IFeePlan_DetailService : IService<FeePlan_Detail, Int32> { }
    public interface IPrizeService : IService<Prize, Int32> { }
    public interface IClass_Module_Day_StudentService : IService<Class_Module_Day_Student, Int32> { }
    public interface IStudent_ClassHistoryService : IService<Student_ClassHistory, Int32> { }
    public interface IRole_TypeService : IService<Role_Type, Int32> { }
    public interface IUser_TypeService : IService<User_Type, Int32> { }
    public interface IUser_ProfileService : IService<User_Profile, Int32> { }
    public interface IUser_CompanyService : IService<User_Company, Int32> { }
    public interface ITimekeeperService : IService<Timekeeper, Int32> {
        Timekeeper CheckExistTimekeeper(Timekeeper current, int maxWaitingMinutes);
    }
}
