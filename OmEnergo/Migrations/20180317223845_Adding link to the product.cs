using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Addinglinktotheproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Stabilizers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stabilizers_ProductId",
                table: "Stabilizers",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stabilizers_Products_ProductId",
                table: "Stabilizers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stabilizers_Products_ProductId",
                table: "Stabilizers");

            migrationBuilder.DropIndex(
                name: "IX_Stabilizers_ProductId",
                table: "Stabilizers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Stabilizers");
        }
    }
}
