module app.admin {
    'use strict';

    interface IManageUsersScope {
        getAll: () => void;
        addUser: () => void;
        editUser: (rowData: services.IUser) => void;
    }

    class ManageUsersController implements IManageUsersScope {
        results: Array<services.IUser>;
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
                pageSize: 100
            }),
            pageable: {
                pageSize: 10
            },
            columns: [
                { field: "fullName", title: "Full Name", width: "320px", template: $("#fullName").html() },
                { field: "emailAddress", title: "Email Address", width: "200px", template: $("#email").html() },
                { field: "orgs", title: "Organization", template: $("#org").html() },
                { field: "role.roleName", title: "Role", width: "200px" },
                {
                    template: "<button class=\"k-button\" ng-click=\"vm.editUser(dataItem)\">Edit </button>"
                }
            ]
        };

        static $inject = [
            '$location',
            '$window',
            '$uibModal',
            'userService',
            'notificationFactory',
            'common',
            'config',
            'modalHelper'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private userService: app.services.IUserService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private modalHelper: common.IModalHelper) {
            
            this.checkPermissions();
        }

        checkPermissions() {
            this.userService.checkEditPermissions()
                .then((data: boolean) => {
                    if (data) {
                        this.common.activateController([this.getAll()], '');
                    } else {
                        this.$location.path('/').search({ x: 'u', url: this.$location.url() });
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting users. Please contact admin.");
                });
        }

        getAll(): ng.IPromise<void> {
            return this.userService.getAllUsersWithOrganization()
                .then((data: Array<app.services.IUser>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Users found');
                    } else {
                        _.each(data, (user) => {
                            user.orgs = "";
                            _.each(user.organizations, (org) => {
                                user.orgs += org.organization.organizationName + ",";
                            });
                        });
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting users. Please contact admin.");
                });
        }

        addUser(): void {
            var values = {
                parentUser: () => {
                    return null;
                },
                isNewUser: () => {
                    return true;
                },
                currentOrganization: () => {
                    return null;
                },
                role: () => {
                    return "";
                },
                isPersonnel: () => {
                    return false;
                }
            };           

            this.modalHelper.showModal("/app/modal.templates/editUser.html", "app.modal.templates.EditUserController", values)
                .then((data:any) => {
                    this.getAll();
                })
                .catch(() => {
                });

        }

        editUser(rowData): void {
            var values = {
                
                parentUser: () => {
                    return rowData;
                },
                isNewUser: () => {
                    return false;
                },
                currentOrganization: () => {
                    return null;
                },
                role: () => {
                    return "";
                },
                isPersonnel: () => {
                    return false;
                }
                
            };

            this.modalHelper.showModal("/app/modal.templates/editUser.html", "app.modal.templates.EditUserController", values)
                .then((data: any) => {
                    this.getAll();
                })
                .catch(() => {
                });
        }

        onGetOrg(org: services.IOrganization) {
            event.stopPropagation();

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
                        return org.organizationId;
                    },
                    organization: () => {
                        return org;
                    },
                    facilities: () => {
                        return null;
                    },
                    users: () => {
                        return this.results;
                    }
                }
            });

            instance.result.then(() => {
                this.getAll();
            }, () => {
            });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.ManageUsersController',
        ManageUsersController);
}