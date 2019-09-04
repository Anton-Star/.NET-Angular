module app.modal.templates {
    'use strict';

    interface IValues {
        title: string;
        text: string;
        buttonText: string;
    }

    class DisplayController {

        static $inject = [
            '$scope',
            '$timeout',
            '$uibModal',
            'notificationFactory',
            'common',
            'config',
            'currentUser',
            '$uibModalInstance',
            'values'
        ];

        constructor(
            private $scope: ng.IScope,
            private $timeout: ng.ITimeoutService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private currentUser: services.IUser,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private values: IValues) {
        }

        onSave(): void {
            this.$uibModalInstance.close();
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.DisplayController',
        DisplayController);
} 