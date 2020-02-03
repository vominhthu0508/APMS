using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;
using System.Web.Script.Serialization;
using System.IO;
using PagedList;
using PagedList.Mvc;
using XT.Web.Models;
using XT.Web.External.MVCAttributes;
using System.Configuration;

namespace XT.Web.Controllers
{
    public partial class AdminController : AdminBaseController
    {
        public ActionResult Welcome()
        {
            return RedirectToLogin();
        }

        public ActionResult Index()
        {
            //MemoryStream memoryStream = new MemoryStream();
            ////TextWriter IService_sw = new StreamWriter(memoryStream);
            //TextWriter Service_sw = new StreamWriter(memoryStream);//"Service.cs");
            //var assembly = typeof(Account).Assembly;
            //var models =
            //    from type in assembly.GetExportedTypes()
            //    where type.GetInterfaces().Contains(typeof(IEntity)) && type.IsClass && !type.IsAbstract
            //    select type;
            //foreach (var t in models)
            //{
            //    //IService_sw.WriteLine("public interface I" + t.Name + "Service : IService<" + t.Name + ", Int32>{}");
            //    var name = t.Name;
            //    Service_sw.WriteLine("public class " + name + "Service : Service<" + name + ", Int32>, I" + name + "Service");
            //    Service_sw.WriteLine("{");
            //    Service_sw.WriteLine("  public " + name + "Service(IUow uow): base(uow){}");
            //    Service_sw.WriteLine("  //used for FindByName in Manage");
            //    Service_sw.WriteLine("  public override string NameForFinding");
            //    Service_sw.WriteLine("  {");
            //    Service_sw.WriteLine("      get");
            //    Service_sw.WriteLine("      {");
            //    Service_sw.WriteLine("          return \"" + name + "_Name\";");
            //    Service_sw.WriteLine("      }");
            //    Service_sw.WriteLine("  }");
            //    Service_sw.WriteLine("}");
            //}

            ////IService_sw.Close();
            ////Service_sw.Close();
            //Service_sw.Flush();
            //Service_sw.Close();

            //return File(memoryStream.GetBuffer(),
            //    "text/plain",
            //    "Service.cs");  

            return View();
        }

        #region private
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string url; // url to return
            string message = ""; // message to display (optional)

            url = Helper.SaveAs(AppSettings.UploadImagesAdmin, upload).Replace("~", "");

            // since it is an ajax request it requires this string
            string output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\", \"" + message + "\");</script></body></html>";
            return Content(output);
        }
        ////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////

        
        #endregion private
    }
}