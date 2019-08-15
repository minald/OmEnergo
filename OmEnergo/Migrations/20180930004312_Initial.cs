using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Sections",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Description = table.Column<string>(nullable: true),
					MainImageLink = table.Column<string>(nullable: true),
					Name = table.Column<string>(nullable: true),
					ParentSectionId = table.Column<int>(nullable: true),
					ProductModelProperties = table.Column<string>(nullable: true),
					ProductProperties = table.Column<string>(nullable: true),
					SequenceNumber = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Sections", x => x.Id);
					table.ForeignKey(
						name: "FK_Sections_Sections_ParentSectionId",
						column: x => x.ParentSectionId,
						principalTable: "Sections",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Description = table.Column<string>(nullable: true),
					MainImageLink = table.Column<string>(nullable: true),
					Name = table.Column<string>(nullable: true),
					Properties = table.Column<string>(nullable: true),
					SectionId = table.Column<int>(nullable: true),
					SequenceNumber = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Products", x => x.Id);
					table.ForeignKey(
						name: "FK_Products_Sections_SectionId",
						column: x => x.SectionId,
						principalTable: "Sections",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "ProductModels",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(nullable: true),
					Price = table.Column<double>(nullable: false),
					ProductId = table.Column<int>(nullable: true),
					Properties = table.Column<string>(nullable: true),
					SectionId = table.Column<int>(nullable: true),
					SequenceNumber = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProductModels", x => x.Id);
					table.ForeignKey(
						name: "FK_ProductModels_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_ProductModels_Sections_SectionId",
						column: x => x.SectionId,
						principalTable: "Sections",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ProductModels_ProductId",
				table: "ProductModels",
				column: "ProductId");

			migrationBuilder.CreateIndex(
				name: "IX_ProductModels_SectionId",
				table: "ProductModels",
				column: "SectionId");

			migrationBuilder.CreateIndex(
				name: "IX_Products_SectionId",
				table: "Products",
				column: "SectionId");

			migrationBuilder.CreateIndex(
				name: "IX_Sections_ParentSectionId",
				table: "Sections",
				column: "ParentSectionId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ProductModels");

			migrationBuilder.DropTable(
				name: "Products");

			migrationBuilder.DropTable(
				name: "Sections");
		}
	}
}
