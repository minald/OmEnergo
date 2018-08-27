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

                    b.Property<string>("InputVoltage");

                    b.Property<string>("LongDescription");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("NetworkFrequency");

                    b.Property<string>("OutputVoltageRange");

                    b.Property<int>("PhasesAmount");

                    b.Property<int?>("SectionId");

                    b.Property<string>("Series");

                    b.Property<string>("ShortDescription");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Autotransformers");
                });

            modelBuilder.Entity("OmEnergo.Models.AutotransformerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Dimensions");

                    b.Property<string>("MaximalWorkingAmperage");

                    b.Property<string>("Name");

                    b.Property<string>("NominalPower");

                    b.Property<double>("Price");

                    b.Property<int?>("ProductId");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("AutotransformerModels");
                });

            modelBuilder.Entity("OmEnergo.Models.CommonProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("Name");

                    b.Property<string>("Properties");

                    b.Property<int?>("SectionId");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("CommonProducts");
                });

            modelBuilder.Entity("OmEnergo.Models.CommonProductModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CommonProductId");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<string>("Properties");

                    b.HasKey("Id");

                    b.HasIndex("CommonProductId");

                    b.ToTable("CommonProductModels");
                });

            modelBuilder.Entity("OmEnergo.Models.Inverter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoolingMethod");

                    b.Property<string>("Efficiency");

                    b.Property<string>("FrequencyOfOutputVoltage");

                    b.Property<string>("Indication");

                    b.Property<string>("InputVoltageRange");

                    b.Property<string>("LongDescription");

                    b.Property<string>("MainImageLink");

                    b.Property<int>("PhasesAmount");

                    b.Property<int?>("SectionId");

                    b.Property<string>("Series");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("SwitchingTime");

                    b.Property<string>("WorkingTemperature");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Inverters");
                });

            modelBuilder.Entity("OmEnergo.Models.InverterModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BatteryVoltage");

                    b.Property<string>("Dimensions");

                    b.Property<string>("Name");

                    b.Property<string>("NominalPower");

                    b.Property<string>("PeakPower");

                    b.Property<double>("Price");

                    b.Property<int?>("ProductId");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("InverterModels");
                });

            modelBuilder.Entity("OmEnergo.Models.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("EnglishName");

                    b.Property<string>("MainImageLink");

                    b.Property<int?>("ParentSectionId");

                    b.Property<string>("RussianName");

                    b.HasKey("Id");

                    b.HasIndex("ParentSectionId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("OmEnergo.Models.Stabilizer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("AdjustableDelay");

                    b.Property<string>("AllowableDurableOverload");

                    b.Property<string>("BypassMode");

                    b.Property<string>("Efficiency");

                    b.Property<string>("ImplementedProtections");

                    b.Property<string>("Indication");

                    b.Property<string>("InputVoltageRange");

                    b.Property<string>("InstallationType");

                    b.Property<string>("LongDescription");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("OperatingFrequencyOfNetwork");

                    b.Property<int?>("PhasesAmount");

                    b.Property<int?>("SectionId");

                    b.Property<string>("Series");

                    b.Property<bool?>("ShortCircuitProtection");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("StabilizationAccuracy");

                    b.Property<string>("StabilizationType");

                    b.Property<string>("SwitchingTime");

                    b.Property<bool?>("VoltageSurgesProtection");

                    b.Property<string>("WorkingTemperature");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Stabilizers");
                });

            modelBuilder.Entity("OmEnergo.Models.StabilizerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Dimensions");

                    b.Property<string>("LoadConnection");

                    b.Property<string>("Name");

                    b.Property<string>("NetworkConnection");

                    b.Property<string>("NominalPower");

                    b.Property<double>("Price");

                    b.Property<int?>("ProductId");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("StabilizerModels");
                });

            modelBuilder.Entity("OmEnergo.Models.Switch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("MainImageLink");

                    b.Property<string>("MaximalAmperage");

                    b.Property<string>("NominalVoltage");

                    b.Property<string>("ProtectionDegree");

                    b.Property<int?>("SectionId");

                    b.Property<string>("Series");

                    b.Property<string>("WorkingTemperatureRange");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Switches");
                });

            modelBuilder.Entity("OmEnergo.Models.SwitchModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AvailableWireLength");

                    b.Property<string>("Dimensions");

                    b.Property<string>("MaximalAmperage");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int?>("ProductId");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("SwitchModels");
                });

            modelBuilder.Entity("OmEnergo.Models.Autotransformer", b =>
                {
                    b.HasOne("OmEnergo.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");
                });

            modelBuilder.Entity("OmEnergo.Models.AutotransformerModel", b =>
                {
                    b.HasOne("OmEnergo.Models.Autotransformer", "Product")
                        .WithMany("Models")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("OmEnergo.Models.CommonProduct", b =>
                {
                    b.HasOne("OmEnergo.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");
                });

            modelBuilder.Entity("OmEnergo.Models.CommonProductModel", b =>
                {
                    b.HasOne("OmEnergo.Models.CommonProduct", "CommonProduct")
                        .WithMany("Models")
                        .HasForeignKey("CommonProductId");
                });

            modelBuilder.Entity("OmEnergo.Models.Inverter", b =>
                {
                    b.HasOne("OmEnergo.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");
                });

            modelBuilder.Entity("OmEnergo.Models.InverterModel", b =>
                {
                    b.HasOne("OmEnergo.Models.Inverter", "Product")
                        .WithMany("Models")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("OmEnergo.Models.Section", b =>
                {
                    b.HasOne("OmEnergo.Models.Section", "ParentSection")
                        .WithMany("ChildSections")
                        .HasForeignKey("ParentSectionId");
                });

            modelBuilder.Entity("OmEnergo.Models.Stabilizer", b =>
                {
                    b.HasOne("OmEnergo.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");
                });

            modelBuilder.Entity("OmEnergo.Models.StabilizerModel", b =>
                {
                    b.HasOne("OmEnergo.Models.Stabilizer", "Product")
                        .WithMany("Models")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("OmEnergo.Models.Switch", b =>
                {
                    b.HasOne("OmEnergo.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");
                });

            modelBuilder.Entity("OmEnergo.Models.SwitchModel", b =>
                {
                    b.HasOne("OmEnergo.Models.Switch", "Product")
                        .WithMany("Models")
                        .HasForeignKey("ProductId");
                });
#pragma warning restore 612, 618
        }
    }
}
