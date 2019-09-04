module app.modal.templates {
    'use strict';

    interface IDocumentLibrary {
        file: string;
        fileName: string;
        isBusy: boolean;
        selected: Array<services.IDocument>;
        rowSelected: (event) => void;
        //add: () => void;
        ok: () => void;
        cancel: () => void;
    }

    class DocumentLibraryController implements IDocumentLibrary {
        file: any;
        files = []; //storing whole files
        //fileNames = [];//storing files name
        factOnly = false;
        //extensions = [];//storing files extensions
        fileName = "";
        isBusy = false;
        selected: Array<services.IDocument>;
        gridOptions: any = {};
        allSelected: Array<services.IDocument>;
        tempSelected: Array<services.IDocument> = [];
        extension = "";
        allFiles = [];
        selectedReplacement: any;
        docs: services.IDocument[];

        static $inject = [
            '$q',
            '$scope',
            'documentService',
            'trueVaultService',
            'appLogService',
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'allowMultiple',
            'accessToken',
            'isReadOnly',
            'organization',
            'appUniqueId'
        ];

        constructor(
            private $q: ng.IQService,
            private $scope: ng.IScope,
            private documentService: services.IDocumentService,
            private trueVaultService: services.ITrueVaultService,
            private appLogService: services.IAppLogService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private allowMultiple: boolean,
            private accessToken: services.IAccessToken,
            private isReadOnly?: boolean,
            private organization?: string,
            private appUniqueId?: string) {
            if (this.allowMultiple) {
                this.gridOptions = {
                    sortable: true,
                    filterable: {
                        operators: {
                            string: {
                                contains: "Contains"
                            }
                        }
                    },
                    change: (e) => {
                        return this.rowSelected(e);
                    },
                    selectable: "multiple",
                    dataSource: new kendo.data.DataSource({ data: [], pageSize: 10 }),
                    pageable: {
                        pageSize: 10
                    },
                    columns: [
                        { field: "id", title: "Document Name", hidden: "true" },
                        { field: "name", title: "Document Name" },
                        { field: "originalName", title: "Original File Name" },
                        { field: "createdDate", title: "Created Date" },
                        { field: "isLatestVersion", title: "Latest Version", template: "#= isLatestVersionString #" }
                    ]
                };
            } else {
                this.gridOptions = {
                    sortable: true,
                    filterable: {
                        operators: {
                            string: {
                                contains: "Contains"
                            }
                        }
                    },
                    change: (e) => {
                        return this.rowSelected(e);
                    },
                    selectable: "row",
                    dataSource: new kendo.data.DataSource({ data: [], pageSize: 10 }),
                    pageable: {
                        pageSize: 10
                    },
                    columns: [
                        { field: "id", title: "Document Name", hidden: "true" },
                        { field: "name", title: "Document Name" },
                        { field: "originalName", title: "Original File Name" },
                        { field: "createdDate", title: "Created Date" },
                        { field: "isLatestVersion", title: "Latest Version", template: "#= isLatestVersionString#" }
                    ]
                };
            }

            this.isReadOnly = this.isReadOnly || false;

            if (this.common.documents) {
                _.each(this.common.documents, (d: services.IDocument) => {
                    d.isLatestVersionString = d.isLatestVersion ? "X" : "";
                });

                this.docs = _.orderBy(this.common.documents, 'name');

                console.log('docs', this.docs);

                this.gridOptions.dataSource.data(this.common.documents);    
            }
            

            $scope.$watch("vm.files", () => {
                //this.fileNames = [];//needed to be empty when values are changed otherwise new values are appended after old values
                //this.extensions = [];//same above case
                this.allFiles = [];
                for (var file = 0; file < this.files.length; file++) {
                    this.allFiles.push({
                        fileName: this.files[file].name.replace(/\.[^/.]+$/, ""),
                        file: this.files[file],
                        extension: this.files[file].name.substring(this.files[file].name.lastIndexOf(".")),
                        replacementOf: ""
                    });
                    //this.fileNames[file] = this.files[file].name.replace(/\.[^/.]+$/, "");
                    //this.extensions[file] = this.files[file].name.substring(this.files[file].name.indexOf("."));
                }
            });
        }

        getLatestText(dataItem) {
            return dataItem.isLatestVersion ? 'X' : '';
        }

        onSelectedNewVersion(e) {
            console.log(e, this.selectedReplacement);
        }

        rowSelected(event): void {
            var grid = event.sender;

            if (this.allowMultiple) {
                this.allSelected = [];

                var rows = grid.select();
                _.each(rows, (r) => {
                    var selectedItem = grid.dataItem(r);
                    this.allSelected.push(angular.copy(selectedItem));
                });

            } else {
                this.selected = [];
                var selectedItem = grid.dataItem(grid.select());
                this.selected.push(angular.copy(selectedItem));
            }

            try {
                this.$scope.$apply();
            } catch (e) {
                
            }
            
        }

        addMulitpleFiles() {

            var hasDocNames = [];

            for (var k = 0; k < this.allFiles.length; k++) {
                var fileName = this.allFiles[k].fileName;

                var f = _.find(this.common.documents, (d: services.IDocument) => {
                    var docName = "";
                    if (d.name.substring(d.name.length - 4).indexOf('.') > -1) {
                        docName = d.name.substring(0, d.name.length - 4);
                    } else {
                        docName = d.name.substring(0, d.name.length - 5);
                    }

                    return docName.toLowerCase() === fileName.toLowerCase() || d.name.toLowerCase() == fileName.toLowerCase();
                });

                if (f) {
                    hasDocNames.push(fileName);
                }
            }

            if (hasDocNames.length > 0) {
                var err = "Error: Your document name is not unique.  Please rename the following:  ";

                _.each(hasDocNames, (d) => {
                    err += d + ',';
                });

                err = err.substr(0, err.length - 1);

                this.notificationFactory.error(err);
                return;
            }

            this.common.showSplash();
            let my = this;

            var org = "";
            if (this.organization) {
                org = this.organization;
            } else {
                org = this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : ""
            }

            for (var j = 0; j < this.allFiles.length; j++) {
                var name = this.allFiles[j].file._file.name;
                var ext = name.substring(name.lastIndexOf("."));

                if (this.allFiles[j].fileName.indexOf(ext) === -1)
                {
                    this.allFiles[j].fileName += ext;     
                }
            }
          
            for (var i = 0; i < this.allFiles.length; i++) {
                var fileName = this.allFiles[i].fileName;

                this.appLogService.add("Uploading Document. vault: " + this.accessToken.vaultId +
                    "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                    "; originalFileName: " + this.files[i].name);

                this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.accessToken.vaultId, this.allFiles[i].file._file,
                        fileName, this.allFiles[i].file._file.name, this.allFiles[i].replacementOf)
                    .then((data: services.ITrueValueResponse) => {
                        if (data.response.result !== "success") {
                            this.appLogService.add("Error Uploading Document. vault: " + this.accessToken.vaultId +
                                "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                                "; originalFileName: " + data.response.blob_filename +
                                "; Error: " + data.response.result);

                            this.notificationFactory.error("Error trying to save document. Please contact support.");
                            this.common.hideSplash();
                        } else {
                            this.documentService.add(this.organization, data.fileName, data.originalFileName, this.common.currentUser.documentLibraryAccessToken,
                                data.response.blob_id, this.factOnly, this.appUniqueId, data.replacementOf)
                                .then((dta: services.IGenericServiceResponse<services.IDocument>) => {
                                    if (dta.hasError) {
                                        this.notificationFactory.error(dta.message);
                                    } else {
                                        this.common.documents = this.common.documents || [];
                                        dta.item.isLatestVersionString = dta.item.isLatestVersion ? 'X' : '';

                                        if (data.replacementOf && data.replacementOf !== "") {
                                            var rec = _.find(this.common.documents, (d: services.IDocument) => {
                                                return d.id === data.replacementOf;
                                            });

                                            if (rec) {
                                                rec.isLatestVersion = false;
                                                rec.isLatestVersionString = "";
                                            }
                                        }

                                        this.common.documents.unshift(dta.item);
                                        console.log('docs', this.common.documents);
                                        this.gridOptions.dataSource.data(this.common.documents);
                                        this.docs = _.orderBy(this.common.documents, 'name');
                                        this.notificationFactory.success("Document saved successfully.");
                                        this.common.hideSplash();

                                        this.files = [];
                                        this.allFiles = [];
                                        this.file = null;

                                        var grid = $('#library').data('kendoGrid');
                                        var gridData = grid.dataSource.data();

                                        var rowsToSelect = "";
                                        this.tempSelected.push(dta.item);
                                        var temp = this.tempSelected;
                                        gridData.forEach(function(entry: any) {

                                            var found = _.find(temp,
                                                (doc: services.IDocument) => {
                                                    return entry.id === doc.id;
                                                });

                                            if (found) {
                                                rowsToSelect = rowsToSelect + 'tr[data-uid="' + entry.uid + '"]' + ","
                                            }
                                        });

                                        rowsToSelect = rowsToSelect.replace(/,\s*$/, "");
                                        grid.select(rowsToSelect);
                                    }
                                })
                                .catch(() => {
                                    this.notificationFactory.error("Error trying to save document. Please contact support.");
                                });
                        }
                    })
                    .catch(function (e) {

                        my.appLogService.add("Error Downloading Document. vault: " +
                            my.accessToken.vaultId +
                            "; AccessToken: " + my.common.currentUser.documentLibraryAccessToken +
                            "; Error:" + e +
                            "; Error Code: " + arguments[1]);

                        if (typeof (e) === 'string' && e.indexOf("404.13") != -1) {
                            my.notificationFactory.error("Error trying to save document. Maximum allowed upload file size is 30MB.");
                        }
                        else {
                            my.notificationFactory.error("Error trying to save to True Vault. Please contact support.");
                        }

                        my.common.hideSplash();
                    });
            }
        }


        //addMultipleFiles(): ng.IPromise<services.IGenericServiceResponse<services.IDocument>> {
        //    var deferred = this.$q.defer();
            
        //    this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.accessToken.vaultId, this.files[i]._file, this.fileNames[i])
        //        //.success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IDocument>>): void => {
        //        //    deferred.resolve(response);
        //        //})
        //        //.error((e) => {
        //        //    deferred.reject(e);
        //        //});

        //    return deferred.promise;
        //}

        //add(): void {

        //    this.common.showSplash();

        //    var org = "";
        //    if (this.organization) {
        //        org = this.organization;
        //    } else {
        //        org = this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : ""
        //    }

        //    if (this.fileName.indexOf(".") === -1) {
        //        this.fileName += this.extension;
        //    }

        //    this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.accessToken.vaultId, this.file, this.fileName)
        //        .then((data: services.ITrueVaultBlobResponse) => {
        //            if (data.result !== "success") {
        //                this.notificationFactory.error("Error trying to save document. Please contact support.");
        //                this.common.hideSplash();
        //            } else {
        //                this.documentService.add(this.organization, this.fileName, this.common.currentUser.documentLibraryAccessToken, data.blob_id)
        //                    .then((data: services.IGenericServiceResponse<services.IDocument>) => {
        //                        if (data.hasError) {
        //                            this.notificationFactory.error(data.message);
        //                        } else {
        //                            this.common.$q.all([this.getDocuments()]).then(() => {
        //                                this.notificationFactory.success("Document saved successfully.");
        //                                this.common.hideSplash();

        //                                var grid = $('#library').data('kendoGrid');
        //                                var gridData = grid.dataSource.data();

        //                                var rowsToSelect = "";
        //                                this.tempSelected.push(data.item);
        //                                var temp = this.tempSelected;
        //                                gridData.forEach(function (entry: any) {

        //                                    var found = _.find(temp, (doc: services.IDocument) => {
        //                                        return entry.id === doc.id;
        //                                    });

        //                                    if (found) {
        //                                        rowsToSelect = rowsToSelect + 'tr[data-uid="' + entry.uid + '"]' + ","
        //                                    }
        //                                })

        //                                rowsToSelect = rowsToSelect.replace(/,\s*$/, "");
        //                                grid.select(rowsToSelect);

        //                            });
        //                        }
        //                    })
        //                    .catch(() => {
        //                        this.notificationFactory.error("Error trying to save document. Please contact support.");
        //                    });
        //            }
        //        })
        //        .catch((e) => {
        //            if (typeof(e) === 'string' && e.indexOf("404.13") != -1) {
        //                this.notificationFactory.error("Error trying to save document. Maximum allowed upload file size is 30MB.");
        //            }
        //            else {
        //                this.notificationFactory.error("Error trying to save to True Vault. Please contact support.");
        //            }

        //            this.common.hideSplash();
        //        });
        //}

        isDisabled() {
            return (this.selected == undefined || this.selected == null || this.selected.length == 0) &&
                (this.allSelected == undefined || this.allSelected == null || this.allSelected.length == 0);
        }

        ok(): void {
            if (this.allowMultiple) {
                this.$uibModalInstance.close(this.allSelected);
            } else {
                this.$uibModalInstance.close(this.selected);
            }
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.DocumentLibraryController',
        DocumentLibraryController);
} 