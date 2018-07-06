using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Addingautotransformeranditsmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autotransformers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InputVoltageRange = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    MainImageLink = table.Column<string>(nullable: true),
                    OutputVoltageRange = table.Column<string>(nullable: true),
                    PhasesAmount = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autotransformers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Autotransformers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AutotransformerModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AutotransformerId = table.Column<int>(nullable: true),
                    Dimensions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutotransformerModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutotransformerModels_Autotransformers_AutotransformerId",
                        column: x => x.AutotransformerId,
                        principalTable: "Autotransformers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutotransformerModels_AutotransformerId",
                table: "AutotransformerModels",
                column: "AutotransformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Autotransformers_ProductId",
                table: "Autotransformers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutotransformerModels");

            migrationBuilder.DropTable(
                name: "Autotransformers");
        }
    }
}
