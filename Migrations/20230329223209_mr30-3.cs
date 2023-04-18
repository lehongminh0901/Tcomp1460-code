using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcomp1.Migrations
{
    public partial class mr303 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_Acade_AcadeId",
                table: "ideas");

            migrationBuilder.DropTable(
                name: "Acade");

            migrationBuilder.DropColumn(
                name: "AcademicId",
                table: "ideas");

            migrationBuilder.RenameColumn(
                name: "AcadeId",
                table: "ideas",
                newName: "IdAdemic");

            migrationBuilder.RenameIndex(
                name: "IX_ideas_AcadeId",
                table: "ideas",
                newName: "IX_ideas_IdAdemic");

            migrationBuilder.CreateTable(
                name: "Ademics",
                columns: table => new
                {
                    IdAdemic = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Enddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tcomp1UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ademics", x => x.IdAdemic);
                    table.ForeignKey(
                        name: "FK_Ademics_AspNetUsers_tcomp1UserId",
                        column: x => x.tcomp1UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ademics_tcomp1UserId",
                table: "Ademics",
                column: "tcomp1UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_Ademics_IdAdemic",
                table: "ideas",
                column: "IdAdemic",
                principalTable: "Ademics",
                principalColumn: "IdAdemic",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_Ademics_IdAdemic",
                table: "ideas");

            migrationBuilder.DropTable(
                name: "Ademics");

            migrationBuilder.RenameColumn(
                name: "IdAdemic",
                table: "ideas",
                newName: "AcadeId");

            migrationBuilder.RenameIndex(
                name: "IX_ideas_IdAdemic",
                table: "ideas",
                newName: "IX_ideas_AcadeId");

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
                    tcomp1UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "IX_Acade_tcomp1UserId",
                table: "Acade",
                column: "tcomp1UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_Acade_AcadeId",
                table: "ideas",
                column: "AcadeId",
                principalTable: "Acade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
