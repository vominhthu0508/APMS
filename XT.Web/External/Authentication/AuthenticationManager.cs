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

namespace XT.Web.External
{
    public partial class AuthenticationManager
    {
        #region IsAuthenticated
        public static bool IsAuthenticated
        {
            get
            {
                //only session
                return AuthenticationSession.IsAuthenticated;

                //if (AuthenticationSession.IsAuthenticated) || AuthenticationCookie.IsAuthenticated)
                //{
                //    return true;
                //}
                //return false;
            }
        }

        /// <summary>
        /// Is basic authorized used for AuthorizeCore
        /// </summary>
        public static bool IsBasicAuthorized
        {
            get
            {
                return IsAuthenticated;// && Account_Type_Id != (int)AccountTypeEnum.User;
            }
        }

        /// <summary>
        /// Is Just Authorized for type (not IsAdmin)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAuthorized(RoleTypeEnum type)
        {
            return AuthenticationSession.IsAuthorized(type);
        }

        /// <summary>
        /// Is Authorized but not id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsNotAuthorized(int id)
        {
            return IsAuthorized(RoleTypeEnum.User) && User_Profile_Id != id;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        //Is Authorized for Types

        public static bool IsAdmin
        {
            get
            {
                return AuthenticationSession.IsAdmin;
                //return true;
                //return Is(AccountTypeEnum.Admin);
                //return (IsAuthenticated && AuthenticationSession.IsAdmin);
            }
        }

        public static bool IsMod
        {
            get
            {
                return AuthenticationSession.IsMod;
                //return Is(AccountTypeEnum.Mod);
                //return (IsAuthenticated && AuthenticationSession.IsMod);
            }
        }

        /// <summary>
        /// Is type || IsMod
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Is(RoleTypeEnum type)
        {
            return AuthenticationSession.Is(type);
        }
        #endregion

        #region Profile Fields

        public static int Id
        {
            get { return AuthenticationSession.GetClaimInt32(ClaimTypes.PrimarySid); }
        }

        public static int User_Profile_Id
        {
            get { return AuthenticationSession.GetClaimInt32(ClaimTypes.Sid); }
        }

        public static string Account_Username
        {
            get { return AuthenticationSession.GetClaim(ClaimTypes.NameIdentifier); }
        }

        public static string Account_Email
        {
            get { return AuthenticationSession.GetClaim(ClaimTypes.Email); }
        }

        public static string Account_Avatar
        {
            get { return AuthenticationSession.GetClaim(ClaimTypes.UserData); }
        }

        public static string Account_Name
        {
            get {
                int MAX_NAME = 20;
                var name = AuthenticationSession.GetClaim(ClaimTypes.Name);
                return name != null && name.Length > MAX_NAME ? name.Substring(0, MAX_NAME) : name;
            }
        }

        public static string Account_Name_Center
        {
            get {
                return Account_Name + " (" + Company_Name + ")";
            }
        }

        /// <summary>
        /// Để check role trong header menu
        /// </summary>
        public static int Role_Type_Id
        {
            get { return AuthenticationSession.GetClaimInt32(ClaimTypes.Role); }
        }

        //public static string Account_Type_Name
        //{
        //    get { return AuthenticationSession.GetClaim(ClaimTypes.Role); }
        //}

        public static int Account_Status
        {
            get { return AuthenticationSession.GetClaimInt32(ClaimTypes.Anonymous); }
        }

        public static bool Account_HasSetPassword
        {
            get { return AuthenticationSession.GetClaimBoolean(ClaimTypes.IsPersistent); }
        }

        public static int Company_Id
        {
            get { return AuthenticationSession.GetClaimInt32(ClaimTypes.PrimaryGroupSid); }
        }

        public static int Company_Type_Id
        {
            get { return AuthenticationSession.GetClaimInt32(ClaimTypes.GroupSid); }
        }

        public static string Company_Name
        {
            get { return AuthenticationSession.GetClaim(ClaimTypes.GivenName); }
        }
        
        #endregion

        public static void SetAuthentication(Account acc)
        {
            AuthenticationSession.SetAuthentication(acc);
            //AuthenticationCookie.SetAuthentication(acc);//for admin CP only
        }

        public static void Logout()
        {
            AuthenticationSession.Logout();
            //AuthenticationCookie.Logout();
        }

        public static void SetAuthenticationCenter(Account acc, Company center = null)
        {
            AuthenticationSession.SetAuthentication(acc, center);
        }
    }
}