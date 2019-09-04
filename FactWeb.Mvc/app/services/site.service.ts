module app.services {
    'use strict';

    export interface ISiteService {
        search(siteName: string): ng.IPromise<Array<ISite>>;
        searchBySite(siteId: string): ng.IPromise<Array<ISite>>;
        getAll(): ng.IPromise<Array<ISite>>;
        getFlatSites(): ng.IPromise<IFlatSite[]>;
        getByName(name: string): ng.IPromise<ISite>;
        save(site: ISite): ng.IPromise<IGenericServiceResponse<ISite>>;
        getAddressTypesList(): ng.IPromise<Array<IAddressType>>;
        getStatesList(): ng.IPromise<Array<IState>>;
        getCountriesList(): ng.IPromise<Array<ICountry>>;
        getClinicalTypes(): ng.IPromise<Array<IClinicalType>>;
        getProcessingTypes(): ng.IPromise<Array<IProcessingType>>;
        getCollectionProductTypes(): ng.IPromise<Array<ICollectionProductType>>;
        getCBCollectionTypes(): ng.IPromise<Array<ICBCollectionType>>;
        getCBUnitTypes(): ng.IPromise<Array<ICBUnitType>>;
        getClinicalPopulationTypes(): ng.IPromise<Array<IClinicalPopulationType>>;
        getTransplantTypes(): ng.IPromise<Array<ITransplantType>>;
        getCbCategories(): ng.IPromise<Array<ICbCategory>>;
        getAddress(organizationId: number): ng.IPromise<ISite>;
        getSitesByOrganizationId(organizationId: number): ng.IPromise<Array<ISite>>;
        saveCordBloodTotals(total: ISiteCordBloodTransplantTotal): ng.IPromise<IGenericServiceResponse<ISiteCordBloodTransplantTotal>>;
        saveTransplantTotals(total: ISiteTransplantTotal): ng.IPromise<IGenericServiceResponse<ISiteTransplantTotal>>;
        saveCollectionTotals(total: ISiteCollectionTotal): ng.IPromise<IGenericServiceResponse<ISiteCollectionTotal>>;
        saveProcessingTotals(total: ISiteProcessingTotal): ng.IPromise<IGenericServiceResponse<ISiteProcessingTotal>>;
        saveMethodologyTotals(total: ISiteProcessingMethodologyTotal): ng.IPromise<IGenericServiceResponse<ISiteProcessingMethodologyTotal>>;
        getSitesByCompliance(compAppId: string): ng.IPromise<ISite[]>;
        deleteTotal(id: string, type: string): ng.IPromise<IServiceResponse>;
    }

    class SiteService implements ISiteService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        search(siteName: string): ng.IPromise<Array<ISite>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Site/search?siteName=' + siteName)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        searchBySite(siteId: string): ng.IPromise<Array<ISite>> {
            var data = {
                siteId: siteId == null || siteId == '0' || siteId == ' ' ? null : siteId
            };

            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/search?siteId=" + data.siteId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getFlatSites(): ng.IPromise<IFlatSite[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/Flat")
                .success((response: ng.IHttpPromiseCallbackArg<IFlatSite[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAll(): ng.IPromise<Array<ISite>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getall")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getByName(name: string): ng.IPromise<ISite> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site?name=" + encodeURIComponent(name))
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(site: ISite): ng.IPromise<IGenericServiceResponse<ISite>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Site', JSON.stringify(site))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ISite>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveCordBloodTotals(total: ISiteCordBloodTransplantTotal): ng.IPromise<IGenericServiceResponse<ISiteCordBloodTransplantTotal>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Site/CordBloodTotal', JSON.stringify(total))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ISiteCordBloodTransplantTotal>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveTransplantTotals(total: ISiteTransplantTotal): ng.IPromise<IGenericServiceResponse<ISiteTransplantTotal>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Site/TransplantTotal', JSON.stringify(total))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ISiteCordBloodTransplantTotal>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveCollectionTotals(total: ISiteCollectionTotal): ng.IPromise<IGenericServiceResponse<ISiteCollectionTotal>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Site/CollectionTotal', JSON.stringify(total))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ISiteCollectionTotal>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveProcessingTotals(total: ISiteProcessingTotal): ng.IPromise<IGenericServiceResponse<ISiteProcessingTotal>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Site/ProcessingTotal', JSON.stringify(total))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ISiteProcessingTotal>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveMethodologyTotals(total: ISiteProcessingMethodologyTotal): ng.IPromise<IGenericServiceResponse<ISiteProcessingMethodologyTotal>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Site/ProcessingMethodologyTotal', JSON.stringify(total))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ISiteProcessingMethodologyTotal>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        getAddressTypesList(): ng.IPromise<Array<IAddressType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/AddressTypes")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IAddressType>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getSitesByOrganizationId(organizationId: number): ng.IPromise<Array<ISite>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/SitesByOrganizationId?organizationId=" + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAddress(organizationId: number): ng.IPromise<ISite> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/Address?organizationId=" + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<ISite>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }


        getStatesList(): ng.IPromise<Array<IState>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getStatesListAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IState>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCountriesList(): ng.IPromise<Array<ICountry>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getCountriesListAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICountry>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getClinicalTypes(): ng.IPromise<Array<IClinicalType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getClinicalTypesAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IClinicalType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getProcessingTypes(): ng.IPromise<Array<IProcessingType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getProcessingTypesAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IProcessingType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCollectionProductTypes(): ng.IPromise<Array<ICollectionProductType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getCollectionProductTypesAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICollectionProductType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCBCollectionTypes(): ng.IPromise<Array<ICBCollectionType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getCBCollectionTypesAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICBCollectionType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCBUnitTypes(): ng.IPromise<Array<ICBUnitType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getCBUnitTypesAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICBUnitType>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getClinicalPopulationTypes(): ng.IPromise<Array<IClinicalPopulationType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getClinicalPopulationTypesAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IClinicalPopulationType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getTransplantTypes(): ng.IPromise<Array<ITransplantType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/getTransplantTypesAsync")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ITransplantType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCbCategories(): ng.IPromise<Array<ICbCategory>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/CBCategory")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICbCategory>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getSitesByCompliance(compAppId: string): ng.IPromise<ISite[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Site/Compliance?compAppId=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        deleteTotal(id: string, type: string): ng.IPromise<IServiceResponse> {
            var url = "/api/Site/";
            switch (type) {
                case "CB":
                    url += "CordBloodTotal/";
                    break;
                case "TO":
                    url += "TransplantTotal/";
                    break;
                case "CO":
                    url += "CollectionTotal/";
                    break;
                case "PO":
                    url += "ProcessingTotal/";
                    break;
                case "MO":
                    url += "ProcessingMethodologyTotal/";
                    break;
            }

            url += id;

            var deferred = this.$q.defer();
            this.$http
                .delete(url)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

    }

    factory.$inject = [
        '$http',
        '$q',
        'config'
    ];
    function factory($http: ng.IHttpService,
        $q: ng.IQService,
        config: IConfig): ISiteService {
        return new SiteService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('siteService',
        factory);
} 