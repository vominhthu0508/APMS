using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using XT.Model;
using XT.BusinessService;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using XT.Web.Controllers;

namespace XT.Web.External
{
    public class AuthenticationSession
    {
        public static string USER_SESSION = "Account";

        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.Session != null && HttpContext.Current.Session[USER_SESSION] != null;
            }
        }

        public static void SetAuthentication(Account acc, Company center = null)
        {
            if (acc == null)
            {
                acc = new Account
                {
                    Id = 1,
                    Account_Name = "demo",
                    Account_Username = "demo",
                    User_Profile_Id = 1,
                    Account_Email = "demo@gmail.com",
                    Account_Avatar = AppSettings.DefaultAccountAvatar,
                    HasSetPassword = true,
                    User_Profile = new User_Profile
                    {
                        Id = 1,
                        Role_Type = new Role_Type { 
                            Role_Type_Name = "Admin"
                        }
                    }
                };
            }

            if (acc.Id == 0)
                throw new ArgumentNullException("User Id");
            if (string.IsNullOrEmpty(acc.Account_Username))
                throw new ArgumentNullException("User Username");
            if (acc.User_Profile_Id == 0)
                throw new ArgumentNullException("User Profile Id");

            if (string.IsNullOrEmpty(acc.Account_Email))
                throw new ArgumentNullException("User Email");
            if (string.IsNullOrEmpty(acc.Account_Avatar))
                throw new ArgumentNullException("User Avatar");
            if (string.IsNullOrEmpty(acc.Account_Name))
                throw new ArgumentNullException("User Name");

            var user_type = acc.User_Profile != null ? acc.GetRoleId().ToString() : "0";

            if (acc.GetRoleId() > (int)RoleTypeEnum.Mod)
            {
                if (acc.User_Profile.Companies_List.Count() == 0)
                    throw new ArgumentNullException("Company Invalid");
                else
                {
                    if (center == null)
                        center = acc.User_Profile.Companies_List.FirstOrDefault();
                }
            }
            else
            {
                if (center == null)
                    center = IoCConfig.Service<ICompanyService>().FindById(BaseController.CURRENT_COMPANY);
            }

            var center_id = center == null ? 0 : center.Id;
            var center_type = center == null ? (int)CompanyTypeEnum.Arena : center.Company_Type_Id;
            var center_name = center == null ? "" : center.Company_Name_Abbrev;

            //create claim basic.
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid, acc.Id.ToString()),
                    new Claim(ClaimTypes.Sid, acc.User_Profile_Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, acc.Account_Username),
                    new Claim(ClaimTypes.Name, acc.Account_Name),
                    new Claim(ClaimTypes.Email, acc.Account_Email),
                    new Claim(ClaimTypes.UserData, acc.Account_Avatar),
                    new Claim(ClaimTypes.Role, user_type),
                    new Claim(ClaimTypes.Anonymous, acc.Status.ToString()),
                    new Claim(ClaimTypes.IsPersistent, acc.HasSetPassword.ToString()),

                    //others
                    new Claim(ClaimTypes.PrimaryGroupSid, center_id.ToString()),
                    new Claim(ClaimTypes.GroupSid, center_type.ToString()),
                    new Claim(ClaimTypes.GivenName, center_name.ToString()),
                };

            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[USER_SESSION] = claims;
            }
        }

        public static void Logout()
        {
            //Delete session
            HttpContext.Current.Session[USER_SESSION] = null;
        }

        //Get Claims
        public static int GetClaimInt32(string claimType)
        {
            return int.Parse(GetClaim(claimType) ?? "0");
        }

        public static bool GetClaimBoolean(string claimType)
        {
            return bool.Parse(GetClaim(claimType));
        }

        public static string GetClaim(string claimType)
        {
            if (HttpContext.Current.Session != null)
            {
                var acc = HttpContext.Current.Session[USER_SESSION] as List<Claim>;
                if (acc != null)
                {
                    var claim = acc.FirstOrDefault(c => c.Type == claimType);
                    if (claim != null)
                    {
                        return claim.Value;
                    }
                }
            }
            return null;
        }

        public static bool IsAuthorized(RoleTypeEnum type)
        {
            return IsAuthenticated && AuthenticationManager.Role_Type_Id == (int)type;
        }

        public static bool IsAdmin
        {
            get
            {
                return IsAuthorized(RoleTypeEnum.Admin);
            }
        }

        public static bool IsMod
        {
            get
            {
                return IsAuthorized(RoleTypeEnum.Mod) || IsAdmin;
            }
        }

        //public static bool IsUser
        //{
        //    get
        //    {
        //        return AuthenticationSession.GetClaim(ClaimTypes.Role) == "User";
        //    }
        //}

        /// <summary>
        /// Is type || IsMod
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Is(RoleTypeEnum type)
        {
            return IsAuthorized(type) || IsMod;
        }
    }
}