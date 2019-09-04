module app.admin {
    'use strict';

    interface IDocumentLibraryScope {
    }

    class DocumentLibraryController implements IDocumentLibraryScope {
        files = []; //storing whole files
        fileNames = [];//storing files name
        extensions = [];//storing files extensions
        results: Array<services.IDocument>;
        byRequirement: Array<services.ISectionDocument>;
        latest: Array<services.IDocument>;
        org = "";
        uniqueId: string;
        notAuthorizedToDelete: boolean;
        user: services.IUser;
        compAppId: string;
        isComplianceApplication: boolean;
        application: services.IApplication;
        accessToken: services.IAccessToken;
        extension = "";
        img = "";
        selectedSite = "";
        docs: services.IDocument[];

        gridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            },
            columns: [
                { headerTemplate: $("#selectAll").html(), template: $("#check").html(), width: "50px" },
                { field: "name", title: "Document Name" },
                { field: "createdDate", title: "Upload Date" },
                { field: "associationTypes", title: "Associations" },
                { field: "isLatestVersion", title: "Latest Version", template: "#= isLatestVersionString#" },
                { "template": "<button class=\"k-button\" ng-click=\"vm.onDownload(dataItem)\">Download</button><button class=\"k-button\" ng-hide=\"vm.hideDelete(dataItem)\" ng-click=\"vm.onRemove(dataItem)\">Delete</button>" }
            ]
        };
        byReqOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: false,
            groupable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10,
                group: [
                    { field: "siteName" },
                    { field: "requirement" }
                ]
            }),
            pageable: {
                pageSize: 10,
                pageSizes: [ 5, 10, 25, 50 ]
            },
            columns: [
                { headerTemplate: $("#selectAllByReq").html(), template: $("#checkByReq").html(), width: "50px" },
                { field: "siteName", groupHeaderTemplate: "#= value #", hidden: true },
                { field: "requirement", groupHeaderTemplate: "#= value #", hidden: true },
                { field: "document.name", title: "Document Name" },
                { field: "document.createdDate", title: "Upload Date" },
                { field: "document.associationTypes", title: "Associations" },
                { "template": "<button class=\"k-button\" ng-click=\"vm.onDownload(dataItem.document)\">Download</button><button class=\"k-button\" ng-hide=\"vm.hideDelete(dataItem.document)\" ng-click=\"vm.onRemove(dataItem)\">Delete</button>" }
            ]
        };
        latestOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            },
            columns: [
                { headerTemplate: $("#selectAllLatest").html(), template: $("#checkLatest").html(), width: "50px" },
                { field: "name", title: "Document Name" },
                { field: "createdDate", title: "Upload Date" },
                { field: "associationTypes", title: "Associations" },
                { "template": "<button class=\"k-button\" ng-click=\"vm.onDownload(dataItem)\">Download</button><button class=\"k-button\" ng-hide=\"vm.hideDelete(dataItem)\" ng-click=\"vm.onRemove(dataItem)\">Delete</button>" }
            ]
        };
        file: any;
        fileName = "";
        factOnly = false;
        selectAll = false;
        noChecked = true;
        selectAllByReq = false;
        noCheckedByReq = true;
        noCheckedLatest = true;
        selectAllLatest = false;
        sites: string[] = [];
        byReqData: services.ISectionDocument[];
        replacementOfs = [];

        static $inject = [
            '$q',
            '$window',
            '$rootScope',
            '$scope',
            '$location',
            'accountService',
            'trueVaultService',
            'documentService',
            'requirementService',
            'inspectionScheduleService',
            'applicationService',
            'appLogService',
            'notificationFactory',
            'common',
            'config',
            '$uibModal'
        ];
        constructor(

            private $q: ng.IQService,
            private $window: ng.IWindowService,
            private $rootScope: ng.IRootScopeService,
            private $scope: ng.IScope,
            private $location: ng.ILocationService,
            private accountService: services.IAccountService,
            private trueVaultService: services.ITrueVaultService,
            private documentService: app.services.IDocumentService,
            private requirementService: services.IRequirementService,
            private inspectionScheduleService: services.IInspectionScheduleService,
            private applicationService: services.IApplicationService,
            private appLogService: services.IAppLogService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.org = $location.search().org;
            this.uniqueId = $location.search().app;
            this.compAppId = $location.search().c;

            if (this.org === "") {
                this.org = this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "";
            }

            if (this.compAppId && this.compAppId !== "") {
                this.isComplianceApplication = true;

                this.getSectionsWithDocs(this.compAppId, true);

                this.common.checkItemValue(this.config.events.applicationLoaded, this.common.application, false)
                    .then(() => {
                        this.application = this.common.application;
                    });
            }
            else {
                this.isComplianceApplication = false;
                this.common.checkItemValue(this.config.events.applicationLoaded, this.common.application, false)
                    .then(() => {
                        this.application = this.common.application;

                        this.getSectionsWithDocs(this.common.application.uniqueId, false);
                    });
            }

            if (this.common.currentUser == null || this.common.currentUser == undefined) {
                $scope.$watch("vm.common.currentUser", (newVal: services.IUser, oldVal) => {
                    if (this.common.currentUser != null && this.common.currentUser != undefined) {
                        this.user = this.common.currentUser;
                        common.activateController([this.checkDeleteAccess(), this.getAccessToken()], '');
                    }
                });

            } else {
                this.user = this.common.currentUser;
                common.activateController([this.checkDeleteAccess(), this.getAccessToken()], '');
            }

            $scope.$watch("vm.files", () => {
                this.fileNames = [];//needed to be empty when values are changed otherwise new values are appended after old values
                this.extensions = [];//same above case
                this.replacementOfs = [];
                console.log('files', this.files);
                for (var file = 0; file < this.files.length; file++) {
                    this.fileNames[file] = this.files[file].name.replace(/\.[^/.]+$/, "");
                    this.extensions[file] = this.files[file].name.substring(this.files[file].name.lastIndexOf("."));
                    this.replacementOfs.push("");
                }
            });

            this.common.checkItemValue(this.config.events.documentsLoaded, this.common.documents, false)
                .then(() => {
                    console.log('docs');
                    var docs = this.common.documents;
                    this.latest = [];
                    _.each(docs, (doc: services.IDocument) => {
                        doc.isSelected = false;
                        doc.isLatestVersionString = "";
                        if (doc.isLatestVersion) {
                            doc.isLatestVersionString = "X";
                            this.latest.push(doc);
                        }
                    });

                    this.results = docs;
                    this.docs = _.orderBy(this.common.documents, 'name');
                    this.gridOptions.dataSource.data(docs);

                    console.log('latest', this.latest);

                    this.latestOptions.dataSource.data(this.latest);

                });


        }

        getUser(): ng.IPromise<void> {
            return this.accountService.getCurrentUser()
                .then((user: services.IUser) => {

                    this.common.currentUser = user;
                    this.user = user;
                })
                .catch(() => {
                    this.notificationFactory.error("Cannot get user. Please contact support");
                });
        }

        getAccessToken(): ng.IPromise<void> {
            return this.documentService.getAccessToken(this.org)
                .then((data: services.IAccessToken) => {
                    this.accessToken = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to get access to document Library. Please contact support.");
                });
        }

        getSectionsWithDocs(appId: string, isComp: boolean): ng.IPromise<void> {
            console.log('started reqs');
            return this.requirementService.getRequirementsWithDocuments(appId, isComp ? "Y": "N")
                .then((data: Array<services.ISectionDocument>) => {

                    console.log('reqs', data);
                    this.byRequirement = data;
                    if (data != null && data.length > 0) {
                        var i = 0;
                        var s = "";
                        _.each(data, (d) => {
                            if (d.siteName !== s) {
                                i++;
                                s = d.siteName;
                            }
                            d.siteName = i + ". " + d.siteName;
                            d.req = d.requirement.substring(0, d.requirement.indexOf(' '));
                            d.requirement = "<a  ng-click=\"vm.onAnswerReview('" + d.requirement + "','" + d.requirementId + "','" + d.document.appUniqueId + "')\">" + d.requirement + "</a>";

                            var f = _.find(this.sites, (s) => {
                                return d.siteName === s;
                            });

                            if (!f) {
                                this.sites.push(d.siteName);
                            }
                        });

                        console.log(this.sites);
                        this.byReqData = data;
                        this.byReqOptions.dataSource.data(data);
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        onSelectedSite() {
            _.each(this.byReqData, (r) => {
                r.document.isSelected = false;
            });
            this.selectAllByReq = false;
            this.noCheckedByReq = true;

            var data = this.getSiteData();
            this.byReqOptions.dataSource.data(data);
        }

        getSiteData(): services.ISectionDocument[] {
            if (this.selectedSite === '') {
                return this.byReqData;
            } else {
                var recs = _.filter(this.byReqData, (d: services.ISectionDocument) => {
                    return d.siteName === this.selectedSite;
                });

                return recs;
            }
        }

        onAnswerReview(req, requirementId: string, appUniqueId: string) {

            if (this.common.isReviewer())
            {
                var roleName = this.common.currentUser.role.roleName.toLowerCase();
                var isUser: boolean = false;
                if (roleName == "user") {
                    isUser = true;
                }
                //if it is reviewer then open following view
                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/applicationAnswersReview.html",
                    controller: "app.modal.templates.ApplicationAnswersReviewController",
                    controllerAs: "vm",
                    size: 'xxl',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        values: () => {
                            return {
                                section: null,
                                appUniqueId: appUniqueId,
                                isUser: isUser,
                                requirementId: requirementId,
                                organization: this.org
                            };
                        }
                    }
                });
            }
            else {
                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/application.html",
                    controller: "app.modal.templates.ApplicationController",
                    controllerAs: "vm",
                    size: 'xxl',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        appId: () => {
                            return appUniqueId;
                        },
                        reqId: () => {
                            return requirementId;
                        },
                        appType: () => {
                            return "";
                        },
                        accessType: () => {
                            return "";
                        },
                        organization: () => {
                            return this.org;
                        },
                        section: () => {
                            return null;
                        },
                        questions: () => {
                            return null;
                        },
                        appDueDate: () => {
                            return "";
                        },
                        appUniqueId: () => {
                            return "";
                        },
                        submittedDate: () => {
                            return "";
                        },
                        site: () => {
                            return "";
                        },
                        appStatus: () => {
                            return this.application.applicantApplicationStatusName;
                        }

                    }
                });
            }
        }

        onSelectAllByReq() {
            var recs = this.getSiteData();
            console.log(recs);
            _.each(recs, (sectionDocument: services.ISectionDocument) => {
                sectionDocument.document.isSelected = this.selectAllByReq;
            });

            this.noCheckedByReq = !this.selectAllByReq;

            this.byReqOptions.dataSource.data(recs);

        }

        onSelectByReq(document: services.IDocument) {
            if (!document.isSelected && this.selectAll) {
                this.selectAll = false;
            }

            var record = _.find(this.byRequirement, (doc: services.ISectionDocument) => {
                return doc.document.id === document.id;
            });

            if (record) {
                record.document.isSelected = document.isSelected;
            }

            var found = _.find(this.byRequirement, (doc: services.ISectionDocument) => {
                return doc.document.isSelected;
            });

            this.noCheckedByReq = found ? false : true;
        }
        
        onDownloadCheckedByReq(): void {
            var promises = [];
            let my = this;

            var zip = new JSZip();
            var folder = zip.folder(this.application.organizationName);

            _.each(this.byRequirement, (sectionDocument: services.ISectionDocument) => {
                if (sectionDocument.document.isSelected) {
                    this.appLogService.add("Downloading Document. vault: " +
                        (sectionDocument.document.isBaaDocument ? this.config.genericKey : this.accessToken.vaultId) +
                        "; RequestValues: " + sectionDocument.document.requestValues +
                        "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                        "; FactKey: " + this.config.factKey);

                    promises.push(this.trueVaultService.getBlob(sectionDocument.document.isBaaDocument ? this.config.genericKey : this.accessToken.vaultId,
                        sectionDocument.document.requestValues,
                        this.common.currentUser.documentLibraryAccessToken,
                        this.config.factKey)
                        .then((data: any) => {
                            var fileType = this.trueVaultService.getFileType(sectionDocument.document.name);
                            var file = new Blob([data.response], { type: fileType });
                            folder.file(sectionDocument.req + " - " + sectionDocument.document.name, file);
                        })
                        .catch(function (e) {
                            my.appLogService.add("Error Downloading Document. vault: " +
                                (sectionDocument.document.isBaaDocument ? my.config.genericKey : my.accessToken.vaultId) +
                                "; RequestValues: " + sectionDocument.document.requestValues +
                                "; AccessToken: " + my.common.currentUser.documentLibraryAccessToken +
                                "; FactKey: " + my.config.factKey +
                                "; Error:" + e +
                                "; Error Code: " + arguments[1]);
                        }));
                }
            });

            this.$q.all(promises).then(() => {
                zip.generateAsync({ type: "blob" })
                    .then((content) => {
                        saveAs(content, this.application.organizationName + ".zip");
                    });
            });
        }

        onSelectAll() {
            _.each(this.results, (document: services.IDocument) => {
                document.isSelected = this.selectAll;
            });

            this.noChecked = !this.selectAll;

            this.gridOptions.dataSource.data(this.results);
        }

        onSelect(document: services.IDocument): void {
            if (!document.isSelected && this.selectAll) {
                this.selectAll = false;
            }

            var record = _.find(this.results, (doc: services.IDocument) => {
                return doc.id === document.id;
            });

            if (record) {
                record.isSelected = document.isSelected;
            }

            var found = _.find(this.results, (doc: services.IDocument) => {
                return doc.isSelected;
            });

            this.noChecked = found ? false : true;
        }

        onDownloadChecked(): void {
            var promises = [];
            let my = this;

            var zip = new JSZip();
            var folder = zip.folder(this.application.organizationName);

            _.each(this.results, (document: services.IDocument) => {
                if (document.isSelected) {
                    this.appLogService.add("Downloading Document. vault: " +
                        (document.isBaaDocument ? this.config.genericKey : this.accessToken.vaultId) +
                        "; RequestValues: " + document.requestValues +
                        "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                        "; FactKey: " + this.config.factKey);

                    promises.push(this.trueVaultService
                        .getBlob(document.isBaaDocument ? this.config.genericKey : this.accessToken.vaultId,
                            document.requestValues,
                            this.common.currentUser.documentLibraryAccessToken,
                            this.config.factKey)
                        .then((data: any) => {
                            var fileType = this.trueVaultService.getFileType(document.name);
                            var file = new Blob([data.response], { type: fileType });
                            folder.file(document.name, file);
                        })
                        .catch(function (e) {
                            my.appLogService.add("Error Downloading Document. vault: " +
                                (document.isBaaDocument ? my.config.genericKey : my.accessToken.vaultId) +
                                "; RequestValues: " + document.requestValues +
                                "; AccessToken: " + my.common.currentUser.documentLibraryAccessToken +
                                "; FactKey: " + my.config.factKey +
                                "; Error:" + e +
                                "; Error Code: " + arguments[1]);
                        }));
                }
            });

            this.$q.all(promises).then(() => {
                zip.generateAsync({ type: "blob" })
                    .then((content) => {
                        saveAs(content, this.application.organizationName + ".zip");
                    });
            });
        }

        onSelectAllLatest() {
            _.each(this.latest, (document: services.IDocument) => {
                document.isSelected = this.selectAllLatest;
            });

            this.noCheckedLatest = !this.selectAllLatest;

            this.latestOptions.dataSource.data(this.latest);
        }

        onSelectLatest(document: services.IDocument): void {
            if (!document.isSelected && this.selectAllLatest) {
                this.selectAllLatest = false;
            }

            var record = _.find(this.results, (doc: services.IDocument) => {
                return doc.id === document.id;
            });

            if (record) {
                record.isSelected = document.isSelected;
            }

            var found = _.find(this.results, (doc: services.IDocument) => {
                return doc.isSelected;
            });

            this.noCheckedLatest = found ? false : true;
        }

        onDownloadCheckedLatest(): void {
            var promises = [];
            let my = this;

            var zip = new JSZip();
            var folder = zip.folder(this.application.organizationName);

            _.each(this.latest, (document: services.IDocument) => {
                if (document.isSelected) {
                    this.appLogService.add("Downloading Document. vault: " +
                        (document.isBaaDocument ? this.config.genericKey : this.accessToken.vaultId) +
                        "; RequestValues: " + document.requestValues +
                        "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                        "; FactKey: " + this.config.factKey);

                    promises.push(this.trueVaultService
                        .getBlob(document.isBaaDocument ? this.config.genericKey : this.accessToken.vaultId,
                        document.requestValues,
                        this.common.currentUser.documentLibraryAccessToken,
                        this.config.factKey)
                        .then((data: any) => {
                            var fileType = this.trueVaultService.getFileType(document.name);
                            var file = new Blob([data.response], { type: fileType });
                            folder.file(document.name, file);
                        })
                        .catch(function (e) {
                            my.appLogService.add("Error Downloading Document. vault: " +
                                (document.isBaaDocument ? my.config.genericKey : my.accessToken.vaultId) +
                                "; RequestValues: " + document.requestValues +
                                "; AccessToken: " + my.common.currentUser.documentLibraryAccessToken +
                                "; FactKey: " + my.config.factKey +
                                "; Error:" + e +
                                "; Error Code: " + arguments[1]);
                        }));
                }
            });

            this.$q.all(promises).then(() => {
                zip.generateAsync({ type: "blob" })
                    .then((content) => {
                        saveAs(content, this.application.organizationName + ".zip");
                    });
            });
        }

        onDownload(document: services.IDocument): void {
            var token = document.isBaaDocument ? this.config.genericKey : document.vaultId || this.accessToken.vaultId;
            let my = this;

            this.appLogService.add("Downloading Document. vault: " +
                (document.isBaaDocument ? this.config.genericKey : this.accessToken.vaultId) +
                "; RequestValues: " + document.requestValues +
                "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                "; FactKey: " + this.config.factKey);

            this.trueVaultService.getBlob(token, document.requestValues, this.common.currentUser.documentLibraryAccessToken, this.config.factKey)
                .then((data: any) => {
                    saveAs(data.response, document.name);
                })
                .catch(function (e) {
                    my.appLogService.add("Error Downloading Document. vault: " +
                        (document.isBaaDocument ? my.config.genericKey : my.accessToken.vaultId) +
                        "; RequestValues: " + document.requestValues +
                        "; AccessToken: " + my.common.currentUser.documentLibraryAccessToken +
                        "; FactKey: " + my.config.factKey +
                        "; Error:" + e +
                        "; Error Code: " + arguments[1]);

                    my.notificationFactory.error("Cannot get document from True Vault. " + e);
                });
        }

        onRemove(document: services.IDocument): void {
            if (document.hasResponses) {
                if (!confirm("WARNING: This document has responses asssociated with it. Are you sure you want to delete?")) {
                    return;
                }
            } else {
                if (!confirm("Are you sure you want to delete this document?")) {
                    return;
                }
            }

            this.common.showSplash();

            this.documentService.remove(this.org, document)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        for (var i = 0; i < this.results.length; i++) {
                            if (this.results[i].id === document.id) {
                                this.results.splice(i, 1);
                                break;
                            }
                        }
                        this.gridOptions.dataSource.data(this.results);

                        if (document.isLatestVersion) {
                            for (var i = 0; i < this.latest.length; i++) {
                                if (this.latest[i].id === document.id) {
                                    this.latest.splice(i, 1);
                                    break;
                                }
                            }
                            this.latestOptions.dataSource.data(this.latest);
                        }

                        this.notificationFactory.success("Document removed successfully");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to remove document.");
                    this.common.hideSplash();
                });
        }

        onReplacement(index) {
            console.log('index', index, this.replacementOfs);
        }

        addMulitpleFiles() {

            for (var j = 0; j < this.fileNames.length; j++) {
                this.fileNames[j] += this.extensions[j];
            }

            var hasDocNames = [];

            for (var k = 0; k < this.fileNames.length; k++) {
                var f = _.find(this.common.documents, (d: services.IDocument) => {
                    return d.name.toLowerCase() === this.fileNames[k].toLowerCase();
                });

                if (f) {
                    hasDocNames.push(this.fileNames[k]);
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

            for (var i = 0; i < this.fileNames.length; i++) {

                this.appLogService.add("Uploading Document. vault: " + this.accessToken.vaultId +
                    "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                    "; originalFileName: " + this.files[i].name);

                this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.accessToken.vaultId, this.files[i]._file, this.fileNames[i], this.files[i].name, this.replacementOfs[i])
                    .then((data: services.ITrueValueResponse) => {
                        if (data.response.result !== "success") {
                            this.appLogService.add("Error Uploading Document. vault: " + this.accessToken.vaultId +
                                "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                                "; originalFileName: " + data.response.blob_filename +
                                "; Error: " + data.response.result);

                            this.notificationFactory.error("Error trying to save document. Please contact support.");
                            this.common.hideSplash();
                        } else {
                            this.documentService.add(this.org, data.fileName, data.originalFileName, this.common.currentUser.documentLibraryAccessToken, data.response.blob_id, this.factOnly, "", data.replacementOf)
                                .then((r: services.IGenericServiceResponse<services.IDocument>) => {
                                        if (r.hasError) {
                                            this.notificationFactory.error(r.message);
                                        } else {
                                            r.item.isLatestVersionString = "X";
                                            this.common.documents.unshift(r.item);

                                            if (data.replacementOf !== "") {
                                                var f = _.find(this.common.documents, (d: services.IDocument) => {
                                                    return d.id === data.replacementOf;
                                                });

                                                if (f) {
                                                    f.isLatestVersion = false;
                                                    f.isLatestVersionString = "";
                                                }
                                            }

                                            this.gridOptions.dataSource.data(this.common.documents);
                                            this.docs = _.orderBy(this.common.documents, 'name');

                                            

                                            this.latest = _.filter(this.common.documents, (d: services.IDocument) => {
                                                return d.isLatestVersion;
                                            })

                                            this.latestOptions.dataSource.data(this.latest);

                                            this.notificationFactory.success("Document saved successfully.");
                                            this.common.hideSplash();
                                        }
                                    })
                                .catch(() => {
                                    this.notificationFactory.error("Error trying to save document. Please contact support.");
                                });
                        }
                    })
                    .catch((e) => {
                        this.appLogService.add("Error Uploading Document. vault: " + this.accessToken.vaultId +
                            "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                            "; Error: " + e);

                        if (typeof (e) === 'string' && e.indexOf("404.13") != -1) {
                            this.notificationFactory.error("Error trying to save document. Maximum allowed upload file size is 30MB.");
                        }
                        else {
                            this.notificationFactory.error("Error trying to save to True Vault. Please contact support.");
                        }

                        this.common.hideSplash();
                    });
            }
        }

        addFile(): void {
            this.common.showSplash();

            if (this.fileName.indexOf(".") === -1) {
                this.fileName += this.extension;
            }

            this.appLogService.add("Uploading Document. vault: " + this.accessToken.vaultId +
                "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                "; originalFileName: " + this.file.name);

            this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.accessToken.vaultId, this.file, this.fileName, this.file.name, "")
                .then((data: services.ITrueVaultBlobResponse) => {
                    if (data.result !== "success") {
                        this.appLogService.add("Error Uploading Document. vault: " + this.accessToken.vaultId +
                            "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                            "; originalFileName: " + this.file.name +
                            "; Error: " + data.result);

                        this.notificationFactory.error("Error trying to save document. Please contact support.");
                        this.common.hideSplash();
                    } else {
                        this.documentService.add(this.application.organizationName, this.fileName, this.file.name, this.common.currentUser.documentLibraryAccessToken, data.blob_id, this.factOnly)
                            .then((data: services.IGenericServiceResponse<services.IDocument>) => {
                                if (data.hasError) {
                                    this.notificationFactory.error(data.message);
                                } else {
                                    this.common.documents.push(data.item);
                                    this.gridOptions.dataSource.data(this.common.documents);
                                    this.docs = _.orderBy(this.common.documents, 'name');

                                    this.latest = _.filter(this.common.documents, (d: services.IDocument) => {
                                        return d.isLatestVersion;
                                    });

                                    this.latestOptions.dataSource.data(this.latest);

                                    this.file = "";
                                    this.fileName = "";
                                    this.notificationFactory.success("Document saved successfully.");
                                    this.common.hideSplash();
                                }
                            })
                            .catch(() => {
                                this.notificationFactory.error("Error trying to save document. Please contact support.");
                            });
                    }
                })
                .catch((e) => {
                    this.appLogService.add("Error Uploading Document. vault: " + this.accessToken.vaultId +
                        "; AccessToken: " + this.common.currentUser.documentLibraryAccessToken +
                        "; originalFileName: " + this.file.name +
                        "; Error: " + e);
                    this.notificationFactory.error("Error trying to save document. Please contact support.");
                    this.common.hideSplash();
                });
        }

        checkDeleteAccess(): ng.IPromise<void> {
            return this.inspectionScheduleService.getAccreditationRole(this.user.userId, this.uniqueId)
                .then((data: services.IGenericServiceResponse<services.IAccreditationRole>) => {
                    if (data.item != null) {
                        this.notAuthorizedToDelete = true;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting accreditation role.");
                });
        }

        hideDelete(dataItem: services.IDocument) {
            if (this.common.isInspector() && this.common.inspectorHasAccess) return true;

            return false;
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.DocumentLibraryController',
        DocumentLibraryController);
}