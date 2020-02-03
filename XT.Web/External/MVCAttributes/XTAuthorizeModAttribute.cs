using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XT.Web.External.MVCAttributes
{
    public class XTAuthorizeModAttribute : XTAuthorizeAttribute
    {
        public XTAuthorizeModAttribute()
        {

        }

        public XTAuthorizeModAttribute(string roles)
        {
            Roles = roles;
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return AuthenticationManager.IsMod || AuthenticationManager.IsAdmin;
        }
    }
}