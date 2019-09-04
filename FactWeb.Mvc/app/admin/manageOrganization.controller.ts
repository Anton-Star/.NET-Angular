module app.admin {
    'use strict';

    interface IManageOrganizationScope {
        getAll: () => void;
        editOrganization: (organization: services.IOrganization) => void;
    }

    class ManageOrganizationController implements IManageOrganizationScope {
        results: Array<services.IOrganization>;
        facilities: services.IFacility[];
        users: services.IUser[];
        gridOptions = {
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
                pageSize: 100
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$window',
            '$uibModal',
            'cacheService',
            'facilityService',
            'organizationService',
            'userService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private facilityService: services.IFacilityService,
            private organizationService: services.IOrganizationService,
            private userService: services.IUserService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Manage Organization",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Manage Organization', isActive: true }
                ]
            });
            this.getFacilities();
            this.getUsers();

            common.activateController([this.getAll()], 'manageOrganizationController');
        }

        getAll(): ng.IPromise<void> {
            return this.organizationService.getFlatOrganizations()
                .then((data: Array<app.services.IOrganization>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Organizations found');
                    } else {
                        _.each(data, (org) => {
                            org.organizationFormattedId = "O00000".substr(0, 6 - org.organizationId.toString().length) + org.organizationId;
                            org.accreditationStatusName = org.accreditationStatusItem
                                ? org.accreditationStatusItem.name
                                : "";
                        });
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getFacilities(): ng.IPromise<void> {
            return this.facilityService.getAllFlat()
                .then((data: services.IFacility[]) => {
                    this.facilities = data;
                });
        }

        getUsers(): ng.IPromise<void> {
            return this.userService.getAllUsers(false)
                .then((data: Array<app.services.IUser>) => {

                    if (data == null) {
                        this.notificationFactory.error("No Users records found.");
                    } else {
                        data = _.filter(data, (u) => {
                            return u.isActive;
                        });
                        data.sort((a, b) => {
                            var nameA = a.lastName.toLowerCase(), nameB = b.lastName.toLowerCase();
                            if (nameA < nameB) //sort string ascending
                                return -1;
                            if (nameA > nameB)
                                return 1;
                            return 0; //default return value (no sorting)
                        });

                        this.users = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        editOrganization(organization: services.IOrganization): void {
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
                        return organization.organizationId;
                    },
                    organization: () => {
                        return organization.facilities == null ? null : organization;
                    },
                    facilities: () => {
                        return this.facilities;
                    },
                    users: () => {
                        return this.users;
                    }
                }
            });

            instance.result.then((data) => {
                this.users = data.users;
                
                this.facilities = data.facilities;
                //this.getAll();
            }, (data) => {
                this.users = data.users;
                this.facilities = data.facilities;
            });

        }

        onAddNew() {
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
                        return null;
                    },
                    organization: () => {
                        return {};
                    },
                    facilities: () => {
                        return this.facilities;
                    },
                    users: () => {
                        return this.users;
                    }
                }
            });

            instance.result.then((facilities) => {
                this.facilities = facilities;
                this.getAll();
            }, (facilities) => {
                this.facilities = facilities;
            });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.ManageOrganizationController',
        ManageOrganizationController);
}