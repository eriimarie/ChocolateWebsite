namespace ChocoShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using Entities;
    using ChocoShop.Extensions;
    using System.Collections.Generic;

    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("Role").HasData(
                new Role { Id = 1, Name = "Administrator" }
               , new Role { Id = 2, Name = "User" });

            modelBuilder.Entity<User>(u =>
            {
                u.ToTable("User");
                u.HasData(new User
                {
                    FirstName = "Choco",
                    LastName = "Admin",
                    Id = 1,
                    Email = "admin@choco.com",
                    Password = "ChocoAdmin".ToHash(),
                    PhoneNumber = "9658968695",
                    IsActive = true,
                    RoleId = 1
                });
            });

            // Configure relationships  
            modelBuilder
            .Entity<User>()
            .HasOne(e => e.Role)
            .WithMany(c => c.Users);



            //Bookings
            modelBuilder.Entity<Address>()
                .HasOne(e => e.User)
                .WithMany(c => c.Addresses);

            modelBuilder.Entity<Product>();

            modelBuilder.Entity<Order>().ToTable("Order")
              .HasMany(e => e.Items)
              .WithOne(c => c.Order);

            modelBuilder.Entity<CartItem>().ToTable("CartItem")
                .HasOne(e => e.Item)
                .WithMany(e => e.CartItems);
            modelBuilder.Entity<CartItem>().ToTable("CartItem")
                .HasOne(e => e.User)
                .WithMany(e => e.CartItems);

            modelBuilder.Entity<OrderItem>().ToTable("OrderItem")
                .HasOne(e => e.Order)
                .WithMany(e => e.Items);
        }
    }

}
