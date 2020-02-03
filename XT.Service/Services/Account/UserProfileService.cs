using XT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public class UserProfileService : RegisterService<User_Profile>, IUserProfileService
    {
        public UserProfileService(IUow uow)
            : base(uow)
        {
        }

        ///////////////////////////////////////////////////////////////////////////

        public User_Profile getUserProfileByEmail(string email, int id = 0)
        {
            return FindByCriteria(a => a.User_Profile_Email.ToLower() == email.ToLower()
                && a.Id != id);
        }

        public User_Profile getUserBySocialId(string id)
        {
            return FindByCriteria(a => a.User_Profile_Facebook == id);
        }

        public override User_Profile Update(User_Profile profile)
        {
            foreach (var acc in profile.Accounts)
            {
                acc.UpdateProfile(profile);
            }

            return base.Update(profile);
        }

        public override void Delete(User_Profile profile)
        {
            foreach (var acc in profile.Accounts)
            {
                GetRepository<Account>().Delete(acc);
            }

            base.Delete(profile);
        }

        public void Active(User_Profile profile)
        {
            foreach (var acc in profile.Accounts)
            {
                GetRepository<Account>().Active(acc);
            }

            base.Active(profile);
        }

        public bool DeleteUser(int id)
        {
            Delete(id);

            var account = GetRepository<Account>().FindByCriteria(a => a.User_Profile_Id == id);
            if (account != null)
            {
                GetRepository<Account>().Delete(account);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Active User Profile & Active Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ActiveUser(int id)
        {
            Active(id);

            var account = GetRepository<Account>().FindByCriteria(a => a.User_Profile_Id == id);
            if (account != null)
            {
                GetRepository<Account>().Active(account);
            }

            return true;
        }
    }
}
