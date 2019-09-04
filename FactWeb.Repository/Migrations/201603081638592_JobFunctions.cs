namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobFunctions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserJobFunction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        JobFunctionId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobFunction", t => t.JobFunctionId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.JobFunctionId);
            
            CreateTable(
                "dbo.JobFunction",
                c => new
                    {
                        JobFunctionId = c.Guid(nullable: false),
                        JobFunctionName = c.String(),
                        JobFunctionOrder = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobFunctionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserJobFunction", "UserId", "dbo.User");
            DropForeignKey("dbo.UserJobFunction", "JobFunctionId", "dbo.JobFunction");
            DropIndex("dbo.UserJobFunction", new[] { "JobFunctionId" });
            DropIndex("dbo.UserJobFunction", new[] { "UserId" });
            DropTable("dbo.JobFunction");
            DropTable("dbo.UserJobFunction");
        }
    }
}
