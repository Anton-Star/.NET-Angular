module app.modal.templates {
    'use strict';

    interface IFacilityDemography {
        organizations: Array<services.IOrganization>;
        masterServiceTypes: Array<services.IMasterServiceTypeItem>;
        users: Array<services.IUser>;        
        serviceTypes: Array<services.IServiceTypeItem>;
        facility: services.IFacility;
        save: () => void;
        cancel: () => void;        
        isEditMode: boolean;
        readOnly: boolean;
    }

    class FacilityDemographyController implements IFacilityDemography {
        organizations: Array<services.IOrganization>;
        masterServiceTypes: Array<services.IMasterServiceTypeItem>;
        users: Array<services.IUser>;       
        serviceTypes: Array<services.IServiceTypeItem>;
        serviceTypesFiltered: Array<services.IServiceTypeItem>;
        facility: services.IFacility;
        resultsCT: Array<services.ISite>;
        resultsCB: Array<services.ISite>;
        resultsSiteTotalsCT: Array<services.ISiteTotalItem>;
        resultsSiteTotalsCB: Array<services.ISiteTotalItem>;
        isEditMode = false;
        readOnly = false;
        facilities: services.IFacility[];
        netcordMembershipTypes: services.INetcordMembershipType[];
        options = {
            format: "#",
            decimals: 0
        };
        
        gridOptionsCT = {
            sortable: true,
            filterable: true,
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "siteId", title: "Id", hidden: true },
                {
                    field: "siteName", title: "Site Name", width: "300px", hidden: false,
                    template: "<a href=\"\" ng-click=\"vm.editSite(dataItem)\">#=siteName#</a>"
                },
                { field: "siteCollectionProductType.name", title: "Collection Product Type", hidden: false},
                { field: "siteCBCollectionType.name", title: "CB Collection Type", hidden: true},
                { field: "siteClinicalPopulationType.name", title: "Transplant Population Type", hidden: false },
                { field: "siteCBUnitType.name", title: "CB Unit Type ", hidden: true }
            ],
            pageable: {
                pageSize: 10
            }
        };
        gridOptionsCB = {
            sortable: true,
            filterable: true,
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "siteId", title: "Id", hidden: true },
                {
                    field: "siteName", title: "Site Name", width: "300px", hidden: false,
                    template: "<a href=\"\" ng-click=\"vm.editSite(dataItem)\">#=siteName#</a>"
                },
                { field: "siteCollectionProductType.name", title: "Collection Product Type", hidden: true },
                { field: "siteCBCollectionType.name", title: "CB Collection Type", hidden: false },
                { field: "siteClinicalPopulationType.name", title: "Transplant Population Type", hidden: true },
                { field: "siteCBUnitType.name", title: "CB Unit Type ", hidden: false }
            ],
            pageable: {
                pageSize: 10
            }
        };
        gridOptionsSiteTotalsCT = {
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
        gridOptionsSiteTotalsCB = {
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

        showOther = false;

        static $inject = [
            '$location',
            '$window',
            '$uibModal',
            'facilityService',
            'siteService',
            'cacheService',
            'organizationService',
            'notificationFactory',
            'common',
            'modalHelper',
            '$uibModalInstance',
            'userService',
            'parentFacility',
            'isReadonly',
            'facilityAccredidations'
        ];

        constructor(
            private $location: ng.ILocationService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private facilityService: services.IFacilityService,
            private siteService: services.ISiteService,
            private cacheService: services.ICacheService,
            private organizationService: services.IOrganizationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private modalHelper: common.IModalHelper,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private userService: services.IUserService,
            private parentFacility: services.IFacility,
            private isReadonly: boolean,            
            private facilityAccredidations: Array<services.IFacilityAccreditationItem>) {

            //initialize state variables.
            _.each(this.facilityAccredidations, (val) => {
                val.isSelected = false;
            });

            common.activateController([this.getNetcordMembershipTypes(),this.getFacilities(), this.getOrganizationByFacility(), this.getMasterServiceTypes(), this.getAllUsers(), this.getServiceTypes()], 'facilityDemographyController');

            if (parentFacility != null) {
                this.getFacilityById();
            }
        }

        getFacilityById(): ng.IPromise<void> {
            return this.facilityService.getByIdWithChild(this.parentFacility.facilityId)
                .then((data: services.IFacility) => {
                    
                    this.facility = data;
                    this.facility.hrsa = this.parentFacility.hrsa;
                    this.facility.facilityNumber = this.facility.facilityNumber || "F00000".substr(0, 6 - this.facility.facilityId.toString().length) + this.facility.facilityId;
                    console.log(this.facility, this.parentFacility);
                    _.each(this.facility.sites, (site) => {
                        site.siteCollectionProductType = site.siteCollectionProductType || { id: 0, name: "" };
                        site.siteCBCollectionType = site.siteCBCollectionType || { id: 0, name: "" };
                        site.siteClinicalPopulationType = site.siteClinicalPopulationType || { id: 0, name: "" };
                        site.siteCBUnitType = site.siteCBUnitType || { id: 0, name: "" };
                    });

                    this.resultsCT = this.facility.sites;
                    this.resultsCB = this.facility.sites;
                    this.gridOptionsCT.dataSource.data(this.facility.sites);
                    this.gridOptionsCB.dataSource.data(this.facility.sites);
                    this.resultsSiteTotalsCT = this.facility.siteTotals;
                    this.resultsSiteTotalsCB = this.facility.siteTotals;                    
                    this.gridOptionsSiteTotalsCT.dataSource.data(this.facility.siteTotals);
                    this.gridOptionsSiteTotalsCB.dataSource.data(this.facility.siteTotals);
                    this.getCBCollectionSiteTypes();
                    this.isEditMode = true;
                    this.readOnly = this.isReadonly;

                    _.forEach(this.facility.facilityAccreditation, (facilityAccredidation: services.IFacilityAccreditationItem) => {
                        var found = _.find(this.facilityAccredidations, (type: services.IFacilityAccreditationItem) => {
                            return facilityAccredidation.id === type.id;
                        });

                        if (found) {
                            found.isSelected = true;
                        }
                        if (facilityAccredidation.id == 6) {
                            this.showOther = true;
                        }
                    });
                });
        }
        
        getFacilities(): ng.IPromise<void> {
            return this.facilityService.getAllFlat()
                .then((data: services.IFacility[]) => {
                    this.facilities = data;
                });
        }

        getNetcordMembershipTypes(): ng.IPromise<void> {
            return this.cacheService.getNetcordMembershipTypes()
                .then((data: services.INetcordMembershipType[]) => {
                    this.netcordMembershipTypes = data;
                });
        }

        getOrganizationByFacility(): ng.IPromise<void> {
            if (this.parentFacility != null && this.parentFacility.facilityId != null) {
                return this.organizationService.getOrgByFacilityRelation(this.parentFacility.facilityId.toString(), true)
                    .then((data: Array<services.IOrganization>) => {
                        if (data != null) {
                            this.organizations = data;
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error getting organizations.");
                    });
            }
        }

        getMasterServiceTypes(): ng.IPromise<void> {
            return this.facilityService.getMasterServiceTypes()
                .then((data: Array<services.IMasterServiceTypeItem>) => {
                    if (data != null) {
                        this.masterServiceTypes = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting Master Service Types.");
                });
        }

        getCBCollectionSiteTypes() {
            return this.facilityService.getCBCollectionSiteTypes(this.facility.facilityId)
                .then((data: string) => {
                    if (data != null) {
                        this.facility.cbCollectionSiteType = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting CB Collection Site Types.");
                });
        }

        getAllUsers() {
            return this.userService.getAllUsers(false)
                .then((data: Array<app.services.IUser>) => {
                    if (data != null) {
                        this.users = this.common.sortUsers(data);
                    }
                    
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting users.");
                });
        }
        
        getServiceTypes(): ng.IPromise<void> {
            return this.facilityService.getServiceTypes()
                .then((data: Array<services.IServiceTypeItem>) => {
                    if (data != null) {
                        this.serviceTypes = data;
                        this.serviceTypesFiltered = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting Service Types.");
                });
        }

        addUser(): void {
            var values = {
                parentUser: () => {
                    return null;
                },
                isNewUser: () => {
                    return true;
                },
                currentOrganization: () => {
                    return null;
                },
                role: () => {
                    return "";
                },
                isPersonnel: () => {
                    return false;
                }
            };

            this.modalHelper.showModal("/app/modal.templates/editUser.html", "app.modal.templates.EditUserController", values)
                .then((data: services.IUser) => {
                    if (data == null) return;

                    this.facility.facilityDirectorId = data.userId;

                    this.users.push(data);
                    this.users = this.common.sortUsers(this.users);
                })
                .catch(() => {
                });

        }

        save(): void {
            this.common.showSplash();
            this.facility.facilityAccreditation = this.facilityAccredidations.filter(val => val.isSelected == true);
            this.facilityService.save(this.facility)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Facility saved successfully.");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.common.hideSplash();
                });
            this.$uibModalInstance.close();
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }

        masterServiceTypeChange(): void {
            var id = parseInt(this.facility.masterServiceTypeId.toString());
            this.serviceTypes = this.serviceTypesFiltered.filter(function (el) {
                return el.masterServiceTypeId === id;
            });
         
        }

        editSite(rowData): ng.IPromise<void> {
            return this.siteService.searchBySite(rowData.siteId)
                .then((data: Array<app.services.ISite>) => {
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
                                    parentSite: data[0],
                                    isNewSite: false,
                                    facilities: this.facilities,
                                    siteName: data[0].siteName
                                }
                            }
                        }
                    });

                    instance.result.then(() => {
                    }, () => {
                    });
                });
        }

        onFacilityChecked(faI: services.IFacilityAccreditationItem) {
            if (faI.id == 6)
                this.showOther = faI.isSelected;
            //if (faI.isSelected) {
                
            //    this.facility.facilityAccreditation.push(faI);
            //} else {
            //    this.facility.facilityAccreditation = this.facility.facilityAccreditation.filter(item => item.id != faI.id);
            //}
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.FacilityDemographyController',
        FacilityDemographyController);
} 