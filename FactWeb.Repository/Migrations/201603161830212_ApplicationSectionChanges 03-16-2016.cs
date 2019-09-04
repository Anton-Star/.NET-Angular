namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationSectionChanges03162016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSection", "ApplicationSectionHelpText", c => c.String());
            AddColumn("dbo.ApplicationSection", "ApplicationSectionIsVariance", c => c.Boolean());
            AddColumn("dbo.ApplicationSection", "ApplicationSectionVersion", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSection", "ApplicationSectionVersion");
            DropColumn("dbo.ApplicationSection", "ApplicationSectionIsVariance");
            DropColumn("dbo.ApplicationSection", "ApplicationSectionHelpText");
        }
    }
}
