using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyChiTieu_3TL.Migrations
{
    /// <inheritdoc />
    public partial class mi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMuc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoaiDanhMuc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhMuc__3214EC07A011E51C", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaiKhoan__3214EC074873427D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiaoDich",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoaiGiaoDich = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayGiaoDich = table.Column<DateOnly>(type: "date", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdDanhMuc = table.Column<int>(type: "int", nullable: false),
                    IdTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GiaoDich__3214EC07CDE51CC7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__GiaoDich__IdDanh__4E88ABD4",
                        column: x => x.IdDanhMuc,
                        principalTable: "DanhMuc",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__GiaoDich__IdTaiK__4F7CD00D",
                        column: x => x.IdTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDich_IdDanhMuc",
                table: "GiaoDich",
                column: "IdDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDich_IdTaiKhoan",
                table: "GiaoDich",
                column: "IdTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "UQ__TaiKhoan__A9D10534E0599D9B",
                table: "TaiKhoan",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiaoDich");

            migrationBuilder.DropTable(
                name: "DanhMuc");

            migrationBuilder.DropTable(
                name: "TaiKhoan");
        }
    }
}
