module app.services {
    'use strict';

    export interface IOutcomeStatusService {
        getAll(): ng.IPromise<Array<IOutcomeStatus>>;
    }

    class OutcomeStatusService implements IOutcomeStatusService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<IOutcomeStatus>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/OutcomeStatus')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOutcomeStatus>>): void => {
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
        config: IConfig): IOutcomeStatusService {
        return new OutcomeStatusService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('outcomeStatusService',
        factory);
} 