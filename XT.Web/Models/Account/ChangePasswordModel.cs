using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XT.Web.Models
{
    public class ChangePasswordModel : IValidatableObject
    {
        //[Required(ErrorMessage = "Mật khẩu cũ bắt buộc")]
        //[StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} kí tự.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới bắt buộc")]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} kí tự.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu nhập lại bắt buộc")]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} kí tự.", MinimumLength = 7)]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu nhập lại")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (XT.Web.External.AuthenticationManager.Account_HasSetPassword)
            {
                if (string.IsNullOrEmpty(OldPassword))
                {
                    var mss = new ValidationResult("Mật khẩu cũ bắt buộc");
                    res.Add(mss);
                }
                else if (OldPassword.Length < 7)
                {
                    var mss = new ValidationResult("Mật khẩu cũ phải có ít nhất 7 kí tự.");
                    res.Add(mss);
                }
            }
            return res;
        }
    }
}