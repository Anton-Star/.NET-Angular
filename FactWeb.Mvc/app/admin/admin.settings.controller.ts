module app.admin {
    'use strict';

    interface IOrgType {
        name: string;
        title: string;
    }

    interface ISettingsScope {
        settings: Array<services.IApplicationSetting>;
        save(): void;
    }

    class SettingsController implements ISettingsScope {
        settings: Array<services.IApplicationSetting>;

        static $inject = [
            '$window',
            '$uibModal',
            'applicationSettingService',
            'cacheService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private applicationSettingService: services.IApplicationSettingService,
            private cacheService: services.ICacheService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            common.activateController([this.getSettings()], 'SettingsController');
        }

        getSettings(): ng.IPromise<void> {
            return this.cacheService.getApplicationSettings()
                .then((data: Array<services.IApplicationSetting>) => {
                    this.settings = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting settings. Please contact support.");
                });
        }

        save(): void {
            this.common.showSplash();

            this.applicationSettingService.save(this.settings)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application Settings saved successfully.");
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
        .module('app.admin')
        .controller('app.admin.SettingsController',
        SettingsController);
} 