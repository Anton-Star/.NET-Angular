namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03012016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "AddressProvince", c => c.String());
            DropColumn("dbo.Address", "AddressCounty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Address", "AddressCounty", c => c.String());
            DropColumn("dbo.Address", "AddressProvince");
        }
    }
}
