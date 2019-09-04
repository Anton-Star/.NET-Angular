module app.services {
    'use strict';

    export interface IQuestionTypeService {
        getAll(): ng.IPromise<Array<IQuestionType>>;
    }

    class QuestionTypeService implements IQuestionTypeService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<IQuestionType>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/QuestionType')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IQuestionType>>): void=> {
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
        config: IConfig): IQuestionTypeService {
        return new QuestionTypeService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('questionTypeService',
        factory);
} 