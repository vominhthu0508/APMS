using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using XT.BusinessService;
using XT.Model;

namespace XT.Web.Models
{
    public class RegisterModel<U> : IValidatableObject where U : class, IEntity<Int32>
    {
        //public int Id { get; set; }

        //[StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} kí tự.", MinimumLength = 6)]
        //[RegularExpression(@"^(?=.{3,100}$)([A-Za-z0-9][._()\[\]-]?)*$", ErrorMessage =
        //    "{0} không hợp lệ")]
        //[Required(ErrorMessage = "Tên đăng nhập bắt buộc")]
        //[Display(Name = "Username")]
        public string Username { get; set; }

        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} kí tự.", MinimumLength = 7)]
        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
        [Required(ErrorMessage = "Mật khẩu nhập lại bắt buộc")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email bắt buộc")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(30)]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Họ tên bắt buộc")]
        public string Name { set; get; }

        public bool isAgree { get; set; }

        public virtual U ToModel(bool hasAlreadyVerified = false)
        {
            return null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Username = Email;
            List<ValidationResult> res = new List<ValidationResult>();
            if (!isAgree)
            {
                ValidationResult mss = new ValidationResult("Vui lòng đồng ý với điều khoản trước khi đăng ký!", new[] { "CustomError" });
                
                res.Add(mss);
            }
            return res;
        }
    }

    public class RegisterUserModel : RegisterModel<User_Profile>
    {
        public override User_Profile ToModel(bool hasAlreadyVerified = false)
        {
            return new User_Profile()
            {
                User_Profile_Name = Name,
                User_Profile_Email = Email,
                User_Profile_Avatar = AppSettings.DefaultAccountAvatar,
                Role_Type_Id = (int)RoleTypeEnum.User,
                User_Type_Id = (int)UserTypeEnum.User
            };
        }
    }
}