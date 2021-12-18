using Gmt_Asset_Tracker.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker.Models
{
    public class Asset
    {
        // GENERAL
        public int Id { get; set; }

        [Display(Name = "Asset Name")]
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Asset_name { get; set; }
        [Display(Name = "Description")]
        //[Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Asset_description { get; set; }

        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a category")]
        public int CategoryId { get; set; }

        [Display(Name = "Location")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a location")]
        public int LocationId { get; set; }

        [Display(Name = "Department")]
        [Range(1, int.MaxValue, ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }
        
        [Display(Name = "Asset State")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose an asset state")]
        public int AssetStateId { get; set; }

        [Display(Name = "Asset Tag")]
        [Required]
        public string Asset_tag { get; set; }

        [Display(Name = "Service Tag")]
        public string Service_tag { get; set; }

        [Display(Name = "Assigned To")]
        public string Assigned_to { get; set; }
        
        //FINANCIAL

        [Display(Name = "Purchased Price")]
        public string Purchased_price { get; set; }

        [Display(Name = "Vendor")]
        [Required]
        public int VendorId { get; set; } = 1;

        [Display(Name = "Delivery Date")]
        [Required]
        public DateTime Delivery_date { get; set; }

        [Display(Name = "Requisition Pack")]
        public string Requistion_pack { get; set; }

        [NotMapped]
        [FileExtension]
        [Display(Name = "Upload Asset Requisition Pack")]
        public IFormFile RequistionpackUpload { get; set; }

        // ASSET VERIFICATION
        [Display (Name = "Physical Check")]
        public int CheckId { get; set; }

        public string Image { get; set; }

        [Display (Name = "Present Location")]
        //[Required]
        public int? PresentLocationId { get; set; }

        [Display (Name = "Present User")]
        public string Present_user { get; set; }

        [NotMapped]
        [FileExtension]
        [Display(Name = "Choose Image")]
        public IFormFile ImageUpload { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("LocationId")]
        
        public virtual Location Location { get; set; }

        [ForeignKey("PresentLocationId")]
        [Display(Name = "Present Location")]
        public virtual Location Present_location { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("AssetStateId")]
        [Display(Name = "Asset State")]
        public virtual Asset_State Asset_State { get; set; }

        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }

        [ForeignKey("CheckId")]
        [Display(Name = "Physical Check")]
        public virtual Physical_check Physical_check { get; set; }
        
       
    }
}
