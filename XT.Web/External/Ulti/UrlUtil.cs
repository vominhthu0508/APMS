using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XT.Model;
using XT.BusinessService;

namespace XT.Web.External
{
    public static class UrlUlti
    {
        //General Functions
        #region General Functions
        //public static string ToSafeUrl(this string s)
        //{
        //    s = s.Convert_Chuoi_Khong_Dau();
        //    s = Regex.Replace(s, @"[^a-zA-Z0-9]", "-");
        //    s = Regex.Replace(s, @"-{2,}", "-");

        //    return s;

        //    //return s.Replace(" - ", "-")
        //    //    .Replace(" ", "-").Replace("--", "-");

        //    //return s.Replace("\\", "-").Replace("/", "_").Replace("+", "-")
        //    //    .Replace(",", "-").Replace(".", "-").Replace("%", "-")
        //    //    .Replace(" ", "-");

        //    //return Regex.Replace(s, @"[^\w\s-]", "-");
        //}

        //public static string Convert_Chuoi_Khong_Dau(this string s)
        //{
        //    Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
        //    string strFormD = s.Normalize(NormalizationForm.FormD);
        //    return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        //}
        #endregion

        #region ISearchableEntity
        public static string GoIndexSearchable(this UrlHelper Url, ISearchableEntity entity, string modelName)
        {
            //var url = Url.GoIndex();

            //var type = GetTypeModelName(modelName);
            //if (type != null)
            //{
            //    //var model = Convert.ChangeType(entity, type);
            //    var model = entity;
            //    var methodName = "GoIndex_" + modelName;
            //    url = typeof(UrlUlti).GetMethod(methodName)
            //        .Invoke(null, new object[] { Url, model }) as string;
            //}

            var model = entity;
            var methodName = "GoIndex_" + modelName;
            var url = typeof(UrlUlti).GetMethod(methodName)
                .Invoke(null, new object[] { Url, model }) as string;

            return url;
        }
        #endregion

        #region Account

        //Register
        ///////////////////////////////////////////////////////////////////////////
        public static string GoAccountRegisterUser(this UrlHelper Url)
        {
            return Url.Action("RegisterUser", "Account");
        }

        public static string GoAccountRegisterSucccess(this UrlHelper Url)
        {
            return Url.Action("RegisterSucccess", "Account");
        }

        //Login
        ///////////////////////////////////////////////////////////////////////////
        public static string GoAccountLogin(this UrlHelper Url)
        {
            return Url.Action("Login", "Account");
        }

        public static string GoAccountLogout(this UrlHelper Url)
        {
            return Url.Action("Logout", "Account");
        }

        public static string GoAccountChangePassword(this UrlHelper Url)
        {
            return Url.Action("ChangePassword", "Account");
        }

        public static string GoAccountActive(this UrlHelper Url)
        {
            return Url.Action("Active", "Account");
        }

        //Change Profile
        ///////////////////////////////////////////////////////////////////////////
        public static string GoAccountRecoverPassword(this UrlHelper Url)
        {
            return Url.Action("RecoverPassword", "Account");
        }

        public static string GoAccountChangeProfileInfo(this UrlHelper Url)
        {
            return Url.Action("ChangeProfileInfo", "Account");
        }

        public static string GoAccountProfile(this UrlHelper Url)
        {
            return Url.Action("Index", "User");
        }
        #endregion

        #region Home
        public static string GoIndex(this UrlHelper Url)
        {
            return Url.Action("Index", "Home");
        }
        #endregion

        #region Admin
        public static string GoAccountAdminFirstPage(this UrlHelper Url)
        {
            return Url.Action("Index", "Admin");
        }
        public static string GoUserProfile(this UrlHelper Url, int user_id = 0)
        {
            return Url.Action("StudentDetail", "ManageAcademic", new { id = user_id });
        }
        public static string GoFCProfile(this UrlHelper Url, int user_id = 0)
        {
            return Url.Action("FacultyDetail", "ManageAcademic", new { id = user_id });
        }
        #endregion
    }
}