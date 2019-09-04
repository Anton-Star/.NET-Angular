module app.Compliance {
    'use strict';

    class ScheduleController {
        application: services.IComplianceApplication;
        compAppId = "";
        accessType = "";

        static $inject = [
            '$location',
            '$uibModal',
            'config',
            'applicationService',
            'notificationFactory',
            'common'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private config: IConfig,
            private applicationService: services.IApplicationService,
            private notificationFactory: blocks.INotificationFactory,
            private common: app.common.ICommonFactory) {

            this.compAppId = $location.search().c;

            if (this.compAppId !== "") {
                this.getAccess();
            }

        }

        getApplicationById(): ng.IPromise<void> {
            return this.applicationService.getComplianceApplicationById(this.compAppId, false)
                .then((data: services.IComplianceApplication) => {
                    this.application = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data");
                });
        }

        getAccess(): ng.IPromise<void> {
            return this.applicationService.getComplianceApplicationAccess(this.compAppId)
                .then((accessType: string) => {
                    if (accessType != null && accessType !== "") {
                        this.accessType = accessType;
                        this.common.activateController([this.getApplicationById()], '');
                    } else {
                        this.$location.path('/').search({ x: 'u', url: this.$location.url() });
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting access");
                    this.$location.path('/');
                });
        }

        checkForAccess(application: services.IApplication): ng.IPromise<void> {
            return this.applicationService.getApplicationAccess(application.uniqueId)
                .then((accessType: string) => {
                    application.accessType = accessType;
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        submit(): void {

            var found = _.find(this.application.applications, (app) => {
                return app.applicationTypeName.indexOf("CB") > -1;
            });

            this.common.showSplash();

            this.applicationService.sendForInspection(this.compAppId, this.application.organizationName, this.application.coordinator.userId, found != null)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application submitted for inspection successfully.");
                        
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.common.hideSplash();
                });
        }

    }

    angular
        .module('app.inspection')
        .controller('app.inspection.ScheduleController',
        ScheduleController);
}  