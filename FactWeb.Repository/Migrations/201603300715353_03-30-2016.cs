namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03302016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationSectionScopeType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationSectionId = c.Guid(nullable: false),
                        ScopeTypeId = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        IsActual = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationSection", t => t.ApplicationSectionId)
                .ForeignKey("dbo.ScopeType", t => t.ScopeTypeId)
                .Index(t => t.ApplicationSectionId)
                .Index(t => t.ScopeTypeId);
            
            DropColumn("dbo.ApplicationSection", "ApplicationSectionUniqueIdentifier");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationSection", "ApplicationSectionUniqueIdentifier", c => c.String());
            DropForeignKey("dbo.ApplicationSectionScopeType", "ScopeTypeId", "dbo.ScopeType");
            DropForeignKey("dbo.ApplicationSectionScopeType", "ApplicationSectionId", "dbo.ApplicationSection");
            DropIndex("dbo.ApplicationSectionScopeType", new[] { "ScopeTypeId" });
            DropIndex("dbo.ApplicationSectionScopeType", new[] { "ApplicationSectionId" });
            DropTable("dbo.ApplicationSectionScopeType");
        }
    }
}
