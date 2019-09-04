module app.services {
    'use strict';

    export interface ICollectionTypeService {
        getAll(): ng.IPromise<Array<ICollectionType>>;
    }

    class CollectionTypeService implements ICollectionTypeService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<ICollectionType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/CollectionType")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICollectionType>>): void => {
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
        config: IConfig): ICollectionTypeService {
        return new CollectionTypeService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('collectionTypeService',
        factory);
} 