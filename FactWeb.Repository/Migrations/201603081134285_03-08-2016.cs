namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03082016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseRFIComments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseRFIComments");
        }
    }
}
