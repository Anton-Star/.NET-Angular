namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SectionChange52616 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationSection", "ApplicationSectionName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationSection", "ApplicationSectionName", c => c.String(nullable: false, maxLength: 300));
        }
    }
}
