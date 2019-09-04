namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AssociationType5516 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationResponseComment", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationResponseComment", "QuestionId", "dbo.ApplicationSectionQuestion");
            DropIndex("dbo.ApplicationResponseComment", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationResponseComment", new[] { "QuestionId" });
            CreateTable(
                "dbo.DocumentAssociationType",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    AssociationTypeId = c.Guid(nullable: false),
                    DocumentId = c.Guid(nullable: false),
                    CreatedBy = c.String(nullable: false, maxLength: 100),
                    CreatedDate = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 100),
                    UpdatedDate = c.DateTime(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssociationType", t => t.AssociationTypeId)
                .ForeignKey("dbo.Document", t => t.DocumentId)
                .Index(t => t.AssociationTypeId)
                .Index(t => t.DocumentId);

            CreateTable(
                "dbo.AssociationType",
                c => new
                {
                    AssociationTypeId = c.Guid(nullable: false),
                    AssociationTypeName = c.String(),
                    CreatedBy = c.String(nullable: false, maxLength: 100),
                    CreatedDate = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 100),
                    UpdatedDate = c.DateTime(),
                })
                .PrimaryKey(t => t.AssociationTypeId);

            //AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseId", c => c.Int(nullable: false));
            AddColumn("dbo.Document", "ApplicationId", c => c.Int());
            //AddColumn("dbo.ApplicationResponse", "ApplicationResponseRFICreatedBy", c => c.String());
            //AddColumn("dbo.ApplicationResponse", "ApplicationResponseRFIComments", c => c.String());
            //AddColumn("dbo.ApplicationResponse", "ApplicationResponseCitationComments", c => c.String());
            //CreateIndex("dbo.ApplicationResponseComment", "ApplicationResponseId");
            CreateIndex("dbo.Document", "ApplicationId");
            //AddForeignKey("dbo.ApplicationResponseComment", "ApplicationResponseId", "dbo.ApplicationResponse", "ApplicationResponseId");
            AddForeignKey("dbo.Document", "ApplicationId", "dbo.Application", "ApplicationId");
            //DropColumn("dbo.ApplicationResponseComment", "ApplicationId");
            //DropColumn("dbo.ApplicationResponseComment", "QuestionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationResponseComment", "QuestionId", c => c.Guid());
            AddColumn("dbo.ApplicationResponseComment", "ApplicationId", c => c.Int());
            DropForeignKey("dbo.DocumentAssociationType", "DocumentId", "dbo.Document");
            DropForeignKey("dbo.DocumentAssociationType", "AssociationTypeId", "dbo.AssociationType");
            DropForeignKey("dbo.Document", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationResponseComment", "ApplicationResponseId", "dbo.ApplicationResponse");
            DropIndex("dbo.DocumentAssociationType", new[] { "DocumentId" });
            DropIndex("dbo.DocumentAssociationType", new[] { "AssociationTypeId" });
            DropIndex("dbo.Document", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationResponseComment", new[] { "ApplicationResponseId" });
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseCitationComments");
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseRFIComments");
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseRFICreatedBy");
            DropColumn("dbo.Document", "ApplicationId");
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseId");
            DropTable("dbo.AssociationType");
            DropTable("dbo.DocumentAssociationType");
            CreateIndex("dbo.ApplicationResponseComment", "QuestionId");
            CreateIndex("dbo.ApplicationResponseComment", "ApplicationId");
            AddForeignKey("dbo.ApplicationResponseComment", "QuestionId", "dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionId");
            AddForeignKey("dbo.ApplicationResponseComment", "ApplicationId", "dbo.Application", "ApplicationId");
        }
    }
}
