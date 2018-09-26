using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class RenamepropertyfromCommonProducttoProductproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_Products_CommonProductId",
                table: "ProductModels");

            migrationBuilder.RenameColumn(
                name: "CommonProductId",
                table: "ProductModels",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductModels_CommonProductId",
                table: "ProductModels",
                newName: "IX_ProductModels_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_Products_ProductId",
                table: "ProductModels",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_Products_ProductId",
                table: "ProductModels");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductModels",
                newName: "CommonProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductModels_ProductId",
                table: "ProductModels",
                newName: "IX_ProductModels_CommonProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_Products_CommonProductId",
                table: "ProductModels",
                column: "CommonProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
