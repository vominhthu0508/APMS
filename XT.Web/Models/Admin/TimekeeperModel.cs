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
    public partial class TimekeeperModel : Timekeeper, IValidatableObject
    {
        [Required(ErrorMessage = "Chụp hình bắt buộc!")]
        public string imageData { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();

            var existed = IoCConfig.Service<ITimekeeperService>().CheckExistTimekeeper(this, TimeKeeperHelper.GetMaxWaitingMinutes());
            if (existed != null)
            {
                res.Add(new ValidationResult("Bạn vừa mới checkin rồi"));
            }

            return res;
        }

        public override IEntity ToModel()//add
        {
            var model = new Timekeeper
            {
                Photo = AppSettings.DefaultAccountAvatar
            };

            return ToModel(model);
        }

        public override IEntity ToModel(IEntity _model)//edit: _model = old
        {
            //pre-processing: this
            Company_Id = AuthenticationManager.Company_Id;
            User_Id = AuthenticationManager.User_Profile_Id;
            Checkin_Date = DateTime.Now;
            IP_Modem = HttpContext.Current.Request.UserHostAddress;

            if (imageData != null && imageData.Length > 1)
            {
                var date = Checkin_Date.ToString("yyyyddMMHHmmss");
                var fileName = "CheckTimekeeper_Web_" + AuthenticationManager.Account_Username + "_" + User_Id + "_" + date + ".jpg";
                Photo = Helper.SaveAs(AppSettings.UploadImagesAdmin, imageData, fileName);
            }

            var model = _model as Timekeeper;
            model.CopyModel(this);

            //post-processing (cho foreign references)

            return model;
        }
    }
}