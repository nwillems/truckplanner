using Microsoft.EntityFrameworkCore.Migrations;

namespace truckplannermodel.Migrations
{
    public partial class ComputedStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TruckPlanId",
                table: "LocationLogEntry",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationLogEntry_TruckPlanId",
                table: "LocationLogEntry",
                column: "TruckPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLogEntry_TruckPlans_TruckPlanId",
                table: "LocationLogEntry",
                column: "TruckPlanId",
                principalTable: "TruckPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationLogEntry_TruckPlans_TruckPlanId",
                table: "LocationLogEntry");

            migrationBuilder.DropIndex(
                name: "IX_LocationLogEntry_TruckPlanId",
                table: "LocationLogEntry");

            migrationBuilder.DropColumn(
                name: "TruckPlanId",
                table: "LocationLogEntry");
        }
    }
}
