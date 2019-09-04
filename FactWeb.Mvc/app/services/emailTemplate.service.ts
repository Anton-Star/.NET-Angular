module app.services {
    'use strict';

    export interface IEmailTemplateService {
        getAll(): ng.IPromise<Array<IEmailTemplate>>;
    }

    class EmailTemplateService implements IEmailTemplateService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<IEmailTemplate>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/EmailTemplate")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IEmailTemplate>>): void => {
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
        config: IConfig): IEmailTemplateService {
        return new EmailTemplateService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('emailTemplateService',
        factory);
} 