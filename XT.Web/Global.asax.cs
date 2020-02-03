using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using XT.BusinessService;
using XT.Model;
using XT.Repository;
using System.Data.Linq;
using System.Web.Optimization;
using XT.Web.Controllers;
using SimpleInjector;
using System.Globalization;
using System.Threading;
using System.ComponentModel.Design;
using System.Resources;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace XT.Web
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (model != null)
            {
                var date = model.AttemptedValue;

                if (String.IsNullOrEmpty(date))
                    return null;

                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue(bindingContext.ModelName));
                try
                {
                    return DateTime.Parse(date);
                }
                catch (Exception)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, String.Format("\"{0}\" is invalid.", bindingContext.ModelName));
                    return null;
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IoCConfig.Register();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // event is raised each time a new session is created     
            //CultureInfo newCulture = CultureInfo.CreateSpecificCulture("vi-VN");
            //newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "/";
            //Thread.CurrentThread.CurrentCulture = newCulture;
            //XT.Web.External.AuthenticationManager.SetAuthentication(null);//set tạm cho admin login without pass, login thật sẽ remove đi
            External.CultureHelper.SetLanguage();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // event is raised when a session is abandoned or expires
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "/";
            //newCulture.NumberFormat.CurrencyDecimalSeparator = ",";
            //newCulture.NumberFormat.CurrencyGroupSeparator = ".";

            CultureInfo newCulture = CultureInfo.CreateSpecificCulture("vi-VN");
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            //newCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }

        //protected void Application_EndRequest(Object sender, EventArgs e)
        //{
        //    IoCConfig.GetInstance<IUow>().Commit();
        //}

        protected void Application_Error(object sender, EventArgs e)
        {
            if (AppSettings.ShowErrorPage == true)
            {
                Exception lastError = Server.GetLastError();
                Server.ClearError();

                int statusCode = 0;

                if (lastError.GetType() == typeof(HttpException))
                {
                    statusCode = ((HttpException)lastError).GetHttpCode();
                }
                else
                {
                    // Not an HTTP related error so this is a problem in our code, set status to
                    // 500 (internal server error)
                    statusCode = 500;
                }

                HttpContextWrapper contextWrapper = new HttpContextWrapper(this.Context);

                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("otherException", null);
                routeData.Values.Add("statusCode", statusCode);
                routeData.Values.Add("exception", lastError);
                routeData.Values.Add("message", lastError.Message);
                routeData.Values.Add("isAjaxRequet", contextWrapper.Request.IsAjaxRequest());

                IController controller = new ErrorController();

                RequestContext requestContext = new RequestContext(contextWrapper, routeData);

                controller.Execute(requestContext);
                Response.End();
            }
        }
    }
}