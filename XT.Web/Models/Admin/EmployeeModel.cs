using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;

namespace XT.Web.Models
{
    public class EmployeeModel : User_Profile
    {
        public override IEntity ToModel()//add
        {
            var model = new User_Profile();
            return ToModel(model);
        }

        //Status = (int)EntityStatus.Visible
        public override IEntity ToModel(IEntity _model)//edit: _model = old
        {
            var model = _model as User_Profile;//old

            model.CopyModel(this);
            
            //Class Change
            if (AuthenticationManager.Company_Id > 0)
            {
                model.User_Companies.Add(new User_Company
                {
                    Company_Id = AuthenticationManager.Company_Id,
                    Created_Date = DateTime.Now,
                });
            }

            return model;
        }
    }
}