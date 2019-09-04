namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05112016_UserChanges_RemoveRequiredConstraint : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.User", "ix_user_emailAddress");
            AlterColumn("dbo.User", "UserEmailAddress", c => c.String(maxLength: 100));
            CreateIndex("dbo.User", "UserEmailAddress", unique: true, name: "ix_user_emailAddress");
        }
        
        public override void Down()
        {
            DropIndex("dbo.User", "ix_user_emailAddress");
            AlterColumn("dbo.User", "UserEmailAddress", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.User", "UserEmailAddress", unique: true, name: "ix_user_emailAddress");
        }
    }
}
