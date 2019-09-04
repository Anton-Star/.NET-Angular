module app.modal.templates {
    'use strict';

    class BugController {
        comment: string;

        static $inject = [
            '$location',
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'bugService'
        ];

        constructor(
            private $location: ng.ILocationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private bugService: services.IBugService) {

            console.log($location.absUrl());
        }

        onCancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }

        onSave(): void {
            this.common.showSplash();

            this.bugService.addBug(this.comment, this.$location.absUrl(), this.$location.search().app)
                .then(() => {
                    this.notificationFactory.success("Bug Saved Successfully");
                    this.common.hideSplash();
                    this.onCancel();
                })
                .catch((e) => {
                    this.notificationFactory.error("Error saving bug: " + e);
                    this.common.hideSplash();
                });
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.BugController',
        BugController);
} 