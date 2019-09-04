module app.services {
    'use strict';

    export interface ICpiTypeService {
        getAll(): ng.IPromise<ICpiType[]>;
    }

    class CpiTypeService implements ICpiTypeService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<ICpiType[]> {
            var deferred = this.$q.defer();
            this.$http.get("/api/CpiType")
                .success((response: ng.IHttpPromiseCallbackArg<{}>): void => {
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
        config: IConfig): ICpiTypeService {
        return new CpiTypeService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('cpiTypeService',
        factory);
} 