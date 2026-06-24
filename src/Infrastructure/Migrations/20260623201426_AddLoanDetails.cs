using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LnNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BorrowerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TenureMonths = table.Column<int>(type: "int", nullable: false),
                    EmiAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisbursementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmisPaid = table.Column<int>(type: "int", nullable: false),
                    EmisRemaining = table.Column<int>(type: "int", nullable: false),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Collateral = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanDetails_LnNo",
                table: "LoanDetails",
                column: "LnNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanDetails");
        }
    }
}
