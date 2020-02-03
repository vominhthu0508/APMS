using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XT.Web.Models
{
    public class RecoverPasswordFinishModel
    {
        [Required(ErrorMessage = "Username bắt buộc")]
        [Display(Name = "Username")]
        [StringLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mã khôi phục bắt buộc")]
        [Display(Name = "Mã khôi phục")]
        [StringLength(200)]
        public string RecoverKey { get; set; }

        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} kí tự.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu nhập lại bắt buộc")]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} kí tự.", MinimumLength = 7)]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}