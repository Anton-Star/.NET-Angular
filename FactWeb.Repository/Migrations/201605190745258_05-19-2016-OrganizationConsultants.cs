namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05192016OrganizationConsultants : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationConsutant",
                c => new
                    {
                        OrganizationConsutantId = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        ConsultantId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationConsutantId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.User", t => t.ConsultantId)
                .Index(t => t.OrganizationId)
                .Index(t => t.ConsultantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationConsutant", "ConsultantId", "dbo.User");
            DropForeignKey("dbo.OrganizationConsutant", "OrganizationId", "dbo.Organization");
            DropIndex("dbo.OrganizationConsutant", new[] { "ConsultantId" });
            DropIndex("dbo.OrganizationConsutant", new[] { "OrganizationId" });
            DropTable("dbo.OrganizationConsutant");
        }
    }
}
