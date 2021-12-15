using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        [Display (Name = "Vendor")]
        public string Vendor_name { get; set; }
    }
}
