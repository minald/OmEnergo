using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    EnglishName = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    RussianName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Autotransformers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InputVoltage = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    NetworkFrequency = table.Column<string>(nullable: true),
                    OutputVoltageRange = table.Column<string>(nullable: true),
                    PhasesAmount = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autotransformers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Autotransformers_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inverters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CoolingMethod = table.Column<string>(nullable: true),
                    Efficiency = table.Column<string>(nullable: true),
                    FrequencyOfOutputVoltage = table.Column<string>(nullable: true),
                    Indication = table.Column<string>(nullable: true),
                    InputVoltageRange = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    PhasesAmount = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    SwitchingTime = table.Column<string>(nullable: true),
                    WorkingTemperature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inverters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inverters_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stabilizers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdjustableDelay = table.Column<bool>(nullable: true),
                    AllowableDurableOverload = table.Column<string>(nullable: true),
                    BypassMode = table.Column<string>(nullable: true),
                    Efficiency = table.Column<string>(nullable: true),
                    ImplementedProtections = table.Column<string>(nullable: true),
                    Indication = table.Column<string>(nullable: true),
                    InputVoltageRange = table.Column<string>(nullable: true),
                    InstallationType = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    OperatingFrequencyOfNetwork = table.Column<string>(nullable: true),
                    PhasesAmount = table.Column<int>(nullable: true),
                    SectionId = table.Column<int>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    ShortCircuitProtection = table.Column<bool>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    StabilizationAccuracy = table.Column<string>(nullable: true),
                    StabilizationType = table.Column<string>(nullable: true),
                    SwitchingTime = table.Column<string>(nullable: true),
                    VoltageSurgesProtection = table.Column<bool>(nullable: true),
                    WorkingTemperature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stabilizers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stabilizers_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Switches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    MaximalAmperage = table.Column<string>(nullable: true),
                    NominalVoltage = table.Column<string>(nullable: true),
                    ProtectionDegree = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    WorkingTemperatureRange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Switches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Switches_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AutotransformerModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dimensions = table.Column<string>(nullable: true),
                    MaximalWorkingAmperage = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NominalPower = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutotransformerModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutotransformerModels_Autotransformers_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Autotransformers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InverterModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BatteryVoltage = table.Column<string>(nullable: true),
                    Dimensions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NominalPower = table.Column<string>(nullable: true),
                    PeakPower = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InverterModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InverterModels_Inverters_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Inverters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StabilizerModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dimensions = table.Column<string>(nullable: true),
                    LoadConnection = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NetworkConnection = table.Column<string>(nullable: true),
                    NominalPower = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StabilizerModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StabilizerModels_Stabilizers_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Stabilizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SwitchModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvailableWireLength = table.Column<double>(nullable: false),
                    Dimensions = table.Column<string>(nullable: true),
                    MaximalAmperage = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwitchModels_Switches_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Switches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutotransformerModels_ProductId",
                table: "AutotransformerModels",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Autotransformers_SectionId",
                table: "Autotransformers",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InverterModels_ProductId",
                table: "InverterModels",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Inverters_SectionId",
                table: "Inverters",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StabilizerModels_ProductId",
                table: "StabilizerModels",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stabilizers_SectionId",
                table: "Stabilizers",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Switches_SectionId",
                table: "Switches",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchModels_ProductId",
                table: "SwitchModels",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutotransformerModels");

            migrationBuilder.DropTable(
                name: "InverterModels");

            migrationBuilder.DropTable(
                name: "StabilizerModels");

            migrationBuilder.DropTable(
                name: "SwitchModels");

            migrationBuilder.DropTable(
                name: "Autotransformers");

            migrationBuilder.DropTable(
                name: "Inverters");

            migrationBuilder.DropTable(
                name: "Stabilizers");

            migrationBuilder.DropTable(
                name: "Switches");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
