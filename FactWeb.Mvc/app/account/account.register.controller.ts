module app.account {
    'use strict';

    interface IOrgType {
        name: string;
        title: string;
    }

    interface IRegisterScope {
        firstName: string;
        lastName: string;
        email: string;
        password: string;
        passwordConfirm: string;
        orgTypes: Array<IOrgType>;
        save: () => void;
        //onOrgTypeSelected: (orgType: IOrgType) => void;
        selectedOrgType: IOrgType;
        newOrganization: string;
        selectedOrganization: services.IOrganization;
        findOrg: () => void;
        passwordsMatch(): boolean;
        isInvalidPassword(): boolean;
    }

    class RegisterController implements IRegisterScope {
        firstName: string;
        lastName: string;
        email: string;
        password = "";
        passwordConfirm = "";
        newOrganization: string;
        selectedOrganization: services.IOrganization;
        orgTypes = [
            { name: "existing", title: "Existing company/organization" },
            { name: "new", title: "Add a new company/organization" },
            { name: "none", title: "Not Affiliated" }];
        selectedOrgType: IOrgType;

        static $inject = [
            '$window',
            '$uibModal',
            'accountService',
            'notificationFactory',
            'currentUser',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private accountService: app.services.IAccountService,
            private notificationFactory: app.blocks.INotificationFactory,
            private currentUser: app.services.IUser,
            private common: app.common.ICommonFactory,
            private config: IConfig) {

            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Create A Personal Profile",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Create A Personal Profile', isActive: true }
                ]
            });
        }

        save(): void {
            this.common.showSplash();
            
        }

        findOrg(): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/organizationSearch.html",
                controller: "app.modal.templates.OrganizationSearchController",
                controllerAs: "vm",
                size: 'lg'
            });

            instance.result.then((selectedOrg: services.IOrganization) => {
                this.selectedOrganization = selectedOrg;
            });
        }

        passwordsMatch(): boolean {
            return this.password === this.passwordConfirm;
        }

        isInvalidPassword(): boolean {
            var hasError = false;

            if (!this.password ||
                this.password.length < 8 ||
                this.password.search(/[a-z]/i) < 0 ||
                this.password.search(/[A-Z]/i) < 0 ||
                (this.password.search(/[0-9]/) < 0 &&
                    this.password.search(/[!@#$%^&*()-=+`~{}\|;:><,/\? "]/) < 0)) {
                hasError = true;
            }

            return hasError || !this.passwordsMatch();
        }
    }

    angular
        .module('app.account')
        .controller('app.account.RegisterController',
        RegisterController);
} 