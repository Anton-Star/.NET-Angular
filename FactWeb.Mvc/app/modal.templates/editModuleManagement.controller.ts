module app.modal.templates {
    'use strict';

    class EditModuleManagementController {
        static $inject = [
            'applicationService',
            'notificationFactory',
            'common',
            'module',
            '$uibModalInstance'
        ];

        constructor(
            private applicationService: services.IApplicationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private module: services.IApplicationType,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {
            
            
        }

        save(): void {
            this.common.showSplash();
            this.applicationService.saveApplicationType(this.module)
                .then((data: services.IGenericServiceResponse<services.IApplicationType>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application Type Saved Successfully.");
                        this.$uibModalInstance.close(this.module);
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.common.hideSplash();
                });
            
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.EditModuleManagementController',
        EditModuleManagementController);
} 