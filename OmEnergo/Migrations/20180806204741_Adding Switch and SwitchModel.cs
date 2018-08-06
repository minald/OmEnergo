using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class AddingSwitchandSwitchModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ProductId = table.Column<int>(nullable: true),
                    ProtectionDegree = table.Column<string>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    WorkingTemperatureRange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Switches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Switches_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
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
                    SwitchId = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwitchModels_Switches_SwitchId",
                        column: x => x.SwitchId,
                        principalTable: "Switches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Switches_ProductId",
                table: "Switches",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchModels_SwitchId",
                table: "SwitchModels",
                column: "SwitchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SwitchModels");

            migrationBuilder.DropTable(
                name: "Switches");
        }
    }
}
