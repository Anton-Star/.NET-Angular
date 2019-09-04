module app.services {
    'use strict';

    export interface IReportReviewStatusService {
        getAll(): ng.IPromise<Array<IReportReviewStatus>>;
    }

    class ReportReviewStatusService implements IReportReviewStatusService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getAll(): ng.IPromise<Array<IReportReviewStatus>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/ReportReviewStatus')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IReportReviewStatus>>): void => {
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
        config: IConfig): IReportReviewStatusService {
        return new ReportReviewStatusService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('reportReviewStatusService',
        factory);
} 