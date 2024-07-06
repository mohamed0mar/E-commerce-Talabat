using E_Commerce.API.Extensions;
using E_Commerce.API.Middleware;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Repository._Identity;
using E_Commerce.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace E_Commerce.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configer Service
			// Add services to the container.

			builder.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});

			builder.Services.AddSwaggerServices();

			builder.Services.AddDbContext<StoreContext>(options =>
			{
				options/*.UseLazyLoadingProxies()*/.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddDbContext<ApplicationIdentityContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationIdentityContext>();

			builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			builder.Services.AddApplicationServices();

			builder.Services.AddAuthServices(builder.Configuration);

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", policyOptions =>
				{
					//i know its wrong to use localhost here and i should save it in appsetting 
					//but i try a lot and not work 
					policyOptions.WithOrigins("http://localhost:4200")
					   .AllowAnyHeader()
					   .AllowAnyMethod();
				});
			});

			
			#endregion

			var app = builder.Build();

			#region Apply Maigrations | DataSeed

			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			//Ask CLR to create object from StoreContext | ApplicationIdentityContext Exceplicitly
			var _dbContext = services.GetRequiredService<StoreContext>();
			var _identityDbContext = services.GetRequiredService<ApplicationIdentityContext>();

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync();

				await _identityDbContext.Database.MigrateAsync();

				await StoreContextSeed.SeedAsync(_dbContext);

				var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				await ApplicationIdentityContextSeed.SeedUserAsync(_userManager);

			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "There Is An Error Occured During You Apply Maigration");
			}

			#endregion


			#region Configer | Middelwere |Pipline
			// Configure the HTTP request pipeline.
			app.UseMiddleware<ExceptionMiddleware>();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			// CORS policy
			app.UseCors("MyPolicy");

			// Authentication and Authorization
			app.UseAuthentication();
			app.UseAuthorization();

			// Error handling
			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.MapControllers();

			#endregion

			app.Run();
		}
	}
}
