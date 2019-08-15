using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
	public partial class AddingEnglishName : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "EnglishName",
				table: "Sections",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "EnglishName",
				table: "Products",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "EnglishName",
				table: "ProductModels",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "EnglishName",
				table: "Sections");

			migrationBuilder.DropColumn(
				name: "EnglishName",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "EnglishName",
				table: "ProductModels");
		}
	}
}
