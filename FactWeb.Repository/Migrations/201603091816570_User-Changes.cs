namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserWebPhotoPath", c => c.String(maxLength: 100));
            AddColumn("dbo.User", "UserEmailOptOut", c => c.Boolean());
            AddColumn("dbo.User", "UserMailOptOut", c => c.Boolean());
            AddColumn("dbo.User", "UserResumePath", c => c.String(maxLength: 500));
            AddColumn("dbo.User", "UserStatementOfCompliancePath", c => c.String(maxLength: 500));
            AddColumn("dbo.User", "UserAgreedToPolicyDate", c => c.DateTime());
            AddColumn("dbo.User", "UserAnnualProfessionHistoryFormPath", c => c.String(maxLength: 500));
            AddColumn("dbo.User", "UserMedicalLicensePath", c => c.String(maxLength: 500));
            AddColumn("dbo.User", "UserCompletedStep2", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserCompletedStep2");
            DropColumn("dbo.User", "UserMedicalLicensePath");
            DropColumn("dbo.User", "UserAnnualProfessionHistoryFormPath");
            DropColumn("dbo.User", "UserAgreedToPolicyDate");
            DropColumn("dbo.User", "UserStatementOfCompliancePath");
            DropColumn("dbo.User", "UserResumePath");
            DropColumn("dbo.User", "UserMailOptOut");
            DropColumn("dbo.User", "UserEmailOptOut");
            DropColumn("dbo.User", "UserWebPhotoPath");
        }
    }
}
