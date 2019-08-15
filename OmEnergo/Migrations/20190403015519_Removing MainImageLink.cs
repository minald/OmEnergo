using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OmEnergo.Migrations
{
	public partial class RemovingMainImageLink : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "MainImageLink",
				table: "Sections");

			migrationBuilder.DropColumn(
				name: "MainImageLink",
				table: "Products");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "MainImageLink",
				table: "Sections",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "MainImageLink",
				table: "Products",
				nullable: true);
		}
	}
}
