using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookshelf.Migrations
{
    /// <inheritdoc />
    public partial class AjusteLivroNaLista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "LivrosNaLista");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataCadastro", "SenhaHash" },
                values: new object[] { new DateTime(2025, 5, 29, 2, 0, 9, 134, DateTimeKind.Utc).AddTicks(5923), "$2a$11$0RYaZe52fhLn/KueaZSNC.3PJpvvwADk4F3.Fn06VdN.qw2DLcvlq" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LivrosNaLista",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataCadastro", "SenhaHash" },
                values: new object[] { new DateTime(2025, 5, 29, 1, 50, 23, 210, DateTimeKind.Utc).AddTicks(3408), "$2a$11$Os46nvjVqhEpLRhZ6MyYaubhABE4O/XW2GcidaeHKi2yfJvt47LZq" });
        }
    }
}
