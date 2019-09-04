module app.admin {
    'use strict';

    class OverviewController {
        application: services.IApplication;
        applicationType: string = "";
        sites: Array<services.ISite> = [];
        inspection: services.IInspection;
        selectedApplication: number;
        selectedSiteName = "";
        siteDescription: string;
        overallImpressions: string;
        commendablePractices: string;
        inspectors: Array<services.IInspectionScheduleDetail> = [];
        gridOptions = {
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "isInspectionComplete", template: $("#complete").html(), title: "&nbsp;", width: "50px" },
                { field: "user.firstName", template: $("#name").html(), title: "Name" },
                { field: "roleText", title: "Role" },
                { field: "siteName", title: "Site" },
                { field: "inspectorCompletionDate", template: $("#completionDate").html(), title: "Completion Date" }
            ],
            pageable: {
                pageSize: 10
            }
        };
        isInspector = false;
        isComplete = false;
        isOutcomeReviewed = true;
        applicationUniqueId: string = "";
        compAppId = "";
        isCurrentApp = true;
        canComplete = true;
        //app: services.IComplianceApplication;
        isLead = false;
        isTrainee = false;
        isUserSite = true;
        isComplianceApplication = false;
        isSaved = true;
        inspector: services.IInspectionScheduleDetail = null;
        //submittedDate: Date;
        //appDueDate: Date;
        //inspectionDate: Date;
        //compApp: services.IComplianceApplication;

        static $inject = [
            '$q',
            '$rootScope',
            '$location',
            '$window',
            'applicationService',
            'organizationService',
            'inspectionService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            $q: ng.IQService,
            $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private $window: ng.IWindowService,
            private applicationService: services.IApplicationService,
            private organizationService: services.IOrganizationService,
            private inspectionService: app.services.IInspectionService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {

            this.applicationUniqueId = $location.search().app;
            this.compAppId = $location.search().c;

            if (this.common.currentUser) {
                this.isInspector = this.common.currentUser.role.roleName === this.config.roles.inspector;
            }

            this.isComplianceApplication = $location.search().c && $location.search().c !== "";

            $rootScope.$on(this.config.events.userLoggedIn, (data: any, args: any) => {
                this.isInspector = args.roleName === this.config.roles.inspector;
                this.checkComplete();
                this.checkForTrainee();
            });

            this.common.showSplash();
            //this.getApplications();
            this.getDetails();

            this.common.checkItemValue(this.config.events.complianceApplicationInspectorsLoaded, this.common.compAppInspectors, false)
                .then(() => {
                    this.inspectors = this.common.compAppInspectors;
                    if (this.inspectors != null) {
                        _.each(this.inspectors, (i) => {
                            i.roleText = "";

                            if (i.isLead) {
                                i.roleText += "Lead";
                            }

                            if (i.isMentor) {
                                if (i.roleText !== "") {
                                    i.roleText += ", Mentor";
                                } else {
                                    i.roleText = "Mentor";
                                }
                            }

                            if (i.roleText === "") {
                                i.roleText = i.roleName;
                            }

                            
                        });
                        this.gridOptions.dataSource.data(this.inspectors);
                    }

                    this.checkComplete();
                    this.checkForTrainee();
                    this.process();
                });

            this.common.checkItemValue(this.config.events.compAppLoaded, this.common.compApp, false)
                .then(() => {
                    _.each(this.common.compApp.complianceApplicationSites, (appSite) => {
                        var found = _.find(this.sites, (site) => {
                            return site.siteId === appSite.siteId;
                        });

                        if (!found) {
                            this.sites.push({
                                siteId: appSite.siteId,
                                siteName: appSite.siteName
                            });
                        }
                    });
                    

                    this.sites = _.orderBy(this.sites, (site) => {
                        return site.siteName;
                    });

                    this.process();
                });

            this.common.checkItemValue(this.config.events.applicationLoaded, this.common.application, false)
                .then(() => {
                    this.application = this.common.application;

                    if (this.common.isFact() || this.common.isInspector()) {
                        this.application.applicantApplicationStatusName = this.application.applicationStatusName;
                    }

                    this.applicationType = this.application.applicationTypeName;

                    this.process();

                    console.log('app loaded', this.common.application);
                });
        }

        process() {
            if (this.common.currentUser && this.inspectors.length > 0 && this.sites && this.sites.length > 0 && this.application) {
                var inspec = _.find(this.inspectors, (i) => {
                    return i.userId.replace("'", "").replace("'", "") === this.common.currentUser.userId;
                });

                if (inspec) {
                    this.selectedSiteName = inspec.siteName;
                } else {
                    this.selectedSiteName = this.application.site ? this.application.site.siteName : "";
                }

                this.onSiteSelected(true);   
            }
        }

        //getApplications(): ng.IPromise<void> {
        //    return this.applicationService.getApp(this.applicationUniqueId)
        //        .then((app: services.IApplication) => {
        //            this.application = app;

        //            console.log('app', this.application);

        //            this.common.$broadcast(this.config.events.coordinatorSet, { coordinator: this.application.coordinator });

        //            if (this.common.isFact() || this.common.isInspector()) {
        //                this.application.applicantApplicationStatusName = this.application.applicationStatusName;
        //            }

                    

                    
        //        });
        //}

        onSiteSelected(fromInside?: boolean): void {
            fromInside = fromInside || false;

            var selectedSite = _.find(this.sites, (site) => {
                return site.siteName === this.selectedSiteName;
            });

            //this.isCurrentApp = selectedSite.siteId === this.application.site.siteId;

            if (!fromInside) {
                this.common.showSplash();
            }
            
            this.inspectionService.getInspectionBySite(this.compAppId, selectedSite.siteName)
                .then((data: services.IInspection[]) => {
                    console.log('site', data);
                    if (data.length > 1) {
                        this.inspection = data[data.length - 1];
                        this.overallImpressions = this.inspection.overallImpressions;
                        this.commendablePractices = data[0].commendablePractices;
                        this.siteDescription = data[0].siteDescription;
                    } else if (data.length > 0) {
                        this.inspection = data[0];
                        this.overallImpressions = this.inspection.overallImpressions;
                        this.commendablePractices = this.inspection.commendablePractices;
                        this.siteDescription = this.inspection.siteDescription;
                    } else {
                        this.siteDescription = "";
                        this.commendablePractices = "";
                        this.overallImpressions = "";
                    }

                    if (this.siteDescription === "" ||
                        this.overallImpressions === "" ||
                        this.commendablePractices === "") {
                        this.isSaved = false;
                    }

                    var inspector = _.find(this.inspectors, (i) => {
                        return i.userId.replace("'", "").replace("'", "") === this.common.currentUser.userId &&
                            i.siteName === selectedSite.siteName;
                    });
                    if (inspector) {
                        this.isUserSite = true;
                        this.isComplete = inspector.isInspectionComplete || false;
                    } else {
                        this.isUserSite = false;
                    }


                    this.common.hideSplash();

                    this.checkComplete();
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data");
                    this.common.hideSplash();
                });
        }

        //getInspectors(): ng.IPromise<void> {
        //    return this.inspectionService.getCompAppInspectors(this.compAppId, false)
        //        .then((items: Array<services.IInspectionScheduleDetail>) => {
        //            console.log('inspectors', items);
        //            this.inspectors = items;
        //            if (items != null) {
        //                _.each(items, (i) => {
        //                    i.roleText = "";

        //                    if (i.isLead) {
        //                        i.roleText += "Lead"
        //                    }

        //                    if (i.isMentor) {
        //                        if (i.roleText !== "") {
        //                            i.roleText += ", Mentor";
        //                        } else {
        //                            i.roleText = "Mentor";    
        //                        }
        //                    }

        //                    if (i.roleText === "") {
        //                        i.roleText = i.roleName;
        //                    }
        //                });
        //                this.gridOptions.dataSource.data(items);
        //            }
                        
        //            this.checkComplete();
        //            this.checkForTrainee();
        //        });
        //}

        checkForTrainee() {
            var found = _.find(this.inspectors, (item) => {
                return item.userId.replace("'", "").replace("'", "") === this.common.currentUser.userId && item.roleName === "Inspector Trainee";
            });

            if (found) {
                this.isTrainee = true;
            } else {
                this.isTrainee = false;
            }
        }

        getDetails(): ng.IPromise<void> {
            return this.inspectionService.getInspection(this.applicationUniqueId)
                .then((data: services.IInspection[]) => {
                    if (data.length > 1) {
                        this.inspection = data[data.length - 1];
                        this.overallImpressions = this.inspection.overallImpressions;
                        this.commendablePractices = data[0].overallImpressions;
                        this.siteDescription = data[0].siteDescription;
                    } else if (data.length > 0) {
                        this.inspection = data[0];
                        this.overallImpressions = this.inspection.overallImpressions;
                        this.commendablePractices = this.inspection.overallImpressions;
                        this.siteDescription = this.inspection.siteDescription;
                    } else {
                        this.siteDescription = "";
                        this.commendablePractices = "";
                        this.overallImpressions = "";
                    }
                });
        }

        checkComplete(): void {
            if (!this.common.currentUser) return;
            var user = _.find(this.inspectors, (i) => {
                return i.user.userId === this.common.currentUser.userId && i.siteName === this.selectedSiteName;
            });

            var leaduser = _.find(this.inspectors, (i) => {
                return i.user.userId === this.common.currentUser.userId && i.isLead;
            });

            if (leaduser) {
                this.isLead = true;
            }

            if (user) {
                this.isComplete = user.isInspectionComplete;

                if (!user.isLead || !this.common.compApp) return;

                this.isLead = true;

                if (user.isClinical && !user.reviewedOutcomesData) {
                    this.isOutcomeReviewed = false;
                }

                this.isOutcomeReviewed = true;

                var f = _.find(this.inspectors, (i) => {
                    return !i.isInspectionComplete && i.roleText !== 'Inspector Trainee';
                });

                if (f || !this.inspection || !this.inspection.allSitesWithDetails) {
                    this.canComplete = false;
                    return;
                }

                var notComplete = _.find(this.inspectors, (i: any) => {
                    return i.inspectorCompletionDate == null && i.inspectionCompletionDate == null && i.roleText !== 'Inspector Trainee';
                });

                if (notComplete) {
                    this.canComplete = false;
                }

                _.each(this.common.compApp.complianceApplicationSites, (site) => {
                    _.each(site.appResponses, (application) => {
                        if (application.isInspectionResponsesComplete != undefined && !application.isInspectionResponsesComplete) {
                            this.canComplete = false;
                            return false;
                        }
                    });

                    if (!this.canComplete) {
                        return false;
                    }
                });

                if (!this.canComplete) return;

                var found = _.find(this.inspectors, (i) => {
                    return !i.isInspectionComplete && i.user.userId !== this.common.currentUser.userId && i.roleText !== 'Inspector Trainee';
                });

                if (found) {
                    this.canComplete = false;
                    return;
                }
            }
        }
        
        save(): void {
            this.common.showSplash();

            var selected = _.find(this.sites, (site) => {
                return site.siteName === this.selectedSiteName;
            });

            this.inspectionService.save(this.application.uniqueId, selected.siteId, this.siteDescription, this.commendablePractices, this.overallImpressions)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item == true) {
                            if (data.message != '' && data.message != null)
                                this.notificationFactory.success(data.message);

                            this.isSaved = this.siteDescription !== "" && this.commendablePractices !== "" && this.overallImpressions !== "";

                        } else {
                            this.notificationFactory.error("Error.");
                        }
                    }

                    this.common.hideSplash();
                    //this.onSiteSelected(true);
                    
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }

        onInspectorComplete(): void {
            this.common.showSplash();

            var appId;

            var site = _.find(this.common.compApp.complianceApplicationSites, (s) => {
                return s.siteName === this.selectedSiteName;
            });

            if (site) {
                appId = site.appResponses[0].applicationId;
            }

            if (!appId) {
                appId = this.inspection.applicationId;
            }

            try {
                this.applicationService.setInspectorComplete(appId)
                    .then((data: services.IServiceResponse) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        }
                        else {
                            try {
                                var user = _.find(this.inspectors, (i) => {
                                    return i.user.userId === this.common.currentUser.userId && i.siteName === this.selectedSiteName;
                                });

                                if (user) {
                                    user.isInspectionComplete = true;
                                    user.inspectorCompletionDate = new Date();
                                }
                                this.gridOptions.dataSource.data(this.inspectors);

                            } catch (ex) {

                            }

                            this.isComplete = true;
                            this.notificationFactory.success("Success");
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to update application status. Please contact support.");
                        this.common.hideSplash();
                    });
            } catch (ex) {
                this.notificationFactory.error("Error trying to update. Please contact support.");
                this.common.hideSplash();
            }
        }

        cancel(): void {
            this.clearForm();
        }

        clearForm(): void {
            this.selectedApplication = null;
            this.selectedSiteName = "";
            this.siteDescription = "";
            this.overallImpressions = "";
            this.commendablePractices = "";
        }

        onEval() {
            window.open('https://www.surveymonkey.com/r/FACTInspectorEvaluation', '_blank');
        }
    }

    angular
        .module('app.inspector')
        .controller('app.inspector.OverviewController',
        OverviewController);
} 