using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Display(Name = "Department Name")]
        public string Department_name { get; set; }
    }
}
