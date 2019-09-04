module app.services {
    'use strict';

    export interface IEmailService {
        send(to: string, cc: string, subject: string, html: string, includeAccReport: boolean, cycleNumber: number, orgName: string, appId: string): ng.IPromise<{}>;
    }

    class EmailService implements IEmailService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        send(to: string, cc: string, subject: string, html: string, includeAccReport: boolean, cycleNumber: number, orgName: string, appId: string): ng.IPromise<{}> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Email", {
                    to: to,
                    cc: cc,
                    subject: subject,
                    html: html,
                    includeAccReport: includeAccReport,
                    cycleNumber: cycleNumber,
                    orgName: orgName,
                    appId: appId
                })
                .success((response: ng.IHttpPromiseCallbackArg<{}>): void => {
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
        config: IConfig): IEmailService {
        return new EmailService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('emailService',
        factory);
} 