module app.Eligibility {
    'use strict';

    export interface IApplicationGroupByOrg {
        organizationName: string;
        applications: Array<services.IApplication>;
    }

    export interface IPastApps {
        org?: string;
        apps?: services.IApplication[];
    }

    export interface IAppCache {
        userId: string;
        date: Date;
        results: services.IApplication[];
        sorted: services.IApplication[];
        pastApps: IPastApps[];
    }

    class HomeController {
        results: Array<services.IApplication>;
        previousApplications: Array<IApplicationGroupByOrg>;
        sorted: Array<services.IApplication>;
        showCoordinatorView = false;
        pastApps: IPastApps[] = [];
        dataAsOf: Date = new Date();


        static $inject = [
            '$rootScope',
            '$location',
            'localStorageService',
            'applicationService',
            'notificationFactory',
            'config',
            'common'
        ];
        constructor(
            private $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private localStorageService: any,
            private applicationService: app.services.IApplicationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory
        ) {    

            var _this = this;

            if (common.currentUser) {
                this.init();
            } else {
                this.$rootScope.$on(this.config.events.userLoggedIn, (data: any, args: any) => {
                    _this.init();
                });    
            }            
        }

        init(): void {
            var hasData = false;

            if (this.localStorageService.get("apps") != null && this.localStorageService.get("apps") !== "") {
                var data = this.localStorageService.get("apps");

                var appCache: IAppCache = JSON.parse(data);

                if (this.common.currentUser && this.common.currentUser.userId == appCache.userId) {
                    var now = moment().add(-30, 'minutes').toDate();
                    var dte = moment(appCache.date).toDate();

                    if (now < dte) {
                        this.results = appCache.results;
                        this.sorted = appCache.sorted;
                        this.pastApps = appCache.pastApps;
                        hasData = true;
                        this.dataAsOf = dte;
                    }
                }
            } 

            if (!hasData) {
                this.getAllApplications();
            }

            this.showCoordinatorView = this.common.currentUser.role.roleName === this.config.roles.factAdministrator ||
                this.common.currentUser.role.roleName === this.config.roles.factCoordinator;
        }

        getAllApplications(): ng.IPromise<void> {
            this.common.showSplash();
            return this.applicationService.getAllApplications()
                .then((data: Array<app.services.IApplication>) => {
                    
                    if (data == null) {
                        this.notificationFactory.error('Not Application found.');
                    } else {
                        this.results = data;
                        
                        this.sorted = data;
                        this.sorted.sort(function (a, b) {
                            var nameA = a.organizationName.toLowerCase(), nameB = b.organizationName.toLowerCase()
                            if (nameA < nameB) //sort string ascending
                                return -1
                            if (nameA > nameB)
                                return 1
                            return 0 //default return value (no sorting)
                        })

                        var apps = _.filter(this.sorted, (app) => {
                            return app.applicantApplicationStatusName.indexOf('Declined') !== -1 ||
                                app.applicantApplicationStatusName.indexOf('Complete') !== -1 ||
                                app.applicantApplicationStatusName.indexOf('Cancelled') !== -1;
                        });

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

                        this.localStorageService.set("apps", JSON.stringify({
                            userId: this.common.currentUser.userId,
                            results: this.results,
                            sorted: this.sorted,
                            pastApps: this.pastApps,
                            date: new Date()
                        }));

                        this.dataAsOf = new Date();
                    }

                    this.common.hideSplash();
                    
                })
                .catch(() => {
                    this.notificationFactory.error("Error while getting applications.");
                    this.common.hideSplash();
                });
        }
        
        showOrganization(orgName: string,i:number): boolean
        {
            if (i === 0) return true;

            if (i < this.sorted.length) {
                if (orgName == this.sorted[i-1].organizationName) {
                    return false;
                }
                else
                    return true;
            }
            return false;
        }
        
    }

    angular
        .module('app.eligibility')
        .controller('app.eligibility.HomeController',
        HomeController);
} 