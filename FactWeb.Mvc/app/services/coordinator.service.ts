module app.services {
    'use strict';

    export interface ICoordinatorService {        
        getCoordinatorView(id: string): ng.IPromise<ICoordinatorViewItem>;   
        getAppSections(id: string): ng.IPromise<Array<IApplicationSection>>;  
        getApplicationResponseStatus(): ng.IPromise<Array<IApplicationResponseStatusItem>>;    
        updateAnswerResponseStatus(section: Array<IApplicationSection>): ng.IPromise<IServiceResponse>;    
        getApplicationSections(type: string): ng.IPromise<Array<IApplicationSection>>;        //duplicate
        getRFIView(app: string, compAppId: string): ng.IPromise<IRfiViewItem>;//duplicate
        updateSection(section: Array<IApplicationSection>): ng.IPromise<IServiceResponse>;    
        save(compAppId: string, accreditationGoal: string, inspectionScope: string, accreditedSince: Date, overallImpressions: string, siteDescription: string, commendablePractices: string, typeDetail: string): ng.IPromise<IServiceResponse>;
        savePersonnel(orgId: number, personnel: IPersonnel[]): ng.IPromise<IServiceResponse>;
    }

    class CoordinatorService implements ICoordinatorService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }
            
        getCoordinatorView(id: string): ng.IPromise<ICoordinatorViewItem> {
            var deferred = this.$q.defer();            
            this.$http
                .get('/api/Coordinator?appliactionUniqueId=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<ICoordinatorViewItem>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAppSections(id: string): ng.IPromise<Array<IApplicationSection>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Sections?id=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationSection>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getApplicationResponseStatus(): ng.IPromise<Array<IApplicationResponseStatusItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/ResponseStatus")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationResponseStatusItem>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  
        
        updateAnswerResponseStatus(section: Array<IApplicationSection>): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/updateAnswerResponseStatus",  section)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        updateSection(section: Array<IApplicationSection>): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/updateSection", section)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplicationSections(type: string): ng.IPromise<Array<IApplicationSection>> {
            
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Sections?type=' + type)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationSection>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getRFIView(app: string, compAppId: string): ng.IPromise<IRfiViewItem> {
            var deferred = this.$q.defer();
            this.$http
                //TODO: This needs updated to allow any application type
                .get('/api/Application/RfiView?app=' + app + "&compAppId=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<IRfiViewItem>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        save(compAppId: string, accreditationGoal: string, inspectionScope: string, accreditedSince: Date, overallImpressions: string, siteDescription: string, commendablePractices: string, typeDetail: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Coordinator', {
                    complianceApplicationId: compAppId,
                    accreditationGoal: accreditationGoal,
                    inspectionScope: inspectionScope,
                    accreditedSinceDate: accreditedSince,
                    overallImpressions: overallImpressions,
                    siteDescription: siteDescription,
                    commendablePractices: commendablePractices,
                    typeDetail: typeDetail
                })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        savePersonnel(orgId: number, personnel: IPersonnel[]): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Coordinator/Personnel', {
                    orgId: orgId,
                    personnel: personnel
                })
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
        config: IConfig): ICoordinatorService {
        return new CoordinatorService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('coordinatorService',
        factory);
} 