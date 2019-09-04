namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FactWeb.Repository.FactWebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FactWeb.Repository.FactWebContext context)
        {
            //context.Roles.AddOrUpdate(
            //    x => x.Name,
            //    new Role { Name = Constants.Roles.FACTAdministrator, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.PrimaryContact, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.User, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.QualityManager, CreatedBy = "irfanm", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.FACTConsultantCoordinator, CreatedBy = "irfanm", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.OrganizationalDirector, CreatedBy = "irfanm", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.FACTConsultant, CreatedBy = "kamranu", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.NonSystemUser, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.Inspector, CreatedBy = "kamranu", CreatedDate = DateTime.Now },
            //    new Role { Name = Constants.Roles.FACTCoordinator, CreatedBy = "irfanm", CreatedDate = DateTime.Now });

            //context.ApplicationTypes.AddOrUpdate(
            //    x => x.Name,
            //    new ApplicationType { Name = Constants.ApplicationTypes.CT, IsManageable = true, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new ApplicationType { Name = Constants.ApplicationTypes.CordBlood, IsManageable = true, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new ApplicationType { Name = Constants.ApplicationTypes.Eligibility, CreatedBy = "nick", CreatedDate = DateTime.Now });

            //context.AddressTypes.AddOrUpdate(
            //    x => x.Name,
            //    new AddressType { Name = Constants.AddressTypes.Office, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new AddressType { Name = Constants.AddressTypes.Residence, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new AddressType { Name = Constants.AddressTypes.Billing, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new AddressType { Name = Constants.AddressTypes.Mailing, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new AddressType { Name = Constants.AddressTypes.Other, CreatedBy = "nasirw", CreatedDate = DateTime.Now });


            //context.DocumentTypes.AddOrUpdate(
            //   x => x.Name,
            //   new DocumentType { Name = Constants.DocumentTypes.PreInspectionEvidence, CreatedBy = "kamranu", CreatedDate = DateTime.Now },
            //   new DocumentType { Name = Constants.DocumentTypes.PostInspectionEvidence, CreatedBy = "kamranu", CreatedDate = DateTime.Now });


            //context.ApplicationResponseStatuses.AddOrUpdate(
            //   x => x.Name,
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.ForReview, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.Reviewed, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.Compliant, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.NotCompliant, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.NA, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.NoResponseRequested, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.RFI, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.RFICompleted, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.RfiFollowup, CreatedBy = "kamranu@5thmethod.com", CreatedDate = DateTime.Now },
            //   new ApplicationResponseStatus { Name = Constants.ApplicationResponseStatus.New, CreatedBy = "nick", CreatedDate = DateTime.Now });

            //context.QuestionTypes.AddOrUpdate(
            //    x => x.Name,
            //    new QuestionType { Name = Constants.QuestionTypes.TextArea, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.RadioButtons, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.Checkboxes, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.DocumentUpload, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.Date, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.DateRange, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.PeoplePicker, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.Text, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new QuestionType { Name = Constants.QuestionTypes.Multiple, CreatedBy = "nick", CreatedDate = DateTime.Now });

            ////context.ApplicationStatuses.AddOrUpdate(
            ////    x => x.Name,
            ////    new ApplicationStatus { Name = Constants.ApplicationStatus.Pending, CreatedBy = "nick", CreatedDate = DateTime.Now },
            ////    new ApplicationStatus { Name = Constants.ApplicationStatus.Applied, CreatedBy = "nick", CreatedDate = DateTime.Now },
            ////    new ApplicationStatus { Name = Constants.ApplicationStatus.InReview, CreatedBy = "nick", CreatedDate = DateTime.Now },
            ////    new ApplicationStatus { Name = Constants.ApplicationStatus.InspectionScheduled, CreatedBy = "nick", CreatedDate = DateTime.Now },
            ////    new ApplicationStatus { Name = Constants.ApplicationStatus.Approved, CreatedBy = "nick", CreatedDate = DateTime.Now },
            ////    new ApplicationStatus { Name = Constants.ApplicationStatus.Declined, CreatedBy = "nick", CreatedDate = DateTime.Now });

            //context.ApplicationStatuses.AddOrUpdate(
            //    x => x.Name,
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.InProgress, CreatedBy = "kamran", CreatedDate = DateTime.Now , NameForApplicant = Constants.ApplicationStatusForApplicants.InProgress },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.ForReview, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.ApplicationSubmitted },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.RFI, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.RFI },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.RFIReview, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.RFISubmitted},
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.SchedulingInspection, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.ApplicationSubmitted},
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.PreInspection, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.InspectionScheduled },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.OnSiteInspection, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.UnderInspection  },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.PostInspectionReview, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.PendingCommitteeReview },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.AccreditationCommittee, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.PendingCommitteeReview },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.PostCommitteeReview, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.PendingCommitteeReview},
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.ApplicantResponse, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.PostCommitteeRFI },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.ApplicantResponseReview, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.PostCommitteeRFISubmitted },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.Complete, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.Complete },
            //    new ApplicationStatus { Name = Constants.ApplicationStatus.Cancelled, CreatedBy = "kamran", CreatedDate = DateTime.Now, NameForApplicant = Constants.ApplicationStatusForApplicants.Complete });



            //context.JobFunctions.AddOrUpdate(
            //    x => x.Name,
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ApheresisCollectionFacilityDirector, Order = 0, IsActive = true, ReportingOrder = 3, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ApheresisCollectionFacilityMedicalDirector, Order = 0, IsActive = true, ReportingOrder = 4, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.BmtAdministrator, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.BoneMarrowCollectionFacilityDirector, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.BoneMarrowCollectionFacilityMedicalDirector, Order = 0, IsActive = true, ReportingOrder = 2, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ClinicalProgramDirector, Order = 0, IsActive = true, ReportingOrder = 1, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ClinicalTransplantCoordinator, Order = 0, ReportingOrder = 0, IncludeInReporting = false, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CollectionFacilitySupervisor, Order = 0, ReportingOrder = 0, IncludeInReporting = false, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CollectionNurse, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ConsultingPhysician, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CordBloodBankDirector, Order = 0, IsActive = true, ReportingOrder = 11, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CordBloodBankMedicalDirector, Order = 0, IsActive = true, ReportingOrder = 12, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CordBloodBankPersonnel, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CordBloodCollectionSiteDirector, Order = 0, IsActive = true, ReportingOrder = 13, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CordBloodProcessingFacilityDirector, Order = 0, IsActive = true, ReportingOrder = 14, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.DataManager, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.FactLiaison, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.MedicalTechnologist, Order = 0, ReportingOrder = 0, IncludeInReporting = false, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.Other, Order = 1, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ProcessingFacilityDirector, Order = 0, IsActive = true, ReportingOrder = 5, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ProcessingFacilityMedicalDirector, Order = 0, IsActive = true, ReportingOrder = 6, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ProcessingFacilitySupervisor, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.QmManager, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.QmSupervisor, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.RegistryPersonnel, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.TransplantNurse, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.TransplantPhysician, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = false, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ClinicalQualityManagementSupervisor, Order = 0, IsActive = true, ReportingOrder = 7, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.MarrowQualityManagementSupervisor, Order = 0, IsActive = true, ReportingOrder = 8, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ApheresisQualityManagementSupervisor, Order = 0, IsActive = true, ReportingOrder = 9, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.ProcessingQualityManagementSupervisor, Order = 0, IsActive = true, ReportingOrder = 10, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.CordBloodQualityManagementSupervisor, Order = 0, IsActive = true, ReportingOrder = 15, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.OrganizationDirector, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "irfanm" },
            //    new JobFunction { Id = Guid.NewGuid(), Name = Constants.JobFunctions.OrganizationAdmin, Order = 0, IsActive = true, ReportingOrder = 0, IncludeInReporting = true, CreatedDate = DateTime.Now, CreatedBy = "irfanm" }
            //    );

            //context.Languages.AddOrUpdate(
            //    x => x.Name,
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.English, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Arabic, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Armenian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Azerbaijani, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Bulgarian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Catalan, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Chinese, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Croatian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Czech, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Danish, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Estonian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Filipino, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Finnish, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.French, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.German, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Greek, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Hebrew, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Hindi, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Hungarian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Japanese, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Icelandic, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Indonesian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Irish, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Italian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Korean, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Latvian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Lithuanian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Macedonian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Norwegian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Persian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Polish, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Portuguese, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Romanian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Russian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Serbian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Slovenian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Slovak, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Spanish, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Swedish, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Thai, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Turkish, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Ukrainian, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Vietnamese, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Language { Id = Guid.NewGuid(), Name = Constants.Languages.Welsh, Order = 2, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" });

            //context.Memberships.AddOrUpdate(
            //    x => x.Name,
            //    new Membership { Id = Guid.NewGuid(), Name = Constants.Memberships.Asbmt, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Membership { Id = Guid.NewGuid(), Name = Constants.Memberships.Asfa, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Membership { Id = Guid.NewGuid(), Name = Constants.Memberships.Ebmt, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Membership { Id = Guid.NewGuid(), Name = Constants.Memberships.Isct, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Membership { Id = Guid.NewGuid(), Name = Constants.Memberships.IsctEurope, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Membership { Id = Guid.NewGuid(), Name = Constants.Memberships.Jacie, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new Membership { Id = Guid.NewGuid(), Name = Constants.Memberships.NetCord, Order = 1, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "nick" });

            //context.MasterServiceTypes.AddOrUpdate(
            //x => x.Name,
            //new MasterServiceType { Name = Constants.MasterServiceType.CellularTherapy, ShortName = "CT", CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new MasterServiceType { Name = Constants.MasterServiceType.CordBlood, ShortName = "CB", CreatedBy = "kamran", CreatedDate = DateTime.Now });

            //context.FacilityAccreditations.AddOrUpdate(
            //x => x.Name,
            //new FacilityAccreditation { Name = Constants.FacilityAccreditations.AABB, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new FacilityAccreditation { Name = Constants.FacilityAccreditations.CAP, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new FacilityAccreditation { Name = Constants.FacilityAccreditations.JCAHO, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new FacilityAccreditation { Name = Constants.FacilityAccreditations.ASHI, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new FacilityAccreditation { Name = Constants.FacilityAccreditations.EFI, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new FacilityAccreditation { Name = Constants.FacilityAccreditations.Other, CreatedBy = "kamran", CreatedDate = DateTime.Now });

            //context.AccreditationStatus.AddOrUpdate(
            //x => x.Name,
            //new AccreditationStatus { Name = Constants.AccreditationsStatus.Accredited, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //new AccreditationStatus { Name = Constants.AccreditationsStatus.Pending, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //new AccreditationStatus { Name = Constants.AccreditationsStatus.Suspended, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //new AccreditationStatus { Name = Constants.AccreditationsStatus.Expired, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //new AccreditationStatus { Name = Constants.AccreditationsStatus.Withdrawn, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //new AccreditationStatus { Name = Constants.AccreditationsStatus.GracePeriod, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.AccreditationRoles.AddOrUpdate(
            //x => x.Name,
            //new AccreditationRole { Name = Constants.AccreditationsRoles.Inspectors, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new AccreditationRole { Name = Constants.AccreditationsRoles.Auditors, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new AccreditationRole { Name = Constants.AccreditationsRoles.InspectorTraineesAHO, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new AccreditationRole { Name = Constants.AccreditationsRoles.Observers, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new AccreditationRole { Name = Constants.AccreditationsRoles.PrimaryReviewer, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            //new AccreditationRole { Name = Constants.AccreditationsRoles.InspectorTrainees, CreatedBy = "kamran", CreatedDate = DateTime.Now });

            //context.ApplicationVersionStatuses.AddOrUpdate(
            //    x => x.Name,
            //    new ComplianceApplicationApprovalStatus { Id = Guid.NewGuid(), Name = Constants.ComplianceApplicationApprovalStatuses.Planning, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new ComplianceApplicationApprovalStatus { Id = Guid.NewGuid(), Name = Constants.ComplianceApplicationApprovalStatuses.PendingApproval, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new ComplianceApplicationApprovalStatus { Id = Guid.NewGuid(), Name = Constants.ComplianceApplicationApprovalStatuses.Approved, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new ComplianceApplicationApprovalStatus { Id = Guid.NewGuid(), Name = Constants.ComplianceApplicationApprovalStatuses.Submitted, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new ComplianceApplicationApprovalStatus { Id = Guid.NewGuid(), Name = Constants.ComplianceApplicationApprovalStatuses.Closed, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new ComplianceApplicationApprovalStatus { Id = Guid.NewGuid(), Name = Constants.ComplianceApplicationApprovalStatuses.Reject, CreatedBy = "kamran", CreatedDate = DateTime.Now });

            //context.AssociationTypes.AddOrUpdate(
            //    x => x.Name,
            //    new AssociationType { Id = Guid.NewGuid(), Name = Constants.AssociationTypes.Evidence, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new AssociationType { Id = Guid.NewGuid(), Name = Constants.AssociationTypes.RfiResponse, CreatedBy = "nick", CreatedDate = DateTime.Now }
            //    );

            //context.Credential.AddOrUpdate(
            //    x => x.Name,
            //    new Credential { Name = Constants.Credentials.MD, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.PhD, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.MSN, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.MBA, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.MS, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.MA, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.APRN, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.RN, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.LPN, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.MT, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.MLS, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.MSc, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.BSc, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.BS, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Credential { Name = Constants.Credentials.BA, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.OutcomeStatus.AddOrUpdate(
            //    x => x.Name,
            //    new OutcomeStatus { Name = Constants.OutcomeStatus.Level1, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new OutcomeStatus { Name = Constants.OutcomeStatus.Level2, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new OutcomeStatus { Name = Constants.OutcomeStatus.Level3, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new OutcomeStatus { Name = Constants.OutcomeStatus.Level4, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new OutcomeStatus { Name = Constants.OutcomeStatus.Level5, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new OutcomeStatus { Name = Constants.OutcomeStatus.Level6, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.ReportReviewStatus.AddOrUpdate(
            //    x => x.Name,
            //    new ReportReviewStatus { Name = Constants.ReportReviewStatus.Initial, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ReportReviewStatus { Name = Constants.ReportReviewStatus.Renewal, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ReportReviewStatus { Name = Constants.ReportReviewStatus.Response, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ReportReviewStatus { Name = Constants.ReportReviewStatus.ReInspection, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ReportReviewStatus { Name = Constants.ReportReviewStatus.AddOn, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ReportReviewStatus { Name = Constants.ReportReviewStatus.Reapplication, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ReportReviewStatus { Name = Constants.ReportReviewStatus.Appeal, CreatedBy = "nasirw", CreatedDate = DateTime.Now });


            //context.CommentType.AddOrUpdate(
            //   x => x.Name,
            //   new CommentType { Name = Constants.CommentTypes.RFI, CreatedBy = "kamranu", CreatedDate = DateTime.Now },
            //   new CommentType { Name = Constants.CommentTypes.Citation, CreatedBy = "kamranu", CreatedDate = DateTime.Now },
            //   new CommentType { Name = Constants.CommentTypes.Suggestion, CreatedBy = "kamranu", CreatedDate = DateTime.Now },
            //   new CommentType { Name = Constants.CommentTypes.Coordinator, CreatedBy = "kamranu", CreatedDate = DateTime.Now },
            //   new CommentType { Name = Constants.CommentTypes.FACTOnly, CreatedBy = "imalik", CreatedDate = DateTime.Now });
            

            //context.CBCollectionType.AddOrUpdate(
            //    x => x.Name,
            //    new CBCollectionType { Name = Constants.CBCollectionType.Fixed, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new CBCollectionType { Name = Constants.CBCollectionType.NonFixed, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new CBCollectionType { Name = Constants.CBCollectionType.FixedAndNonFixed, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.ClinicalPopulationType.AddOrUpdate(
            //    x => x.Name,
            //    new ClinicalPopulationType { Name = Constants.ClinicalPopulationType.Adult, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ClinicalPopulationType { Name = Constants.ClinicalPopulationType.Pediatric, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ClinicalPopulationType { Name = Constants.ClinicalPopulationType.AdultandPediatric, CreatedBy = "nasirw", CreatedDate = DateTime.Now });
            //// new ClinicalPopulationType { Name = Constants.ClinicalPopulationType.CTOnly, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.ClinicalType.AddOrUpdate(
            //    x => x.Name,
            //    new ClinicalType { Name = Constants.ClinicalType.InPatient, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ClinicalType { Name = Constants.ClinicalType.OutPatient , CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.CollectionProductType.AddOrUpdate(
            //    x => x.Name,
            //    new CollectionProductType { Name = Constants.CollectionProductType.PeripheralBlood, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new CollectionProductType { Name = Constants.CollectionProductType.Marrow, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new CollectionProductType { Name = Constants.CollectionProductType.PBRIPO, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new CollectionProductType { Name = Constants.CollectionProductType.MRIPO, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.ProcessingType.AddOrUpdate(
            //    x => x.Name,
            //    new ProcessingType { Name = Constants.ProcessingType.Minimal, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ProcessingType { Name = Constants.ProcessingType.MoreThanMinimal, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new ProcessingType { Name = Constants.ProcessingType.OffSiteStorage, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.TransplantType.AddOrUpdate(
            //    x => x.Name,
            //    new TransplantType { Name = Constants.TransplantType.AutologousAdult, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new TransplantType { Name = Constants.TransplantType.AllogeneicAdult, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new TransplantType { Name = Constants.TransplantType.AutologousPediatric, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new TransplantType { Name = Constants.TransplantType.AllogeneicPediatric, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.CBUnitType.AddOrUpdate(
            //    x => x.Name,
            //    new CBUnitType { Name = Constants.CBUnitType.Related, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new CBUnitType { Name = Constants.CBUnitType.Unrelated, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new CBUnitType { Name = Constants.CBUnitType.RelatedAndUnrelated, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.Country.AddOrUpdate(
            //    x => x.Name,
            //    new Country { Name = Constants.Countries.UnitedStatesOfAmerica, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Afghanistan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Albania, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Algeria, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Andorra, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Angola, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.AntiguaAndBarbuda, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Argentina, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Armenia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Australia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Austria, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Azerbaijan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Bahamas, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Bahrain, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Bangladesh, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Barbados, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Belarus, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Belgium, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Belize, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Benin, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Bhutan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Bolivia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.BosniaAndHerzegovina, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Botswana, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Brazil, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Brunei, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Bulgaria, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.BurkinaFaso, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Burundi, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.CaboVerde, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Cambodia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Cameroon, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Canada, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.CentralAfricanRepublic, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Chad, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Chile, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.China, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Colombia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Comoros, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.CongoRepublic, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.CongoDemocraticRepublic, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.CostaRica, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.CotedIvoire, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Croatia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Cuba, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Cyprus, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.CzechRepublic, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Denmark, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Djibouti, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Dominica, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.DominicanRepublic, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Ecuador, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Egypt, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.ElSalvador, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.EquatorialGuinea, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Eritrea, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Estonia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Ethiopia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Fiji, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Finland, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.France, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Gabon, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Gambia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Georgia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Germany, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Ghana, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Greece, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Grenada, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Guatemala, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Guinea, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.GuineaBissau, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Guyana, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Haiti, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Honduras, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Hungary, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Iceland, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.India, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Indonesia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Iran, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Iraq, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Ireland, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Israel, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Italy, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Jamaica, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Japan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Jordan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Kazakhstan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Kenya, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Kiribati, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Kosovo, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Kuwait, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Kyrgyzstan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Laos, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Latvia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Lebanon, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Lesotho, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Liberia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Libya, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Liechtenstein, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Lithuania, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Luxembourg, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Macedonia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Madagascar, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Malawi, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Malaysia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Maldives, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Mali, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Malta, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.MarshallIslands, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Mauritania, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Mauritius, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Mexico, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Micronesia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Moldova, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Monaco, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Mongolia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Montenegro, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Morocco, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Mozambique, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.MyanmarBurma, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Namibia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Nauru, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Nepal, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Netherlands, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.NewZealand, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Nicaragua, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Niger, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Nigeria, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.NorthKorea, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Norway, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Oman, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Pakistan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Palau, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Palestine, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Panama, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.PapuaNewGuinea, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Paraguay, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Peru, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Philippines, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Poland, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Portugal, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Qatar, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Romania, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Russia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Rwanda, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.StKittsAndNevis, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Samoa, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SanMarino, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SaoTomeAndPrincipe, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SaudiArabia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Senegal, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Serbia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Seychelles, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SierraLeone, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Singapore, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Slovakia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Slovenia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SolomonIslands, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Somalia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SouthAfrica, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SouthKorea, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SouthSudan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Spain, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.SriLanka, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Sudan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Suriname, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Swaziland, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Sweden, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Switzerland, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Syria, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Taiwan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Tajikistan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Tanzania, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Thailand, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Togo, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Tonga, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Tunisia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Turkey, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Turkmenistan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Tuvalu, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Uganda, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Ukraine, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.UnitedArabEmirates, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.UnitedKingdom, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Uruguay, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Uzbekistan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Vanuatu, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.VaticanCity, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Venezuela, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Vietnam, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Yemen, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Zambia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new Country { Name = Constants.Countries.Zimbabwe, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.State.AddOrUpdate(
            //    x => x.Name,
            //    new State { Name = Constants.States.Alabama, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Alaska, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Arizona, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Arkansas, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.California, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Colorado, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Connecticut, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Delaware, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Florida, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Georgia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Hawaii, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Idaho, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Illinois, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Indiana, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Iowa, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Kansas, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Kentucky, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Louisiana, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Maine, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Maryland, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Massachusetts, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Michigan, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Minnesota, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Mississippi, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Missouri, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Montana, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Nebraska, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Nevada, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.NewHampshire, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.NewJersey, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.NewMexico, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.NewYork, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.NorthCarolina, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.NorthDakota, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Ohio, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Oklahoma, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Oregon, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Pennsylvania, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Rhode, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.SouthCarolina, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.SouthDakota, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Tennessee, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Texas, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Utah, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Vermont, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Virginia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Washington, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.WestVirginia, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Wisconsin, CreatedBy = "nasirw", CreatedDate = DateTime.Now },
            //    new State { Name = Constants.States.Wyoming, CreatedBy = "nasirw", CreatedDate = DateTime.Now });

            //context.CbCategories.AddOrUpdate(
            //    x=>x.Name,
            //    new CBCategory {Name = Constants.CBCategories.Banked, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new CBCategory { Name = Constants.CBCategories.Released, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new CBCategory { Name = Constants.CBCategories.Outcome, CreatedDate = DateTime.Now, CreatedBy = "nick" });

            //context.TransplantCellTypes.AddOrUpdate(
            //    x=>x.Name,
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.Hpc, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.ApheresisHpc, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.MarrowHpc, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.CordBloodTCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.ApheresisTCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.MarrowTCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.CordBloodImmuneEffectorCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.CardiacCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.IsletCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.AdiposeCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.RenalCells, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new TransplantCellType { Id = Guid.NewGuid(), Name = Constants.TransplantCellTypes.HepaticCells, CreatedDate = DateTime.Now, CreatedBy = "nick" }
            //    );

            //context.CollectionTypes.AddOrUpdate(
            //    x=>x.Name,
            //    new CollectionType { Id = Guid.NewGuid(), Name = Constants.CollectionType.Apheresis, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new CollectionType { Id = Guid.NewGuid(), Name = Constants.CollectionType.Marrow, CreatedDate = DateTime.Now, CreatedBy = "nick" },
            //    new CollectionType { Id = Guid.NewGuid(), Name = Constants.CollectionType.CordBlood, CreatedDate = DateTime.Now, CreatedBy = "nick" });

            //context.BAAOwner.AddOrUpdate(
            //    x => x.Name,
            //    new BAAOwner {Name = Constants.BaaOwners.Fact, CreatedDate = DateTime.Now, CreatedBy = "nick"},
            //    new BAAOwner {Name = Constants.BaaOwners.Applicant, CreatedDate = DateTime.Now, CreatedBy = "nick"});

            //context.CacheStatuses.AddOrUpdate(
            //    x=>x.ObjectName,
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.AppSettings, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now},
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.CbUnitTypes, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.CbCategories, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.TransplantCellTypes, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.ClinicalPopulationTypes, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.TransplantTypes, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.CollectionTypes, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.ProcessingTypes, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.States, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.Countries, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.EmailTemplates, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.ReportReviewStatuses, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.OutcomeStatuses, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.AccreditationStatuses, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.Organizations, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.Facilities, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.Sites, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.ActiveVersions, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now },
            //    new CacheStatus { ObjectName = Constants.CacheStatuses.NetcordMembershipTypes, LastChangeDate = DateTime.Now, CreatedBy = "nick", CreatedDate = DateTime.Now });

            //context.NetcordMembershipTypes.AddOrUpdate(
            //    x => x.Name,
            //    new NetcordMembershipType
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = Constants.NetcordMembershipTypes.Provisional,
            //        CreatedBy = "Nick",
            //        CreatedDate = DateTime.Now
            //    },
            //    new NetcordMembershipType
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = Constants.NetcordMembershipTypes.Associate,
            //        CreatedBy = "Nick",
            //        CreatedDate = DateTime.Now
            //    },
            //    new NetcordMembershipType
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = Constants.NetcordMembershipTypes.Full,
            //        CreatedBy = "Nick",
            //        CreatedDate = DateTime.Now
            //    });
            ////context.ServiceTypes.AddOrUpdate(
            ////x => x.Name,
            ////new ServiceType { Name = Constants.ServiceType.ClinicalProgramCT,FacilityTypeId=1, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            ////new ServiceType { Name = Constants.ServiceType.ProcessingCT,FacilityTypeId=1, CreatedBy = "kamran", CreatedDate = DateTime.Now},
            ////new ServiceType { Name = Constants.ServiceType.ApheresisCollection,FacilityTypeId=1, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            ////new ServiceType { Name = Constants.ServiceType.MarrowCollectionCT,FacilityTypeId=1, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            ////new ServiceType { Name = Constants.ServiceType.CBCollection,FacilityTypeId=2, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            ////new ServiceType { Name = Constants.ServiceType.CBProcessing,FacilityTypeId=2, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            ////new ServiceType { Name = Constants.ServiceType.CBBank,FacilityTypeId=2, CreatedBy = "kamran", CreatedDate = DateTime.Now },
            ////new ServiceType { Name = Constants.ServiceType.CBStorage,FacilityTypeId=2, CreatedBy = "kamran", CreatedDate = DateTime.Now });

            //context.ApplicationSettings.AddOrUpdate(
            //    x=> x.Name,
            //    new Model.ApplicationSetting { Name = Constants.ApplicationSettings.InspectionReminder, Value = "0", SystemName = "InspectionReminder", CreatedDate = DateTime.Now, CreatedBy = "nick"}
            //    );

            //context.EmailTemplates.AddOrUpdate(
            //    x=>x.Name,
            //    new EmailTemplate { Id = Guid.NewGuid(), Name = Constants.EmailTemplates.SendBackForRfi, Html = "<html><head></head><body><p>Thank you for submitting your application. We are in the process of reviewing your submission and need additional information. For details regarding the requested information, access your application within the portal.</p><p>Organization: {Org Name}<br>RFI Due Date: {RFI Due Date}<br><br>Access your application here: {URL}<br><br>After accessing your application, select the Quick View.  All standards that appear in red have a Request for Information (RFI).   Click on the red standard number to view the questions and the RFI comment(s).</p><p>{Custom Notes Here}</p><p>Feel free to contact me if you have any questions.</p><p><span style=\"color: #AD122A\">{CoordinatorName}, {CoordinatorCredentials}</span><br />{CoordinatorTitle}<br /><img src=\"http://fact5mdev.azurewebsites.net/content/images/fact-head.png\" height=\"60\" /><br />{CoordinatorPhoneNumber}<br />{CoordinatorEmailAddress}</p><p>This email has been automatically generated by the FACT Accreditation Portal.</p></body></html>", CreatedDate = DateTime.Now, CreatedBy = "nick" });

        }
    }
}
