using System.ComponentModel.DataAnnotations;

namespace Gmt_Asset_Tracker.Models
{
    public class Asset_State
    {
        public int Id { get; set; }

        [Display(Name = "Asset State")]
        public string Asset_state { get; set; }
    }
}
