using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class AddSectionIdtoCommonProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "CommonProductModels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommonProductModels_SectionId",
                table: "CommonProductModels",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonProductModels_Sections_SectionId",
                table: "CommonProductModels",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonProductModels_Sections_SectionId",
                table: "CommonProductModels");

            migrationBuilder.DropIndex(
                name: "IX_CommonProductModels_SectionId",
                table: "CommonProductModels");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "CommonProductModels");
        }
    }
}
