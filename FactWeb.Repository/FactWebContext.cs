using FactWeb.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FactWeb.Repository
{
    public class FactWebContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<OrganizationAddress> OrganizationAddresses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<ApplicationResponseStatus> ApplicationResponseStatuses { get; set; }        
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<SiteAddress> SiteAddresses { get; set; }
        public DbSet<FacilityAddress> FacilityAddresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Credential> Credential { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserCredential> UserCredential { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<ApplicationStatusHistory> ApplicationStatusHistory { get; set; }
        public DbSet<ApplicationTypeCategory> ApplicationTypeCategories { get; set; }
        public DbSet<AccreditationRole> AccreditationRoles { get; set; }
        public DbSet<InspectionSchedule> InspectionSchedules { get; set; }
        public DbSet<InspectionCategory> InspectionCategories { get; set; }
        public DbSet<InspectionScheduleDetail> InspectionScheduleDetails { get; set; }
        public DbSet<InspectionScheduleSite> InspectionScheduleSites { get; set; }
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public DbSet<ScopeType> ScopeTypes { get; set; }
        public DbSet<ComplianceApplication> ComplianceApplications { get; set; }
        public DbSet<ApplicationSection> ApplicationSections { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<ApplicationSectionQuestion> ApplicationSectionQuestions { get; set; }
        public DbSet<ApplicationSectionQuestionAnswer> ApplicationSectionQuestionAnswers { get; set; }
        public DbSet<ApplicationSectionQuestionAnswerDisplay> ApplicationSectionQuestionAnswerDisplays { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<Distance> Distances { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<OrganizationDocument> OrganizationDocuments { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<JobFunction> JobFunctions { get; set; }
        public DbSet<UserJobFunction> UserJobFunctions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<UserLanguage> UserLanguages { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<UserMembership> UserMemberships { get; set; }
        public DbSet<ApplicationResponse> ApplicationResponses { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<ClinicalType> ClinicalType { get; set; }
        public DbSet<ProcessingType> ProcessingType { get; set; }
        public DbSet<CollectionProductType> CollectionProductType { get; set; }
        public DbSet<CBCollectionType> CBCollectionType { get; set; }
        public DbSet<ClinicalPopulationType> ClinicalPopulationType { get; set; }
        public DbSet<TransplantType> TransplantType { get; set; }
        public DbSet<CBUnitType> CBUnitType { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<FacilitySite> FacilitySite { get; set; }
        public DbSet<SiteScopeType> SiteScopeType { get; set; }
        public DbSet<SiteTransplantType> SiteTransplantType { get; set; }
        public DbSet<SiteClinicalType> SiteClinicalType { get; set; }
        public DbSet<ApplicationSectionScopeType> ApplicationSectionScopeTypes { get; set; }        
        public DbSet<ApplicationSectionQuestionScopeType> ApplicationSectionQuestionScopeTypes { get; set; }
        public DbSet<MasterServiceType> MasterServiceTypes { get; set; }
        public DbSet<FacilityAccreditation> FacilityAccreditations { get; set; }
        public DbSet<ApplicationVersion> ApplicationVersions { get; set; }
        public DbSet<AccreditationStatus> AccreditationStatus { get; set; }
        public DbSet<BAAOwner> BAAOwner { get; set; }
        public DbSet<ComplianceApplicationApprovalStatus> ApplicationVersionStatuses { get; set; }
        public DbSet<Template> Templates { get; set; }
        //public DbSet<SiteApplicationVersion> SiteApplicationVersions { get; set; }
        public DbSet<ApplicationResponseComment> ApplicationResponseComments { get; set; }        
        public DbSet<DocumentAssociationType> DocumentAssociationTypes { get; set; }
        public DbSet<AssociationType> AssociationTypes { get; set; }
        public DbSet<FacilityAccreditationMapping> FacilityAccreditationMapping { get; set; }
        //public DbSet<SiteApplicationVersionQuestionNotApplicable> SiteApplicationVersionQuestionNotApplicables { get; set; }
        public DbSet<ApplicationQuestionNotApplicable> ApplicationQuestionNotApplicables { get; set; }
        public DbSet<OrganizationConsutant> OrganizationConsutants { get; set; }
        public DbSet<OutcomeStatus> OutcomeStatus { get; set; }
        public DbSet<ReportReviewStatus> ReportReviewStatus { get; set; }
        public DbSet<AccreditationOutcome> AccreditationOutcome { get; set; }
        public DbSet<ApplicationResponseTrainee> ApplicationResponseTrainee { get; set; }
        public DbSet<Inspection> Inspection { get; set; }
        public DbSet<CommentType> CommentType { get; set; }
        public DbSet<OrganizationAccreditationCycle> OrganizationAccreditationCycles { get; set; }
        public DbSet<CBCategory> CbCategories { get; set; }
        public DbSet<TransplantCellType> TransplantCellTypes { get; set; }
        public DbSet<SiteTransplantTotal> SiteTransplantTotals { get; set; }
        public DbSet<CollectionType> CollectionTypes { get; set; }
        public DbSet<SiteCollectionTotal> SiteCollectionTotals { get; set; }
        public DbSet<SiteProcessingMethodologyTotal> SiteProcessingMethodologyTotals { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<SiteProcessingTotalTransplantCellType> SiteProcessingTotalTransplantCellTypes { get; set; }
        public DbSet<UserOrganization> UserOrganizations { get; set; }
        public DbSet<CacheStatus> CacheStatuses { get; set; }
        public DbSet<NetcordMembershipType> NetcordMembershipTypes { get; set; }
        //public DbSet<OrganizationNetcordMembership> OrganizationNetcordMemberships { get; set; }
        public DbSet<AccessRequest> AccessRequests { get; set; }
        public DbSet<OrganizationBAADocument> OrganizationBAADocuments { get; set; }
        public DbSet<OrganizationFacility> OrganizationFacilities { get; set; }
        public DbSet<FacilityCibmtr> FacilityCibmtrs { get; set; }
        public DbSet<FacilityCibmtrOutcomeAnalysis> FacilityCibmtrOutcomeAnalyses { get; set; }
        public DbSet<FacilityCibmtrDataManagement> FacilityCibmtrDataManagements { get; set; }
        public DbSet<CpiType> CpiTypes { get; set; }

        public DbSet<ApplicationVersionCache> ApplicationVersionCaches { get; set; }
        public DbSet<ComplianceApplicationSubmitApproval> ComplianceApplicationSubmitApprovals { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<ApplicationSubmitApproval> ApplicationSubmitApprovals { get; set; }
        public DbSet<ComplianceApplicationInspectionDetail> ComplianceApplicationInspectionDetails { get; set; }

        public new Database Database
        {
            get
            {
                return base.Database;
            }
        }

        public FactWebContext() : base("name=FactWeb.Repository.FactWebContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null) return;

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public IDbSet<T> SetEntity<T>() where T : class
        {
            return Set<T>();
        }

        public new void SaveChanges()
        {

            base.SaveChanges();
        }

        public void SetModified<T>(T item) where T : class
        {
            if (Entry(item).State == EntityState.Detached)
            {
                SetEntity<T>().Attach(item);
            }

            Entry(item).State = EntityState.Modified;
        }

        public void RollbackChange<T>(T item) where T : class
        {
            Entry(item).State = EntityState.Unchanged;
        }

        public void SeedUnitTestData()
        {
            Database.ExecuteSqlCommand("exec SeedUnittestData");
        }
    }
}
