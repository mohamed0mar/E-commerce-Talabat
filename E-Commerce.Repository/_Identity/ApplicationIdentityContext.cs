using E_Commerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository._Identity
{
	public class ApplicationIdentityContext: IdentityDbContext<ApplicationUser>
	{
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options)
            :base(options)
        {
            
        }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Address>().ToTable("Addresses");
		}
	}
}
