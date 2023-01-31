using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstates.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_PropertTypes_PropertTypeId",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Property_PropertTypeId",
                table: "Property");

            migrationBuilder.RenameColumn(
                name: "PropertTypeId",
                table: "Property",
                newName: "PropertyTypeId");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Property_TypeId",
                table: "Property",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_PropertTypes_TypeId",
                table: "Property",
                column: "TypeId",
                principalTable: "PropertTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_PropertTypes_TypeId",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Property_TypeId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Property");

            migrationBuilder.RenameColumn(
                name: "PropertyTypeId",
                table: "Property",
                newName: "PropertTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertTypeId",
                table: "Property",
                column: "PropertTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_PropertTypes_PropertTypeId",
                table: "Property",
                column: "PropertTypeId",
                principalTable: "PropertTypes",
                principalColumn: "Id");
        }
    }
}
