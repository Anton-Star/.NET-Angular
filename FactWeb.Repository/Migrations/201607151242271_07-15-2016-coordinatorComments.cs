namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07152016coordinatorComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseCoorindatorComment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseCoorindatorComment");
        }
    }
}
