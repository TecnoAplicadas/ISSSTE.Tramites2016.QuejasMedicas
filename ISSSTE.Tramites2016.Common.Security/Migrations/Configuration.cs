using System.Data.Entity.Migrations;
using ISSSTE.Tramites2016.Common.Security.Identity;

namespace ISSSTE.Tramites2016.Common.Security.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<IsssteIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}