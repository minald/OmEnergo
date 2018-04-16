using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Relocatingweightanddimensionstomodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "Stabilizers");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Stabilizers");

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "StabilizerModels",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "StabilizerModels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "StabilizerModels");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "StabilizerModels");

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "Stabilizers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Stabilizers",
                nullable: true);
        }
    }
}
