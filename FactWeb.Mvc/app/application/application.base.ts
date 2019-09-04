module app.application {
    'use strict';

    export abstract class ApplicationBase {
        protected rootScope: ng.IRootScopeService;
        protected config: IConfig;
        protected common: common.ICommonFactory;
        protected location: ng.ILocationService;
        protected documentService: services.IDocumentService;
        protected notificationFactory: blocks.INotificationFactory;
        protected applicationService: services.IApplicationService;

        protected application: services.IApplication; //current application instance
        protected compApplication: services.ICompApplication; //current compliance application instance        
        protected selectedItem: services.IHierarchyData;
        protected requirements: services.IHierarchyData[]; //equal to application.sections        
        /*REMOVE*/protected questions: Array<services.IQuestion> = []; //ToDo: We can potentially remove this.
        protected rfis: services.IRfi[] = [];
        protected appUniqueId: string = ""; //application unique id
        protected compAppUniqueId: string = ""; //compliance application unique id

        protected accessToken: services.IAccessToken = null; //DL access token.
        protected accessType: string = ""; //used to authenticate current application request
        protected orgName: string = ""; //string name of organization
        protected organization: services.IOrganization; //full organization
        protected applicationType = ""; //type of application

        protected isFlagged: boolean = false;  //Any question is flagged
        protected isComplete: boolean = false; //If application is complete
        protected agreeTerms: boolean = false; //agree flag when submitting an application
        protected isComplianceApplication: boolean = false;
        protected hasRfi: boolean = false; //any section has an RFI.
        protected hasRfiResponse: boolean = false; //RFI response was sent
        protected allReviewed: boolean = true; //All answers are reviewed or not ?

        protected isUser: boolean = false;
        protected isLead = false;
        protected template: services.IEmailTemplate;
        protected annualAppCompleteTemplate: services.IEmailTemplate;
        protected urlSetting: services.IApplicationSetting;

        constructor(
            private $q: ng.IQService,
            private timeout: ng.ITimeoutService,
            rootScope: ng.IRootScopeService,
            location: ng.ILocationService,
            documentService: services.IDocumentService,
            applicationService: services.IApplicationService,
            config: IConfig,
            common: common.ICommonFactory,
            notificationFactory: blocks.INotificationFactory) {

            var _this = this;
            this.rootScope = rootScope;
            this.location = location;
            this.documentService = documentService;
            this.applicationService = applicationService;
            this.config = config;
            this.common = common;
            this.notificationFactory = notificationFactory;

            this.rootScope.$on(this.config.events.userLoggedIn, (data: any, args: any) => {
                _this.init();
            });
            this.rootScope.$on(this.config.events.reloadSections, (data: any, args: any) => {
                //_this.onReloadSection(args.section);
            });

            this.appUniqueId = this.location.search().app;
            this.compAppUniqueId = this.location.search().c;
            if (this.compAppUniqueId) this.isComplianceApplication = true;
        }

        abstract init(): void;
        abstract onAccessGranted(): void;
        abstract processSection(section: services.IApplicationSection, isRoot: boolean, hasOutcome?: boolean, parentPart?: string, parentName?: string): services.IHierarchyData;
        abstract onApplicationDetailsLoaded(): void;
        abstract onApplicationSectionsLoaded(): void;
        abstract onCompApplicationLoaded(): void;
        abstract onReloadSection(section: services.IHierarchyData): void; //Use this override if all sections need to be reloaded

        //processAppChanges(args: any) {
        //    if (!this.compApplication  || !this.compApplication.complianceApplicationSites) return;

        //    for (var i = 0; i < this.compApplication.complianceApplicationSites.length; i++) {
        //        var appSite = this.compApplication.complianceApplicationSites[i];
        //        var hasChanges = false;

        //        for (var j = 0; j < appSite.applications.length; j++) {
        //            var app = appSite.applications[j];

        //            var row = _.find(app.sections, (s) => {
        //                return this.common.containsRow(s, args.rowId, false);
        //            });

        //            if (row) {
        //                this.common.getCircleColorForSections(app.sections, this.common.isFact());
        //                hasChanges = true;
        //            }
        //        }

        //        if (hasChanges) {
        //            this.common.getCircleForSite(appSite.applications, this.common.isFact());
        //        }
        //    }
        //}

        protected processResponseUpdates(responses: services.IApplicationSectionResponse[], updatedResponses: services.IApplicationSectionResponse[], nextSection: services.IApplicationSectionResponse): services.IApplicationSectionResponse[] {
            _.each(responses, (response) => {
                var foundResponse = _.find(updatedResponses, (updatedResponse) => {
                    return response.applicationSectionId === updatedResponse.applicationSectionId;
                });

                if (foundResponse) {
                    if (foundResponse.sectionStatusName !== response.sectionStatusName) {
                        response.sectionStatusName = foundResponse.sectionStatusName;
                    }

                    if (foundResponse.isVisible !== response.isVisible) {
                        response.isVisible = foundResponse.isVisible;    
                    }

                    if ((response.nextSection != null || foundResponse.nextSection != null) &&
                        ((response.nextSection == null && foundResponse.nextSection != null) ||
                        (foundResponse.nextSection == null && response.nextSection != null) ||
                        foundResponse.nextSection.applicationSectionUniqueIdentifier !== response.nextSection.applicationSectionUniqueIdentifier)) {
                        response.nextSection = foundResponse.nextSection;    
                    }

                    if (nextSection && nextSection.applicationSectionId === response.applicationSectionId) {
                        nextSection = response;
                    }

                    if (response.children && response.children.length > 0) {
                        response.children = this.processResponseUpdates(response.children, foundResponse.children, nextSection);
                    }
                }
            });

            return responses;
        }

        protected checkValues() {
            //this.rootScope.$on('AppChanged', (data: any, args: any) => {
            //    this.processAppChanges(args);
            //});

            //this.rootScope.$on('BulkChanges', (data: any, args: any) => {
            //    this.processAppChanges(args);
            //});

            this.common.checkItemValue(this.config.events.emailTemplatesLoaded, this.config.emailTemplates, false)
                .then(() => {
                    this.template = _.find(this.config.emailTemplates, (temp: services.IEmailTemplate) => {
                        return temp.name === "Send Back For RFI";
                    });

                    this.annualAppCompleteTemplate = _.find(this.config.emailTemplates, (temp: services.IEmailTemplate) => {
                        return temp.name === "Annual Application Complete";
                    });
                });

            this.common.checkItemValue(this.config.events.applicationSettingsLoaded, this.common.applicationSettings, false)
                .then(() => {
                    this.urlSetting = _.find(this.common.applicationSettings, (s: services.IApplicationSetting) => {
                        return s.name === "System Base Url";
                    });
                });

            this.common.checkItemValue(this.config.events.applicationLoaded, this.common.application, false)
                .then(() => {
                    this.loadAppDetails();
                });

            this.common.checkItemValue(this.config.events.accessTokenLoaded, this.common.accessToken, false)
                .then(() => {
                    this.accessToken = this.common.accessToken;
                });

            this.common.checkItemValue(this.config.events.organizationLoaded, this.common.organization, false)
                .then(() => {
                    this.organization = this.common.organization;
                    this.common.isDirector(this.common.application.organizationName);
                });

            this.common.checkItemValue(this.config.events.userInspectorLoaded, this.common.isUserLeadInspector, false)
                .then(() => {
                    this.isUser = this.common.isUserLeadInspector;
                });

            this.common.checkItemValue(this.config.events.documentsLoaded, this.common.documents, false)
                .then(() => {
                });

            this.common.checkItemValue(this.config.events.inspectionScheduleDetailsLoaded, this.common.inspectionScheduleDetails, false)
                .then(() => {
                    var isLead = false;
                    if (!this.common.currentUser) {
                        this.rootScope.$on(this.config.events.userLoggedIn, () => {
                            if (this.common.inspectionScheduleDetails != null) {
                                for (var i = 0; i < this.common.inspectionScheduleDetails.length; i++) {
                                    if (this.common.inspectionScheduleDetails[i].userId.replace("'", "").replace("'", "") === this.common.currentUser.userId &&
                                        this.common.inspectionScheduleDetails[i].isLead) {
                                        isLead = true;
                                        break;
                                    }
                                }
                            }

                            this.isLead = isLead;        
                        });
                    } else {
                        if (this.common.inspectionScheduleDetails != null) {
                            for (var i = 0; i < this.common.inspectionScheduleDetails.length; i++) {
                                if (this.common.inspectionScheduleDetails[i].userId.replace("'", "").replace("'", "") === this.common.currentUser.userId &&
                                    this.common.inspectionScheduleDetails[i].isLead) {
                                    isLead = true;
                                    break;
                                }
                            }
                        }

                        this.isLead = isLead;    
                    }

                });

            if (this.isComplianceApplication) {
                this.common.checkItemValue(this.config.events.compAppLoaded, this.common.compApp, true)
                    .then(() => {
                        this.isFlagged = this.common.compApp.hasFlag;
                        this.compApplication = this.common.compApp;
                        this.onCompApplicationLoaded();
                        this.common.hideSplash();
                    });
                //this.common.checkItemValue(this.config.events.complianceApplicationLoaded, this.common.compApplication, true)
                //    .then(() => {
                //        console.log('compApp', this.common.compApplication);

                //        this.isFlagged = this.common.compApplication.hasFlag;
                //        this.compApplication = this.common.compApplication;
                //        this.onCompApplicationLoaded();
                //        this.common.hideSplash();
                //    });
            } else {
                this.common.checkItemValue(this.config.events.applicationSectionsLoaded, this.common.applicationSections, true)
                    .then(() => {
                        console.log('sections', this.common.applicationSections);
                        this.loadApplicationsSections();
                        this.common.hideSplash();
                    });
            }
        }

        loadAppDetails() {

            this.application = this.common.application;
            this.orgName = this.common.application.organizationName;
            this.applicationType = this.common.application.applicationTypeName;

            this.common.$broadcast(this.config.events
                .coordinatorSet,
                { coordinator: this.application.coordinator });

            this.onApplicationDetailsLoaded();
        }

        loadApplicationsSections() {
            var data: services.IHierarchyData[] = [];
            _.each(this.common.applicationSections, (value: services.IApplicationSection) => {
                var row = this.processSection(value, true);
                if (row != null) {
                    data.push(row);
                }
            });
            this.requirements = data;

            this.onApplicationSectionsLoaded();
        }

        //Common Application Controller Functions
        protected checkForAccess(): void {
            var promise = null;
            //this.common.showSplash();
            if (!this.isComplianceApplication) {
                this.applicationService.getApplicationAccess(this.appUniqueId)
                    .then((accessType: string) => {
                        if (accessType != null && accessType !== "") {
                            this.accessType = accessType;
                            this.onAccessGranted();
                        } else {
                            this.common.loginRedirect();
                            //this.common.hideSplash();
                        }
                    })
                    .catch((e) => {
                        this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                        //this.common.hideSplash();
                    })
            }
            else {
                this.applicationService.getComplianceApplicationAccess(this.compAppUniqueId)
                    .then((accessType: string) => {
                        if (accessType != null && accessType !== "") {
                            this.accessType = accessType;
                            this.onAccessGranted();
                        } else {
                            this.common.loginRedirect();
                            this.common.hideSplash();
                        }
                    })
                    .catch((e) => {
                        this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                        this.common.hideSplash();
                    })
            }
        }

        protected getAccessToken(organizationName: string = ""): ng.IPromise<void> {
            if (this.common.accessToken && this.common.accessToken != null) {
                this.accessToken = this.common.accessToken;
            } else {
                var orgName = organizationName === "" ? this.application.organizationName : organizationName;
                return this.documentService.getAccessToken(orgName)
                    .then((data: services.IAccessToken) => {
                        this.accessToken = data;
                    })
                    .catch(() => {
                        this.notificationFactory.error("Unable to get access to document Library. Please contact support.");
                    });
            }
            
        }

        //load simple application details.
        protected getApplicationDetails(): ng.IPromise<void> {

            return this.applicationService.getApp(this.appUniqueId)
                .then((app: services.IApplication) => {
                    this.application = app;
                    this.orgName = app.organizationName;
                    this.applicationType = app.applicationTypeName;

                    this.common.$broadcast(this.config.events.orgSet, { organization: app.organizationName, orgId: app.organizationId });
                    this.common.$broadcast(this.config.events.coordinatorSet, { coordinator: this.application.coordinator });

                    this.onApplicationDetailsLoaded();
                })
                .catch(() => {
                    this.notificationFactory.error("Cannot get application. Please contact support");
                });
        }

        //load application sections (nested)
        protected getApplicationSections(): ng.IPromise<void> {
            return this.applicationService.getAppSections(this.appUniqueId)
                .then((items: Array<services.IApplicationSection>) => {
                    var data: services.IHierarchyData[] = [];
                    _.each(items, (value: services.IApplicationSection) => {
                        var row = this.processSection(value, true);
                        if (row != null) {
                            data.push(row);
                        }
                    });
                    this.requirements = data;
                    
                    this.onApplicationSectionsLoaded();
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        //load compliance application
        //protected getCompApplication(): ng.IPromise<void> {
        //    return this.applicationService.getComplianceApplicationById(this.compAppUniqueId, false)
        //        .then((data: services.IComplianceApplication) => {
        //            this.rfis = [];
        //            _.each(data.complianceApplicationSites, (appSite: services.IComplianceApplicationSite) => {
        //                _.each(appSite.applications, (app: services.IApplication) => {
        //                    app.open = true;
        //                    if (!this.application) {
        //                        //not everytime do we load application object seperately using [getApplicationDetails()]. In case it is not populated earlier, populate the application object here
        //                        if (this.appUniqueId === app.uniqueId) {
        //                            this.application = app;
        //                            this.orgName = this.application.organizationName;
        //                            this.common.$broadcast(this.config.events.orgSet, { organization: app.organizationName, orgId: app.organizationId });
        //                        }
        //                    }
        //                    //var appSections = [];
        //                    //_.each(app.sections, (section: services.IApplicationSection) => {
        //                    //    appSections.push(this.processSection(section, true, app.hasOutcome));
        //                    //});
        //                    //setNextSection is currently only implemented for application submission. Assuming it will be required for reviewer view as well.
        //                    app.sections = this.common.setNextSection(app.sections, app.sections, null);
        //                    //app.sections = appSections;
        //                });
        //            });

        //            this.compApplication = data;
        //            this.common.$broadcast(this.config.events.coordinatorSet, { coordinator: this.compApplication.coordinator });

        //            this.onCompApplicationLoaded();

        //        })
        //        .catch(() => {
        //            this.notificationFactory.error("Error getting compliance application. Please contact support.");
        //        });
        //}

        protected hasQuestionsVisible(section: services.IHierarchyData): boolean {

            if (section.questions == null) return false;

            var found = _.find(section.questions, (question) => {
                return !question.isHidden;
            });

            if (found) {
                return true;
            } else {
                return false;
            }
        }

        protected findAppSection(section: services.IHierarchyData, sectionId: string): services.IHierarchyData {
            if (section.id === sectionId) {
                return section;
            }

            if (section.children) {
                for (var i = 0; i < section.children.length; i++) {
                    var sec = this.findAppSection(section.children[i], sectionId);

                    if (sec != null) {
                        return section.children[i];
                    }
                }
            }

            return null;
        }

        protected findSection(section: services.IApplicationSectionResponse, sectionId: string): services.IApplicationSectionResponse {
            if (section.id === sectionId || section.applicationSectionId === sectionId) {
                return section;
            }

            if (section.children) {
                for (var i = 0; i < section.children.length; i++) {
                    var sec = this.findSection(section.children[i], sectionId);

                    if (sec != null) {
                        return section.children[i];
                    }
                }
            }

            return null;
        }

        protected shouldBeRemoved(section: services.IHierarchyData): boolean {
            var sect = _.cloneDeep(section);

            if (sect.children != null && sect.children.length > 0) {
                for (var i = 0; i < sect.children.length; i++) {
                    if (this.shouldBeRemoved(sect.children[i])) {
                        sect.children.splice(i, 1);
                        i--;
                    }
                }
            }

            if ((sect.children == null || sect.children.length === 0) && !this.hasQuestionsVisible(sect)) {
                return true;
            }

            return false;
        }

        protected checkComplete(reqs: services.IHierarchyData[], isRoot: boolean, isRfiStatus: boolean): boolean {
            if (this.isComplete) return true;

            console.log('reqs', this.requirements);

            if (!reqs) return false;
            for (var i = 0; i < reqs.length; i++) {

                var req = reqs[i];
                if (!req.isVisible) {
                    continue;
                }

                if (req.children && req.children.length > 0) {
                    if (!this.checkComplete(req.children, false, isRfiStatus)) {
                        return false;
                    }
                }

                if (req.questions && req.questions.length > 0) {
                    for (var j = 0; j < req.questions.length; j++) {
                        var question = req.questions[j];

                        var answered = false;//TFS: 1061 - Start with unanswered.

                        if (!question.isHidden) {

                            if (question.answers && question.answers.length > 0 && (question.type === this.config.questionTypes.checkboxes || question.type === this.config.questionTypes.radioButtons)) { //question was a radio OR checkbox
                                var found = _.find(question.answers, (answer: any) => {
                                    return answer.selected === true;
                                });

                                if (found) answered = true;
                            } else { //TFS:1061 - modified the logic to accomodate those scenarios where a text answer is given and then removed afterwards.
                                if (question.questionResponses && question.questionResponses.length > 0) {
                                    if (question.type === this.config.questionTypes.dateRange) {
                                        answered = question.questionResponses[0].fromDate !== "" &&
                                            question.questionResponses[0].toDate !== "";
                                    }
                                    else if (question.type === this.config.questionTypes.peoplePicker) {//check user property
                                        answered = question.questionResponses[0].user ? true : false;
                                    } else if (question.type === this.config.questionTypes.multiple) {
                                        answered = true;
                                        var hasField = false;

                                        if (question.questionTypeFlags.date) {
                                            answered = question.questionResponses[0].fromDate !== "" &&
                                                question.questionResponses[0].toDate !== "";
                                            hasField = true;
                                        }

                                        if (answered && question.questionTypeFlags.peoplePicker) {
                                            answered = question.questionResponses[0].user ? true : false;
                                            hasField = true;
                                        }

                                        if (answered && question.questionTypeFlags.documentUpload) {
                                            answered = question.questionResponses[0].document ? true : false;
                                            hasField = true;
                                        }

                                        if (answered &&
                                        (question.questionTypeFlags.textArea ||
                                            question.questionTypeFlags.textBox ||
                                            question.questionTypeFlags.date)) {
                                            answered = question.questionResponses[0].otherText ? true : false;    
                                            hasField = true;
                                        }

                                        if (!hasField) {
                                            answered = question.questionResponses.length > 1;
                                        }
                                        
                                    } else {
                                        if (question.type === this.config.questionTypes.documentUpload) {
                                            answered = question.questionResponses[0].document ? true : false;
                                        }
                                        else {//check otherText property
                                            answered = question.questionResponses[0].otherText ? true : false;
                                        }
                                    }
                                }
                            }


                            if (!answered || (question.flag && !question.isHidden)) {
                                return false;
                            }

                            if (isRfiStatus && question.answerResponseStatusName === this.config.applicationSectionStatuses.rfi) {
                                if (question
                                    .responseCommentsRFI ==
                                    null ||
                                    question.responseCommentsRFI.length === 0) return false;

                                var submitted = moment(this.application.submittedDate);

                                var f = _.find(question.applicationResponseComments, (c: services.IApplicationResponseComment) => {
                                    var commentDate = moment(c.createdDate);
                                    return c.commentFrom.role.roleId !== this.config.roleIds.factAdministrator && 
                                        c.commentFrom.role.roleId !== this.config.roleIds.factQualityManager && 
                                        c.commentFrom.role.roleId !== this.config.roleIds.factCoordinator && 
                                        c.commentFrom.role.roleId !== this.config.roleIds.inspector && commentDate > submitted;
                                });

                                if (!f) {
                                    return false;
                                }
                            }

                            


                        }
                    }
                }
            }

            return true;
        }

        protected isCompComplete(isRfiStatus: boolean): boolean {

            if (this.isComplete) return true;

            if (!this.compApplication || !this.compApplication.complianceApplicationSites) return false;

            for (var i = 0; i < this.compApplication.complianceApplicationSites.length; i++) {
                var appSite = this.compApplication.complianceApplicationSites[i];
                for (var j = 0; j < appSite.appResponses.length; j++) {
                    var app = appSite.appResponses[j];

                    var found = _.find(app.applicationSectionResponses, (r) => {
                        return r.isVisible &&
                            r.sectionStatusName !== "Complete" &&
                            r.sectionStatusName !== "RFI Complete" &&
                            r.sectionStatusName !== "RFI Completed";
                    });

                    if (found)
                        return false;

                    //if (!this.checkComplete(app.sections, true, isRfiStatus)) {
                    //    return false;
                    //}
                }
            }

            return true;
        }

        //process sections in checklist views of both compliance and non-complaince applications 
        protected processSectionForChecklistView(section: services.IApplicationSection, isRoot: boolean, hasOutcome?: boolean, parentPart?: string, parentName?: string): services.IHierarchyData {
            var row: services.IHierarchyData = {
                partNumber: section.partNumber.toString(),
                name: section.name,
                hasChildren: false,
                id: section.id,
                questions: section.questions,
                uniqueIdentifier: section.uniqueIdentifier,
                statusName: section.status,
                helpText: section.helpText,
                isVisible: true,
                appUniqueId: section.appUniqueId,
                circle: section.circle,
                circleStatusName: section.circleStatusName
            };

            if (section.questions && section.questions.length > 0) {
                _.each(section.questions, (question: services.IQuestion) => {
                    if (question.type === this.config.questionTypes.radioButtons || question.type === this.config.questionTypes.checkboxes)
                        this.questions.push(question);

                    if (question.questionResponses == undefined || question.questionResponses == null || question.questionResponses.length === 0) {
                        question.questionResponses = [{}];
                    }

                    if (question.answerResponseStatusName === this.config.applicationSectionStatuses.rfi) {
                        var submitted = moment(this.application.submittedDate);

                        var found = _.find(question.applicationResponseComments, (c: services.IApplicationResponseComment) => {
                            var commentDate = moment(c.createdDate);
                            return c.commentFrom.role.roleName !== this.config.roles.factAdministrator &&
                                c.commentFrom.role.roleName !== this.config.roles.inspector && commentDate > submitted;
                        });

                        if (!found) {
                            this.hasRfiResponse = false;
                        }

                        if (this.application.applicationStatusId === 9) {
                            this.common.$broadcast(this.config.events.hasRfi, { hasRfi: true });    
                        }
                        
                    }
                });

                if (!row.circle || row.circle === "") {
                    this.common.setCircleApplicantFromQuestions(row);    
                }
            }

            var anyflags = _.some(row.questions, (question: services.IQuestion) => {
                return question.flag && !question.isHidden;
            });

            if (anyflags) {
                this.isFlagged = true;
            }

            if (parentPart) {
                row.partNumber = parentPart + "." + row.partNumber;
            }

            if (parentName) {
                row.parentName = parentName;
            }

            if (section.children && section.children.length > 0) {
                row.hasChildren = true;
                row.children = [];
                _.each(section.children, (value: services.IApplicationSection) => {
                    var child = this.processSectionForChecklistView(value, false, hasOutcome, row.partNumber, row.uniqueIdentifier + ": " + row.name);

                    if (child != null) {
                        row.children.push(child);
                    }
                });
            }

            if (this.shouldBeRemoved(row)) {
                row.isVisible = false;
            } else {
                if (row.children && row.children.length > 0) {
                    if (!row.circle || row.circle === "") {
                        this.common.setCircleApplicantFromChild(row);    
                    }
                }
            }

            return row;
        }
        //process sections in reviewer view
        protected processSectionForReviewerView(section: services.IApplicationSection, isRoot: boolean, hasOutcome?: boolean, parentPart?: string, parentName?: string): services.IHierarchyData {
            var row: services.IHierarchyData = {
                partNumber: section.partNumber.toString(),
                name: section.name,
                hasChildren: false,
                id: section.id,
                questions: section.questions,
                uniqueIdentifier: section.uniqueIdentifier,
                isVisible: true,
                appUniqueId: section.appUniqueId
            };

            if (parentPart) {
                row.partNumber = parentPart + "." + row.partNumber;
            }

            if (parentName) {
                row.parentName = parentName;
            }

            if (section.children && section.children.length > 0) {
                row.hasChildren = true;
                row.children = [];

                _.each(section.children, (value: services.IApplicationSection) => {
                    row.children.push(this.processSectionForReviewerView(value, false, hasOutcome, row.partNumber, row.uniqueIdentifier + ": " + row.name));
                });
            }

            if (this.shouldBeRemoved(row)) {
                row.isVisible = false;
            } else {
                if (section.children && section.children.length > 0) {
                    this.common.setCircleReviewerFromChild(row);
                } else {
                    this.common.setCircleFromQuestions(row, this.rfis, true);
                }
            }

            //check for 'ANY' RFI
            var found = _.find(row.questions, (q: services.IQuestion) => {
                return q.answerResponseStatusName === "RFI";
            });
            if (found) {
                this.hasRfi = true;
                row.hasRfi = true;
            }
            else { row.hasRfi = false; }

            //check if all questions are reviewed or not
            var notAllReviewed = _.find(row.questions, (q: services.IQuestion) => {
                return (q.answerResponseStatusName !== "Reviewed" && q.answerResponseStatusName !== "RFI" && q.answerResponseStatusName !== "RFI/Followup" && q.answerResponseStatusName != null);
            });
            if (notAllReviewed) {
                this.allReviewed = false;
            }

            //check if any question is flagged or not
            var flaggedFound = _.find(row.questions, (q: services.IQuestion) => {
                return q.flag == true && !q.isHidden;
            });
            if (flaggedFound) {
                this.isFlagged = true;
                row.hasFlags = true;
            }

            return row;
        }
        //process sections in quick view

    }



}