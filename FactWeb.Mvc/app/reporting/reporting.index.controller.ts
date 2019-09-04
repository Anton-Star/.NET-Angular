module app.Renewal {
    'use strict';

    interface IActivityReport {
        startDate?: string;
        endDate?: string;
        orgNumber?: string;
        orgName?: string;
        city?: string;
        state?: string;
        country?: string;
        reportReviewStatus?: string;
        committeeDate?: string;
        outcomeStatus?: string;
        facilityType?: string;
        origStatus?: string;
        newStatus?: string;
        changedDate?: string;
        sortBy?: string;
    }

    class IndexController {
        compAppId: string;
        appId: string;
        orgId: number;
        selectedReport: string;
        orgName: string;
        reportPath: string;
        reportParms: Object;
        reportHeight: string;
        siteName: string;
        showSite = false;
        application: services.IComplianceApplication;
        sites: Array<services.ISite> = [];
        //sites: Array<services.IFlatSite> = [];
        activity: IActivityReport;
        reportReviewStatuses: services.IReportReviewStatus[];
        isMentor = false;
        mentor: services.IInspectionScheduleDetail;
        isReload = false;
        isComplianceApplication = false;

        static $inject = [
            '$rootScope',
            '$location',
            '$window',
            'config',
            'cacheService',
            'inspectionService',
            'applicationService',
            'notificationFactory',
            'siteService',
            'common'
        ];
        constructor(
            private $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private $window: ng.IWindowService,
            private config: IConfig,
            private cacheService: services.ICacheService,
            private inspectionService: services.IInspectionService,
            private applicationService: services.IApplicationService,
            private notificationFactory: blocks.INotificationFactory,
            private siteService: services.ISiteService,
            private common: app.common.ICommonFactory) {

            this.compAppId = $location.search().c;
            this.appId = $location.search().app;
            this.orgId = $location.search().orgId;

            this.activity = {
                sortBy: "CommitteeDate"
            }

            if (this.$location.search().c && this.$location.search().c !== "")
                this.isComplianceApplication = true;

            if ($location.search().r && !this.isReload) {
                //debugger;
                this.selectedReport = $location.search().r;
                this.orgName = $location.search().org;
                this.processReport();
            }

            $rootScope.$on(this.config.events.onReportChanged, (data: any, args: any) => {
                //debugger;
                //this.isReload = true;
                //this.$window.location.reload();

                this.selectedReport = args.report;
                this.processReport();
            });

        }

        processReport() {
            switch (this.selectedReport) {
                case this.config.reportNames.application:
                    this.showSite = true;
                    //this.common.activateController([this.getApplicationById()], '');
                    this.common.activateController([this.getSites()], '');
                    break;
                case this.config.reportNames.traineeInspectionSummary:
                    this.common.activateController([this.getInspectors()], '');
                    break;
                case this.config.reportNames.activity:
                    break;
            }

            if (this.selectedReport !== this.config.reportNames.activity) {
                this.onSelectedReport();
            }
        }

        getInspectors(): ng.IPromise<void> {
            if (this.compAppId !== "") {
                return this.inspectionService.getCompAppInspectors(this.compAppId, true)
                    .then((items: Array<services.IInspectionScheduleDetail>) => {
                        for (var i = 0; i < items.length; i++) {
                            if (items[i].isMentor && items[i].user.userId === this.common.currentUser.userId) {
                                this.isMentor = true;
                                this.mentor = items[i];
                                break;
                            }
                        }
                    });
            } else {
                return this.inspectionService.getInspectors(this.appId)
                    .then((items: Array<services.IInspectionScheduleDetail>) => {
                        var inspector = _.find(items, (row: services.IInspectionScheduleDetail) => {
                            return row.user.userId === this.common.currentUser.userId;
                        });

                        this.isMentor = inspector && inspector.isMentor;
                    });
            }

        }

        getSites(): ng.IPromise<void> {
            return this.siteService.getSitesByCompliance(this.$location.search().c)
                .then((data: Array<app.services.ISite>) => {
                    if (data != null) {                      
                        this.sites = data;                      
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting sites.");
                });
        }

        //getFlatSites(): ng.IPromise<void> {
        //    return this.siteService.getFlatSites()
        //        .then((sites: services.IFlatSite[]) => {
        //            if (sites != null && sites.length > 0) {
        //                this.sites = sites;
        //            }
        //            else
        //                this.notificationFactory.error("No sites");
        //        })
        //        .catch(() => {
        //            this.notificationFactory.error("Error getting sites");
        //        });
        //}

        //getApplicationById(): ng.IPromise<void> {
        //    return this.applicationService.getComplianceApplicationById(this.compAppId)
        //        .then((app: services.IComplianceApplication) => {
        //            this.application = app;
        //            _.each(app.complianceApplicationSites, (appSite) => {
        //                var found = _.find(this.sites, (site) => {
        //                    return site.siteId === appSite.site.siteId;
        //                });

        //                if (!found) {
        //                    this.sites.push(appSite.site);
        //                }
        //            });

        //            this.sites = _.orderBy(this.sites, (site) => {
        //                return site.siteName;
        //            });
        //        })
        //        .catch(() => {
        //            this.notificationFactory.error("Error getting data");
        //        });
        //}

        onSubmitFeedback() {
            this.common.showSplash();

            this.inspectionService.saveMentorFeedback(this.mentor)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Mentor Feedback saved successfully.");
                    }

                    this.common.hideSplash();

                })
                .catch(() => {
                    this.notificationFactory.error("Error saving. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onMentorComplete() {
            this.common.showSplash();

            this.inspectionService.sendMentorCompleteEmail(this.compAppId)
                .then(data => {
                    if (data.hasError) {
                        this.notificationFactory.error('Error Marking complete. ' + data.message);
                    } else {
                        this.notificationFactory.success('Marking complete succeeded.');
                    }

                    this.common.hideSplash();
                })
                .catch(err => {
                    this.notificationFactory.error('Error Marking Complete. Please contact support.');
                    this.common.hideSplash();
                });
        }

        onSelectedReport() {
            var selected = _.find(this.config.reports, (report) => {
                return report.name === this.selectedReport;
            });

            if (selected) {
                this.reportPath = selected.url;
                switch (this.selectedReport.toLowerCase()) {
                    case this.config.reportNames.accreditationReport.toLowerCase():
                    case this.config.reportNames.inspectionSummary.toLowerCase():
                    case this.config.reportNames.traineeInspectionSummary.toLowerCase():
                        this.reportParms = {
                            'complianceApplicationId': this.compAppId,
                            'orgName': this.orgName
                        };
                        break;
                    case this.config.reportNames.cbInspectionRequest.toLowerCase():
                    case this.config.reportNames.ctInspectionRequest.toLowerCase():
                        this.reportParms = {
                            'complianceApplicationId': this.compAppId
                        };
                        break;
                    case this.config.reportNames.application.toLowerCase():
                        this.reportParms = {
                            'complianceApplicationId': this.compAppId,
                            'siteName': this.siteName === "" ? null : this.siteName
                        };
                        break;
                    case this.config.reportNames.activity.toLowerCase():
                        this.reportParms = {
                            'StartDate': this.activity.startDate,
                            'EndDate': this.activity.endDate,
                            'OrgNumber': this.activity.orgNumber,
                            'OrgName': this.activity.orgName,
                            'City': this.activity.city,
                            'State': this.activity.state,
                            'Country': this.activity.country,
                            'ReportReviewStatus': this.activity.reportReviewStatus,
                            'CommitteeDate': this.activity.committeeDate,
                            'OutcomeStatus': this.activity.outcomeStatus,
                            'FacilityType': this.activity.facilityType,
                            'OrigStatus': this.activity.origStatus,
                            'NewStatus': this.activity.newStatus,
                            'ChangeDate': this.activity.changedDate,
                            'SortBy': this.activity.sortBy
                        };
                        break;
                }
                
            }
        }

    }

    angular
        .module('app.renewal')
        .controller('app.reporting.IndexController',
        IndexController);
}  