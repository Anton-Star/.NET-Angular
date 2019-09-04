module app.services {
    'use strict';

    export interface IQuestionService {
        save(item: IQuestion): ng.IPromise<IGenericServiceResponse<services.IQuestion>>;
        remove(id: string): ng.IPromise<IServiceResponse>;
        getDisplays(questionId: string): ng.IPromise<IQuestionAnswerDisplay[]>;
        getSectionQuestions(uniqueId: string, sectionId: string): ng.IPromise<IQuestion[]>;
    }

    class QuestionService implements IQuestionService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        save(item: IQuestion): ng.IPromise<IGenericServiceResponse<services.IQuestion>> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Question", {
                    id: item.id,
                    type: item.type,
                    complianceNumber: item.complianceNumber,
                    text: item.text,
                    description: item.description,
                    order: item.order,
                    sectionId: item.sectionId || null,
                    scopeTypes: item.scopeTypes,
                    questionTypeFlags: item.questionTypeFlags || null
                })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<services.IQuestion>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        remove(id: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .delete("/api/Question/" + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getDisplays(questionId: string): ng.IPromise<IQuestionAnswerDisplay[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Question/Displays?qid=' + questionId)
                .success((response: ng.IHttpPromiseCallbackArg<IQuestionAnswerDisplay[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getSectionQuestions(uniqueId: string, sectionId: string): ng.IPromise<IQuestion[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Question/Section?uniqueId=' + uniqueId + '&sectionId=' + sectionId)
                .success((response: ng.IHttpPromiseCallbackArg<IQuestionAnswerDisplay[]>): void => {
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
        config: IConfig): IQuestionService {
        return new QuestionService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('questionService',
        factory);
} 