namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentVisibleToApplicant", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentVisibleToApplicant");
        }
    }
}
