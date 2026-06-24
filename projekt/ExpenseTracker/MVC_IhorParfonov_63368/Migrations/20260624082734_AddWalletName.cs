using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_IhorParfonov_63368.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WalletName",
                table: "UserSettings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletName",
                table: "UserSettings");
        }
    }
}
