namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppSettingChange6316 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSetting", "SettingName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSetting", "SettingName");
        }
    }
}
