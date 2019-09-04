namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccOutcome_EmailFlag_617 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccreditationOutcome", "AccreditationOutcomeSendEmail", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccreditationOutcome", "AccreditationOutcomeSendEmail");
        }
    }
}
