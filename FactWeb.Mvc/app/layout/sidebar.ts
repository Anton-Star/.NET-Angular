module app.layout {
    'use strict';

    interface ISidebarController {
        routes: Array<IRoute>;
        items: Array<IOpenItem>;
        isCurrent(route: string): boolean;
        getCurrentClass(route: string): string;
        setOpen(text: string): void;
        getItem(text: string): IOpenItem;
        getChevron(text: string): string;
    }

    interface IOpenItem {
        text: string;
        isOpen: boolean;
    }

    interface IRouteRoute {
        originalPath: string;
    }

    interface INgRouteService {
        current: INgRoute;
    }

    interface INgRoute extends INgRouteService {
        $$route: IRouteRoute;
    }

    interface IParams extends ng.route.IRouteParamsService {
        id: string;
    }

    class SidebarController implements ISidebarController {
        fullName: string;
        items = [
            { text: 'Admin', isOpen: false }
        ];
        routes: Array<IRoute> = [];
        showAppMenu = false;
        isComplianceApp = false;
        isApplication = false;
        isAdminOpen = false;
        isEmailSent = false;
        appId = "";
        appIdURL = "";
        versionTitle = "";
        role = "";
        org = "";
        orgName = "";
        orgId = "";
        compAppId = "";
        isTrainee = false;
        isMentor = false;
        isInspectionCompleted = false;
        application: services.IApplication;
        appStatusHistory: services.IApplicationStatusHistory;
        organization: services.IOrganization;
        inspectionSchedule: services.IInspectionSchedule;
        coordinator: services.IUser;
        canManageUsers = false;
        hasRfi = false;
        hadRFI = false;
        inspectors: Array<services.IInspectionScheduleDetail> = [];
        organizations: Array<services.IOrganization>;
        applications: Array<services.ICoordinatorApplication>;
        staff: Array<services.IUser>;
        showAccredReport = false;
        accessToken: services.IAccessToken;
        submittedCompId: string;
        inspectorHasAccess = false;
        showCoordinatorView = true;
        isInspectionComplete = false;
        isCb = false;
        showOutcome = false;
        outcomeSet = false;
        private isGettingAccess = false;

        static $inject = [
            '$rootScope',
            '$window',
            '$q',
            '$scope',
            '$location',
            '$uibModal',
            '$route',
            'config',
            'cacheService',
            'accreditationOutcomeService',
            'inspectionScheduleService',
            'applicationService',
            'organizationService',
            'userService',
            'documentService',
            'emailTemplateService',
            'inspectionService',
            'applicationSettingService',
            'common',
            'routes',
            'notificationFactory'
        ];
        constructor(private $rootScope: ng.IRootScopeService,
            private $window: ng.IWindowService,
            private $q: ng.IQService,
            private $scope: ng.IScope,
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $route: INgRouteService,
            private config: IConfig,
            private cacheService: services.ICacheService,
            private accreditationOutcomeService: services.IAccreditationOutcomeService,
            private inspectionScheduleService: services.IInspectionScheduleService,
            private applicationService: services.IApplicationService,
            private organizationService: services.IOrganizationService,
            private userService: services.IUserService,
            private documentService: services.IDocumentService,
            private emailTemplateService: services.IEmailTemplateService,
            private inspectionService: services.IInspectionService,
            private applicationSettingService: services.IApplicationSettingService,
            private common: app.common.ICommonFactory,
            routes: Array<IRoute>,
            private notificationFactory: blocks.INotificationFactory) {
            var vm = this;
            routes.forEach((route) => {
                if (route.title !== "") {// && route.isShown) {
                    vm.routes.push(route);
                }
            });

            this.onLocationChange(null);

            this.submittedCompId = null;

            if (this.$location.search().sub == undefined || this.$location.search().sub === "") {
                this.appId = this.$location.search().app;
                this.compAppId = this.$location.search().c;
            }
            
            this.versionTitle = this.$location.search().ver;
            //this.isCb = this.versionTitle && this.versionTitle.indexOf("CT") === -1 && this.versionTitle.indexOf("Cellular Therapy") === -1;
            this.showAppMenu = this.appId != null && this.appId !== "";
            this.checkUrl($location.url());

            if (common.currentUser != null) {
                this.role = common.currentUser.role.roleName;

                this.showCoordinatorView = this.role === this.config.roles.factAdministrator || this.role === this.config.roles.factCoordinator || this.role === this.config.roles.factQualityManager || this.role === this.config.roles.qualityManager;
                 
                if (this.appId) {
                    this.getPendingComplianceApp(this.appId);    
                }

                this.getInspectorAccess();                
            }

            this.scanForEvents();

            $scope.$watch('vm.org', () => {
                this.orgName = encodeURIComponent(this.org);
            });

            
        }

        onShowLegend() {
            this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/legend.html",
                controller: "app.modal.templates.LegendController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window'
            });
        }

        getAccessToken() {
            this.documentService.getAccessToken(this.org)
                .then((data: services.IAccessToken) => {
                    this.accessToken = data;
                    this.common.accessToken = data;
                    this.common.$broadcast(this.config.events.accessTokenLoaded, { token: data });
                });
        }

        getAccessTokenById(appId: string) {
            this.isGettingAccess = true;
            this.documentService.getAccessTokenById(appId)
                .then((data: services.IAccessToken) => {
                    console.log(data);
                    this.accessToken = data;
                    this.common.accessToken = data;
                    this.common.$broadcast(this.config.events.accessTokenLoaded, { token: data });
                    this.isGettingAccess = false;
                });
        }

        getPostInspectionDocuments(): ng.IPromise<void> {
            return this.documentService.getPostInspection(this.org)
                .then((data: services.IDocument[]) => {
                    this.common.postInspectionDocuments = data;
                    this.common.$broadcast(this.config.events.postInspectionDocumentsLoaded, { docs: data });
                });
        }

        getPendingComplianceApp(app) {
            if (!app) {
                this.submittedCompId = null;
                return;
            }

            this.applicationService.getSubmittedComplianceApplication(app)
                .then((data) => {
                    if (data != null && data.submittedComplianceId != null && this.common.currentUser.userId === data.orgDirectorId) {
                        this.submittedCompId = data.submittedComplianceId;
                    } else {
                        this.submittedCompId = null;
                    }
                });
        }

        getDocumentLibrary(appId: string): ng.IPromise<void> {
            return this.documentService.getByApp(appId)
                .then((data: Array<services.IDocument>) => {
                    console.log('docs', data);
                    this.common.documents = data;
                    this.common.$broadcast(this.config.events.documentsLoaded, { docs: data });

                });

        }
        getAccreditationRole(appId: string) {

            if (!appId || appId === "") {
                return;
            }

            this.inspectionScheduleService.getInspectionScheduleDetailForCompliance(appId)
                .then((data: services.IInspectionScheduleDetail[]) => {
                    var complete = true;
                    var isTrainee = false;
                    var isMentor = false;

                    this.common.inspectionScheduleDetails = data;
                    this.common.$broadcast(this.config.events.inspectionScheduleDetailsLoaded, { data: data });

                    for (let i = 0; i < data.length; i++) {
                        if (data[i].roleName.indexOf("Trainee") > -1) {
                            isTrainee = true;
                        }

                        if (data[i].isMentor) {
                            isMentor = true;
                        }

                        if (complete && !data[i].isInspectionComplete) {
                            complete = false;
                        }

                        if (isTrainee && !complete) {
                            break;
                        }
                    }

                    this.isInspectionComplete = complete;
                    this.isTrainee = isTrainee;
                    this.isMentor = isMentor;
                })
                .catch(() => {
                    this.isTrainee = false;
                    this.isInspectionComplete = false;
                });
        }

        getInspectionCompletionStatus(appId: string) {
            if (!appId || appId === "") {
                return;
            }

            //this.applicationService.getInspectionCompletionStatus(appId)
            //    .then((data: boolean) => {
            //        if (data != null) {                        
            //            this.isInspectionCompleted = data;
            //        }
            //    });
        }

        getInspectorAccess() {
            if (this.role && this.role === this.config.roles.inspector) {
                this.inspectorHasAccess = false;
                if (this.$location.search().c) {
                    this.applicationService.getComplianceApplicationAccess(this.$location.search().c)
                        .then((data) => {
                            this.inspectorHasAccess = data === "Inspector";
                            this.common.inspectorHasAccess = this.inspectorHasAccess;
                        });
                } else if (this.$location.search().app) {
                    this.applicationService.getApplicationAccess(this.$location.search().app)
                        .then((data) => {
                            this.inspectorHasAccess = data === "Inspector";
                            this.common.inspectorHasAccess = this.inspectorHasAccess;
                        });
                }
                
            }
            
        }

        getEmailTemplates(): ng.IPromise<void> {

            return this.emailTemplateService.getAll()
                .then((data: services.IEmailTemplate[]) => {
                    this.config.emailTemplates = data;
                    this.common.$broadcast(this.config.events.emailTemplatesLoaded, { templates: data });
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        getOrgDetails(orgName): ng.IPromise<void> {

            return this.organizationService.getByName(orgName, false, true)
                .then((org: services.IOrganization) => {
                    this.organization = org;
                    console.log('org', this.organization);

                    this.common.organization = org;
                    this.common.$broadcast(this.config.events.organizationLoaded, { org: org });
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        getApplicationSettings() {

            this.applicationSettingService.getAll()
                .then((data: services.IApplicationSetting[]) => {
                    this.common.applicationSettings = data;
                    this.common.$broadcast(this.config.events.applicationSettingsLoaded, { settings: data });
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        getCompApplication(compAppId: string) {
            this.getCompApp(compAppId);


            

        }

        getCompApp(compAppId: string): ng.IPromise<void> {
            return this.applicationService.getCompApplication(compAppId, null)
                .then((data) => {
                    console.log('compApp', data);

                    this.common.compApp = data;

                    this.common.$broadcast(this.config.events.compAppLoaded,
                        { compApp: this.common.compApp });
                });
        }

        getApplicationSections(appId: string) {
            console.log('getting sections', new Date()); 
            this.applicationService.getAppSections(appId)
                .then((items: Array<services.IApplicationSection>) => {
                    this.common.applicationSections = items;

                    this.common.$broadcast(this.config.events.applicationSectionsLoaded, { sections: items });
                })
                .catch((e) => {
                    console.log(e);
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        getCompAppInspectors(compId: string) {

            this.inspectionService.getCompAppInspectors(compId, true)
                .then((items: Array<services.IInspectionScheduleDetail>) => {

                    this.common.compAppInspectors = items;
                    this.common.$broadcast(this.config.events.complianceApplicationInspectorsLoaded,
                        { inspectors: items });

                    var user = _.find(items, (i) => {
                        return i.user.userId === this.common.currentUser.userId;
                    });

                    if (user) {
                        var u = _.find(items, (i) => {
                            return i.user.userId === this.common.currentUser.userId && i.isClinical;
                        });

                        if (u) {
                            this.showOutcome = true;
                        } else {
                            this.showOutcome = false;
                            this.outcomeSet = true;
                        }
                    } else if (!this.common.isInspector() && !this.outcomeSet) {
                        this.showOutcome = true;
                    } else {
                        this.showOutcome = false;
                        this.outcomeSet = true;
                    }

                    
                });
        }

        checkUserInspector(appId: string): ng.IPromise<void> {
            return this.inspectionService.getInspectors(appId)
                .then((items: services.IInspectionScheduleDetail[]) => {
                    this.common.applicationInspectors = items;
                    this.common.$broadcast(this.config.events.applicationInspectorsLoaded, { inspectors: items });

                    var user = _.find(items, (i) => {
                        return i.user.userId === this.common.currentUser.userId;
                    });

                    if (user) {
                        this.common.isUserLeadInspector = user.isLead;
                    } else {
                        this.common.isUserLeadInspector = false;
                    }

                    this.common.$broadcast(this.config.events.userInspectorLoaded,
                        { val: this.common.isUserLeadInspector });
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        checkEmailSent() {
            if (this.$location.search().app) {
                this.accreditationOutcomeService.getAccreditationOutcomeByApp(this.$location.search().app)
                    .then((data: services.IGenericServiceResponse<services.IAccreditationOutcome>) => {
                        if (data.item != null) {
                            this.isEmailSent = data.item.sendEmail;
                        }
                    });
            }
        }

        scanForEvents() {
            this.$rootScope.$on(this.config.events.userLoggedIn, (data: any, args: any) => {
                this.role = args.roleName;

                //this.getEmailTemplates();
                this.getApplicationSettings();

                this.showCoordinatorView = this.role === this.config.roles.factAdministrator || this.role === this.config.roles.factCoordinator || this.role === this.config.roles.factQualityManager || this.role === this.config.roles.qualityManager;

                if (this.$location.search().app || this.$location.search().c) {
                    this.getInspectorAccess();
                    this.getAccreditationRole(this.$location.search().c);
                }

                if (!this.compAppId && this.appId) {
                    this.getPendingComplianceApp(this.$location.search().app);
                }
            });

            this.$rootScope.$on(this.config.events.userImpersonated, (data: any, args: any) => {
                this.role = this.common.currentUser.role.roleName;
            });

            this.$rootScope.$on(this.config.events.userImpersonatedEnd, (data: any, args: any) => {
                this.role = this.common.currentUser.role.roleName;
            });

            this.$rootScope.$on(this.config.events.orgSet, (data: any, args: any) => {                                
                this.orgId = args.orgId;
                this.org = args.organization;

                if ((!this.common.accessToken || this.common.accessToken == null) && !this.isGettingAccess) {
                    this.getAccessToken();
                }

                //if (!this.common.organization || this.common.organization == null) {
                //    this.getOrgDetails(this.org);    
                //}

                if (this.common.isFact()) {
                    this.getPostInspectionDocuments();
                }
                
            });

            this.$rootScope.$on(this.config.events.coordinatorSet, (data: any, args: any) => {
                this.coordinator = args.coordinator;
            });

            this.$rootScope.$on('$locationChangeStart', (event, toState, toParams, fromState, fromParams) => {
                this.onLocationChange(toState);

            });
        }

        onLocationChange(toState) {
            var appChanged = false;

            if (this.$location.search().sub == undefined || this.$location.search().sub === "") {
                if (this.config.emailTemplates == null) {
                    this.getEmailTemplates();
                }

                if (this.common.applicationSettings == null) {
                    this.getApplicationSettings();
                }

                if (this.$location.search().app && this.appId !== this.$location.search().app) {
                    this.getInspectorAccess();
                    this.checkEmailSent();
                    appChanged = true;
                    this.common.resetApplication();
                    this.hadRFI = false;
                }

                if (this.appId !== this.$location.search().app) {
                    this.getPendingComplianceApp(this.$location.search().app);
                    appChanged = true;
                    this.common.resetApplication();
                    this.hadRFI = false;
                }

                if (this.$location.search().c !== "" && this.compAppId !== this.$location.search().c) {
                    this.showAccreditationReport(this.$location.search().c);
                    this.getInspectorAccess();
                    this.getAccreditationRole(this.$location.search().c);

                    appChanged = true;

                    if (this.$location.search().c != undefined && this.$location.search().c != null) {
                        this.common.resetApplication();
                        this.hadRFI = false;

                        this.checkUserInspector(this.$location.search().c);
                        this.getCompApplication(this.$location.search().c);
                        this.getCompAppInspectors(this.$location.search().c);
                    }
                }

                if ((this.$location.search().c == undefined || this.$location.search().c === "") &&
                    this.$location.search().app != undefined &&
                    this.$location.search().app !== "" && (this.$location.search().sub == undefined || this.$location.search().sub === "") &&
                    this.appId !== this.$location.search().app) {
                    this.getApplicationSections(this.$location.search().app);
                }

                this.appId = this.$location.search().app;
                this.compAppId = this.$location.search().c;
                if ((this.appId || this.compAppId) && this.$location.search().ver) {
                    this.versionTitle = this.$location.search().ver;
                    //this.isCb = this.versionTitle.indexOf("CT") === -1 && this.versionTitle.indexOf("Cellular Therapy") === -1;
                }

                this.showAppMenu = this.appId != null && this.appId !== "";

                if (appChanged && this.$location.search().app != undefined && this.$location.search().app != null) {
                    this.getApplication();
                    this.getDocumentLibrary(this.appId);
                    this.getAccessTokenById(this.appId);
                }
            }           

            if (this.$location.search().app == undefined || this.$location.search().app == null) {
                this.hasRfi = false;
            }

            if (toState) {
                var loc = toState.indexOf("#");
                var url = toState.substring(loc + 1);

                this.checkUrl(url);
            }

            this.getInspectionCompletionStatus(this.appId);

            if (this.showAppMenu) {
                //this.hadRFI = false;
            }
        }

        checkUrl(url: string) {
            if (url.indexOf("Compliance?") > -1 && !this.$location.search().sub) {
                this.isComplianceApp = true;
                this.isApplication = false;
            } else if (url.indexOf("Application?") > -1) {
                this.isComplianceApp = false;
                this.isApplication = true;
            } else {
                this.isComplianceApp = this.compAppId !== "";
                this.isApplication = this.compAppId === "";
            }
        }

        isCurrent(route: string): boolean {
            if (!this.$route.current || !this.$route.current.$$route || !this.$route.current.$$route.originalPath) return false;

            return this.$route.current.$$route.originalPath.indexOf(route) > -1;
        }

        getCurrentClass(route: string): string {
            if (!this.$route.current || !this.$route.current.$$route || !this.$route.current.$$route.originalPath) return '';

            if (this.$route.current.$$route.originalPath.indexOf(route) > -1) {
                this.setOpen(route);
                return 'selected';
            } else {
                return '';
            }
        }

        setOpen(text: string): void {

            var item = _.find(this.items, (item: IOpenItem) => {
                return item.text === text;
            });

            if (item) {
                item.isOpen = !item.isOpen;
                if (text === "Admin") {
                    this.isAdminOpen = item.isOpen;
                }
            }


        }

        getItem(text: string): IOpenItem {
            var item = _.find(this.items, (item: IOpenItem) => {
                return item.text === text;
            });

            return item;
        }

        getChevron(text: string): string {
            var item = _.find(this.items, (item: IOpenItem) => {
                return item.text === text;
            });

            if (item && item.isOpen) {
                return 'glyphicon glyphicon-chevron-up';
            } else {
                return 'glyphicon glyphicon-chevron-down';
            }
        }

        inspectionScheduleClick(): void {
            this.getApplication();
        }

        showAccreditationReport(id: string): void {
            if (!id) {
                this.showAccredReport = false;
                return;
            }

            this.applicationService.showAccreditationReport(id)
                .then((data) => {
                    this.showAccredReport = data;
                });
        }

        onReport(report: string){
            if (this.$location.absUrl().indexOf('Reporting') > -1) {
                this.common.$broadcast(this.config.events.onReportChanged, { report: report });
            } else {
                var url = "#/Reporting?app=" + this.appId + "&c=" + this.compAppId + "&ver=" + this.versionTitle + "&org=" + this.orgName + "&r=";

                switch (report) {
                    case this.config.reportNames.application:
                        url += "Application";
                        url += "&orgId=" + this.orgId;
                        break;
                    case this.config.reportNames.traineeInspectionSummary:
                        url += "Trainee%20Inspection%20Summary";
                        break;
                    case this.config.reportNames.inspectionSummary:
                        url += "Inspection%20Summary";
                        break;
                    case this.config.reportNames.accreditationReport:
                        url += "Accreditation%20Report";
                        break;
                    case this.config.reportNames.cbInspectionRequest:
                        url += "CB%20Inspection%20Request";
                        break;
                    case this.config.reportNames.ctInspectionRequest:
                        url += "CT%20Inspection%20Request";
                        break;
                    case this.config.reportNames.activity:
                        url += "Activity";
                        break;
                }

                this.$window.location.href = url;
            }
            
        }

        getApplicationHistory(): void {
            this.applicationService.getApplicationStatusHistory(this.appId, this.compAppId)
                .then((data: Array<services.IApplicationStatusHistory>) => {
                    if (data != null) {
                        var rfiStatus = _.find(data, (appStatus: services.IApplicationStatusHistory) => {
                            return appStatus.applicationStatusOld.id === 9 || appStatus.applicationStatusNew.id === 9
                                || appStatus.applicationStatusOld.id === 21 || appStatus.applicationStatusNew.id === 21;
                        });

                        if (rfiStatus) {
                            this.hadRFI = true;
                        }
                    }
                });
        }

        getApplication(): void {

            this.applicationService.getApp(this.appId)
                .then((data: services.IApplication) => {
                    console.log('myapp', data);
                    this.application = data;
                    this.common.application = data;

                    this.common.$broadcast(this.config.events.applicationLoaded, { app: data });

                    if (this.application == null) return;

                    this.common.$broadcast(this.config.events.orgSet,
                        {
                            organization: this.common.application.organizationName,
                            orgId: this.common.application.organizationId
                        });

                    this.getOrganization(false);

                    if (this.application.applicationStatusId == 9 || this.application.applicationStatusId == 21) {
                        this.hadRFI = true;
                        this.hasRfi = true;
                    }
                    else {
                        this.hasRfi = false;
                        this.getApplicationHistory();
                    }
                });
        }

        getOrganization(getSchedule: boolean): ng.IPromise<void> {
            return this.organizationService.getById(this.application.organizationId)
                .then((data: services.IOrganization) => {
                    this.organization = data;
                    this.common.organization = data;

                    console.log('org', this.organization); 

                    this.common.$broadcast(this.config.events.organizationLoaded, { org: this.organization });

                    var found = _.find(this.common.organization.facilities, (org) => {
                        return org.serviceTypeName.indexOf("Clinical Program") > -1;
                    });

                    if (found) {
                        if (!this.outcomeSet) {
                            this.showOutcome = true;
                        }
                    } else {
                        this.showOutcome = false;
                        this.outcomeSet = true;
                    }

                    if (getSchedule) {
                        this.getInspectionSchedule();
                    }
                });
        }

        getInspectionSchedule() {
            this.inspectionScheduleService.getInspectionSchedule(this.application.organizationId, this.application.applicationId)
                .then((data: services.IGenericServiceResponse<services.IInspectionSchedule>) => {
                    if (data.item != null) {
                        this.inspectionSchedule = data.item;
                        this.showInspectionSchedule();
                    }
                });
        }

        showInspectionSchedule() {
            var organizationId = this.organization.organizationId;
            var applicationId = this.application.applicationId;
            var inspectionScheduleId = this.inspectionSchedule.inspectionScheduleId;
            var organization = this.organization;

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/inspectionScheduleDetail.html",
                controller: "app.modal.templates.InspectionScheduleDetailController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    inspectionScheduleId: function () {
                        return inspectionScheduleId;
                    },
                    organizationId: function () {
                        return organizationId;
                    },
                    applicationId: function () {
                        return applicationId;
                    },
                    startDateSaved: function () {
                        return "";
                    },
                    endDateSaved: function () {
                        return "";
                    },
                    organization: () => {
                        return organization;
                    }
                }
            });

            instance.result.then(() => {

            });

        }

        onEdit() {
            this.common.showSplash();
            this.$q.all([this.getOrganization(false), this.getUsers(), this.getApplications()])
                .then(() => {
                    this.common.hideSplash();
                    this.openManageApplication();
                });
        }

        onInspectionReport() {
            this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/manageInspectionReport.html",
                controller: "app.modal.templates.ManageInspectionReportController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                windowClass: 'app-modal-window'
            });
        }

        openManageApplication() {
            var found = _.find(this.applications, (a) => {
                return a.applicationId === this.application.applicationId ||
                    (a.complianceApplicationId === this.application.complianceApplicationId && this.application.complianceApplicationId != null);
            });
            console.log('app', this.application, found);

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/applicationManagement.html",
                controller: "app.modal.templates.ApplicationManagementController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            organizations: [ this.organization ],
                            applications: this.applications,
                            coordinators: this.staff,
                            application: found
                        };
                    }
                }
            });

            instance.result.then((application: services.ICoordinatorApplication) => {
            });
        }

        getOrganizations(): ng.IPromise<void> {
            if (this.organizations) return null;

            return this.organizationService.getAll(false, false)
                .then((data: Array<services.IOrganization>) => {
                    this.organizations = data;
                });
        }

        getApplications(): ng.IPromise<void> {
            if (this.applications) return null;

            return this.applicationService.getCoordinatorApplications(true)
                .then((data: Array<services.ICoordinatorApplication>) => {
                    console.log('apps', data);
                    this.applications = data;
                });
        }

        getUsers(): ng.IPromise<void> {
            if (this.staff) return null;
            return this.userService.getFactStaff()
                .then((data: Array<services.IUser>) => {
                    this.staff = data;
                });
        }

        closeNav() {
            //document.getElementById("mySidenav").style.width = "0";
            $("#mySidenav").hide();
            $("#spnMenuOpen").show();
        }

    }

    angular
        .module('app.layout')
        .controller('app.layout.SidebarController',
        SidebarController);

}