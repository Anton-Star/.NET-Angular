module app.services {
    'use strict';

    export interface INetcordMembershipService {
        getTypes(): ng.IPromise<INetcordMembershipType[]>;
    }

    class NetcordMembershipService implements INetcordMembershipService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getTypes(): ng.IPromise<INetcordMembershipType[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/NetcordMembership/Types')
                .success((response: ng.IHttpPromiseCallbackArg<INetcordMembershipType[]>): void => {
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
        config: IConfig): INetcordMembershipService {
        return new NetcordMembershipService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('netcordMembershipService',
        factory);
} 