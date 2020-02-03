using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XT.Web.Models
{
    public class RecoverPasswordModel
    {
        [Required(ErrorMessage = "Email bắt buộc")]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Username { get; set; }
    }
}