module app.services {
    'use strict';

    export interface IOrganizationService {
        getSimpleOrganizations(): ng.IPromise<ISimpleOrganization[]>;
        search(organizationName: string, city: string, state: string): ng.IPromise<Array<IOrganization>>;
        searchByOrgFacility(organizationId: number, facilityId: number): ng.IPromise<Array<IOrganization>>;
        getOrgByFacilityRelation(facilityId: string, strongRelation: boolean): ng.IPromise<Array<IOrganization>>;
        getAll(includeFac?: boolean, includeAll?: boolean): ng.IPromise<Array<IOrganization>>;
        getFlatOrganizations(): ng.IPromise<IOrganization[]>;
        getById(organizationId: number): ng.IPromise<IOrganization>;
        getByName(organizationName: string, includeFac: boolean, includeAll: boolean): ng.IPromise<IOrganization>;
        getOrganizationApplications(organizationId: number): ng.IPromise<Array<IApplication>>;
        save(organization: IOrganization): ng.IPromise<IGenericServiceResponse<IOrganization>>;
        update(organization: IOrganization): ng.IPromise<IServiceResponse>;
        getOrganizationTypes(): ng.IPromise<Array<IOrganizationTypeItem>>;
        getAccreditationStatus(): ng.IPromise<Array<IAccreditationStatus>>;
        getAccreditedServices(organizationId: number): ng.IPromise<string>;
        getByEligibilitySubmitted(): ng.IPromise<Array<IOrganization>>;
        getBAAOwner(): ng.IPromise<Array<IBAAOwner>>;
        getBAADocuments(organizationId: number): ng.IPromise<Array<IOrganizationBAADocumentItem>>;
        getOrgUsers(org: string, includeAll: boolean): ng.IPromise<Array<IUser>>;
        getOrgInspectors(applicationUniqueId: string): ng.IPromise<IInspection>;
        getOrgConsultants(organizationId: number): ng.IPromise<Array<IOrganizationConsultant>>;
        getOrgSites(name: string): ng.IPromise<Array<ISite>>;
        getSites(applicationUniqueId: string): ng.IPromise<Array<ISite>>;
        getApplicationStatus(): ng.IPromise<Array<IApplicationStatus>>; // duplicate
        updateApplicationStatus(applicationTypeId: number, applicationStatusName: string, organizationId: number): ng.IPromise<IServiceResponse>;// duplicate
        isDirector(orgName: string): ng.IPromise<boolean>;
        getNameByCompliance(id: string): ng.IPromise<string>;
    }

    class OrganizationService implements IOrganizationService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getSimpleOrganizations(): angular.IPromise<app.services.ISimpleOrganization[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Organization/Simple')
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISimpleOrganization>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise
        }

        getOrgUsers(org: string, includeAll: boolean): ng.IPromise<Array<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Organization/Users?name=' + encodeURIComponent(org) + '&includeAll=' + (includeAll ? "Y" :"N"))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IUser>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getOrgInspectors(applicationUniqueId: string): ng.IPromise<IInspection> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Organization/' + applicationUniqueId + '/Inspectors')
                .success((response: ng.IHttpPromiseCallbackArg<IInspection>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getNameByCompliance(id: string): ng.IPromise<string> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Organization/Compliance?id=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<string>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getOrgConsultants(organizationId: number): ng.IPromise<Array<IOrganizationConsultant>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Organization/' + organizationId + '/Consultants')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganizationConsultant>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        search(organizationName: string, city: string, state: string): ng.IPromise<Array<IOrganization>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Organization/search?organizationName=' + encodeURIComponent(organizationName) + "&city=" + city + "&state=" + state)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganization>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        searchByOrgFacility(organizationId: number, facilityId: number): ng.IPromise<Array<IOrganization>> {
            var data = {
                organizationId: organizationId,
                facilityId: facilityId
            };

            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/search?organizationId=" + data.organizationId + "&facilityId=" + data.facilityId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganization>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getOrgByFacilityRelation(facilityId: string, strongRelation: boolean): ng.IPromise<Array<IOrganization>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/?facilityId=" + facilityId + "&strongRelation=" + strongRelation)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganization>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAll(includeFac?: boolean, includeAll?: boolean): ng.IPromise<Array<IOrganization>>  {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization?includeFac=" + (includeFac == true ? "Y" : "N") + "&includeAll=" + (includeAll == true ? "Y" : "N"))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganization>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getFlatOrganizations(): ng.IPromise<IOrganization[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/Flat")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganization>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getByEligibilitySubmitted(): ng.IPromise<Array<IOrganization>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/EligibilitySubmitted")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganization>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getById(organizationId: number): ng.IPromise<IOrganization> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/" + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<IOrganization>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getByName(organizationName: string, includeFac: boolean, includeAll: boolean): ng.IPromise<IOrganization> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/Name?organizationName=" + encodeURIComponent(organizationName) + "&includeFac=" + (includeFac ? "Y": "N") + "&includeAll=" + (includeAll ? "Y" : "N"))
                .success((response: ng.IHttpPromiseCallbackArg<IOrganization>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getOrganizationApplications(organizationId: number): ng.IPromise<Array<IApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/" + organizationId + "/Applications")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(organization: IOrganization): ng.IPromise<IGenericServiceResponse<IOrganization>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Organization', JSON.stringify(organization))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IOrganization>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        update(organization: IOrganization): ng.IPromise<IServiceResponse> {            
            var deferred = this.$q.defer();
            this.$http
                .put('/api/Organization', JSON.stringify(organization))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        getOrganizationTypes(): ng.IPromise<Array<IOrganizationTypeItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/Types")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganizationTypeItem>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAccreditedServices(organizationId: number): ng.IPromise<string> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/AccreditedServices/" + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<string>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAccreditationStatus(): ng.IPromise <Array<IAccreditationStatus>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/AccreditationStatus")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IAccreditationStatus>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getBAAOwner(): ng.IPromise <Array<IBAAOwner>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/BaaOwner")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IBAAOwner>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getBAADocuments(organizationId): ng.IPromise<Array<IOrganizationBAADocumentItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/BAADocuments/" + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganizationBAADocumentItem>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getOrgSites(name: string): ng.IPromise<Array<ISite>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/Sites?name=" + encodeURIComponent(name))
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getSites(applicationUniqueId: string): ng.IPromise<Array<ISite>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Organization/Sites?app=" + applicationUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISite>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplicationStatus(): ng.IPromise<Array<IApplicationStatus>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Status")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationStatus>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  

        updateApplicationStatus(applicationTypeId: number, applicationStatusName: string, organizationId: number): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .put(
                "/api/Application/Status", JSON.stringify({ applicationTypeId: applicationTypeId, applicationStatusName: applicationStatusName, organizationId: organizationId }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        isDirector(orgName: string): ng.IPromise<boolean> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Organization/Director?organizationName=" + encodeURIComponent(orgName))
                .success((response: ng.IHttpPromiseCallbackArg<boolean>): void => {
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
        config: IConfig): IOrganizationService {
        return new OrganizationService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('organizationService',
        factory);
} 