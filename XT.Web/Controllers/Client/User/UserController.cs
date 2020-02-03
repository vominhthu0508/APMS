using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using XT.Repository;
using XT.BusinessService;
using XT.Model;
using XT.Utilities;
using XT.Web.External;
using XT.Web.External.MVCAttributes;
using XT.Web.Models;
using PagedList;
using PagedList.Mvc;

namespace XT.Web.Controllers
{
    [XTAuthorize]
    public class UserController : BaseController
    {
        int PAGE_SIZE = 5;
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}
