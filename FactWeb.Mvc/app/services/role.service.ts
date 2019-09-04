module app.services {
    'use strict';

    export interface IRoleService {
        getRoles(): ng.IPromise<Array<IRole>>;
        getRolesByRole(roleId: number): ng.IPromise<Array<IRole>>;
    }

    class RoleService implements IRoleService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getRoles(): ng.IPromise<Array<IRole>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Role")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IRole>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getRolesByRole(roleId: number): ng.IPromise<Array<IRole>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Role?roleId=' + roleId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IRole>>): void => {
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
        config: IConfig): IRoleService {
        return new RoleService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('roleService',
        factory);
} 