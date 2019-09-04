namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentCoordinatorComment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentCoordinatorComment");
        }
    }
}
