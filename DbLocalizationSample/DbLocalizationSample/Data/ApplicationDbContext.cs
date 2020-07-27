using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XLocalizer.DB.Models;

namespace DBLocalizationSample.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<XDbCulture> XDbCultures { get; set; }
        public DbSet<XDbResource> XDbResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<XDbResource>()
                .HasKey(r => r.ID);

            builder.Entity<XDbResource>()
                .HasIndex(r => new { r.CultureID, r.Key })
                .IsUnique();
            builder.Entity<XDbResource>()
                .HasOne(t => t.Culture)
                .WithMany(t => t.Translations as IEnumerable<XDbResource>)
                .OnDelete(DeleteBehavior.Cascade);

            builder.SeedCultures();
            builder.SeedResourceData();

            base.OnModelCreating(builder);
        }
    }
}
