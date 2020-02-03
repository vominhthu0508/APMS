using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class UserTypeService : Service<User_Type, Int32>, IUserTypeService
    {
        public UserTypeService(IUow uow)
            : base(uow)
        {
        }
    }
}
