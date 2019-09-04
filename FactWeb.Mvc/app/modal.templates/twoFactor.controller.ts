module app.home {
    'use strict';

    class TwoFactorController {
        code: string;
        hasError = false;

        static $inject = [
            'localStorageService',
            '$uibModal',
            '$uibModalInstance',
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
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private $rootScope: ng.IRootScopeService,
            private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            private $cookies: ng.cookies.ICookiesService,
            private accountService: app.services.IAccountService,
            private notificationFactory: app.blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory) {

            this.common.activateController([this.getTwoFactor()], '');
        }

        getTwoFactor(): ng.IPromise<void> {
            return this.accountService.getTwoFactor()
                .then(() => {
                    this.notificationFactory.success("Two Factor Code sent to your email.");
                })
                .catch((e) => {
                    console.log(e);
                    this.notificationFactory.error("Error sending Two Factor Code. Please Contact Support.");
                });
        }

        onSubmit() {
            this.common.showSplash();

            this.accountService.validateTwoFactor(this.code)
                .then((response: string) => {
                    if (response) {
                        this.$uibModalInstance.close(response);
                    } else {
                        this.hasError = true;
                    }

                    this.common.hideSplash();
                })
                .catch((e) => {
                    console.log(e);
                    this.notificationFactory.error("Error validating Two Factor Code. Please contact support.");
                });
        }
        
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.TwoFactorController',
        TwoFactorController);
} 