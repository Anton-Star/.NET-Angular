module app.services {
    'use strict';

    export interface IOrganizationFacilityService {
        getOrganizationFacility(): ng.IPromise<Array<IOrganizationFacility>>;
        saveRelation(organizationFacilityId:number, organizationId: number, facilityId: number, relation: boolean): ng.IPromise<IGenericServiceResponse<boolean>>;
        deleteRelation(organizationFacilityId: number): ng.IPromise<IGenericServiceResponse<boolean>>;
        search(organizationId: number, facilityId: number): ng.IPromise<Array<IOrganizationFacility>>;
        getSitesByOrganization(organizationId: number): ng.IPromise<Array<IFacilitySite>>;
    }

    class OrganizationFacilityService implements IOrganizationFacilityService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }
            
        getOrganizationFacility(): ng.IPromise<Array<IOrganizationFacility>> {
            var deferred = this.$q.defer();            
            this.$http
                .get('/api/OrganizationFacility')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganizationFacility>>): void=> {                    
                    deferred.resolve(response);
                })
                .error((e) => {                    
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveRelation(organizationFacilityId:number,organizationId: number, facilityId: number, relation: boolean): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/OrganizationFacility/saveRelationAsync', JSON.stringify({ organizationFacilityId:organizationFacilityId, organizationId: organizationId, facilityId: facilityId, relation: relation}))                
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void=> {                    
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });            
            return deferred.promise;
        }

        deleteRelation(organizationFacilityId: number): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/OrganizationFacility/deleteRelationAsync', JSON.stringify({ organizationFacilityId: organizationFacilityId}))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        search(organizationId: number, facilityId: number): ng.IPromise<Array<IOrganizationFacility>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/OrganizationFacility/Search?organizationId=' + organizationId + "&facilityId=" + facilityId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganizationFacility>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getSitesByOrganization(organizationId: number): ng.IPromise<Array<IFacilitySite>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/OrganizationFacility/GetSitesByOrganization?organizationId=' + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IFacilitySite>>): void=> {
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
        config: IConfig): IOrganizationFacilityService {
        return new OrganizationFacilityService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('organizationFacilityService',
        factory);
} 