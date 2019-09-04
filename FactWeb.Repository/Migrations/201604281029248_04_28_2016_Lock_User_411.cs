namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04_28_2016_Lock_User_411 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserFailedLoginAttempts", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserFailedLoginAttempts");
        }
    }
}
