declare module app.services {
    interface ICpiType {
        id: string;
        name: string;
    }
    interface IExport {
        row: string;
    }
    interface IInspectionOverallDetail {
        inspectionScheduleId: number;
        siteDescription: string;
        commendablePractices: string;
        overallImpressions: string;
    }
    interface ICompAppApproval {
        firstName: string;
        lastName: string;
        emailAddress: string;
        isApproved: boolean;
        approvalDate: Date;
    }
    interface IApplicationSectionResponse {
        applicationSectionId: string;
        applicationSectionName: string;
        applicationSectionHelpText: string;
        applicationSectionUniqueIdentifier: string;
        hasQuestions: boolean;
        isVisible: boolean;
        hasFlag: boolean;
        sectionStatusName: string;
        hasRfiComment: boolean;
        hasCitationComment: boolean;
        hasFactOnlyComment: boolean;
        hasSuggestions: boolean;
        children?: IApplicationSectionResponse[];
        questions?: IQuestion[];
        nextSection?: IApplicationSectionResponse;
        id?: string;

        name?: string;
        uniqueIdentifier?: string;
    }

    interface IAppResponse {
        applicationId: number;
        applicationUniqueId: string;
        applicationTypeName: string;
        applicationSectionResponses: IApplicationSectionResponse[];
        applicationTypeStatusName: string;
        isInspectionResponsesComplete: boolean;
    }

    interface IApplicationSiteResponse {
        siteName: string;
        siteId: number;
        statusName: string;
        appResponses: IAppResponse[];
        submittedDate: Date;
        dueDate: Date;
        inspectionDate: Date;
    }

    interface ICompApplication {
        id: string;
        organizationId: number;
        organizationName: string;
        hasQmRestriction: boolean;
        applicationStatus: string;
        hasRfi: boolean;
        hasFlag: boolean;
        complianceApplicationSites: IApplicationSiteResponse[];
    }

    interface ISimpleOrganization {
        organizationId: number;
        organizationName: string;
        accreditationDate?: Date;
        accreditationExpirationDate?: Date;
        accreditationStatusId?: number;
        accreditationStatusName?: string;
        complianceApplicationId?: string;
        applicationUniqueId?: string;
        eligibilityApplicationUniqueId?: string;
        renewalApplicationUniqueId?: string;
    }
    interface IInspectionDetail {
        inspectionId: number;
        siteId: number;
        siteDescription: string;
        overallImpressions: string;
        commendablePractices: string;
    }
    interface ITwoFactor {
        id: string;
        code: string;
    }
    interface IGuidMetaData {
        id: string;
        name: string;
    }

    interface IMetaData {
        id: number;
        name: string;
    }

    interface INetcordMembershipType {
        id: string;
        name: string;
    }

    interface ISiteType {
        version: IApplicationVersion;
        hasType: boolean;
        hasTypeString: string;
        versionTitle: string;
        notApplicables: Array<INotApplicables>;
    }

    interface ISiteApp {
        site: string;
        siteId: number;
        fullSite: ISite,
        types: Array<ISiteType>;
        isEditMode: boolean;
        isStrong?: boolean;
        standards?: string;
    }

    interface IAccessToken {
        accessToken: string;
        expirationDate: Date;
        vaultId: string;
        userId: string;
    }

    interface ITrueValueResponse {
        response: ITrueVaultBlobResponse;
        fileName: string;
        originalFileName: string;
        replacementOf: string;
    }

    interface ITrueVaultBlobResponse {
        blob_filename: string;
        blob_id: string;
        blob_size: string;
        result: string;
        owner_id: string;
        transaction_id: string;
    }

    export interface ICoordinatorApplication {
        organizationId: number;
        organizationName: string;
        location: string;
        applicationTypeId: number;
        applicationTypeName: string;
        coordinatorId: string;
        coordinator: string;
        inspectionScheduleInspectionDate?: Date;
        applicationDueDate: Date;
        outcomeStatusName: string;
        applicationStatusId: number;
        applicationStatusName: string;
        applicationId: number;
        complianceApplicationId: string;
        applicationVersionTitle: string;
        applicationUniqueId: string;
        inspectionDateString?: string;
        dueDateString?: string;
        applicationIsActive?: boolean;
    }

    export interface IUser {
        userId?: string;
        type?: string;
        firstName?: string;
        lastName?: string;
        fullName?: string;
        emailAddress?: string;
        preferredPhoneNumber?: string;
        extension?: string;
        workPhoneNumber?: string;
        role?: IRole;
        organizations?: Array<IUserOrganization>;
        orgs?: string;
        isLocked?: boolean;
        isActive?: boolean;
        webPhotoPath?: string;
        emailOptOut?: boolean;
        mailOptOut?: boolean;
        resumePath?: string;
        statementOfCompliancePath?: string;
        agreedToPolicyDate?: Date;
        annualProfessionHistoryFormPath?: string;
        medicalLicensePath?: string;
        medicalLicenseExpiry?: Date;
        completedStep2?: boolean;
        isAuditor?: boolean;
        isObserver?: boolean;
        languages?: Array<ILanguage>;
        jobFunctions?: Array<IJobFunction>;
        memberships?: Array<IUserMembership>;
        credentials?: Array<services.ICredential>;
        userType?: string;
        isImpersonation?: boolean;
        canManageUsers?: boolean;
        documentLibraryAccessToken?: string;
        code?: string;
        title?: string;
    }

    export interface IUserOrganization {
        organization: IOrganization;
        jobFunction: IJobFunction;
    }

    export interface IRfiViewItem {
        rfisBeforeInspection:number,
        rfisAfterInspection: number,
        totalRFIStandards: number,
        totalRFIs: number,
        totalSites: number,
        siteApplicationSection: Array<ISiteApplicationSection>;
    }

    export interface ISiteApplicationSection {
        siteItem: ISite;
        applicationSectionItem: Array<IApplicationSection>;
    }

    export interface IRole {
        roleId: number;
        roleName: string;
    }

    export interface IOrganization {
        organizationId: number;
        organizationFormattedId?: string;
        organizationName: string;
        organizationNumber: string;
        organizationPhoneUS: string;
        organizationPhoneUSExt: number;
        organizationPhone: string;
        organizationPhoneExt: number;
        organizationFaxUS: string;
        organizationFaxUSExt: number;
        organizationFax: number;
        oganizationFaxExt: number;
        oganizationEmail: string;
        oganizationWebSite: string;
        organizationAddressItem: IAddressItem;
        organizationTypeItem: IOrganizationTypeItem;
        facilities: Array<IOrganizationFacility>;
        organizationDirectors: IUser[];
        primaryUser: IUser;
        accreditationStatusItem: IAccreditationStatus;
        streetAddress1?: string;
        streetAddress2?: string;
        city?: string;
        zip?: string;
        stateProvince?: string;
        country?: string;
        baaOwnerItem: IBAAOwner;
        baaExecutionDate: string;
        baaDocumentVersion: string;
        baaVerifiedDate: string;
        accreditationDate: string;
        accreditationExpirationDate: string;
        accreditationExtensionDate: string;
        accreditedSince: string;
        comments: string;
        facilityItems: Array<IFacility>;
        eligibilityApplicationUniqueId?: string;
        complianceApplicationUniqueId?: string;
        renewalApplicationUniqueId?: string;
        applicationUniqueId?: string;
        cycleNumber?: string;
        cycleEffectiveDate?: string;
        description?: string;
        spatialRelationship?: string;
        baaDocumentPath?: string;
        //netcordMemberships?: IOrgNetcordMembership[]; 
        //newNetcordMembership?: IOrgNetcordMembership;
        bAADocumentItems?: IOrganizationBAADocumentItem[];
        documentLibraryVaultId?: string;
        useTwoYearCycle?: boolean;
        facilityDirectors?: string[];
        accreditationStatusName?: string;
        ccEmailAddresses?: string;
    }

    export interface IOrganizationBAADocumentItem {
        id: string;
        organizationId: number;
        documentId: string;
        documentItem: IDocument;
    }

    export interface IOrgNetcordMembership {
        id?: string;
        netcordMembershipType?: INetcordMembershipType;
        membershipDate?: Date;
        isCurrent?: boolean;
    }

    export interface IOrganizationTypeItem {
        organizationTypeId: number;
        organizationTypeName: string;
    }

    export interface ITemplate {
        id?: string;
        name: string;
        text: string;
    }

    export interface IFacility {
        facilityId: number;
        facilityName: string;
        facilityNumber?: string;
        facilityAddress: string;
        isActive: boolean;
        facilityDirectorId?: string;
        otherSiteAccreditationDetails: string;
        maxtrixMax: string;
        qmRestrictions: boolean;
        netCordMembership: boolean;
        hrsa: boolean;
        masterServiceTypeId?: number;
        netcordMembershipTypeId?: string;
        facilityAccreditationId?: number;
        provisionalMembershipDate: Date;
        associateMembershipDate: Date;
        fullMembershipDate: Date;
        serviceTypeId: number;
        cbCollectionSiteType: string;
        facilityAccreditation: Array<IFacilityAccreditationItem>;
        facilityDirector: IUser;
        serviceType: IServiceTypeItem;
        masterServiceTypeItem: IMasterServiceTypeItem;
        masterServiceType: IMasterServiceTypeItem;
        sites: Array<ISite>;
        siteTotals: Array<ISiteTotalItem>;
        primaryOrganizationId?: string;
        primaryOrganizationName?: string;
    }

    export interface IFacilityAccreditationItem {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface IComplianceApplicationApprovalStatus {
        id: string;
        name: string;
    }

    export interface IComplianceApplicationServiceType {
        complianceApplicationSite: IComplianceApplicationSite;
        serviceType: string;
    }

    export interface ICopyComplianceApplicationModel {
        complianceApplicationId: string;
        copyFromSite: string;
        applicationType: string;
        copyToSite: string;
        deleteOriginal: boolean;
        applicationStatus: string;
    }

    export interface IComplianceApplication {
        id?: string;
        organizationId: number;
        organizationName: string;
        accreditationGoal: string;
        applicationStatus: string;
        applicationStatusName?: string;
        inspectionScope: string;
        coordinator: IUser;
        rejectionComments: string;
        accreditationStatus: string;
        complianceApplicationSites?: Array<IComplianceApplicationSite>;
        complianceApplicationServiceTypes?: IComplianceApplicationServiceType[];
        approvalStatus: IComplianceApplicationApprovalStatus;
        applications: Array<IApplication>;
        site: ISite;
        dueDate?: string;
        dueDt?: Date;
        isReinspection?: boolean;
        hasRfi?: boolean;
        hasFlag?: boolean;
        typeDetail?: string;
    }

    export interface IComplianceApplicationSite {
        applications?: Array<IApplication>;
        site: ISite;
        circle?: string;
        inspectionDetails?: IInspectionDetail;
    }

    export interface IComplianceApplicationSiteVersion {
        id: string;
        applicationVersion: IApplicationVersion;
        questionsNotApplicable: Array<ISiteApplicationVersionQuestionNotApplicable>;
    }

    export interface ISiteApplicationVersionQuestionNotApplicable {
        id?: string;
        siteApplicationVersionId: string;
        questionId: string;
    }

    export interface ISiteTotalItem {
        inPatient: number;
        outPatient: number;
        marrowCollection: number;
        apheresisCollection: number;
        processing: number;
        fixed: number;
        nonFixed: number;
        fixedNonFixed: number;
    }


    export interface IMasterServiceTypeItem {
        id: number;
        name: string;
        shortName: string;
    }

    export interface IServiceTypeItem {
        id: number;
        name: string;
        masterServiceTypeId?: number;
        masterServiceTypeItem: IMasterServiceTypeItem;
    }

    export interface ISiteAddressItem {
        id: number;
        siteId: number;
        addressId: number;
        isPrimaryAddress?: boolean;
        addressType: string;
        address: IAddressItem;
    }

    export interface IAddressItem {
        id: number;
        addressType: IAddressType;
        street1: string;
        street2: string;
        city: string;
        state?: IState;
        province: string;
        phone: string;
        zipCode: string;
        country: ICountry;
        logitude: string;
        latitude: string;
        localId: number;
    }

    export interface IJobFunction extends IGuidMetaData {
        selected: boolean;
    }

    export interface IMembership {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface ILanguage {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface IUserMembership {
        membership: IMembership,
        membershipNumber?: string;
        isSelected?: boolean;
    }

    export interface IOrganizationFacility {
        organizationFacilityId: number;
        facilityId: number;
        facilityName: string;
        facilityAddress: string;
        organizationId: number;
        organizationName: string;
        serviceTypeName?: string;
        inspectionDate: string;
        relation: string;
        createdBy: string;
        createdOn: string;
    }

    export interface IOrganizationConsultant {
        organizationConsultantId: number;
        organizationId: number;
        startDate: string;
        endDate: string;
        organizationName: string;
        consultantId: string;
        consultantName: string;
        createdBy: string;
        createdOn: string;
        user: IUser;
    }


    export interface IOrganizationFacilityPage {
        organizationFacilityItems: Array<IOrganizationFacility>;
        organizationItem: Array<IOrganization>;
        facilityItems: Array<IFacility>;
    }

    export interface INotApplicables {
        id?: string;
        applicationId: number;
        questionId: string;
    }

    export interface ICacheStatus {
        objectName: string;
        lastChangeDate: Date;
    }

    export interface IApplication {
        applicationId?: number;
        applicationTypeId: number;
        applicationTypeName?: string;
        applicantApplicationStatusName?: string;
        applicationStatusId?: number;
        applicationStatusName?: string;
        organizationName?: string;
        organizationId?: number;
        currentCycleNumber: number;
        cycleNumber: number;
        createdBy?: string;
        color?: string;
        uniqueId?: string;
        coordinator?: IUser;
        primarySite?: ISite;
        createdDate?: Date;
        inspectionDate?: Date;
        rfiDueDate?: string;
        dueDate?: Date;
        inspectionDateString?: string;
        createdDateString?: string;
        dueDateString?: string;
        outcomeStatusName?: string;
        applicationVersionTitle?: string;
        applicationVersionId?: string;
        complianceApplicationId?: string;
        notApplicables?: Array<INotApplicables>;
        sections?: Array<IHierarchyData>;
        allQuestions?: Array<IQuestion>;
        accessType?: string;
        site?: ISite;
        isInspectorComplete?: boolean;
        submittedDate?: Date;
        submittedDateString: string;
        applicationsWithRfis?: IApplicationWithRfi[];
        complianceApproval?: ISubmittedCompliance;
        showAccredReport?: boolean;
        hasOutcome?: boolean;
        open?: boolean;
        updatedDate?: Date;
        hasQmRestrictions?: boolean;
        circle?: string;
        standards?: string;
        typeDetail?: string;
    }

    export interface IApp {
        uniqueId: string;
        sections: IHierarchyData[];
    }

    export interface IApplicationWithRfi {
        uniqueId: string;
        complianceAppId: string;
        applicationSectionId: string;
        siteId: number;
        status: string;
        requirementNumber: string;
    }

    export interface IApplicationType {
        applicationTypeId: number;
        applicationTypeName: string;
        isManageable?: boolean;
    }

    export interface ISiteQuestionResponse {
        siteItems: ISite;
        questionResponse: Array<IQuestionResponse>;
    }

    export interface IOverview {
        type: string;
        typeName: string;
        accreditedSince?: Date;
        inspectionDate: string;
        standards: string;
        accreditationGoal: string;
        inspectionScope: string;
    }

    export interface IPersonnel {
        name: string;
        jobFunction: string;
        showOnAccReport: boolean;
        overrideJobFunctionId: string;
    }

    export interface IStatistics {
        siteName: string;
        asOfDate?: Date;
        relatedBanked: number;
        relatedReleased: number;
        relatedOutcome: number;
        unrelatedBanked: number;
        unrelatedReleased: number;
        unrelatedOutcome: number;
    }

    export interface IInspectionTeamMember {
        role: string;
        name?: string;
        names: Array<string>;
        site?: string;
        email?: string;
    }

    export interface ICoordinatorViewItem {
        overview: IOverview;
        personnel: IPersonnel[];
        statistics: IStatistics;
        complianceApplication: IComplianceApplication;
        inspectionTeamMembers: Array<IInspectionTeamMember>;
    }

    export interface IInspectionSchedule {
        inspectionScheduleId: string;
        organizationId: string
        applicationId: string
        organizationName: string;
        applicationTypeId: string;
        applicationTypeName: string;
        inspectionDate: string;
        isCompleted: string;
        completedDate: string;
        startDate: string;
        endDate: string;
        isArchive?: boolean;
        siteId?: number;
        siteName: string;
        complianceApplicationId: string;
        appUniqueId: string;
    }

    export interface IScopeType {
        scopeTypeId: number;
        name: string;
        importName: string;
        isActive: boolean;
        isArchived: boolean;
        isSelected?: boolean;
    }

    export interface IInspectionSchedulePage {
        applicationTypeItem: Array<IApplicationType>;
        organizationItem: Array<IOrganization>;
    }

    export interface IInspectionScheduleDetail {
        inspectionScheduleDetailId: number;
        userId: string;
        fullName: string;
        roleId: number;
        roleName: string;
        inspectionCategoryId: number;
        inspectionCategoryName: string;
        facilityId: number;
        facilityName: string;
        isLead: boolean;
        isMentor: boolean;
        isArchive?: boolean;
        isInspectionComplete: boolean;
        inspectorCompletionDate?: Date;
        mentorFeedback?: string;
        reviewedOutcomesData?: boolean;
        isClinical?: boolean;
        user: IUser;
        siteName: string;
        roleText?: string;
        isOverridden?: boolean;
    }

    export interface IInspectionScheduleDetailPage {
        inspectionScheduleId: number;
        inspectionDate: string;
        applicationTypeId: number;
        applcattionId: number;
        archiveExist: boolean;
        weakFacilities: Array<IOrganizationFacility>;
        selectedFacilities: Array<IOrganizationFacility>;
        users: Array<IUser>;
        roles: Array<IAccreditationRole>;
        inspectionCategories: Array<IInspectionCategory>;
        inspectionScheduleDetailItems: Array<IInspectionScheduleDetail>;
        facilitySites: Array<app.services.IFacilitySite>;
    }

    export interface IAccreditationRole {
        accreditationRoleId: number;
        accreditationRoleName: string;
    }

    export interface IInspectionCategory {
        inspectionCategoryId: number;
        inspectionCategoryName: string;
    }

    export interface IApplicationSetting {
        id: number;
        name: string;
        value: string;
    }

    export interface IApplicationResponseStatusItem {
        id: number;
        name: string;
        nameForApplicant: string;
        description?: string;
    }

    export interface IApplicationVersion {
        id?: string;
        title: string;
        versionNumber: string;
        isActive: boolean;
        copyFromId?: string;
        applicationType: IApplicationType;
        applicationSections?: Array<IApplicationSection>;
        isShown?: boolean;
    }

    export interface IApplicationSection {
        id?: string;
        isActive?: boolean;
        name?: string;
        comments?: string;
        partNumber?: string;
        uniqueIdentifier?: string;
        status?: string;
        isVariance?: boolean;
        helpText?: string;
        version?: string;
        order: string;        
        children?: Array<IApplicationSection>;
        questions?: Array<IQuestion>;
        scopeTypes?: Array<services.IScopeType>;
        parentId?: string;
        applicationTypeName?: string;
        versionId?: string;
        appUniqueId?: string;
        isVisible?: boolean;
        circleStatusName?: string;
        circle?: string;
        statusName?: string;
        parent?: IApplicationSection;
        applicationSectionId?: string;
    }

    export interface IQuestionType {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface IQuestionTypeFlags {
        textArea: boolean;
        radioButtons: boolean;
        checkboxes: boolean;
        documentUpload: boolean;
        date: boolean;
        dateRange: boolean;
        peoplePicker: boolean;
        textBox: boolean;
    }

    export interface IQuestionResponse {
        id?: number;
        document?: IDocument;
        fromDate?: string;
        toDate?: string;
        userId?: string;
        user?: IUser;
        otherText?: string;
        coordinatorComment?: string;
        updatedBy?: IUser;
        updatedDate?: Date;
    }

    export interface IApplicationResponseComment {
        applicationResponseCommentId: number;
        applicationId?: number;
        questionId?: string;
        rfiComment: string;
        citationComment: string;
        coordinatorComment: string;
        commentType: ICommentType;
        fromUser: string;
        ToUser: string;
        documentId: string;
        comment: string;
        commentOverride: string;
        commentFrom: IUser;
        commentTo: string;
        document: IDocument;
        createdBy: string;
        createdDate: string;
        createdDte: Date;
        updatedBy: string;
        updatedDate: string;
        updatedDt?: Date;
        selected?: boolean;
        answerResponseStatusId?: number;      
        isOverridden?: boolean;  
        includeInReporting: boolean;
        visibleToApplicant: boolean;        
        commentDocuments: IDocument[];
    }

    export interface IQuestion {
        id?: string;
        text?: string;
        type?: string;
        flag?: boolean;
        comments?: string;
        commentLastUpdatedBy?: string;
        commentDate?: Date;
        orgComment?: string;
        answerResponseStatusId?: number;
        answerResponseStatusName?: string;
        visibleAnswerResponseStatusId?: number;
        visibleAnswerResponseStatusName?: string;
        commentType?: ICommentType;        
        commentsEntered?: boolean;
        description?: string;
        complianceNumber?: number;
        answers: Array<IAnswer>;
        leftAnswers?: IAnswer[];
        rightAnswers?: IAnswer[];
        sectionId?: string;
        sectionName?: string;
        sectionUniqueIdentifier?: string;
        order?: number;
        isCollapsed?: boolean;
        scopeTypes?: Array<IScopeType>;
        isHidden?: boolean;
        hiddenBy?: Array<IQuestionAnswerDisplay>;
        questionTypeFlags?: IQuestionTypeFlags;
        questionResponses?: Array<IQuestionResponse>;
        applicationResponseComments?: Array<IApplicationResponseComment>;
        responseCommentsRFI?: Array<IApplicationResponseComment>;
        responseCommentsCitation?: Array<IApplicationResponseComment>;
        responseCommentsSuggestion?: Array<IApplicationResponseComment>;
        responseCommentsCoordinator?: Array<IApplicationResponseComment>;//coordinator = fact response
        responseCommentsFactResponse?: Array<IApplicationResponseComment>;
        responseCommentsFactOnly?: Array<IApplicationResponseComment>;
        isNotApplicable?: boolean;
        responseComment?: string;
        file?: string;
        fileName?: string;
        documentId?: string; 
        applicationResponseCommentId?: number;
        siteQuestionResponse?: Array<ISiteQuestionResponse>;
        editor?: any;
        editIndex?: number;
        editMode?: boolean;
        statusChanged?: boolean;
        documents?: IDocument[];
    }

    export interface IAnswer {
        id: string;
        active: boolean;
        selected?: boolean;
        text: string;
        isExpectedAnswer: boolean;
        order?: number;
        questionId?: string;
        hidesQuestions?: Array<IQuestionAnswerDisplay>;
    }

    export interface IQuestionAnswerDisplay {
        id: string;
        questionId: string;
        answerId: string;
        requirementNumber: string,
        complianceNumber?: number,
        questionText: string;
        hiddenByQuestionId?: string;
    }

    export interface IDocument {
        id: string;
        requestValues: string;
        name: string;
        originalName: string;
        hasResponses: boolean;
        associationTypes: string;
        createdDate: string;
        createdBy?: string;
        organizationName?: string;
        isBaaDocument?: boolean;
        isSelected?: boolean;
        includeInReporting?: boolean;
        appUniqueId: string;
        vaultId?: string;
        isLatestVersion?: boolean;
        isLatestVersionString?: string;
    }

    export interface ISectionDocument {
        siteName: string;
        requirement: string;
        req?: string;
        document: IDocument;
        requirementId: string;
    }

    export interface IServiceResponse {
        hasError: boolean;
        message: string;
    }

    export interface IGenericServiceResponse<T> {
        hasError: boolean;
        message: string;
        item: T;
    }

    export interface IApplicationData {
        hierarchyData: IApplicationHierarchyData;
        isFlagged: boolean;
        isComplete: boolean;
    }

    export interface IApplicationHierarchyData {
        partNumber?: string;
        uniqueIdentifier?: string;
        name?: string;
        hasChildren?: boolean;
        status?: string;
        children?: Array<IApplicationHierarchyData>;
        items?: Array<IApplicationHierarchyData>;
        questions?: Array<services.IQuestion>;
        parentName?: string;
        id?: string;
        helpText?: string;
        isVariance: boolean;
        version?: string;
        order: string;
        parentId?: string;
        applicationTypeName?: string;
        scopeTypes?: Array<services.IScopeType>;
        versionId?: string;
        isNotApplicable?: boolean;
    }

    export interface IAuditLog {
        auditLogId: number;
        userName: string;
        ipAddress: string;
        dateTime: string;
        description: string;
    }

    export interface IAddressType extends IMetaData {
    }

    export interface IState extends IMetaData {
    }

    export interface ICountry extends IMetaData {
    }

    export interface IClinicalType {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface IProcessingType {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface ICollectionProductType extends IMetaData {
    }

    export interface ICBCollectionType extends IMetaData {
    }

    export interface ICBUnitType extends IMetaData {
    }

    export interface IClinicalPopulationType extends IMetaData {
    }

    export interface ITransplantType {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface ICredential {
        id: number;
        name: string;
        isSelected?: boolean;
    }

    export interface IFlatSite {
        siteId: number;
        siteName: string;
        siteStartDate: Date;
        facilityId: number;
        facilityName: string;
        organizationId: number;
        organizationName: string;
    }

    export interface ISite {
        siteId?: number;
        siteName?: string;
        siteStartDate?: string;
        startDate?: Date;
        sitePhone?: string;
        siteStreetAddress1?: string;
        siteStreetAddress2?: string;
        siteCity?: string;
        siteZip?: string;
        siteState?: IState;
        siteProvince?: string;
        siteCountry?: ICountry;
        siteIsPrimarySite?: boolean;
        isPrimaryAddress?: boolean;
        siteAddresses?: Array<services.ISiteAddressItem>;
        transplantTypes?: Array<services.ITransplantType>;
        clinicalTypes?: Array<services.IClinicalType>;
        processingTypes?: Array<services.IProcessingType>;
        siteCollectionProductType?: ICollectionProductType;
        siteClinicalPopulationType?: IClinicalPopulationType;
        siteCBCollectionType?: ICBCollectionType;
        siteCBUnitType?: ICBUnitType;
        siteUnitsBanked?: number;
        siteUnitsBankedDate?: string;
        scopeTypes?: Array<services.IScopeType>;
        siteFacilityId?: number;
        siteDescription?: string;
        siteInspectionDate?: string;
        rfiInSite?: number;
        facilities?: Array<IFacility>;
        siteCordBloodTransplantTotals?: Array<ISiteCordBloodTransplantTotal>;
        siteTransplantTotals?: Array<ISiteTransplantTotal>;
        siteCollectionTotals?: Array<ISiteCollectionTotal>;
        siteProcessingTotals?: Array<ISiteProcessingTotal>;
        siteProcessingMethodologyTotals?: Array<ISiteProcessingMethodologyTotal>;
        siteTypes?: string;
        isStrong?: boolean;
    }

    export interface ISiteTransplantTotal {
        id?: string;
        siteId: number;
        transplantCellType: ITransplantCellType;
        clinicalPopulationType: IClinicalPopulationType;
        transplantType: ITransplantType;
        isHaploid: boolean;
        numberOfUnits?: number;
        startDate: string;
        endDate: string;
    }

    export interface ISiteCollectionTotal {
        id?: string;
        siteId: number;
        collectionType: ICollectionType;
        clinicalPopulationType: IClinicalPopulationType;
        numberOfUnits?: number;
        startDate: string;
        endDate: string;
    }

    export interface ISiteProcessingTotal {
        id?: string;
        siteId: number;
        siteProcessingTotalTransplantCellTypes: Array<ISiteProcessingTotalTransplantCellType>;
        selectedTypes: Array<string>;
        types?: string;
        numberOfUnits?: number;
        startDate: string;
        endDate: string;
    }

    export interface ISiteProcessingTotalTransplantCellType {
        id?: string;
        siteProcessingTotalId?: string;
        transplantCellType: ITransplantCellType;
    }

    export interface ISiteProcessingMethodologyTotal {
        id?: string;
        siteId: number;
        processingType: IProcessingType;
        platformCount?: number;
        protocolCount?: number;
        startDate: string;
        endDate: string;
    }

    export interface IFacilitySite {
        organizationId: number;
        organizationName: number;
        relation: string;
        facilitySiteId: number;
        facilityId: number;
        facilityName: string;
        siteId: number;
        siteName: string;
        createdBy: string;
        createdOn: string;
        inspectionDate: string;
        isOverridden: boolean;
    }

    export interface ICollectionType {
        id: string;
        name: string;
    }

    export interface IFacilitySitePage {
        facilitySiteItems: Array<IFacilitySite>;
        siteItems: Array<ISite>;
        facilityItems: Array<IFacility>;
    }
    
    export interface IApplicationStatus {
        id: number;
        name: string;
        nameForApplicant: string;
    }

    export interface IApplicationStatusHistory {
        id: number;
        applicationId: string;
        applicationStatusOld: IApplicationStatus;
        applicationStatusNew: IApplicationStatus;
        createdDate: string;
        createdBy: string;
    }

    export interface IAccreditationStatus {
        id: string;
        name: string;
    }

    export interface ICommentType {
        id: string;
        name: string;
    }

    export interface IBAAOwner {
        id: string;
        name: string;
    }

    export interface ICommentsResult {
        commentEntered: boolean;
        totalComments: number;
    }

    export interface IOutcomeStatus extends IMetaData {
    }

    export interface IReportReviewStatus extends IMetaData {
    }

    export interface IAccreditationOutcome {
        id: number;
        organizationId: number;
        organizationName: string;
        applicationId: number;
        applicationName: string;
        outcomeStatusId: number;
        outcomeStatusName: string;
        reportReviewStatusId: number;
        reportReviewStatusName: string;
        committeeDate: string;
        createdBy: string;
        createdDate: string;
        sendEmail: boolean;
        dueDate: Date;
    }

    export interface IInspection {
        id: number;
        applicationId?: number;
        siteApplicationVersionId?: number;
        inspectorId: number;
        commendablePractices: string;
        overallImpressions: string;
        siteDescription: string;
        allSitesWithDetails?: boolean;
        user: IUser;
        isReinspection: boolean;
    }

    export interface ISectionItemStatus {
        applicationSectionId: string;
        hasAttachments: boolean;
        hasCitationNotes: boolean;
        hasSuggestions: boolean;
        hasRFIComments: boolean;
        hasFACTOnlyComments: boolean;         
        isFlag:boolean;
        statusName: string;
        uniqueIdentifier: string;
    }

    export interface ISectionRootItem {
        applicationSectionId: string;
        name: string;
        uniqueIdentifier: string;
        items: Array<ISectionItemStatus>;
    }

    export interface IApplicationStatusView {
        applicationStatus: string;
        coordinator?: IUser;
        dueDate: Date;
        inspectionDate: Date;
        organizationName: string;
        submittedDate: string;
        applicationSectionRootItems: Array<ISectionRootItem>;
    }

    export interface IApplicationResponseStatus {
        applicationStatus: string;
        coordinator?: IUser;
        dueDate: Date;
        inspectionDate: Date;
        updatedBy: string;
        updatedDate: string;
        applicationSectionRootItems: Array<ISectionRootItem>;
    }

    export interface IHierarchyData {
        partNumber: string;
        name: string;
        hasChildren: boolean;
        status?: string;
        helpText?: string;
        children?: Array<IHierarchyData>;
        items?: Array<IHierarchyData>;
        nextSection?: IHierarchyData;
        parent?: IHierarchyData;
        questions: Array<services.IQuestion>;
        parentName?: string;
        uniqueIdentifier: string;
        id: string;
        statusName?: string;
        isVisible: boolean;
        appUniqueId: string;
        hasRfi?: boolean;
        circleStatusName?: string;
        circle?: string;
        hasFlags?: boolean;
    }

    export interface ICbCategory {
        id: number;
        name: string;
    }

    export interface ISiteCordBloodTransplantTotal {
        id?: number;
        siteId: number;
        cbUnitType: ICBUnitType;
        cbCategory: ICbCategory;
        numberOfUnits?: number;
        asOfDate: string;
        asOfDateUnformatted?: moment.Moment;
    }

    export interface ITransplantCellType {
        id: string;
        name: string;
    }

    export interface IEmailTemplate {
        id: string;
        name: string;
        html: string;
        to?: string;
        cc?: string;
        subject?: string;
    }

    export interface IReport {
        name: string;
        url: string;
    }

    export interface IRequestAccess {
        serviceType: string;
        organizationName: string;
        organizationAddress: string;
        directorName: string;
        directorEmailAddress: string;
        primaryContactName: string;
        primaryContactEmailAddress: string;
        primaryContactPhoneNumber: string;
        masterServiceTypeOtherComment: string;
        otherComments: string;
    }

    export interface ISubmittedCompliance
    {
        submittedComplianceId: string;
        orgDirectorId: string;
    }

    export interface ICtTotal {
        siteId: number;
        siteName: string;
        total: string;
        dateRange: string;
    }

    export interface ICbTotal {
        siteName: string;
        category: string;
        unitType: string;
        numberOfUnits: number;
        asOfDate: Date;
    }

    export interface ICibmtrDataMgmt {
        id?: string;
        cibmtrId: string;
        auditDate?: Date;
        criticalFieldErrorRate?: number;
        randomFieldErrorRate?: number;
        overallFieldErrorRate?: number;
        isCapIdentified?: boolean;
        auditorComments?: string;
        cpiLetterDate?: Date;
        orderDate?: Date;
        cpiTypeId?: string;
        cpiTypeName?: string;
        cpiComments?: string;
        correctiveActions?: string;
        factProgressDetermination?: string;
        isAuditAccuracyRequired?: boolean;
        additionalInformation?: string;
        progressOnImplementation?: string;
        inspectorInformation?: string;
        inspectorCommendablePractices?: string;
    }

    export interface ICibmtrOutcomeAnalysis {
        id?: string;
        cibmtrId: string;
        reportYear?: number;
        survivalScore?: number;
        sampleSize?: number;
        actualPercent?: number;
        predictedPercent?: number;
        lowerPercent?: number;
        upperPercent?: number;
        comparativeDataSource?: string;
        publishedOneYearSurvival?: string;
        programOneYearSurvival?: string;
        comments?: string;
        reportedCausesOfDeath?: string;
        correctiveActions?: string;
        factImprovementPlan?: string;
        additionalInformationRequested?: string;
        progressOnImplementation?: string;
        inspectorInformation?: string;
        inspectorCommendablePractices?: string;
        inspector100DaySurvival?: string;
        inspector1YearSurvival?: string;
        isNotRequired?: boolean;
    }

    export interface ICibmtr {
        id?: string;
        centerNumber?: string;
        ccnName?: string;
        transplantSurvivalReportName?: string;
        displayName?: string;
        isNonCibmtr: boolean;
        facilityName: string;
        cibmtrOutcomeAnalyses?: ICibmtrOutcomeAnalysis[];
        cibmtrDataMgmts?: ICibmtrDataMgmt[];
        isActive
    }

    export interface IRfi {
        requirementNumber: string;
        questionNumber: number;
        comment: string;
    }

    //status view
    interface IFilter {
        filterName: string;
        isChecked: boolean;
    }

    //coordinator view
    interface ITotal {
        siteName: string;
        unitType: string;
        categories: ICategory[];
    }

    interface ICategory {
        category: string;
        numberOfUnits: number;
        asOfDate: Date;
    }
    //answer review base
    interface ICommentQuestion {
        questionId: string;
    }
    
    interface IValues {
        section: Eligibility.IApplicationHierarchyData;
        appUniqueId: string;
        isUser: boolean;
        questionId?: string;
        accessToken: services.IAccessToken;
    }

    interface IApplicationReport {
        applicationId: number;
        applicationSectionId?: string;
        hasSection: number;
        text: string;
        response: string;
        comments: string;
        type: string;
    }

    interface ICompAppInspectionDetail {
        id?: string;
        complianceApplicationId: string;
        inspectionScheduleId?: number;
        inspectionScheduleIdString?: string;
        inspectorsNeeded?: number;
        clinicalNeeded?: number;
        adultSimpleExperienceNeeded?: boolean;
        adultMediumExperienceNeeded?: boolean;
        adultAnyExperienceNeeded?: boolean;
        pediatricSimpleExperienceNeeded?: boolean;
        pediatricMediumExperienceNeeded?: boolean;
        pediatricAnyExperienceNeeded?: boolean;
        comments?: string;
    }
}
