using Microsoft.EntityFrameworkCore;
using MyEshop.Controllers;
using MyEshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Data
{
    public class MyEshopContext : DbContext
    {
        public MyEshopContext(DbContextOptions<MyEshopContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryToProduct> CategoryToProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail>  OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryToProduct>().HasKey
                (t => new { t.CategoryId, t.ProductId });

            // Influent API --- 
            //modelBuilder.Entity<Product>(
            //    p =>
            //    {
            //        p.HasKey(w => w.Id);
            //        p.OwnsOne<Item>(navigationExpression: w => w.Item);
            //        p.HasOne<Item>(navigationExpression: w => w.Item).WithOne(w => w.Product)
            //            .HasForeignKey<Item>(w => w.Id);
            //    }
            //    );

            modelBuilder.Entity<Item>(
                i =>
                {
                    // change data type of one column in table
                    i.Property(w => w.Price).HasColumnType("Money");
                    i.HasKey(w => w.Id);
                }
                );

            #region Seed Data Category
            modelBuilder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1,
                Name = "برنامه نویسی موبایل",
                Description = "برنامه نویسی موبایل"
            },
             new Category()
             {
                 Id = 2,
                 Name = "برنامه نویسی وب",
                 Description = "برنامه نویسی وب"
             },
              new Category()
              {
                  Id = 3,
                  Name = "طراحی سایت",
                  Description = "طراحی سایت"
              },
              new Category()
              {
                  Id = 4,
                  Name = "سئو",
                  Description = "سئو سایت"
              }
                );

            modelBuilder.Entity<Item>().HasData(
                new Item()
                {
                    Id = 1,
                    Price = 1500.2M,
                    QuantityInStock = 6
                },
             new Item()
             {
                 Id = 2,
                 Price = 314.0M,
                 QuantityInStock = 4
             },
             new Item()
             {
                 Id = 3,
                 Price = 412.5M,
                 QuantityInStock = 7
             },
            new Item()
            {
                Id = 4,
                Price = 321.1M,
                QuantityInStock = 2
            });

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    ItemId = 1,
                    Name = "آموزش پروژه محور asp.net core 3.0",
                    Description = "آموزش پروژه محور asp.net core 3.0"
                },
                 new Product()
                 {
                     Id = 2,
                     ItemId = 2,
                     Name = "آموزش مقدماتی تا پیشرفته Blazor",
                     Description = "آموزش مقدماتی تا پیشرفته Blazor"
                 },
                  new Product()
                  {
                      Id = 3,
                      ItemId = 3,
                      Name = "آموزش اپلیکشن از پیش رونده",
                      Description = "آموزش اپلیکشن از پیش رونده"
                  }
                );

            modelBuilder.Entity<CategoryToProduct>().HasData(
                new CategoryToProduct() { CategoryId = 1 , ProductId=1 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 4, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 1, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 4, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 1, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 4, ProductId = 3 }
                );
            #endregion
            base.OnModelCreating(modelBuilder);

        }
    }


}
