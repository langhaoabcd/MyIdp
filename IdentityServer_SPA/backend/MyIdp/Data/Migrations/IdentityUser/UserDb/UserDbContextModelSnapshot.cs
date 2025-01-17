﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyIdp.Data;

namespace MyIdp.Data.Migrations.IdentityUser.UserDb
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyIdp.Models.SysUser", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("last_login_time")
                        .HasColumnType("datetime2");

                    b.Property<string>("nickname")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("password")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("phone")
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime?>("register_time")
                        .HasColumnType("datetime2");

                    b.Property<string>("username")
                        .HasColumnType("varchar(20)");

                    b.HasKey("id");

                    b.ToTable("SysUser");
                });
#pragma warning restore 612, 618
        }
    }
}
