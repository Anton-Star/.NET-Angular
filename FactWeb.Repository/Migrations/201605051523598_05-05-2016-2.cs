namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _050520162 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseRFICreatedBy");
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseRFIComments");
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseCitationComments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseCitationComments", c => c.String());
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseRFIComments", c => c.String());
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseRFICreatedBy", c => c.String());
        }
    }
}
