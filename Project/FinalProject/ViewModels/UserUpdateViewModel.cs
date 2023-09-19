using FinalProject.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
	public class UserUpdateViewModel:BaseModel
	{
		public IFormFile? file { get; set; }
		[Required]
		public string? UserName { get; set; }
		[Required]
		[EmailAddress]
		public string? Email { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public string? Surname { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string? CurrentPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string? NewPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare("NewPassword")]
		public string? ConfirmNewPassword { get; set; }
	}
}
