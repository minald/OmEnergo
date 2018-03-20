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
                name: "Products",
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
                    table.PrimaryKey("PK_Products", x => x.Id);
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
                    Dimensions = table.Column<string>(nullable: true),
                    Efficiency = table.Column<string>(nullable: true),
                    Indication = table.Column<string>(nullable: true),
                    InputVoltageRange = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    OperatingFrequencyOfNetwork = table.Column<string>(nullable: true),
                    PhasesAmount = table.Column<int>(nullable: true),
                    Price = table.Column<double>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    ShortCircuitProtection = table.Column<bool>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    StabilizationAccuracy = table.Column<string>(nullable: true),
                    StabilizationType = table.Column<string>(nullable: true),
                    SwitchingTime = table.Column<string>(nullable: true),
                    VoltageSurgesProtection = table.Column<bool>(nullable: true),
                    Weight = table.Column<int>(nullable: true),
                    WorkingTemperature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stabilizers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stabilizers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Booklets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Link = table.Column<string>(nullable: true),
                    StabilizerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booklets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booklets_Stabilizers_StabilizerId",
                        column: x => x.StabilizerId,
                        principalTable: "Stabilizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Link = table.Column<string>(nullable: true),
                    StabilizerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Stabilizers_StabilizerId",
                        column: x => x.StabilizerId,
                        principalTable: "Stabilizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booklets_StabilizerId",
                table: "Booklets",
                column: "StabilizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_StabilizerId",
                table: "Pictures",
                column: "StabilizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stabilizers_ProductId",
                table: "Stabilizers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booklets");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Stabilizers");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
