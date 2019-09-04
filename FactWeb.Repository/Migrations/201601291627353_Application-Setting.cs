namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationSetting",
                c => new
                    {
                        ApplicationSettingId = c.Int(nullable: false, identity: true),
                        ApplicationSettingName = c.String(nullable: false, maxLength: 500),
                        ApplicationSettingValue = c.String(nullable: false, maxLength: 500),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationSettingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApplicationSetting");
        }
    }
}
