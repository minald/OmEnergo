using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Addinginverteranditsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inverters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CoolingMethod = table.Column<string>(nullable: true),
                    Efficiency = table.Column<string>(nullable: true),
                    FrequencyOfInputVoltage = table.Column<string>(nullable: true),
                    Indication = table.Column<string>(nullable: true),
                    InputVoltageRange = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    PhasesAmount = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    SwitchingTime = table.Column<string>(nullable: true),
                    WorkingTemperature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inverters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inverters_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InverterModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dimensions = table.Column<string>(nullable: true),
                    InverterId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InverterModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InverterModels_Inverters_InverterId",
                        column: x => x.InverterId,
                        principalTable: "Inverters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InverterModels_InverterId",
                table: "InverterModels",
                column: "InverterId");

            migrationBuilder.CreateIndex(
                name: "IX_Inverters_ProductId",
                table: "Inverters",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InverterModels");

            migrationBuilder.DropTable(
                name: "Inverters");
        }
    }
}
