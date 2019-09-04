namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentLibrary112916 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Document", "RequestValues", c => c.String());
            DropColumn("dbo.Document", "DocumentPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Document", "DocumentPath", c => c.String(nullable: false, maxLength: 2000));
            DropColumn("dbo.Document", "RequestValues");
        }
    }
}
