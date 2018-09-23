using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class AddingpropertiesrelatedtoSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductModelProperties",
                table: "Sections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductProperties",
                table: "Sections",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductModelProperties",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "ProductProperties",
                table: "Sections");
        }
    }
}
