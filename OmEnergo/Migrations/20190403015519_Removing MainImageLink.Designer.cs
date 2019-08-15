﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using OmEnergo.Infrastructure.Database;
using System;

namespace OmEnergo.Migrations
{
	[DbContext(typeof(OmEnergoContext))]
	[Migration("20190403015519_Removing MainImageLink")]
	partial class RemovingMainImageLink
	{
		protected override void BuildTargetModel(ModelBuilder modelBuilder)
		{
#pragma warning disable 612, 618
			modelBuilder
				.HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

			modelBuilder.Entity("OmEnergo.Models.ConfigKey", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd();

					b.Property<string>("Key");

					b.Property<string>("Value");

					b.HasKey("Id");

					b.ToTable("ConfigKeys");
				});

			modelBuilder.Entity("OmEnergo.Models.Product", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd();

					b.Property<string>("Description");

					b.Property<string>("EnglishName");

					b.Property<string>("Name");

					b.Property<string>("Properties");

					b.Property<int?>("SectionId");

					b.Property<int>("SequenceNumber");

					b.HasKey("Id");

					b.HasIndex("SectionId");

					b.ToTable("Products");
				});

			modelBuilder.Entity("OmEnergo.Models.ProductModel", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd();

					b.Property<string>("EnglishName");

					b.Property<string>("Name");

					b.Property<double>("Price");

					b.Property<int?>("ProductId");

					b.Property<string>("Properties");

					b.Property<int?>("SectionId");

					b.Property<int>("SequenceNumber");

					b.HasKey("Id");

					b.HasIndex("ProductId");

					b.HasIndex("SectionId");

					b.ToTable("ProductModels");
				});

			modelBuilder.Entity("OmEnergo.Models.Section", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd();

					b.Property<string>("Description");

					b.Property<string>("EnglishName");

					b.Property<string>("Name");

					b.Property<int?>("ParentSectionId");

					b.Property<string>("ProductModelProperties");

					b.Property<string>("ProductProperties");

					b.Property<int>("SequenceNumber");

					b.HasKey("Id");

					b.HasIndex("ParentSectionId");

					b.ToTable("Sections");
				});

			modelBuilder.Entity("OmEnergo.Models.Product", b =>
				{
					b.HasOne("OmEnergo.Models.Section", "Section")
						.WithMany("Products")
						.HasForeignKey("SectionId");
				});

			modelBuilder.Entity("OmEnergo.Models.ProductModel", b =>
				{
					b.HasOne("OmEnergo.Models.Product", "Product")
						.WithMany("Models")
						.HasForeignKey("ProductId");

					b.HasOne("OmEnergo.Models.Section", "Section")
						.WithMany("ProductModels")
						.HasForeignKey("SectionId");
				});

			modelBuilder.Entity("OmEnergo.Models.Section", b =>
				{
					b.HasOne("OmEnergo.Models.Section", "ParentSection")
						.WithMany("ChildSections")
						.HasForeignKey("ParentSectionId");
				});
#pragma warning restore 612, 618
		}
	}
}
