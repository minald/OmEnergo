using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Changinginverterinputtooutput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InputVoltageRange",
                table: "Inverters",
                newName: "OutputVoltageRange");

            migrationBuilder.RenameColumn(
                name: "FrequencyOfInputVoltage",
                table: "Inverters",
                newName: "FrequencyOfOutputVoltage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OutputVoltageRange",
                table: "Inverters",
                newName: "InputVoltageRange");

            migrationBuilder.RenameColumn(
                name: "FrequencyOfOutputVoltage",
                table: "Inverters",
                newName: "FrequencyOfInputVoltage");
        }
    }
}
