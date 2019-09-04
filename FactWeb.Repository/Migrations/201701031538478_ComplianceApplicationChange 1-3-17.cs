namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ComplianceApplicationChange1317 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplianceApplication", "ComplianceApplicationShowAccreditationReport", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComplianceApplication", "ComplianceApplicationShowAccreditationReport");
        }
    }
}
