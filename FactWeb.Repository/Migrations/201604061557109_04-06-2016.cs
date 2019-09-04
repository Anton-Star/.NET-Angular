namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _04062016 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Facility", "FacilityNumber", c => c.String());
            AddColumn("dbo.Facility", "FacilityIsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facility", "FacilityIsActive");
            //DropColumn("dbo.Facility", "FacilityNumber");
        }
    }
}
