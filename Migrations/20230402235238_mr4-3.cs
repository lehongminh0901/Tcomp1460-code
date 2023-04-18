using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcomp1.Migrations
{
    public partial class mr43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_Ademics_AdemicsIdAdemic",
                table: "ideas");

            migrationBuilder.DropIndex(
                name: "IX_ideas_AdemicsIdAdemic",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "AdemicsIdAdemic",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ideas");

            migrationBuilder.AddColumn<string>(
                name: "IdAdemic",
                table: "ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdCategory",
                table: "ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ideas_IdAdemic",
                table: "ideas",
                column: "IdAdemic");

            migrationBuilder.CreateIndex(
                name: "IX_ideas_IdCategory",
                table: "ideas",
                column: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_Ademics_IdAdemic",
                table: "ideas",
                column: "IdAdemic",
                principalTable: "Ademics",
                principalColumn: "IdAdemic",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_categories_IdCategory",
                table: "ideas",
                column: "IdCategory",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_Ademics_IdAdemic",
                table: "ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_ideas_categories_IdCategory",
                table: "ideas");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "IX_ideas_IdAdemic",
                table: "ideas");

            migrationBuilder.DropIndex(
                name: "IX_ideas_IdCategory",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "IdAdemic",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                table: "ideas");

            migrationBuilder.AddColumn<string>(
                name: "AdemicsIdAdemic",
                table: "ideas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ideas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ideas_AdemicsIdAdemic",
                table: "ideas",
                column: "AdemicsIdAdemic");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_Ademics_AdemicsIdAdemic",
                table: "ideas",
                column: "AdemicsIdAdemic",
                principalTable: "Ademics",
                principalColumn: "IdAdemic");
        }
    }
}
