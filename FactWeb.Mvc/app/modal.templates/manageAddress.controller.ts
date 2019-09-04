module app.modal.templates {
    'use strict';

    interface IManageAddress {
        siteAddress: services.ISiteAddressItem;
        address: services.IAddressItem;
        addressTypesList: Array<services.IAddressType>;
        statesList: Array<services.IState>;
        countriesList: Array<services.ICountry>;
        isCountryUSA: boolean;
        save: () => void;
        cancel: () => void;
    }

    class ManageAddressController implements IManageAddress {
        siteAddress: services.ISiteAddressItem;
        address: services.IAddressItem;
        addressTypesList: Array<services.IAddressType>;
        statesList: Array<services.IState>;
        countriesList: Array<services.ICountry>;
        isCountryUSA = false;

        static $inject = [
            '$uibModal',
            'siteService',
            'notificationFactory',
            'common',
            'oldAddress',
            'isNewAddress',
            '$uibModalInstance'
        ];

        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private siteService: services.ISiteService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private oldAddress: services.ISiteAddressItem,
            private isNewAddress: boolean,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            if (oldAddress != null) {
                this.address = oldAddress.address;

                if (this.address.country.id == 1) {
                    this.isCountryUSA = true;
                }
                else {
                    this.isCountryUSA = false;
                }
            }

            common.activateController([this.getStatesList(), this.getCountriesList(), this.getAddressTypesList()], 'manageAddressController');
        }

        getAddressTypesList(): ng.IPromise<void> {
            return this.siteService.getAddressTypesList()
                .then((data: Array<app.services.IAddressType>) => {
                    if (data == null) {
                        this.notificationFactory.error("No Address Types found");
                    } else {
                        this.addressTypesList = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting address types. Please contact support.");
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

        selectedCountryChange() {
            var selCountry = this.address.country.id;

            if (selCountry == 1) {
                this.isCountryUSA = true;
            }
            else {
                this.isCountryUSA = false;
            }

            if (this.address.state != undefined) {
                this.address.state.id = 0;
            }
            if (this.address.province != undefined) {
                this.address.province = "";
            }
        }

        save(): void {
           
            var selAddressTypeId = this.address.addressType.id;
            var selAddressType = this.addressTypesList.filter(function (addressType) {
                return addressType.id == selAddressTypeId;
            })[0];

            this.address.addressType.name = selAddressType.name;

            if (this.siteAddress == null) {
                this.siteAddress = { id: null, siteId: null, addressId: null, addressType: this.address.addressType.name, address: this.address};
            }

            this.siteAddress.address = this.address;
            this.$uibModalInstance.close(this.siteAddress);
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.ManageAddressController',
        ManageAddressController);
} 