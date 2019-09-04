namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05112016_UserChanges_RemoveRequiredConstraint2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "UserPasswordChangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "UserPasswordChangeDate", c => c.DateTime(nullable: false));
        }
    }
}
