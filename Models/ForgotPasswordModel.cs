using System.ComponentModel.DataAnnotations;

namespace Gmt_Asset_Tracker.Models
{
	public class ForgotPasswordModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}