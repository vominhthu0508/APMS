using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using XT.Model;
using XT.Web.External;

namespace XT.Web.Models
{
    public class User_ProfileModel
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public int Account_Id { get; set; }

        [Required(ErrorMessage = "Email bắt buộc")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string User_Profile_Email { get; set; }

        [Required(ErrorMessage = "Họ tên bắt buộc")]
        [Display(Name = "Họ tên")]
        [StringLength(100)]
        public string User_Profile_Name { get; set; }

        //[Required(ErrorMessage = "Số điện thoại bắt buộc")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Điện thoại")]
        [StringLength(100)]
        public string User_Profile_Phone { get; set; }

        //[Required(ErrorMessage = "Địa chỉ bắt buộc")]
        public string User_Profile_Address { get; set; }
        public DateTime? User_Profile_Birthday { get; set; }
        public int? User_Profile_Gender { get; set; }
        public string User_Profile_Avatar { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }

        public User_ProfileModel()
        { }

        public User_Profile ToModel(User_Profile u)
        {
            if (u.Id == 0)
            {
                u.User_Profile_Avatar = AppSettings.DefaultAccountAvatar;
            }
            if (uploadFile != null)
            {
                u.User_Profile_Avatar = Helper.SaveAs(AppSettings.UploadUserPhotos, uploadFile);
            }
            u.User_Profile_Name = User_Profile_Name;
            u.User_Profile_Phone = User_Profile_Phone;
            u.User_Profile_Address = User_Profile_Address;
            u.User_Profile_Birthday = User_Profile_Birthday;
            u.User_Profile_Email = User_Profile_Email;
            u.User_Profile_Gender = User_Profile_Gender;
            u.Created_Date = DateTime.Now;
            return u;
        }
    }
}