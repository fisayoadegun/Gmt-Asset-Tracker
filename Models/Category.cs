using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        public string Category_name { get; set; }
    }
}
