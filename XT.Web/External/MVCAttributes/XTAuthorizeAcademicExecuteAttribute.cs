using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using XT.Model;

namespace XT.Web.External.MVCAttributes
{
    /// <summary>
    /// AcademicHead | AAE
    /// </summary>
    public class XTAuthorizeAcademicExecuteAttribute : XTAuthorizeAttribute
    {
        public XTAuthorizeAcademicExecuteAttribute()
        {

        }

        public XTAuthorizeAcademicExecuteAttribute(string roles)
        {
            Roles = roles;
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return AuthenticationManager.IsAcademicExecutive;
        }
    }
}