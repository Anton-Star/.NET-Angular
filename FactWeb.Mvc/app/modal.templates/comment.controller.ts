module app.modal.templates {
    'use strict';

    interface IValues {
        title: string
    }

    class CommentController {
        comment: string;

        static $inject = [
            '$uibModal',
            'notificationFactory',
            'common',
            'config',
            '$uibModalInstance',
            'values'
        ];

        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private values: IValues) {
        }

        onCancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
        
        onSave(): void {
           this.$uibModalInstance.close(this.comment);
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.CommentController',
        CommentController);
} 