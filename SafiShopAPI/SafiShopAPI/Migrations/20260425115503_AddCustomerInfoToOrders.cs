using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafiShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerInfoToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerEmail",
                table: "Orders",
                newName: "CustomerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Orders",
                newName: "CustomerEmail");
        }
    }
}
