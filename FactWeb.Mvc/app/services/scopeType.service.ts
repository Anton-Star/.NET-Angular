module app.services {
    'use strict';

    export interface IScopeTypeService {
        get(): ng.IPromise<Array<IScopeType>>;
        getAllActiveNonArchivedAsync(): ng.IPromise<Array<IScopeType>>;
        save(scopeTypeId:number,scopeName:string, importName:string, isArchived:boolean, isActive:boolean): ng.IPromise<IGenericServiceResponse<IScopeType>>;
        delete(scopeTypeId: number): ng.IPromise<IGenericServiceResponse<IScopeType>>;        
    }

    class ScopeTypeService implements IScopeTypeService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        get(): ng.IPromise<Array<IScopeType>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/ScopeType/getAsync')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IScopeType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAllActiveNonArchivedAsync(): ng.IPromise<Array<IScopeType>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/ScopeType/getAllActiveNonArchivedAsync')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IScopeType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        

        save(scopeTypeId: number, scopeName: string, importName: string, isArchived:boolean, isActive: boolean): ng.IPromise<IGenericServiceResponse<IScopeType>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/ScopeType/save', JSON.stringify({ scopeTypeId: scopeTypeId, name: scopeName, importName: importName, isArchived: isArchived, isActive: isActive }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IScopeType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }
        
        delete(scopeTypeId: number): ng.IPromise<IGenericServiceResponse<IScopeType>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/ScopeType/delete', JSON.stringify({ scopeTypeId: scopeTypeId }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IScopeType>>): void=> {
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
        config: IConfig): IScopeTypeService {
        return new ScopeTypeService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('scopeTypeService',
        factory);
} 