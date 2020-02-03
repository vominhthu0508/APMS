using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XT.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email bắt buộc")]
        [Display(Name = "Email")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}