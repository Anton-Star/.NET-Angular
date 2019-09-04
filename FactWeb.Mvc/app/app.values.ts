interface IEvents {
    controllerActivateSuccess: string;
    controllerActivating: string;
    userLoggedIn: string;
    userLoggedOff: string;
    pageNameSet: string;
    entitiesChanged: string;
    entitiesImported: string;
    hasChangesChanged: string;
    spinnerToggle: string;
    requirementSaved: string;
    loginPageInit: string;
    otherPageInit: string;
    orgSet: string;
    bulkUpdate: string;
    userImpersonated: string;
    userImpersonatedEnd: string;
    initDataLoaded: string;
    coordinatorSet: string;
    cacheInvalidated: string;
    hasRfi: string;
    reloadSections: string;
    emailTemplatesLoaded: string;
    applicationSettingsLoaded: string;
    applicationLoaded: string;
    complianceApplicationLoaded: string;
    compAppLoaded: string;
    accessTokenLoaded: string;
    organizationLoaded: string;
    userInspectorLoaded: string;
    applicationSectionsLoaded: string;
    applicationInspectorsLoaded: string;
    complianceApplicationInspectorsLoaded: string;
    postInspectionDocumentsLoaded: string;
    documentsLoaded: string;
    inspectionScheduleDetailsLoaded: string;
    inspectorsLoaded: string;
    onReportChanged: string;
}

interface IApplicationSectionStatuses {
    complete: string;
    notStarted: string; //same as in progress
    partial: string; //TODO: Remove this status. for backward compatibility only
    inProgress: string;
    //partial: string; [1508] - Partial = InProgress
    notCompliant: string;
    rfiFollowUp: string;
    noResponseRequested: string;
    flagged: string;
    reviewed: string;
    forReview: string;
    rfi: string;
    notApplicable: string;
    compliant: string;
    rfiCompleted: string;
    new: string;
    rfiComments: string;
    citation: string;
    attachments: string;
    submitted: string;
    
}

interface IRoleIds {
    inspector: number;
    factAdministrator: number;
    factCoordinator: number;
    factConsultantCoordinator: number;
    factQualityManager: number;
    user: number;
    factConsultant: number;
    nonSystemUser: number;
    qualityManager: number;
}

interface IRoles {
    inspector: string;
    factAdministrator: string;
    factCoordinator: string;
    factConsultantCoordinator: string;
    factQualityManager: string;
    //organizationalDirector: string;
    user: string;
    factConsultant: string;
    nonSystemUser: string;
    qualityManager: string;
}

interface IReportNames {
    inspectionSummary: string;
    traineeInspectionSummary: string;
    cbInspectionRequest: string;
    ctInspectionRequest: string;
    application: string;
    activity: string;
    accreditationReport: string;
}

interface IEmailTypes {
    sendToInspector: string;
    backToRfi: string;
    annualComplete: string;
}

interface IApplicationStatuses {
    pendingDirector: string;
    applied: string;
    underReview: string;
    inspectionScheduled: string;
    approved: string;
    declined: string;
    inProgress: string;
    forReview: string;
    rfiInProgress: string;
    rfiReview: string;
    cancelled: string;
    complete: string;
}

interface IApplicationResponseStatuses {
    reviewed: string;
    forReview: string;
    compliant: string;
    notCompliant: string;
    na: string;
    noResponseRequested: string;
    rfi: string;
    rfiCompleted: string;
    rfiFollowup: string;
    new: string;
    completed: string;
}

interface IConfig {
    trueVaultPath: string;
    busyIndicator: string;
    events: IEvents;
    keyCodes: Object;
    remoteServiceName: string;
    applicationSectionStatuses: IApplicationSectionStatuses;
    version: string;
    applicationTypes: Array<app.services.IApplicationType>;
    applicationTypeNames: IApplicationTypes;
    applicationStatuses: IApplicationStatuses;
    roles: IRoles;
    roleIds: IRoleIds;
    questionTypes: IQuestionTypes;
    reportNames: IReportNames;
    reports: Array<app.services.IReport>;
    cacheNames: ICacheNames;
    emailTypes: IEmailTypes;
    genericKey: string;
    factKey: string;
    useTwoFactor: boolean;
    qmMessage: string;    
    emailTemplates: app.services.IEmailTemplate[];
    applicationResponseStatuses: IApplicationResponseStatuses;
}

interface IApplicationTypes {
    complianceCt: string;
    complianceCb: string;
    eligibility: string;
    complianceCommon: string;
    annual: string;
    renewal: string;
    netcord: string;
}

interface IQuestionTypes {
    multiple: string;
    textArea: string;
    textBox: string;
    radioButtons: string;
    checkboxes: string;
    documentUpload: string;
    date: string;
    dateRange: string;
    peoplePicker: string;
}

interface ICacheNames {
    appSettings: string;
    cbUnitTypes: string;
    cbCategories: string;
    transplantCellTypes: string;
    clinicalPopulationTypes: string;
    transplantTypes: string;
    collectionTypes: string;
    processingTypes: string;
    states: string;
    countries: string;
    emailTemplates: string;
    reportReviewStatuses: string;
    outcomeStatuses: string;
    accreditationStatuses: string;
    organizations: string;
    facilities: string;
    sites: string;
    cacheStatuses: string;
    netcordMembershipTypes: string;
    activeVersions: string;
}

((): void => {
    'use strict';
     
    var currentUser: app.services.IUser = {
        userId: '',
        type: '',
        firstName: '',
        lastName: '',
        fullName: '',
        emailAddress: '',
        preferredPhoneNumber: '',
        workPhoneNumber: '',
        role: { roleId: 0, roleName: '' },
        organizations: [],
        isLocked: false,
        webPhotoPath: '',
        emailOptOut: false,
        mailOptOut: false,
        resumePath: '',
        statementOfCompliancePath: '',
        annualProfessionHistoryFormPath: '',
        medicalLicensePath: '',
        completedStep2: false,
        isImpersonation: false
    };

    var keyCodes = {
        backspace: 8,
        tab: 9,
        enter: 13,
        esc: 27,
        space: 32,
        pageup: 33,
        pagedown: 34,
        end: 35,
        home: 36,
        left: 37,
        up: 38,
        right: 39,
        down: 40,
        insert: 45,
        del: 46
    };

    var dbNames: ICacheNames = {
        appSettings: 'appSettings',
        cbUnitTypes: 'cbUnitTypes',
        cbCategories: 'cbCategories',
        transplantCellTypes: 'transplantCellTypes',
        clinicalPopulationTypes: 'clinicalPopulationTypes',
        transplantTypes: 'transplantTypes',
        collectionTypes: 'collectionTypes',
        processingTypes: 'processingTypes',
        states: 'states',
        countries: 'countries',
        emailTemplates: 'emailTemplates',
        reportReviewStatuses: 'reportReviewStatuses',
        outcomeStatuses: 'outcomeStatuses',
        accreditationStatuses: 'accreditationStatuses',
        organizations: 'organizations',
        facilities: 'facilities',
        sites: 'sites',
        cacheStatuses: 'cacheStatuses',
        netcordMembershipTypes: 'netcordMembershipTypes',
        activeVersions: 'activeVersions'
    };

    var applicationSectionStatuses: IApplicationSectionStatuses = {
        complete: "Complete",
        notStarted: "Not Started",
        partial: "In Progress", //partial shud be removed. But we need to keep it till refactoring is complete
        inProgress: "In Progress",
        //ApplicationResponseStatus
        reviewed: "Reviewed",
        forReview: "For Review",
        compliant: "Compliant",
        notCompliant: "Not Compliant",
        notApplicable: "N/A",
        noResponseRequested: "No Response Requested",
        rfi: "RFI",
        rfiCompleted: "RFI Completed",
        rfiFollowUp: "RFI/Followup",
        new: "New",
        //flags
        flagged: "Flagged",
        rfiComments: "RFI Comments",
        citation: "Citation",
        attachments: "Attachments",
        submitted: "submitted"        
    };

    var applicationResponseStatuses: IApplicationResponseStatuses = {
        reviewed: "Reviewed",
        forReview: "For Review",
        compliant: "Compliant",
        notCompliant: "Not Compliant",
        na: "N/A",
        noResponseRequested: "No Response Requested",
        rfi: "RFI",
        rfiCompleted: "RFI Completed",
        rfiFollowup: "RFI/Followup",
        new: "New",
        completed: "Completed"
    }

    var questionTypes: IQuestionTypes = {
        multiple: "Multiple",
        textArea: "Text Area",
        textBox: "Text Box",
        radioButtons: "Radio Buttons",
        checkboxes: "Checkboxes",
        documentUpload: "Document Upload",
        date: "Date",
        dateRange: "Date Range",
        peoplePicker: "People Picker"
    }

    var events: IEvents = {
        controllerActivateSuccess: 'controller.activateSuccess',
        controllerActivating: 'controller.activating',
        userLoggedIn: 'userLoggedIn',
        userLoggedOff: 'userLoggedOff',
        entitiesChanged: 'datacontext.entitiesChanged',
        entitiesImported: 'datacontext.entitiesImported',
        hasChangesChanged: 'datacontext.hasChangesChanged',
        spinnerToggle: 'spinner.toggle',
        pageNameSet: 'pageNameSet',
        requirementSaved: 'requirementSaved',
        loginPageInit: 'loginPageInit',
        otherPageInit: 'otherPageInit',
        orgSet: 'orgSet',
        bulkUpdate:'bulkUpdate',
        userImpersonated: 'User Impersonated',
        userImpersonatedEnd: 'userImpersonatedEnd',
        initDataLoaded: 'initDataLoaded',
        coordinatorSet: 'coordinatorSet',
        cacheInvalidated: 'cacheInvalidated',
        hasRfi: 'hasRfi',
        reloadSections: 'reloadSections',
        emailTemplatesLoaded: 'emailTemplatesLoaded',
        applicationSettingsLoaded: 'applicationSettingsLoaded',
        applicationLoaded: 'applicationLoaded',
        complianceApplicationLoaded: 'complianceApplicationLoaded',
        compAppLoaded: 'compAppLoaded',
        accessTokenLoaded: 'accessTokenLoaded',
        organizationLoaded: 'organizationLoaded',
        userInspectorLoaded: 'userInspectorLoaded',
        applicationSectionsLoaded: 'applicationSectionsLoaded',
        applicationInspectorsLoaded: 'applicationInspectorsLoaded',
        complianceApplicationInspectorsLoaded: 'complianceApplicationInspectorsLoaded',
        postInspectionDocumentsLoaded: 'postInspectionDocumentsLoaded',
        documentsLoaded: 'documentsLoaded',
        inspectionScheduleDetailsLoaded: 'inspectionScheduleDetailsLoaded',
        inspectorsLoaded: 'inspectorsLoaded',
        onReportChanged: 'onReportChanged'
    };

    var appStatuses: IApplicationStatuses = {
        pendingDirector: "Pending Director Approval",
        applied: "Applied",
        underReview: "Under Review",
        inspectionScheduled: "Inspection Scheduled",
        approved: "Approved",
        declined: "Declined",
        inProgress: "In Progress",
        forReview: "For Review",
        rfiInProgress: "RFI In Progress",
        rfiReview: "RFI Review",
        cancelled: "Cancelled",
        complete: "Complete"
    };

    var config = {
            qmMessage: "",
            trueVaultPath: "https://api.truevault.com/v1/",
            busyIndicator: 'overlay', // 2 options: spinner or overlay
            events: events,
            keyCodes: keyCodes,
            remoteServiceName: "api/",
            applicationSectionStatuses: applicationSectionStatuses,
            //applicationSettings: [],
            version: '1.1.0',
            questionTypes: questionTypes,
            cacheNames: dbNames,
            applicationStatuses: appStatuses,
            applicationResponseStatuses: applicationResponseStatuses,
            useTwoFactor: true,
            applicationTypes: [
                {
                    id: 1,
                    name: "Compliance CT Application"
                },
                {
                    id: 2,
                    name: "Compliance CB Application"
                },
                {
                    id: 3,
                    name: "Eligibility Application"
                },
                {
                    id: 4,
                    name: "Compliance Common Application"
                },
                {
                    id: 5,
                    name: "Annual Application"
                },
                {
                    id: 6,
                    name: "Renewal Application"
                },
                { id: 8, name: "NETCORD" }
            ],
            applicationTypeNames: {
                complianceCt: "Compliance CT Application",
                complianceCb: "Compliance CB Application",
                eligibility: "Eligibility Application",
                complianceCommon: "Compliance Common Application",
                annual: "Annual Application",
                renewal: "Renewal Application",
                netcord: "NETCORD"
            },
            roles: {
                inspector: "Inspector",
                factAdministrator: "FACT Administrator",
                factCoordinator: "FACT Coordinator",
                factConsultantCoordinator: "FACT Consultant Coordinator",
                user: "User",
                factQualityManager: "FACT Quality Manager",
                factConsultant: "FACT Consultant",
                //organizationalDirector: "Organizational Director",
                nonSystemUser: "Non-System User",
                qualityManager: "FACT Quality Manager"
            },
            roleIds: {
                inspector: 32,
                factAdministrator: 1,
                factCoordinator: 34,
                factConsultantCoordinator: 33,
                user: 3,
                factQualityManager: 27,
                factConsultant: 30,
                //organizationalDirector: "Organizational Director",
                nonSystemUser: 19,
                qualityManager: 27
            },
        reportNames: {
            inspectionSummary: "Inspection Summary",
            traineeInspectionSummary: "Trainee Inspection Summary",
            cbInspectionRequest: "CB Inspection Request",
            ctInspectionRequest: "CT Inspection Request",
            application: "Application",
            activity: "Activity",
            accreditationReport: "Accreditation Report"
        },
        reports: [
            { name: "Inspection Summary", url: "Inspection/Inspection Summary" },
            { name: "Trainee Inspection Summary", url: "Inspection/Trainee Inspection Summary" },
            { name: "CB Inspection Request", url: "Inspection/CB Inspection Request" },
            { name: "CT Inspection Request", url: "Inspection/CT Inspection Request" },
            { name: "Application", url: "Reports/Application" },
            { name: "Activity", url: "Reports/Activity" },
            { name: "Accreditation Report", url: "Inspection/Accreditation Report" }
        ],
        emailTypes: {
            sendToInspector: "SendToInspector",
            backToRfi: "backToRfi",
            annualComplete: "annualComplete"
        }
    };

    angular
        .module('app')
        .value('currentUser', currentUser);

    angular
        .module('app')
        .value('config', config);

    angular.module('app')
        .value('froalaConfig', {key: 'te1C2sD6D7C4D5H5G4jqyznlyD-8mvtdE5lvbzG2B1A2B2D6B1B1C4G1D3=='});
})();