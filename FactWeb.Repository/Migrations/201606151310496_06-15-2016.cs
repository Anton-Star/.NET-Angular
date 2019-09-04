namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06152016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationResponseTrainee",
                c => new
                    {
                        ApplicationResponseTraineeId = c.Int(nullable: false, identity: true),
                        ApplicationResponseId = c.Int(nullable: false),
                        ApplicationResponseStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationResponseTraineeId)
                .ForeignKey("dbo.ApplicationResponse", t => t.ApplicationResponseId)
                .ForeignKey("dbo.ApplicationResponseStatus", t => t.ApplicationResponseStatusId)
                .Index(t => t.ApplicationResponseId)
                .Index(t => t.ApplicationResponseStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationResponseTrainee", "ApplicationResponseStatusId", "dbo.ApplicationResponseStatus");
            DropForeignKey("dbo.ApplicationResponseTrainee", "ApplicationResponseId", "dbo.ApplicationResponse");
            DropIndex("dbo.ApplicationResponseTrainee", new[] { "ApplicationResponseStatusId" });
            DropIndex("dbo.ApplicationResponseTrainee", new[] { "ApplicationResponseId" });
            DropTable("dbo.ApplicationResponseTrainee");
        }
    }
}
