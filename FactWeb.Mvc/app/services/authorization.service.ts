module app.services {
    'use strict';

    export interface IAuthorizationService {
        isRouteAuthorized(url: string): boolean;
    }

    class AuthorizationService implements IAuthorizationService {
        constructor(private routes: Array<IRoute>,
            private common: app.common.ICommonFactory) { }

        isRouteAuthorized(url: string): boolean {            
            var route = _.find(this.routes, (route: IRoute) => {                             
                //return (route.url === url.substring(1, url.lastIndexOf('?')) || route.url === url );
                return (route.url === url);
                
            });
            
            if (route && route.roles) {
                if (!this.common.currentUser) {
                    return false;
                }
                
                var found = _.find(route.roles, (role: string) => {
                    return role === this.common.currentUser.role.roleName;
                });

                return found ? true : false;
            }
            return true;
        }
    }

    factory.$inject = [
        'routes',
        'common'
    ];
    function factory(
        routes: Array<IRoute>,
        common: app.common.ICommonFactory): IAuthorizationService {
        return new AuthorizationService(routes, common);
    }

    angular
        .module('app.services')
        .factory('authorizationService',
        factory);
} 