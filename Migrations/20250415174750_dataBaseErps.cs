using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace measurement_generator.Migrations
{
    /// <inheritdoc />
    public partial class dataBaseErps : Migration
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
                    haveFile = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erps", x => x.CodId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Erps");
        }
    }
}
