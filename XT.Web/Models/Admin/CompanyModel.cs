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
    public class CompanyModel : Company
    {
        public HttpPostedFileBase uploadFile { get; set; }

        public override IEntity ToModel()//add
        {
            var model = new Company();
            return ToModel(model);
        }

        public override IEntity ToModel(IEntity _model)//edit
        {
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                this.Company_Logo = Helper.SaveAs(AppSettings.UploadImagesAdmin, uploadFile);
            }
            var model = _model as Company;
            model.CopyModel(this);
            //if (uploadFile != null && uploadFile.ContentLength > 0)
            //{
            //    model.Company_Logo = Helper.SaveAs(AppSettings.UploadImagesAdmin, uploadFile);
            //}

            return model;
        }
    }
}