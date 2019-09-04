module app.services {
    'use strict';

    export interface IJobFunctionService {
        getAll(): ng.IPromise<Array<IJobFunction>>;
    }

    class JobFunctionService implements IJobFunctionService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<IJobFunction>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/JobFunction')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IJobFunction>>): void=> {
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
        config: IConfig): IJobFunctionService {
        return new JobFunctionService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('jobFunctionService',
        factory);
} 