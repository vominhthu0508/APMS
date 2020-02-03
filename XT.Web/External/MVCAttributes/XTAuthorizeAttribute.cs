using XT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XT.Web.External.MVCAttributes
{
    public class XTAuthorizeAttribute : AuthorizeAttribute
    {
        public XT.Model.RoleTypeEnum[] Types;
        public XTAuthorizeAttribute()
        {

        }

        //public XTAuthorizeAttribute(string roles)
        //{
        //    Roles = roles;
        //}

        public XTAuthorizeAttribute(params RoleTypeEnum[] types)
        {
            Types = types;
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            //if (!string.IsNullOrEmpty(Roles))
            //{
            //    var roles = Roles.Split(',');
            //    foreach (var r in roles)
            //    {
            //        var _r = r.Trim();
            //        var type = _r.ToEnum<AccountTypeEnum>();
            //        if (AuthenticationManager.Is(type))
            //            return true;
            //    }
            //}

            if (AuthenticationManager.IsMod)
                return true;

            if (Types != null)
            {
                foreach (var type in Types)
                {
                    if (AuthenticationManager.Is(type))
                    {
                        return true;
                    }
                }

                return false;
            }

            return AuthenticationManager.IsBasicAuthorized;// IsAuthenticated;
        }

        //Không sửa phần này
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        message = "NotAuthorized",
                        LogOnUrl = urlHelper.Action("Login", "Account")
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = RedirectToAction("Account", "Login", HttpContext.Current.Request.RawUrl);
            }
        }

        private ActionResult RedirectToAction(string controller, string action, string returnurl = "")
        {
            return new RedirectToRouteResult(new RouteValueDictionary(new { controller = controller, action = action, returnurl = returnurl }));
        }
    }

    //public class AuthUserAttribute : AuthorizeAttribute
    //{

    //    public XT.Model.AccountTypeEnum[] SecurityGroups;
    //    public string Groups { get; set; }

    //    protected override bool AuthorizeCore(HttpContextBase httpContext)
    //    {
    //        bool valid = false;


    //    }

    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {

    //    }
    //}
}