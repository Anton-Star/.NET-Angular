module app.services {
    'use strict';

    export interface IMembershipService {
        getAll(): ng.IPromise<Array<IMembership>>;
    }

    class MembershipService implements IMembershipService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<IMembership>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Membership/Get')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IMembership>>): void=> {
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
        config: IConfig): IMembershipService {
        return new MembershipService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('membershipService',
        factory);
} 