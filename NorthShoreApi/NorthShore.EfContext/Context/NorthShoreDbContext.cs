using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NorthShore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.EfContext.Context
{
    public class NorthShoreDbContext : IdentityDbContext<IdentityUser>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<FoodMenuMapping> FoodMenuMappings { get; set; }

        public NorthShoreDbContext(DbContextOptions<NorthShoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FoodMenuMapping>(fmp => {

                fmp.HasOne(m => m.Food)
                    .WithMany(f => f.MenuMappings)
                    .HasPrincipalKey(f => f.Id)
                    .HasForeignKey(m => m.FoodId)
                    .HasConstraintName("FK_FoodMenuMapping_Food");

                fmp.HasOne(m => m.Menu)
                    .WithMany(m => m.FoodMappings)
                    .HasPrincipalKey(m => m.Id)
                    .HasForeignKey(m => m.MenuId)
                    .HasConstraintName("FK_FoodMenuMapping_Mood");
            });
        }
    }
}
