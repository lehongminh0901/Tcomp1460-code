﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcomp1.Migrations
{
    public partial class mr303p3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Ademics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Ademics");
        }
    }
}
