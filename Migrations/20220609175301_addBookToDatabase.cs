using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadBook.Migrations
{
    /// <inheritdoc />
    public partial class addBookToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "allBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookPages = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PdfRequestId = table.Column<int>(type: "int", nullable: false),
                    PageIndex = table.Column<int>(type: "int", nullable: false),
                    TextPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextWidthAndHeight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkType = table.Column<bool>(type: "bit", nullable: false),
                    GoToPage = table.Column<int>(type: "int", nullable: false),
                    GoToPageTextPostion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoToPageTextWidthAndHeight = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "allBooks");

            migrationBuilder.DropTable(
                name: "contents");
        }
    }
}
