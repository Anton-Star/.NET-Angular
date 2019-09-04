module app.Inspector {
    'use strict';

    interface IPastApps {
        org?: string;
        apps?: services.IApplication[];
    }

    class InspectionsController {
        results: Array<services.IApplication>;
        pastApps: IPastApps[] = [];

        static $inject = [
            'applicationService',
            'notificationFactory',
            'config',
            'common'
        ];
        constructor(
            private applicationService: app.services.IApplicationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory) {

            common.activateController([this.getAllApplications()], '');
        }

        getAllApplications(): ng.IPromise<void> {
            return this.applicationService.getInspectorApplications()
                .then((data: Array<app.services.IApplication>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Application found.');
                    } else {

                        var apps = [];
                        this.results = [];

                        _.each(data, (app) => {
                            if (app.applicationStatusName.indexOf('Declined') !== -1 ||
                                app.applicationStatusName.indexOf('Complete') !== -1 ||
                                app.applicationStatusName.indexOf('Cancelled') !== -1) {

                                apps.push(app);
                            } else {
                                this.results.push(app);
                            }
                        });

                        if (this.results.length > 0) {
                            this.common.$broadcast(this.config.events.coordinatorSet, { coordinator: this.results[0].coordinator });    
                        }
                        
                        var orgs = _.uniqBy(apps, (a) => {
                            return a.organizationName;
                        });

                        _.each(orgs, (o) => {
                            var pastApp: IPastApps = {
                                org: o.organizationName,
                                apps: _.filter(apps, (a) => {
                                    return a.organizationName === o.organizationName
                                })
                            };

                            this.pastApps.push(pastApp);
                        });

                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error while getting applications.");
                });
        }
    }

    angular
        .module('app.inspector')
        .controller('app.inspector.InspectionsController',
        InspectionsController);
} 