using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Changingfloattodouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "StabilizerModels",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "StabilizerModels",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
