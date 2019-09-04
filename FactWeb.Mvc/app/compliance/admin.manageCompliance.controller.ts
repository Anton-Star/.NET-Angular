module app.compliance {
    'use strict';

    interface IComplianceConfigurationScope {
    }

    interface IVersion {
        name: string;
        version: services.IApplicationVersion;
    }

    class ManageComplianceController implements IComplianceConfigurationScope {
        organizations: Array<services.ISimpleOrganization>;
        staff: Array<services.IUser>;
        selectedOrg: services.ISimpleOrganization;
        selectedOrganization: string;
        sites: Array<services.ISite>;
        application: services.IComplianceApplication;
        app: services.IApplication;
        templates: Array<services.ITemplate>;

        isDirector: boolean = false;
        complianceApplicationId: string;
        //complianceApplication: app.services.IComplianceApplication;
        complianceApplications: services.IComplianceApplication[];
        approvalStatus: app.services.IComplianceApplicationApprovalStatus;
        serialNumber: string;
        //facilities: services.IFacility[];
        stillProcessing = false;
        isPreview = false;
        isApproved = false;
        title = "";
        appId = "";
        compAppId = "";
        appType = "Eligibility";

        froalaOptions = {
            toolbarButtons: ["bold", "italic", "underline", "|", "align", "formatOL", "formatUL"],
            key: 'te1C2sD6D7C4D5H5G4jqyznlyD-8mvtdE5lvbzG2B1A2B2D6B1B1C4G1D3=='
        };

        versions: Array<IVersion> = [];
        accTemplate: string;
        scopeTemplate: string;
        applicationSites: Array<services.ISiteApp> = [];
        applicationStatuses: services.IApplicationStatus[];
        reportReviewStatuses: services.IReportReviewStatus[];
        editingRow: services.ISiteApp;  
        allowDirectorMessage = true;
        defautDueDate: Date = new Date();
        grid: kendo.ui.Grid;
        appsGridOptions = {
            sortable: true,
            selectable: true,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            change: (e) => {
                this.onRowSelected(e);
            }
        };

        static $inject = [
            '$rootScope',
            '$scope',
            '$q',
            '$window',
            '$location',
            '$uibModal',
            'organizationService',
            'applicationService',
            'applicationStatusService',
            'reportReviewStatusService',
            'templateService',
            'userService',
            'cacheService',
            'requirementService',
            'facilityService',
            'versionService',
            'siteService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $rootScope: ng.IRootScopeService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private organizationService: services.IOrganizationService,
            private applicationService: services.IApplicationService,
            private applicationStatusService: services.IApplicationStatusService,
            private reportReviewStatusService: services.IReportReviewStatusService,
            private templateService: services.ITemplateService,
            private userService: services.IUserService,
            private cacheService: services.ICacheService,
            private requirementService: services.IRequirementService,
            private facilityService: services.IFacilityService,
            private versionService: services.IVersionService,
            private siteService: services.ISiteService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            //this.getFacilities();
            
            this.complianceApplicationId = $location.search().sub || $location.search().app;
            this.serialNumber = $location.search().sn;
            this.isPreview = $location.search().preview && $location.search().preview === "Y";
            this.selectedOrganization = $location.search().org;

            if (!this.selectedOrganization) {
                this.selectedOrganization = "";
            }

            if (this.complianceApplicationId) {
                this.common.showSplash();
                this.common.activateController([this.getOrganizations(), this.getVersions(), this.getComplianceApplicationById()], '');
            } else {
                this.common.activateController([this.getOrganizations(), this.getVersions(), this.getApplicationStatuses(), this.getReportReviewStatuses()], '');
            }
            
            
        }

        getReportReviewStatuses(): ng.IPromise<void> {
            return this.reportReviewStatusService.getAll()
                .then((data) => {
                    this.reportReviewStatuses = data;
                });
        }

        getApplicationStatuses(): ng.IPromise<void> {
            return this.applicationStatusService.getApplicationStatus()
                .then((data) => {
                    this.applicationStatuses = data;
                });
        }

        //getFacilities(): ng.IPromise<void> {
        //    return this.facilityService.getAllFlat()
        //        .then((data: services.IFacility[]) => {
        //            this.facilities = data;
        //        });
        //}

        getOrganizations(): ng.IPromise<void> {
            console.log('getting orgs', new Date());
            return this.organizationService.getSimpleOrganizations()
                .then((data: Array<services.ISimpleOrganization>) => {
                    console.log('orgs', data, new Date());
                    if (data != null) {
                        this.organizations = data.filter(function (el) {                            
                            return el.accreditationStatusName == null || el.accreditationStatusName != "Withdrawn";
                        });
                    }

                    if (this.selectedOrganization && this.versions && this.versions.length > 0) {
                        this.onSelectedOrganization();
                    } else if (this.selectedOrganization != undefined &&
                        this.selectedOrganization != null &&
                        this.selectedOrganization !== "") {
                        this.onSelectedOrganization();
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organizations.");
                });
        }

        getTemplates(): ng.IPromise<void> {
            if (this.templates == null && (this.complianceApplicationId == null || this.complianceApplicationId == '')) {
                console.log('getting templates', new Date());
                return this.templateService.getAll()
                    .then((data: Array<services.ITemplate>) => {
                        console.log('got templates', new Date(), data);
                        this.templates = data;
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error getting templates.");
                    });
            }
        }

        getUsers(): ng.IPromise<void> {
            if (this.staff == null && (this.complianceApplicationId == null || this.complianceApplicationId == '')) {
                console.log('getting users', new Date());
                return this.userService.getFactStaff()
                    .then((data: Array<services.IUser>) => {
                        console.log('got users', new Date(), data);
                        this.staff = data;
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error getting users.");
                    });
            }
        }

        getSimpleVersions(): ng.IPromise<void> {
            return this.versionService.getActiveSimple()
                .then((data: Array<services.IApplicationVersion>) => {
                    console.log('versions simple', data);
                    if (this.versions == null || this.versions.length == 0) {
                        _.each(data, (version: services.IApplicationVersion) => {
                            if (version.applicationType.isManageable) {
                                this.versions.push({
                                    name: version.applicationType.applicationTypeName,
                                    version: version
                                });
                            }
                        });
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting versions.");
                });
        }

        getVersions(): ng.IPromise<void> {
            console.log('getting versions', new Date());   
            return this.versionService.getActive()
                .then((data: Array<services.IApplicationVersion>) => {
                    console.log('versions', data, new Date());
                    if (this.versions == null || this.versions.length == 0) {
                        _.each(data, (version: services.IApplicationVersion) => {
                            if (version.applicationType.isManageable) {
                                this.versions.push({
                                    name: version.applicationType.applicationTypeName,
                                    version: version
                                });
                            }                          
                        });
                    }

                    if (this.selectedOrganization && this.organizations && this.organizations.length > 0) {
                        this.onSelectedOrganization();
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting versions.");
                });
        }

        onManageOrganization() {            
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/editOrganization.html",
                controller: "app.modal.templates.EditOrganizationController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: 'static',
                windowClass: 'app-modal-window',
                resolve: {
                    organizationid: () => {
                        return this.selectedOrg.organizationId;
                    },
                    organization: () => {
                        return null;
                    },
                    facilities: () => {
                        return null;
                    },
                    users: () => {
                        return null;
                    }
                }
            });

            instance.result.then(() => {
                
            }, () => {
            });
        }

        onViewApplication() {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/fullApplication.html",
                controller: "app.modal.templates.FullApplicationController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    applicationId: () => {
                        return this.selectedOrg.eligibilityApplicationUniqueId;
                    }
                }
            });

            instance.result.then(() => {

            }, () => {
            });
        }
        
        onViewRenewalApplication() {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/fullApplication.html",
                controller: "app.modal.templates.FullApplicationController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    applicationId: () => {
                        return this.selectedOrg.renewalApplicationUniqueId;
                    }
                }
            });

            instance.result.then(() => {

            }, () => {
            });
        }

        onRowSelected(event) {
            var grid = event.sender;
            var selectedItem = grid.dataItem(grid.select());
            this.application = selectedItem;
            this.application.dueDt = moment(this.application.dueDate).toDate();
            if (this.application && this.application.applications && this.application.applications.length > 0) {
                this.app = this.application.applications[0];
            }

            this.applicationSites = [];
            this.processApp();
            this.$scope.$apply();
            
        }

        processApp() {
            this.applicationSites = [];
            if (this.application == null) {
                return;
            }

            console.log(this.sites);
            
            this.application.complianceApplicationSites = this.application.complianceApplicationSites || [];

            _.each(this.sites, (site: services.ISite) => {
                var applicationSite = _.find(this.application.complianceApplicationSites, (appSite: services.IComplianceApplicationSite) => {
                    return appSite.site != null && appSite.site.siteId === site.siteId;
                });

                if (applicationSite) {
                    var types: Array<services.ISiteType> = [];

                    _.each(this.versions, (v) => {
                        types.push({
                            version: v.version,
                            hasType: false,
                            hasTypeString: 'No',
                            notApplicables: [],
                            versionTitle: ""
                        });
                    });

                    var row: services.ISiteApp = {
                        siteId: site.siteId,
                        site: site.siteName,
                        fullSite: site,
                        isEditMode: false,
                        types: types,
                        isStrong: site.isStrong,
                        standards: ""
                    };

                    _.each(applicationSite.applications, (app) => {
                        var record = _.find(row.types, (t) => {
                            return t.version.applicationType.applicationTypeName === app.applicationTypeName;
                        });

                        if (record) {
                            record.hasType = true;
                            record.hasTypeString = "Yes";
                            record.versionTitle = app.applicationVersionTitle;
                            record.notApplicables = app.notApplicables || [];
                        }

                        if (row.standards !== "") {
                            row.standards += ", ";
                        }

                        row.standards += app.standards;
                    });

                    if (this.complianceApplicationId != null && this.complianceApplicationId !== '') {
                        if (row.types.length > 0) {
                            this.applicationSites.push(row);
                        }
                    } else {
                        this.applicationSites.push(row);    
                    }

                    
                } else {
                    var types: Array<services.ISiteType> = [];

                    _.each(this.versions, (v) => {
                        types.push({
                            version: v.version,
                            hasType: false,
                            hasTypeString: 'No',
                            notApplicables: [],
                            versionTitle: ""
                        });
                    });

                    if (this.complianceApplicationId == null || this.complianceApplicationId === '') {
                        this.applicationSites.push({
                            siteId: site.siteId,
                            site: site.siteName,
                            fullSite: site,
                            isEditMode: false,
                            types: types
                        });
                    }

                    

                    this.application.complianceApplicationSites.push({
                        site: site,
                        applications: []
                    });
                }
            });
        }

        onAddNew() {
            this.applicationSites = [];

            this.application.id = "";
            this.allowDirectorMessage = false;

            if (this.application.approvalStatus) {
                this.application.approvalStatus.name = "Planning";
            }
            

            _.each(this.sites, (site: services.ISite) => {
                var types: Array<services.ISiteType> = [];

                _.each(this.versions, (v) => {
                    types.push({
                        version: v.version,
                        hasType: false,
                        hasTypeString: 'No',
                        notApplicables: [],
                        versionTitle: ""
                    });
                });

                this.applicationSites.push({
                    siteId: site.siteId,
                    site: site.siteName,
                    fullSite: site,
                    isEditMode: false,
                    types: types
                });

                this.application.complianceApplicationSites.push({
                    site: site,
                    applications: []
                });
            });
        }

        isUserDirector() {
            this.isDirector = false;

            this.organizationService.isDirector(this.selectedOrg.organizationName)
                .then((data) => {
                    console.log('isdirector', data);
                    this.isDirector = data;
                });
        }

        onSelectedOrganization(): void {
            this.allowDirectorMessage = true;
            this.applicationSites = [];
            this.application = null;
            this.selectedOrg = _.find(this.organizations, (org: services.ISimpleOrganization) => {
                return org.organizationName.trim() === this.selectedOrganization.trim();
            });

            if (this.selectedOrg != undefined) {
                //this.common.$broadcast(this.config.events.orgSet,
                //    { orgId: this.selectedOrg.organizationId, organization: this.selectedOrg.organizationName });

                this.isUserDirector();

                console.log('org', this.selectedOrg);

                this.appType = this.selectedOrg.renewalApplicationUniqueId != null ? "Renewal" : "Eligibility";

                this.common.showSplash();
                this.$q.all([this.getApplication(), this.getSites(), this.getUsers(), this.getTemplates()])
                    .then(() => {
                        this.processApp();
                        _.each(this.applicationSites, (s) => {
                            if (s.fullSite &&
                                s.fullSite.facilities &&
                                s.fullSite.facilities.length > 0 &&
                                s.fullSite.facilities[0].serviceType) {
                                if (s.fullSite.facilities[0].serviceType.name.indexOf("CB") > -1) {
                                    s.fullSite.siteTypes = "";
                                    if (s.fullSite.siteCBCollectionType && s.fullSite.siteCBCollectionType != null) {
                                        s.fullSite.siteTypes = s.fullSite.siteCBCollectionType.name;
                                    }

                                    if (s.fullSite.siteCBUnitType && s.fullSite.siteCBUnitType != null) {
                                        if (s.fullSite.siteTypes && s.fullSite.siteTypes !== "") {
                                            s.fullSite.siteTypes += ", ";
                                        }

                                        s.fullSite.siteTypes += s.fullSite.siteCBUnitType.name;
                                    }
                                    
                                } else if (s.fullSite.facilities[0].serviceType.name.indexOf("Clinical") > -1 || 
                                    (s.fullSite.facilities[0].serviceType.name.indexOf("Collection") > -1 && !s.fullSite.siteCollectionProductType)) {
                                    s.fullSite.siteTypes = "";
                                    _.each(s.fullSite.transplantTypes, (tt) => {
                                        s.fullSite.siteTypes += tt.name + ", ";
                                    });
                                    if (s.fullSite.siteTypes.length > 0) {
                                        s.fullSite.siteTypes = s.fullSite.siteTypes
                                            .substr(0, s.fullSite.siteTypes.length - 2);
                                    }
                                } else if (s.fullSite.facilities[0].serviceType.name.indexOf("Processing") > -1) {
                                    s.fullSite.siteTypes = "";
                                    _.each(s.fullSite.processingTypes, (pt) => {
                                        s.fullSite.siteTypes += pt.name + ", ";
                                    });
                                    if (s.fullSite.siteTypes.length > 0) {
                                        s.fullSite.siteTypes = s.fullSite.siteTypes
                                            .substr(0, s.fullSite.siteTypes.length - 2);
                                    }
                                } else if (s.fullSite.facilities[0].serviceType.name.indexOf("Collection") > -1 && s.fullSite.siteCollectionProductType) {
                                    s.fullSite.siteTypes = s.fullSite.siteCollectionProductType.name;
                                }
                            }
                        });

                        

                        this.common.hideSplash();

                        if (this.application && !this.application.dueDate && this.selectedOrg.accreditationExpirationDate) {
                            var accreditationExpiryDate = new Date(this.selectedOrg.accreditationExpirationDate.toString());//Date.parse(this.selectedOrg.accreditationExpirationDate);
                            this.defautDueDate.setDate(accreditationExpiryDate.getDate() + 276);

                            this.application.dueDate = this.defautDueDate.getMonth() + '/' + this.defautDueDate.getDate() + '/' + this.defautDueDate.getFullYear();
                        }
                    });
            }
        }

        getType(site: services.ISiteApp, type: IVersion): boolean {
            var siteType = _.find(site.types, (t) => {
                return t.version.applicationType.applicationTypeName === type.version.applicationType.applicationTypeName;
            });

            if (siteType) {
                return siteType.hasType;
            }

            return false;
        }

        getVersion(site: services.ISiteApp, type: IVersion): string {
            var siteType = _.find(site.types, (t) => {
                return t.version.applicationType.applicationTypeName === type.version.applicationType.applicationTypeName;
            });

            if (siteType) {
                return siteType.versionTitle;
            }

            return "";
        }

        getCreds(app: services.IComplianceApplication) {

            if (!app || !app.coordinator) {
                return "";
            }

            var creds = "";
            _.each(app.coordinator.credentials, (c) => {
                creds += c.name + ", ";
            });

            if (creds !== "") {
                creds = ", " + creds;
                creds = creds.substring(0, creds.length - 2);
            }

            return creds;
        }

        getApplication(): ng.IPromise<void> {
            console.log('getting app', new Date());
            return this.applicationService.getComplianceApplication(this.selectedOrganization)
                .then((data: services.IComplianceApplication[]) => {
                    console.log('got app', new Date());
                    if (this.complianceApplicationId) {
                        var app = _.find(data, (c: services.IComplianceApplication) => {
                            return c.id === this.complianceApplicationId;
                        });

                        this.complianceApplications = [app];
                    } else {
                        this.complianceApplications = data;
                    }
                        
                    this.appsGridOptions.dataSource.data(this.complianceApplications);

                    if (this.complianceApplications.length === 1) {
                        this.application = this.complianceApplications[0];
                        this.application.dueDt = moment(this.application.dueDate).toDate();

                        if (this.application.approvalStatus &&
                            this.application.approvalStatus.name.indexOf('Active') > -1) {
                            //this.setApproved();
                        }

                        if (this.application && this.application.applications && this.application.applications.length > 0) {
                            this.app = this.application.applications[0];
                        }

                        console.log('compApp', this.application);

                        //this.processApp();
                    } else if (this.complianceApplications.length === 0) {
                        this.application = {
                            organizationId: this.selectedOrg.organizationId,
                            organizationName: this.selectedOrg.organizationName,
                            accreditationGoal: "",
                            applicationStatus: "",
                            inspectionScope: "",
                            coordinator: null,
                            rejectionComments: "",
                            accreditationStatus: "",
                            approvalStatus: null,
                            applications: [],
                            site: null
                        };
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting compliance application");
                });
        }

        getSites(): ng.IPromise<void> {
            this.sites = [];
            console.log('getting sites', new Date());
            return this.organizationService.getOrgSites(this.selectedOrganization)
                .then((data: Array<services.ISite>) => {
                    console.log('got sites', new Date(), data);
                    this.sites = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organization sites");
                });
        }

        onEdit(site: services.ISiteApp): void {
            site.isEditMode = true;
        }

        hasChanged(site: services.ISiteApp, version: IVersion): void {
            var type = _.find(site.types, (t) => {
                return t.version.applicationType.applicationTypeName === version.version.applicationType.applicationTypeName;
            });

            if (type) {
                type.hasType = !type.hasType;
            }
        }

        onUpdate(site: services.ISiteApp): void {
            _.each(site.types, (t) => {
               if (t.hasType && t.versionTitle === "") {
                   var version = _.find(this.versions, (v: IVersion) => {
                       return v.version.applicationType.applicationTypeName === t.version.applicationType.applicationTypeName;
                   });

                   if (version) {
                       t.versionTitle = version.version.title;
                   }
               } 
            });

            site.isEditMode = false;
        }

        onAccTemplateChange(): void {
            var accTemplate = _.find(this.templates, (template: services.ITemplate) => {
                return template.id === this.accTemplate;
            });
            if (accTemplate) {
                this.application.accreditationGoal = accTemplate.text;
            }
        }

        onScopeTemplateChange(): void {
            var scopeTemplate = _.find(this.templates, (template: services.ITemplate) => {
                return template.id === this.scopeTemplate;
            });
            if (scopeTemplate) {
                this.application.inspectionScope = scopeTemplate.text;
            }
        }

        getNotApplicables(applicationId: number, reqs: Array<services.IApplicationHierarchyData>): Array<services.INotApplicables> {
            var result: Array<services.INotApplicables> = [];

            _.each(reqs, (req) => {
                if (req.questions == undefined || req.questions == null || req.questions.length > 0) {
                    _.each(req.questions, (question) => {
                        if (question.isNotApplicable) {
                            result.push({
                                questionId: question.id,
                                applicationId: applicationId
                            });
                        }
                    });
                }

                if (req.hasChildren) {
                    var items = this.getNotApplicables(applicationId, req.children);

                    _.each(items, (item) => {
                        result.push(item);
                    });
                }
            });

            return result;
        }

        onNotApplicableClick(site: services.ISiteApp, appType: string) {
            var version: services.IApplicationVersion = null;
            var versionId = "";
            var notApp: Array<services.INotApplicables>;

            var compSite = _.find(this.application.complianceApplicationSites, (s) => {
                return s.site.siteId === site.siteId;
            });

            if (compSite) {
                var app = _.find(compSite.applications, (a) => {
                    return a.applicationTypeName === appType;
                });

                if (app) {
                    versionId = app.applicationVersionId;
                }
            }
            
            var appVersion = _.find(site.types, (v) => {
                return v.version.applicationType.applicationTypeName === appType;
            });
            
            if (appVersion) {

                version = appVersion.version;
                notApp = appVersion.notApplicables;
            }

            if (version != null) {
                if (versionId === "") {
                    versionId = version.id;
                }
                
                var actVersion = _.find(this.versions, (v: IVersion) => {
                    return v.version.id === versionId;
                });

                if (!actVersion) {
                    this.common.showSplash();
                    this.versionService.getById(versionId)
                        .then((data) => {
                            this.common.hideSplash();

                            this.showNotApplicables(site, appType, data, notApp);
                        })
                        .catch(() => {
                            this.notificationFactory.error("Cannot load requirements. Please contact support.");
                            this.common.hideSplash();
                        });
                } else {
                    this.showNotApplicables(site, appType, actVersion.version, notApp);
                }                
            }
        }

        showNotApplicables(site: services.ISiteApp, appType: string, version: services.IApplicationVersion, notApp: services.INotApplicables[]) {
            var reqs: Array<services.IApplicationHierarchyData> = [];
            _.each(version.applicationSections, (section: services.IApplicationSection) => {
                reqs.push(this.requirementService.processSection(section));
            });

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/notApplicableRequirement.html",
                controller: "app.modal.templates.NotApplicableRequirementController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                resolve: {
                    values: () => {
                        return {
                            requirements: reqs,
                            notApplicables: notApp
                        };
                    }
                }
            });

            instance.result.then((requirements: Array<services.IApplicationHierarchyData>) => {
                var notApplicables = this.getNotApplicables(0, requirements);
                var currentSite = _.find(this.applicationSites, (appSite) => {
                    return appSite.site === site.site;
                });

                if (currentSite) {
                    var type = _.find(currentSite.types, (t) => {
                        return t.version.applicationType.applicationTypeName === appType;
                    });

                    if (type) {
                        type.notApplicables = notApplicables;
                    }
                }

                console.log('notApplicables', notApplicables);
            }, () => {
            });
        }

        onCopyApplication() {

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/copyApplication.html",
                controller: "app.modal.templates.CopyApplicationController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                resolve: {
                    values: () => {
                        return {
                            application: this.application,
                            sites: this.applicationSites,
                            applicationStatuses: this.applicationStatuses
                        };
                    }
                }
            });

            instance.result.then((app: any) => {
                this.application = app.app;
                this.application.dueDt = moment(this.application.dueDate).toDate();
                this.applicationSites = [];
                this.processApp();
            }, () => {
            });
        }

        isValid(): boolean {
            return true;
        }

        onSave(): void {
            var appSites: Array<services.IComplianceApplicationSite> = [];

            _.each(this.applicationSites, (appSite: services.ISiteApp) => {
                var any = _.find(appSite.types, (t) => {
                    return t.hasType;
                });

                if (any) {
                    var apps = [];

                    _.each(appSite.types, (type) => {
                        if (type.hasType) {
                            apps.push({
                                applicationTypeId: type.version.applicationType.applicationTypeId,
                                applicationVersionId: type.version.id,
                                notApplicables: type.notApplicables
                            });
                        }
                    });

                    var applicationSite = _.find(this.application.complianceApplicationSites, (site: services.IComplianceApplicationSite) => {
                        return appSite.siteId === site.site.siteId;
                    });

                    if (applicationSite) {
                        applicationSite.applications = apps;
                    } else {
                        applicationSite = {
                            applications: apps,
                            site: appSite.fullSite
                        };
                    }

                    appSites.push(applicationSite);
                }
            });

            this.common.showSplash();
            try {
                this.application.organizationId = this.selectedOrg.organizationId;

                this.application.complianceApplicationSites = appSites;

                console.log(this.application);

                this.applicationService.setupComplianceApplication(this.application)
                    .then((data: services.IGenericServiceResponse<string>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.application.id = data.item;
                            this.notificationFactory.success("Compliance Application saved successfully.");
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error saving Compliance Application");
                        this.common.hideSplash();
                    });
            } catch (ex) {
                this.notificationFactory.error("Error trying to save data: " + ex);
                this.common.hideSplash();
            }
            
        }

        onApproveEligibility(): void {
            if (!confirm("Are you sure you want to Approve " + this.appType + "?")) {
                return;
            }

            this.common.showSplash();

            this.applicationService.approveEligibility(this.application)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application submitted for eligibility approval successfully");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving");
                    this.common.hideSplash();
                });
        }

        onApproval(): void {
            if (!confirm("Are you sure you want to submit this application for approval?")) {
                return;
            }

            this.common.showSplash();

            if (this.application.approvalStatus == null) {
                this.application.approvalStatus = {
                    id: "",
                    name: "Submitted"
                };
            } else {
                this.application.approvalStatus.name = "Submitted";
            }

            this.applicationService.setComplianceApplicationApprovalStatus(this.application.id, this.application.approvalStatus.name, this.serialNumber || "", "")
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application submitted for approval successfully");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving");
                    this.common.hideSplash();
                });
        }

        saveTypes(): void {
            var site = _.find(this.applicationSites, (appSite: services.ISiteApp) => {
                return appSite.site === this.editingRow.site;
            });

            if (site) {
                site.types = this.editingRow.types;
            }

            this.grid.refresh();
        }

        getComplianceApplicationById(): ng.IPromise<void> {
            if (this.complianceApplicationId) {
                console.log('getting comp', new Date());
                return this.organizationService.getNameByCompliance(this.complianceApplicationId)
                    .then((data: string) => {
                        console.log('comp', new Date());
                    //this.complianceApplication = data;
                    this.selectedOrganization = data;

                    if (this.organizations && this.organizations.length > 0 && this.versions && this.versions.length > 0) {
                        this.onSelectedOrganization();

                    } else {
                        this.common.showSplash();
                        this.stillProcessing = true;
                    }

                        console.log('sects', data);
                    })
                .catch(() => {
                    this.notificationFactory.error("Error getting data");
                });
            }
        }

        setApproved() {
            for (var i = 0; i < this.application.complianceApplicationSites.length; i++) {
                var site = this.application.complianceApplicationSites[i];

                if (site.applications && site.applications.length > 0) {
                    var app = site.applications[0];
                    this.title = app.applicationVersionTitle;
                    this.appId = app.uniqueId;
                    this.compAppId = app.complianceApplicationId;
                }
            }
            

            this.isApproved = true;
        }

        updateStatus(isApproved: boolean): void {
            //var app: any = {};
            //this.complianceApplication = app;
            //this.complianceApplication.id = this.complianceApplicationId;
            this.approvalStatus = { id: "", name: "" };
            this.approvalStatus.name = isApproved ? "Approved / Active" : "Reject";
            //this.complianceApplication.approvalStatus = this.approvalStatus;

            if (!isApproved) {
                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/comment.html",
                    controller: "app.modal.templates.CommentController",
                    controllerAs: "vm",
                    size: 'lg',
                    backdrop: false,
                    keyboard: false,
                    windowClass: 'app-modal-window',
                    resolve: {
                        values: () => {
                            return {
                                title: "Rejection Reason"
                            };
                        }
                    }
                });

                instance.result.then((comment: string) => {
                        this.saveStatus(comment);
                    },
                    () => {
                        this.notificationFactory.error("Save Cancelled");
                    });
            } else {
                this.saveStatus("");
            }

            

            
        }

        saveStatus(comments: string) {
            this.common.showSplash();

            this.applicationService.setComplianceApplicationApprovalStatus(this.complianceApplicationId, this.approvalStatus.name, this.serialNumber, comments)
                .then((data: app.services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Status updated successfully");

                        if (this.approvalStatus.name === "Approved / Active") {
                            this.setApproved();
                            this.application.approvalStatus.name = "Approved / Active";
                        } else {
                            this.application.approvalStatus.name = "Reject";
                        }
                        
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error updating status. Please contact system administrator.");
                    this.common.hideSplash();
                });
        }

        editSite(rowData): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/newSite.html",
                controller: "app.modal.templates.NewSiteController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            parentSite: rowData,
                            isNewSite: false,
                            facilities: null,
                            siteName: rowData.siteName
                        }
                    }
                }
            });

            instance.result.then(() => {
            }, () => {
            });
        }

        onDeleteApplication(): void {
            if (!confirm("Are you sure you want to delete the application?")) {
                return;
            }

            this.common.showSplash();

            this.applicationService.cancelComplianceApplication(this.application.id)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application deleted successfully");
                        this.application = null;
                        this.selectedOrganization = "";
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error deleting application. Please contact support.");
                    this.common.hideSplash();
                });
        }
    }

    angular
        .module('app.compliance')
        .controller('app.compliance.ManageComplianceController',
        ManageComplianceController);
}  