using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeRoleNeverGeneratedId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE AppUserDomainRoles DROP CONSTRAINT FK_AppUserDomainRoles_AppRoles_RoleId;");
            migrationBuilder.Sql("ALTER TABLE UserRolePermissions DROP CONSTRAINT FK_UserRolePermissions_AppRoles_RoleId;");

            migrationBuilder.Sql("ALTER TABLE AppRoles DROP CONSTRAINT PK_AppRoles;");

            migrationBuilder.Sql("ALTER TABLE AppRoles ADD Id_new bigint NOT NULL DEFAULT 0;");
            migrationBuilder.Sql("UPDATE AppRoles SET Id_new = Id;");
            migrationBuilder.Sql("ALTER TABLE AppRoles DROP COLUMN Id;");
            migrationBuilder.Sql("EXEC sp_rename 'AppRoles.Id_new', 'Id', 'COLUMN';");

            migrationBuilder.Sql("ALTER TABLE AppRoles ADD CONSTRAINT PK_AppRoles PRIMARY KEY (Id);");

            migrationBuilder.Sql(@"ALTER TABLE AppUserDomainRoles ADD CONSTRAINT FK_AppUserDomainRoles_AppRoles_RoleId 
        FOREIGN KEY (RoleId) REFERENCES AppRoles(Id) ON DELETE CASCADE;");
            migrationBuilder.Sql(@"ALTER TABLE UserRolePermissions ADD CONSTRAINT FK_UserRolePermissions_AppRoles_RoleId 
        FOREIGN KEY (RoleId) REFERENCES AppRoles(Id);");
        }



    }
}
