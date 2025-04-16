using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace measurement_generator.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Erps",
                columns: table => new
                {
                    CodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DnsIot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ScopeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssociatedGroups = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Authenticated = table.Column<bool>(type: "bit", nullable: false),
                    PackageQuantity = table.Column<int>(type: "int", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndexH = table.Column<double>(type: "float", nullable: false),
                    HaveFile = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erps", x => x.CodId);
                });

            migrationBuilder.CreateTable(
                name: "LastMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codId = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    pressureInputHighLimit = table.Column<float>(type: "real", nullable: true),
                    pressureInputLowLimit = table.Column<float>(type: "real", nullable: true),
                    pressureInput = table.Column<float>(type: "real", nullable: true),
                    pressureOutput = table.Column<float>(type: "real", nullable: true),
                    pressureOutputHighLimit = table.Column<float>(type: "real", nullable: true),
                    pressureOutputLowLimit = table.Column<float>(type: "real", nullable: true),
                    shutoffZASL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    flow = table.Column<float>(type: "real", nullable: true),
                    PDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    regulator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryReg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastMeasurements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Erps");

            migrationBuilder.DropTable(
                name: "LastMeasurements");
        }
    }
}
