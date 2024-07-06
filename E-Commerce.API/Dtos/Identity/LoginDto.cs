using System.ComponentModel.DataAnnotations;

namespace E_Commerce.API.Dtos.Identity
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;
		[Required]
		public string Password { get; set; } = null!;
	}
}
