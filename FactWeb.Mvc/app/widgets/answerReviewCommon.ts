//common view for both reviewer and coordinator

module app.widgets {
    'use strict';

    class AnswerReviewCommon implements ng.IDirective {

        //local variables which are not instance specific for this directives
        private isFact = false;
        private isInspector: boolean = false;
        private isUser: boolean = false;
        private currentUserId: string = "";
        private counter: number = 0;

        private statusTypes: Array<services.IApplicationResponseStatusItem>;
        private commentTypes: Array<services.ICommentType>;
        private addedComment: services.ICommentQuestion[] = []; //seems to be irrelevant. Remove this later.
        private accessToken: services.IAccessToken;

        constructor(
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $compile: ng.ICompileService,
            private $templateRequest: ng.ITemplateRequestService,
            private applicationService: services.IApplicationService,
            private applicationResponseCommentService: services.IApplicationResponseCommentService,
            private documentService: services.IDocumentService,
            private trueVaultService: services.ITrueVaultService,
            private config: IConfig,
            private common: common.ICommonFactory,
            private notificationFactory: blocks.INotificationFactory) {

            this.isUser = this.common.isUser() || !this.common.inspectorHasAccess;
            this.isFact = this.common.isFact();
            this.isInspector = this.common.inspectorHasAccess;
            this.currentUserId = this.common.currentUser.userId;
            this.counter = 0;

            //we can make response status and comment types part of the constructor load but the callback is not guaranteed to be called before scope.link
            //this.common.activateController[this.getApplicationResponseStatus(), this.getCommentTypes(), ''];
            this.statusTypes = this.common.getResponseStatusTypes();
        }

        getApplicationResponseStatus(): ng.IPromise<void> {
            return this.applicationService.getApplicationResponseStatus() //duplicate
                .then((data: Array<app.services.IApplicationResponseStatusItem>) => {
                    if (data != null) {
                        if (this.common.isInspector()) {
                            this.statusTypes = data.filter(function (statusType) {
                                return (statusType.name == 'Compliant' || statusType.name == 'Not Compliant' || statusType.name == 'N/A' || statusType.name == 'RFI');
                            });
                        }
                        else {
                            this.statusTypes = data;
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });

        }

        findAndRemove(array, id): boolean {
            var found = false;
            for (var i = 0; i < array.length; i++) {
                if (array[i].applicationResponseCommentId === id) {
                    array.splice(i, 1);
                    found = true;
                    break;
                }
            }

            return found;
        }

        static factory(): ng.IDirectiveFactory {
            var directive = (
                $location: ng.ILocationService,
                $uibModal: ng.ui.bootstrap.IModalService,
                $compile: any,
                $templateRequest,
                applicationService: services.IApplicationService,
                applicationResponseCommentService: services.IApplicationResponseCommentService,
                documentService: services.IDocumentService,
                trueVaultService: services.ITrueVaultService,
                config: IConfig,
                common: common.ICommonFactory,
                notificationFactory: blocks.INotificationFactory) =>
                new AnswerReviewCommon($location, $uibModal, $compile, $templateRequest, applicationService, applicationResponseCommentService, documentService, trueVaultService, config, common, notificationFactory);

            directive.$inject = [
                '$location',
                '$uibModal',
                '$compile',
                '$templateRequest',
                'applicationService',
                'applicationResponseCommentService',
                'documentService',
                'trueVaultService',
                'config',
                'common',
                'notificationFactory'
            ];

            return directive;
        }

        terminal = true;
        restrict = 'E';

        scope = {
            application: "=application",
            question: "=question",
            isCoordinatorView: "=iscoordinatorview",
            accessToken: "=accesstoken",
            //statusTypes: "@statustypes", //We may load these within the directive itself.
            commentTypes: "=commenttypes" //We may load these within the directive itself.
        };

        link = (scope: any, element: any, attributes: any) => {
            scope.statusTypes = this.statusTypes;
            scope.currentUserId = this.currentUserId;
            scope.isUser = this.common.isUser();
            scope.isFact = this.isFact;
            scope.isInspector = this.isInspector;
            scope.counter = ++this.counter * 100;
            scope.question.isCollapsed = true;
            scope.question.editIndex = 0;
            scope.common = this.common;

            scope.showRfiDocMessage = () => {
                if (scope.question.responseCommentsRFI && scope.question.responseCommentsRFI.length > 0) {
                    var found = _.find(scope.question.responseCommentsRFI, (r: services.IApplicationResponseComment) => {
                        return r.commentDocuments && r.commentDocuments.length > 0;
                    });

                    if (!found) return false;

                    if (scope.question.type === "Document Upload") {
                        return true;
                    }

                    if (scope.question.type === "Multiple" && scope.question.questionTypeFlags.documentUpload) {
                        return true;
                    }    
                }               

                return false;
            };

            //scope
            var _thisScope = this;
            this.$templateRequest("/app/widgets/answerReviewCommon.html").then((html) => {
                var template = angular.element(html);
                element.append(template);
                _thisScope.$compile(template)(scope);
            });

            scope.getSelectedAnswer = (answers: Array<services.IAnswer>): services.IAnswer[] => {
                var answerText = [];
                _.each(answers, (answer: services.IAnswer) => {
                    if (answer.selected === true) {
                        answerText.push(answer.text);
                    }
                });
                return answerText;
            }

            scope.getUnSelectedAnswer = (answers: services.IAnswer[]): services.IAnswer[] => {
                var answerText = [];
                _.each(answers, (answer: services.IAnswer) => {
                    if (answer.selected === false) {
                        answerText.push(answer.text);
                    }
                });
                return answerText;
            }

            scope.enableCommentButton = (question: services.IQuestion): boolean => {
                if(scope.application != null && scope.application != undefined) {
                    if (scope.application.applicationStatusId == 5) // if application is approved no one can enter comments
                        return true;
                    else
                        return false;
                }
                return true;
            }

            scope.flagClick = (question: services.IQuestion): void => {
                question.flag = !question.flag;
            }

            scope.onStatusSelected = (question: services.IQuestion, status: services.IApplicationResponseStatusItem): void => {
                question.answerResponseStatusId = status.id;
                question.answerResponseStatusName = status.name;
                question.statusChanged = true;

                if (question.answerResponseStatusName == this.config.applicationSectionStatuses.rfi) // if RFI status selected alert user to enter rfi comments
                {
                    question.commentType = this.common.getCommentType(1);
                    question.isCollapsed = false;
                }
                else if (question.answerResponseStatusName == this.config.applicationSectionStatuses.rfiFollowUp)
                {
                    question.commentType = this.common.getCommentType(1);
                    question.isCollapsed = false;
                }
                else if (question.answerResponseStatusName == this.config.applicationSectionStatuses.notCompliant) // if "not compliance" status selected alert user to enter citation comments
                {
                    question.commentType = this.common.getCommentType(2);
                    question.isCollapsed = false;
                }
                else {
                    question.isCollapsed = true;
                }

            }

            scope.onAddCloseClicked = (question: services.IQuestion): void => {
                question.isCollapsed = !question.isCollapsed;
                question.editIndex = 0;
                question.responseComment = "";
            }

            scope.saveComment = (question: services.IQuestion, comment: services.IApplicationResponseComment): void => {
                if (scope.validateAddComment(question, comment) == true) {
                    if (question.file && question.file != "" && question.fileName && question.fileName != "") {
                        if (!this.accessToken) {
                            this.documentService.getAccessToken(scope.application.organizationName)
                                .then((data: services.IAccessToken) => {
                                    this.accessToken = data;
                                    scope.saveCommentDocument(question);
                                })
                                .catch(() => {
                                    this.notificationFactory.error("Unable to get access to document Library. Please contact support.");
                                });
                        } else {
                            scope.saveCommentDocument(question);
                        }
                    }
                    else {
                        scope.saveResponseComment(question, comment);
                    }
                }

            }

            scope.showDocumentLibrary = (question: services.IQuestion): void => {
                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/documentLibrary.html",
                    controller: "app.modal.templates.DocumentLibraryController",
                    controllerAs: "vm",
                    size: 'xl',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        allowMultiple: () => {
                            return true;
                        },
                        isReadOnly: () => {
                            return false;
                        },
                        organization: () => {
                            return this.common.application.organizationName;
                        },
                        accessToken: () => {
                            return this.common.accessToken;
                        },
                        appUniqueId: () => {
                            return this.$location.search().app;
                        }
                    }
                });

                instance.result.then((documents: Array<services.IDocument>) => {
                    console.log(documents);
                    if (!question.documents) question.documents = [];

                    if (documents.length > 0) {
                        for (var i = 0; i < documents.length; i++) {
                            var found = _.find(question.documents, (d) => {
                                return d.id === documents[i].id;
                            });
                            if (!found) {
                                question.documents.push(documents[i]);
                            }
                            
                        }
                    }

                }, () => {
                });

            }

            scope.onDocumentDownload = (document: services.IDocument): void => {
                this.trueVaultService.getBlob(this.common.accessToken.vaultId, document.requestValues, this.common.currentUser.documentLibraryAccessToken, this.config.factKey)
                    .then((data: any) => {
                        var fileType = this.trueVaultService.getFileType(document.name);
                        var file = new Blob([data.response], { type: fileType });
                        saveAs(file, document.name);
                    })
                    .catch((e) => {
                        this.notificationFactory.error("Cannot get document from True Vault. " + e);
                    });
            }

            scope.saveCommentDocument = (question: services.IQuestion, comment: services.IApplicationResponseComment) => {
                var token = this.accessToken;

                if (question.fileName.indexOf(".") === -1) {
                    var file: any = question.file;

                    var extension = file.name.substring(file.name.indexOf("."));
                    question.fileName += extension;
                }

                this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, token.vaultId, question.file, question.fileName, question.fileName)
                    .then((data: services.ITrueVaultBlobResponse) => {
                        if (data.result !== "success") {
                            this.notificationFactory.error("Error trying to save document. Please contact support.");
                            this.common.hideSplash();
                        } else {
                            this.documentService.add(scope.application.organizationName, question.fileName, question.fileName, this.common.currentUser.documentLibraryAccessToken, data.blob_id, scope.application.uniqueId)
                                .then((data: services.IGenericServiceResponse<services.IDocument>) => {
                                    if (data.hasError) {
                                        this.notificationFactory.error(data.message);
                                    } else {
                                        question.documentId = data.item.id;
                                        scope.saveResponseComment(question, comment);
                                        question.file = null;
                                        question.fileName = "";
                                    }
                                })
                                .catch(() => {
                                    this.notificationFactory.error("Error trying to save document. Please contact support.");
                                });
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error saving data to True Vault. Please contact support");
                    });
            }

            scope.saveResponseComment = (question: services.IQuestion, comment: services.IApplicationResponseComment): void => {
                this.common.showSplash();
                var _that = this;

                var commentType = (question.editIndex === 0) ? question.commentType : comment.commentType;
                //TODO: Replace the following code. Bcz of some obscure reasons, commentType is appearing as a number
                commentType = this.common.processCommentType(commentType);

                if (scope.application) {
                    this.applicationResponseCommentService.save(question.applicationResponseCommentId, question.responseComment, question.documentId, question.answerResponseStatusId, scope.application.applicationId, question.id, commentType, question.documents)
                        .then((data: app.services.IGenericServiceResponse<services.IApplicationResponseComment>) => {
                            if (data.hasError) {
                                this.notificationFactory.error(data.message);
                                this.common.hideSplash();
                            } else {
                                debugger;
                                if (data.item != null) {
                                    if (scope.isCoordinatorView && data.item.commentOverride) {
                                        data.item.comment = data.item.commentOverride;
                                    }
                                    if (question.applicationResponseCommentId == 0 || question.applicationResponseCommentId == undefined) {
                                        data.item.commentType = commentType;
                                        if (question.applicationResponseComments != null && question.applicationResponseComments != undefined)
                                            question.applicationResponseComments.unshift(data.item);
                                        else {
                                            question.applicationResponseComments = [];
                                            question.applicationResponseComments.unshift(data.item);
                                        }
                                        scope.adjustCommentsArrays(question);

                                        if (data.message != '' && data.message != null) {
                                            this.notificationFactory.success(data.message);
                                        }

                                        this.common.$broadcast('commentAdded', { commentType: commentType});
                                    }
                                    else {

                                        var commentFound = _.find(question.applicationResponseComments, (u) => {
                                            return u.applicationResponseCommentId === question.applicationResponseCommentId;
                                        });
                                        if (commentFound) angular.copy(data.item, commentFound);

                                        scope.adjustCommentsArrays(question);

                                        this.notificationFactory.success("Comment saved");
                                    }

                                    question.applicationResponseCommentId = 0;
                                    question.responseComment = "";
                                    question.isCollapsed = true;
                                    question.editIndex = 0;

                                    this.addedComment.push({
                                        questionId: question.id
                                    });
                                }
                                else {
                                    this.notificationFactory.error("Error.");
                                }
                            }
                            this.common.hideSplash();
                        })
                        .catch((e) => {
                            this.notificationFactory.error("Cannot Save Response. " + e);
                            this.common.hideSplash();
                        });
                }
                else {
                    this.notificationFactory.error("Cannot Save Response. Application object not loaded!!!");
                }
            }

            scope.validateAddComment = (question: services.IQuestion, comment: services.IApplicationResponseComment): boolean => {

                var commentType = question.editIndex === 0 ? question.commentType : comment.commentType;

                if (commentType == null) {
                    alert("select comment type");
                    return false;
                }

                if (question.responseComment == "") {
                    alert("Enter comments");
                    return false;
                }

                if (question.file) {
                    if (!question.fileName) {
                        alert("Enter file Name");
                        return false;
                    }
                }
                //if (question.file != null && question.file != "" && question.fileName == "") {}

                return true;
            }

            scope.validateEdit = (comment: services.IApplicationResponseComment): boolean => {

                // check only creater of the comment can edit or delete the comment.
                if (comment.createdBy != this.common.currentUser.emailAddress) {
                    return false;
                }
                return true;
            }

            scope.editComment = (question: services.IQuestion, comment: services.IApplicationResponseComment, editIndex: number, type: string): void => {
                if (type === 'edit') {
                    question.applicationResponseCommentId = comment.applicationResponseCommentId;
                    question.responseComment = comment.comment;
                    question.editIndex = editIndex; //we need to maintain editIndex at question level bcz comments get repeated.
                    question.isCollapsed = true;
                    comment.commentType = comment.commentType;
                } else {
                    scope.deleteComment(question, comment, editIndex);
                }
            }

            scope.deleteComment = (question: services.IQuestion,
                comment: services.IApplicationResponseComment,
                editIndex: number): void => {

                if (question.answerResponseStatusId === 3) { //Check 'for review'
                    if (!confirm("Are you sure you want to delete this comment? It cannot be undone.")) return;
                } else {
                    if (!confirm("Are you sure you want to delete this comment? This will change the review status to 'For Review'.")) return;
                }


                this.common.showSplash();

                this.applicationResponseCommentService.delete(comment.applicationResponseCommentId)
                    .then((data) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.notificationFactory.success("Comment deleted successfully.");

                            var found = this.findAndRemove(scope.question.responseCommentsCitation,
                                comment.applicationResponseCommentId);

                            if (!found) {
                                found = this.findAndRemove(scope.question.responseCommentsSuggestion,
                                    comment.applicationResponseCommentId);
                            }

                            if (!found) {
                                found = this.findAndRemove(scope.question.responseCommentsFactResponse,
                                    comment.applicationResponseCommentId);
                            }

                            if (!found) {
                                found = this.findAndRemove(scope.question.responseCommentsFactOnly,
                                    comment.applicationResponseCommentId);
                            }

                            if (!found) {
                                this.findAndRemove(scope.question.responseCommentsRFI, comment.applicationResponseCommentId);
                            }

                            this.findAndRemove(scope.question.applicationResponseComments,
                                comment.applicationResponseCommentId);

                            question.answerResponseStatusId = 3; //Change to For Review per bug 1487
                            question.answerResponseStatusName = this.config.applicationSectionStatuses.forReview;
                        }

                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to delete comment. Please contact support.");
                        this.common.hideSplash();
                    });
            }

            scope.hideRFICommentFromUser = (comment: services.IApplicationResponseComment): boolean => {
                var updatedDate = new Date(comment.createdDate || comment.updatedDate);

                if (scope.isUser && scope.application && scope.application.submittedDate != null) {
                    if (comment.commentTo === "FACT Administrator") {
                        return false;
                    }
                    if (scope.application.applicantApplicationStatusName !== "RFI" && updatedDate > scope.application.submittedDate)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }

            scope.onMarkComment = (comment: services.IApplicationResponseComment): void => {
                this.common.showSplash();
                this.applicationResponseCommentService.markCommentVisibility(comment.applicationResponseCommentId, comment.visibleToApplicant)
                    .then((data) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.notificationFactory.success("Comment visibility updated successfully.");
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error setting comment visibility. Please contact support.");
                        this.common.hideSplash();
                    });
            }

            scope.onIncludeInReporting = (comment: services.IApplicationResponseComment): void => {
                this.common.showSplash();
                this.applicationResponseCommentService.markCommentInclusion(comment.applicationResponseCommentId, comment.includeInReporting)
                    .then((data) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.notificationFactory.success(comment.includeInReporting ? "Comment included in reporting." : "Comment excluded from reporting.");
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error updating comment. Please contact support.");
                        this.common.hideSplash();
                    });
            }

            scope.onDocumentDownload = (document: services.IDocument): void => {
                this.trueVaultService.onDocumentDownload(document, scope.application.organizationName, this.common.accessToken);
            }

            scope.adjustCommentsArrays = (question: services.IQuestion): void => {
                question.responseCommentsCitation = [];
                question.responseCommentsCoordinator = [];
                question.responseCommentsFactOnly = [];
                question.responseCommentsFactResponse = [];
                question.responseCommentsRFI = [];
                question.responseCommentsSuggestion = [];
                _.each(question.applicationResponseComments, (comment: services.IApplicationResponseComment) => {
                    comment.createdDte = moment(comment.createdDate).toDate();

                    if (comment.visibleToApplicant) {
                        switch (parseInt(comment.commentType.id)) {
                            case 1: question.responseCommentsRFI.push(comment); break;
                            case 2: question.responseCommentsCitation.push(comment); break;
                            case 3: question.responseCommentsSuggestion.push(comment); break;
                            case 4: question.responseCommentsCoordinator.push(comment); break;
                            case 5: question.responseCommentsFactResponse.push(comment); break;
                            case 6: question.responseCommentsFactOnly.push(comment); break;
                            default: break;
                        }
                    }
                   
                });
            }

        }
    }


    angular
        .module('app.widgets')
        .directive('answerReviewCommon', AnswerReviewCommon.factory());
}