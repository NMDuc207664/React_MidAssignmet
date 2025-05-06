using Library_API_2._0.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_API_2._0.Infrastructure.Identity
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData(
            new Genre
            {
                Id = Guid.Parse("9BD8855A-634C-4248-8641-F7FEF2DC97C3"),
                Name = "Đang cập nhật",
            }
         );
        }
    }
}