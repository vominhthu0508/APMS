using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XT.Web.External
{
    public static class LocalHelper
    {
        //Hàm dùng để GetResource trên Views (Html.GetResource)
        public static String GetResource(this HtmlHelper html, String val)
        {
            return XT.Web.LocalResource.Resource.ResourceManager.GetString(val);
        }
    }
}
