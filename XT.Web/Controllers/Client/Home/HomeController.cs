using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Facebook;
using Newtonsoft.Json.Linq;
using XT.BusinessService;
using XT.Model;
using XT.Web.External;
using XT.Web.External.MVCAttributes;
using XT.Web.Models;
using System.Threading;
using System.Collections;
using System.Linq.Expressions;
using PagedList;
using PagedList.Mvc;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Drawing;

namespace XT.Web.Controllers
{
    public class HomeController : BaseController
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Setup()
        {
            ///////////////////////////////////////////////////
            var CURRENT_COMPANY = 3;//3: AMMHCM

            //reset HCM feeplans (DeleteForever)
            //var hcms_ins = IoCConfig.Service<IStudent_FeePlan_InstallmentService>()
            //    .FindAllByCriteria(f => f.Student_FeePlan.Student.Class.Company_Id == CURRENT_COMPANY);
            //foreach (var ins in hcms_ins)
            //{
            //    IoCConfig.Service<IStudent_FeePlan_InstallmentService>().DeleteForever(ins);
            //}

            //var hcms = IoCConfig.Service<IStudent_FeePlanService>()
            //    .FindAllByCriteria(f => f.Student.Class.Company_Id == CURRENT_COMPANY);
            //foreach (var f in hcms)
            //{
            //    IoCConfig.Service<IStudent_FeePlanService>().DeleteForever(f);
            //}

            //reset HCM schedule
            //var slist = IoCConfig.Service<IClass_ModuleService>()
            //    .FindAllByCriteria(f => f.Class.Company_Id == CURRENT_COMPANY);
            //foreach (var ins in slist)
            //{
            //    ins.Resource_LT_Id = 1;
            //    ins.Resource_TH_Id = 1;
            //    ins.Resource_Exam_Id = 1;
            //    IoCConfig.Service<IClass_ModuleService>().Update(ins);
            //}

            ////reset HCM resource
            //var rlist = IoCConfig.Service<IResourceService>()
            //    .FindAllByCriteria(f => f.Company_Id == CURRENT_COMPANY);
            //foreach (var ins in rlist)
            //{
            //    IoCConfig.Service<IResourceService>().DeleteForever(ins);
            //}

            ///////////////////////////////////////////////////

            if (AppSettings.GenerateSampleData)
            {
                //////////////////////////////////////////////////////////////////////////////////////////
                //Truoc khi chay setup phai chay create database script (LinkHouse_Test_Create.sql)
                //////////////////////////////////////////////////////////////////////////////////////////

                //ROLE ENUMS
                var role_service = IoCConfig.Service<IRole_TypeService>();
                if (role_service.FindAll().Count() == 0)
                {
                    //ROLE TYPE
                    var account_types = new List<Role_Type>();
                    foreach (RoleTypeEnum type in Enum.GetValues(typeof(RoleTypeEnum)))
                    {
                        account_types.Add(new Role_Type { Id = (int)type, Role_Type_Name = type.ToString(), Created_Date = DateTime.Now });
                    }
                    role_service.AddAll(account_types.ToArray());
                }

                //USER ENUMS
                var user_service = IoCConfig.Service<IUser_TypeService>();
                if (user_service.FindAll().Count() == 0)
                {
                    //ROLE TYPE
                    var account_types = new List<User_Type>();
                    foreach (UserTypeEnum type in Enum.GetValues(typeof(UserTypeEnum)))
                    {
                        account_types.Add(new User_Type { Id = (int)type, User_Type_Name = type.ToString(), Created_Date = DateTime.Now });
                    }
                    user_service.AddAll(account_types.ToArray());
                }

                var website = AppSettings.Website;

                //USER_PROFILE
                var user = new User_Profile
                {
                    User_Profile_Name = "admin",
                    User_Profile_Email = "admin@" + website,
                    User_Profile_Avatar = "~/Images/default_avatar.jpg",
                    User_Profile_Phone = "0909123456",
                    Status = (int)EntityStatus.Visible,
                    Created_Date = DateTime.Now,
                    Role_Type_Id = (int)RoleTypeEnum.Admin,
                    User_Type_Id = (int)UserTypeEnum.CH,
                };
                user = IoCConfig.Service<IUserProfileService>().Add(user);

                //ACCOUNT
                var admin = new Account
                {
                    User_Profile_Id = user.Id,
                    Account_Username = "admin",
                    Account_Name = user.User_Profile_Name,
                    Account_Email = user.User_Profile_Email,
                    Account_Avatar = user.User_Profile_Avatar,
                    Account_Password = PasswordEncryptManager.EncryptPassword("@1234567"),
                    Status = (int)EntityStatus.Visible,
                    Created_Date = DateTime.Now,
                    HasSetPassword = true,
                };
                IoCConfig.Service<IAccountService>().AddAll(new[] { admin });

            }
            return View();
        }
       
        #region localization

        public ActionResult ChangeCurrentCulture(int id)
        {
            //  
            // Change the current culture for this user.  
            //  
            CultureHelper.SetLanguage(id);
            //  
            // Cache the new current culture into the user HTTP session.   
            //  \
            //  
            // Redirect to the same page from where the request was made!   
            //  
            return Redirect(Request.UrlReferrer.ToString());
        }

        #endregion

        


    }
}
