using System;
using System.Collections.Generic;

namespace FactWeb.Infrastructure
{
    public static class Constants
    {
        public class BaaOwners
        {
            public const string Fact = "FACT";
            public const string Applicant = "Applicant";
        }
        public static class FactRoles
        {
            public static List<string> Names => new List<string>
            {
                Constants.Roles.FACTAdministrator,
                Constants.Roles.QualityManager,
                Constants.Roles.FACTCoordinator
                //Constants.Roles.FACTStaffCoordinator
            };
        }
        public class StorageNames
        {
            public const string FactWeb = "factweb-storage";
            public const string FactWebPhotos = "factweb-photos";
            public const string FactWebResumes = "factweb-resumes";
            public const string FactWebCompliance = "factweb-compliance";
            public const string FactWebHistory = "factweb-professionalhistory";
            public const string FactWebLicense = "factweb-license";
        }

        public class DocumentTypes
        {
            public const string WebPhoto = "webphoto";
            public const string Resume = "resume";
            public const string Compliance = "compliance";
            public const string ProfessionalHistory = "professionalhistory";
            public const string MedicalLicense = "medicallicense";
            public const string PreInspectionEvidence = "Pre-Inspection Evidence";
            public const string PostInspectionEvidence = "Post-Inspection Evidence";
        }

        public class AffiliationType
        {
            public const string Existing = "existing";
            public const string New = "new";
            public const string None = "none";
        }

        public static class Roles
        {
            public const string FACTAdministrator = "FACT Administrator";
            //public const string OrganizationAdmin = "Organization Admin";
            public const string User = "User";
            public const string QualityManager = "FACT Quality Manager";
            public const string FACTCoordinator = "FACT Coordinator";
            public const string FACTConsultantCoordinator = "FACT Consultant Coordinator";
            public const string OrganizationalDirector = "Organizational Director";
            public const string NonSystemUser = "Non-System User";
            public const string Inspector = "Inspector";
            public const string FACTConsultant = "FACT Consultant";
        }

        public enum Role : int
        {
            FACTAdministrator = 1,
            PrimaryContact = 2,
            User = 3,
            QualityManager = 27,
            OrganizationalDirector = 28,
            NonSystemUser = 19,
            FACTConsultant = 30,
            Inspector = 32,
            FACTConsultantCoordinator = 33,
            FACTCoordinator = 34
        }

        public enum OutcomeStatuses : int
        {
            Level1 = 1,
            Level2 = 2,
            Level3 = 3,
            Level4 = 4,
            Level5 = 5,
            Level6 = 6
        }

        
        public static class AddressTypes
        {
            public const string Office = "Office";
            public const string Residence = "Residence";
            public const string Billing = "Billing";
            public const string Mailing = "Mailing";
            public const string Other = "Other";
        }

        public enum MasterServiceTypes
        {
            CellularTherapy = 1,
            CordBlood = 2
        }

        public enum ServiceTypes
        {
            ClinicalProgramCt = 1,
            ProcessingCt = 2,
            ApheresisCollection = 3,
            MarrowCollectionCt = 4,
            CbCollection = 5,
            CbProcessing = 6,
            CbBank = 7,
            CbStorage = 8
        }

        public enum ApplicationResponseStatuses
        {
            Reviewed = 2,
            ForReview = 3,
            NotCompliant = 4,
            Na = 5,
            NoResponseRequested = 7,
            Rfi = 8,
            RfiCompleted = 9,
            RfiFollowup = 10,
            New = 11
        }

        public static class ApplicationResponseStatus
        {
            public const string ForReview = "For Review";
            public const string Reviewed = "Reviewed";
            public const string Compliant = "Compliant";
            public const string NotCompliant = "Not Compliant";
            public const string NA = "N/A";
            public const string NoResponseRequested = "No Response Requested";
            public const string RFI = "RFI";
            public const string RFICompleted = "RFI Completed";
            public const string RfiFollowup = "RFI/Followup";
            public const string New = "New";
            public const string InProgress = "In Progress";
            public const string Complete = "Complete";
            public const string NotStarted = "Not Started";
        }

        public class ConfigurationConstants
        {
            public const string NewRequestMessage = "NewRequestMessage";
            public const string EmailUserName = "EmailUserName";
            public const string EmailPassword = "EmailPassword";
            public const string EmailHost = "EmailHost";
            public const string EmailPort = "EmailPort";
            public const string EmailFromAddress = "EmailFromAddress";
            public const string Url = "Url";
            public const string FactPortalEmailAddress = "FactPortalEmailAddress";
            public const string StorageConnectionString = "StorageConnectionString";
            public const string StaffEmailList = "StaffEmailList";
            public const string K2Server = "K2Server";
            public const string K2Host = "K2Host";
            public const string K2UserName = "K2UserName";
            public const string K2Password = "K2Password";
            public const string K2Port = "K2Port";
            public const string ScheduleInspectionEmail = "ScheduleInspectionEmail";
            public const string DocumentLibraryApiKey = "DocumentLibraryApiKey";
            public const string ReportingUserName = "ReportingUserName";
            public const string ReportingPassword = "ReportingPassword";
            public const string FactOnlyVault = "FactOnlyVault";
            public const string UseAnnualApproval = "UseAnnualApproval";
        }

        public class OrganizationFacilityConstant
        {
            public const string DuplicateRelation = "Consultant is already assigned to this organization for the date range defined.";
            public const string OneProcessType = "This Organization is already associated with this Facility";

            public const string OneProcessingOneApheresis =
                "There must be a 'Processing CT' service type facility and one 'Apheresis Collection or 'Marrow Collection.";

            public const string RelationDeletedSuccessfully = "Relation deleted successfully";
            public const string RelationSavedSuccessfully = "Relation saved successfully";
        }

        public class OrganizationConsultantConstant
        {
            public const string ConsultantAssociatedSuccessfully = "Consultant associated successfully";
            public const string ConsultantRemovedSuccessfully = "Consultant removed successfully";
        }

        public class GeoCodeConstants
        {
            public const string GeoCodeRestUrl = "GeoCodeRestUrl";
            public const string GeoCodeApiKey = "GeoCodeApiKey";
        }

        public class ApplicationSettings
        {
            public const string SystemUrl = "System Base Url";
            public const string InspectorMileageRange = "Miles to Exclude Inspectors";
            public const string PasswordResetTimeout = "Timeout on Password Reset Tokens (in hours)";
            public const string RequirementManagementSetName = "Requirement Management Set Name";
            public const string AutoSaveTimer = "Auto Save Timer(In Seconds)";
            public const string AutoCcEmailAddress = "Auto CC Email Address";
            public const string ApplicationStaffOverview = "Application Staff Overview";
            public const string InspectionReminder = "Inspector Reminder: Repeat every X days";
            public const string CoordinatorSupervisorEmail = "Supervisor, Accreditation Services Email";
        }

        public class QuestionTypes
        {
            public const string TextArea = "Text Area";
            public const string RadioButtons = "Radio Buttons";
            public const string Checkboxes = "Checkboxes";
            public const string DocumentUpload = "Document Upload";
            public const string Date = "Date";
            public const string DateRange = "Date Range";
            public const string PeoplePicker = "People Picker";
            public const string Text = "Text Box";
            public const string Multiple = "Multiple";
        }

        public enum ApplicationType
        {
            ComplianceCtApplication = 1,
            ComplianceCbApplication = 2,
            EligibilityApplication = 3,
            AnnualApplication = 5,
            RenewalApplication = 6
        }

        public class ApplicationTypes
        {
            public const string CT = "Compliance CT Application";
            public const string CordBlood = "Compliance CB Application";
            public const string Common = "Compliance Common Application";
            public const string Eligibility = "Eligibility Application";
            public const string Annual = "Annual Application";
            public const string Renewal = "Renewal Application";
        }

        public enum ApplicationStatuses
        {
            Complete = 19,
            Cancelled = 20,
            ForReview = 8,
            Rfi = 9,
            RfiReview = 10,
            ApplicantResponse = 17
        }

        public class ApplicationStatus
        {
            public const string Pending = "Pending";
            public const string Applied = "Applied";
            public const string InReview = "In Review";
            public const string InspectionScheduled = "Inspection Scheduled";
            public const string Approved = "Approved";
            public const string Declined = "Declined";

            public const string InProgress = "In Progress";
            public const string ForReview = "For Review ";
            public const string RFI = "RFI In Progress";
            public const string RFIReview = "RFI Review";
            public const string SchedulingInspection = "Scheduling Inspection";
            public const string PreInspection = "Pre-Inspection ";
            public const string OnSiteInspection = "On-Site Inspection";
            public const string PostInspectionReview = "Post Inspection Review ";
            public const string AccreditationCommittee = "Accreditation Committee ";
            public const string PostCommitteeReview = "Post Committee Review ";
            public const string ApplicantResponse = "Applicant Response";
            public const string ApplicantResponseReview = "Applicant Response Review";
            public const string Complete = "Complete";
            public const string Cancelled = "Cancelled";
            public const string AwaitingDirectorApproval = "Awaiting Director Approval";
            public const string PastDue = "Past Due";
        }

        public class ApplicationStatusForApplicants
        {
            public const string InProgress = "In Progress";
            public const string ApplicationSubmitted = "Application Submitted";
            public const string RFI = "RFI";
            public const string RFISubmitted = "RFI Submitted";
            public const string InspectionScheduled = "Inspection Scheduled";
            public const string UnderInspection = "Under Inspection";
            public const string PendingCommitteeReview = "Pending Committee Review";
            public const string PostCommitteeRFI = "Post Committee RFI";
            public const string PostCommitteeRFISubmitted = "Post Committee RFI Submitted";
            public const string Complete = "Complete";

        }


        public class ScopeType
        {
            public const string DuplicateScopeName = "Scope Name already exist";
            public const string DuplicateImportName = "Import Name already exist";
            public const string DuplicateScopeNameAndImportNames = "Both Scope Name and Import Name already exist";
        }

        public class ServiceType
        {
            public const string ClinicalProgramCT = "Clinical Program CT";
            public const string ProcessingCT = "Processing CT";
            public const string ApheresisCollection = "Apheresis Collection";
            public const string MarrowCollectionCT = "Marrow Collection CT";
            public const string CBCollection = "CB Collection";
            public const string CBProcessing = "CB Processing";
            public const string CBBank = "CB Bank";
            public const string CBStorage = "CB Storage";
        }

        public class FacilityAccreditations
        {
            public const string AABB = "AABB";
            public const string CAP = "CAP";
            public const string JCAHO = "JCAHO";
            public const string ASHI = "ASHI";
            public const string EFI = "EFI";
            public const string Other = "Other";
        }

        public class AccreditationsRoles
        {
            public const string Inspectors = "Inspector";
            public const string Auditors = "Auditor";
            public const string Observers = "Observer";
            public const string InspectorTrainees = "Inspector Trainee";
            public const string PrimaryReviewer = "Primary Reviewer";
        }

        public enum AccreditationRoles
        {
            Inspector = 1,
            Auditor = 2,
            InspectorTrainee = 3,
            Observer = 4,
            PrimaryReviewer = 6
        }

        public class AccreditationsStatus
        {
            public const string Accredited = "Accredited";
            public const string Pending = "Pending";
            public const string Suspended = "Suspended";
            public const string Expired = "Expired";
            public const string Withdrawn = "Withdrawn";
            public const string GracePeriod = "Grace Period";
        }

        public class MasterServiceType
        {
            public const string CellularTherapy = "Cellular Therapy";
            public const string CordBlood = "Cord Blood";
        }

        public class JobFunctions
        {
            public const string ClinicalProgramDirector = "Clinical Program Director";
            public const string TransplantPhysician = "Transplant Physician";
            public const string ConsultingPhysician = "Consulting Physician";
            public const string ClinicalTransplantCoordinator = "Clinical Transplant Coordinator";
            public const string TransplantNurse = "Transplant Nurse";
            public const string DataManager = "Data Manager";
            public const string BoneMarrowCollectionFacilityDirector = "Bone Marrow Collection Facility Director";

            public const string BoneMarrowCollectionFacilityMedicalDirector =
                "Bone Marrow Collection Facility Medical Director";

            public const string ApheresisCollectionFacilityDirector = "Apheresis Collection Facility Director";

            public const string ApheresisCollectionFacilityMedicalDirector =
                "Apheresis Collection Facility Medical Director";

            public const string CollectionFacilitySupervisor = "Collection Facility Supervisor";
            public const string CollectionNurse = "Collection Nurse";
            public const string ProcessingFacilityDirector = "Processing Facility Director";
            public const string ProcessingFacilityMedicalDirector = "Processing Facility Medical Director";
            public const string ProcessingFacilitySupervisor = "Processing Facility Supervisor";
            public const string MedicalTechnologist = "Medical Technologist";
            public const string BmtAdministrator = "BMT Administrator";
            public const string QmManager = "QM Manager";
            public const string QmSupervisor = "QM Supervisor";
            public const string CordBloodBankDirector = "Cord Blood Bank Director";
            public const string CordBloodBankMedicalDirector = "Cord Blood Bank Medical Director";
            public const string CordBloodCollectionSiteDirector = "Cord Blood Collection Site Director";
            public const string CordBloodProcessingFacilityDirector = "Cord Blood Processing Facility Director";
            public const string CordBloodBankPersonnel = "Cord Blood Bank Personnel";
            public const string FactLiaison = "FACT Liaison";
            public const string RegistryPersonnel = "Registry Personnel";
            public const string Other = "Other";
            public const string ClinicalQualityManagementSupervisor = "Clinical Quality Management Supervisor";
            public const string MarrowQualityManagementSupervisor = "Marrow Quality Management Supervisor";
            public const string ApheresisQualityManagementSupervisor = "Apheresis Quality Management Supervisor";
            public const string ProcessingQualityManagementSupervisor = "Processing Quality Management Supervisor";
            public const string CordBloodQualityManagementSupervisor = "Cord Blood Quality Management Supervisor";
            public const string OrganizationDirector = "Organization Director";
            public const string OrganizationAdmin = "Primary Contact";
        }

        public class JobFunctionIds
        {
            public static Guid OrganizationDirector = Guid.Parse("042e1b14-995a-47b2-b927-292dd5ff4998");
        }

        public class Languages
        {
            public const string English = "English";
            public const string Arabic = "Arabic";
            public const string Armenian = "Armenian";
            public const string Azerbaijani = "Azerbaijani";
            public const string Bulgarian = "Bulgarian";
            public const string Catalan = "Catalan";
            public const string Chinese = "Chinese";
            public const string Croatian = "Croatian";
            public const string Czech = "Czech";
            public const string Danish = "Danish";
            public const string Estonian = "Estonian";
            public const string Filipino = "Filipino";
            public const string Finnish = "Finnish";
            public const string French = "French";
            public const string German = "German";
            public const string Greek = "Greek";
            public const string Hebrew = "Hebrew";
            public const string Hindi = "Hindi";
            public const string Hungarian = "Hungarian";
            public const string Icelandic = "Icelandic";
            public const string Indonesian = "Indonesian";
            public const string Irish = "Irish";
            public const string Italian = "Italian";
            public const string Japanese = "Japanese";
            public const string Korean = "Korean";
            public const string Latvian = "Latvian";
            public const string Lithuanian = "Lithuanian";
            public const string Macedonian = "Macedonian";
            public const string Norwegian = "Norwegian";
            public const string Persian = "Persian";
            public const string Polish = "Polish";
            public const string Portuguese = "Portuguese";
            public const string Romanian = "Romanian";
            public const string Russian = "Russian";
            public const string Serbian = "Serbian";
            public const string Slovak = "Slovak";
            public const string Slovenian = "Slovenian";
            public const string Spanish = "Spanish";
            public const string Swedish = "Swedish";
            public const string Thai = "Thai";
            public const string Turkish = "Turkish";
            public const string Ukrainian = "Ukrainian";
            public const string Vietnamese = "Vietnamese";
            public const string Welsh = "Welsh";
        }

        public class Memberships
        {
            public const string Isct = "ISCT";
            public const string IsctEurope = "ISCT-Europe";
            public const string Ebmt = "EBMT";
            public const string Asbmt = "ASBMT";
            public const string Asfa = "ASFA";
            public const string NetCord = "NetCord";
            public const string Jacie = "JACIE";
        }

        public class FacilitySiteConstant
        {
            public const string DuplicateRelation = "This Site is already associated with this Facility";
            public const string RelationDeletedSuccessfully = "Relation deleted successfully";
            public const string RelationSavedSuccessfully = "Relation saved successfully";
        }

        public class ComplianceApplicationApprovalStatuses
        {
            public const string Planning = "Planning";
            public const string PendingApproval = "Pending Approval";
            public const string Approved = "Approved / Active";
            public const string Submitted = "Submitted";
            public const string Closed = "Closed";
            public const string Reject = "Reject"; // todo need to confirm from Nick
        }

        public class AssociationTypes
        {
            public const string Evidence = "Evidence";
            public const string RfiResponse = "RFI Response";
        }

        public class ResponseComments
        {
            public const string Applicant = "Applicant";
        }

        public class Credentials
        {
            public const string MD = "MD";
            public const string PhD = "PhD";
            public const string MSN = "MSN";
            public const string MBA = "MBA";
            public const string MS = "MS";
            public const string MA = "MA";
            public const string APRN = "APRN";
            public const string RN = "RN";
            public const string LPN = "LPN";
            public const string MT = "MT";
            public const string MLS = "MLS";
            public const string MSc = "MSc";
            public const string BSc = "BSc";
            public const string BS = "BS";
            public const string BA = "BA";
        }

        public class OutcomeStatus
        {
            public const string Level1 = "Level 1";
            public const string Level2 = "Level 2";
            public const string Level3 = "Level 3";
            public const string Level4 = "Level 4";
            public const string Level5 = "Level 5";
            public const string Level6 = "Level 6";
        }

        public class EmailTemplateName
        {
            public const string AccreditationLevel1 = "Accreditation Level 1";
            public const string AccreditationOther = "Accreditation Other";
        }

        public class ReportReviewStatus
        {
            public const string Initial = "Initial";
            public const string Renewal = "Renewal";
            public const string Response = "Response";
            public const string ReInspection = "Re-inspection";
            public const string AddOn = "Add-on";
            public const string Reapplication = "Reapplication";
            public const string Appeal = "Appeal";
        }

        public class CommentTypes
        {
            public const string RFI = "RFI";
            public const string Citation = "Citation";
            public const string Suggestion = "Suggestion";
            public const string FACTResponse = "FACT Response"; //Bug:966 (update dbo.CommentType set CommentTypeName = 'FACT Response' where CommentTypeId = 4)
            public const string FACTOnly = "FACT Only";
        }

        public static class AccreditationStatuses
        {
            public const string Accredited = "Accredited";
            public const string New = "New";
            public const string Suspended = "Suspended";
            public const string Expired = "Expired";
            public const string Withdrawn = "Withdrawn";
            public const string GracePeriod = "Grace Period";
        }

        public class ApplicationSectionStatusView
        {
            public const string Reviewed = "Reviewed";
            public const string ForReview = "For Review";
            public const string Compliant = "Compliant";
            public const string NotCompliant = "Not Compliant";
            public const string NotApplicable = "N/A";
            public const string NoResponseRequested = "No Response Requested";
            public const string RFI = "RFI";
            public const string RFICompleted = "RFI Completed";
            public const string RFIFollowUp = "RFI/Followup";
            public const string New = "New";
            
            public const string Flagged = "Flagged";
            public const string RFIComments = "RFI Comments";
            public const string Citation = "Citation";
            public const string Attachments = "Attachments";

            public const string NoResponse = "No Response";
            public const string Incomplete = "Incomplete";
            public const string Partial = "Partial";
            public const string NotStarted = "Not Started"; //If there are no responses given
            public const string Complete = "Complete";
            public const string InProgress = "In Progress";
        }

        public enum AccreditationStatus : int
        {
            Initial = 1,
            Renewal = 2,
            ReApplication = 3
        }

        public enum AddressType : int
        {
            Office = 1,
            Residence = 2,
            Billing = 3,
            Mailing = 4,
            Other = 5
        }

        public class Countries
        {
            public const string UnitedStatesOfAmerica = "United States of America";
            public const string Afghanistan = "Afghanistan";
            public const string Albania = "Albania";
            public const string Algeria = "Algeria";
            public const string Andorra = "Andorra";
            public const string Angola = "Angola";
            public const string AntiguaAndBarbuda = "Antigua and Barbuda";
            public const string Argentina = "Argentina";
            public const string Armenia = "Armenia";
            public const string Australia = "Australia";
            public const string Austria = "Austria";
            public const string Azerbaijan = "Azerbaijan";
            public const string Bahamas = "Bahamas";
            public const string Bahrain = "Bahrain";
            public const string Bangladesh = "Bangladesh";
            public const string Barbados = "Barbados";
            public const string Belarus = "Belarus";
            public const string Belgium = "Belgium";
            public const string Belize = "Belize";
            public const string Benin = "Benin";
            public const string Bhutan = "Bhutan";
            public const string Bolivia = "Bolivia";
            public const string BosniaAndHerzegovina = "Bosnia and Herzegovina";
            public const string Botswana = "Botswana";
            public const string Brazil = "Brazil";
            public const string Brunei = "Brunei";
            public const string Bulgaria = "Bulgaria";
            public const string BurkinaFaso = "Burkina Faso";
            public const string Burundi = "Burundi";
            public const string CaboVerde = "Cabo Verde";
            public const string Cambodia = "Cambodia";
            public const string Cameroon = "Cameroon";
            public const string Canada = "Canada";
            public const string CentralAfricanRepublic = "Central African Republic";
            public const string Chad = "Chad";
            public const string Chile = "Chile";
            public const string China = "China";
            public const string Colombia = "Colombia";
            public const string Comoros = "Comoros";
            public const string CongoRepublic = "Congo, Republic of the";
            public const string CongoDemocraticRepublic = "Congo, Democratic Republic of the";
            public const string CostaRica = "Costa Rica";
            public const string CotedIvoire = "Cote d'Ivoire";
            public const string Croatia = "Croatia";
            public const string Cuba = "Cuba";
            public const string Cyprus = "Cyprus";
            public const string CzechRepublic = "Czech Republic";
            public const string Denmark = "Denmark";
            public const string Djibouti = "Djibouti";
            public const string Dominica = "Dominica";
            public const string DominicanRepublic = "Dominican Republic";
            public const string Ecuador = "Ecuador";
            public const string Egypt = "Egypt";
            public const string ElSalvador = "El Salvador";
            public const string EquatorialGuinea = "Equatorial Guinea";
            public const string Eritrea = "Eritrea";
            public const string Estonia = "Estonia";
            public const string Ethiopia = "Ethiopia";
            public const string Fiji = "Fiji";
            public const string Finland = "Finland";
            public const string France = "France";
            public const string Gabon = "Gabon";
            public const string Gambia = "Gambia";
            public const string Georgia = "Georgia";
            public const string Germany = "Germany";
            public const string Ghana = "Ghana";
            public const string Greece = "Greece";
            public const string Grenada = "Grenada";
            public const string Guatemala = "Guatemala";
            public const string Guinea = "Guinea";
            public const string GuineaBissau = "Guinea-Bissau";
            public const string Guyana = "Guyana";
            public const string Haiti = "Haiti";
            public const string Honduras = "Honduras";
            public const string Hungary = "Hungary";
            public const string Iceland = "Iceland";
            public const string India = "India";
            public const string Indonesia = "Indonesia";
            public const string Iran = "Iran";
            public const string Iraq = "Iraq";
            public const string Ireland = "Ireland";
            public const string Israel = "Israel";
            public const string Italy = "Italy";
            public const string Jamaica = "Jamaica";
            public const string Japan = "Japan";
            public const string Jordan = "Jordan";
            public const string Kazakhstan = "Kazakhstan";
            public const string Kenya = "Kenya";
            public const string Kiribati = "Kiribati";
            public const string Kosovo = "Kosovo";
            public const string Kuwait = "Kuwait";
            public const string Kyrgyzstan = "Kyrgyzstan";
            public const string Laos = "Laos";
            public const string Latvia = "Latvia";
            public const string Lebanon = "Lebanon";
            public const string Lesotho = "Lesotho";
            public const string Liberia = "Liberia";
            public const string Libya = "Libya";
            public const string Liechtenstein = "Liechtenstein";
            public const string Lithuania = "Lithuania";
            public const string Luxembourg = "Luxembourg";
            public const string Macedonia = "Macedonia";
            public const string Madagascar = "Madagascar";
            public const string Malawi = "Malawi";
            public const string Malaysia = "Malaysia";
            public const string Maldives = "Maldives";
            public const string Mali = "Mali";
            public const string Malta = "Malta";
            public const string MarshallIslands = "Marshall Islands";
            public const string Mauritania = "Mauritania";
            public const string Mauritius = "Mauritius";
            public const string Mexico = "Mexico";
            public const string Micronesia = "Micronesia";
            public const string Moldova = "Moldova";
            public const string Monaco = "Monaco";
            public const string Mongolia = "Mongolia";
            public const string Montenegro = "Montenegro";
            public const string Morocco = "Morocco";
            public const string Mozambique = "Mozambique";
            public const string MyanmarBurma = "Myanmar(Burma)";
            public const string Namibia = "Namibia";
            public const string Nauru = "Nauru";
            public const string Nepal = "Nepal";
            public const string Netherlands = "Netherlands";
            public const string NewZealand = "New Zealand";
            public const string Nicaragua = "Nicaragua";
            public const string Niger = "Niger";
            public const string Nigeria = "Nigeria";
            public const string NorthKorea = "North Korea";
            public const string Norway = "Norway";
            public const string Oman = "Oman";
            public const string Pakistan = "Pakistan";
            public const string Palau = "Palau";
            public const string Palestine = "Palestine";
            public const string Panama = "Panama";
            public const string PapuaNewGuinea = "Papua New Guinea";
            public const string Paraguay = "Paraguay";
            public const string Peru = "Peru";
            public const string Philippines = "Philippines";
            public const string Poland = "Poland";
            public const string Portugal = "Portugal";
            public const string Qatar = "Qatar";
            public const string Romania = "Romania";
            public const string Russia = "Russia";
            public const string Rwanda = "Rwanda";
            public const string StKittsAndNevis = "St.Kitts and Nevis";
            public const string Samoa = "Samoa";
            public const string SanMarino = "San Marino";
            public const string SaoTomeAndPrincipe = "Sao Tome and Principe";
            public const string SaudiArabia = "Saudi Arabia";
            public const string Senegal = "Senegal";
            public const string Serbia = "Serbia";
            public const string Seychelles = "Seychelles";
            public const string SierraLeone = "Sierra Leone";
            public const string Singapore = "Singapore";
            public const string Slovakia = "Slovakia";
            public const string Slovenia = "Slovenia";
            public const string SolomonIslands = "Solomon Islands";
            public const string Somalia = "Somalia";
            public const string SouthAfrica = "South Africa";
            public const string SouthKorea = "South Korea";
            public const string SouthSudan = "South Sudan";
            public const string Spain = "Spain";
            public const string SriLanka = "Sri Lanka";
            public const string Sudan = "Sudan";
            public const string Suriname = "Suriname";
            public const string Swaziland = "Swaziland";
            public const string Sweden = "Sweden";
            public const string Switzerland = "Switzerland";
            public const string Syria = "Syria";
            public const string Taiwan = "Taiwan";
            public const string Tajikistan = "Tajikistan";
            public const string Tanzania = "Tanzania";
            public const string Thailand = "Thailand";
            public const string Togo = "Togo";
            public const string Tonga = "Tonga";
            public const string Tunisia = "Tunisia";
            public const string Turkey = "Turkey";
            public const string Turkmenistan = "Turkmenistan";
            public const string Tuvalu = "Tuvalu";
            public const string Uganda = "Uganda";
            public const string Ukraine = "Ukraine";
            public const string UnitedArabEmirates = "United Arab Emirates";
            public const string UnitedKingdom = "United Kingdom";
            public const string Uruguay = "Uruguay";
            public const string Uzbekistan = "Uzbekistan";
            public const string Vanuatu = "Vanuatu";
            public const string VaticanCity = "Vatican City";
            public const string Venezuela = "Venezuela";
            public const string Vietnam = "Vietnam";
            public const string Yemen = "Yemen";
            public const string Zambia = "Zambia";
            public const string Zimbabwe = "Zimbabwe";
        }

        public class States
        {
            public const string Alabama = "Alabama";
            public const string Alaska = "Alaska";
            public const string Arizona = "Arizona";
            public const string Arkansas = "Arkansas";
            public const string California = "California";
            public const string Colorado = "Colorado";
            public const string Connecticut = "Connecticut";
            public const string Delaware = "Delaware";
            public const string Florida = "Florida";
            public const string Georgia = "Georgia";
            public const string Hawaii = "Hawaii";
            public const string Idaho = "Idaho";
            public const string Illinois = "Illinois";
            public const string Indiana = "Indiana";
            public const string Iowa = "Iowa";
            public const string Kansas = "Kansas";
            public const string Kentucky = "Kentucky";
            public const string Louisiana = "Louisiana";
            public const string Maine = "Maine";
            public const string Maryland = "Maryland";
            public const string Massachusetts = "Massachusetts";
            public const string Michigan = "Michigan";
            public const string Minnesota = "Minnesota";
            public const string Mississippi = "Mississippi";
            public const string Missouri = "Missouri";
            public const string Montana = "Montana";
            public const string Nebraska = "Nebraska";
            public const string Nevada = "Nevada";
            public const string NewHampshire = "New Hampshire";
            public const string NewJersey = "New Jersey";
            public const string NewMexico = "New Mexico";
            public const string NewYork = "New York";
            public const string NorthCarolina = "North Carolina";
            public const string NorthDakota = "North Dakota";
            public const string Ohio = "Ohio";
            public const string Oklahoma = "Oklahoma";
            public const string Oregon = "Oregon";
            public const string Pennsylvania = "Pennsylvania";
            public const string Rhode = "Rhode";
            public const string SouthCarolina = "South Carolina";
            public const string SouthDakota = "South Dakota";
            public const string Tennessee = "Tennessee";
            public const string Texas = "Texas";
            public const string Utah = "Utah";
            public const string Vermont = "Vermont";
            public const string Virginia = "Virginia";
            public const string Washington = "Washington";
            public const string WestVirginia = "West Virginia";
            public const string Wisconsin = "Wisconsin";
            public const string Wyoming = "Wyoming";
        }

        public class CBCollectionType
        {
            public const string Fixed = "Fixed";
            public const string NonFixed = "Non-Fixed";
            public const string FixedAndNonFixed = "Fixed and Non-Fixed";
        }

        public class ClinicalPopulationType
        {
            public const string Adult = "Adult";
            public const string Pediatric = "Pediatric";
            public const string AdultandPediatric = "Adult and Pediatric";
            //public const string CTOnly = "CT Only";
        }

        public class ClinicalType
        {
            public const string InPatient = "In Patient";
            public const string OutPatient = "Out Patient";
        }

        public class CollectionProductType
        {
            public const string PeripheralBlood = "Peripheral Blood (PB)";
            public const string Marrow = "Marrow";
            public const string PBRIPO = "Peripheral Blood For Research and IND Products Only";
            public const string MRIPO = "Marrow For Research and IND Products Only";
        }

        public class ProcessingType
        {
            public const string Minimal = "Minimal";
            public const string MoreThanMinimal = "More than Minimal";
            public const string OffSiteStorage = "Off-Site Storage";
        }

        public class TransplantType
        {
            public const string AutologousAdult = "Autologous Adult";
            public const string AllogeneicAdult = "Allogeneic Adult";
            public const string AutologousPediatric = "Autologous Pediatric";
            public const string AllogeneicPediatric = "Allogeneic Pediatric";
        }

        public enum TransplantTypes
        {
            AutologousAdult = 1,
            AllogeneicAdult = 2,
            AutologousPediatric = 5,
            AllogeneicPediatric = 6
        }

        public class CBUnitType
        {
            public const string Related = "Related";
            public const string Unrelated = "Unrelated";
            public const string RelatedAndUnrelated = "Related and Unrelated";
        }

        public class CBCategories
        {
            public const string Banked = "Banked";
            public const string Released = "Released";
            public const string Outcome = "Outcome";
        }

        public class TransplantCellTypes
        {
            public const string Hpc = "HPC";
            public const string ApheresisHpc = "Apheresis HPC";
            public const string MarrowHpc = "Marrow HPC";
            public const string CordBloodTCells = "Cord Blood T Cells";
            public const string ApheresisTCells = "Apheresis T Cells";
            public const string MarrowTCells = "Marrow T Cells";
            public const string CordBloodImmuneEffectorCells = "Cord Blood Immune Effector Cells";
            public const string CardiacCells = "Cardiac Cells";
            public const string IsletCells = "Islet Cells";
            public const string AdiposeCells = "Adipose Cells";
            public const string RenalCells = "Renal Cells";
            public const string HepaticCells = "Hepatic Cells";
        }

        public class CollectionType
        {
            public const string Apheresis = "Apheresis";
            public const string Marrow = "Marrow";
            public const string CordBlood = "Cord Blood";
        }

        public class CacheStatuses
        {
            public const string AppSettings = "appSettings";
            public const string CbUnitTypes = "cbUnitTypes";
            public const string CbCategories = "cbCategories";
            public const string TransplantCellTypes = "transplantCellTypes";
            public const string ClinicalPopulationTypes = "clinicalPopulationTypes";
            public const string TransplantTypes = "transplantTypes";
            public const string CollectionTypes = "collectionTypes";
            public const string ProcessingTypes = "processingTypes";
            public const string States = "states";
            public const string Countries = "countries";
            public const string EmailTemplates = "emailTemplates";
            public const string ReportReviewStatuses = "reportReviewStatuses";
            public const string OutcomeStatuses = "outcomeStatuses";
            public const string AccreditationStatuses = "accreditationStatuses";
            public const string Organizations = "organizations";
            public const string Facilities = "facilities";
            public const string Sites = "sites";
            public const string NetcordMembershipTypes = "netcordMembershipTypes";
            public const string ActiveVersions = "activeVersions";
        }

        public class NetcordMembershipTypes
        {
            public const string Provisional = "Provisional";
            public const string Associate = "Associate";
            public const string Full = "Full";
        }

        public class EmailTemplates
        {
            public const string SendBackForRfi = "Send Back For RFI";
            public const string AccreditationOutcomeSubject = "FACT Accreditation Committee Outcome - ";
        }

        public class Reports
        {
            public const string InspectionSummary = "Inspection Summary";
            public const string CtInspectionRequest = "CT Inspection Request";
            public const string CbInspectionRequest = "CB Inspection Request";
            public const string TraineeInspectionSummary = "Trainee Inspection Summary";
            public const string Application = "Application";
            public const string SingleApplication = "Single Application";
            public const string Response = "Response";
            public const string Activity = "Activity";
            public const string AccreditationReport = "Accreditation Report";
            public const string OutcomesData = "Cibmtr";
        }
    }
}
