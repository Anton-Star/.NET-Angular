namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserPasswordResetExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserPasswordResetExpirationDate");
        }
    }
}
