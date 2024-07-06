using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Auth_Service
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			var authClaims=new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.DisplayName),
				new Claim(ClaimTypes.Email,user.Email),
			};

			var userRoles = await userManager.GetRolesAsync(user);
			foreach (var role in userRoles)
				authClaims.Add(new Claim(ClaimTypes.Role, role));

			//AuthKey
			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"]));
			//Registert Claims

			var token = new JwtSecurityToken(
				audience: _configuration["Jwt:ValidAudience"],
				issuer: _configuration["Jwt:ValidIssuer"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
