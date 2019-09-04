namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04_27_2016_UserLock_411 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserIsLocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserIsLocked");
        }
    }
}
