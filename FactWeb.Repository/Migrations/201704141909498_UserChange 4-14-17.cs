namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChange41417 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserTwoFactorCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserTwoFactorCode");
        }
    }
}
