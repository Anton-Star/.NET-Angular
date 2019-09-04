namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05052016_511_ApplicationResponse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponseStatus", "ApplicationResponseStatusNameForApplicant", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationResponseStatus", "ApplicationResponseStatusNameForApplicant");
        }
    }
}
