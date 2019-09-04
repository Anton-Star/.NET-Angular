module app.services {
    'use strict';

    export interface IAccountService {
        login(username: string, password: string): ng.IPromise<IGenericServiceResponse<IUser>>;
        reminder(emailAddress: string, firstName: string, lastName: string): ng.IPromise<IServiceResponse>;
        updatePassword(token: string, password: string, passwordConfirm: string): ng.IPromise<IServiceResponse>;
        getCurrentUser(): ng.IPromise<IUser>;
        addFile(file, extension, obj): ng.IPromise<IGenericServiceResponse<string>>;
        update(user: IUser): ng.IPromise<IServiceResponse>;
        logout(): ng.IPromise<IServiceResponse>;
        getFailedAttempts(emailAddress: string): ng.IPromise<IGenericServiceResponse<number>>;
        setFailedAttempts(emailAddress: string, failedAttempts: number): ng.IPromise<IGenericServiceResponse<number>>;
        impersonate(user: string): ng.IPromise<IGenericServiceResponse<IUser>>;
        stopImpersonate(): ng.IPromise<IGenericServiceResponse<IUser>>;
        requestAccess(model: IRequestAccess): ng.IPromise<IServiceResponse>;
        getTwoFactor(): ng.IPromise<{}>;
        validateTwoFactor(code: string): ng.IPromise<string>;
    }



    class AccountService implements IAccountService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        login(username: string, password: string): ng.IPromise<IGenericServiceResponse<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/login', JSON.stringify({ username: username, password: password}))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IUser>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        register(firstName: string, lastName: string, emailAddress: string, password: string, passwordConfirm: string, affiliationType: string, orgName: string): ng.IPromise<IGenericServiceResponse<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/register', JSON.stringify({
                    firstName: firstName,
                    lastName: lastName,
                    emailAddress: emailAddress,
                    password: password,
                    passwordConfirm: passwordConfirm,
                    affiliationType: affiliationType,
                    orgName: orgName
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IUser>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        reminder(emailAddress: string, firstName: string, lastName: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/reminder', JSON.stringify({
                    firstName: firstName,
                    lastName: lastName,
                    emailAddress: emailAddress,
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        updatePassword(token: string, password: string, passwordConfirm: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/UpdatePassword', JSON.stringify({
                    token: token,
                    password: password,
                    passwordConfirm: passwordConfirm,
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        update(user: IUser): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/Update', JSON.stringify({
                    user: user
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCurrentUser(): ng.IPromise<IUser> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Account/CurrentUser')
                .success((response: ng.IHttpPromiseCallbackArg<IUser>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        addFile(file, extension, obj): ng.IPromise<IGenericServiceResponse<string>> {
            var deferred = this.$q.defer();
            var data = new FormData();
            data.append("file", file);

            var request: ng.IRequestConfig = {
                method: 'POST',
                url: '/api/Account/AddFile?extension=' + extension + '&fileType=' + file.type + '&obj=' + obj,
                data: data,
                headers: {
                    'Content-Type': undefined
                }
            };

            this.$http(request)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<string>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        logout(): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/Logout', null)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getFailedAttempts(emailAddress: string): ng.IPromise<IGenericServiceResponse<number>> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Account/FailedAttempts?emailAddress=" + encodeURIComponent(emailAddress))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<number>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        setFailedAttempts(emailAddress: string, failedAttempts: number): ng.IPromise<IGenericServiceResponse<number>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/FailedAttempts', JSON.stringify({
                        emailAddress: emailAddress,
                        failedAttempts: failedAttempts,
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<number>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        impersonate(user: string): ng.IPromise<IGenericServiceResponse<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/Impersonate?u=' + user, {})
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<number>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        stopImpersonate(): ng.IPromise<IGenericServiceResponse<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .delete('/api/Account/Impersonate', {})
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IUser>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        requestAccess(model: IRequestAccess): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/RequestAccess', { record: model })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getTwoFactor(): ng.IPromise<{}> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Account/TwoFactor")
                .success((response: ng.IHttpPromiseCallbackArg<{}>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        validateTwoFactor(code: string): ng.IPromise<string> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Account/TwoFactor', { code: code })
                .success((response: ng.IHttpPromiseCallbackArg<string>): void => {
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
        config: IConfig): IAccountService {
        return new AccountService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('accountService',
        factory);
} 