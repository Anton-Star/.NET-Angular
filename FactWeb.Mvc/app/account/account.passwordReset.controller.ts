module app.account {
    'use strict';

    interface IOrgType {
        name: string;
        title: string;
    }

    interface IResetScope {
        user: services.IUser;
        password: string;
        passwordConfirm: string;
        token: string;
        save: () => void;
        checkFields: () => boolean;
    }

    class ResetController implements IResetScope {
        user: services.IUser;
        password: string;
        passwordConfirm: string;
        token: string;

        static $inject = [
            '$window',
            '$location',
            'userService',
            'accountService',
            'notificationFactory',
            'currentUser',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            private userService: services.IUserService,
            private accountService: services.IAccountService,
            private notificationFactory: blocks.INotificationFactory,
            private currentUser: services.IUser,
            private common: common.ICommonFactory,
            private config: IConfig) {

            this.token = $location.search().token;

            common.activateController([this.getUser()], 'resetController');
        }

        getUser(): ng.IPromise<void> {
            return this.userService.getByToken(this.token)
                .then((data: services.IUser) => {
                    this.user = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to find user. Please contact support.");
                });
        }

        checkFields(): boolean {
            return this.password === "" || this.passwordConfirm === "" || this.passwordConfirm !== this.password;
        }

        validatePassword(): void {
            if (this.common.isValidPassword(this.password) == true) {
                this.notificationFactory.warning("Passwords must be at least 8 characters in length with at least 1 numeric, and special character.");
            }
        }

        isInvalidPassword(): boolean {
            var hasError = false;
            if (this.common.isValidPassword(this.password) == true) {
                hasError = true;
            }
            
            return hasError;
        }

        save(): void {
            this.common.showSplash();

            this.accountService.updatePassword(this.token, this.password, this.passwordConfirm)
                .then((data: app.services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Password has been successfully updated.");
                        this.$location.path('/');
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to change password. Please contact support.");
                    this.common.hideSplash();
                });
        }
    }

    angular
        .module('app.account')
        .controller('app.account.ResetController',
        ResetController);
} 