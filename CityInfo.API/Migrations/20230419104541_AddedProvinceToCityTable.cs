using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.API.Migrations
{
    public partial class AddedProvinceToCityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Cities",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                table: "pointsOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 7, 3, "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.", "Eiffel Tower" });

            migrationBuilder.InsertData(
                table: "pointsOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 8, 3, "The world's largest museum.", "The Louvre" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "pointsOfInterests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "pointsOfInterests",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Cities");
        }
    }
}
