module app.admin {
    'use strict';

    interface IFacilitySiteScope {
        saveRelation(): void;
        deleteRelation: (facilitySiteId) => void;
        add: (dataItem) => void;
        selectedSite: number;
        saveMode: boolean;
        selectedFacility: number;
    }

    class FacilitySiteController implements IFacilitySiteScope {

        sites: Array<services.ISite>;
        facilities: Array<services.IFacility>;
        facilitiesSites: Array<services.IFacilitySite>;
        facilitySiteId: number;
        selectedSite: number;
        selectedFacility: number;
        results: Array<services.IFacilitySite>;
        saveMode: boolean;
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
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$window',
            '$uibModal',
            'facilitySiteService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private facilitySiteService: app.services.IFacilitySiteService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.saveMode = false;
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Facility Site Relationship",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Facility Site Relationship', isActive: true }
                ]
            });

            common.activateController([this.getFacilitySite()], 'facilitySiteController');
        }

        getFacilitySite(): ng.IPromise<void> {
            return this.facilitySiteService.getFacilitySite()
                .then((data: app.services.IGenericServiceResponse<app.services.IFacilitySitePage>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    }
                    else {
                        if (data.item == null) {
                            this.notificationFactory.error('no items');
                        } else {
                            this.sites = data.item.siteItems;
                            this.facilities = data.item.facilityItems;
                            this.facilitiesSites = data.item.facilitySiteItems;
                            this.results = data.item.facilitySiteItems;
                            this.gridOptions.dataSource.data(data.item.facilitySiteItems);
                        }
                    }

                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        saveRelation(): void {
            this.common.showSplash();

            this.facilitySiteService.saveRelation(this.facilitySiteId, this.selectedSite, this.selectedFacility)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item == true) {
                            this.getFacilitySite();
                            this.clearForm();
                            this.saveMode = false;
                            if (data.message != '' && data.message != null)
                                this.notificationFactory.success(data.message);

                        } else {
                            this.notificationFactory.error("Error.");
                        }
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }

        deleteRelation(facilitySiteId): void {
            var confirmation = confirm("Are you sure you want to delete this relation ?");
            if (confirmation) {
                this.common.showSplash();
                this.facilitySiteService.deleteRelation(facilitySiteId)
                    .then((data: app.services.IGenericServiceResponse<boolean>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {

                            if (data.item == true) {
                                this.getFacilitySite();
                                this.notificationFactory.success(data.message);
                            } else {
                                this.notificationFactory.error("Error.");
                            }
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                    });
            }
        }

        cancel(): void {
            this.clearForm();
            this.saveMode = false;
        }

        clearForm(): void {
            this.selectedSite = 0;
            this.selectedFacility = 0;
            this.facilitySiteId = 0;
        }

        add(dataItem): void {
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
                            parentSite: null,
                            isNewSite: true,
                            facilities: this.facilities,
                            siteName: ""
                        }
                    }
                }
            });

            instance.result.then(() => {
                this.getFacilitySite();
            });

        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.FacilitySiteController',
        FacilitySiteController);
} 