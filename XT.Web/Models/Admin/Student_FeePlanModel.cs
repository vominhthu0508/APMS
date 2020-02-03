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
    public class Student_FeePlanModel : Student_FeePlan
    {
        //Status = (int)EntityStatus.Visible
        public override IEntity ToModel()//add
        {
            var model = new Student_FeePlan();

            return ToModel(model);
        }

        public override IEntity ToModel(IEntity _model)//edit
        {
            var model = _model as Student_FeePlan;
            var currentFeePlanId = model.FeePlan_Id;

            model.CopyModel(this);

            if (model.Id == 0 || currentFeePlanId != FeePlan_Id)
            {
                var currentFeePlan = IoCConfig.Service<IFeePlanService>().FindById(FeePlan_Id);
                if (currentFeePlan != null && currentFeePlan.IsValid())
                {
                    model.SetupFeePlanInstallments(currentFeePlan);
                }
            }

            return model;
        }
    }
}