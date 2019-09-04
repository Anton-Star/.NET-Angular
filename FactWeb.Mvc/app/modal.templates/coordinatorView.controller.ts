module app.modal.templates {
    'use strict';

    interface ICoordinatorView {

    }

    interface ICommentQuestion {
        questionId: string;
    }

    class CoordinatorViewController implements ICoordinatorView {
        selectedQuestion: Array<services.IApplicationResponseComment> = [];
        allSections: Array<services.IApplicationSection>
        statusTypes: Array<services.IApplicationResponseStatusItem>;
        isFact = false;
        isInspector: boolean = false;
        application: services.IApplication;
        isRfi = false;
        readOnly: boolean = false;
        organization: string;
        addedComment: ICommentQuestion[] = [];
        commentTypes: Array<services.ICommentType>;
        accessToken: services.IAccessToken;

        static $inject = [
            'applicationService',
            'coordinatorService',
            'notificationFactory',
            'config',
            'common',
            '$uibModalInstance',
            'documentService',
            'trueVaultService',
            'applicationResponseCommentService',
            'allSection',
            'appUniqueId'
        ];

        constructor(
            private applicationService: services.IApplicationService,
            private coordinatorService: services.ICoordinatorService,
            private notificationFactory: app.blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private documentService: services.IDocumentService,
            private trueVaultService: services.ITrueVaultService,
            private applicationResponseCommentService: services.IApplicationResponseCommentService,
            private allSection: Array<services.IApplicationSection>,
            private appUniqueId: string) {

            this.allSections = this.allSection;
            this.getApplication();

            if (this.allSections) {
                _.each(this.allSections, (section: services.IApplicationSection) => {
                    if (section.questions && section.questions.length) {
                        for (var i = 0; i < section.questions.length; i++) {
                            var found = _.find(section.questions[i].applicationResponseComments, (c: services.IApplicationResponseComment) => {
                                return c.commentType != null && (c.commentType.name === "RFI" || c.commentType.name === "Citation");
                            });

                            if (found) {
                                section.questions[i].commentType = this.common.getCommentType(1);
                            }

                            //if (section.questions[i].answerResponseStatusName !== "RFI" &&
                            //    section.questions[i].answerResponseStatusName !== "RFI Completed" &&
                            //    section.questions[i].answerResponseStatusName !== "Complete") {

                            //    section.questions.splice(i, 1);
                            //    i--;
                            //} else {
                            //    section.questions[i].commentTypeId = 1;
                            //}
                        }
                    }
                });
            }

            this.isInspector = this.common.currentUser.role.roleName === this.config.roles.inspector;

            if (this.allSections) {
                _.each(this.allSections, (section: services.IApplicationSection) => {
                    _.each(section.questions, (value: services.IQuestion) => {
                        if (value.answerResponseStatusId == 0) {
                            value.answerResponseStatusId = 1;
                        }
                    });
                });
            }

            this.isFact = this.common.currentUser.role.roleName === this.config.roles.factAdministrator;
        }

        getApplication() {
            this.applicationService.getApp(this.appUniqueId)
                .then((data: services.IApplication) => {
                    this.application = data;

                    this.getAccessToken();

                    this.isRfi = data.applicationStatusName === "RFI" || data.applicationStatusName === "RFI In Progress";

                    var rfiDate = new Date(this.application.rfiDueDate);
                    var now = moment();
                    var tomorrow = now.add(1, "days").toDate();

                    if (this.application.rfiDueDate != "" && rfiDate < tomorrow) {
                        this.readOnly = true;
                    }

                    this.common.activateController([this.getApplicationResponseStatus(), this.getCommentTypes()], 'CoordinatorViewController');
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting application");
                });
        }

        getAccessToken(): ng.IPromise<void> {
            return this.documentService.getAccessToken(this.application.organizationName)
                .then((data: services.IAccessToken) => {
                    this.accessToken = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to get access to document Library. Please contact support.");
                });
        }

        getApplicationResponseStatus(): ng.IPromise<void> {
            return this.applicationService.getApplicationResponseStatus() //duplicate
                .then((data: Array<app.services.IApplicationResponseStatusItem>) => {
                    if (data != null) {
                        this.organization = this.application.organizationName;

                        if (this.common.currentUser.role.roleName.indexOf("Inspector") != -1) {
                            this.statusTypes = data.filter(function (statusType) {
                                return (statusType.name == 'Compliant' || statusType.name == 'Not Compliant' || statusType.name == 'N/A' || statusType.name == 'RFI');
                            });
                        }
                        else if (this.application.applicationTypeName.indexOf("Compliance") != -1) {
                            this.statusTypes = data.filter(function (statusType) {
                                return (statusType.name != 'Compliant' && statusType.name != 'Not Compliant');
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

        getCommentTypes(): ng.IPromise<void> {
            return this.applicationResponseCommentService.getCommentTypes()
                .then((data: Array<app.services.ICommentType>) => {
                    if (data != null) {
                        this.commentTypes = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("No comment type found.");
                });
        }

        validateSave(): boolean {
            return true;
        }

        getSelectedAnswer(answers: Array<services.IAnswer>): string {
            var answerTest = "";
            _.each(answers, (answer: services.IAnswer) => {
                if (answer.selected == true) {
                    answerTest = answer.text;
                    return answerTest
                }
            });
            return answerTest;
        }

        enableCommentButton(question: services.IQuestion): boolean {
            if (this.application != null && this.application != undefined) {
                if (this.application.applicationStatusId == 5) // if application is approved no one can enter comments            
                    return true;
                else
                    return false;
            }

            return true;
        }

        flagClick(question: services.IQuestion): void {
            question.flag = !question.flag;
        }

        onStatusSelected(question: services.IQuestion, status: services.IApplicationResponseStatusItem, index: string): void {
            question.answerResponseStatusId = status.id;
            question.answerResponseStatusName = status.name;

            if (question.answerResponseStatusName == "RFI") // if RFI status selected alert user to enter rfi comments
            {
                question.commentType.id = '1';
                (<any>$("#divAdditionalComments" + index)).collapse('show');
                $('#lblCommentRequired' + index).show();

            }
            else if (question.answerResponseStatusName == "Not Compliant") // if "not compliance" status selected alert user to enter citation comments
            {
                question.commentType.id = '2';
                (<any>$("#divAdditionalComments" + index)).collapse('show');
                $('#lblCommentRequired' + index).show();
            }
            else {
                (<any>$("#divAdditionalComments" + index)).collapse('hide');
                $('#lblCommentRequired' + index).hide();
            }
        }

        saveComment(question: services.IQuestion): void {
            if (this.validateAddComment(question) == true) {
                if (question.file && question.file != "" && question.fileName && question.fileName != "") {
                    this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.accessToken.vaultId, question.file, question.fileName, question.fileName, "")
                        .then((data: services.ITrueVaultBlobResponse) => {
                            if (data.result !== "success") {
                                this.notificationFactory.error("Error trying to save document. Please contact support.");
                                this.common.hideSplash();
                            } else {
                                this.documentService.add(this.organization, question.fileName, question.fileName, this.common.currentUser.documentLibraryAccessToken, data.blob_id, false, this.appUniqueId)
                                    .then((data: services.IGenericServiceResponse<services.IDocument>) => {
                                        if (data.hasError) {
                                            this.notificationFactory.error(data.message);
                                        } else {
                                            question.documentId = data.item.id;
                                            this.saveResponseComment(question);
                                        }
                                    })
                                    .catch(() => {
                                        this.notificationFactory.error("Error trying to save document. Please contact support.");
                                    });
                            }
                        })
                        .catch((e) => {
                            if (e.indexOf("404.13") != -1) {
                                this.notificationFactory.error("Error trying to save document. Maximum allowed upload file size is 30MB.");
                            }
                            else {
                                this.notificationFactory.error("Error trying to save to True Vault. Please contact support.");
                            }

                            this.common.hideSplash();
                        });
                }
                else {
                    this.saveResponseComment(question);
                }
            }
        }

        saveResponseComment(question: services.IQuestion): void {
            this.common.showSplash();
            //this.applicationResponseCommentService.save(question.applicationResponseCommentId, question.responseComment, question.documentId, question.answerResponseStatusId, this.application.applicationId, question.id, question.commentTypeId)
            //    .then((data: app.services.IGenericServiceResponse<services.IApplicationResponseComment>) => {
            //        if (data.hasError) {
            //            this.notificationFactory.error(data.message);
            //            this.common.hideSplash();
            //        } else {

            //            if (data.item != null) {
            //                if (question.applicationResponseCommentId == 0 || question.applicationResponseCommentId == undefined) {

            //                    if (question.applicationResponseComments != null && question.applicationResponseComments != undefined)
            //                        question.applicationResponseComments.push(data.item);
            //                    else {
            //                        question.applicationResponseComments = [];
            //                        question.applicationResponseComments.push(data.item);
            //                    }

            //                    if (question.commentTypeId == 1)
            //                        question.responseCommentsRFI.push(data.item);
            //                    else if (question.commentTypeId == 2)
            //                        question.responseCommentsCitation.push(data.item);
            //                    else if (question.commentTypeId == 3)
            //                        question.responseCommentsSuggestion.push(data.item);
            //                    else if (question.commentTypeId == 5)
            //                        question.responseCommentsFactResponse.push(data.item);
            //                    else if (question.commentTypeId == 6)
            //                        question.responseCommentsFactOnly.push(data.item);

            //                    if (data.message != '' && data.message != null) {
            //                        this.notificationFactory.success(data.message);
            //                    }
            //                }
            //                else {
            //                    var commentFound = _.find(question.applicationResponseComments, (u) => {
            //                        return u.applicationResponseCommentId === question.applicationResponseCommentId;
            //                    });

            //                    if (commentFound)
            //                        angular.copy(data.item, commentFound);

            //                    if (question.commentTypeId == 1) {
            //                        var commentFound1 = _.find(question.responseCommentsRFI, (u) => {
            //                            return u.applicationResponseCommentId === question.applicationResponseCommentId;
            //                        });

            //                        if (commentFound1)
            //                            angular.copy(data.item, commentFound1);
            //                    }
            //                    else if (question.commentTypeId == 2) {
            //                        var commentFound1 = _.find(question.responseCommentsCitation, (u) => {
            //                            return u.applicationResponseCommentId === question.applicationResponseCommentId;
            //                        });

            //                        if (commentFound1)
            //                            angular.copy(data.item, commentFound1);

            //                    }
            //                    else if (question.commentTypeId == 3) {
            //                        var commentFound1 = _.find(question.responseCommentsSuggestion, (u) => {
            //                            return u.applicationResponseCommentId === question.applicationResponseCommentId;
            //                        });

            //                        if (commentFound1)
            //                            angular.copy(data.item, commentFound1);

            //                    }
            //                    else if (question.commentTypeId == 5) {
            //                        var commentFound1 = _.find(question.responseCommentsFactResponse, (u) => {
            //                            return u.applicationResponseCommentId === question.applicationResponseCommentId;
            //                        });

            //                        if (commentFound1)
            //                            angular.copy(data.item, commentFound1);
            //                    }
            //                    else if (question.commentTypeId == 6) {
            //                        var commentFound1 = _.find(question.responseCommentsFactOnly, (u) => {
            //                            return u.applicationResponseCommentId === question.applicationResponseCommentId;
            //                        });
                                    
            //                        if (commentFound1)
            //                            angular.copy(data.item, commentFound1);
            //                    }

            //                    this.notificationFactory.success("Comment saved");
            //                }

            //                question.applicationResponseCommentId = 0;
            //                question.responseComment = "";

            //                this.addedComment.push({
            //                    questionId: question.id
            //                });
            //            }
            //            else {
            //                this.notificationFactory.error("Error.");
            //            }
            //        }
            //        this.common.hideSplash();
            //    })
            //    .catch(() => {
            //        this.notificationFactory.error("Error.");
            //        this.common.hideSplash();
            //    });
        }

        validateAddComment(question: services.IQuestion): boolean {
            return true;
        }

        validateEdit(comment: services.IApplicationResponseComment): boolean {
            // check only creater of the comment can edit or delete the comment.
            if (comment.createdBy != this.common.currentUser.emailAddress) {
                return false;
            }
            return true;
        }

        editComment(question: services.IQuestion, comment: services.IApplicationResponseComment): void {
            //question.applicationResponseCommentId = comment.applicationResponseCommentId;
            //question.responseComment = comment.comment;
            ////question.commentTypeId = parseInt(comment.commentType.id);
        }

        onInclude(comment: services.IApplicationResponseComment, section: services.IApplicationSection) {
            var sect = _.find(this.allSections, (sec: services.IApplicationSection) => {
                return sec.id === section.id;
            });

            if (sect) {
                var question = _.find(sect.questions, (qq: services.IQuestion) => {
                    return qq.id === comment.questionId;
                });

                if (question) {
                    var res = _.find(question.applicationResponseComments, (c: services.IApplicationResponseComment) => {
                        return c.applicationResponseCommentId === comment.applicationResponseCommentId;
                    });

                    if (res) {
                        res.includeInReporting = comment.includeInReporting;
                    }
                }
            }
        }

        save(): void {
            var validationResult = this.validateSave();

            if (validationResult) {
                this.coordinatorService.updateSection(this.allSections)
                    .then((data: app.services.IServiceResponse) => {
                        this.notificationFactory.success("Record saved Successfully");
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                    });
            }
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.CoordinatorViewController',
        CoordinatorViewController);
} 