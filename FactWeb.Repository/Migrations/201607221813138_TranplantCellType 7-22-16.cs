namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranplantCellType72216 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransplantCellType",
                c => new
                    {
                        TransplantCellTypeId = c.Guid(nullable: false),
                        TransplantCellTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransplantCellTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransplantCellType");
        }
    }
}
