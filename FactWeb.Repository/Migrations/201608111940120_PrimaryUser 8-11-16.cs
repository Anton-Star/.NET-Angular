namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryUser81116 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Organization", "PrimaryUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organization", "PrimaryUserId", c => c.Int());
        }
    }
}
