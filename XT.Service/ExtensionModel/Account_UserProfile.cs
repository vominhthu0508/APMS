using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.Model
{
    //class được tạo ra cho riêng phần ManageAccount của AdminController
    public class Account_UserProfile
    {
        public Account Account { get; set; }
        public User_Profile User_Profile { get; set; }
    }
}
