using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XT.Web.Models;

namespace XT.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index(int statusCode = 404, Exception exception = null, bool isAjaxRequet = false)
        {
            //Response.StatusCode = statusCode;

            // If it's not an AJAX request that triggered this action then just retun the view
            if (!isAjaxRequet)
            {
                ErrorModel model = new ErrorModel { HttpStatusCode = statusCode, Exception = exception };

                return View(model);
            }
            else
            {
                // Otherwise, if it was an AJAX request, return an anon type with the message from the exception
                var errorObjet = new { message = exception.Message };
                return Json(errorObjet, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
