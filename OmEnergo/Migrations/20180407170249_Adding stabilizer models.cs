using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
    public partial class Addingstabilizermodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booklets");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Stabilizers");

            migrationBuilder.CreateTable(
                name: "StabilizerModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: true),
                    StabilizerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StabilizerModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StabilizerModels_Stabilizers_StabilizerId",
                        column: x => x.StabilizerId,
                        principalTable: "Stabilizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StabilizerModels_StabilizerId",
                table: "StabilizerModels",
                column: "StabilizerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StabilizerModels");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Stabilizers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Booklets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Link = table.Column<string>(nullable: true),
                    StabilizerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booklets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booklets_Stabilizers_StabilizerId",
                        column: x => x.StabilizerId,
                        principalTable: "Stabilizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Link = table.Column<string>(nullable: true),
                    StabilizerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Stabilizers_StabilizerId",
                        column: x => x.StabilizerId,
                        principalTable: "Stabilizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booklets_StabilizerId",
                table: "Booklets",
                column: "StabilizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_StabilizerId",
                table: "Pictures",
                column: "StabilizerId");
        }
    }
}
