module app.admin {
    'use strict';

    interface IManageSitesScope {
        getAll: () => void;
        addSite: () => void;
        editSite: (rowData: services.ISite) => void;
    }

    class ManageSitesController implements IManageSitesScope {
        results: Array<services.IFlatSite>;
        facilityAccredidations: Array<services.IFacilityAccreditationItem>;
        facilities: services.IFacility[];
        localSites: services.ISite[] = [];
        gridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 100
            }),
            columns: [
            ],
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$window',
            '$timeout',
            '$uibModal',
            'cacheService',
            'siteService',
            'notificationFactory',
            'common',
            'config',
            'organizationService',
            'facilityService'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $timeout: ng.ITimeoutService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private siteService: app.services.ISiteService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private organizationService: app.services.IOrganizationService,
            private facilityService: app.services.IFacilityService) {

            this.gridOptions.columns = [
                { field: "siteId", title: "Id", hidden: "true" },
                { field: "siteName", title: "Site Name", width: "300px", template: $("#siteEdit").html() },
                { field: "siteStartDate", title: "Start Date", width: "120px", template: $("#startDate").html() },
                { field: "facilityName", title: "Facility Name", width: "240px", template: $("#facilityEdit").html() },
                { field: "organizationName", title: "Organization Name", width: "240px", template: $("#organizationEdit").html() },
                { template: $("#edit").html() }
            ];


            common.activateController([this.getAll(), this.getFacilities()], 'manageSitesController');
        }

        getAll(): ng.IPromise<void> {
            this.common.showSplash();
            return this.siteService.getFlatSites()
                .then((data: Array<app.services.IFlatSite>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Sites found');
                    } else {
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                    this.common.hideSplash();

                    console.log('sites', data);
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting sites. Please contact admin.");
                });
        }

        getFacilities(): ng.IPromise<void> {
            return this.facilityService.getAllFlat()
                .then((data: services.IFacility[]) => {
                    this.facilities = data;
                });
        }

        addLocal(site: services.ISite) {
            for (var i = 0; i < this.localSites.length; i++) {
                if (this.localSites[i].siteId === site.siteId) {
                    this.localSites.splice(i, 1);
                    break;
                }
            }

            this.localSites.push(site);
        }

        addSite(): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/newSite.html",
                controller: "app.modal.templates.NewSiteController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            parentSite: {},
                            isNewSite: true,
                            facilities: this.facilities,
                            siteName: ""
                        };
                    }
                }
            });

            instance.result.then((site: services.ISite) => {
                this.getAll();
            });

        }

        editSite(rowData): void {
            var site = _.find(this.localSites, (s: services.ISite) => {
                return s.siteId == rowData.siteId;
            });

            if (site == null) {
                this.common.showSplash();
                this.siteService.getByName(rowData.siteName)
                    .then((data: services.ISite) => {

                        site = data;
                        this.common.hideSplash();
                        var instance = this.$uibModal.open({
                            animation: true,
                            templateUrl: "/app/modal.templates/newSite.html",
                            controller: "app.modal.templates.NewSiteController",
                            controllerAs: "vm",
                            size: 'lg',
                            backdrop: false,
                            windowClass: 'app-modal-window',
                            resolve: {
                                values: () => {
                                    return {
                                        parentSite: site,
                                        isNewSite: false,
                                        facilities: this.facilities,
                                        siteName: rowData.siteName
                                    };
                                }
                            }
                        });

                        instance.result.then((site: services.ISite) => {
                            for (var i = 0; i < this.results.length; i++) {
                                var s = this.results[i];

                                if (s.siteId === site.siteId) {
                                    s.siteName = site.siteName;
                                    s.facilityId = site.facilities[0].facilityId;
                                    s.facilityName = site.facilities[0].facilityName;
                                    break;
                                }
                            }

                            this.addLocal(site);
                        }, (site: services.ISite) => {
                            this.addLocal(site);
                        });
                    });
            }
            else {

                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/newSite.html",
                    controller: "app.modal.templates.NewSiteController",
                    controllerAs: "vm",
                    size: 'lg',
                    backdrop: false,
                    windowClass: 'app-modal-window',
                    resolve: {
                        values: () => {
                            return {
                                parentSite: site,
                                isNewSite: false,
                                facilities: this.facilities,
                                siteName: rowData.siteName
                            };
                        }
                    }
                });

                instance.result.then((site: services.ISite) => {
                    for (var i = 0; i < this.results.length; i++) {
                        var s = this.results[i];

                        if (s.siteId === site.siteId) {
                            s.siteName = site.siteName;
                            s.facilityId = site.facilities[0].facilityId;
                            s.facilityName = site.facilities[0].facilityName;
                            break;
                        }
                    }

                    this.addLocal(site);
                }, (site: services.ISite) => {
                    this.addLocal(site);
                });
            }

            
        }

        onTotals(site: services.IFlatSite): void {
            var s = _.find(this.localSites, (s: services.ISite) => {
                return s.siteId === site.siteId;
            });

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/siteTotals.html",
                controller: "app.modal.templates.SiteTotalsController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            site: s,
                            siteName: site.siteName
                        };
                    }
                }
            });

            instance.result.then((site: services.ISite) => {
                //this.addLocal(site);
            });
        }

        editOrganization(dataItem) {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/editOrganization.html",
                controller: "app.modal.templates.EditOrganizationController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: 'static',
                windowClass: 'app-modal-window',
                resolve: {
                    organizationid: () => {
                        return dataItem.organizationId;
                    },
                    organization: () => {
                        return null;
                    }, 
                    facilities: () => {
                        return null;
                    },
                    users: () => {
                        return null;
                    }
                }
            });
        }

        editFacility(dataItem) {
            var fac = _.find(this.facilities, (s: services.IFacility) => {
                return s.facilityId == dataItem.facilityId;
            });

            this.facilityService.getFacilityAccredidations()
                .then((data: Array<services.IMasterServiceTypeItem>) => {
                    if (data != null) {

                        var instance = this.$uibModal.open({
                            animation: true,
                            templateUrl: "/app/modal.templates/facilityDemography.html",
                            controller: "app.modal.templates.FacilityDemographyController",
                            controllerAs: "vm",
                            size: 'lg',
                            backdrop: false,
                            windowClass: 'app-modal-window',
                            resolve: {
                                parentFacility: function () {
                                    return fac;
                                },
                                isReadonly: function () {
                                    return false;
                                },
                                facilityAccredidations: function () {
                                    return data;
                                }
                            }
                        });

                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.ManageSitesController',
        ManageSitesController);
}