namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppLog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationResponse", "ApplicationResponseStatusId", "dbo.ApplicationResponseStatus");
            DropIndex("dbo.ApplicationResponse", new[] { "ApplicationResponseStatusId" });
            CreateTable(
                "dbo.AppLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Thread = c.String(nullable: false, maxLength: 255),
                        Level1 = c.String(nullable: false, maxLength: 50),
                        Logger = c.String(nullable: false, maxLength: 255),
                        Message = c.String(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseStatusId");
            DropTable("dbo.ApplicationResponseStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationResponseStatus",
                c => new
                    {
                        ApplicationResponseStatus = c.Int(nullable: false, identity: true),
                        ApplicationResponseStatusName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationResponseStatus);
            
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseStatusId", c => c.Int());
            DropTable("dbo.AppLog");
            CreateIndex("dbo.ApplicationResponse", "ApplicationResponseStatusId");
            AddForeignKey("dbo.ApplicationResponse", "ApplicationResponseStatusId", "dbo.ApplicationResponseStatus", "ApplicationResponseStatus");
        }
    }
}
