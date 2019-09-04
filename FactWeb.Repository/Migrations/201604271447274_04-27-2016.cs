namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04272016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseCitationComments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseCitationComments");
        }
    }
}
