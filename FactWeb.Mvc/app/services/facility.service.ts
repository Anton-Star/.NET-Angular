module app.services {
    'use strict';

    export interface IFacilityService {
        getAll(): ng.IPromise<Array<IFacility>>;
        getAllFlat(): ng.IPromise<IFacility[]>;
        getById(facilityId: number): ng.IPromise<IFacility>;
        getByIdWithChild(facilityId: number): ng.IPromise<IFacility>;
        getMasterServiceTypes(): ng.IPromise<Array<IMasterServiceTypeItem>>;
        getServiceTypes(): ng.IPromise<Array<IServiceTypeItem>>;
        getFacilityAccredidations(): ng.IPromise<Array<IFacilityAccreditationItem>>;
        save(facility: IFacility): ng.IPromise<IGenericServiceResponse<IFacility>>;
        delete(facilityId: number): ng.IPromise<IGenericServiceResponse<IFacility>>;
        getCBCollectionSiteTypes(facilityId: number): ng.IPromise<string>;
        getCibmtrData(facilityName: string): ng.IPromise<ICibmtr[]>;
        getCibmtrDataForOrg(orgName: string): ng.IPromise<ICibmtr[]>;
        saveCibmtr(data: ICibmtr): ng.IPromise<IGenericServiceResponse<string>>;
        saveOutcomes(data: ICibmtrOutcomeAnalysis[]): ng.IPromise<IServiceResponse>;
        saveCibmtrOutcome(data: ICibmtrOutcomeAnalysis): ng.IPromise<IGenericServiceResponse<string>>;
        saveCibmtrData(data: ICibmtrDataMgmt): ng.IPromise<IGenericServiceResponse<string>>;
        updateCibmtrs(outcomes: ICibmtrOutcomeAnalysis[], mgmts: ICibmtrDataMgmt[]): ng.IPromise<IServiceResponse>;
    }

    class FacilityService implements IFacilityService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<IFacility>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Facility/AllActive")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IFacility>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  

        getAllFlat(): ng.IPromise<Array<IFacility>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Facility/Flat")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IFacility>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  

        getById(facilityId: number): ng.IPromise<IFacility> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/FacilityWithChild/" + facilityId)
                .success((response: ng.IHttpPromiseCallbackArg<IFacility>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getByIdWithChild(facilityId: number): ng.IPromise<IFacility> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/FacilityWithChild/" + facilityId)
                .success((response: ng.IHttpPromiseCallbackArg<IFacility>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getMasterServiceTypes(): ng.IPromise<Array<IMasterServiceTypeItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Facility/MasterServiceTypes")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IMasterServiceTypeItem>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getServiceTypes(): ng.IPromise<Array<IServiceTypeItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Facility/ServiceTypes")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IServiceTypeItem>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getFacilityAccredidations(): ng.IPromise<Array<IFacilityAccreditationItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Facility/FacilityAccredidations")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IFacilityAccreditationItem>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getCBCollectionSiteTypes(facilityId: number): ng.IPromise<string> {
            var deferred = this.$q.defer();            
            this.$http
                .get("/api/Facility/CBCollectionSiteTypes?facilityId=" + facilityId)
                .success((response: ng.IHttpPromiseCallbackArg<string>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        save(facility: IFacility): ng.IPromise<IGenericServiceResponse<IFacility>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Facility', JSON.stringify(facility))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IFacility>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }             

        
        delete(facilityId: number): ng.IPromise<IGenericServiceResponse<IFacility>> {            
            var deferred = this.$q.defer();
            this.$http
                .delete('/api/Facility?facilityId=' + facilityId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IFacility>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }   

        getCibmtrData(facilityName: string): ng.IPromise<ICibmtr[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Facility/Cibmtr?facilityName=" + encodeURIComponent(facilityName))
                .success((response: ng.IHttpPromiseCallbackArg<ICibmtr[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCibmtrDataForOrg(orgName: string): ng.IPromise<ICibmtr[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Facility/Cibmtr?orgName=" + encodeURIComponent(orgName))
                .success((response: ng.IHttpPromiseCallbackArg<ICibmtr[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveCibmtr(data: ICibmtr): ng.IPromise<IGenericServiceResponse<string>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Facility/Cibmtr', data)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<string>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveOutcomes(data: ICibmtrOutcomeAnalysis[]): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Facility/Cibmtr/Outcomes', data)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveCibmtrOutcome(data: ICibmtrOutcomeAnalysis): ng.IPromise<IGenericServiceResponse<string>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Facility/Cibmtr/Outcome', data)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<string>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        updateCibmtrs(outcomes: ICibmtrOutcomeAnalysis[], mgmts: ICibmtrDataMgmt[]): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .put('/api/Facility/Cibmtr', {
                    cibmtrOutcomeAnalyses: outcomes,
                    cibmtrDataMgmts: mgmts
                })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveCibmtrData(data: ICibmtrDataMgmt): ng.IPromise<IGenericServiceResponse<string>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Facility/Cibmtr/Data', data)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<string>>): void => {
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
        config: IConfig): IFacilityService {
        return new FacilityService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('facilityService',
        factory);
} 