using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface IUserProfileService : IRegisterService<User_Profile>
    {
        User_Profile getUserProfileByEmail(string email, int id = 0);
        User_Profile getUserBySocialId(string id);

        bool DeleteUser(int id);
        bool ActiveUser(int id);
    }
}
