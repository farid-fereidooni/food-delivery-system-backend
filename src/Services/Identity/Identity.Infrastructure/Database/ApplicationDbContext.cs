using Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace Identity.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().Property(p => p.FirstName).HasMaxLength(200).IsRequired(false);
        modelBuilder.Entity<ApplicationUser>().Property(p => p.LastName).HasMaxLength(200).IsRequired(false);
        modelBuilder.Entity<ApplicationUser>().Ignore(p => p.FullName);
        modelBuilder.Entity<ApplicationUser>().ToTable("users");

        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("roles");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");

        modelBuilder.Entity<OpenIddictEntityFrameworkCoreApplication>().ToTable("applications");
        modelBuilder.Entity<OpenIddictEntityFrameworkCoreScope>().ToTable("scopes");
        modelBuilder.Entity<OpenIddictEntityFrameworkCoreAuthorization>().ToTable("authorizations");
        modelBuilder.Entity<OpenIddictEntityFrameworkCoreToken>().ToTable("tokens");
    }
}
