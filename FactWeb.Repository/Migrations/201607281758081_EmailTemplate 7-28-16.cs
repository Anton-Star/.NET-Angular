namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailTemplate72816 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailTemplate",
                c => new
                    {
                        EmailTemplateId = c.Guid(nullable: false),
                        EmailTemplateName = c.String(),
                        EmailTemplateHtml = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmailTemplateId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailTemplate");
        }
    }
}
