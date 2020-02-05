using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using XT.Model;
using XT.BusinessService;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace XT.Web.External
{
    public partial class AuthenticationManager
    {
        #region Employee
        /// <summary>
        /// IsMod
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CanManageEmployee()
        {
            return IsCenterHead;
        }
        #endregion

        #region Option
        public static bool CanManageAcademic()
        {
            return IsAcademicExecutive;
        }

        public static bool CanManageFinance()
        {
            return IsFinanceExecutive;
        }

        public static bool CanManageData()
        {
            return CanManageAcademic() || CanManageFinance();
        }

        public static bool CanMakeAttendance()
        {
            return IsAcademic;
        }
        #endregion

        #region Timekeeper
        public static bool CanViewEmployeeTimekeeperData(int user_Id)
        {
            if (user_Id == User_Profile_Id || CanManageEmployee())
                return true;

            var user = IoCConfig.Service<IUser_ProfileService>().FindById(user_Id);
            if (user != null)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}