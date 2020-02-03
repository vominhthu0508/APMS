using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class AccountService : Service<Account, Int32>, IAccountService
    {
        public AccountService(IUow uow)
            : base(uow)
        {
        }

        /////////////////////////////////////////////////////////////////////

        //Tìm tất cả alive account - account đang sống (visible/active & inactive)
        public IEnumerable<Account> FindAllAlive()
        {
            return base.FindAll().Where(acc => acc.Status != (int)EntityStatus.Invisible);
        }

        //getAccount By Id
        #region getAccount By Id
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //Hàm dùng cho TRƯỚC ĐĂNG KÝ
        public Account GetAccountByActiveKey(string key)
        {
            return FindAllAlive().FirstOrDefault(a => a.Account_ActiveKey == key);
        }

        //Hàm dùng cho TRƯỚC ĐĂNG KÝ - check trùng username => find all alive (visible + inactive) (not invisible/deleted)
        //public bool CheckExistUsername(string username)
        //{
        //    if (FindAllAlive().FirstOrDefault(acc => acc.Account_Username.ToLower() == username.ToLower()) != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Get account by username (get từ FindAllAlive)
        /// - Dùng trong RecoverPassword & RecoverPasswordFinish
        /// Dùng vì sẽ CheckAccountError để trả về lỗi thông báo phù hợp (null, IsInactive, IsValid)
        /// - Dùng trong PasswordEncryptManager/Login
        /// Không quan tâm active chưa, chỉ cần đúng pass
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public Account GetAccountByIdentity(string identity)
        {
            identity = identity.ToLower();
            return FindAllAlive().FirstOrDefault(u =>
                u.Account_Username.ToLower().Equals(identity)
                || u.Account_Email.ToLower().Equals(identity));
        }

        /// <summary>
        /// Get Account by profile ID (get từ FindAllAlive)
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="account_type"></param>
        /// <returns></returns>
        public Account GetAccountByProfileId(int profileId)
        {
            return FindAllAlive().Where(acc => acc.User_Profile_Id == profileId).FirstOrDefault();
        }

        #endregion

        #region add/edit
        public Account Create(string username, string password, int profile_id, IRegisterEntity<Int32> info = null)
        {
            var account = new Account()
            {
                Account_Username = username,
                Account_Password = password,
                User_Profile_Id = profile_id,
                Status = (int)EntityStatus.UnPublic,//chưa kích hoạt => kích hoạt mới thành Visible
                Account_ActiveKey = Guid.NewGuid().ToString()//key để active
            };

            if (info != null)
            {
                account.Account_Name = info.Name;
                account.Account_Email = info.Email;
                account.Account_Avatar = info.Avatar;
            }

            return account;
        }

        public bool DeleteByProfile(int profileId)
        {
            var acc = GetAccountByProfileId(profileId);
            if (acc != null)
            {
                Delete(acc);
                return true;
            }

            return false;
        }

        public bool DeleteAccount(int id)
        {
            var account = FindById(id);
            if (account != null)
            {
                //delete account
                Delete(account);

                //delete profile
                GetRepository<User_Profile>().Delete(account.User_Profile_Id);

                return true;
            }

            return false;
        }

        public Account UpdateStatus(Account account)
        {
            return Update(account);
        }

        public Account UpdateAccount(Account account)
        {
            return Update(account);
        }

        public Account UpdateAccount(Account account, IRegisterEntity<Int32> info)
        {
            account.Account_Name = info.Name;
            account.Account_Email = info.Email;
            account.Account_Avatar = info.Avatar;
            return Update(account);
        }

        public Account UpdateAccount(User_Profile profile)
        {
            var acc = FindByCriteria(a => a.User_Profile_Id == profile.Id);
            if (acc != null)
            {
                acc.Account_Name = profile.User_Profile_Name;
                acc.Account_Avatar = profile.User_Profile_Avatar;
                acc.Account_Email = profile.User_Profile_Email;

                acc = Update(acc);
            }

            return acc;
        }
        #endregion
    }
}
