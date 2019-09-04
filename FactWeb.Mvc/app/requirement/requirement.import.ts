module app.requirement {
    'use strict';

    class ImportController {
        applicationTypes: Array<services.IApplicationType>;
        requirementSettingName: string;
        selectedApplicationType: string = "";
        versionName: string = "";
        versionNumber: string = "";
        file: any;

        static $inject = [
            'applicationService',
            'requirementService',
            'cacheService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private applicationService: services.IApplicationService,
            private requirementService: services.IRequirementService,
            private cacheService: services.ICacheService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            common.activateController([this.getApplicationTypes(), this.getAppSettings()], '');
        }

        getAppSettings(): ng.IPromise<void> {
            return this.cacheService.getApplicationSettings()
                .then((data) => {
                    var setting = _.find(data, (setting: services.IApplicationSetting) => {
                        return setting.name === "Requirement Management Set Name";
                    });

                    if (setting) {
                        this.requirementSettingName = setting.value;
                    }
                });
        }

        getApplicationTypes(): ng.IPromise<void> {
            return this.applicationService.getApplicationTypes()
                .then((data: Array<services.IApplicationType>) => {
                    this.applicationTypes = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting types. Please contact support.");
                });
        }

        onUpload(): void {
            if (!confirm("This is a long running process. Are you sure you want to continue?")) return;

            this.common.showSplash();

            this.requirementService.import(this.file, this.selectedApplicationType, this.versionName, this.versionNumber)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Version imported successfully.");
                    }
                    
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to import. Please contact support.");
                    this.common.hideSplash();
                });
        }
        

    }

    angular
        .module('app.requirement')
        .controller('app.requirement.ImportController',
        ImportController);
} 