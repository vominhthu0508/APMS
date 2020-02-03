using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml.Linq;
using XT.BusinessService;
using XT.Model;

namespace XT.Web.External
{
    public class EmailHelper
    {
        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        //SEND EMAIL

        #region SEND EMAIL
        public static void SendMail(string ToEmail, string CCEmail = "", string Subject = "", string Body = "")
        {
            if (AppSettings.EmailInUse == true)
            {
                //EmailServiceReference.MyServiceSoapClient ws = new EmailServiceReference.MyServiceSoapClient();
                //ws.SendEmailBySESAsync(AppSettings.EmailToken, AppSettings.ContactEmail, ToEmail, Subject, Body);
            }
        }

        /// <summary>
        /// Đăng ký user (gửi email)
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="contract"></param>
        public static void SendMail_RegisterInform(ControllerContext ctx, Account account)
        {
            if (account != null)
            {
                var name = account.Account_Name;
                var email = account.Account_Email;

                string website = AppSettings.Website;

                string subject = "Chào mừng bạn đã đăng ký tài khoản tại " + website;
                string body = RenderViewToString(ctx, "../EmailTemplate/PartialRegisterEmailTemplate_Inform", Tuple.Create(account, name, email));

                string to_email = email;

                //CC Emails
                string cc_email = AppSettings.EmailAdmin;

                SendMail(to_email, cc_email, subject, body);
            }
        }

        public static void SendMail_RegisterActiveSuccess(ControllerContext ctx, Account account)
        {
            if (account != null)
            {
                var name = account.Account_Name;
                var email = account.Account_Email;

                string website = AppSettings.Website;

                string subject = "Đăng ký tài khoản thành công tại " + website;
                string body = RenderViewToString(ctx, "../EmailTemplate/PartialRegisterEmailTemplate_ActiveSuccess", Tuple.Create(account, name, email));

                string to_email = email;

                //CC Emails
                string cc_email = AppSettings.EmailAdmin;

                SendMail(to_email, cc_email, subject, body);
            }
        }

        /// <summary>
        /// Email thông báo cách thức phục hồi mật khẩu
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        public static void SendMail_RecoverPassword(ControllerContext ctx, Account account)
        {
            if (account != null)
            {
                var name = account.Account_Name;
                var email = account.Account_Email;

                string website = AppSettings.Website;

                string subject = "Khôi phục mật khẩu tài khoản tại " + website + " dành cho " + name;
                string body = RenderViewToString(ctx, "../EmailTemplate/PartialRegisterEmailTemplate_RecoverPassword", Tuple.Create(account, name, email));

                string to_email = email;

                //CC Emails
                string cc_email = AppSettings.EmailAdmin;

                SendMail(to_email, cc_email, subject, body);
            }
        }

        
        #endregion

    }
}