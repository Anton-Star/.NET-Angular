module app.modal.templates {
    'use strict';

    interface IRouteRoute {
        originalPath: string;
    }

    interface INgRouteService {
        current: INgRoute;
    }

    interface INgRoute extends INgRouteService {
        $$route: IRouteRoute;
    }

    class LegendController {
        statuses: services.IApplicationResponseStatusItem[];

        static $inject = [
            '$route',
            '$uibModal',
            'applicationResponseStatusService',
            'common',
            'config',
            '$uibModalInstance'
        ];

        constructor(
            private $route: INgRouteService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private applicationResponseStatusService: services.IApplicationResponseStatusService,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            common.activateController([this.getStatuses()], '');
        }

        getStatuses(): ng.IPromise<void> {
            return this.applicationResponseStatusService.getApplicationResponseStatus()
                .then((data) => {
                    this.statuses = [];

                    if (this.$route.current.$$route.originalPath.indexOf("Reviewer") > -1) {
                        this.statuses = _.filter(data, (d) => {
                            return d.name === this.config.applicationResponseStatuses.forReview ||
                                d.name === this.config.applicationResponseStatuses.reviewed ||
                                d.name === this.config.applicationResponseStatuses.completed ||
                                d.name === this.config.applicationResponseStatuses.rfiCompleted ||
                                d.name === this.config.applicationResponseStatuses.compliant ||
                                d.name === this.config.applicationResponseStatuses.notCompliant ||
                                d.name === this.config.applicationResponseStatuses.rfi ||
                                d.name === this.config.applicationResponseStatuses.new ||
                                d.name === this.config.applicationResponseStatuses.rfiFollowup ||
                                d.name === this.config.applicationResponseStatuses.na ||
                                d.name === this.config.applicationResponseStatuses.noResponseRequested;
                        });
                    } else {
                        this.statuses = _.filter(data, (d) => {
                            return d.name === this.config.applicationResponseStatuses.completed ||
                                d.name === this.config.applicationResponseStatuses.rfiCompleted ||
                                d.name === this.config.applicationResponseStatuses.compliant ||
                                d.name === this.config.applicationResponseStatuses.rfi ||
                                d.name === this.config.applicationResponseStatuses.new ||
                                d.name === this.config.applicationResponseStatuses.rfiFollowup ||
                                d.name === this.config.applicationResponseStatuses.na ||
                                d.name === this.config.applicationResponseStatuses.noResponseRequested;
                        });
                    }
                });
        }

        onClose(): void {
            this.$uibModalInstance.close();
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.LegendController',
        LegendController);
}  