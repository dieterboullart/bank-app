using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Data.Migrations
{
    public partial class _0001_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m),
                    MinBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    HolderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Persons_HolderId",
                        column: x => x.HolderId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    SourceBankAccountId = table.Column<int>(type: "INTEGER", nullable: true),
                    DestinationBankAccountId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_HolderId",
                table: "Accounts",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
            
            // Add initial data to database
            SeedDatabase(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Persons");
        }
        
        private void SeedDatabase(MigrationBuilder migrationBuilder)
        {
            // Persons
            migrationBuilder.InsertData(
                table: "Persons", 
                columns: new []{ "Id", "FirstName", "LastName" },
                values: new object[] {1, "Florian", "Goeteyn"}
            );

            migrationBuilder.InsertData(
                table: "Persons", 
                columns: new []{ "Id", "FirstName", "LastName" },
                values: new object[] {2, "Pieter", "Remerie"}
            );

            migrationBuilder.InsertData(
                table: "Persons", 
                columns: new []{ "Id", "FirstName", "LastName" },
                values: new object[] {3, "Bart", "Bruynooghe"}
            );

            migrationBuilder.InsertData(
                table: "Persons", 
                columns: new []{ "Id", "FirstName", "LastName" },
                values: new object[] {4, "Jelle", "Vandendriesche"}
            );
            
            // Bank accounts
            migrationBuilder.InsertData(
                table: "Accounts", 
                columns: new []
                {"Id", "Balance", "MinBalance", "HolderId"},
                values: new object[] {1, 1023, 0, 1}
            );
            migrationBuilder.InsertData(
                table: "Accounts", 
                columns: new []
                {"Id", "Balance", "MinBalance", "HolderId"},
                values: new object[] {2, 768, -500, 2}
            );
            migrationBuilder.InsertData(
                table: "Accounts", 
                columns: new []
                {"Id", "Balance", "MinBalance", "HolderId"},
                values: new object[] {3, 1540, 0, 3}
            );
            migrationBuilder.InsertData(
                table: "Accounts", 
                columns: new []
                {"Id", "Balance", "MinBalance", "HolderId"},
                values: new object[] {4, 2088, -250, 4}
            );
            migrationBuilder.InsertData(
                table: "Accounts", 
                columns: new []
                {"Id", "Balance", "MinBalance", "HolderId"},
                values: new object[] {5, 0, 0, 4}
            );
        }
    }
}
