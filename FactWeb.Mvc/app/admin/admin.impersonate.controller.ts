module app.admin {
    'use strict';

    class ImpersonateController {
        users: Array<services.IUser>;
        user: string;

        static $inject = [
            '$location',
            'accountService',
            'userService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $location: ng.ILocationService,
            private accountService: services.IAccountService,
            private userService: services.IUserService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {

            common.activateController([this.getUsers()], '');
        }

        getUsers(): ng.IPromise<void> {
            return this.userService.getUsersForImpersonation()
                .then((data: Array<app.services.IUser>) => {
                    var found = _.find(data, (d) => {
                        return d.fullName == 'Ceballos, Darisabel';
                    })
                    console.log('users', data, found);
                    this.users = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting users. Please contact system admin.");
                });
        }

        onImpersonate() {
            this.common.showSplash();

            console.log(this.user);

            this.accountService.impersonate(this.user)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.common.currentUser = data.item;
                        this.common.currentUser.isImpersonation = true;
                        this.common.$broadcast(this.config.events.userImpersonated, {
                            fullName: this.common.currentUser.firstName + " " + this.common.currentUser.lastName,
                            orgName: this.common.currentUser.organizations && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "",
                            roleName: this.common.currentUser.role.roleName
                        });

                        if (this.common.currentUser.role.roleName === this.config.roles.inspector) {
                            this.$location.path('/Inspector/Inspections');
                        } else if (this.common.currentUser.role.roleName === this.config.roles.factAdministrator || this.common.currentUser.role.roleName.indexOf(this.config.roles.factCoordinator) > -1) {
                            this.$location.path("/Coordinator/Applications");
                        } else {
                            this.$location.path('/Overview');
                        } 
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }
        
    }

    angular
        .module('app.admin')
        .controller('app.admin.ImpersonateController',
        ImpersonateController);
} 