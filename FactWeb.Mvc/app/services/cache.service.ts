module app.services {
    'use strict';

    export interface ICacheService {
        getApplicationSettings(): ng.IPromise<IApplicationSetting[]>;
        getCbCategories(): ng.IPromise<ICbCategory[]>;
        getCbUnitTypes(): ng.IPromise<ICBUnitType[]>;
        getTransplantCellTypes(): ng.IPromise<ITransplantCellType[]>;
        getClinicalPopulationTypes(): ng.IPromise<IClinicalPopulationType[]>;
        getTransplantTypes(): ng.IPromise<ITransplantType[]>;
        getCollectionTypes(): ng.IPromise<ICollectionType[]>;
        getProcessingTypes(): ng.IPromise<IProcessingType[]>;
        getStates(): ng.IPromise<IState[]>;
        getCountries(): ng.IPromise<ICountry[]>;
        getEmailTemplates(): ng.IPromise<IEmailTemplate[]>;
        getReportReviewStatuses(): ng.IPromise<IReportReviewStatus[]>;
        getOutcomeStatuses(): ng.IPromise<IOutcomeStatus[]>;
        getAccreditationStatuses(): ng.IPromise<IAccreditationStatus[]>;
        getOrganizations(): ng.IPromise<Array<IOrganization>>;
        getFacilities(): ng.IPromise<Array<IFacility>>;
        getSites(): ng.IPromise<ISite[]>;
        getActiveVersions(): ng.IPromise<IApplicationVersion[]>;
        getNetcordMembershipTypes(): ng.IPromise<INetcordMembershipType[]>;
        clearCache(dbName?: string): void;
    }

    class CacheService implements ICacheService {
        dbs: string[];
        database: PouchDB.Database<{}>;

        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private notificationFactory: blocks.INotificationFactory,
            private applicationSettingService: IApplicationSettingService,
            private siteService: ISiteService,
            private transplantCellTypeService: ITransplantCellTypeService,
            private collectionTypeService: ICollectionTypeService,
            private emailTemplateService: IEmailTemplateService,
            private reportReviewStatusService: IReportReviewStatusService,
            private outcomeStatusService: IOutcomeStatusService,
            private organizationService: IOrganizationService,
            private facilityService: IFacilityService,
            private versionService: IVersionService,
            private netcordMembershipService: INetcordMembershipService,
            private localStorageService: any) {

            this.dbs = [
                this.config.cacheNames.appSettings,
                this.config.cacheNames.cbUnitTypes,
                this.config.cacheNames.cbCategories,
                this.config.cacheNames.transplantCellTypes,
                this.config.cacheNames.clinicalPopulationTypes,
                this.config.cacheNames.transplantTypes,
                this.config.cacheNames.collectionTypes,
                this.config.cacheNames.processingTypes,
                this.config.cacheNames.states,
                this.config.cacheNames.countries,
                this.config.cacheNames.emailTemplates,
                this.config.cacheNames.reportReviewStatuses,
                this.config.cacheNames.outcomeStatuses,
                this.config.cacheNames.organizations,
                this.config.cacheNames.facilities,
                this.config.cacheNames.sites,
                this.config.cacheNames.cacheStatuses,
                this.config.cacheNames.netcordMembershipTypes,
                this.config.cacheNames.activeVersions
            ];

        }

        clearCache(dbName?: string) {
            if (dbName) {
                this.clearCacheByName(dbName);
            } else {
                _.each(this.dbs,
                    (db) => {
                        this.clearCacheByName(db);
                    });
            }
        }

        clearCacheByName(name: string) {
            this.localStorageService.remove(name);
        }

        getNetcordMembershipTypes(): ng.IPromise<INetcordMembershipType[]> {
            var deferred = this.$q.defer();
            var items: INetcordMembershipType[] = [];
            var db = this.config.cacheNames.netcordMembershipTypes;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.netcordMembershipService.getTypes()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getActiveVersions(): ng.IPromise<IApplicationVersion[]> {
            var deferred = this.$q.defer();
            var items: IApplicationVersion[] = [];
            var db = this.config.cacheNames.activeVersions;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);               
            } else {
                this.versionService.getActive()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getSites(): ng.IPromise<ISite[]> {
            var deferred = this.$q.defer();
            var items: ISite[] = [];
            var db = this.config.cacheNames.sites;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getFacilities(): ng.IPromise<Array<IFacility>> {
            var deferred = this.$q.defer();
            var items: IFacility[] = [];
            var db = this.config.cacheNames.facilities;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);
                
                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.facilityService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getOrganizations(): ng.IPromise<Array<IOrganization>> {
            var deferred = this.$q.defer();
            var items: IOrganization[] = [];
            var db = this.config.cacheNames.organizations;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.organizationService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getAccreditationStatuses(): ng.IPromise<IAccreditationStatus[]> {
            var deferred = this.$q.defer();
            var items: IAccreditationStatus[] = [];
            var db = this.config.cacheNames.accreditationStatuses;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.organizationService.getAccreditationStatus()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;

        }

        getOutcomeStatuses(): ng.IPromise<IOutcomeStatus[]> {
            var deferred = this.$q.defer();
            var items: IOutcomeStatus[] = [];
            var db = this.config.cacheNames.outcomeStatuses;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.outcomeStatusService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;

        }

        getReportReviewStatuses(): ng.IPromise<IReportReviewStatus[]> {
            var deferred = this.$q.defer();
            var items: IReportReviewStatus[] = [];
            var db = this.config.cacheNames.reportReviewStatuses;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.reportReviewStatusService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;

        }

        getEmailTemplates(): ng.IPromise<IEmailTemplate[]> {
            var deferred = this.$q.defer();
            var items: IEmailTemplate[] = [];
            var db = this.config.cacheNames.emailTemplates;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.emailTemplateService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
            
        }

        getCountries(): ng.IPromise<ICountry[]> {
            var deferred = this.$q.defer();
            var items: ICountry[] = [];
            var db = this.config.cacheNames.countries;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getCountriesList()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getStates(): ng.IPromise<IState[]> {
            var deferred = this.$q.defer();
            var items: IState[] = [];
            var db = this.config.cacheNames.states;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getStatesList()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getProcessingTypes(): ng.IPromise<IProcessingType[]> {
            var deferred = this.$q.defer();
            var items: IProcessingType[] = [];
            var db = this.config.cacheNames.processingTypes;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getProcessingTypes()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getCollectionTypes(): ng.IPromise<ICollectionType[]> {
            var deferred = this.$q.defer();
            var items: ICollectionType[] = [];
            var db = this.config.cacheNames.collectionTypes;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.collectionTypeService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
            
        }

        getTransplantTypes(): ng.IPromise<ITransplantType[]> {
            var deferred = this.$q.defer();
            var items: ITransplantType[] = [];
            var db = this.config.cacheNames.transplantTypes;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getTransplantTypes()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getClinicalPopulationTypes(): ng.IPromise<IClinicalPopulationType[]> {
            var deferred = this.$q.defer();
            var items: IClinicalPopulationType[] = [];
            var db = this.config.cacheNames.clinicalPopulationTypes;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getClinicalPopulationTypes()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getTransplantCellTypes(): ng.IPromise<ITransplantCellType[]> {
            var deferred = this.$q.defer();
            var items: ITransplantCellType[] = [];
            var db = this.config.cacheNames.transplantCellTypes;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.transplantCellTypeService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
            
        }

        getCbCategories(): ng.IPromise<ICbCategory[]> {
            var deferred = this.$q.defer();
            var items: ICbCategory[] = [];
            var db = this.config.cacheNames.cbCategories;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getCbCategories()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getCbUnitTypes(): ng.IPromise<ICBUnitType[]> {
            var deferred = this.$q.defer();
            var items: ICBUnitType[] = [];
            var db = this.config.cacheNames.cbUnitTypes;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.siteService.getCBUnitTypes()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }

        getApplicationSettings(): ng.IPromise<IApplicationSetting[]> {
            var deferred = this.$q.defer();
            var items: IApplicationSetting[] = [];
            var db = this.config.cacheNames.appSettings;

            if (this.localStorageService.get(db) != null && this.localStorageService.get(db) !== "") {
                var data = this.localStorageService.get(db);

                items = JSON.parse(data);

                setTimeout(() => {
                    deferred.resolve(items);
                }, 1000);
            } else {
                this.applicationSettingService.getAll()
                    .then((data) => {
                        items = data;

                        this.localStorageService.set(db, JSON.stringify(data));

                        deferred.resolve(items);
                    });
            }

            return deferred.promise;
        }
    }

    factory.$inject = [
        '$http',
        '$q',
        'common',
        'config',
        'notificationFactory',
        'applicationSettingService',
        'siteService',
        'transplantCellTypeService',
        'collectionTypeService',
        'emailTemplateService',
        'reportReviewStatusService',
        'outcomeStatusService',
        'organizationService',
        'facilityService',
        'versionService',
        'netcordMembershipService',
        'localStorageService'
    ];
    function factory($http: ng.IHttpService,
        $q: ng.IQService,
        common: app.common.ICommonFactory,
        config: IConfig,
        notificationFactory: app.blocks.INotificationFactory,
        applicationSettingService: app.services.IApplicationSettingService,
        siteService: app.services.ISiteService,
        transplantCellTypeService: app.services.ITransplantCellTypeService,
        collectionTypeService: app.services.ICollectionTypeService,
        emailTemplateService: app.services.IEmailTemplateService,
        reportReviewStatusService: app.services.IReportReviewStatusService,
        outcomeStatusService: app.services.IOutcomeStatusService,
        organizationService: app.services.IOrganizationService,
        facilityService: app.services.IFacilityService,
        versionService: IVersionService,
        netcordMembershipService: INetcordMembershipService,
        localStorageService: any): app.services.ICacheService {
        return new CacheService($http,
            $q,
            common,
            config,
            notificationFactory,
            applicationSettingService,
            siteService,
            transplantCellTypeService,
            collectionTypeService,
            emailTemplateService,
            reportReviewStatusService,
            outcomeStatusService,
            organizationService,
            facilityService,
            versionService,
            netcordMembershipService,
            localStorageService);
    }

    angular
        .module('app.services')
        .factory('cacheService',
        factory);
} 