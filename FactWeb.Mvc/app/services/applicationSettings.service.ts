module app.services {
    'use strict';

    export interface IApplicationSettingService {
        getAll(): ng.IPromise<Array<IApplicationSetting>>;
        getByName(name: string): ng.IPromise<IApplicationSetting>;
        set(name: string, value: string): ng.IPromise<IServiceResponse>;
        save(settings: Array<IApplicationSetting>): ng.IPromise<IServiceResponse>;
    }

    class ApplicationSettingService implements IApplicationSettingService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }
        
        getAll(): ng.IPromise<Array<IApplicationSetting>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/ApplicationSetting")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationSetting>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getByName(name: string): ng.IPromise<IApplicationSetting> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/ApplicationSetting/" + name)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        set(name: string, value: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/ApplicationSetting", {
                    applicationSettingName: name,
                    applicationSettingValue: value
                })
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(settings: Array<IApplicationSetting>): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .put("/api/ApplicationSetting", {
                    settings: settings
                })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
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
        config: IConfig): IApplicationSettingService {
        return new ApplicationSettingService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('applicationSettingService',
        factory);
} 