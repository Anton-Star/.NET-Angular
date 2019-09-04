namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03142016_AuditLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLog",
                c => new
                    {
                        AuditLogId = c.Int(nullable: false, identity: true),
                        AuditLogUserName = c.String(),
                        AuditLogIPAddress = c.String(),
                        AuditLogDateTime = c.DateTimeOffset(nullable: false, precision: 7),
                        AuditLogDescription = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AuditLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuditLog");
        }
    }
}
