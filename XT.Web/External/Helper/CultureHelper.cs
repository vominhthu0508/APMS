using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using XT.Model;
namespace XT.Web.External
{
    public class CultureHelper
    {
        private const string LANG_ID = "Lang_Id";
        private const string LANG_CODE = "Lang_Code";
        private const string CENTER_ID = "Center_Id";
        //constructor   
        public CultureHelper()
        {
        }
        // Properties  
        public static int Lang_Id
        {
            get
            {
                if (HttpContext.Current.Handler == null ||
                    HttpContext.Current.Request.Cookies[LANG_ID] == null)
                {
                    return (int)LanguageEnum.vi;
                }
                return int.Parse(HttpContext.Current.Request.Cookies[LANG_ID].Value);
            }
            set
            {
                Lang_Code = value == (int)LanguageEnum.vi ? "vi-VN" : "en-GB";
                HttpCookie myCookie = new HttpCookie(LANG_ID);
                myCookie.Value = value.ToString();
                myCookie.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
                ExecuteLanguage();
                // resource lay tu current UI culture 
            }
        }

        public static int Center_Id
        {
            get
            {
                if (HttpContext.Current.Handler == null ||
                    HttpContext.Current.Request.Cookies[CENTER_ID] == null)
                {
                    return 0;
                }
                return int.Parse(HttpContext.Current.Request.Cookies[CENTER_ID].Value);
            }
            set
            {
                HttpCookie myCookie = new HttpCookie(CENTER_ID);
                myCookie.Value = value.ToString();
                myCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public static string Lang_Code { 
            get {
                if (HttpContext.Current.Request.Cookies[LANG_CODE] == null)
                {
                    return "vi-VN";
                }
                return HttpContext.Current.Request.Cookies[LANG_CODE].Value;
            }
            set {
                HttpCookie myCookie = new HttpCookie(LANG_CODE);
                myCookie.Value = value.ToString();
                myCookie.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public static void ExecuteLanguage()
        {
            // calling CultureHelper class properties for setting 
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lang_Code);
        }

        public static void SetLanguage(int id = (int)LanguageEnum.vi)
        {
            Lang_Id = id;
        }

        public static void SetCenter(int id = 0)
        {
            Center_Id = id;
        }
    }

}