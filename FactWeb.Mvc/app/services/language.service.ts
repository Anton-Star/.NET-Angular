module app.services {
    'use strict';

    export interface ILanguageService {
        getAll(): ng.IPromise<Array<ILanguage>>;
    }

    class LanguageService implements ILanguageService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<ILanguage>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Language/Get')            
                .success((response: ng.IHttpPromiseCallbackArg<Array<ILanguage>>): void=> {
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
        config: IConfig): ILanguageService {
        return new LanguageService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('languageService',
        factory);
} 