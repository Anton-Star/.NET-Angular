module app.services {
    'use strict';

    export interface ITransplantCellTypeService {
        getAll(): ng.IPromise<Array<ITransplantCellType>>;
    }

    class TransplantCellTypeService implements ITransplantCellTypeService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<ITransplantCellType>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/TransplantCellType")
                .success((response: ng.IHttpPromiseCallbackArg<Array<ITransplantCellType>>): void => {
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
        config: IConfig): ITransplantCellTypeService {
        return new TransplantCellTypeService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('transplantCellTypeService',
        factory);
} 