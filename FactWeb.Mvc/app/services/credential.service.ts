module app.services {
    'use strict';

    export interface ICredentialService {
        getAll(): ng.IPromise<Array<ICredential>>;
    }

    class CredentialService implements ICredentialService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<ICredential>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Credential')
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICredential>>): void => {
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
        config: IConfig): ICredentialService {
        return new CredentialService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('credentialService',
        factory);
} 