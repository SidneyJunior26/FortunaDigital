using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FortunaDigital.Infraistructure.Migrations
{
    /// <inheritdoc />
    public partial class InicialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdRaffleNumber = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaymentMethod = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    QrCode = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PixCode = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BarCode = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentLink = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalValue = table.Column<float>(type: "float(7,2)", nullable: false),
                    Paid = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Raffle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalValue = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    TotalByNumber = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Amount = table.Column<int>(type: "int(4)", nullable: false),
                    DrawDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Rules = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Level = table.Column<int>(type: "int(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raffle", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int(2)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RaffleNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdUser = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdRaffle = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdPayment = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Amount = table.Column<int>(type: "int(10)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaffleNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaffleNumbers_Payments_IdPayment",
                        column: x => x.IdPayment,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaffleNumbers_Raffle_IdRaffle",
                        column: x => x.IdRaffle,
                        principalTable: "Raffle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaffleNumbers_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RaffleNumbers_IdPayment",
                table: "RaffleNumbers",
                column: "IdPayment",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaffleNumbers_IdRaffle",
                table: "RaffleNumbers",
                column: "IdRaffle");

            migrationBuilder.CreateIndex(
                name: "IX_RaffleNumbers_IdUser",
                table: "RaffleNumbers",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaffleNumbers");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Raffle");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
