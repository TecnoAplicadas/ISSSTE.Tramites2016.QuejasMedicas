using System.Data.Entity.Migrations;

namespace ISSSTE.Tramites2016.Common.Security.Migrations
{
    public class UserProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("admin.IsssteProcedures", "DirectionId", "admin.IsssteDirections");
            DropIndex("admin.IsssteProcedures", new[] {"DirectionId"});
            CreateTable(
                    "admin.IsssteUserProperties",
                    c => new
                    {
                        Id = c.String(false, 128),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                        Value = c.String(),
                        ValueDescription = c.String(true)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("admin.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);

            AddColumn("admin.IsssteProcedures", "Area", c => c.String());
            AlterColumn("admin.AspNetUsers", "Name", c => c.String());
            DropColumn("admin.IsssteProcedures", "DirectionId");
            DropColumn("admin.AspNetUsers", "DelegationId");
            DropTable("admin.IsssteDirections");
        }

        public override void Down()
        {
            CreateTable(
                    "admin.IsssteDirections",
                    c => new
                    {
                        Id = c.String(false, 128),
                        Description = c.String(false)
                    })
                .PrimaryKey(t => t.Id);

            AddColumn("admin.AspNetUsers", "DelegationId", c => c.Int(false));
            AddColumn("admin.IsssteProcedures", "DirectionId", c => c.String(maxLength: 128));
            DropForeignKey("admin.IsssteUserProperties", "UserId", "admin.AspNetUsers");
            DropIndex("admin.IsssteUserProperties", new[] {"UserId"});
            AlterColumn("admin.AspNetUsers", "Name", c => c.String(false));
            DropColumn("admin.IsssteProcedures", "Area");
            DropTable("admin.IsssteUserProperties");
            CreateIndex("admin.IsssteProcedures", "DirectionId");
            AddForeignKey("admin.IsssteProcedures", "DirectionId", "admin.IsssteDirections", "Id");
        }
    }
}