using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_API_2._0.Infrastructure.Identity
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {

        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData(
            new Author
            {
                Id = Guid.Parse("B625C683-D11B-4B3D-B440-CFDD51C26E92"),
                Name = "Đang cập nhật",
            }
           );
        }
    }
}