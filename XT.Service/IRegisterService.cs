using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public class EmailInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public interface IRegisterService<U> : IService<U, Int32> where U : class, IRegisterEntity<Int32>
    {
        bool CheckExistEmail(string email, int id = 0);//Check exist email trong phần đăng ký Add
        EmailInfo GetEmailInfo(U u);//Lấy thông tin để send email trong phần Active hay Adding của AccountManagementItem
        //void Active(U u);//Active Account
    }
}
