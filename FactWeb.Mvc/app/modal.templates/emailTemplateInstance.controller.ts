module app.modal.templates {
    'use strict';

    class EmailTemplateInstanceController {
        static $inject = [
            '$scope',
            '$timeout',
            '$uibModal',
            'cacheService',
            'notificationFactory',
            'common',
            'config',
            'currentUser',
            '$uibModalInstance',
            'template'
        ];

        constructor(
            private $scope: ng.IScope,
            private $timeout: ng.ITimeoutService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private currentUser: services.IUser,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private template: string) {
        }

        onSave(): void {
            this.$uibModalInstance.close({
                body: this.template
            });
        }

        onCancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.EmailTemplateInstanceController',
        EmailTemplateInstanceController);
} 