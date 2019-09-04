module app.modal.templates {
    'use strict';

    interface IValues {
        parentSite: services.ISite;
        siteName: string;
        isNewSite: boolean;
        facilities: services.IFacility[];
    }

    interface INewSite {
        site: services.ISite;
        statesList: Array<services.IState>;
        countriesList: Array<services.ICountry>;
        clinicalTypes: Array<services.IClinicalType>;
        processingTypes: Array<services.IProcessingType>;
        collectionProductTypes: Array<services.ICollectionProductType>;
        cbCollectionTypes: Array<services.ICBCollectionType>;
        cbUnitTypes: Array<services.ICBUnitType>,
        transplantTypes: Array<services.ITransplantType>;
        scopeTypes: Array<services.IScopeType>;
        siteAddressItems: Array<services.ISiteAddressItem>;
        isCountryUSA: boolean;
        isFacilityCT: boolean;
        isFacilityCB: boolean;
        isDateValid: boolean;
        siteUnitsBankedDate: string;
        save: () => void;
        cancel: () => void;
    }

    class NewSiteController implements INewSite {
        site: services.ISite;
        statesList: Array<services.IState>;
        countriesList: Array<services.ICountry>;
        clinicalTypes: Array<services.IClinicalType>;
        processingTypes: Array<services.IProcessingType>;
        collectionProductTypes: Array<services.ICollectionProductType>;
        cbCollectionTypes: Array<services.ICBCollectionType>;
        cbUnitTypes: Array<services.ICBUnitType>;
        transplantTypes: Array<services.ITransplantType>;
        scopeTypes: Array<services.IScopeType>;
        siteAddressItems: Array<services.ISiteAddressItem> = [];
        isCountryUSA = true;
        isFacilityCT = false;
        isFacilityCB = false;
        isDateValid = true;
        siteUnitsBankedDate = "";
        parentSite: services.ISite;
        isNewSite: boolean;
        facilities: services.IFacility[];
        countryId: number = 1;

        results: Array<services.ISiteAddressItem>;
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
        comboOptions = {
            open: this.common.onOpenCombo
        };

        static $inject = [
            '$timeout',
            '$uibModalInstance',
            '$uibModal',
            '$q',
            'siteService',
            'cacheService',
            'scopeTypeService',
            'notificationFactory',
            'common',
            'modalHelper',
            'values'
        ];

        constructor(
            private $timeout: ng.ITimeoutService,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $q: ng.IQService,
            private siteService: services.ISiteService,
            private cacheService: services.ICacheService,
            private scopeTypeService: services.IScopeTypeService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private modalHelper: common.IModalHelper,
            private values: IValues) {

            $timeout(() => {
                    this.common.onModalOpen();
                },
                300);
            

            this.parentSite = values.parentSite;
            this.facilities = values.facilities;
            this.isNewSite = values.isNewSite;
            
            if (this.parentSite != null) {
                this.site = this.parentSite;

                if (this.parentSite.siteAddresses) {
                    this.results = this.parentSite.siteAddresses;
                    this.gridOptions.dataSource.data(this.parentSite.siteAddresses);
                    this.siteAddressItems = this.parentSite.siteAddresses;
                }

                if (this.site.siteCountry) {
                    this.countryId = this.site.siteCountry.id;

                    if (this.site.siteCountry.id != 1) {
                        this.isCountryUSA = false;
                    }
                }
            }
            
            if (this.facilities) {
                this.checkSite();
            } else {
                this.getFacilities();
            }

            if (!this.parentSite && values.siteName && !this.isNewSite) {
                this.getSite();
            } else {
                this.common.showSplash();
                $q.all([this.getScopeTypes(), this.getStatesList(), this.getCountriesList(), this.getClinicalTypes(), this.getProcessingTypes(), this.getCollectionProductTypes(), this.getCBCollectionTypes(), this.getCBUnitTypes(), this.getTransplantTypes()])
                    .then(() => {
                        //this.common.goToTop();
                        this.common.hideSplash();
                    });
                //common.activateController(, 'newSiteController');
            }
        }

        getSite() {
            this.common.showSplash();
            this.siteService.getByName(this.values.siteName)
                .then((data: services.ISite) => {
                   
                    this.parentSite = data;
                    this.site = data;
                    this.siteAddressItems = data.siteAddresses;
                    this.checkSite();
                    this.$q.all([this.getScopeTypes(), this.getStatesList(), this.getCountriesList(), this.getClinicalTypes(), this.getProcessingTypes(), this.getCollectionProductTypes(), this.getCBCollectionTypes(), this.getCBUnitTypes(), this.getTransplantTypes()])
                        .then(() => {
                            //this.common.goToTop();
                            this.common.hideSplash();
                        });
                });
        }

        getFacilities() {
            this.cacheService.getFacilities()
                .then((data: Array<services.IFacility>) => {
                    this.facilities = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting facilities. Please contact support.");
                });
        }

        checkSite() {
            if (this.site != undefined) {

                if (this.site.siteCountry != undefined) {
                    this.countryId = this.site.siteCountry.id;

                    if (this.site.siteCountry.id != 1) {
                        this.isCountryUSA = false;
                    }
                }

                if (this.site.siteFacilityId != undefined) {
                    var selFac = this.site.siteFacilityId;

                    var selFacility = this.facilities.filter(function (fac) {
                        return fac.facilityId == selFac;
                    })[0];


                    if (selFacility.masterServiceTypeId == 1) {
                        this.isFacilityCT = true;
                        this.isFacilityCB = false;
                    }
                    else if (selFacility.masterServiceTypeId == 2) {
                        this.isFacilityCT = false;
                        this.isFacilityCB = true;
                    }
                    else {
                        this.isFacilityCT = false;
                        this.isFacilityCB = false;
                    }
                }
            }
        }

        getScopeTypes(): ng.IPromise<void> {
            return this.scopeTypeService.get()
                .then((data: Array<services.IScopeType>) => {
                    this.scopeTypes = data;

                    if (!this.isNewSite) {
                        _.forEach(this.scopeTypes, (scopeType: services.IScopeType) => {
                            var found = _.find(this.site.scopeTypes, (type: services.IScopeType) => {
                                return scopeType.scopeTypeId === type.scopeTypeId;
                            });

                            if (found) {
                                scopeType.isSelected = true;
                            } else {
                                scopeType.isSelected = false;
                            }
                        });
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting scope types. Please contact support");
                });
        }

        getStatesList(): ng.IPromise<void> {
            return this.siteService.getStatesList()
                .then((data: Array<app.services.IState>) => {
                    if (data == null) {
                        this.notificationFactory.error("No States found");
                    } else {
                        this.statesList = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting states. Please contact support.");
                });
        }

        getCountriesList(): ng.IPromise<void> {
            return this.siteService.getCountriesList()
                .then((data: Array<app.services.ICountry>) => {
                    if (data == null) {
                        this.notificationFactory.error("No Countries found");
                    } else {
                        this.countriesList = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting countries. Please contact support.");
                });
        }

        getClinicalTypes(): ng.IPromise<void> {
            return this.siteService.getClinicalTypes()
                .then((data: Array<app.services.IClinicalType>) => {
                    if (data == null) {
                        this.notificationFactory.error("No Clinical Type records");
                    } else {
                        this.clinicalTypes = data;

                        if (!this.isNewSite) {
                            _.forEach(this.clinicalTypes, (clinicalType: services.IClinicalType) => {
                                var found = _.find(this.site.clinicalTypes, (type: services.IClinicalType) => {
                                    return clinicalType.id === type.id;
                                });

                                if (found) {
                                    clinicalType.isSelected = true;
                                } else {
                                    clinicalType.isSelected = false;
                                }
                            });
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting clinical types. Please contact support.");
                });
        }

        getProcessingTypes(): ng.IPromise<void> {
            return this.siteService.getProcessingTypes()
                .then((data: Array<app.services.IProcessingType>) => {
                    if (data == null) {
                        this.notificationFactory.error("No Processing Type records");
                    } else {
                        this.processingTypes = data;

                        if (!this.isNewSite) {
                            _.forEach(this.processingTypes, (processingType: services.IProcessingType) => {
                                var found = _.find(this.site.processingTypes, (type: services.IProcessingType) => {
                                    return processingType.id === type.id;
                                });

                                if (found) {
                                    processingType.isSelected = true;
                                } else {
                                    processingType.isSelected = false;
                                }
                            });
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting processing types. Please contact support.");
                });
        }

        getCollectionProductTypes(): ng.IPromise<void> {
            return this.siteService.getCollectionProductTypes()
                .then((data: Array<app.services.ICollectionProductType>) => {
                    if (data == null) {
                        this.notificationFactory.error("No Collection Product Type records");
                    } else {
                        this.collectionProductTypes = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting collection product types. Please contact support.");
                });
        }

        getCBCollectionTypes(): ng.IPromise<void> {
            return this.siteService.getCBCollectionTypes()
                .then((data: Array<app.services.ICBCollectionType>) => {
                    if (data == null) {
                        this.notificationFactory.error("No CB Collection Type records");
                    } else {
                        this.cbCollectionTypes = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting CB collection types. Please contact support.");
                });
        }

        getCBUnitTypes(): ng.IPromise<void> {
            return this.siteService.getCBUnitTypes()
                .then((data: Array<app.services.ICBUnitType>) => {
                    if (data == null) {
                        this.notificationFactory.error("No CB Unit Type records");
                    } else {
                        this.cbUnitTypes = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting CB unit types. Please contact support.");
                });
        }

        getTransplantTypes(): ng.IPromise<void> {
            return this.siteService.getTransplantTypes()
                .then((data: Array<app.services.ITransplantType>) => {
                    if (data == null) {
                        this.notificationFactory.error("No Transplant Type records");
                    } else {
                        this.transplantTypes = data;

                        if (!this.isNewSite) {
                            _.forEach(this.transplantTypes, (transplantType: services.ITransplantType) => {
                                var found = _.find(this.site.transplantTypes, (type: services.ITransplantType) => {
                                    return transplantType.id === type.id;
                                });

                                if (found) {
                                    transplantType.isSelected = true;
                                } else {
                                    transplantType.isSelected = false;
                                }
                            });
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting transplant types. Please contact support.");
                });
        }

        selectedCountryChange() {
            var selCountry = this.countryId;

            if (selCountry == 1) {
                this.isCountryUSA = true;
            }
            else {
                this.isCountryUSA = false;
            }

            if (this.site.siteState != undefined) {
                this.site.siteState.id = 0;
            }
            if (this.site.siteProvince != undefined) {
                this.site.siteProvince = "";
            }
        }

        selectedFacilityChange() {
            var selFac = this.site.siteFacilityId;

            var selFacility = this.facilities.filter(function (fac) {
                return fac.facilityId == selFac;
            })[0];

            if (selFacility.masterServiceTypeId == 1) {
                this.isFacilityCT = true;
                this.isFacilityCB = false;
            }
            else if (selFacility.masterServiceTypeId == 2) {
                this.isFacilityCT = false;
                this.isFacilityCB = true;
            }
            else {
                this.isFacilityCT = false;
                this.isFacilityCB = false;
            }

            this.site.siteCBCollectionType = null; //{ 'id': 0, name: '' };
            this.site.siteClinicalPopulationType = null; //{ 'id': 0, name: '' };
            this.site.siteCollectionProductType = null; //{ 'id': 0, name: '' };
            this.site.siteCBUnitType = null; //{ 'id': 0, name: '' };
            //this.site.siteUnitsBanked = 0;
            //this.site.siteUnitsBankedDate = "";
            //this.siteUnitsBankedDate = "";
        }

        isDate = function (x) {
            return x instanceof Date;
        };

        validateDates() {
            this.isDateValid = false;

            if (this.site.siteStartDate != null && this.isDate(this.site.siteStartDate)
                && (this.siteUnitsBankedDate == ""
                    || (this.site.siteUnitsBankedDate != "" && this.isDate(this.site.siteUnitsBankedDate)))) {
                this.isDateValid = true;
            }            
        }

        addAddress(): void {
            
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/manageAddress.html",
                controller: "app.modal.templates.ManageAddressController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    oldAddress: () => {
                        return null;
                    },
                    isNewAddress: () => {
                        return true;
                    }
                }
            });

            instance.result.then((result: services.ISiteAddressItem) => {
                
                if (this.siteAddressItems.length == 0) {
                    result.address.localId = 1;
                    this.siteAddressItems.push(result);
                }
                else {
                    result.address.localId = this.siteAddressItems[this.siteAddressItems.length - 1].address.localId + 1;
                    this.siteAddressItems.push(result);
                }
                this.results = this.siteAddressItems;
                this.gridOptions.dataSource.data(this.siteAddressItems);
            }, () => {
            });
        }

        editAddress(rowData): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/manageAddress.html",
                controller: "app.modal.templates.ManageAddressController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    oldAddress: () => {
                        return rowData;
                    },
                    isNewAddress: () => {
                        return false;
                    }
                }
            });

            instance.result.then((result: services.ISiteAddressItem) => {
                var selAddressId = result.address.localId;
                var selAddress = this.siteAddressItems.filter(function (address) {
                    return address.address.localId == selAddressId;
                })[0];

                var index = this.siteAddressItems.indexOf(selAddress);

                this.siteAddressItems.splice(index, 1);
                this.siteAddressItems.push(result);
                this.results = this.siteAddressItems;
                this.gridOptions.dataSource.data(this.siteAddressItems);
            }, () => {
            });
        }

        deleteAddress(rowData): void {
           
            var selAddressId = rowData.localId;
            var selAddress = this.siteAddressItems.filter(function (address) {
                return address.address.localId == selAddressId;
            })[0];

            var index = this.siteAddressItems.indexOf(selAddress);

            this.siteAddressItems.splice(index, 1);
            this.results = this.siteAddressItems;
            this.gridOptions.dataSource.data(this.siteAddressItems);
        }

        save(): void {
            this.common.showSplash();

            if (!this.site.siteCountry) {
                var country: services.ICountry = {
                    id: this.countryId,
                    name: ""
                };

                this.site.siteCountry = country;
            }
            else {
                this.site.siteCountry.id = this.countryId;
            }
            
            this.scopeTypes = this.scopeTypes.filter(function (scopeType) {
                return scopeType.isSelected === true;
            });

            this.site.scopeTypes = this.scopeTypes;

            this.transplantTypes = this.transplantTypes.filter(function (transplantType) {
                return transplantType.isSelected === true;
            });

            this.site.transplantTypes = this.transplantTypes;

            this.clinicalTypes = this.clinicalTypes.filter(function (clinicalType) {
                return clinicalType.isSelected === true;
            });

            this.site.clinicalTypes = this.clinicalTypes;

            this.processingTypes = this.processingTypes.filter(function (processingType) {
                return processingType.isSelected === true;
            });
            
            this.site.processingTypes = this.processingTypes;
            this.site.siteAddresses = this.siteAddressItems;

            this.siteService.save(this.site)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (this.isNewSite) {
                            this.notificationFactory.success("Site added successfully.");
                        }
                        else {
                            this.notificationFactory.success("Site updated successfully.");
                        }
                    }

                    this.$uibModalInstance.close(this.site);
                    this.common.onModalClose();

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.common.hideSplash();
                });
        }

        cancel(): void {
            if (this.isNewSite) {
                this.$uibModalInstance.dismiss('cancel');
            } else {
                this.$uibModalInstance.close(this.site);
            }

            this.common.onModalClose();
        }
        
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.NewSiteController',
        NewSiteController);
} 