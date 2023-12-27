using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorLoggingSeriLog.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DIMPersonelTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIMPersonelTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Personels",
                columns: table => new
                {
                    CodeMeli = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDDimPersonelType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personels", x => x.CodeMeli);
                    table.ForeignKey(
                        name: "FK_Personels_DIMPersonelTypes_IDDimPersonelType",
                        column: x => x.IDDimPersonelType,
                        principalTable: "DIMPersonelTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DIMPersonelTypes",
                columns: new[] { "ID", "Title" },
                values: new object[,]
                {
                    { 1, "حقیقی" },
                    { 2, "حقوقی" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personels_IDDimPersonelType",
                table: "Personels",
                column: "IDDimPersonelType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personels");

            migrationBuilder.DropTable(
                name: "DIMPersonelTypes");
        }
    }
}
