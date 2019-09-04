namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentUpdate13117 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Document", "DocumentIncludeInReporting", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Document", "DocumentIncludeInReporting");
        }
    }
}
