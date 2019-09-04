module app.services {
    'use strict';

    export interface IAnswerService {
        save(item: IAnswer): ng.IPromise<IGenericServiceResponse<string>>;
        remove(id: string): ng.IPromise<IServiceResponse>;
        removeHides(id: string): ng.IPromise<IServiceResponse>;
        addHides(answerId: string, questions: Array<IQuestion>): ng.IPromise<IGenericServiceResponse<Array<IQuestionAnswerDisplay>>>;
    }

    class AnswerService implements IAnswerService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        save(item: IAnswer): ng.IPromise<IGenericServiceResponse<string>> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Answer", {
                    id: item.id,
                    questionId: item.questionId || null,
                    text: item.text,
                    order: item.order,
                    isExpectedAnswer: item.isExpectedAnswer
                })
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
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
                .delete("/api/Answer/" + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        removeHides(id: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .delete("/api/Answer/RemoveHides/" + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        addHides(answerId: string, questions: Array<IQuestion>): ng.IPromise<IGenericServiceResponse<Array<IQuestionAnswerDisplay>>> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Answer/AddHides", {
                    answerId: answerId,
                    questions: questions
                })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<Array<IQuestionAnswerDisplay>>>): void => {
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
        config: IConfig): IAnswerService {
        return new AnswerService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('answerService',
        factory);
} 