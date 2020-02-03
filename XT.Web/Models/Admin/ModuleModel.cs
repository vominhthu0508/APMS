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
    public class ModuleModel : Module
    {
        [Required]
        [StringLength(100)]
        public string Module_Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Module_Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Module_Name_Portal { get; set; }

        [Required]
        [StringLength(100)]
        public string Semester_Name_Portal { get; set; }
    }
}