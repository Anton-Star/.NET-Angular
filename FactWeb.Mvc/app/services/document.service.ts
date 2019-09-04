module app.services {
    'use strict';

    //maintain a local dictionary of access token objects.
    interface IAccessTokenDictionary {
        orgName: string;
        accessToken: services.IAccessToken;
    }

    export interface IDocumentService {
        add(orgName: string, fileName: string, originalFileName: string, vaultId: string, blobId: string, factOnly: boolean, appUniqueId?: string, replacementOfId?: string): ng.IPromise<IGenericServiceResponse<IDocument>>;
        addBAA(orgName: string, fileName, originalFileName: string, vaultId: string, blobId: string): ng.IPromise<IGenericServiceResponse<IDocument>>;
        addExcludeLibrary(orgName: string, file, fileName): ng.IPromise<IServiceResponse>;
        getByOrg(org: string): ng.IPromise<Array<IDocument>>;
        getByApp(appId: string): ng.IPromise<IDocument[]>;
        getBAAByOrg(org: string): ng.IPromise<Array<IDocument>>;
        remove(orgName: string, document: IDocument): ng.IPromise<IServiceResponse>;
        removeBAADocument(orgName: string, document: IDocument): ng.IPromise<IServiceResponse>;
        getAccessToken(orgName: string): ng.IPromise<IAccessToken>;
        getAccessTokenById(appId: string): ng.IPromise<IAccessToken>;
        getPostInspection(orgName: string): ng.IPromise<IDocument[]>;
        saveIncludeInReporting(orgName: string, documents: IDocument[]): ng.IPromise<IServiceResponse>;
    }

    class DocumentService implements IDocumentService {
        private accessTokenArray: IAccessTokenDictionary[];        

        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) {

            this.accessTokenArray = [];
        }

        getByOrg(org: string): ng.IPromise<Array<IDocument>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Document?org=' + encodeURIComponent(org))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IDocument>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getByApp(appId: string): ng.IPromise<IDocument[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Document?appId=' + appId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IDocument>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getBAAByOrg(org: string): ng.IPromise<Array<IDocument>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Document/BAA?org=' + encodeURIComponent(org))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IDocument>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        add(orgName: string, fileName: string, originalFileName: string, vaultId: string, blobId: string, factOnly: boolean, appUniqueId?: string, replacementOfId?: string): ng.IPromise<IGenericServiceResponse<IDocument>> {
            var values = {
                "vault_id":vaultId,
                "blob_id": blobId
            };

            var deferred = this.$q.defer();
            this.$http
                .post("/api/Document", {
                    organizationName: orgName,
                    name: fileName,
                    originalName: originalFileName,
                    requestValues: blobId,
                    appUniqueId: appUniqueId,
                    staffOnly: factOnly,
                    replacementOfId: replacementOfId
                })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IDocument>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        addBAA(orgName: string, fileName: string, originalFileName: string, vaultId: string, blobId: string): ng.IPromise<IGenericServiceResponse<IDocument>> {
            var values = {
                "vault_id": vaultId,
                "blob_id": blobId
            };

            var deferred = this.$q.defer();
            this.$http
                .post("/api/Document/AddBAA", {
                    organizationName: orgName,
                    name: fileName,
                    originalName: originalFileName,
                    requestValues: blobId
                })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IDocument>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        addExcludeLibrary(orgName: string, file, fileName): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            var data = new FormData();
            data.append("file", file);

            var request: ng.IRequestConfig = {
                method: 'POST',
                url: '/api/Document/Exclude?orgName=' + encodeURIComponent(orgName) + '&fileName=' + fileName,
                data: data,
                headers: {
                    'Content-Type': undefined
                }
            };

            this.$http(request)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        remove(orgName: string, document: IDocument): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Document?orgName=" + encodeURIComponent(orgName) + '&id=' + document.id, {})
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        removeBAADocument(orgName: string, document: IDocument): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Document/BAA?orgName=" + encodeURIComponent(orgName) + '&id=' + document.id, {})
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAccessToken(orgName: string): ng.IPromise<IAccessToken> {
            var deferred = this.$q.defer();
            
            this.$http
                .get('/api/Document/Access?name=' + encodeURIComponent(orgName))
                .success((response: any): void => {
                    
                    //this.accessTokenArray.push({
                    //    "orgName": orgName,
                    //    "accessToken": response
                    //});
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAccessTokenById(appId: string): ng.IPromise<IAccessToken> {
            var deferred = this.$q.defer();

            this.$http
                .get('/api/Document/Access?appId=' + appId)
                .success((response: any): void => {

                    //this.accessTokenArray.push({
                    //    "orgName": orgName,
                    //    "accessToken": response
                    //});
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getPostInspection(orgName: string): ng.IPromise<IDocument[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Document/Post?org=' + encodeURIComponent(orgName))
                .success((response: ng.IHttpPromiseCallbackArg<IDocument[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveIncludeInReporting(orgName: string, documents: IDocument[]): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Document/IncludeInReporting",
                {
                    orgName: orgName,
                    documents: documents
                })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
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
        config: IConfig): IDocumentService {
        return new DocumentService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('documentService',
        factory);
} 