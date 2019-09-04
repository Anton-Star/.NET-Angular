module app.services {
    'use strict';

    export interface IAppLogService {
        add(message: string): ng.IPromise<{}>;
    }

    class AppLogService implements IAppLogService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        add(message: string): ng.IPromise<{}> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/AppLog", {
                    message: message
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
        config: IConfig): IAppLogService {
        return new AppLogService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('appLogService',
        factory);
} 