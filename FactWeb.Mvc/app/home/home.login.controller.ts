module app.home {
    'use strict';

    interface ILoginScope {
        username: string;
        password: string;
        rememeberMe: boolean;
        loginFailure: boolean;
        failedAttempts: number;
        login: () => void;
    }

    class LoginController implements ILoginScope {
        username: string;
        password: string;
        rememeberMe = false;
        loginFailure = false;
        failedAttempts = 0;
        redirectPage: string;

        static $inject = [
            'localStorageService',
            '$uibModal',
            '$rootScope',
            '$window',
            '$location',
            '$cookies',
            'accountService',
            'notificationFactory',
            'config',
            'common'
        ];
        constructor(
            private localStorageService: any,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $rootScope: ng.IRootScopeService,
            private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            private $cookies: ng.cookies.ICookiesService,
            private accountService: app.services.IAccountService,
            private notificationFactory: app.blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory) {

            this.redirectPage = $location.search().page;
            var unauth = $location.search().x;

            if (unauth) {
                notificationFactory.error("Access Denied");                
            }

            this.username = this.$cookies.get("rememberMe");

            if (this.username != null) {
                this.rememeberMe = true;
            }

            this.localStorageService.set("impersonate", "N");

            this.common.$broadcast(this.config.events.loginPageInit);
        }

        login(): void {            
            this.loginFailure = false;


            if (this.failedAttempts >= 5) {
                this.notificationFactory.error("Your account has been locked out. Please contact a system admin.");
                this.common.hideSplash();
                return;
            } 

            this.common.showSplash();

            this.accountService.getFailedAttempts(this.username)
                .then((data: app.services.IGenericServiceResponse<number>) => {
                    if (data.hasError) {
                        this.failedAttempts = 0;
                    } else {
                        this.failedAttempts = data.item;

                        if (this.failedAttempts >= 5) {
                            this.notificationFactory.error("Your account has been locked out. Please contact a system admin.");
                            this.common.hideSplash();
                            return;
                        } 

                        if (this.rememeberMe) {
                            this.$cookies.put("rememberMe", this.username);
                        }
                        else {
                            this.$cookies.remove("rememberMe");
                        }

                        this.accountService.login(this.username, this.password)
                            .then((data: app.services.IGenericServiceResponse<app.services.IUser>) => {
                                if (data.hasError) {
                                    if (data.message === "Password Expired") {
                                        var mdl = this.$uibModal.open({
                                            animation: true,
                                            templateUrl: "/app/modal.templates/display.html",
                                            controller: "app.modal.templates.DisplayController",
                                            controllerAs: "vm",
                                            size: 'lg',
                                            windowClass: 'app-modal-window',
                                            resolve: {
                                                values: () => {
                                                    return {
                                                        title: "Password Expired",
                                                        text: "To keep your information secure, passwords expire every 6 months. You must reset your password to proceed.",
                                                        buttonText: "Continue"
                                                    }
                                                }
                                            }
                                        });

                                        mdl.result.then(() => {
                                            this.$location.url('/Account/PasswordReminder');
                                        });
                                    } else {
                                        this.notificationFactory.error(data.message);
                                    }
                                    this.common.hideSplash();
                                } else {
                                    if (data.item == null) {
                                        this.loginFailure = true;

                                        this.failedAttempts = this.failedAttempts + 1;
                                        this.setFailedAttempts(this.username);
                                        this.common.hideSplash();
                                    } else {
                                        this.failedAttempts = 0;
                                        this.setFailedAttempts(this.username);

                                        if (this.config.useTwoFactor) {
                                            var tf: string = this.localStorageService.get("codes");
                                            var codes: services.ITwoFactor[] = null;

                                            var found = false;

                                            if (tf) {
                                                codes = JSON.parse(tf);
                                                var code = _.find(codes,
                                                    (c: services.ITwoFactor) => {
                                                        return c.id === data.item.userId;
                                                    });

                                                if (code) {
                                                    found = true;
                                                }
                                            }

                                            if (!found) {
                                                var instance = this.$uibModal.open({
                                                    animation: true,
                                                    templateUrl: "/app/modal.templates/twoFactor.html",
                                                    controller: "app.modal.templates.TwoFactorController",
                                                    controllerAs: "vm",
                                                    size: 'lg',
                                                    windowClass: 'app-modal-window'
                                                });

                                                instance.result.then((response: string) => {
                                                    if (codes) {
                                                        codes.push({
                                                            id: data.item.userId,
                                                            code: response
                                                        });
                                                    } else {
                                                        codes = [{ id: data.item.userId, code: response }];
                                                    }

                                                    this.localStorageService.set("codes", JSON.stringify(codes));
                                                    this.loggedIn(data);
                                                });
                                            } else {
                                                this.loggedIn(data);
                                            }
                                        } else {
                                            this.loggedIn(data);
                                        }

                                        
                                    }
                                }
                            })
                            .catch((e) => {
                                this.notificationFactory.error("Error trying to log in. Please contact support.");
                                this.common.hideSplash();
                            });
                    }
                });

            
        }

        loggedIn(data: app.services.IGenericServiceResponse<app.services.IUser>) {
            console.log('user', data.item);
            this.common.currentUser = data.item;
            this.common.currentUser.isImpersonation = false;
            this.common.$broadcast(this.config.events.userLoggedIn, {
                fullName: this.common.currentUser.firstName + " " + this.common.currentUser.lastName,
                orgName: this.common.currentUser.organizations && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "",
                roleName: this.common.currentUser.role.roleName
            });
            this.common.hideSplash();
            (this.$rootScope as any).is403 = false; //this variable was set to true to prevent multiple notifications in case of HHTP 403

            if ((this.redirectPage) && (this.redirectPage !== "/") && this.redirectPage !== "%2F") {
                this.$location.url(this.redirectPage); //Handle that situation where the redirect after login contains QS.                                
            } else if (this.common.currentUser.role.roleName === this.config.roles.inspector) {
                this.$location.url('/Inspector/Inspections');
            } else if (this.common.currentUser.role.roleName === this.config.roles.factAdministrator ||
                this.common.currentUser.role.roleName.indexOf(this.config.roles.factCoordinator) > -1) {
                this.$location.url("/Coordinator/Applications");
            } else if (this.common.currentUser.role.roleName === this.config.roles.factConsultantCoordinator) {
                this.$location.url("/Coordinator/Applications?a=Y");
            } else {
                this.$location.url('/Overview');
            }
        }

        setFailedAttempts(username: string): void {
            this.accountService.setFailedAttempts(this.username, this.failedAttempts)
                .then((data: app.services.IGenericServiceResponse<number>) => {
                    if (data.hasError) {
                        this.failedAttempts = 0;
                    } else {
                        if (this.failedAttempts == 3) {
                            this.notificationFactory.error("User will get locked after 2 more unsuccessful login attempts.");
                        }
                        else if (this.failedAttempts == 5) {
                            this.notificationFactory.error("User locked. Please contact system admin.");
                        }
                    }
                });
        }
    }

    angular
        .module('app.home')
        .controller('app.home.LoginController',
        LoginController);
} 