using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gmt_Asset_Tracker.Data;
using System.Xml.Linq;
using Gmt_Asset_Tracker.Models;
using Microsoft.AspNetCore.Http;

namespace Gmt_Asset_Tracker.ViewModels
{
	public class AssetViewModel
	{
		public string Id { get; set; }

		[Display(Name = "Asset Name")]
		[Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
		public string Asset_name { get; set; }

		[Display(Name = "Description")]
		public string Asset_description { get; set; }

		[Display(Name = "Category")]
		[Range(1, int.MaxValue, ErrorMessage = "You must choose a category")]
		public string CategoryId { get; set; }

		[Display(Name = "Location")]
		[Range(1, int.MaxValue, ErrorMessage = "You must choose a location")]
		public string LocationId { get; set; }

		[Display(Name = "Department")]
		[Range(1, int.MaxValue, ErrorMessage = "Department is required")]
		public string DepartmentId { get; set; }

		[Display(Name = "Asset State")]
		[Range(1, int.MaxValue, ErrorMessage = "You must choose an asset state")]
		public string AssetStateId { get; set; }

		[Display(Name = "Asset Tag")]
		[Required]
		public string Asset_tag { get; set; }

		[Display(Name = "Service Tag")]
		[Required]
		public string Service_tag { get; set; }

		[Display(Name = "Assigned To")]
		public string Assigned_to { get; set; }

		//FINANCIAL

		[Display(Name = "Purchased Price")]
		public string Purchased_price { get; set; }

		[Display(Name = "Vendor")]
		[Range(1, int.MaxValue, ErrorMessage = "You must choose a vendor")]
		public string VendorId { get; set; }

		[Display(Name = "Delivery Date")]
		[Required]
		public string Delivery_date { get; set; }

		[Display(Name = "Requisition Pack")]
		public string Requistion_pack { get; set; }

		// ASSET VERIFICATION
		[Display(Name = "Physical Check")]
		[Range(1, int.MaxValue, ErrorMessage = "You must choose a check state")]
		public string CheckId { get; set; }

		public string Image { get; set; }

		[Display(Name = "Present Location")]
		[Range(1, int.MaxValue, ErrorMessage = "You must choose a present location")]
		public string PresentLocationId { get; set; }

		[Display(Name = "Present User")]
		public string Present_user { get; set; }

		[NotMapped]
		[FileExtension]
		[Display(Name = "Choose Image")]
		public IFormFile ImageUpload { get; set; }
	}
}