using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.Model
{
    #region FrameworkEnums
    public enum LanguageEnum
    {
        vi = 1,
        en = 2
    }

    public enum RoleTypeEnum
    {
        Admin = 1,
        Mod = 2,
        CH = 3,
        AH = 4,
        DAH = 5,
        AAE = 6,
        SRO = 7,
        LBKS = 8,
        FC = 9,
        HV = 10,
        User = 11
    }

    public enum UserTypeEnum
    { 
        CH = 1,
        AH = 2,
        AAE = 3,
        CS = 4,
        SRO = 5,
        User = 6
    }

    public enum GenderEnum
    {
        Male = 1,
        Female = 0
    }

    public enum SearchModelEnum
    { 
        None = 0,
        ByName = 1,
        ByNameWithParent = 2,
        ByOthers = 3
    }
    #endregion FrameworkEnums

    #region Company
    public enum CompanyTypeEnum
    {
        Arena = 1,
        Aptech = 2,
    }
    #endregion

    #region Module
    public enum ModuleTypeEnum
    {
        LT = 1,
        TH = 2,
        LT_TH = 3
    }

    public enum ModulePortalTypeEnum
    { 
        Portal = 1,
        Extra = 2,
        //PortalWithoutMark = 3
    }

    public enum ModuleExamTypeEnum
    {
        No_Test = 0,
        Test = 1
    }

    public enum SemesterEnum
    {
        Sem_1 = 1,
        Sem_2 = 2,
        Sem_3 = 3,
        Sem_4 = 4
    }
    #endregion

    #region Faculty
    public enum FacultyTypeEnum
    {
        FullTime = 1,
        PartTime = 2,
    }
    #endregion

    #region Class
    public enum ClassDayEnum
    {
        //MWF = 1,
        //TTS = 2,
        _2_4_6 = 1,
        _3_5_7 = 2,
        Sunday = 3
    }

    /// <summary>
    /// Trạng thái học của lớp
    /// </summary>
    public enum ClassStatusEnum
    {
        Scheduled = 0,
        Studying = 1,
        Finished = 2,
    }

    public enum ClassModuleStatusEnum
    {
        Scheduled = 0,
        Studying = 1,
    }

    public enum ClassModuleDayStatusEnum
    {
        Scheduled = 0,
        Studying = 1,
        FC_Off = 2,
        Center_Off = 3,
        Test = 4
    }

    public enum ClassModuleStudentExamEnum
    { 
        Relearn = -1,
        Valid = 1
    }
    #endregion

    #region Student
    public enum StudentAcademicStatusEnum
    {
        Studying = 1,
        Delay = 2,
        Dropout = 3,
        Note = 4,
        Upgrade = 5,
        Transfer = 7,
        Finished = 100
    }

    public enum StudentClassModuleAttendanceEnum
    {
        P = 1,
        A = 2,
        PA = 3,
    }

    public enum StudentClassModuleStatusEnum
    {
        Official = 1,
        Guest = 2
    }
    #endregion

    #region BookOrder
    public enum IndentStatusEnum
    {
        Pending = 0,
        Approved = 1
    }
    #endregion

    #region Prize
    public enum PrizeTypeEnum
    {
        Module = 1,
        Semester = 2
    }
    #endregion

    #region FeePlan
    public enum FeePlanTypeEnum
    {
        Installment = 1,
        Lumpsum = 2,
        Upgrade = 3
    }

    public enum InstallmentStatusEnum
    {
        Planned = 0,
        Finished = 1,
        ExtendWait = 2,
        ExtendOK = 3,
        Deposit = 4
    }

    public enum DueStatusEnum
    { 
        NoDue = 1,
        HasDue = 2
    }
    #endregion

    #region Timekeeper
    public enum TimekeeperInOutEnum
    {
        In = 1,
        Out = -1
    }

    #endregion
}
