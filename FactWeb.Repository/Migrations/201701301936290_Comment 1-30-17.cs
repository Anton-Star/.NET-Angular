namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comment13017 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentIncludeInReporting", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentIncludeInReporting");
        }
    }
}
