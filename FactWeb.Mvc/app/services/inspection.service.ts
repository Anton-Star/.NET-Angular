module app.services {
    'use strict';

    export interface IInspectionService {
        save(appUniqueId: string, siteId: number, siteDescription: string, commendablePractices: string, overallImpressions: string, isOverride?: boolean): ng.IPromise<IGenericServiceResponse<boolean>>;
        getInspection(applicationUniqueId: string): ng.IPromise<IInspection[]>;
        getInspectionBySite(applicationUniqueId: string, siteName: string): ng.IPromise<IInspection[]>;
        getCompAppInspectors(compAppId: string, includeOthers: boolean): ng.IPromise<Array<IInspectionScheduleDetail>>;
        getInspectors(appId: string): ng.IPromise<IInspectionScheduleDetail[]>;
        saveMentorFeedback(item: IInspectionScheduleDetail): ng.IPromise<IServiceResponse>;
        setReviewOutcome(compAppId: string): ng.IPromise<IServiceResponse>;
        getInspectionDetails(compAppId: string): ng.IPromise<IInspectionOverallDetail>;
        saveInspectionDetailFromCoordinator(inspectionDetail: IInspectionDetail): ng.IPromise<IServiceResponse>;
        saveDetail(detail: IInspectionOverallDetail): ng.IPromise<IServiceResponse>;
        sendMentorCompleteEmail(compAppId: string): ng.IPromise<IServiceResponse>;
    }

    class InspectionService implements IInspectionService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        save(appUniqueId: string, siteId: number, siteDescription: string, commendablePractices: string, overallImpressions: string, isOverride?: boolean): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Inspection', JSON.stringify({
                    applicationUniqueId: appUniqueId,
                    siteId: siteId,
                    siteDescription: siteDescription,
                    commendablePractices: commendablePractices,
                    overallImpressions: overallImpressions,
                    isOverride: isOverride || false
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveInspectionDetailFromCoordinator(inspectionDetail: IInspectionDetail): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Inspection/Coordinator', JSON.stringify({
                    inspectionId: inspectionDetail.inspectionId,
                    siteId: inspectionDetail.siteId,
                    siteDescription: inspectionDetail.siteDescription,
                    commendablePractices: inspectionDetail.commendablePractices,
                    overallImpressions: inspectionDetail.overallImpressions
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveDetail(detail: IInspectionOverallDetail): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Inspection/Detail', detail)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        getInspection(applicationUniqueId: string): ng.IPromise<IInspection[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Inspection?app=' + applicationUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<IInspection[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        getInspectionBySite(applicationUniqueId: string, siteName: string): ng.IPromise<IInspection[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Inspection?app=' + applicationUniqueId + "&site=" + encodeURIComponent(siteName))
                .success((response: ng.IHttpPromiseCallbackArg<IInspection[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        getCompAppInspectors(compAppId: string, includeOthers: boolean): ng.IPromise<Array<IInspectionScheduleDetail>> {
            var deferred = this.$q.defer();
            var url = '/api/Inspection/Inspectors?app=' + compAppId + "&includeOthers=" + (includeOthers ? "Y" : "N");
            this.$http
                .get(url)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IInspectionScheduleDetail>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        getInspectors(appId: string): ng.IPromise<IInspectionScheduleDetail[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Inspection/InspectorsByApp?app=' + appId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IInspectionScheduleDetail>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        saveMentorFeedback(item: IInspectionScheduleDetail): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("api/Inspection/MentorFeedback", { inspectionScheduleDetailId: item.inspectionScheduleDetailId, mentorFeedback: item.mentorFeedback })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        setReviewOutcome(compAppId: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("api/Inspection/Outcome?compAppId=" + compAppId, {})
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        getInspectionDetails(compAppId: string): ng.IPromise<IInspectionOverallDetail> {
            var deferred = this.$q.defer();
            this.$http
                .get("api/Inspection/Details?compAppId=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<IInspectionOverallDetail>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        sendMentorCompleteEmail(compAppId: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .get("api/Inspection/MentorComplete?compAppId=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<IInspectionDetail[]>): void => {
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
        config: IConfig): IInspectionService {
        return new InspectionService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('inspectionService',
        factory);
} 