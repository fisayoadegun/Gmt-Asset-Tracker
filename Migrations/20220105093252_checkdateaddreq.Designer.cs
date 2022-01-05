﻿// <auto-generated />
using System;
using Gmt_Asset_Tracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gmt_Asset_Tracker.Migrations
{
    [DbContext(typeof(AssetTrackerContext))]
    [Migration("20220105093252_checkdateaddreq")]
    partial class checkdateaddreq
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Asset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssetStateId")
                        .HasColumnType("int");

                    b.Property<string>("Asset_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Asset_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Asset_tag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Assigned_to")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("CheckId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Check_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Delivery_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int?>("PresentLocationId")
                        .HasColumnType("int");

                    b.Property<string>("Present_user")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Purchased_price")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Requistion_pack")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Service_tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VendorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssetStateId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CheckId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("LocationId");

                    b.HasIndex("PresentLocationId");

                    b.HasIndex("VendorId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Asset_State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Asset_state")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Asset_States");
                });

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category_name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Department_name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Location_name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Physical_check", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Check_name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Physical_Checks");
                });

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Vendor_name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Gmt_Asset_Tracker.Models.Asset", b =>
                {
                    b.HasOne("Gmt_Asset_Tracker.Models.Asset_State", "Asset_State")
                        .WithMany()
                        .HasForeignKey("AssetStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gmt_Asset_Tracker.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gmt_Asset_Tracker.Models.Physical_check", "Physical_check")
                        .WithMany()
                        .HasForeignKey("CheckId");

                    b.HasOne("Gmt_Asset_Tracker.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gmt_Asset_Tracker.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gmt_Asset_Tracker.Models.Location", "Present_location")
                        .WithMany()
                        .HasForeignKey("PresentLocationId");

                    b.HasOne("Gmt_Asset_Tracker.Models.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset_State");

                    b.Navigation("Category");

                    b.Navigation("Department");

                    b.Navigation("Location");

                    b.Navigation("Physical_check");

                    b.Navigation("Present_location");

                    b.Navigation("Vendor");
                });
#pragma warning restore 612, 618
        }
    }
}
