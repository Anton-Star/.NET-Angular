module app.services {
    'use strict';

    export interface IUserService {
        getByToken(token: string): ng.IPromise<IUser>;
        getAllUsers(includeAll: boolean): ng.IPromise<Array<IUser>>;
        getAuditorObservers(): ng.IPromise<IUser[]>;
        getAllUsersWithOrganization(): ng.IPromise<Array<IUser>>;
        getByOrganization(organizationId: number): ng.IPromise<Array<IUser>>;
        getUsersNearSite(siteList:string): ng.IPromise<Array<IUser>>;   
        getAccreditationRoles(): ng.IPromise<Array<IAccreditationRole>>;        
        save(user: IUser, password: string, confirmPassword: string, isNewUser: boolean, addToExisting: boolean): ng.IPromise<IServiceResponse>;        
        getFactStaff(): ng.IPromise<Array<IUser>>;     
        setAuditorObserver(id: string, isAuditor: boolean, isObserver: boolean): ng.IPromise<IServiceResponse>; 
        getUsersForImpersonation(): ng.IPromise<Array<IUser>>;
        checkEditPermissions(): ng.IPromise<boolean>;
        getConsultants(): ng.IPromise<IUser[]>;
    }

    class UserService implements IUserService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        getByToken(token: string): ng.IPromise<IUser> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/User/Token/' + token)
                .success((response: ng.IHttpPromiseCallbackArg<IUser>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAllUsers(includeAll: boolean): ng.IPromise<Array<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User?includeAll=" + (includeAll ? "Y" : "N"))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IUser>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAuditorObservers(): ng.IPromise<IUser[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/AuditorObserver")
                .success((response: ng.IHttpPromiseCallbackArg<IUser[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAllUsersWithOrganization(): ng.IPromise<Array<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/GetAllUsersWithOrganization")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IUser>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        getByOrganization(organizationId: number): ng.IPromise<Array<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/" + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IUser>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getFactStaff(): ng.IPromise<Array<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/FactStaff")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IUser>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        getUsersNearSite(siteList: string): ng.IPromise<Array<IUser>>{
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/Site/" + siteList)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IUser>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAccreditationRoles(): ng.IPromise<Array<IAccreditationRole>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/AccreditationRoles")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IAccreditationRole>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(user: IUser, password: string, confirmPassword: string, isNewUser: boolean, addToExisting: boolean): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/User/Save", JSON.stringify({
                    user: user,
                    password: password,
                    confirmPassword: confirmPassword,
                    isNewUser: isNewUser,
                    addToExistingUser: addToExisting
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        setAuditorObserver(id: string, isAuditor: boolean, isObserver: boolean): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                    "/api/User/AuditorObserver", JSON.stringify({
                        userId: id,
                        isAuditor: isAuditor,
                        isObserver: isObserver
                    }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getUsersForImpersonation(): ng.IPromise<Array<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/Impersonation")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IAccreditationRole>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        checkEditPermissions(): ng.IPromise<boolean> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/EditPermissions")
                .success((response: ng.IHttpPromiseCallbackArg<boolean>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getConsultants(): ng.IPromise<IUser[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/User/Consultant")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IUser>>): void => {
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
        config: IConfig): IUserService {
        return new UserService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('userService',
        factory);
} 