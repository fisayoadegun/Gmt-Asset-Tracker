using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Display (Name = "Location")]
        public string Location_name { get; set; }
    }
}
