using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kr_v7.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chickens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    EggsPerMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    CageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chickens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Salary = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChickenId = table.Column<int>(type: "INTEGER", nullable: true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cages_Chickens_ChickenId",
                        column: x => x.ChickenId,
                        principalTable: "Chickens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cages_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cages_ChickenId",
                table: "Cages",
                column: "ChickenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cages_EmployeeId",
                table: "Cages",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cages");

            migrationBuilder.DropTable(
                name: "Chickens");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
