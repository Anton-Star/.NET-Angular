namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentChange101716 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponseComment", "CommentOverride", c => c.String());
            AddColumn("dbo.ApplicationResponseComment", "OverridenBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponseComment", "OverridenBy");
            DropColumn("dbo.ApplicationResponseComment", "CommentOverride");
        }
    }
}
