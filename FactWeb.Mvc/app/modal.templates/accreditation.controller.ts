module app.modal.templates {
    'use strict';

    interface IValues {
        org: services.IOrganization;
        application: services.IApplication;
        outcome: services.IOutcomeStatus;
        reportViewStatus: services.IReportReviewStatus;
    }

    class AccreditationController {
        to = "";
        cc = "";
        subject = "";
        html = "";
        documents: Array<services.IDocument> = [];
        emailTemplates: services.IEmailTemplate[];
        accessToken: services.IAccessToken;
        includeAccreditationReport = true;

        static $inject = [
            '$scope',
            '$timeout',
            '$uibModal',
            'cacheService',
            'documentService',
            'notificationFactory',
            'common',
            'config',
            'currentUser',
            '$uibModalInstance',
            'values',
            'accreditationOutcomeService',
            'trueVaultService'
        ];

        constructor(
            private $scope: ng.IScope,
            private $timeout: ng.ITimeoutService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private documentService: services.IDocumentService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private currentUser: services.IUser,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private values: IValues,
            private accreditationOutcomeService: services.IAccreditationOutcomeService,
            private trueVaultService: services.ITrueVaultService) {

            common.activateController([this.getAccessToken(), this.getAccrediationEmailItems()], '');
        }

        getTemplates(): ng.IPromise<void> {
            return this.cacheService.getEmailTemplates()
                .then((data) => {
                    this.emailTemplates = data;
                    var template = _.find(this.emailTemplates, (template) => {
                        return template.name === (this.values.outcome.name === "Level 1" ? "Accreditation Level 1" : "Accreditation Other");
                    });

                    if (template) {
                        this.html = template.html;
                    }

                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data. Please contact support.");
                });
        }

        getAccrediationEmailItems(): ng.IPromise<void> {
            return this.accreditationOutcomeService.getAccrediationEmailItems(this.values.outcome.name, this.values.org.organizationId, this.values.application.uniqueId)
                .then((data: app.services.IGenericServiceResponse<services.IEmailTemplate>) => {
                    if (data != null) {
                        this.to = data.item.to;
                        this.cc = data.item.cc;
                        this.subject = data.item.subject;
                        this.html = data.item.html;
                    }

                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data. Please contact support.");
                });
        }


        getAccessToken(): ng.IPromise<void> {
            return this.documentService.getAccessToken(this.values.org.organizationName)
                .then((data: services.IAccessToken) => {

                    this.accessToken = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to get access to document Library. Please contact support.");
                });
        }

        showDocumentLibrary(): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/documentLibrary.html?x=x",
                controller: "app.modal.templates.DocumentLibraryController",
                controllerAs: "vm",
                size: 'xl',
                backdrop: false,
                keyboard: false,
                resolve: {
                    allowMultiple: () => {
                        return true;
                    },
                    accessToken: () => {
                        return this.accessToken;
                    },
                    isReadOnly: () => {
                        return null;
                    },
                    organization: () => {
                        return this.values.org.organizationName;
                    },
                    appUniqueId: () => {
                        return undefined;
                    }
                }
            });

            instance.result.then((documents: Array<services.IDocument>) => {
                _.each(documents, (doc) => {
                    this.documents.push(doc);
                });
            }, () => {
            });
        }

        onSave(): void {
            this.$uibModalInstance.close({
                templateHtml: this.html,
                documents: this.documents,
                to: this.to,
                cc: this.cc,
                subject: this.subject,
                includeAccreditationReport: this.includeAccreditationReport
            });
        }

        onDocumentDownload(document: services.IDocument): void {
            this.trueVaultService.getBlob(this.accessToken.vaultId,
                document.requestValues,
                this.common.currentUser.documentLibraryAccessToken,
                this.config.factKey)
                .then((data: any) => {
                    var fileType = this.trueVaultService.getFileType(document.name);
                    var file = new Blob([data.response], { type: fileType });
                    saveAs(file, document.name);
                })
                .catch((e) => {
                    this.notificationFactory.error("Cannot get document from True Vault. " + e);
                });
        }

        onCancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.AccreditationController',
        AccreditationController);
} 