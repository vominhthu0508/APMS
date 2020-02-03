using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;
using XT.Web.Models;
using System.Configuration;
using XT.Web.External.MVCAttributes;
using XT.Utilities;
using Newtonsoft.Json.Linq;
using System.IO;
using Facebook;
using System.Net;
using System.Security.Claims;

namespace XT.Web.Controllers
{
    public class AccountController : BaseController
    {
        string ACCOUNT_CONTROLLER = "Account";

        string ACCOUNT_MSG_INVALIDACCOUNT = "Tài khoản này không tồn tại";

        #region private

        protected ActionResult RedirectToRegisterSuccess(string message = "")
        {
            return RedirectToAction("RegisterSucccess", ACCOUNT_CONTROLLER);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////
        ////SetLoginAuthentication Method for Login and LoginJs action

        private void SetLoginAuthentication(Account acc)
        {
            //if (acc.IsAdmin())
            //{
            //    var center = IoCConfig.Service<ICompanyService>().FindById(CURRENT_COMPANY);
            //    XT.Web.External.AuthenticationManager.SetAuthenticationCenter(acc, center);
            //}
            //else
            //{
            //    XT.Web.External.AuthenticationManager.SetAuthentication(acc);
            //}
            XT.Web.External.AuthenticationManager.SetAuthentication(acc);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////
        ////CheckAccountError

        private string CheckAccountError(Account acc)
        {
            if (acc == null)//Có tồn tại không?
            {
                return "Không tồn tại tài khoản này hoặc mật khẩu không chính xác";
            }
            else if (acc.IsInactive())//Có kích hoạt không?
            {
                return "Tài khoản này chưa kích hoạt. Vui lòng kiểm tra Email để kích hoạt. Cảm ơn!";
            }
            else if (!acc.IsValid())//Có bị xóa không?
            {
                return "Tài khoản này tạm thời đang bị khóa. Vui lòng liên hệ để biết thêm thông tin chi tiết. Cảm ơn!";
            }

            return string.Empty;
        }

        /// <summary>
        /// acc == null: không tồn tại => false,
        /// acc.IsInactive => false,
        /// acc.IsNotValid => false,
        /// string.Empty => true
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        private bool CheckAccount(Account acc)
        {
            var error = CheckAccountError(acc);
            if (!string.IsNullOrEmpty(error))
            {
                SetCustomError(error);
                return false;
            }

            return true;
        }

        private bool CheckLoginAndAuthentication(Account acc)
        {
            if (CheckAccount(acc))
            {
                SetLoginAuthentication(acc);
                return true;
            }

            return false;
        }

        private Account CheckLogin(LoginModel login)
        {
            var acc = PasswordEncryptManager.Login(login.Username, login.Password);
            if (CheckLoginAndAuthentication(acc))
            {
                return acc;
            }

            return null;
        }

        #endregion

        //RegisterUser => RegisterUserSuccess => SendMail_RegisterInform => done
        //=> Active => "OK" => SendMail_RegisterActiveSuccess => done
        //=> Login: Login Page/Ajax/Facebook => done
        //=> Logout => done
        //=> ChangePassword
        //=> RecoverPassword => RecoverPasswordFinish
        //=> ChangeProfileInfo

        #region Register User
        //- Đăng ký => invisible => active => visible
        //- Đăng ký => check trùng email
        //    + Cho phép dùng lại email của account đã xóa => find all visible
        //    + Những user chưa active => invisible => không dc tính => bị mất email
        //    => vô lý!

        //- Đăng ký thành công => Inactive => set active => Visible
        //- Lúc đăng ký 
        //    => check trùng username => find all alive (visible + inactive) (not invisible/deleted)
        //    => check trùng email trong bảng profile => find all visible profile
        //- Xóa profile
        //    + Xóa account
        //    + Xóa profile
        [HttpGet]
        public ActionResult RegisterUser(string session = "", string token = "")
        {
            if (session == "" || token == "")
            {
                return RedirectToAction("RegisterUser", new { session = Guid.NewGuid().ToString(), token = Guid.NewGuid().ToString() });
            }
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisterUserModel register)
        {
            if (ModelState.IsValid)
            {
                var response = this.VerifyRecaptcha(true);
                if (!response)
                {
                    return View(register);
                }

                //SỬAAAAAAAAAAAAAAAAAA
                var managementItem = new AccountManagementItem<User_Profile, IUserProfileService>();
                var type = RoleTypeEnum.User;

                //không sửaaaaaaaaaaa
                managementItem.ModelState = ModelState;
                managementItem.register = register;

                if (managementItem.Validating())
                {
                    //not yet active, not yet verified
                    //has set password
                    if (managementItem.Adding(this.ControllerContext) != null)
                    {
                        return RedirectToRegisterSuccess();
                    }
                    ViewBag.CustomError = true;
                }
            }

            return View(register);
        }

        [HttpGet]
        public ActionResult RegisterSucccess()
        {
            return View();
        }

        #endregion

        #region Active User
        /// <summary>
        /// Active user sau khi đăng ký thông qua key được gửi về email của user
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult Active(string key)
        {
            //if (string.IsNullOrEmpty(key))
            //{
            //    return RedirectToError();
            //}

            var managementItem = new AccountManagementItem<User_Profile, IUserProfileService>();
            if (!managementItem.Active(this.ControllerContext, key))
            {
                return RedirectToError(ACCOUNT_MSG_INVALIDACCOUNT);
            }
            var AccountService = IoCConfig.Service<AccountService>();
            Account activeAcc = AccountService.FindAll().Where(f => f.Account_ActiveKey == key).First();
            if (!CheckLoginAndAuthentication(activeAcc))
            {
                return RedirectToLogin();
            }
            //var accountService = IoCConfig.Service<IAccountService>();
            //var account = accountService.GetAccountByActiveKey(key);
            //if (account == null)
            //{
            //    return RedirectToError(ACCOUNT_MSG_INVALIDACCOUNT);
            //}

            //accountService.Active(account);

            ////send email active thành công
            //EmailHelper.SendMail_RegisterActiveSuccess(this.ControllerContext, account);

            return View();
        }
        #endregion

        #region Login

        #region Login Page
        //Login trong trang Login (login bình thường)
        public ActionResult Login(string error = "", string returnurl = "")
        {
            if (!string.IsNullOrEmpty(error))
            {
                SetCustomError(error);
            }

            return View(new LoginModel { ReturnUrl = returnurl });
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            //if (ModelState.IsValid)
            //{
            //    if (login.Username == "admin" && login.Password == AppSettings.DefaultPassword)
            //    {
            //        return Redirect(Url.GoAccountAdminFirstPage());
            //    }
            //}

            if (ModelState.IsValid)
            {
                var acc = CheckLogin(login);
                if (acc != null)
                {
                    var returnUrl = login.ReturnUrl;
                    if (returnUrl != null && !string.IsNullOrEmpty(returnUrl.Replace("/", "")))
                    {
                        return Redirect(login.ReturnUrl);
                    }
                    switch (acc.GetRoleId())
                    {
                        case (int)RoleTypeEnum.Admin: return Redirect(Url.GoAccountAdminFirstPage());
                        //case (int)RoleTypeEnum.ModData: return Redirect(Url.GoAccountAdminFirstPage());
                    }

                    return RedirectToHome();
                }
            }

            return View(login);
        }
        #endregion Login Page

        #region Login Ajax
        //Login khi click button Đăng nhập ở màn hình popup
        [HttpPost]
        public ActionResult LoginJs(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var acc = CheckLogin(login);
                if (acc != null)
                {
                    var accountType = acc.GetRoleId();
                    var accountName = acc.Account_Name;
                    var accountAvatar = Url.Content(acc.Account_Avatar);

                    return Json(new { success = true, accountType = accountType, accountName = accountName, accountAvatar = accountAvatar });
                }
            }

            return Error();
        }
        #endregion Login Ajax

        #region Login Facebook
        public static dynamic CheckLoginFacebook(string accessToken)
        {
            var website = AppSettings.Website;
            var message = "";
            if (!string.IsNullOrEmpty(accessToken))
            {
                //1. Lấy thông tin facebook info
                //2. Tìm FB ID đã có chưa
                //3. Nếu chưa có 
                //3.1 Tìm Email đã có chưa
                //3.2 Nếu tồn tại email
                //3.3 Thay đổi email sang email khác (Guid ID)
                var client = new FacebookClient(accessToken);
                dynamic result;
                try
                {
                    result = client.Get("me", new { fields = "id,name,email,picture,cover,gender" });
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Có lỗi xảy ra, vui lòng đăng nhập lại" };
                }

                if (result.id == null)
                {
                    return new { success = false, message = "Tài khoản Facebook không hợp lệ hoặc bạn chưa cấp quyền truy cập Facebook!" };
                }

                var facebook_userID = result.id;

                var accountService = IoCConfig.Service<IAccountService>();
                var userProfileService = IoCConfig.Service<IUserProfileService>();
                User_Profile user = userProfileService.getUserBySocialId(facebook_userID);

                if (user == null)
                {
                    WebClient webclient = new WebClient();
                    JObject jsonUserInfo = null;
                    string JsonResult = string.Empty;

                    var userGuidId = Guid.NewGuid().ToString();
                    var username = userGuidId;
                    var email = username + "@" + website;
                    var avatar = AppSettings.DefaultAccountAvatar;
                    var name = "Guest";

                    #region get username
                    if (result.username != null)
                    {
                        username = result.username;
                        email = username + "@" + website;
                    }
                    #endregion

                    #region get email
                    if (result.email != null)
                    {
                        email = result.email;
                        if (username == userGuidId)
                            username = email;
                    }

                    user = userProfileService.getUserProfileByEmail(email);
                    if (user != null)//trùng email
                    {
                        userGuidId = Guid.NewGuid().ToString();
                        username = userGuidId;
                        email = username + "@" + website;
                    }
                    #endregion get email

                    #region get name
                    if (result.name != null)
                    {
                        name = result.name;
                    }
                    #endregion get name

                    #region get avatar
                    try
                    {
                        JsonResult = webclient.DownloadString(string.Concat(
                               "https://graph.facebook.com/" + facebook_userID
                               + "/picture?redirect=false&type=large"));
                        jsonUserInfo = JObject.Parse(JsonResult);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    if (jsonUserInfo["data"] != null && jsonUserInfo["data"]["url"] != null)
                    {
                        avatar = jsonUserInfo["data"]["url"].ToString();

                        //save avatar to folder
                        byte[] data;
                        try
                        {
                            data = webclient.DownloadData(avatar);

                            avatar = "~/Uploads/UserUploads/" + userGuidId + "_avatar.jpg";
                            System.IO.File.WriteAllBytes(
                                Path.Combine(System.Web.HttpContext.Current.Server.MapPath(avatar)), data);
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                        }
                    }
                    #endregion get avatar

                    //#region get gender
                    //if (result.gender != null)
                    //{
                    //    if (result.gender == "female")
                    //        gender = 2;
                    //}
                    //#endregion get gender

                    //add user
                    user = new User_Profile();

                    user.User_Profile_Facebook = facebook_userID;
                    user.User_Profile_Name = name;
                    user.User_Profile_Email = email;
                    user.User_Profile_Avatar = avatar;

                    webclient.Dispose();

                    if (user != null)//user mới
                    {
                        return new { success = true, user = user, isNew = true, username = username, message = message };
                    }

                    message = "Thông tin tài khoản có thể bị trùng, vui lòng kiểm tra lại hoặc đăng nhập theo tài khoản khác";
                }
                else//user exists
                {
                    if (user.IsValid())//user cũ
                    {
                        return new { success = true, user = user, isNew = false, message = message };
                    }

                    message = "Tài khoản này tạm thời đang bị khóa. Vui lòng liên hệ để biết thêm thông tin chi tiết. Cảm ơn!";
                }
            }

            return new { success = false, message = message };
        }

        //Login khi click button Facebooking Login, sẽ gọi action này mà không gọi action Login
        //Đây là action giả, không có View, tất cả sẽ được redirect đi nơi khác
        //Nếu có lỗi xảy ra, redirect sang action Login với error message
        //Nếu thành công, lưu Session và redirect về Home
        [HttpPost]
        public ActionResult LoginFacebook(string accessToken, string return_url)
        {
            var res = CheckLoginFacebook(accessToken);
            if (res.success == true)//thành công
            {
                var user = res.user as User_Profile;
                Account acc = null;
                //save session
                if (res.isNew == true)//nếu user mới
                {
                    user.SetRole(RoleTypeEnum.User);

                    //adding
                    var managementItem = new AccountManagementItem<User_Profile, IUserProfileService>();
                    managementItem.ModelState = ModelState;

                    acc = managementItem.AddingNonRegister(this.ControllerContext, user, true);
                }
                else//nếu user cũ
                {
                    acc = IoCConfig.Service<IAccountService>().GetAccountByProfileId(user.Id);
                }

                if (CheckLoginAndAuthentication(acc))
                {
                    return Success();
                }
            }

            return RedirectToLoginError(res.message);
        }
        #endregion Login Facebook

        #endregion

        #region Logout

        [HttpGet]
        public ActionResult Logout()
        {
            External.AuthenticationManager.Logout();

            return RedirectToLogin();
        }

        #endregion Logout

        #region ChangePassword
        [HttpGet]
        [XTAuthorize]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel
            {
                OldPassword = XT.Web.External.AuthenticationManager.Account_HasSetPassword ? "" : AppSettings.DefaultPassword
            };
            return View(model);
        }

        [HttpPost]
        [XTAuthorize]
        public ActionResult ChangePassword(ChangePasswordModel register)
        {
            if (ModelState.IsValid)
            {
                //var id = XT.Web.External.AuthenticationManager.Id;
                var accountService = IoCConfig.Service<IAccountService>();
                Account acc = null;
                if (XT.Web.External.AuthenticationManager.Account_HasSetPassword)
                {
                    acc = PasswordEncryptManager.Login(XT.Web.External.AuthenticationManager.Account_Username, register.OldPassword);
                }
                else
                {
                    acc = accountService.FindById(XT.Web.External.AuthenticationManager.Id);
                }

                if (!CheckAccount(acc))
                {
                    return View(register);
                }

                acc.Account_Password = PasswordEncryptManager.EncryptPassword(register.NewPassword);
                acc.HasSetPassword = true;

                acc = accountService.Update(acc);

                if (acc != null)
                {
                    SetLoginAuthentication(acc);
                    SetSuccess("Thay đổi mật khẩu thành công!");
                }
                else
                {
                    SetCustomError("Thay đổi thất bại! Vui lòng thực hiện lại");
                }
            }

            return View(register);
        }

        #endregion

        #region RecoverPassword
        [HttpGet]
        public ActionResult RecoverPassword(string session = "", string token = "")
        {
            if (session == "" || token == "")
            {
                return RedirectToAction("RecoverPassword", new { session = Guid.NewGuid().ToString(), token = Guid.NewGuid().ToString() });
            }
            return View();
        }

        [HttpPost]
        public ActionResult RecoverPassword(RecoverPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var response = this.VerifyRecaptcha();
                if (!response)
                {
                    return View(model);
                }
                ///////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////
                //tài khoản phải được kích hoạt rồi mới có thể recover password

                var accountService = IoCConfig.Service<IAccountService>();
                var acc = accountService.GetAccountByIdentity(model.Username);

                if (!CheckAccount(acc))
                {
                    return View(model);
                }

                //Đã tồn tại key => check email
                if (!string.IsNullOrEmpty(acc.Account_RecoverPasswordKey)
                    && acc.Account_RecoverPasswordExpired.HasValue
                    && acc.Account_RecoverPasswordExpired.Value > DateTime.Now)
                {
                    SetCustomError("Chúng tôi đã gửi email khích hoạt cho bạn. Vui lòng làm theo hướng dẫn trong email.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //send request email to recover password
                //1. Tạo 1 key lưu vào account table
                //2. Send key đến email user qua link có attach key
                //VD: http://www.alotro.com/RecoverPasswordFinish?key=...
                //3. Trang RecoverPasswordFinish: get key va get luon account info
                //Input new password va confirm password
                //Submit => Update pass

                var key = Guid.NewGuid().ToString();
                var expired_day = DateTime.Now.AddDays(1);
                acc.Account_RecoverPasswordKey = key;
                acc.Account_RecoverPasswordExpired = expired_day;
                acc = accountService.Update(acc);

                if (acc != null)
                {
                    //start sending email
                    EmailHelper.SendMail_RecoverPassword(this.ControllerContext, acc);
                    //end sending email
                    SetSuccess("Gửi email thành công! Vui lòng kiểm tra email và làm theo những bước hướng dẫn trong email để khôi phục mật khẩu");
                }
                else
                {
                    SetCustomError("Có lỗi xảy ra. Vui lòng thực hiện lại");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult RecoverPasswordFinish(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                SetCustomError("Vui lòng nhập mã khôi phục đã được gửi đến email của bạn");
                return View();
            }
            var model = new RecoverPasswordFinishModel();
            model.RecoverKey = key;
            return View(model);
        }

        [HttpPost]
        public ActionResult RecoverPasswordFinish(RecoverPasswordFinishModel model)
        {
            if (ModelState.IsValid)
            {
                var response = this.VerifyRecaptcha();
                if (!response)
                {
                    return View(model);
                }

                var accountService = IoCConfig.Service<IAccountService>();
                var acc = accountService.GetAccountByIdentity(model.Username);

                if (!CheckAccount(acc))
                {
                    return View(model);
                }

                //Chưa tồn tại key hoặc key hết hiệu lực
                if (!acc.Account_RecoverPasswordExpired.HasValue
                    || acc.Account_RecoverPasswordExpired.Value < DateTime.Now)
                {
                    //reset recover password key
                    acc.Account_RecoverPasswordKey = null;
                    acc.Account_RecoverPasswordExpired = null;
                    accountService.Update(acc);

                    SetCustomError("Mã khôi phục đã hết hiệu lực. Vui lòng thực hiện lại việc khôi phục mật khẩu ở form đăng nhập.");
                }
                else if (acc.Account_RecoverPasswordKey != model.RecoverKey)
                {
                    SetCustomError("Mã khôi phục không đúng. Vui lòng liên hệ lại với ban quản trị.");
                }
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                acc.Account_Password = PasswordEncryptManager.EncryptPassword(model.NewPassword);
                //reset recover password key
                acc.Account_RecoverPasswordKey = null;
                acc.Account_RecoverPasswordExpired = null;
                acc = accountService.Update(acc);

                if (acc != null)
                {
                    ViewBag.CurrentStep = 2;
                    SetSuccess("Khôi phục mật khẩu thành công. Hãy click vào Đăng nhập để bắt đầu sử dụng :)");
                }
                else
                {
                    SetCustomError("Có lỗi xảy ra. Vui lòng thực hiện lại");
                }
            }

            return View(model);
        }
        #endregion

        #region Change Profile
        //Change Profile Info
        [HttpGet]
        [XTAuthorize]
        public ActionResult ChangeProfileInfo(string message = "")
        {
            ChangeUserProfileModel model = new ChangeUserProfileModel();
            model.FromModel(GetCurrentUser());

            if (!string.IsNullOrEmpty(message))
            {
                SetCustomError(message);
            }

            return View(model);
        }

        [HttpPost]
        [XTAuthorize]
        public ActionResult ChangeProfileInfo(ChangeUserProfileModel profile)
        {
            if (ModelState.IsValid)
            {
                var u = GetCurrentUser();
                u = profile.ToModel(u);

                //SỬAAAAAAAAAAAAAAAAAA
                var managementItem = new AccountManagementItem<User_Profile, IUserProfileService>();
                //var type = RoleTypeEnum.User;

                //không sửaaaaaaaaaaa
                managementItem.ModelState = ModelState;
                //managementItem.profile = profile;

                var acc = managementItem.ChangeProfile(this.ControllerContext, u);
                if (acc != null)//~ account != null && account.IsValid() trong ChangeProfile
                {
                    SetLoginAuthentication(acc);
                    SetSuccess("Cập nhật thành công");
                }
                else
                {
                    SetCustomError("Có lỗi xảy ra. Vui lòng kiểm tra lại");
                }
            }

            return View(profile);
        }
        #endregion

        [HttpGet]
        public ActionResult SetCenterAuthentication(int Id = 0, string ReturnUrl = "")
        {
            var center = IoCConfig.Service<ICompanyService>().FindById(Id);
            if (IsValidModel(center))
            {
                var acc = IoCConfig.Service<IAccountService>().FindById(XT.Web.External.AuthenticationManager.Id);
                if (acc.IsCenter(Id))
                {
                    XT.Web.External.AuthenticationManager.SetAuthenticationCenter(acc, center);
                }

            }

            if (ReturnUrl != "")
                return Redirect(ReturnUrl);
            return RedirectToHome();
        }

    }

    public interface IAccountManagementItem
    {
        ModelStateDictionary ModelState { get; set; }
        bool Validating();
        void Delete(ControllerContext ctx, int id);
        bool Active(ControllerContext ctx, string key);
    }

    public class AccountManagementItem<U, T> : IAccountManagementItem
        where U : class, IRegisterEntity<Int32>
        where T : class, IRegisterService<U>
    {
        public ModelStateDictionary ModelState { get; set; }
        public RegisterModel<U> register { get; set; }
        //public ChangeProfileModel<U> profile { get; set; }

        public T GetService()
        {
            return IoCConfig.Service<T>();
        }

        public bool Validating()//check validation for register
        {
            IAccountService accountService = IoCConfig.Service<IAccountService>();
            var res = true;

            //if (accountService.CheckExistUsername(register.Username))
            //{
            //    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
            //    res = false;
            //}

            if (GetService().CheckExistEmail(register.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                res = false;
            }

            return res;
        }

        public bool ValidatingProfile(U u)//check validation for change profile
        {
            var res = true;

            if (GetService().CheckExistEmail(u.Email, u.Obj_Id))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                res = false;
            }

            return res;
        }

        /// <summary>
        /// RegisterUser/LoginFacebook
        /// </summary>
        /// <param name="type"></param>
        /// <param name="hasAlreadyVerified">Nếu true thì set verified status là true trong hàm ToModel</param>
        /// <param name="hasAlreadyActive">False: RegisterUser, True: LoginFacebook</param>
        /// <returns></returns>
        private Account Create(ControllerContext ctx, U u,
            bool hasAlreadyActive = false, bool HasSetPassword = true)
        {
            u = GetService().Add(u);
            if (u != null)
            {
                var accountService = IoCConfig.Service<IAccountService>();

                //Create new Account
                //- Account_Status = Inactive
                //- Active Key được new Guid và được gửi về email sau
                var account = accountService.Create(register.Username,
                        PasswordEncryptManager.EncryptPassword(register.Password),
                        u.Obj_Id, u);
                account.HasSetPassword = HasSetPassword;

                //Add new account
                account = accountService.Add(account);
                if (account != null)
                {
                    if (hasAlreadyActive)
                    {
                        accountService.Active(account);
                        //GetService().Active(u);//user profile khi add đã active sẵn rồi

                        //start sending email
                        EmailHelper.SendMail_RegisterActiveSuccess(ctx, account);
                        //end sending email
                    }
                    else
                    {
                        //start sending email
                        EmailHelper.SendMail_RegisterInform(ctx, account);
                        //end sending email
                    }
                    return account;
                }
            }

            ModelState.AddModelError("CustomError", "Thêm tài khoản thất bại!");
            return null;
        }

        /// <summary>
        /// Used for RegisterUser 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="type"></param>
        /// <param name="hasAlreadyVerified"></param>
        /// <returns></returns>
        public Account Adding(ControllerContext ctx, 
            bool hasAlreadyVerified = false, 
            bool hasAlreadyActive = false)
        {
            U u = register.ToModel(hasAlreadyVerified);
            return Create(ctx, u, hasAlreadyActive);
        }

        /// <summary>
        /// Used for LoginFacebook, need to make a RegisterModel
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="u"></param>
        /// <param name="type"></param>
        /// <param name="hasAlreadyActive"></param>
        /// <returns></returns>
        public Account AddingNonRegister(ControllerContext ctx, U u, 
            bool hasAlreadyActive = false)
        {
            register = new RegisterModel<U> { Username = u.Email, Password = AppSettings.DefaultPassword };
            return Create(ctx, u, hasAlreadyActive, HasSetPassword: false);
        }

        /// <summary>
        /// Active không trả về mà chỉ add error trong ModelState
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="id">User Profile Id</param>
        /// <param name="type"></param>
        public bool Active(ControllerContext ctx, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var accountService = IoCConfig.Service<IAccountService>();
            var account = accountService.GetAccountByActiveKey(key);
            if (account == null)
            {
                return false;
            }

            accountService.Active(account);

            //send email active thành công
            EmailHelper.SendMail_RegisterActiveSuccess(ctx, account);

            return true;
        }

        /// <summary>
        /// Delete không trả về mà chỉ add error trong ModelState
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void Delete(ControllerContext ctx, int id)
        {
            var userService = GetService();
            var u = userService.FindById(id);
            if (u != null)
            {
                //var accountService = IoCConfig.Service<IAccountService>();

                //var account = accountService.GetAccountByProfileId(u.Obj_Id);
                //if (account != null)
                //{
                //    accountService.Delete(account);
                //    userService.Delete(u);

                //    //Delete không gửi email cho user thông báo

                //    return;
                //}
                userService.Delete(u);
            }

            ModelState.AddModelError("CustomError", "Xóa tài khoản thất bại!");
        }

        /// <summary>
        /// Gửi lại email thông báo kích hoạt tài khoản của admin
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void RegisterInform(ControllerContext ctx, int id)
        {
            var u = GetService().FindById(id);
            if (u != null)
            {
                var account = IoCConfig.Service<IAccountService>().GetAccountByProfileId(u.Obj_Id);
                if (account != null)
                {
                    //start sending email
                    EmailHelper.SendMail_RegisterInform(ctx, account);
                    //end sending email
                }
            }
        }

        /// <summary>
        /// Change Profile Info
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public Account ChangeProfile(ControllerContext ctx, U u)
        {
            if (ValidatingProfile(u))
            {
                u = GetService().Update(u);
            }

            return null;
        }
    }

}