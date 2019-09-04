module app.modal.templates {
    'use strict';

    interface IValues {
        to: string;
        cc: string;
        subject: string;
        html: string;
        type: string;
    }

    class BuildEmailController {
        disabled = {
            to: false,
            cc: false,
            subject: false
        };
        hidden = {
            to: false,
            cc: false,
            subject: false,
            accReport: true
        }
        includeAccreditationReport = false;

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

            switch (this.values.type) {
                case this.config.emailTypes.sendToInspector:
                    this.disabled.to = true;
                    this.disabled.cc = true;
                    this.hidden.accReport = false;
                    this.includeAccreditationReport = false;
                    break;
                case this.config.emailTypes.backToRfi:
                    this.hidden.to = false;
                    this.hidden.cc = false;
                    this.hidden.subject = false;
                    this.hidden.accReport = false;
                    this.includeAccreditationReport = false;
                    break;
                case this.config.emailTypes.annualComplete:
                    this.hidden.to = true;
                    this.hidden.cc = true;
                    this.hidden.subject = true;
                    this.hidden.accReport = true;
                    break;
            }
        }

        onSave(): void {
            this.$uibModalInstance.close({
                html: this.values.html,
                to: this.values.to,
                cc: this.values.cc,
                subject: this.values.subject,
                includeAccreditationReport: this.includeAccreditationReport
            });
        }

        onCancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.BuildEmailController',
        BuildEmailController);
} 