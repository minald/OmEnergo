using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class RenameProductintoSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autotransformers_Products_ProductId",
                table: "Autotransformers");

            migrationBuilder.DropForeignKey(
                name: "FK_Inverters_Products_ProductId",
                table: "Inverters");

            migrationBuilder.DropForeignKey(
                name: "FK_Stabilizers_Products_ProductId",
                table: "Stabilizers");

            migrationBuilder.DropForeignKey(
                name: "FK_Switches_Products_ProductId",
                table: "Switches");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Switches",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Switches_ProductId",
                table: "Switches",
                newName: "IX_Switches_SectionId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Stabilizers",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Stabilizers_ProductId",
                table: "Stabilizers",
                newName: "IX_Stabilizers_SectionId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Inverters",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Inverters_ProductId",
                table: "Inverters",
                newName: "IX_Inverters_SectionId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Autotransformers",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Autotransformers_ProductId",
                table: "Autotransformers",
                newName: "IX_Autotransformers_SectionId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Autotransformers_Sections_SectionId",
                table: "Autotransformers",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inverters_Sections_SectionId",
                table: "Inverters",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stabilizers_Sections_SectionId",
                table: "Stabilizers",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Switches_Sections_SectionId",
                table: "Switches",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autotransformers_Sections_SectionId",
                table: "Autotransformers");

            migrationBuilder.DropForeignKey(
                name: "FK_Inverters_Sections_SectionId",
                table: "Inverters");

            migrationBuilder.DropForeignKey(
                name: "FK_Stabilizers_Sections_SectionId",
                table: "Stabilizers");

            migrationBuilder.DropForeignKey(
                name: "FK_Switches_Sections_SectionId",
                table: "Switches");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Switches",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Switches_SectionId",
                table: "Switches",
                newName: "IX_Switches_ProductId");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Stabilizers",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Stabilizers_SectionId",
                table: "Stabilizers",
                newName: "IX_Stabilizers_ProductId");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Inverters",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Inverters_SectionId",
                table: "Inverters",
                newName: "IX_Inverters_ProductId");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Autotransformers",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Autotransformers_SectionId",
                table: "Autotransformers",
                newName: "IX_Autotransformers_ProductId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Autotransformers_Products_ProductId",
                table: "Autotransformers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inverters_Products_ProductId",
                table: "Inverters",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stabilizers_Products_ProductId",
                table: "Stabilizers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Switches_Products_ProductId",
                table: "Switches",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
