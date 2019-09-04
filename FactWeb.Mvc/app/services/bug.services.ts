module app.services {
    'use strict';

    export interface IBugService {
        addBug(text: string, url: string, appUniqueId?: string): ng.IPromise<{}>;
    }

    class BugService implements IBugService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        addBug(text: string, url: string, appUniqueId?: string): ng.IPromise<{}> {
            var deferred = this.$q.defer();

            this.$http
                .post('/api/Bug', {
                    bugText: text,
                    bugUrl: url,
                    applicationUniqueId: appUniqueId == undefined || appUniqueId === "" ? null : appUniqueId
                })
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
        config: IConfig): IBugService {
        return new BugService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('bugService',
        factory);
} 