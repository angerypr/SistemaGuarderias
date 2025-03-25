using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGuarderias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guarderias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guarderias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tutores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GuarderiaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_Guarderias_GuarderiaId",
                        column: x => x.GuarderiaId,
                        principalTable: "Guarderias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ninos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    GuarderiaId = table.Column<int>(type: "int", nullable: false),
                    TutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ninos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ninos_Guarderias_GuarderiaId",
                        column: x => x.GuarderiaId,
                        principalTable: "Guarderias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ninos_Tutores_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Tutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NinoId = table.Column<int>(type: "int", nullable: false),
                    GuarderiaId = table.Column<int>(type: "int", nullable: false),
                    Presente = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asistencias_Guarderias_GuarderiaId",
                        column: x => x.GuarderiaId,
                        principalTable: "Guarderias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asistencias_Ninos_NinoId",
                        column: x => x.NinoId,
                        principalTable: "Ninos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_GuarderiaId",
                table: "Asistencias",
                column: "GuarderiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_NinoId_Fecha",
                table: "Asistencias",
                columns: new[] { "NinoId", "Fecha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Cedula",
                table: "Empleados",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_CorreoElectronico",
                table: "Empleados",
                column: "CorreoElectronico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_GuarderiaId",
                table: "Empleados",
                column: "GuarderiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Nombre",
                table: "Empleados",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Guarderias_Nombre",
                table: "Guarderias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ninos_GuarderiaId",
                table: "Ninos",
                column: "GuarderiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ninos_Nombre",
                table: "Ninos",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Ninos_TutorId",
                table: "Ninos",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutores_Cedula",
                table: "Tutores",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tutores_CorreoElectronico",
                table: "Tutores",
                column: "CorreoElectronico",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencias");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Ninos");

            migrationBuilder.DropTable(
                name: "Guarderias");

            migrationBuilder.DropTable(
                name: "Tutores");
        }
    }
}
