namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UserOrganization81616 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.User", "Organization_Id", "dbo.Organization");
            DropIndex("dbo.User", new[] { "OrganizationId" });
            DropIndex("dbo.User", new[] { "Organization_Id" });
            CreateTable(
                "dbo.UserOrganization",
                c => new
                    {
                        UserOrganizationId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        JobFunctionId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserOrganizationId)
                .ForeignKey("dbo.JobFunction", t => t.JobFunctionId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.OrganizationId)
                .Index(t => t.JobFunctionId);
            
            DropColumn("dbo.User", "OrganizationId");
            DropColumn("dbo.User", "Organization_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Organization_Id", c => c.Int());
            AddColumn("dbo.User", "OrganizationId", c => c.Int());
            DropForeignKey("dbo.UserOrganization", "UserId", "dbo.User");
            DropForeignKey("dbo.UserOrganization", "JobFunctionId", "dbo.JobFunction");
            DropIndex("dbo.UserOrganization", new[] { "JobFunctionId" });
            DropIndex("dbo.UserOrganization", new[] { "OrganizationId" });
            DropIndex("dbo.UserOrganization", new[] { "UserId" });
            DropTable("dbo.UserOrganization");
            CreateIndex("dbo.User", "Organization_Id");
            CreateIndex("dbo.User", "OrganizationId");
            AddForeignKey("dbo.User", "OrganizationId", "dbo.Organization", "OrganizationId");
        }
    }
}
