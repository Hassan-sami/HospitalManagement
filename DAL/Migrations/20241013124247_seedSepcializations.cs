using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seedSepcializations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
             * 
Internal medicine
Surgery
Emergency medicine
Family medicine
Pathology
Pediatrics
Pediatrics
             * 
             * 
             */
            migrationBuilder.InsertData
                (
                    table: "Specialization",
                    columns : new string[] {"Id", "Name"},
                    values : new object[] {1, "Internal medicine" }

                    
                );
            migrationBuilder.InsertData
                (
                    table: "Specialization",
                    columns: new string[] { "Id", "Name" },
                    values: new object[] { 2, "Surgery" }


                );

            migrationBuilder.InsertData
                (
                    table: "Specialization",
                    columns: new string[] { "Id", "Name" },
                    values: new object[] { 3, "Emergency medicine" }


                );
            migrationBuilder.InsertData
                (
                    table: "Specialization",
                    columns: new string[] { "Id", "Name" },
                    values: new object[] { 4, "Family medicine" }


                );
            migrationBuilder.InsertData
                (
                    table: "Specialization",
                    columns: new string[] { "Id", "Name" },
                    values: new object[] { 5, "Pathology" }


                );
            migrationBuilder.InsertData
                (
                    table: "Specialization",
                    columns: new string[] { "Id", "Name" },
                    values: new object[] { 6, "Neurology" }


                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [Specialization]");
        }
    }
}
