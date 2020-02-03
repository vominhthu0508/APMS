using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XT.Model;
using XT.Web.External;

namespace XT.Web.Models
{
    public class ChangeProfileModel<U> where U : class, IEntity<Int32>
    {
        public int Id { get; set; }
        public DateTime Created_Date { get; set; }
        public int Status { get; set; }

        [Required(ErrorMessage = "Email bắt buộc")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Họ tên bắt buộc")]
        [Display(Name = "Họ tên")]
        [StringLength(100)]
        public string Name { get; set; }

        public string Avatar { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }

        public virtual void FromModel(U u)
        {
            
        }

        public virtual U ToModel(U u)
        {
            return null;
        }
    }

    public class ChangeUserProfileModel : ChangeProfileModel<User_Profile>
    {
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        //Constructor

        public ChangeUserProfileModel()
        {
        }

        public override void FromModel(User_Profile user)
        {
            Id = user.Id;
            Name = user.User_Profile_Name;
            Email = user.User_Profile_Email;
            Avatar = user.User_Profile_Avatar;
            Created_Date = user.Created_Date;
            Status = user.Status;
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        //ToModel

        public override User_Profile ToModel(User_Profile u)
        {
            //if (Id == 0)//không up hình thì không change
            //{
            //    Avatar = AppSettings.DefaultAccountAvatar;
            //}
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                u.User_Profile_Avatar = Helper.SaveAs(AppSettings.UploadUserPhotos, uploadFile);
            }

            u.User_Profile_Name = Name;
            u.User_Profile_Email = Email;

            return u;
        }
    }
}