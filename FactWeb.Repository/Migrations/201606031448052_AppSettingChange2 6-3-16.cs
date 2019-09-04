namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppSettingChange26316 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSetting", "ApplicationSettingSystemName", c => c.String());
            DropColumn("dbo.ApplicationSetting", "SettingName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationSetting", "SettingName", c => c.String());
            DropColumn("dbo.ApplicationSetting", "ApplicationSettingSystemName");
        }
    }
}
