using Microsoft.EntityFrameworkCore.Migrations;

namespace truckplannermodel.Migrations
{
    public partial class RobustererRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TruckPlans_Drivers_DriverId",
                table: "TruckPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_TruckPlans_Trucks_TruckId",
                table: "TruckPlans");

            migrationBuilder.AlterColumn<int>(
                name: "TruckId",
                table: "TruckPlans",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "TruckPlans",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TruckPlans_Drivers_DriverId",
                table: "TruckPlans",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TruckPlans_Trucks_TruckId",
                table: "TruckPlans",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TruckPlans_Drivers_DriverId",
                table: "TruckPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_TruckPlans_Trucks_TruckId",
                table: "TruckPlans");

            migrationBuilder.AlterColumn<int>(
                name: "TruckId",
                table: "TruckPlans",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "TruckPlans",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TruckPlans_Drivers_DriverId",
                table: "TruckPlans",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TruckPlans_Trucks_TruckId",
                table: "TruckPlans",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
