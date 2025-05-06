using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Domain.Entities;
using Library_API_2._0.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library_API_2._0.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BorrowingDetail> BorrowingDetails { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<BorrowingRequest> BorrowingRequests { get; set; }
        public DbSet<BorrowingRecord> Records { get; set; }
        public new DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
    .HasKey(ba => new { ba.AuthorId, ba.BookId });
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookGenre>()
                .HasKey(bg => new { bg.GenreId, bg.BookId });
            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowingDetail>()
                .HasKey(bd => new { bd.BookId, bd.BorrowingRequestId });
            modelBuilder.Entity<BorrowingDetail>()
                .HasOne(bd => bd.Book)
                .WithMany(b => b.BorrowingDetails)
                .HasForeignKey(bd => bd.BookId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BorrowingDetail>()
                .HasOne(bd => bd.BorrowingRequest)
                .WithMany(br => br.BorrowingDetails)
                .HasForeignKey(bd => bd.BorrowingRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BorrowingRequest>()
                .HasOne(r => r.User)
                .WithMany(u => u.BorrowingRequests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BorrowingRecord>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(r => r.BorrowingRequest)
                .WithOne()
                .HasForeignKey<BorrowingRecord>(r => r.BorrowingRequestId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(r => r.User)
                .WithMany(u => u.BorrowingRecords)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(r => r.PickUpAdmin)
                .WithMany()
                .HasForeignKey(r => r.PickUpAdminId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(r => r.ReturnAdmin)
                .WithMany()
                .HasForeignKey(r => r.ReturnAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfigration());
            base.OnModelCreating(modelBuilder);

        }
    }
}