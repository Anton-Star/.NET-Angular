module app.services {
    'use strict';

    export interface IFacilitySiteService {
        getFacilitySite(): ng.IPromise<IGenericServiceResponse<IFacilitySitePage>>;
        saveRelation(facilitySiteId: number, siteId: number, facilityId: number): ng.IPromise<IGenericServiceResponse<boolean>>;
        deleteRelation(facilitySiteId: number): ng.IPromise<IGenericServiceResponse<boolean>>;
        search(siteId: number, facilityId: number): ng.IPromise<Array<IFacilitySite>>;
    }

    class FacilitySiteService implements IFacilitySiteService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getFacilitySite(): ng.IPromise<IGenericServiceResponse<IFacilitySitePage>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/FacilitySite/getFacilitySiteByUserIdAsync')
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IFacilitySitePage>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveRelation(facilitySiteId: number, siteId: number, facilityId: number): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/FacilitySite/saveRelationAsync', JSON.stringify({ facilitySiteId: facilitySiteId, siteId: siteId, facilityId: facilityId}))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        deleteRelation(facilitySiteId: number): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/FacilitySite/deleteRelationAsync', JSON.stringify({ facilitySiteId: facilitySiteId }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        search(siteId: number, facilityId: number): ng.IPromise<Array<IFacilitySite>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/FacilitySite/Search?siteId=' + siteId + "&facilityId=" + facilityId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IFacilitySite>>): void=> {
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
        config: IConfig): IFacilitySiteService {
        return new FacilitySiteService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('facilitySiteService',
        factory);
} 