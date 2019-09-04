//module app.admin {
//    'use strict';

//    interface IApplicationResponseComments {
//        file: string;
//        fileName: string;
//        applicationResponseCommentId: number;
//        comment: string;
//        documentId: string;        
//        saveComment(): void;
//        deleteComment: (applicationResponseCommentId) => void;
//        editComment: (rowData) => void;
//        saveMode: boolean;
//    }

//    class ApplicationResponseCommentsController implements IApplicationResponseComments {
//        file: any;
//        fileName = "";
//        applicationResponseCommentId: number = 0;
//        comment: string;
//        documentId: string;       
//        results: Array<app.services.IApplicationResponseComment>;
//        saveMode: boolean;     
//        commentsResult: app.services.ICommentsResult = {totalComments:0,commentEntered:false};   
//        gridOptions = {
//            sortable: true,
//            filterable: {
//                operators: {
//                    string: {
//                        contains: "Contains"
//                    }
//                }
//            },
//            selectable: "row",
//            dataSource: new kendo.data.DataSource({
//                data: [],
//                pageSize: 10
//            }),
//            pageable: {
//                pageSize: 10
//            }
//        };

//        static $inject = [
//            '$window',
//            'documentService',
//            'trueVaultService',
//            'applicationResponseCommentService',
//            'notificationFactory',
//            'common',
//            'config',
//            '$uibModalInstance',
//            'question',
//            'application',      
//            'accessToken'      
//        ];
//        constructor(
//            private $window: ng.IWindowService,
//            private documentService: services.IDocumentService,
//            private trueVaultService: services.ITrueVaultService,
//            private applicationResponseCommentService: app.services.IApplicationResponseCommentService,
//            private notificationFactory: app.blocks.INotificationFactory,
//            private common: app.common.ICommonFactory,
//            private config: IConfig,
//            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
//            private question: services.IQuestion,
//            private application: services.IApplication,
//            private accessToken: services.IAccessToken) {
//            this.saveMode = false;
//            this.common.$broadcast(this.config.events.pageNameSet, {
//                pageName: "Scope Type",
//                breadcrumbs: [
//                    { url: '#/', name: 'Home', isActive: false },
//                    { url: '', name: 'Admin', isActive: true },
//                    { url: '', name: 'Scope Type', isActive: true }
//                ]
//            });

//            common.activateController([this.getComments()], 'ApplicationResponseCommentsController');
//        }


//        getComments(): ng.IPromise<void> {
//            return this.applicationResponseCommentService.get(this.application.applicationId, this.question.id, this.question.answerResponseStatusId, parseInt(this.question.commentType.id))
//                .then((data: Array<app.services.IApplicationResponseComment>) => {                    
//                    if (data != null) {
//                        this.results = data;
//                        this.gridOptions.dataSource.data(data);
//                    }
//                })
//                .catch(() => {
//                    this.notificationFactory.error("Error.");
//                });
//        }

//        saveComment(): void {            
//            if (this.file && this.file != "" && this.fileName && this.fileName != "") {
//                this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.accessToken.vaultId, this.file, this.fileName)
//                    .then((data: services.ITrueVaultBlobResponse) => {
//                        if (data.result !== "success") {
//                            this.notificationFactory.error("Error trying to save document. Please contact support.");
//                            this.common.hideSplash();
//                        } else {
//                            this.documentService.add(this.application.organizationName, this.fileName, this.common.currentUser.documentLibraryAccessToken, data.blob_id, false, this.application.uniqueId)
//                                .then((data: services.IGenericServiceResponse<services.IDocument>) => {
//                                    if (data.hasError) {
//                                        this.notificationFactory.error(data.message);
//                                    } else {
//                                        this.documentId = data.item.id;
//                                        this.save();
//                                    }
//                                })
//                                .catch(() => {
//                                    this.notificationFactory.error("Error trying to save document. Please contact support.");
//                                });
//                        }
//                    })
//                    .catch((e) => {
//                        if (e.indexOf("404.13") != -1) {
//                            this.notificationFactory.error("Error trying to save document. Maximum allowed upload file size is 30MB.");
//                        }
//                        else {
//                            this.notificationFactory.error("Error trying to save to True Vault. Please contact support.");
//                        }

//                        this.common.hideSplash();
//                    });
//            }
//            else
//            {
//                this.save();
//            }            
//        }

//        save(): void {
            
//            if (this.applicationResponseCommentId == 0 && !this.validateAdd())
//            {
//                this.notificationFactory.warning("You already added a comment.");                
//                return;
//            }
            
//            this.common.showSplash();
//            this.applicationResponseCommentService.save(this.applicationResponseCommentId, this.comment, this.documentId, this.question.answerResponseStatusId, this.application.applicationId, this.question.id, this.question.commentType)
//                .then((data: app.services.IGenericServiceResponse<services.IApplicationResponseComment>) => {
//                    if (data.hasError) {
//                        this.notificationFactory.error(data.message);
//                        this.common.hideSplash();
//                    } else {
                        
//                        if (data.item != null) {
//                            this.saveMode = false;
//                            this.commentsResult.commentEntered = true;
//                            if (this.applicationResponseCommentId == 0) {
//                                this.results.push(data.item);
//                                this.gridOptions.dataSource.data(this.results);
//                                if (data.message != '' && data.message != null) {
//                                    this.notificationFactory.success(data.message);
//                                }
//                            }
//                            else {
//                                this.getComments();
//                                this.notificationFactory.success("Comment saved");
//                            }
//                            this.clearForm();

//                        } else {
//                            this.notificationFactory.error("Error.");
//                        }
//                    }
//                    this.common.hideSplash();
//                })
//                .catch(() => {
//                    this.notificationFactory.error("Error.");
//                    this.common.hideSplash();
//                });

//        }
      
//        deleteComment(rowData: app.services.IApplicationResponseComment): void {
//            var confirmation = confirm("Are you sure you want to delete this comment ?");
//            if (confirmation) {
                
//                this.common.showSplash();
//                this.applicationResponseCommentService.delete(rowData.applicationResponseCommentId)
//                    .then((data: app.services.IGenericServiceResponse<boolean>) => {
//                        this.common.$q.all([this.getComments()]).then(() => {
//                            this.notificationFactory.success("Comment deleted successfully.");
//                            this.common.hideSplash();
//                        });
//                    })
//                    .catch(() => {
//                        this.notificationFactory.error("Error.");
//                        this.common.hideSplash();
//                    });
//            }
//        }

//        validateAdd(): boolean {     
            
//            // Last user entered the comment should not be same as current loggedin user
//            if (this.results.length > 0) {
//                if (this.common.currentUser.emailAddress == this.results[0].updatedBy)  // same user cannot enter comment consecutively 
//                    return false;
//            }
//            return true;      
//        }

//        validateEditDelete(rowData: app.services.IApplicationResponseComment): boolean {
            
//            // check only creater of the comment can edit or delete the comment.
//            if (rowData.createdBy != this.common.currentUser.emailAddress) {
//                this.notificationFactory.warning("Comment can only be updated or deleted by created.");
//                return false;
//            }            
//            return true;
//        }

//        editComment(rowData: app.services.IApplicationResponseComment): void {
         
//            if (this.validateEditDelete(rowData)) {
//                this.applicationResponseCommentId = rowData.applicationResponseCommentId;
//                this.comment = rowData.comment;
//                this.documentId = rowData.documentId;
//                this.saveMode = true;
//            }                
//        }
        
//        download(rowData: app.services.IApplicationResponseComment): void {
//            window.open(this.config.trueVaultPath +
//                "download?access_key=" +
//                this.common.currentUser.documentLibraryAccessToken +
//                "&request=" +
//                rowData.document.requestValues);
//        }

//        showDownload(rowData: app.services.IApplicationResponseComment): boolean {
//            return (rowData.documentId != "" && rowData.documentId != null )
//        }

//        cancel(): void {
//            this.clearForm();
//            this.saveMode = false;
//        }

//        clearForm(): void {
//            this.applicationResponseCommentId = 0;
//            this.comment = "";
//            this.documentId = "";    
//            this.fileName = "";
//            this.file = null;

//        }

//        closeModal(): void {
//            this.commentsResult.totalComments = this.results.length;
//            this.$uibModalInstance.close(this.commentsResult);            
//        }

//    }

//    angular
//        .module('app.admin')
//        .controller('app.modal.templates.ApplicationResponseCommentsController',
//        ApplicationResponseCommentsController);
//}