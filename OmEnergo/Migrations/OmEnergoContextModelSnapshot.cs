﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using OmEnergo.Models;
using System;

namespace OmEnergo.Migrations
{
    [DbContext(typeof(OmEnergoContext))]
    partial class OmEnergoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OmEnergo.Models.Autotransformer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("InputVoltageRange");

                    b.Property<string>("LongDescription");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("NetworkFrequency");

                    b.Property<string>("OutputVoltageRange");

                    b.Property<int>("PhasesAmount");

                    b.Property<int?>("ProductId");

                    b.Property<string>("Series");

                    b.Property<string>("ShortDescription");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Autotransformers");
                });

            modelBuilder.Entity("OmEnergo.Models.AutotransformerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AutotransformerId");

                    b.Property<string>("Dimensions");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("AutotransformerId");

                    b.ToTable("AutotransformerModels");
                });

            modelBuilder.Entity("OmEnergo.Models.Inverter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoolingMethod");

                    b.Property<string>("Efficiency");

                    b.Property<string>("FrequencyOfOutputVoltage");

                    b.Property<string>("Indication");

                    b.Property<string>("LongDescription");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("OutputVoltageRange");

                    b.Property<int>("PhasesAmount");

                    b.Property<int?>("ProductId");

                    b.Property<string>("Series");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("SwitchingTime");

                    b.Property<string>("WorkingTemperature");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Inverters");
                });

            modelBuilder.Entity("OmEnergo.Models.InverterModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Dimensions");

                    b.Property<int?>("InverterId");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("InverterId");

                    b.ToTable("InverterModels");
                });

            modelBuilder.Entity("OmEnergo.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("EnglishName");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("RussianName");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("OmEnergo.Models.Stabilizer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("AdjustableDelay");

                    b.Property<string>("AllowableDurableOverload");

                    b.Property<string>("BypassMode");

                    b.Property<string>("Efficiency");

                    b.Property<string>("Indication");

                    b.Property<string>("InputVoltageRange");

                    b.Property<string>("LongDescription");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("OperatingFrequencyOfNetwork");

                    b.Property<int?>("PhasesAmount");

                    b.Property<int?>("ProductId");

                    b.Property<string>("Series");

                    b.Property<bool?>("ShortCircuitProtection");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("StabilizationAccuracy");

                    b.Property<string>("StabilizationType");

                    b.Property<string>("SwitchingTime");

                    b.Property<bool?>("VoltageSurgesProtection");

                    b.Property<string>("WorkingTemperature");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Stabilizers");
                });

            modelBuilder.Entity("OmEnergo.Models.StabilizerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Dimensions");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int?>("StabilizerId");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("StabilizerId");

                    b.ToTable("StabilizerModels");
                });

            modelBuilder.Entity("OmEnergo.Models.Autotransformer", b =>
                {
                    b.HasOne("OmEnergo.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("OmEnergo.Models.AutotransformerModel", b =>
                {
                    b.HasOne("OmEnergo.Models.Autotransformer", "Autotransformer")
                        .WithMany("Models")
                        .HasForeignKey("AutotransformerId");
                });

            modelBuilder.Entity("OmEnergo.Models.Inverter", b =>
                {
                    b.HasOne("OmEnergo.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("OmEnergo.Models.InverterModel", b =>
                {
                    b.HasOne("OmEnergo.Models.Inverter", "Inverter")
                        .WithMany("Models")
                        .HasForeignKey("InverterId");
                });

            modelBuilder.Entity("OmEnergo.Models.Stabilizer", b =>
                {
                    b.HasOne("OmEnergo.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("OmEnergo.Models.StabilizerModel", b =>
                {
                    b.HasOne("OmEnergo.Models.Stabilizer", "Stabilizer")
                        .WithMany("Models")
                        .HasForeignKey("StabilizerId");
                });
#pragma warning restore 612, 618
        }
    }
}
