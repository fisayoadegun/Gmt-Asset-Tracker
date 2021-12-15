using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker.Models
{
    public class Physical_check
    {
        public int Id { get; set; }

        [Display (Name = "Physical Check")]
        public string Check_name { get; set; }
    }
}
