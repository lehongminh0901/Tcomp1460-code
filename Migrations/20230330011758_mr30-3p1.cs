using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcomp1.Migrations
{
    public partial class mr303p1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_Ademics_IdAdemic",
                table: "ideas");

            migrationBuilder.DropIndex(
                name: "IX_ideas_IdAdemic",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "IdAdemic",
                table: "ideas");

            migrationBuilder.CreateTable(
                name: "AdemicIdea",
                columns: table => new
                {
                    AdemicsIdAdemic = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdeasId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdemicIdea", x => new { x.AdemicsIdAdemic, x.IdeasId });
                    table.ForeignKey(
                        name: "FK_AdemicIdea_Ademics_AdemicsIdAdemic",
                        column: x => x.AdemicsIdAdemic,
                        principalTable: "Ademics",
                        principalColumn: "IdAdemic",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdemicIdea_ideas_IdeasId",
                        column: x => x.IdeasId,
                        principalTable: "ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdemicIdea_IdeasId",
                table: "AdemicIdea",
                column: "IdeasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdemicIdea");

            migrationBuilder.AddColumn<string>(
                name: "IdAdemic",
                table: "ideas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ideas_IdAdemic",
                table: "ideas",
                column: "IdAdemic");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_Ademics_IdAdemic",
                table: "ideas",
                column: "IdAdemic",
                principalTable: "Ademics",
                principalColumn: "IdAdemic",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
