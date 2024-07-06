using E_Commerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository._Identity
{
	public static class ApplicationIdentityContextSeed
	{
		public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Mohamed",
					Email = "mohamedmahmoud2572000@gmail.com",
					UserName = "mohamed.mahmoud",
					PhoneNumber = "01112055979"
				};

				await userManager.CreateAsync(user, "P@ssw0rd");
			}
		}
	}
}
