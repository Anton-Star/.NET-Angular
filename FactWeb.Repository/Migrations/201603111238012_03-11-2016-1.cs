namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _031120161 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseRFICreatedBy", c => c.String());
            //DropColumn("dbo.ApplicationResponse", "RFICreatedBy");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.ApplicationResponse", "RFICreatedBy", c => c.String());
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseRFICreatedBy");
        }
    }
}
