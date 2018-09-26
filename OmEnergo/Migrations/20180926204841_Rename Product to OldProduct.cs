using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class RenameProducttoOldProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SwitchModels");

            migrationBuilder.DropTable(
                name: "Switches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_Switches_SectionId",
                table: "Switches",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchModels_ProductId",
                table: "SwitchModels",
                column: "ProductId");
        }
    }
}
