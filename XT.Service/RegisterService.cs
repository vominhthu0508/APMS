using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public abstract class RegisterService<U> : Service<U, Int32> where U : class, IRegisterEntity<Int32>
    {
        public RegisterService(IUow uow)
            : base(uow)
        {
        }

        public bool CheckExistEmail(string email, int id = 0)
        {
            if (FindAllByCriteria(u => u.Obj_Id != id && u.Email.ToLower() == email.ToLower()).FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }

        public EmailInfo GetEmailInfo(U u)
        {
            return new EmailInfo
            {
                Name = u.Name,
                Email = u.Email
            };
        }

        //public void Active(U u)
        //{
        //    u.Status = (int)EntityStatus.Visible;
        //    Update(u);
        //}
    }
}
