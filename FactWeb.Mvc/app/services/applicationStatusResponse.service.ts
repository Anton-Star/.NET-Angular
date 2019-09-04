module app.services {
    'use strict';

    export interface IApplicationResponseStatusService {
        getApplicationResponseStatus(): ng.IPromise<Array<IApplicationResponseStatusItem>>;
        saveStatus(applicationResponseStatusId: number, statusForFACT: string, statusForApplicant: string): ng.IPromise<IGenericServiceResponse<boolean>>;
    }

    class ApplicationResponseStatusService implements IApplicationResponseStatusService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getApplicationResponseStatus(): ng.IPromise<Array<IApplicationResponseStatusItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/ApplicationResponseStatus')
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IApplicationResponseStatusItem>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveStatus(applicationResponseStatusId: number, statusForFACT: string, statusForApplicant: string): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/ApplicationResponseStatus', JSON.stringify({ id: applicationResponseStatusId, name: statusForFACT, nameForApplicant: statusForApplicant }))
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
        config: IConfig): IApplicationResponseStatusService {
        return new ApplicationResponseStatusService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('applicationResponseStatusService',
        factory);
} 