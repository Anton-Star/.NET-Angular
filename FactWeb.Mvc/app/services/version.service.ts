module app.services {
    'use strict';

    export interface IVersionService {
        makeVersionActive(version: services.IApplicationVersion): ng.IPromise<IServiceResponse>;
        add(version: IApplicationVersion): ng.IPromise<IGenericServiceResponse<IApplicationVersion>>;
        getActive(): ng.IPromise<Array<IApplicationVersion>>;
        remove(id: string): ng.IPromise<{}>;
        getById(id: string): ng.IPromise<IApplicationVersion>;
        getActiveSimple(): ng.IPromise<Array<IApplicationVersion>>;
        buildCache(type: number, versionId: string): ng.IPromise<{}>;
    }

    class VersionService implements IVersionService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        makeVersionActive(item: services.IApplicationVersion): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Version", {
                    id: item.id
                })
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        add(version: IApplicationVersion): ng.IPromise<IGenericServiceResponse<IApplicationVersion>>  {
            var deferred = this.$q.defer();
            this.$http
                .put("/api/Version", version)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IApplicationVersion>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getActive(): ng.IPromise<Array<IApplicationVersion>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Version/Active")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationVersion>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getActiveSimple(): ng.IPromise<Array<IApplicationVersion>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Version/Active/Simple")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationVersion>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getById(id: string): ng.IPromise<IApplicationVersion> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Version?version=" + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationVersion>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        remove(id: string): ng.IPromise<{}> {
            var deferred = this.$q.defer();
            this.$http
                .delete("/api/Version/" + id)
                .success((): void => {
                    deferred.resolve({});
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        buildCache(type: number, versionId: string): ng.IPromise<{}> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Version/Cache?type=" + type + "&versionId=" + versionId)
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
        config: IConfig): IVersionService {
        return new VersionService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('versionService',
        factory);
} 