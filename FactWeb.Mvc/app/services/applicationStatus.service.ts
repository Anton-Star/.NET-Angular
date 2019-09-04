module app.services {
    'use strict';

    export interface IApplicationStatusService {
        getApplicationStatus(): ng.IPromise<Array<IApplicationStatus>>;
        saveStatus(applicationStatusId: number, statusForFACT: string, statusForApplicant: string): ng.IPromise<IGenericServiceResponse<boolean>>;
    }

    class ApplicationStatusService implements IApplicationStatusService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getApplicationStatus(): ng.IPromise<Array<IApplicationStatus>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/ApplicationStatus')
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IApplicationStatus>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveStatus(applicationStatusId: number, statusForFACT: string, statusForApplicant: string): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/ApplicationStatus', JSON.stringify({ id: applicationStatusId, name: statusForFACT, nameForApplicant: statusForApplicant }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
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
        config: IConfig): IApplicationStatusService {
        return new ApplicationStatusService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('applicationStatusService',
        factory);
} 