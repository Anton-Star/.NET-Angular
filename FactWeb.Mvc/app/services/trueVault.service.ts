module app.services {
    'use strict';

    export interface ITrueVaultService {
        addFile(accessToken: string, vaultId: string, file, fileName, originalFileName, replacementOf?): ng.IPromise<{}>;
        getBlob(vaultId: string, blobId: string, accessToken: string, retryVaultId: string): ng.IPromise<any>;
        getFileType(fileName: string): string;
        onDocumentDownload(document: IDocument, orgName: string, accessToken?: IAccessToken): void;
    }

    class TrueVaultService implements  ITrueVaultService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private notificationFactory: blocks.INotificationFactory,
            private accountService: services.IAccountService,
            private documentService: services.IDocumentService) {
        }

        ///Standard Document Download function. Always use this generic function
        onDocumentDownload(document: services.IDocument, orgName: string, accessToken: IAccessToken = null): void {
            if (accessToken === null) {
                this.documentService.getAccessToken(orgName)
                    .then((data: any) => {
                        var accessToken = data;
                        this.onDocumentDownloadInternal(accessToken.vaultId, document.requestValues, document.name, this.common.currentUser.documentLibraryAccessToken);
                    })
                    .catch((e) => {
                    });
            } else {
                this.onDocumentDownloadInternal(accessToken.vaultId, document.requestValues, document.name, this.common.currentUser.documentLibraryAccessToken);
            }            
        }
        onDocumentDownloadInternal(vaultId: string, blobId: string, docName: string, dlAccessToken: string) {
            this.getBlob(vaultId, blobId, dlAccessToken, "")
                .then((data: any) => {
                    var fileType = this.getFileType(docName);
                    var file = new Blob([data.response], { type: fileType });
                    saveAs(file, docName);
                })
                .catch((e) => {
                    this.notificationFactory.error("Cannot get document from True Vault. " + e);
                });
        }

        addFile(accessToken: string, vaultId: string, file, fileName, originalFileName, replacementOf?): ng.IPromise<{}> {
            var deferred = this.$q.defer();
            var data = new FormData();
            data.append("file", file);

            var request: ng.IRequestConfig = {
                method: 'POST',
                url: 'https://api.truevault.com/v1/vaults/' + vaultId + "/blobs",
                data: data,
                headers: {
                    'Content-Type': undefined,
                    'Authorization': "Basic " + accessToken
                }
            };

            this.$http(request)
                .success((response: ng.IHttpPromiseCallbackArg<{}>): void => {
                    deferred.resolve({
                        response: response,
                        fileName: fileName,
                        originalFileName: originalFileName,
                        replacementOf: replacementOf
                    });
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getBlob(vaultId: string, blobId: string, accessToken: string, retryVaultId: string): ng.IPromise<any> {
            var deferred = this.$q.defer();

            var request: ng.IRequestConfig = {
                method: 'GET',
                responseType: "blob",
                url: 'https://api.truevault.com/v1/vaults/' + vaultId + "/blobs/" + blobId,
                headers: {
                    'Authorization': "Basic " + accessToken
                }
            };

            this.$http(request)
                .success((response:any, status, headers): void => {
                    deferred.resolve({response: response, headers: headers});
                })
                .error((e) => {
                    var reader = new FileReader();
                    reader.readAsDataURL(e);
                    reader.onloadend = () => {
                        let base64data = reader.result;
                        console.log(base64data);
                    }

                    if (retryVaultId !== "") {
                        request.url = 'https://api.truevault.com/v1/vaults/' + retryVaultId + "/blobs/" + blobId;

                        this.$http(request)
                            .success((response: any, status, headers): void => {
                                deferred.resolve({ response: response, headers: headers });
                            })
                            .error((e) => {
                                this.retryBlobWithGetUser(vaultId, blobId)
                                    .then((data) => {
                                        deferred.resolve(data);
                                    })
                                    .catch((e) => {
                                        deferred.reject(e);
                                    });
                            });
                    } else {
                        this.retryBlobWithGetUser(vaultId, blobId)
                            .then((data) => {
                                deferred.resolve(data);
                            })
                            .catch((e) => {
                                deferred.reject(e);
                            });
                    }
                    
                });

            return deferred.promise;
        }

        retryBlobWithGetUser(vaultId: string, blobId: string): ng.IPromise<any> {
            var deferred = this.$q.defer();

            this.accountService.getCurrentUser()
                .then(data => {
                    this.common.currentUser = data;

                    var request: ng.IRequestConfig = {
                        method: 'GET',
                        responseType: "blob",
                        url: 'https://api.truevault.com/v1/vaults/' + vaultId + "/blobs/" + blobId,
                        headers: {
                            'Authorization': "Basic " + data.documentLibraryAccessToken
                        }
                    };

                    this.$http(request)
                        .success((response: any, status, headers): void => {
                            deferred.resolve({ response: response, headers: headers });
                        })
                        .error((e) => {
                            deferred.reject(e);
                        });
                })
                .catch((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getFileType(fileName: string): string {
            if (fileName.indexOf(".jpg") > -1 || fileName.indexOf(".jpeg") > -1) {
                return "image/jpeg";
            }
            else if (fileName.indexOf(".bm") > -1 || fileName.indexOf(".bmp") > -1) {
                return "image/bmp";
            }
            else if (fileName.indexOf(".gif") > -1) {
                return "image/gif";
            }
            else if (fileName.indexOf(".txt") > -1) {
                return "text/plain";
            }
            else if (fileName.indexOf(".doc") > -1 || fileName.indexOf(".docx") > -1) {
                return "application/msword";
            }
            else if (fileName.indexOf(".xls") > -1 || fileName.indexOf(".docx") > -1) {
                return "application/x-msexcel";
            }
            else if (fileName.indexOf(".pdf") > -1) {
                return "application/pdf";
            }
        }
    }

    function factory($http: ng.IHttpService,
        $q: ng.IQService,
        common: app.common.ICommonFactory,
        config: IConfig,
        notificationFactory: app.blocks.INotificationFactory,
        accountService: services.IAccountService,
        documentService: services.IDocumentService): ITrueVaultService {
        return new TrueVaultService($http,
            $q,
            common,
            config,
            notificationFactory,
            accountService,
            documentService);
    }

    angular
        .module('app.services')
        .factory('trueVaultService',
        factory);
}