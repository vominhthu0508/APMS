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
    public class AuthenticationCookie
    {
        public static string USER_COOKIE = "UserData";

        private static string GetDeviceId()
        {
            //var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            //ManagementObjectCollection mbsList = mbs.Get();
            //string id = "";
            //foreach (ManagementObject mo in mbsList)
            //{
            //    id = mo["ProcessorId"].ToString();
            //    break;
            //}
            //return id;

            return "";
        }

        public static bool IsAuthenticated
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[USER_COOKIE] != null)
                {
                    System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                    string userData = browser.Id + GetDeviceId();
                    var cookie = HttpContext.Current.Request.Cookies[USER_COOKIE];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    if (userData.Equals(ticket.UserData))
                    {
                        var userEmail = ticket.Name;
                        var service = IoCConfig.Service<IAccountService>();

                        var account = service.FindValidByCriteria(acc => acc.Account_Email == userEmail
                                                    && acc.IsUser());

                        if (account != null)
                        {
                            AuthenticationSession.SetAuthentication(account);
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public static void SetAuthentication(Account acc)
        {
            //cookie
            if (acc.IsUser())
            {
                int cookietimeout = 525600;//minutes
                System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                string userData = browser.Id + GetDeviceId();//UserData gồm browser id và Device Id 
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    acc.Account_Email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(cookietimeout),
                    true,
                    userData,
                    "/");
                string encrypt = FormsAuthentication.Encrypt(ticket);//secure
                HttpCookie cookie = new HttpCookie(USER_COOKIE, encrypt);
                cookie.Secure = false;
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddMinutes(cookietimeout);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void Logout()
        {
            //Delete cookie
            HttpCookie myCookie = new HttpCookie(USER_COOKIE);
            myCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
    }
}