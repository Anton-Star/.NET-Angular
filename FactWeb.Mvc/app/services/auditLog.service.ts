module app.services {
    'use strict';

    export interface IAuditLogService {
        getAuditLog(): ng.IPromise<Array<IAuditLog>>;
    }

    class AuditLogService implements IAuditLogService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAuditLog(): ng.IPromise<Array<IAuditLog>> {
            var deferred = this.$q.defer();
            
            this.$http
                .get('/api/AuditLog/GetAuditLogAsync')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IAuditLog>>): void=> {
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
        config: IConfig): IAuditLogService {
        return new AuditLogService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('auditLogService',
        factory);
} 