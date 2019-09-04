module app.services {
    'use strict';
    export interface IAuthInterceptorService {
        request: Function;
    }


    class AuthInterceptorService implements IAuthInterceptorService {
        static $inject = [
            '$q',
            'common',
            '$injector',
            '$location',
            'localStorageService',
            'notificationFactory'
        ];
        constructor(
            private $q: ng.IQService,
            private common: common.ICommonFactory,
            private $injector,
            private $location: ng.ILocationService,
            private localStorageService,
            private notificationFactory: blocks.INotificationFactory) {
        }
        $http;

        request = (config): ng.IPromise<any> => {
            config.headers = config.headers || {};

            //if (this.common.currentUser && this.common.currentUser.userId) {
                var data: string = this.localStorageService.get("codes");

                if (data) {
                    var codes: ITwoFactor[] = JSON.parse(data);
                    var record: ITwoFactor;
                    if (this.common.currentUser && this.common.currentUser.userId) {
                        record = _.find(codes,
                            (c) => {
                                return c.id === this.common.currentUser.userId;
                            });
                    } else if (codes.length > 0) {
                        record = codes[0];
                    }
                    

                    if (record) {
                        config.headers.XOTP = record.code;
                    }
                }
            //}

            return config;
        }
        
        requestError: Function;
        response: Function;
    }
    factory.$inject = [
        '$q',
        'common',
        '$injector',
        '$location',
        'localStorageService',
        'notificationFactory'
    ];
    function factory(
        $q: ng.IQService,
        common: common.ICommonFactory,
        $injector,
        $location: ng.ILocationService,
        localStorageService,
        notificationFactory: blocks.INotificationFactory): IAuthInterceptorService {
        return new AuthInterceptorService($q, common, $injector, $location, localStorageService, notificationFactory);
    }

    angular
        .module('app.services')
        .factory('authInterceptorService',
        factory);
}