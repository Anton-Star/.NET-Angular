namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationVersion452016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationVersion",
                c => new
                    {
                        ApplicationVersionId = c.Guid(nullable: false),
                        ApplicationTypeId = c.Int(nullable: false),
                        ApplicationVersionTitle = c.String(nullable: false, maxLength: 200),
                        ApplicationVersionNumber = c.String(),
                        ApplicationVersionIsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationVersionId)
                .ForeignKey("dbo.ApplicationType", t => t.ApplicationTypeId)
                .Index(t => t.ApplicationTypeId);
            
            AddColumn("dbo.ApplicationSection", "ApplicationVersionId", c => c.Guid());
            CreateIndex("dbo.ApplicationSection", "ApplicationVersionId");
            AddForeignKey("dbo.ApplicationSection", "ApplicationVersionId", "dbo.ApplicationVersion", "ApplicationVersionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationVersion", "ApplicationTypeId", "dbo.ApplicationType");
            DropForeignKey("dbo.ApplicationSection", "ApplicationVersionId", "dbo.ApplicationVersion");
            DropIndex("dbo.ApplicationVersion", new[] { "ApplicationTypeId" });
            DropIndex("dbo.ApplicationSection", new[] { "ApplicationVersionId" });
            DropColumn("dbo.ApplicationSection", "ApplicationVersionId");
            DropTable("dbo.ApplicationVersion");
        }
    }
}
