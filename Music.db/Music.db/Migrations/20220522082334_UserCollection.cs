using Microsoft.EntityFrameworkCore.Migrations;

namespace Music.db.Migrations
{
    public partial class UserCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCollectionSong");

            migrationBuilder.CreateTable(
                name: "UserCollection",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserCollection_Song_SongID",
                        column: x => x.SongID,
                        principalTable: "Song",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCollection_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCollection_SongID",
                table: "UserCollection",
                column: "SongID");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollection_UserID",
                table: "UserCollection",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCollection");

            migrationBuilder.CreateTable(
                name: "UserCollectionSong",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollectionSong", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserCollectionSong_Song_SongID",
                        column: x => x.SongID,
                        principalTable: "Song",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCollectionSong_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCollectionSong_SongID",
                table: "UserCollectionSong",
                column: "SongID");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollectionSong_UserID",
                table: "UserCollectionSong",
                column: "UserID");
        }
    }
}
