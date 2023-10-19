using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FortunaDigital.Infraistructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveColumnRaffleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaffleNumbers_Raffle_IdRaffle",
                table: "RaffleNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_RaffleNumbers_User_IdUser",
                table: "RaffleNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Raffle",
                table: "Raffle");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Raffle",
                newName: "Raffles");

            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "Raffles",
                type: "int",
                maxLength: 1,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Raffles",
                table: "Raffles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RaffleNumbers_Raffles_IdRaffle",
                table: "RaffleNumbers",
                column: "IdRaffle",
                principalTable: "Raffles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RaffleNumbers_Users_IdUser",
                table: "RaffleNumbers",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaffleNumbers_Raffles_IdRaffle",
                table: "RaffleNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_RaffleNumbers_Users_IdUser",
                table: "RaffleNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Raffles",
                table: "Raffles");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Raffles");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Raffles",
                newName: "Raffle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Raffle",
                table: "Raffle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RaffleNumbers_Raffle_IdRaffle",
                table: "RaffleNumbers",
                column: "IdRaffle",
                principalTable: "Raffle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RaffleNumbers_User_IdUser",
                table: "RaffleNumbers",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
