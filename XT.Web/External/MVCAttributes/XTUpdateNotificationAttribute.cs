using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XT.BusinessService;

namespace XT.Web.External.MVCAttributes
{
    public class XTUpdateNotificationAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //Helper.UpdateNotification();
        }
    }
}