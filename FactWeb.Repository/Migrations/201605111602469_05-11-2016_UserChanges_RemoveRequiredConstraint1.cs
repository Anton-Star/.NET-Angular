namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05112016_UserChanges_RemoveRequiredConstraint1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "UserPassword", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "UserPassword", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
