namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationHistory_415 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationStatusHistory",
                c => new
                    {
                        ApplicationStatusHistoryId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        ApplicationStatusIdOld = c.Int(nullable: false),
                        ApplicationStatusIdNew = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationStatusHistoryId)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.ApplicationStatus", t => t.ApplicationStatusIdNew)
                .ForeignKey("dbo.ApplicationStatus", t => t.ApplicationStatusIdOld)
                .Index(t => t.ApplicationId)
                .Index(t => t.ApplicationStatusIdOld)
                .Index(t => t.ApplicationStatusIdNew);
            
            AddColumn("dbo.Address", "SiteAddress_Id", c => c.Int());
            CreateIndex("dbo.Address", "SiteAddress_Id");
            AddForeignKey("dbo.Address", "SiteAddress_Id", "dbo.SiteAddress", "SiteAddressId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationStatusHistory", "ApplicationStatusIdOld", "dbo.ApplicationStatus");
            DropForeignKey("dbo.ApplicationStatusHistory", "ApplicationStatusIdNew", "dbo.ApplicationStatus");
            DropForeignKey("dbo.ApplicationStatusHistory", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.Address", "SiteAddress_Id", "dbo.SiteAddress");
            DropIndex("dbo.ApplicationStatusHistory", new[] { "ApplicationStatusIdNew" });
            DropIndex("dbo.ApplicationStatusHistory", new[] { "ApplicationStatusIdOld" });
            DropIndex("dbo.ApplicationStatusHistory", new[] { "ApplicationId" });
            DropIndex("dbo.Address", new[] { "SiteAddress_Id" });
            DropColumn("dbo.Address", "SiteAddress_Id");
            DropTable("dbo.ApplicationStatusHistory");
        }
    }
}
