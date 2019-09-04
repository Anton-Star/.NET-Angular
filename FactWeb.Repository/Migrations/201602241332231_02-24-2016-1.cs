namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _022420161 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionFlag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionFlag");
        }
    }
}
