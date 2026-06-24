using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_IhorParfonov_63368.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Incomes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Expenses",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Expenses");
        }
    }
}
