namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _040620161 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facility", "FacilityIsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Facility", "FacilityNumber", c => c.Guid());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Facility", "FacilityNumber", c => c.String());
        }
    }
}
