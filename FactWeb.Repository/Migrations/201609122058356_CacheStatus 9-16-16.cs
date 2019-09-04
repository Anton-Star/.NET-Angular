namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CacheStatus91616 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CacheStatus",
                c => new
                    {
                        CacheStatusObjectName = c.String(nullable: false, maxLength: 128),
                        CacheStatusLastChangeDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CacheStatusObjectName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CacheStatus");
        }
    }
}
