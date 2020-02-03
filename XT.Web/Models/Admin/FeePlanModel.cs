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
    public class FeePlanModel : FeePlan
    {
        //Status = (int)EntityStatus.Visible
        public override IEntity ToModel()//add
        {
            var model = new FeePlan();

            if (this.FeePlan_Count > 0)
            {
                int amount = this.FeePlan_Price / this.FeePlan_Count;
                for (int i = 1; i <= this.FeePlan_Count; i++)
                {
                    model.FeePlan_Details.Add(new FeePlan_Detail
                    { 
                        FeePlan_Index = i,
                        FeePlan_Amount = amount,
                        Status = (int)EntityStatus.Visible
                    });
                }
            }

            return ToModel(model);
        }

        public override IEntity ToModel(IEntity _model)//edit
        {
            var model = _model as FeePlan;
            model.CopyModel(this);

            return model;
        }
    }
}