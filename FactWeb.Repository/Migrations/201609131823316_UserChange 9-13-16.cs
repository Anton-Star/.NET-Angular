namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChange91316 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserCanManageUsers", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserCanManageUsers");
        }
    }
}
