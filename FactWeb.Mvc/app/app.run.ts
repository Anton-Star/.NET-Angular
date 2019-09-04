interface IAppCookies {
    userId: string;
}

var my = my || {
    hasUser: false,
    genericKey: "",
    useTwoFactor: true
};

((): void => {
    'use strict';

    angular
        .module('app')
        .run(run);

    run.$inject = [
        '$window',
        '$rootScope',
        '$route',
        '$cookies',
        '$location',
        'common',
        'config',
        'accountService',
        'authorizationService',
        '$uibModalStack'
    ];
    function run(
        $window: any,
        $rootScope: ng.IRootScopeService,
        $route: ng.route.IRouteService,
        $cookies: ng.cookies.ICookiesService,
        $location: ng.ILocationService,
        common: app.common.ICommonFactory,
        config: IConfig,
        accountService: app.services.IAccountService,
        authorizationService: app.services.IAuthorizationService,
        $uibModalStack: ng.ui.bootstrap.IModalStackService): void {

        config.genericKey = my.genericKey;
        config.useTwoFactor = my.useTwoFactor;
        config.factKey = my.factKey;
        
        $rootScope.$on('$routeChangeError', (): void => {
        });

        $rootScope.$on('$routeChangeSuccess', () => {

            var url = $location.url();
            if (url.indexOf("?") > -1) {
                url = url.substring(0, url.indexOf("?"));
            }

            $window.ga('send', 'pageview', { page: url });
        });

        $rootScope.$on('$locationChangeStart', (event, toState, toParams, fromState, fromParams) => {
            var loc = toState.indexOf("#");
            var url = toState.substring(loc + 1);

            $(".root-sect").css("max-height", "100%");
            $(".root-sect").css("overflow", "auto");

            if (url === "/" || url.indexOf("RequestAccess") > -1 || url.indexOf("PasswordReminder") > -1 || url.indexOf("PasswordReset") > -1) {
                common.$broadcast(config.events.loginPageInit);
            } else {
                common.$broadcast(config.events.otherPageInit);
            }
            
            if (my.hasUser && !common.currentUser) {
                accountService.getCurrentUser()
                    .then((user) => {
                        if (user != null) {
                            common.currentUser = user;
                            
                            var credential = "";
                            if (common.currentUser.credentials.length > 0)
                                credential = common.currentUser.credentials[0].name;
                                
                            common.$broadcast(config.events.userLoggedIn, {
                                fullName: common.currentUser.firstName + " " + common.currentUser.lastName + " " + credential,
                                orgName: common.currentUser.organizations && common.currentUser.organizations.length === 1 ? common.currentUser.organizations[0].organization.organizationName : "",
                                roleName: common.currentUser.role.roleName
                            });
                        }

                        var authorized = authorizationService.isRouteAuthorized(url);

                        if (!authorized) {
                            $location.path('/').search({ page: url, x: 'u' });
                        }
                    })
                    .catch((e) => {
                        console.log(e);
                    });
            } else {
                var authorized = authorizationService.isRouteAuthorized(url);

                if (!authorized) {
                    $location.path('/').search({ page: url });
                }
            }
        });

        $rootScope.$on('$routeChangeSuccess', function () {
            $uibModalStack.dismissAll();
        });

        (<any>($rootScope)).location = $location;

        $route.reload();
    }
})();