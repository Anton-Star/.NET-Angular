namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMembershipChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMembership", "UserMembershipNumber", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserMembership", "UserMembershipNumber");
        }
    }
}
