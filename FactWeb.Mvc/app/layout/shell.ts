module app.layout {
    'use strict';

    interface INavigationScope {
        fullName: string;
        showSplash: boolean;
        message: string;
        listenForEvents: () => void;
        spinnerOptions: Object;
        useSpinner: boolean;
        pageName: string;
        breadcrumbs: Array<IBreadcrumb>;
    }

    export interface IBreadcrumb {
        url: string;
        name: string;
        isActive: boolean;
    }

    export interface SignalR {
        cache: any;
    }

    class ShellController implements INavigationScope {
        fullName = "";
        orgName = "";
        showSplash: boolean;
        message = "";
        pageName = "";
        isHomePage = false;
        isInspector = false;
        breadcrumbs: Array<IBreadcrumb>;
        spinnerOptions: Object = {
            radius: 40,
            lines: 7,
            length: 0,
            width: 30,
            speed: 1.7,
            corners: 1.0,
            trail: 100,
            color: '#F58A00'
        };
        useSpinner = true;
        modalInstance: ng.ui.bootstrap.IModalServiceInstance;
        showMenu = false;
        isImpersonation = false;
        cacheStatuses: services.ICacheStatus[];
        cacheDate: string;

        disclaimerUrl = "";
        accessibilityUrl = "";
        privacyUrl = "";
        sitemapUrl = "";
        contactUsUrl = "";

        

        static $inject = [
            '$rootScope',
            '$location',
            '$uibModal',
            '$cookies',
            '$q',
            'localStorageService',
            'config',
            'common',
            'currentUser',
            'notificationFactory',
            'accountService',
            'cacheService',
            'cacheStatusService',
            'applicationSettingService'
        ];
        constructor(private $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $cookies: ng.cookies.ICookiesService,
            private $q: ng.IQService,
            private localStorageService: any,
            private config: IConfig,
            private common: app.common.ICommonFactory,
            private currentUser: app.services.IUser,
            private notificationFactory: blocks.INotificationFactory,
            private accountService: services.IAccountService,
            private cacheService: services.ICacheService,
            private cacheStatusService: services.ICacheStatusService,
            private applicationSettingService: services.IApplicationSettingService
        ) {

            this.listenForEvents();
           
        }

        onMenuClick() {
            this.showMenu = !this.showMenu;
        }

        getSettingValue(name: string): string {
            var found = _.find(this.common.applicationSettings, (s) => {
                return s.name === name;
            });

            if (found) {
                return found.value;
            } else {
                return "";
            }
        }

        listenForEvents(): void {
            this.$rootScope.$on(this.config.events.controllerActivateSuccess, () => {
                //this.showSplash = false;
                if (this.modalInstance) {
                    this.modalInstance.dismiss('cancel');
                }
            });

            this.$rootScope.$on(this.config.events.applicationSettingsLoaded, () => {
                this.disclaimerUrl = this.getSettingValue("Disclaimer Url");
                this.accessibilityUrl = this.getSettingValue("Accessibility Url");
                this.privacyUrl = this.getSettingValue("Privacy Url");
                this.sitemapUrl = this.getSettingValue("Sitemap Url");
                this.contactUsUrl = this.getSettingValue("Contact Us Url");
            });



            this.$rootScope.$on(this.config.events.controllerActivating, () => {
                //this.showSplash = true;
                this.modalInstance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/loading.html",
                    size: 'xxs',
                    backdrop: 'static',
                    keyboard: false
                });
                this.message = "Loading...Please Wait...";
            });

            this.$rootScope.$on(this.config.events.pageNameSet, (data: any, args: any) => {
                this.pageName = args.pageName;
                this.breadcrumbs = args.breadcrumbs;
            });

            this.$rootScope.$on(this.config.events.loginPageInit, () => {
                this.isHomePage = true;
                this.isImpersonation = false;
            });

            this.$rootScope.$on(this.config.events.otherPageInit, () => {
                this.isHomePage = false;
            });

            this.$rootScope.$on(this.config.events.userImpersonated, (data: any, args: any) => {
                this.fullName = args.fullName;
                this.orgName = args.orgName;
                this.isInspector = args.roleName === this.config.roles.inspector;
                this.isImpersonation = true;
                this.localStorageService.set("impersonate", "Y");
            });

            this.$rootScope.$on(this.config.events.cacheInvalidated, (data: any, args: any) => {
                this.cacheService.clearCache(args.dbName);
            });

            this.$rootScope.$on(this.config.events.userLoggedIn, (data: any, args: any) => {
                this.fullName = args.fullName;
                this.orgName = args.orgName;
                this.isInspector = args.roleName === this.config.roles.inspector;
                var impersonate: string = this.localStorageService.get("impersonate");

                if (impersonate) {
                    this.isImpersonation = impersonate === "Y";
                }

                this.getQmRestrictionMessage();
                this.cacheDate = this.$cookies.get("cacheDate");

                this.$q.all([this.getCacheStatuses()])
                    .then(() => {
                        var dte = moment(this.cacheDate);
                        _.each(this.cacheStatuses, (status) => {
                            var current = moment(status.lastChangeDate);

                            if (this.cacheDate == null || dte < current) {
                                this.common.$broadcast(this.config.events.cacheInvalidated,
                                    { dbName: status.objectName });
                            }
                        });

                        this.$cookies.put("cacheDate", new Date().toISOString());
                    });


            });

            this.$rootScope.$on(this.config.events.spinnerToggle, () => {
                this.showSplash = !this.showSplash;
            });
        }

        getQmRestrictionMessage() {
            this.applicationSettingService.getByName("QM Restriction Message")
                .then((data) => {
                    if (data) {
                        this.config.qmMessage = data.value;    
                    }
                });
        }

        getCacheStatuses(): ng.IPromise<void> {
            return this.cacheStatusService.getAll()
                .then((data) => {
                    this.cacheStatuses = data;
                });
        }

        onStopImpersonation() {
            this.common.showSplash();

            this.accountService.stopImpersonate()
                .then((data: app.services.IGenericServiceResponse<services.IUser>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.common.currentUser = data.item;
                        this.isImpersonation = false;
                        this.fullName = this.common.currentUser.firstName + " " + this.common.currentUser.lastName;
                        this.isInspector = false;

                        this.common.$broadcast(this.config.events.userImpersonatedEnd, {});

                        this.$location.url("/Coordinator/Applications");
                        this.localStorageService.set("impersonate", "N");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to log out. Please contact support.");
                    this.common.hideSplash();
                });
        }

        logout(): void {
            this.common.showSplash();

            this.accountService.logout()
                .then((data: app.services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.fullName = "";
                        this.common.currentUser = {};
                        this.$location.path('/#');
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to log out. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onManageAccount() {
            this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/editUser.html",
                controller: "app.modal.templates.EditUserController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    parentUser: () => {
                        return this.common.currentUser;
                    },
                    isNewUser: () => {
                        return false;
                    },
                    currentOrganization: () => {
                        return null;
                    },
                    role: () => {
                        return "";
                    },
                    isPersonnel: () => {
                        return true;
                    }
                }
            });
        }

        openNav() {           
            //document.getElementById("mySidenav").style.width = "405px";  
            $("#mySidenav").show();         
            $("#spnMenuOpen").hide();
        }

        onAddBug() {
            this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/bug.html",
                controller: "app.modal.templates.BugController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window'
            });
        }
        
    }

    angular
        .module('app.layout')
        .controller('app.layout.ShellController',
        ShellController);

}