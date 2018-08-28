using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class CombiningnamesinSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishName",
                table: "Sections");

            migrationBuilder.RenameColumn(
                name: "RussianName",
                table: "Sections",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sections",
                newName: "RussianName");

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                table: "Sections",
                nullable: true);
        }
    }
}
