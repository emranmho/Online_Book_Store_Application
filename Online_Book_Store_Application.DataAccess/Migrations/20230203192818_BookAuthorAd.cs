using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStoreApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BookAuthorAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_UserDetails_UserDetailsId",
                table: "BookAuthors");

            migrationBuilder.DropIndex(
                name: "IX_BookAuthors_UserDetailsId",
                table: "BookAuthors");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "BookAuthors");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BookAuthors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BookAuthors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "BookAuthors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "BookAuthors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "BookAuthors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "BookAuthors");

            migrationBuilder.AddColumn<int>(
                name: "UserDetailsId",
                table: "BookAuthors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_UserDetailsId",
                table: "BookAuthors",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_UserDetails_UserDetailsId",
                table: "BookAuthors",
                column: "UserDetailsId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
