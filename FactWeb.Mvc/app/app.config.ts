((): void => {
    'use strict';

    angular
        .module('app')
        .config(config);

    config.$inject = ['$locationProvider', '$httpProvider'];
    function config($locationProvider: ng.ILocationProvider, $httpProvider: ng.IHttpProvider): void {

        $httpProvider.interceptors.push('authInterceptorService');
        $locationProvider.html5Mode(false);

    }

    var app = angular
        .module('app')        
        .run(run)
        .factory('errorInterceptor', ['$q', '$location', 'common', '$rootScope', function ($q, $location, common, $rootScope) {
            return {
                responseError: function (response) {
                    if (response && response.status === 403) {
                        $rootScope.is403 = true;
                        var pageUrl = $location.url();
                        if ($location.search().x === "u") { //we have already handled this, so do not modify the path again
                            common.hideSplash();  //to handle cases where there were multiple async calls started & the splash persists.                            
                        }
                        else {
                            if (pageUrl === "") {
                                $location.path('/');  
                            } else {
                                $location.path('/').search({ x: 'u', page: pageUrl });      
                            }
                                                      
                        }                       
                    }
                    return $q.reject(response);
                }
            };
        }])
        .config(['$httpProvider', function ($httpProvider) {
            $httpProvider.interceptors.push('errorInterceptor');
        }])
        .filter('urlescape', function () {
            return encodeURIComponent;
        })
        .filter('percentage', ['$filter', ($filter) => {
            return (input, decimals) => {
                return $filter('number')(input * 100, decimals) + '%';
            };
        }]);

    run.$inject = [
        '$location',
        'common'   
    ];
    
    function run(
        $location: ng.ILocationService,
        common: app.common.ICommonFactory
        ): void { };
})(); 