module app.services {
    'use strict';

    export interface ITemplateService {
        getAll(): ng.IPromise<Array<ITemplate>>;
        add(template: ITemplate): ng.IPromise<IGenericServiceResponse<ITemplate>>;
        update(template: ITemplate): ng.IPromise<IServiceResponse>;
        delete(id: string): ng.IPromise<IServiceResponse>;
    }

    class TemplateService implements ITemplateService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<ITemplate>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Template')
                .success((response: ng.IHttpPromiseCallbackArg<Array<ITemplate>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        add(template: ITemplate): ng.IPromise<IGenericServiceResponse<ITemplate>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Template', template)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ITemplate>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        update(template: ITemplate): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .put('/api/Template', template)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        delete(id: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .delete('/api/Template/' + id)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
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
        config: IConfig): ITemplateService {
        return new TemplateService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('templateService',
        factory);
} 