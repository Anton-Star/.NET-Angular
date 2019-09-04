module app.admin {
    'use strict';

    class InspectionScheduleController { 
        organizations: Array<services.IOrganization>;        
        inspectionSchedules: Array<services.IInspectionSchedule>;
        selectedOrganization: number;        
        results: Array<services.IInspectionSchedule>;
        gridOptions = {
            sortable: true,
            //filterable: true,
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
            '$window',
            '$uibModal',
            'inspectionScheduleService',
            'applicationService',
            'cacheService',
            'organizationService',
            'notificationFactory',
            'currentUser',
            'common',
            'config'
        ];

        constructor(
            private $location: ng.ILocationService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private inspectionScheduleService: app.services.IInspectionScheduleService,
            private applicationService: services.IApplicationService,
            private cacheService: services.ICacheService,
            private organizationService: services.IOrganizationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private currentUser: app.services.IUser,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Inspection Scheduler",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Inspection Scheduler', isActive: true }
                ]
            });
            
            common.activateController([this.getOrganizations()], 'inspectionScheduleController');
        }

        getOrganizations(): ng.IPromise<void> {

            return this.organizationService.getFlatOrganizations()
                .then((data: Array<app.services.IOrganization>) => {
                    if (data == null) {
                        this.notificationFactory.error("no organization records");
                    } else {
                        this.organizations = data;
                    }

                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        search(): void {
            this.common.showSplash();
            this.applicationService.getInspectionSchedules(this.selectedOrganization.toString() === "0" ? "" : this.selectedOrganization.toString())
                .then((data: Array<app.services.IInspectionSchedule>) => {
                    console.log(data);                    
                    if (data == null || data .length == 0) {
                        this.notificationFactory.error('no items');
                    } else {
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                    this.common.hideSplash();

                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        delete(inspectionScheduleId): void {

            var confirmation = confirm("Are you sure you want to delete this schedule ?");
            if (confirmation) {
                this.common.showSplash();
                this.inspectionScheduleService.deleteSchedule(inspectionScheduleId)
                    .then((data: app.services.IGenericServiceResponse<boolean>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (data.item == true) {
                                this.search();
                                this.common.showSplash();
                            } else {
                                this.notificationFactory.error("Error.");
                            }
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                        this.common.showSplash();
                    });
            }
        }

        showSchedule(schedule: services.IInspectionSchedule) {
            return schedule.isCompleted === "Yes" || schedule.inspectionScheduleId === "0";
        }

        onCompApp(dataItem: app.services.IInspectionSchedule) {
            this.$location.url('/Compliance?app=' + dataItem.appUniqueId + '&c=' + dataItem.complianceApplicationId);
        }

        add(dataItem, isAddNew: boolean): void {  
            var selectedOrg = _.find(this.organizations, (org: services.IOrganization) => {
                return org.organizationId.toString() === this.selectedOrganization.toString();
            });

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/inspectionScheduleDetail.html",
                controller: "app.modal.templates.InspectionScheduleDetailController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    inspectionScheduleId: function () {
                        return isAddNew ? "0" : dataItem.inspectionScheduleId;
                    },
                    organizationId: function () {
                        return dataItem.organizationId;
                    },
                    applicationId: function () {
                        return dataItem.applicationId;
                    },
                    startDateSaved: function () {
                        return dataItem.startDate;
                    },
                    endDateSaved: function () {
                        return dataItem.endDate;
                    },
                    organization: () => {
                        return selectedOrg;
                    },
                    isReinspect: () => {
                        return dataItem.inspectionScheduleId != undefined &&
                            dataItem.inspectionScheduleId != null &&
                            isAddNew;
                    }
                }
            });

            instance.result.then(() => {
                this.search();
            }, () => {
            });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.InspectionScheduleController', InspectionScheduleController);
}