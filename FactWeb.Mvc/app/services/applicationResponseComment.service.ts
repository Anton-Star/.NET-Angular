module app.services {
    'use strict';

    export interface IApplicationResponseCommentService {        
        get(applicationId: number, questionId: string, answerResponseStatusId: number, commentTypeId: number): ng.IPromise<Array<IApplicationResponseComment>>;
        save(applicationResponseCommentId: number, comment: string, documentId: string, answerResponseStatusId: number, applicationId: number, questionId: string, commentType: ICommentType, documents: IDocument[]): ng.IPromise<IGenericServiceResponse<IApplicationResponseComment>>;        
        delete(applicationResponseCommentId: number): ng.IPromise<IGenericServiceResponse<boolean>>;
        getCommentTypes(): ng.IPromise<Array<ICommentType>>;
        SaveApplication(application: IApplication): ng.IPromise<IServiceResponse>; // duplicate  
        notifyApplicantAboutRFI(applicantId: string): ng.IPromise<IServiceResponse>;
        markCommentVisibility(commentId: number, isVisible: boolean): ng.IPromise<IServiceResponse>;        
        markCommentInclusion(commentId: number, isIncluded: boolean): ng.IPromise<IServiceResponse>
    }

    class ApplicationResponseCommentService implements IApplicationResponseCommentService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        get(applicationId: number, questionId: string, answerResponseStatusId: number, commentTypeId: number): ng.IPromise<Array<IApplicationResponseComment>> {
            var deferred = this.$q.defer();
            
            this.$http
                .get('/api/ApplicationResponseComment/?applicationId=' + applicationId + '&questionId=' + questionId + '&answerResponseStatusId=' + answerResponseStatusId + '&commentTypeId=' + commentTypeId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationResponseComment>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(applicationResponseCommentId: number, comment: string, documentId: string, answerResponseStatusId: number, applicationId: number, questionId: string, commentType: ICommentType, documents: any[]): ng.IPromise<IGenericServiceResponse<IApplicationResponseComment>> {
            var deferred = this.$q.defer();

            var docs = [];

            _.each(documents, (d) => {
                if (d.id) {
                    docs.push({
                        documentId: d.id
                    });
                }
                
            });

            this.$http
                .post('/api/ApplicationResponseComment', JSON.stringify({
                    applicationResponseCommentId: applicationResponseCommentId,
                    comment: comment,
                    documentId: documentId,
                    answerResponseStatusId: answerResponseStatusId,
                    applicationId: applicationId,
                    questionId: questionId,
                    commentType: commentType,
                    commentDocuments: docs
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IApplicationResponseComment>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        save2(applicationResponseCommentId: number, comment: string, documentId: string, answerResponseStatusId: number, appUniqueId: string, questionId: string, commentTypeId: number): ng.IPromise<IGenericServiceResponse<IApplicationResponseComment>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/ApplicationResponseComment2', JSON.stringify({ applicationResponseCommentId: applicationResponseCommentId, comment: comment, documentId: documentId, answerResponseStatusId: answerResponseStatusId, appUniqueId: appUniqueId, questionId: questionId, commentTypeId: commentTypeId }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IApplicationResponseComment>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });
            return deferred.promise;
        }

        delete(applicationResponseCommentId: number): ng.IPromise<IGenericServiceResponse<boolean>> {
            var deferred = this.$q.defer();
            this.$http                
                .delete("/api/ApplicationResponseComment/" + applicationResponseCommentId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<boolean>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCommentTypes(): ng.IPromise<Array<ICommentType>> {
        
            var deferred = this.$q.defer();
            this.$http
                .get("/api/ApplicationResponseComment/CommentTypes/")
                .success((response: ng.IPromise<Array<ICommentType>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        SaveApplication(application: IApplication): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/SaveApplication", JSON.stringify(application))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        notifyApplicantAboutRFI(applicantId:string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http                
                .get("/api/Application/NotifyApplication/?applicantId=" + applicantId)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        markCommentVisibility(commentId: number, isVisible: boolean): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/ApplicationResponseComment/Visibility", {
                    applicationResponseCommentId: commentId,
                    visibleToApplicant: isVisible                    
                })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        markCommentInclusion(commentId: number, isIncluded: boolean): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/ApplicationResponseComment/Inclusion", {
                    applicationResponseCommentId: commentId,
                    includeInReporting: isIncluded
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
        config: IConfig): IApplicationResponseCommentService {
        return new ApplicationResponseCommentService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('applicationResponseCommentService',
        factory);
} 