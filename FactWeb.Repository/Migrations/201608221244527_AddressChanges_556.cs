namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddressChanges_556 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Address", "AddressState", c => c.Int());
            AlterColumn("dbo.Address", "AddressCountry", c => c.Int(nullable: false));
            CreateIndex("dbo.Address", "AddressState");
            CreateIndex("dbo.Address", "AddressCountry");
            //AddForeignKey("dbo.Address", "AddressCountry", "dbo.Country", "CountryId");
            AddForeignKey("dbo.Address", "AddressState", "dbo.State", "StateId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Address", "AddressState", "dbo.State");
            //DropForeignKey("dbo.Address", "AddressCountry", "dbo.Country");
            DropIndex("dbo.Address", new[] { "AddressCountry" });
            DropIndex("dbo.Address", new[] { "AddressState" });
            AlterColumn("dbo.Address", "AddressCountry", c => c.String());
            AlterColumn("dbo.Address", "AddressState", c => c.String());
        }
    }
}
