using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class MessagesEntityAddedUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecepientDeleted",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "RecepientUsername",
                table: "Messages",
                newName: "RecipientUsername");

            migrationBuilder.RenameColumn(
                name: "RecepientId",
                table: "Messages",
                newName: "RecipientDeleted");

            migrationBuilder.AlterColumn<int>(
                name: "RecipientId",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientUsername",
                table: "Messages",
                newName: "RecepientUsername");

            migrationBuilder.RenameColumn(
                name: "RecipientDeleted",
                table: "Messages",
                newName: "RecepientId");

            migrationBuilder.AlterColumn<int>(
                name: "RecipientId",
                table: "Messages",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<bool>(
                name: "RecepientDeleted",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
