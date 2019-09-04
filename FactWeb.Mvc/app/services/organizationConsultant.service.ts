module app.services {
    'use strict';

    export interface IOrganizationConsultantService {
        getOrganizationConsultants(): ng.IPromise<Array<IOrganizationConsultant>>;
        save(organizationConsultantId: number, organizationId: number, consultantId: number, startDate:string, endDate:string): ng.IPromise<IGenericServiceResponse<boolean>>;
        delete(organizationConsultantId: number): ng.IPromise<IGenericServiceResponse<boolean>>;                
    }

    class OrganizationConsultantService implements IOrganizationConsultantService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getOrganizationConsultants(): ng.IPromise<Array<IOrganizationConsultant>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/OrganizationConsultant/')//getOrganizationConsultantsAsync
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganizationConsultant>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(organizationConsultantId: number, organizationId: number, consultantId: number, startDate: string, endDate: string): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/OrganizationConsultant/', JSON.stringify({ organizationConsultantId: organizationConsultantId, organizationId: organizationId, consultantId: consultantId, startDate: startDate, endDate: endDate}))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }
        
        delete(organizationConsultantId: number): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http                
                .delete('/api/OrganizationConsultant/' + organizationConsultantId)            
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
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
        config: IConfig): IOrganizationConsultantService {
        return new OrganizationConsultantService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('organizationConsultantService',
        factory);
} 