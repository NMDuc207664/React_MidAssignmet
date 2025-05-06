using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_API_2._0.Infrastructure.Identity
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
             new IdentityRole
             {
                 Id = "639de03f-7876-4fff-96ec-37f8bd3bf180",
                 Name = "User",
                 NormalizedName = "USER"
             },
             new IdentityRole
             {
                 Id = "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f",
                 Name = "Admin",
                 NormalizedName = "ADMIN"
             }
            );

        }
    }
}