module app.modal.templates {
    'use strict';

    class YesNoController {
        static $inject = [
            '$uibModalInstance',
            'message'
        ];

        constructor(
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private message: string) {
        }

        onYes(): void {
            this.$uibModalInstance.close(true);
        }

        onNo(): void {
            this.$uibModalInstance.close(false);
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.YesNoController',
        YesNoController);
} 