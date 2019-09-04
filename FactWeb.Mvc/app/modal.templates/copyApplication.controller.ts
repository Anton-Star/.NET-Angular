module app.modal.templates {
    'use strict';

    interface IValues {
        application: services.IComplianceApplication;
        sites: services.ISiteApp[];
        applicationStatuses: services.IApplicationStatus[];
    }

    class CopyApplicationController {
        sitesWithApps: services.ISite[] = [];
        copyFrom = "";
        applications: services.IApplication[] = [];
        sites: services.ISiteApp[] = [];
        application = "";
        app: services.IApplication;
        copyTo = "";
        applicationStatus = "";
        deleteOriginal = false;

        static $inject = [
            '$scope',
            '$timeout',
            '$uibModal',
            'applicationService',
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
            private applicationService: services.IApplicationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private currentUser: services.IUser,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private values: IValues) {

            _.each(values.application.complianceApplicationSites, (site: services.IComplianceApplicationSite) => {
                if (site.applications && site.applications.length > 0) {
                    var found = _.find(this.sitesWithApps, (s: services.ISite) => {
                        return s.siteId === site.site.siteId;
                    });

                    if (!found) {
                        this.sitesWithApps.push(site.site);
                    }
                }
            });

        }

        onCopyFrom() {
            this.applications = [];

            var site = _.find(this.values.application.complianceApplicationSites, (site: services.IComplianceApplicationSite) => {
                return site.site.siteName === this.copyFrom;
            });

            if (site) {
                this.applications = site.applications;
            }

            this.onAppSelected();
        }

        onAppSelected() {
            this.sites = [];

            if (this.copyFrom === "" || this.application === "") {
                return;
            }

            this.app = _.find(this.applications, (app: services.IApplication) => {
                return app.applicationTypeName = this.application;
            });

            this.applicationStatus = this.app.applicationStatusName;
            
            _.each(this.values.sites, (site: services.ISiteApp) => {
                if (site.site !== this.copyFrom) {
                    var found = _.find(site.types, (t: services.ISiteType) => {
                        return t.version.applicationType.applicationTypeName === this.app.applicationTypeName && !t.hasType;
                    });

                    if (!found || !found.hasType) {
                        this.sites.push(site);
                    }
                }
            });
        }

        onSave(): void {
            this.common.showSplash();

            var model: services.ICopyComplianceApplicationModel = {
                complianceApplicationId: this.values.application.id,
                copyFromSite: this.copyFrom,
                copyToSite: this.copyTo,
                applicationType: this.application,
                applicationStatus: this.applicationStatus,
                deleteOriginal: this.deleteOriginal
            };

            this.applicationService.copyApplication(model)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application successfully copied.");
                        this.$uibModalInstance.close({
                            app: data.item
                        });
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving data. Please contact support");
                    this.common.hideSplash();
                });

            
        }

        onCancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.CopyApplicationController',
        CopyApplicationController);
} 