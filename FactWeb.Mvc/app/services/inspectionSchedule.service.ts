module app.services {
    'use strict';

    export interface IInspectionScheduleService {
        getDefaultPageData(): ng.IPromise<IGenericServiceResponse<IInspectionSchedulePage>>;        
        getInspectionScheduleDetail(organizationId: string, applicationId: string, inspectionScheduleId: string): ng.IPromise<IGenericServiceResponse<IInspectionScheduleDetailPage>>;
        deleteSchedule(inspectionScheduleId: number): ng.IPromise<IGenericServiceResponse<boolean>>;        
        saveInspectionSchedule(inspectionScheduleDetailId: string, inspectionScheduleId: string, organizationId: string, applicationId: string, selectedUserId: string, selectedRoleId: string, selectedCategoryId: string, lead: boolean, mentor: boolean, startDate: string, endDate: string, selectedFacilityList: Array<IFacilitySite>, selectedSiteId:string): ng.IPromise<IGenericServiceResponse<number>>;
        deleteStaff(inspectionScheduleDetailId: number): ng.IPromise<IGenericServiceResponse<boolean>>;  
        getAllInspectionCategories(): ng.IPromise<Array<IInspectionCategory>>;        
        getInspectionCategories(applicationId:string): ng.IPromise<Array<IInspectionCategory>>;        
        getFacilities(organizationId: string, inspectionScheduleId: string): ng.IPromise<Array<IOrganizationFacility>>;        
        getSites(organizationId: string, inspectionScheduleId: string): ng.IPromise<Array<IFacilitySite>>;        
        getAccreditationRole(userId: string, uniqueId: string): ng.IPromise<IGenericServiceResponse<IAccreditationRole>>;
        getInspectionSchedule(organizationId: number, applicationId: number): ng.IPromise<IGenericServiceResponse<IInspectionSchedule>>;
        getAllInspectionSchedules(organizationId: number, applicationId: number): ng.IPromise<Array<IInspectionSchedule>>;
        getInspectionScheduleDetailForCompliance(appId: string): ng.IPromise<IInspectionScheduleDetail[]>;
        getSchedulesForCompliance(compAppId: string): ng.IPromise<IInspectionSchedule[]>;
    }

    class InspectionScheduleService implements IInspectionScheduleService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getDefaultPageData(): ng.IPromise<IGenericServiceResponse<IInspectionSchedulePage>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/InspectionSchedule/GetDefaultPageDataAsync')
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IInspectionSchedulePage>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        getInspectionScheduleDetail(organizationId: string, applicationId: string, inspectionScheduleId: string): ng.IPromise<IGenericServiceResponse<IInspectionScheduleDetailPage>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/InspectionSchedule/getInspectionScheduleDetail?organizationId=' + organizationId + '&applicationId=' + applicationId + '&inspectionScheduleId=' + inspectionScheduleId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<Array<IInspectionScheduleDetailPage>>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getInspectionSchedule(organizationId: number, applicationId: number): ng.IPromise<IGenericServiceResponse<IInspectionSchedule>>
        {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/InspectionSchedule/getInspectionSchedule?organizationId=' + organizationId + '&applicationId=' + applicationId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<Array<IInspectionScheduleDetailPage>>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAllInspectionSchedules(organizationId: number, applicationId: number): ng.IPromise<Array<IInspectionSchedule>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/InspectionSchedule/getAllInspectionSchedules?organizationId=' + organizationId + '&applicationId=' + applicationId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IInspectionSchedule>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        deleteSchedule(inspectionScheduleId: number): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/InspectionSchedule/DeleteSchedule', JSON.stringify({ inspectionScheduleId: inspectionScheduleId }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveInspectionSchedule(inspectionScheduleDetailId: string, inspectionScheduleId: string, organizationId: string, applicationId: string, selectedUserId: string, selectedRoleId: string, selectedCategoryId: string, lead: boolean, mentor: boolean, startDate: string, endDate: string, selectedSiteList: Array<IFacilitySite>, selectedSiteId: string): ng.IPromise<IGenericServiceResponse<number>> {            
            var deferred = this.$q.defer();
            this.$http
                .post('/api/InspectionSchedule/SaveInspectionSchedule', JSON.stringify({ inspectionScheduleDetailId: inspectionScheduleDetailId, inspectionScheduleId: inspectionScheduleId, organizationId: organizationId, applicationId: applicationId, selectedUserId: selectedUserId, selectedRoleId: selectedRoleId, selectedCategoryId: selectedCategoryId, lead: lead, mentor: mentor, startDate: startDate, endDate: endDate, selectedSiteList: selectedSiteList, selectedSiteId: selectedSiteId}))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }
        
        deleteStaff(inspectionScheduleDetailId: number): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/InspectionSchedule/DeleteStaff', JSON.stringify({ inspectionScheduleDetailId: inspectionScheduleDetailId }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAllInspectionCategories(): ng.IPromise<Array<IInspectionCategory>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/InspectionSchedule/GetInspectionCategories")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IInspectionCategory>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getInspectionCategories(applicationId:string): ng.IPromise<Array<IInspectionCategory>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/InspectionSchedule/GetInspectionCategories?applicationId=" + applicationId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IInspectionCategory>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        getFacilities(organizationId: string, inspectionScheduleId: string): ng.IPromise<Array<IOrganizationFacility>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/InspectionSchedule/GetFacilitiesAsync?organizationId=" + organizationId + "&inspectionScheduleId=" + inspectionScheduleId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IOrganizationFacility>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
                
        getSites(organizationId: string, inspectionScheduleId: string): ng.IPromise<Array<IFacilitySite>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/InspectionSchedule/GetSitesAsync?organizationId=" + organizationId + "&inspectionScheduleId=" + inspectionScheduleId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IFacilitySite>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }       

        getAccreditationRole(userId: string, uniqueId: string): ng.IPromise<IGenericServiceResponse<IAccreditationRole>> {            
            var deferred = this.$q.defer();
            this.$http
                .get("/api/InspectionSchedule/AccreditationRoleByUserId?userId=" + userId + "&uniqueId=" + uniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<IAccreditationRole>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getInspectionScheduleDetailForCompliance(appId: string): ng.IPromise<IInspectionScheduleDetail[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/InspectionSchedule/Compliance?appId=" + appId)
                .success((response: ng.IHttpPromiseCallbackArg<IInspectionScheduleDetail[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getSchedulesForCompliance(compAppId: string): ng.IPromise<IInspectionSchedule[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/InspectionSchedule/GetAllForCompApp?id=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<IInspectionSchedule[]>): void => {
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
        config: IConfig): IInspectionScheduleService {
        return new InspectionScheduleService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('inspectionScheduleService',
        factory);
} 
            