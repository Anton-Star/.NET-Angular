namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06272016orgConsultants : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrganizationConsutant", "OrganizationConsutantStartDate", c => c.DateTime());
            AddColumn("dbo.OrganizationConsutant", "OrganizationConsutantEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrganizationConsutant", "OrganizationConsutantEndDate");
            DropColumn("dbo.OrganizationConsutant", "OrganizationConsutantStartDate");
        }
    }
}
