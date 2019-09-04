namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _03142016_Site_Changes_367 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionComplianceNumber");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionComplianceNumber", c => c.Int());
        }
    }
}
