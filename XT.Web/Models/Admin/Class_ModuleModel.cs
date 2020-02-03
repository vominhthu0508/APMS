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
    public class Class_ModuleModel : Class_Module
    {
        public string ErrorMessage { get; set; }
        //Status = (int)EntityStatus.Visible
        public override IEntity ToModel()//add
        {
            var model = new Class_Module();

            return ToModel(model);
        }

        public override IEntity ToModel(IEntity _model)//edit: _model = old
        {
            var model = _model as Class_Module;//old

            //check ton tai
            var existed = IoCConfig.Service<IClass_ModuleService>()
                .FindValidByCriteria(c => c.Class_Id == this.Class_Id && c.Module_Id == this.Module_Id);
            if (existed != null)
            {
                ErrorMessage = "Lớp đã học môn này với FC " + existed.Faculty.FC_Name + " rồi!";
                return null;
            }

            model.CopyModel(this);

            var class_id = model.Class_Id;
            if (class_id != null)
            {
                var current_class = IoCConfig.Service<IClassService>().FindById(class_id.Value);
                if (current_class != null && current_class.IsValid())
                {
                    model.GenerateSchedule(current_class);
                }
            }

            return model;
        }
    }
}