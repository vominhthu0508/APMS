using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XT.BusinessService;
using XT.Model;
using XT.Web.External;
using XT.Web.External.Constants;

namespace XT.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Current Importing Center
        /// </summary>
        public static int CURRENT_COMPANY
        {
            get{
                return AppSettings.ImportingCenter;
            }
        }//For Import: 1 - AMM2, 2 - AMM1, 3 - AMMHCM
        protected string UNKNOWN_ERROR_MSG = "Có lỗi xảy ra! Vui lòng thực hiện lại";

        protected bool IsValidModel(IEntity model)
        {
            return model != null && model.IsValid();
        }

        #region CustomMessage
        public void SetCustomError(string error)
        {
            ViewBag.CustomError = true;
            ModelState.AddModelError("CustomError", error);
        }

        public void SetSuccess(string message)
        {
            ViewBag.Success = true;
            ModelState.AddModelError("Success", message);
        }

        protected string GetErrorMessage()
        {
            var message = UNKNOWN_ERROR_MSG;
            var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
            if (error != null)
            {
                message = error.ErrorMessage;
            }

            return message;
        }
        #endregion

        #region ActionResult
        protected ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        protected ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Admin");
        }

        protected ActionResult RedirectToError(string message = "")
        {
            return RedirectToAction("Index", "Error", new { message = message });
        }

        protected ActionResult RedirectToLoginError(string message = "")
        {
            if (string.IsNullOrEmpty(message))
                message = GetErrorMessage();
            return RedirectToAction("Login", "Account", new { error = message });
        }
        #endregion

        #region Json
        protected ActionResult MyContent(string content = "")
        {
            return Json(new { content = content });
        }

        protected ActionResult Success(string message = "", bool reload = false, string redirect = "")
        {
            return Json(new { success = true, message = message, reload = reload, redirect = redirect });
        }

        protected ActionResult Error(string message = "")
        {
            if (string.IsNullOrEmpty(message))
                message = GetErrorMessage();

            return Json(new { success = false, message = message });
        }

        protected ActionResult ErrorNotExist(string message = "")
        {
            if (string.IsNullOrEmpty(message))
                message = "Không tồn tại thông tin này";

            return Error(message);
        }
        #endregion

        #region RenderViewToString
        protected string RenderViewToString(string viewName, object model)
        {
            var context = this.ControllerContext;
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
        #endregion

        #region RenderView
        [HttpPost]
        public ActionResult RenderView(string partialView, object obj)
        {
            var content = "";

            content = RenderViewToString(partialView, obj);

            return MyContent(content);
        }
        #endregion

        #region Get Current User
        protected User_Profile GetCurrentUser()
        {
            if (AuthenticationManager.IsAuthenticated)
                return IoCConfig.Service<IUserProfileService>().FindById(AuthenticationManager.User_Profile_Id);//.Clone() as User_Profile;
            return null;
        }

        protected Account GetCurrentAccount()
        {
            if (AuthenticationManager.IsAuthenticated)
                return IoCConfig.Service<IAccountService>().FindById(AuthenticationManager.Id);//.Clone() as User_Profile;
            return null;
        }
        #endregion

        #region Localization
        protected override void ExecuteCore()
        {
            CultureHelper.ExecuteLanguage();
            base.ExecuteCore();
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
        #endregion

        #region Current User
        public static int Current_User_Id
        {
            get
            {
                return External.AuthenticationManager.User_Profile_Id;
            }
        }
        #endregion

        #region RedirectAction

        protected Exception UnauthorizedAccessException()
        {
            return new HttpException((int)System.Net.HttpStatusCode.Unauthorized, MessageConstants.Unauthorized);
        }
        #endregion
    }

    public class EntityManagementService<U, T>
        where U : class, IEntity<Int32>
        where T : class, IService<U, Int32>
    {
        private ActionResult Json(object data)
        {
            var json = new JsonResult();
            json.Data = data;
            return json;
        }

        //public string Identity { get; set; }

        public ActionResult Add(U u)
        {
            var service = IoCConfig.Service<T>();
            if (service.CheckExistIdentity(u))
            {
                return Json(new { success = false, message = "Thông tin đã tồn tại, vui lòng nhập thông tin khác" });
            }
            u = service.Add(u);
            if (u == null)
                return Json(new { success = false });
            return Json(new { success = true, message = "Thêm thông tin thành công" });
        }

        public ActionResult CheckAndEdit(U u)
        {
            var service = IoCConfig.Service<T>();
            var item = service.FindById(u.Obj_Id);
            if (item == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin này" });
            }

            if (service.CheckExistIdentity(u))
            {
                return Json(new { success = false, message = "Thông tin đã tồn tại, vui lòng nhập thông tin khác" });
            }

            u = service.Update(u);
            if (u == null)
                return Json(new { success = false });
            return Json(new { success = true, message = "Sửa thông tin thành công" });
        }

        public ActionResult Edit(U u)
        {
            var service = IoCConfig.Service<T>();
            if (service.CheckExistIdentity(u))
            {
                return Json(new { success = false, message = "Thông tin đã tồn tại, vui lòng nhập thông tin khác" });
            }

            u = service.Update(u);
            if (u == null)
                return Json(new { success = false });
            return Json(new { success = true, message = "Sửa thông tin thành công" });
        }

        public ActionResult Delete(U u)
        {
            var service = IoCConfig.Service<T>();
            service.Delete(u);
            return Json(new { success = true, message = "Xóa thông tin thành công" });
        }

        /// <summary>
        /// Check Exist & Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CheckAndDelete(int id)
        {
            var service = IoCConfig.Service<T>();
            var u = service.FindById(id);
            if (u == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin này" });
            }

            service.Delete(u);
            return Json(new { success = true, message = "Xóa thông tin thành công" });
        }

        public T GetService()
        {
            return IoCConfig.Service<T>();
        }
    }
}
