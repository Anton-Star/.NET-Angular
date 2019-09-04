namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12142016IsPrimaryAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "AddressPhone", c => c.String());
            AddColumn("dbo.SiteAddress", "IsPrimaryAddress", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SiteAddress", "IsPrimaryAddress");
            DropColumn("dbo.Address", "AddressPhone");
        }
    }
}
