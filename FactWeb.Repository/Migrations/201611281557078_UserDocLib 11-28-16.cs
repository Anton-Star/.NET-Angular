namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UserDocLib112816 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserDocumentLibraryUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserDocumentLibraryUserId");
        }
    }
}
