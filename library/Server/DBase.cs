using IdentityServer4.EntityFramework.Options;
using library.Shared;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library.Server
{
    public class DBase : ApiAuthorizationDbContext<IdentityUser>
    {
        public DbSet<User> UsersOverrided { get; set; }
        
        public DBase(DbContextOptions<DBase> options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
			//seeding master admin 
			const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
			const string ROLE_ID = "b18be9c0-aa65-4af8-bd17-00bd9344e575";
			modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = ROLE_ID, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "ZKLJ" },
				new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER", ConcurrencyStamp = "ZKLJ" }
				);


			var hasher = new PasswordHasher<IdentityUser>();

			modelBuilder.Entity<IdentityUser>().HasData(
				new IdentityUser
				{
					Id = ADMIN_ID,
					UserName = "admin",
					NormalizedUserName = "ADMIN",
					PasswordHash = hasher.HashPassword(null, "ASDqwe12!")
				});
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
			{
				RoleId = ROLE_ID,
				UserId = ADMIN_ID
			});
		}
    }
}
