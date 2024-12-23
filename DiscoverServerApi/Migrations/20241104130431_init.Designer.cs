﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.EFCore;

#nullable disable

namespace MyWebServerProject.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241104130431_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Entities.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Tests");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Test1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Test2"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Test3"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
