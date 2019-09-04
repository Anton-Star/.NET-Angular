module app.modal.templates {
    'use strict';

    interface INewOrganization {        
        
        organization: services.IOrganization;  
        //countryList: Array<ICountry>;      
        //stateList: Array<IState>;      
        organizationTypes: Array<services.IOrganizationTypeItem>;      
        selectedOrganizationType: string;
        selectedCountry: string;
        selectedSate: string;
        save: () => void;
        cancel: () => void;
    }

    class NewOrganizationController implements INewOrganization {
        organization: services.IOrganization;
        organizationTypes: Array<services.IOrganizationTypeItem>;      
        selectedCountry: string;
        selectedSate: string;
        selectedOrganizationType: string;
         
        static $inject = [
            'organizationService',
            'notificationFactory',
            'common',
            '$uibModalInstance'
        ];

        constructor(
            private organizationService: services.IOrganizationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {
            common.activateController([this.getOrganizationTypes()], 'newOrganizationController');          
        }

        getOrganizationTypes(): ng.IPromise<void> {
            return this.organizationService.getOrganizationTypes()
                .then((data: Array<app.services.IOrganizationTypeItem>) => {
                    if (data == null) {
                        this.notificationFactory.error("no application type records");
                    } else {
                        
                        this.organizationTypes = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        save(): void { 
            if ((this.organization.cycleNumber != null &&
                this.organization.cycleNumber != undefined &&
                this.organization.cycleNumber !== "") && (this.organization.cycleEffectiveDate == null || this.organization.cycleEffectiveDate == undefined || this.organization.cycleEffectiveDate === "")) {
                this.notificationFactory.error("You must enter a Cycle Effective Date when the Cycle Number is entered.");
                return;
            }

            this.common.showSplash();
            this.organizationService.save(this.organization)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Organization created successfully.");
                        this.$uibModalInstance.close();
                    }

                    this.common.hideSplash();
                });       
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.NewOrganizationController',
        NewOrganizationController);
} 