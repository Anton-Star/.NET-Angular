module app.admin {
    'use strict';

    interface IFacilityDemographyScope {
        getAll(): void;
        add(): void;
        delete: (dataItem: services.IFacility) => void;
        edit: (dataItem: services.IFacility) => void;
        facilityAccredidations: Array<services.IFacilityAccreditationItem>;
    }

    class FacilityDemographyController implements IFacilityDemographyScope {
        results: Array<services.IFacility>;
        facilityAccredidations: Array<services.IFacilityAccreditationItem>;
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
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$window',
            '$uibModal',
            'cacheService',
            'facilityService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private facilityService: app.services.IFacilityService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {            
            common.activateController([this.getAll(), this.getFacilityAccredidations()], 'facilityDemographyController');
        }

        getAll(): ng.IPromise<void> {
            return this.cacheService.getFacilities()
                .then((data: Array<app.services.IFacility>) => {
                    if (data == null) {
                        this.notificationFactory.error('no items');
                    } else {
                        _.each(data, (fac) => {
                            fac.facilityNumber = fac.facilityNumber || "F00000".substr(0, 6 - fac.facilityId.toString().length) + fac.facilityId;
                        });
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getFacilityAccredidations(): ng.IPromise<void> {
            return this.facilityService.getFacilityAccredidations()
                .then((data: Array<services.IMasterServiceTypeItem>) => {                                        
                    if (data != null) {
                        this.facilityAccredidations = data;                        
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }


        add(): void {
            var facAccreditations = this.facilityAccredidations;
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/facilityDemography.html",
                controller: "app.modal.templates.FacilityDemographyController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                windowClass: 'app-modal-window',
                resolve: {    
                    parentFacility: function () {
                        return null;
                    },               
                    isReadonly: function () {
                        return false;
                    },
                    facilityAccredidations: function () {
                        return facAccreditations;
                    }
                }
            });

            instance.result.then((newFacility: services.IFacility) => {                                
                this.results.push(newFacility);
                this.gridOptions.dataSource.data(this.results);                
            }, () => {
            });
        }

        isClinical(dataItem: services.IFacility): boolean {
            return dataItem.serviceType.name.indexOf("Clinical") > -1;
        }

        onCibmtr(dataItem: services.IFacility) {
            this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/cibmtr.html",
                controller: "app.modal.templates.CibmtrController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            facilityName: dataItem.facilityName
                        };
                    }
                }
            });
        }

        delete(dataItem: services.IFacility): void {
            
            var confirmation = confirm("Are you sure you want to delete this facility ?");
            if (confirmation) {
                this.common.showSplash();
                this.facilityService.delete(dataItem.facilityId)
                    .then((data: app.services.IGenericServiceResponse<services.IFacility>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (data.item != null) {
                                this.results = this.results.filter(function (el) {
                                    return el.facilityId !== data.item.facilityId;
                                });
                                this.gridOptions.dataSource.data(this.results);
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

        edit(dataItem: services.IFacility): void {
            var facAccreditations = this.facilityAccredidations;
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
                        return dataItem;
                    },
                    isReadonly: function () {
                        return false;
                    },
                    facilityAccredidations: function () {
                        return facAccreditations;
                    }
                }
            });

            instance.result.then((newFacility: services.IFacility) => {
                this.getAll();
            }, () => {
            });
        }
        
    }

    angular
        .module('app.admin')
        .controller('app.admin.FacilityDemographyController',
        FacilityDemographyController);
} 
