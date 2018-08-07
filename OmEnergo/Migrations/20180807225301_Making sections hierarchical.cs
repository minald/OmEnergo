using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Makingsectionshierarchical : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentSectionId",
                table: "Sections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ParentSectionId",
                table: "Sections",
                column: "ParentSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Sections_ParentSectionId",
                table: "Sections",
                column: "ParentSectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Sections_ParentSectionId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_ParentSectionId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "ParentSectionId",
                table: "Sections");
        }
    }
}
