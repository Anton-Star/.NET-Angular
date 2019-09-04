namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Workflow52016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workflow",
                c => new
                    {
                        WorkflowId = c.Guid(nullable: false),
                        WorkflowName = c.String(),
                        WorkflowActivityName = c.String(),
                        K2SerialNumber = c.String(),
                        K2ProcessInstanceId = c.Int(nullable: false),
                        WorkflowActionName = c.String(),
                        AssignedToUserId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.WorkflowId)
                .ForeignKey("dbo.User", t => t.AssignedToUserId)
                .Index(t => t.AssignedToUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Workflow", "AssignedToUserId", "dbo.User");
            DropIndex("dbo.Workflow", new[] { "AssignedToUserId" });
            DropTable("dbo.Workflow");
        }
    }
}
