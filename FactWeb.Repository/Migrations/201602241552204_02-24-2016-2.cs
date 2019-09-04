namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _022420162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseFlag", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseComments", c => c.String());
            DropColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionFlag");
            DropColumn("dbo.ApplicationSection", "ApplicationSectionComments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationSection", "ApplicationSectionComments", c => c.String());
            AddColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionFlag", c => c.Boolean(nullable: false));
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseComments");
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseFlag");
        }
    }
}
