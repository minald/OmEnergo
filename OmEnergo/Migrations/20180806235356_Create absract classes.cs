using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Createabsractclasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutotransformerModels_Autotransformers_AutotransformerId",
                table: "AutotransformerModels");

            migrationBuilder.DropForeignKey(
                name: "FK_InverterModels_Inverters_InverterId",
                table: "InverterModels");

            migrationBuilder.DropForeignKey(
                name: "FK_StabilizerModels_Stabilizers_StabilizerId",
                table: "StabilizerModels");

            migrationBuilder.DropForeignKey(
                name: "FK_SwitchModels_Switches_SwitchId",
                table: "SwitchModels");

            migrationBuilder.RenameColumn(
                name: "SwitchId",
                table: "SwitchModels",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SwitchModels_SwitchId",
                table: "SwitchModels",
                newName: "IX_SwitchModels_ProductId");

            migrationBuilder.RenameColumn(
                name: "StabilizerId",
                table: "StabilizerModels",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_StabilizerModels_StabilizerId",
                table: "StabilizerModels",
                newName: "IX_StabilizerModels_ProductId");

            migrationBuilder.RenameColumn(
                name: "InverterId",
                table: "InverterModels",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InverterModels_InverterId",
                table: "InverterModels",
                newName: "IX_InverterModels_ProductId");

            migrationBuilder.RenameColumn(
                name: "AutotransformerId",
                table: "AutotransformerModels",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AutotransformerModels_AutotransformerId",
                table: "AutotransformerModels",
                newName: "IX_AutotransformerModels_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutotransformerModels_Autotransformers_ProductId",
                table: "AutotransformerModels",
                column: "ProductId",
                principalTable: "Autotransformers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InverterModels_Inverters_ProductId",
                table: "InverterModels",
                column: "ProductId",
                principalTable: "Inverters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StabilizerModels_Stabilizers_ProductId",
                table: "StabilizerModels",
                column: "ProductId",
                principalTable: "Stabilizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SwitchModels_Switches_ProductId",
                table: "SwitchModels",
                column: "ProductId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutotransformerModels_Autotransformers_ProductId",
                table: "AutotransformerModels");

            migrationBuilder.DropForeignKey(
                name: "FK_InverterModels_Inverters_ProductId",
                table: "InverterModels");

            migrationBuilder.DropForeignKey(
                name: "FK_StabilizerModels_Stabilizers_ProductId",
                table: "StabilizerModels");

            migrationBuilder.DropForeignKey(
                name: "FK_SwitchModels_Switches_ProductId",
                table: "SwitchModels");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "SwitchModels",
                newName: "SwitchId");

            migrationBuilder.RenameIndex(
                name: "IX_SwitchModels_ProductId",
                table: "SwitchModels",
                newName: "IX_SwitchModels_SwitchId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "StabilizerModels",
                newName: "StabilizerId");

            migrationBuilder.RenameIndex(
                name: "IX_StabilizerModels_ProductId",
                table: "StabilizerModels",
                newName: "IX_StabilizerModels_StabilizerId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "InverterModels",
                newName: "InverterId");

            migrationBuilder.RenameIndex(
                name: "IX_InverterModels_ProductId",
                table: "InverterModels",
                newName: "IX_InverterModels_InverterId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AutotransformerModels",
                newName: "AutotransformerId");

            migrationBuilder.RenameIndex(
                name: "IX_AutotransformerModels_ProductId",
                table: "AutotransformerModels",
                newName: "IX_AutotransformerModels_AutotransformerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutotransformerModels_Autotransformers_AutotransformerId",
                table: "AutotransformerModels",
                column: "AutotransformerId",
                principalTable: "Autotransformers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InverterModels_Inverters_InverterId",
                table: "InverterModels",
                column: "InverterId",
                principalTable: "Inverters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StabilizerModels_Stabilizers_StabilizerId",
                table: "StabilizerModels",
                column: "StabilizerId",
                principalTable: "Stabilizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SwitchModels_Switches_SwitchId",
                table: "SwitchModels",
                column: "SwitchId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
