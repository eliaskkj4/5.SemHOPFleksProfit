using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleksProfit.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectricityPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SpotPrice = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfitCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalculationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Capacity = table.Column<double>(type: "float", nullable: false),
                    Revenue = table.Column<double>(type: "float", nullable: false),
                    ElectricityCost = table.Column<double>(type: "float", nullable: false),
                    BatteryLoss = table.Column<double>(type: "float", nullable: false),
                    StartupCost = table.Column<double>(type: "float", nullable: false),
                    TotalProfit = table.Column<double>(type: "float", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfitCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpRegulation = table.Column<double>(type: "float", nullable: false),
                    DownRegulation = table.Column<double>(type: "float", nullable: false),
                    Frequency = table.Column<double>(type: "float", nullable: false),
                    CapacityUtilization = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPerformances", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricityPrices");

            migrationBuilder.DropTable(
                name: "ProfitCalculations");

            migrationBuilder.DropTable(
                name: "SystemPerformances");
        }
    }
}
