using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AdminLogDb
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestAdminLog",
                columns: table => new
                {
                    RequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlCode = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceMethodId = table.Column<int>(type: "int", nullable: false),
                    ServiceOrginalId = table.Column<int>(type: "int", nullable: false),
                    MethodOrginalId = table.Column<int>(type: "int", nullable: false),
                    MethodInput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SummeryData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAdminLog", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "ResponseAdminLog",
                columns: table => new
                {
                    ResponseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlCode = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceMethodId = table.Column<int>(type: "int", nullable: false),
                    ServiceOrginalId = table.Column<int>(type: "int", nullable: false),
                    MethodOrginalId = table.Column<int>(type: "int", nullable: false),
                    MethodOutput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummeryData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorCode = table.Column<int>(type: "int", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointerId = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    MethodInput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseAdminLog", x => x.ResponseId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestAdminLog");

            migrationBuilder.DropTable(
                name: "ResponseAdminLog");
        }
    }
}
