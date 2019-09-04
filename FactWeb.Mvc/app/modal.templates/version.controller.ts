module app.modal.templates {
    'use strict';

    interface IVersionScope {
        isBusy: boolean;
        copyFromVersionId: string;
        version: services.IApplicationVersion;
    }

    class VersionController implements IVersionScope {
        isBusy = false;
        copyFromVersionId = "";
        version: services.IApplicationVersion = {
            title: "",
            versionNumber: "",
            isActive: false,
            copyFromId: "",
            applicationType: {
                applicationTypeId: 0,
                applicationTypeName: ""
            }
        };

        static $inject = [
            'notificationFactory',
            'common',
            'config',
            '$uibModalInstance',
            'versionService',
            'applicationTypeName',
            'versions'
        ];

        constructor(
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private versionService: services.IVersionService,
            private applicationTypeName: string,
            private versions: Array<services.IApplicationVersion>) {

            this.version.applicationType.applicationTypeName = applicationTypeName;
        }

        save(): void {
            this.isBusy = true;
            this.versionService.add(this.version)
                .then((data: services.IGenericServiceResponse<services.IApplicationVersion>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                        this.isBusy = false;
                    } else {
                        this.notificationFactory.success("Requirement Saved successfully.");
                        this.$uibModalInstance.close(data.item);
                        this.isBusy = false;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to connect with web service. Please contact support.");
                    this.isBusy = false;
                });
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.VersionController',
        VersionController);
} 