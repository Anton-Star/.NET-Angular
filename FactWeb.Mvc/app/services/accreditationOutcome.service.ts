module app.services {
    'use strict';

    export interface IAccreditationOutcomeService {
        getAccreditationOutcome(organizationId: number): ng.IPromise<Array<IAccreditationOutcome>>;
        getAccreditationOutcomeByOrgAndApp(organizationId: number, applicationId: number): ng.IPromise<Array<IAccreditationOutcome>>;
        getAccreditationOutcomeByApp(applicationUniqueId: string): ng.IPromise<IGenericServiceResponse<IAccreditationOutcome>>;
        save(organizationId: number, applicationId: string, outcomeStatusId: number, reportReviewStatusId: number,
            committeeDate: string, useTwoYearCycle: boolean, emailContent: string, documents: Array<IDocument>, to: string, cc: string, subject: string,
            includeAccreditationReport: boolean, dueDate: string): ng.IPromise<IGenericServiceResponse<boolean>>;
        remove(id: number): ng.IPromise<IServiceResponse>;
        getAll(): ng.IPromise<Array<IAccreditationOutcome>>;
        getAccrediationEmailItems(outcomeLevel: string, organizationId: number, appUniqueId: string): ng.IPromise<IGenericServiceResponse<IEmailTemplate>>;
    }

    class AccreditationOutcomeService implements IAccreditationOutcomeService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAccreditationOutcome(organizationId: number): ng.IPromise<Array<IAccreditationOutcome>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/AccreditationOutcome?organizationId=' + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IAccreditationOutcome>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAccreditationOutcomeByOrgAndApp(organizationId: number, applicationId: number): ng.IPromise<Array<IAccreditationOutcome>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/AccreditationOutcome?organizationId=' + organizationId + '&applicationId=' + applicationId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IAccreditationOutcome>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        getAccrediationEmailItems(outcomeLevel: string, organizationId: number, appUniqueId: string): ng.IPromise<IGenericServiceResponse<IEmailTemplate>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/AccreditationOutcome/GetAccrediationEmailItems?outcomeLevel=' + outcomeLevel + '&organizationId=' + organizationId+ '&appUniqueId=' + appUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IEmailTemplate>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        getAccreditationOutcomeByApp(applicationUniqueId: string): ng.IPromise<IGenericServiceResponse<IAccreditationOutcome>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/AccreditationOutcome?applicationUniqueId=' + applicationUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IAccreditationOutcome>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAll(): ng.IPromise<Array<IAccreditationOutcome>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/AccreditationOutcome')
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IAccreditationOutcome>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(organizationId: number, applicationId: string, outcomeStatusId: number, reportReviewStatusId: number, committeeDate: string,
            useTwoYearCycle: boolean, emailContent: string, documents: Array<IDocument>, to: string, cc: string, subject: string,
            includeAccreditationReport: boolean, dueDate: string): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/AccreditationOutcome', JSON.stringify({
                    organizationId: organizationId,
                    applicationId: applicationId,
                    outcomeStatusId: outcomeStatusId,
                    reportReviewStatusId: reportReviewStatusId,
                    committeeDate: committeeDate,
                    dueDate: dueDate,
                    useTwoYearCycle: useTwoYearCycle,
                    emailContent: emailContent,
                    attachedDocuments: documents,
                    to: to,
                    cc: cc,
                    subject: subject,
                    includeAccreditationReport: includeAccreditationReport
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        remove(id: number): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .delete("/api/AccreditationOutcome/" + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
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
        config: IConfig): IAccreditationOutcomeService {
        return new AccreditationOutcomeService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('accreditationOutcomeService',
        factory);
} 