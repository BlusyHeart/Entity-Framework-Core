using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstates.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_BuildingTypes_BuildingTypeId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Districts_DistrictId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_PropertTypes_TypeId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTag_Property_PropertiesId",
                table: "PropertyTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Property",
                table: "Property");

            migrationBuilder.RenameTable(
                name: "Property",
                newName: "Properties");

            migrationBuilder.RenameIndex(
                name: "IX_Property_TypeId",
                table: "Properties",
                newName: "IX_Properties_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_DistrictId",
                table: "Properties",
                newName: "IX_Properties_DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_BuildingTypeId",
                table: "Properties",
                newName: "IX_Properties_BuildingTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_BuildingTypes_BuildingTypeId",
                table: "Properties",
                column: "BuildingTypeId",
                principalTable: "BuildingTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Districts_DistrictId",
                table: "Properties",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertTypes_TypeId",
                table: "Properties",
                column: "TypeId",
                principalTable: "PropertTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTag_Properties_PropertiesId",
                table: "PropertyTag",
                column: "PropertiesId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_BuildingTypes_BuildingTypeId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Districts_DistrictId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertTypes_TypeId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTag_Properties_PropertiesId",
                table: "PropertyTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.RenameTable(
                name: "Properties",
                newName: "Property");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_TypeId",
                table: "Property",
                newName: "IX_Property_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_DistrictId",
                table: "Property",
                newName: "IX_Property_DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_BuildingTypeId",
                table: "Property",
                newName: "IX_Property_BuildingTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Property",
                table: "Property",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_BuildingTypes_BuildingTypeId",
                table: "Property",
                column: "BuildingTypeId",
                principalTable: "BuildingTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Districts_DistrictId",
                table: "Property",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_PropertTypes_TypeId",
                table: "Property",
                column: "TypeId",
                principalTable: "PropertTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTag_Property_PropertiesId",
                table: "PropertyTag",
                column: "PropertiesId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
