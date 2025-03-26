﻿// <auto-generated />
using DBLocalizationSample.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbLocalizationSample.Migrations
{
    [DbContext(typeof(XLocalizerDbContext))]
    [Migration("20250326075841_removeidentitystorefromxlodb")]
    partial class removeidentitystorefromxlodb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("XLocalizer.DB.Models.XDbCulture", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EnglishName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.ToTable("XDbCultures");

                    b.HasData(
                        new
                        {
                            ID = "en",
                            EnglishName = "English",
                            IsActive = true,
                            IsDefault = true
                        },
                        new
                        {
                            ID = "tr",
                            EnglishName = "Turkish",
                            IsActive = true,
                            IsDefault = false
                        },
                        new
                        {
                            ID = "ar",
                            EnglishName = "Arabic",
                            IsActive = true,
                            IsDefault = false
                        });
                });

            modelBuilder.Entity("XLocalizer.DB.Models.XDbResource", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CultureID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CultureID", "Key")
                        .IsUnique()
                        .HasFilter("[CultureID] IS NOT NULL AND [Key] IS NOT NULL");

                    b.ToTable("XDbResources");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Comment = "Created by XLocalizer",
                            CultureID = "tr",
                            IsActive = true,
                            Key = "Welcome",
                            Value = "Hoşgeldin"
                        });
                });

            modelBuilder.Entity("XLocalizer.DB.Models.XDbResource", b =>
                {
                    b.HasOne("XLocalizer.DB.Models.XDbCulture", "Culture")
                        .WithMany("Translations")
                        .HasForeignKey("CultureID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Culture");
                });

            modelBuilder.Entity("XLocalizer.DB.Models.XDbCulture", b =>
                {
                    b.Navigation("Translations");
                });
#pragma warning restore 612, 618
        }
    }
}
