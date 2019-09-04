module app.admin {
    'use strict';

    class AuditorObserverManagementController {
        results: Array<services.IUser>;
        allUsers: Array<services.IUser>;
        search = "";
        eligibility = true;
        compliance = true;
        annual = true;
        renewal = true;
        gridOptions = {
            sortable: true,
            selectable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "fullName", title: "Name" },
                { field: "userType", title: "Type" },
                { template: "<button class=\"k-button\" ng-click=\"vm.onEdit(dataItem)\">Edit</button><button class=\"k-button\" ng-click=\"vm.onRemove(dataItem)\">Delete</button>" }
            ],
            pageable: {
                pageSize: 10
            }
        };


        static $inject = [
            '$uibModal',
            'userService',
            'notificationFactory',
            'config',
            'common'
        ];
        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private userService: app.services.IUserService,
            private notificationFactory: app.blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory) {

            common.activateController([this.getApplications()], '');
        }

        getApplications(): ng.IPromise<void> {
            return this.userService.getAllUsers(true)
                .then((data: Array<app.services.IUser>) => {
                    this.allUsers = data;

                    this.getUsers();
                })
                .catch(() => {
                    this.notificationFactory.error("Error while getting users.");
                });
        }

        getUsers() {
            var users = [];

            _.each(this.allUsers, (app) => {


                if (app.isAuditor || app.isObserver) {
                    users.push(app);
                    app.userType = app.isObserver ? "Observer" : "Auditor";
                } else {
                    app.userType = "";
                }


            });

            this.results = users;
            this.gridOptions.dataSource.data(users);
        }

        onEdit(user: services.IUser) {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/auditorObserver.html",
                controller: "app.modal.templates.AuditorObserverController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            users: this.allUsers,
                            user: user
                        };
                    }
                }
            });

            instance.result.then((u: services.IUser) => {
                var foundUser = _.find(this.allUsers, (app) => {
                    return app.userId === u.userId;
                });

                if (foundUser) {
                    foundUser.isAuditor = u.isAuditor;
                    foundUser.isObserver = u.isObserver;
                }

                this.getUsers();

                this.notificationFactory.success("Successfully saved auditor/observer.");
            }, () => {
            });
        }

        onAdd() {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/auditorObserver.html",
                controller: "app.modal.templates.AuditorObserverController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            users: this.allUsers,
                            user: null
                        };
                    }
                }
            });

            instance.result.then((u: services.IUser) => {
                var foundUser = _.find(this.allUsers, (app) => {
                    return app.userId === u.userId;
                });

                if (foundUser) {
                    foundUser.isAuditor = u.isAuditor;
                    foundUser.isObserver = u.isObserver;
                }

                this.getUsers();

                this.notificationFactory.success("Successfully saved auditor/observer.");
            }, () => {
            });
        }

        onRemove(user: services.IUser) {
            if (!confirm("Are you sure you want to remove the Auditor/Observer from this user?")) return;

            this.common.showSplash();
            this.userService.setAuditorObserver(user.userId, false, false)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        var found = _.find(this.allUsers, (u) => {
                            return u.userId === user.userId;
                        });

                        if (found) {
                            found.isObserver = false;
                            found.isAuditor = false;
                        }

                        this.getUsers();

                        this.notificationFactory.success("Auditor/Observer removed successfully.");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving data. Please contact support");
                    this.common.hideSplash();
                });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.AuditorObserverManagementController',
        AuditorObserverManagementController);
} 
