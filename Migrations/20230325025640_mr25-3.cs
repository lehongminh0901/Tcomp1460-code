using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcomp1.Migrations
{
    public partial class mr253 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcadeId",
                table: "ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AcademicId",
                table: "ideas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Acade",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tcomp1UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acade_AspNetUsers_tcomp1UserId",
                        column: x => x.tcomp1UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ideas_AcadeId",
                table: "ideas",
                column: "AcadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Acade_tcomp1UserId",
                table: "Acade",
                column: "tcomp1UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_Acade_AcadeId",
                table: "ideas",
                column: "AcadeId",
                principalTable: "Acade",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_Acade_AcadeId",
                table: "ideas");

            migrationBuilder.DropTable(
                name: "Acade");

            migrationBuilder.DropIndex(
                name: "IX_ideas_AcadeId",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "AcadeId",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "AcademicId",
                table: "ideas");
        }
    }
}
