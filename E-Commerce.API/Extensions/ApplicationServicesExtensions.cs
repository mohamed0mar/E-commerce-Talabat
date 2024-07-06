using E_Commerce.API.Errors;
using E_Commerce.API.Helpers;
using E_Commerce.Core;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Repository;
using E_Commerce.Repository.Basket_Repository;
using E_Commerce.Service.Auth_Service;
using E_Commerce.Service.Cache_Service;
using E_Commerce.Service.Order_Service;
using E_Commerce.Service.Payment_Service;
using E_Commerce.Service.Product_Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Commerce.API.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

			services.AddAutoMapper(typeof(MappingProfiles));

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var erroes = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
													   .SelectMany(P => P.Value.Errors)
													   .Select(E => E.ErrorMessage)
													   .ToList();
					var response = new ApiValidationErrorResponse()
					{
						Errors = erroes
					};
					return new BadRequestObjectResult(response);
				};
			});

			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
			services.AddScoped(typeof(IAuthService), typeof(AuthService));
			services.AddScoped(typeof(IOrderService), typeof(OrderService));
			services.AddScoped(typeof(IProductService), typeof(ProductService));
			services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
			services.AddSingleton(typeof(IResponseService), typeof(ResponseService));

			return services;
		}

		public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
		{


			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"])),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
					};
				});

			return services;

		}

	}
}
