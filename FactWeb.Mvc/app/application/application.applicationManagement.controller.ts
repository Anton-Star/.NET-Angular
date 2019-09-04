module app.Application {
    'use strict';

    interface IApplicationType {
        id: number;
        name: string;
    }

    class ApplicationManagement {
        organizations: Array<services.IOrganization>;
        selectedApplicationType = "";
        selectedOrganization = "";
        annualApplication: IApplicationType = {
            id: 5, name: "Annual Application"
        };
        renewalApplication: IApplicationType = {
            id: 6, name: "Renewal Application"
        };
        eligibilityApplication: IApplicationType = {
            id: 3, name: "Eligibility Application"
        };
        applicationTypes: Array<IApplicationType> = [];
        applications: Array<services.ICoordinatorApplication> = [];
        staff: Array<services.IUser>;
        gridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10,
                group: [{
                    field: "organizationName"
                }]
            }),
            columns: [
                { "field": "organizationName", "title": "Organization Name",
                    groupHeaderTemplate: "#= value#", template: $("#organization").html()},
                { "field": "applicationTypeName", "title": "Application Type", template: $("#applicationType").html() },
                {
                    "field": "applicationStatusName", "title": "Application Status" },
                { "field":"coordinator",template: $("#coordinator").html(), "title": "Coordinator" },
                {
                    template: "<button class=\"k-button\" ng-click=\"vm.onEdit(dataItem)\">Edit </button> <button class=\"k-button\" ng-click=\"vm.onDelete(dataItem)\">Cancel </button>"
                }
            ],
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$q',
            '$window',
            '$uibModal',
            'cacheService',
            'organizationService',
            'applicationService',
            'userService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $q: ng.IQService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private organizationService: app.services.IOrganizationService,
            private applicationService: services.IApplicationService,
            private userService: services.IUserService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            common.activateController([this.getOrganizations(), this.getApplications(), this.getUsers()], '');
        }

        getOrganizations(): ng.IPromise<void> {
            return this.organizationService.getAll(false, false)
                .then((data: Array<services.IOrganization>) => {
                    this.organizations = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getApplications(): ng.IPromise<void> {
            return this.applicationService.getCoordinatorApplications(true)
                .then((data: Array<services.ICoordinatorApplication>) => {
                    this.applications = data;
                    this.gridOptions.dataSource.data(data);
                })
                .catch(() => {
                    this.notificationFactory.error("error getting applications");
                    this.common.hideSplash();
                });
        }

        getUsers(): ng.IPromise<void> {
            return this.userService.getFactStaff()
                .then((data: Array<services.IUser>) => {
                    this.staff = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        onEdit(application: services.IApplication) {
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
                            organizations: this.organizations,
                            applications: this.applications,
                            coordinators: this.staff,
                            application: application
                        };
                    }
                }
            });

            instance.result.then((application: services.ICoordinatorApplication) => {
                var changedApp = _.find(this.applications, (app) => {
                    return app.applicationId === application.applicationId;
                });
                
                if (changedApp) {
                    changedApp.coordinator = application.coordinator;
                    changedApp.coordinatorId = application.coordinatorId;
                    changedApp.applicationStatusId = application.applicationStatusId;
                    changedApp.applicationStatusName = application.applicationStatusName;
                    changedApp.applicationDueDate = application.applicationDueDate;
                    changedApp.dueDateString = application.dueDateString;
                }

                this.gridOptions.dataSource.data(this.applications);
            }, () => {
            });
        }

        onAdd(application: services.IApplication) {
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
                            organizations: this.organizations,
                            applications: this.applications,
                            coordinators: this.staff,
                            application: null
                        };
                    }
                }
            });

            instance.result.then((application: services.ICoordinatorApplication) => {
                this.applications.push(application);
                this.gridOptions.dataSource.data(this.applications);
            }, () => {
            });
        }

        onDelete(application: services.ICoordinatorApplication) {
            if (!confirm("Are you sure you want to cancel this application?")) return;

            this.common.showSplash();
            this.applicationService.cancelApplication(application.applicationUniqueId)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        _.remove(this.applications, (app) => {
                            return app.applicationUniqueId === application.applicationUniqueId;
                        });

                        this.gridOptions.dataSource.data(this.applications);
                        this.notificationFactory.success("Application cancelled successfully.");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error cancelling Application");
                    this.common.hideSplash();
                });
        }

        editOrganization(dataItem: app.services.ICoordinatorApplication) {
            this.organizationService.getById(dataItem.organizationId)
                .then((data: app.services.IOrganization) => {

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
                                return dataItem.organizationId;
                            },
                            organization: () => {
                                return data;
                            },
                            facilities: () => {
                                return null;
                            },
                            users: () => {
                                return null;
                            }
                        }
                    });
                });

        }
        
    }

    angular
        .module('app.application')
        .controller('app.application.ApplicationManagementController',
        ApplicationManagement);
} 