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
        #region IsAuthenticated Extend
        public static bool IsCenterHead
        {
            get
            {
                return Is(RoleTypeEnum.CH);
            }
        }

        public static bool IsAcademicHead
        {
            get
            {
                return IsCenterHead || Is(RoleTypeEnum.AH);
            }
        }

        public static bool IsAcademicExecutive
        {
            get
            {
                return IsAcademicHead || Is(RoleTypeEnum.DAH) || Is(RoleTypeEnum.AAE);
            }
        }

        public static bool IsAcademic
        {
            get
            {
                return IsAcademicExecutive || Is(RoleTypeEnum.SRO);
            }
        }

        public static bool IsFinanceExecutive
        {
            get
            {
                return IsAcademicHead || Is(RoleTypeEnum.LBKS);
            }
        }

        #endregion
    }
}