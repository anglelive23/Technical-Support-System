using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Status
            // Status Table
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Name" },
                values: new object[] { "New" }
            );

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Name" },
                values: new object[] { "Pending" }
            );

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Name" },
                values: new object[] { "In Progress" }
            );

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Name" },
                values: new object[] { "Completed" }
            );

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Name" },
                values: new object[] { "Archived" }
            );
            #endregion

            #region Department
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Name" },
                values: new object[] { "UI/UX Design" }
            );

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Name" },
                values: new object[] { "Frontend" }
            );

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Name" },
                values: new object[] { "Backend" }
            );

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Name" },
                values: new object[] { "IT" }
            );

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Name" },
                values: new object[] { "Mobile" }
            );
            #endregion

            #region Priority
            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Name" },
                values: new object[] { "Low" }
            );

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Name" },
                values: new object[] { "Medium" }
            );

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Name" },
                values: new object[] { "High" }
            );
            #endregion

            #region Roles
            migrationBuilder.InsertData(
                table: "Roles",
                schema: "security",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Client", "CLIENT", Guid.NewGuid().ToString() }
            );

            migrationBuilder.InsertData(
                table: "Roles",
                schema: "security",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Employee", "EMPLOYEE", Guid.NewGuid().ToString() }
            );

            migrationBuilder.InsertData(
                table: "Roles",
                schema: "security",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Admin", "ADMIN", Guid.NewGuid().ToString() }
            );
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Status Table
            migrationBuilder.Sql("DELETE FROM [dbo].[Statuses]");
            // Department Table
            migrationBuilder.Sql("DELETE FROM [dbo].[Departments]");
            // Priority Table
            migrationBuilder.Sql("DELETE FROM [dbo].[Priorities]");
            // Roles Table
            migrationBuilder.Sql("DELETE FROM [security].[Roles]");
        }
    }
}
