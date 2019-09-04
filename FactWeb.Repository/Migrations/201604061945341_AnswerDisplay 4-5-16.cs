namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AnswerDisplay4516 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facility", "FacilityIsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Facility", "FacilityNumber", c => c.Guid());

            CreateTable(
                "dbo.ApplicationSectionQuestionAnswerDisplay",
                c => new
                    {
                        ApplicationSectionQuestionAnswerDisplayId = c.Guid(nullable: false),
                        ApplicationSectionQuestionAnswerId = c.Guid(nullable: false),
                        HidesQuestionId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationSectionQuestionAnswerDisplayId)
                .ForeignKey("dbo.ApplicationSectionQuestion", t => t.HidesQuestionId)
                .ForeignKey("dbo.ApplicationSectionQuestionAnswer", t => t.ApplicationSectionQuestionAnswerId)
                .Index(t => t.ApplicationSectionQuestionAnswerId)
                .Index(t => t.HidesQuestionId);
            
            AlterColumn("dbo.Facility", "FacilityNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationSectionQuestionAnswerDisplay", "ApplicationSectionQuestionAnswerId", "dbo.ApplicationSectionQuestionAnswer");
            DropForeignKey("dbo.ApplicationSectionQuestionAnswerDisplay", "HidesQuestionId", "dbo.ApplicationSectionQuestion");
            DropIndex("dbo.ApplicationSectionQuestionAnswerDisplay", new[] { "HidesQuestionId" });
            DropIndex("dbo.ApplicationSectionQuestionAnswerDisplay", new[] { "ApplicationSectionQuestionAnswerId" });
            AlterColumn("dbo.Facility", "FacilityNumber", c => c.Guid());
            DropTable("dbo.ApplicationSectionQuestionAnswerDisplay");
        }
    }
}
