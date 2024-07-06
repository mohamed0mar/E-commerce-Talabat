using AutoMapper;
using E_Commerce.API.Dtos.Identity;
using E_Commerce.API.Errors;
using E_Commerce.API.Extensions;
using E_Commerce.API.Helpers;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Services.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{

	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IAuthService _authService;
		private readonly IMapper _mapper;

		public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
			IAuthService authService,
			IMapper mapper
            )
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_authService = authService;
			_mapper = mapper;
		}

		[HttpPost("login")] //POST : /api/Account/login
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user=await _userManager.FindByEmailAsync(model.Email);
			if (user is null)
				return Unauthorized(new ApiResponse(401, "Invalid Login"));
			var result=await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
			if (!result.Succeeded) 
				return Unauthorized(new ApiResponse(401, "Invalid Login"));
			return Ok(new UserDto()
			{
				DisplayName=user.DisplayName,
				Email=user.Email,
				Token= await _authService.CreateTokenAsync(user, _userManager)
			});
		}

		
		[HttpPost("register")] //POST : /api/Account/register 
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var user = new ApplicationUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split("@")[0],
				PhoneNumber = model.Phone
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = "Token"
			});

		}


		[Cached(600)]
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email =  User.FindFirstValue(ClaimTypes.Email);

			var user=await _userManager.FindByEmailAsync(email);
			return Ok(new UserDto
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authService.CreateTokenAsync(user, _userManager)
			});
		}

		[Cached(600)]
		[Authorize]
		[HttpGet("address")]
		public async Task<ActionResult<AddressDto>> GetUserAddress()
		{
			var user = await _userManager.FindUserWithAddressAsync(User);
			return Ok(_mapper.Map<Address, AddressDto>(user.Address));
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPut("address")]
		public async Task<ActionResult<Address>> UpdateUserAddress(AddressDto address)
		{
			var updatedAddress = _mapper.Map<Address>(address);

			var user = await _userManager.FindUserWithAddressAsync(User);

			updatedAddress.Id = user.Address.Id;

			user.Address = updatedAddress;

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
				return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });
			return Ok(address);
		}
	}
}
