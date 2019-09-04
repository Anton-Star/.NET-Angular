module app.Application {
    'use strict';

    interface IHistoryScope {
    }

    class HistoryController implements IHistoryScope {
        organization: services.IOrganization;
        application: services.IApplication;
        uniqueId: string;
        compAppId: string;
        inspectionScheduleList: Array<services.IInspectionSchedule>;

        appStatus: Array<services.IApplicationStatusHistory>;
        gridAppStatus = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        gridInspections = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        accreditationOutcomes: Array<services.IAccreditationOutcome>;
        gridAccreditationOutcomes = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$location',
            '$q',
            '$window',
            '$uibModal',
            'organizationService',
            'applicationService',
            'accreditationOutcomeService',
            'notificationFactory',
            'common',
            'config',
            'inspectionScheduleService'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $q: ng.IQService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private organizationService: services.IOrganizationService,
            private applicationService: services.IApplicationService,
            private accreditationOutcomeService: services.IAccreditationOutcomeService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig,
            private inspectionScheduleService: services.IInspectionScheduleService) {
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "History",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Application', isActive: true },
                    { url: '', name: 'History', isActive: true }
                ]
            });
            this.uniqueId = $location.search().app;
            this.compAppId = $location.search().c;
            this.getApplication();
        }

        getOrganization(): ng.IPromise<void> {
            return this.organizationService.getById(this.application.organizationId)
                .then((data: services.IOrganization) => {
                    console.log('org', data);
                    this.organization = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organization.");
                });
        }

        getAppStatus(): ng.IPromise<void> {
            return this.applicationService.getApplicationStatusHistory(this.uniqueId, this.compAppId)
                .then((data: Array<services.IApplicationStatusHistory>) => {
                    this.appStatus = data;
                    this.gridAppStatus.dataSource.data(data);
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting application status history.");
                });
        }

        getInspectionSchedule(): ng.IPromise<void> {
            return this.inspectionScheduleService.getAllInspectionSchedules(this.application.organizationId, this.application.applicationId)
                .then((data: Array<services.IInspectionSchedule>) => {
                    if (data != null) {
                        this.inspectionScheduleList = data;
                        this.gridInspections.dataSource.data(data);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting inspection schedule.");
                });
            //return this.inspectionScheduleService.getInspectionSchedule(this.application.organizationId, this.application.applicationId)
            //    .then((data: services.IGenericServiceResponse<services.IInspectionSchedule>) => {
            //        if (data.item != null) {
            //            this.inspectionSchedule = data.item;
            //        }
            //    })
            //    .catch(() => {
            //        this.notificationFactory.error("Error getting inspection schedule.");
            //    });
        }

        getAccreditationOutcome(): ng.IPromise<void> {
            return this.accreditationOutcomeService.getAccreditationOutcomeByOrgAndApp(this.application.organizationId, this.application.applicationId)
                .then((data: Array<services.IAccreditationOutcome>) => {
                    this.accreditationOutcomes = data;
                    this.gridAccreditationOutcomes.dataSource.data(data);
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting accreditation outcomes.");
                });
        }

        getApplication(): ng.IPromise<void> {
            return this.applicationService.getApp(this.uniqueId)
                .then((data: services.IApplication) => {
                    this.application = data;
                    
                    var methods = [this.getOrganization(), this.getAppStatus(), this.getAccreditationOutcome(), this.getInspectionSchedule()];

                    this.common.activateController(methods, '');
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting application");
                });
        }
    }

    angular
        .module('app.application')
        .controller('app.application.HistoryController',
        HistoryController);
}