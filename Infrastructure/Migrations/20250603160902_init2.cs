using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activation_Users_UserId1",
                table: "Activation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activation",
                table: "Activation");

            migrationBuilder.DropIndex(
                name: "IX_Activation_UserId1",
                table: "Activation");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Activation");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Activation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Activation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activation",
                table: "Activation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Activation_UserId",
                table: "Activation",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activation_Users_UserId",
                table: "Activation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activation_Users_UserId",
                table: "Activation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activation",
                table: "Activation");

            migrationBuilder.DropIndex(
                name: "IX_Activation_UserId",
                table: "Activation");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Activation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Activation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Activation",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activation",
                table: "Activation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activation_UserId1",
                table: "Activation",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Activation_Users_UserId1",
                table: "Activation",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
