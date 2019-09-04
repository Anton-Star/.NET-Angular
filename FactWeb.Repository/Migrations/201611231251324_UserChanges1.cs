namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChanges1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserPhoneExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserPhoneExtension");
        }
    }
}
