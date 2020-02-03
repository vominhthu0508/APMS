using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    //1. Đăng ký => invisible => active => visible
    //1. Đăng ký => check trùng email
    //    + Cho phép dùng lại email của account đã xóa => find all visible
    //    + Những user chưa active => invisible => không dc tính => bị mất email
    //    => vô lý!

    //2. Đăng ký thành công => Inactive => set active => Visible
    //3. Lúc đăng ký 
    //    => check trùng username => find all alive (visible + inactive) (not invisible/deleted)
    //    => check trùng email trong bảng profile => find all visible profile
    //4. Xóa profile
    //    + Xóa account
    //    + Xóa profile
    public interface IAccountService : IService<Account, Int32>
    {
        IEnumerable<Account> FindAllAlive();

        /// Get account by username (get từ FindAllAlive)
        /// - Dùng trong RecoverPassword & RecoverPasswordFinish
        /// Dùng vì sẽ CheckAccountError để trả về lỗi thông báo phù hợp (null, IsInactive, IsValid)
        /// - Dùng trong PasswordEncryptManager/Login
        /// Không quan tâm active chưa, chỉ cần đúng pass
        Account GetAccountByIdentity(string identity);//Get Account By Username
        Account GetAccountByProfileId(int profileId);
        Account GetAccountByActiveKey(string key);

        Account Create(string username, string password, int profile_id, IRegisterEntity<Int32> info = null);
        Account UpdateStatus(Account account);
        Account UpdateAccount(Account account);
        Account UpdateAccount(Account account, IRegisterEntity<Int32> info);
        Account UpdateAccount(User_Profile profile);
        bool DeleteByProfile(int profileId);
        bool DeleteAccount(int id);
    }
}
