using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Addingnewfieldsintosomeclasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OutputVoltageRange",
                table: "Inverters",
                newName: "InputVoltageRange");

            migrationBuilder.RenameColumn(
                name: "InputVoltageRange",
                table: "Autotransformers",
                newName: "InputVoltage");

            migrationBuilder.AddColumn<string>(
                name: "ImplementedProtections",
                table: "Stabilizers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationType",
                table: "Stabilizers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoadConnection",
                table: "StabilizerModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NetworkConnection",
                table: "StabilizerModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NominalPower",
                table: "StabilizerModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatteryVoltage",
                table: "InverterModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NominalPower",
                table: "InverterModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeakPower",
                table: "InverterModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaximalWorkingAmperage",
                table: "AutotransformerModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NominalPower",
                table: "AutotransformerModels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImplementedProtections",
                table: "Stabilizers");

            migrationBuilder.DropColumn(
                name: "InstallationType",
                table: "Stabilizers");

            migrationBuilder.DropColumn(
                name: "LoadConnection",
                table: "StabilizerModels");

            migrationBuilder.DropColumn(
                name: "NetworkConnection",
                table: "StabilizerModels");

            migrationBuilder.DropColumn(
                name: "NominalPower",
                table: "StabilizerModels");

            migrationBuilder.DropColumn(
                name: "BatteryVoltage",
                table: "InverterModels");

            migrationBuilder.DropColumn(
                name: "NominalPower",
                table: "InverterModels");

            migrationBuilder.DropColumn(
                name: "PeakPower",
                table: "InverterModels");

            migrationBuilder.DropColumn(
                name: "MaximalWorkingAmperage",
                table: "AutotransformerModels");

            migrationBuilder.DropColumn(
                name: "NominalPower",
                table: "AutotransformerModels");

            migrationBuilder.RenameColumn(
                name: "InputVoltageRange",
                table: "Inverters",
                newName: "OutputVoltageRange");

            migrationBuilder.RenameColumn(
                name: "InputVoltage",
                table: "Autotransformers",
                newName: "InputVoltageRange");
        }
    }
}
