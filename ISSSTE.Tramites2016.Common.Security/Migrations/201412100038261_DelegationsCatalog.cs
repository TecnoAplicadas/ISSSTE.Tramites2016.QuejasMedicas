using System.Data.Entity.Migrations;

namespace ISSSTE.Tramites2016.Common.Security.Migrations
{
    public partial class DelegationsCatalog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "admin.IsssteDelegations",
                    c => new
                    {
                        Id = c.Int(false),
                        State = c.String(false),
                        Name = c.String(false)
                    })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("admin.IsssteDelegations");
        }
    }
}