module app.services {
    'use strict';

    export interface ICacheStatusService {
        getAll(): ng.IPromise<Array<ICacheStatus>>;
    }

    class CacheStatusService implements ICacheStatusService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<ICacheStatus>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/CacheStatus/')
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICacheStatus>>): void => {
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
        config: IConfig): ICacheStatusService {
        return new CacheStatusService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('cacheStatusService',
        factory);
} 