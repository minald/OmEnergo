using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class CreatingCommonProductModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonProductModel_CommonProducts_CommonProductId",
                table: "CommonProductModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommonProductModel",
                table: "CommonProductModel");

            migrationBuilder.RenameTable(
                name: "CommonProductModel",
                newName: "CommonProductModels");

            migrationBuilder.RenameIndex(
                name: "IX_CommonProductModel_CommonProductId",
                table: "CommonProductModels",
                newName: "IX_CommonProductModels_CommonProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommonProductModels",
                table: "CommonProductModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonProductModels_CommonProducts_CommonProductId",
                table: "CommonProductModels",
                column: "CommonProductId",
                principalTable: "CommonProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonProductModels_CommonProducts_CommonProductId",
                table: "CommonProductModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommonProductModels",
                table: "CommonProductModels");

            migrationBuilder.RenameTable(
                name: "CommonProductModels",
                newName: "CommonProductModel");

            migrationBuilder.RenameIndex(
                name: "IX_CommonProductModels_CommonProductId",
                table: "CommonProductModel",
                newName: "IX_CommonProductModel_CommonProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommonProductModel",
                table: "CommonProductModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonProductModel_CommonProducts_CommonProductId",
                table: "CommonProductModel",
                column: "CommonProductId",
                principalTable: "CommonProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
