using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookStoreApp.API.Data
{
    public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
    {
        public BookStoreDbContext()
        {
        }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
	        base.OnModelCreating(modelBuilder);
	        
           modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Bio).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

           modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EADCD8AAAE")
                    .IsUnique();

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Summary).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Books_ToTable");
            });

           modelBuilder.Entity<IdentityRole>().HasData(
	           new IdentityRole
	           {
		           Name = "User",
		           NormalizedName = "USER",
		           Id = "34806b21-f35a-48f6-899f-c1302ea2a220"
	           },
	           new IdentityRole
	           {
		           Name = "Administration",
		           NormalizedName = "ADMINISTRATION",
		           Id = "f4de428c-c5a6-486a-9b48-2664a14e9607"
	           }
	        );
           var hasher = new PasswordHasher<ApiUser>();

           modelBuilder.Entity<ApiUser>().HasData(
	           new ApiUser
              {
	              Id = "903d090a-e862-4b21-8dfc-4d068c262884",
	              Email = "user@bookstore.com",
		           NormalizedEmail = "USER@BOOKSTORE.COM",
		           UserName = "user@bookstore.com",
		           NormalizedUserName = "USER@BOOKSTORE.COM",
                 FirstName = "System",
                 LastName = "User",
                 PasswordHash = hasher.HashPassword(null, "P@ssword1")
              },
	           new ApiUser
              {
                 Id = "d1710f5f-dae2-4095-acee-e12c9784d4d9",
                 Email = "admin@bookstore.com",
                 NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                 UserName = "admin@bookstore.com",
                 NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                 FirstName = "System",
                 LastName = "Admin",
                 PasswordHash = hasher.HashPassword(null, "P@ssword1")

              }
           );

           modelBuilder.Entity<IdentityUserRole<string>>().HasData(
	           new IdentityUserRole<string>
	           {
                 RoleId = "34806b21-f35a-48f6-899f-c1302ea2a220",
                 UserId = "903d090a-e862-4b21-8dfc-4d068c262884"
              },
	           new IdentityUserRole<string>
	           {
		           RoleId = "f4de428c-c5a6-486a-9b48-2664a14e9607",
		           UserId = "d1710f5f-dae2-4095-acee-e12c9784d4d9"
              }
              );

         OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
