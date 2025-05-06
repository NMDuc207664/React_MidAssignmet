using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_API_2._0.Infrastructure.Identity
{
    public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "474b5aff-b40e-4743-a777-04163a8b3215",
                    RoleId = "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f"
                }
            );

        }
    }
}