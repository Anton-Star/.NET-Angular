module app.modal.templates {
    'use strict';

    interface ICommentQuestion {
        questionId: string;
    }

    interface IValues {
        section: services.IApplicationSectionResponse;
        appUniqueId: string;
        isUser: boolean;
        questionId?: string;
        accessToken: services.IAccessToken;
        organization?: string;
        requirementId: string;
    }

    class ApplicationAnswersReviewController {
        selectedStatus: number;
        application: services.IApplication;
        statusTypes: Array<services.IApplicationResponseStatusItem>;
        commentTypes: Array<services.ICommentType>;
        isTrainee: boolean = false;
        isInspector: boolean = false;
        accessToken: services.IAccessToken;
        isFact = false;
        isRfi = false;
        readOnly: boolean = false;
        organization: string;
        addedComment: ICommentQuestion[] = [];
        isInit = true;
        savePending = false;
        autoSaveTimer = 300000;
        section: services.IApplicationSectionResponse;
        appUniqueId: string;
        isUser: boolean;
        requirementId: string;
        origSection: services.IApplicationSectionResponse;
        options = {
            //keydown: (e) => {
            //    var id = e.sender.element[0].id;
            //    id = id.replace("comment", "");
            //    var question = this.section.questions[id];
            //    question.responseComment = e.sender.element[0].value;
            //    this.$scope.$apply();
            //}
        };
        rfiCommentAdded = false;
        disableCancel = false;

        static $inject = [
            '$rootScope',
            '$uibModal',
            '$scope',
            '$timeout',
            '$location',
            'notificationFactory',
            'documentService',
            'questionService',
            'trueVaultService',
            'common',
            'inspectionScheduleService',
            'applicationResponseCommentService',
            'currentUser',
            '$uibModalInstance',
            'applicationService',
            'values',
            'config'
        ];

        constructor(
            private $rootScope: ng.IRootScopeService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $scope: ng.IScope,
            private $timeout: ng.ITimeoutService,
            private $location: ng.ILocationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private documentService: services.IDocumentService,
            private questionService: services.IQuestionService,
            private trueVaultService: services.ITrueVaultService,
            private common: app.common.ICommonFactory,
            private inspectionScheduleService: services.IInspectionScheduleService,
            private applicationResponseCommentService: services.IApplicationResponseCommentService,
            private currentUser: services.IUser,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private applicationService: services.IApplicationService,
            private values: IValues,
            private config: IConfig) {

            console.log(values);

            if (this.values.section == null) {
                if (this.common.compApp != null) {
                    for (var i = 0; i < this.common.compApp.complianceApplicationSites.length; i++) {
                        var compAppSite = this.common.compApp.complianceApplicationSites[i];

                        for (var j = 0; j < compAppSite.appResponses.length; j++) {
                            if (compAppSite.appResponses[j].applicationUniqueId === this.values.appUniqueId) {
                                this.values.section = this.findSectionHierarchy(compAppSite.appResponses[j]
                                    .applicationSectionResponses,
                                    this.values.requirementId);
                                this.common.activateController([this.getSectionQuestions()], '');
                                break;
                            }
                        }
                    }
                } else {

                    this.applicationService.getAppSections(this.values.appUniqueId)
                        .then((data: Array<services.IApplicationSection>) => {

                            var filteredSection = this.findSection(data, this.values.requirementId);
                            if (filteredSection != null) {
                                this.values.section = this.processSection(filteredSection);
                            }
                            this.common.activateController([this.getSectionQuestions()], '');

                        });
                }
            } else {
                this.common.activateController([this.getSectionQuestions()], '');
            }

            this.$rootScope.$on("commentAdded", (data: any, args: any) => {
                if (args.commentType) {
                    if (args.commentType.name === "RFI") {
                        this.disableCancel = true;
                        this.rfiCommentAdded = true;
                    }
                }
            });
        }

        getSectionQuestions(): ng.IPromise<void> {
            return this.questionService.getSectionQuestions(this.values.appUniqueId, this.values.section.applicationSectionId || this.values.section.id || this.values.section.applicationSectionId)
                .then((data) => {
                    this.values.section.questions = _.filter(data, (q) => {
                        return !q.isHidden;
                    });

                    console.log('questions', data);

                    //this.values.section.questions = data;
                    this.init();
                });
        }

        init() {

            if (this.values.section.children &&
                this.values.section.children.length > 0 &&
                this.values.section.questions &&
                this.values.section.questions.length === 0) {
                for (var j = 0; j < this.values.section.children.length; j++) {
                    if (this.values.section.children[j].questions && this.values.section.children[j].questions.length > 0) {
                        var found = _.find(this.values.section.children[j].questions, (q: services.IQuestion) => {
                            return q.answerResponseStatusName === "RFI" || (q.responseCommentsRFI && q.responseCommentsRFI.length > 0);
                        });

                        if (found) {
                            this.isRfi = true;
                            this.section = this.values.section.children[j];
                            break;
                        }
                    }
                }

                this.section = this.section || this.values.section;

            } else {
                _.each(this.values.section.questions, (q) => {
                    q.isCollapsed = true;
                });

                this.section = this.values.section;    
            }
            
            this.organization = this.values.organization;
            this.appUniqueId = this.values.appUniqueId;
            this.isUser = this.values.isUser;
            this.isFact = this.common.currentUser.role.roleName === this.config.roles.factAdministrator ||
                this.common.currentUser.role.roleName === this.config.roles.factCoordinator;

            //this.getApplication();
            this.common.activateController([this.getApplication(), this.getAccreditationRoleByUserId(), this.getApplicationResponseStatus(), this.getCommentTypes(), this.getAccessToken()], '');
            
            if (this.values.questionId) {
                for (var i = 0; i < this.section.questions.length; i++) {
                    if (this.section.questions[i].id !== this.values.questionId) {
                        this.section.questions.splice(i, 1);
                        i--;
                    } else {
                        this.section.questions[i].commentType = this.common.getCommentType(1);
                    }
                }
            }

            if (this.isUser && this.section && this.section.questions && this.section.questions.length) {
                for (var i = 0; i < this.section.questions.length; i++) {
                    if (this.section.questions[i].answerResponseStatusName !== "RFI" &&
                        this.section.questions[i].answerResponseStatusName !== "RFI Completed" &&
                        this.section.questions[i].answerResponseStatusName !== "Complete" &&
                        this.section.questions[i].answerResponseStatusName !== "RFI/Followup" &&
                        (!this.section.questions[i].responseCommentsRFI || this.section.questions[i].responseCommentsRFI.length === 0)) {

                        this.section.questions.splice(i, 1);
                        i--;
                    } else {
                        this.section.questions[i].commentType = this.common.getCommentType(1);
                    }
                }

            }

            this.isInspector = this.common.currentUser.role.roleName === this.config.roles.inspector;

            if (!this.isFact && !this.isInspector) {
                _.each(this.section.questions, (q) => {
                    var comments: services.IApplicationResponseComment[] = [];

                    _.each(q.responseCommentsRFI, (r) => {
                        if (r.visibleToApplicant ||
                            r.commentFrom.userId === this.common.currentUser.userId ||
                            r.commentFrom.role.roleName === this.config.roles.user) {
                            comments.push(r);
                        }
                    });

                    q.responseCommentsRFI = comments;
                });
            }

            _.each(this.section.questions, (value: services.IQuestion) => {
                if (value.answerResponseStatusId == 0) {
                    value.answerResponseStatusId = 3;
                }

                if (value.responseCommentsCitation && value.responseCommentsCitation.length > 0) {
                    _.each(value.responseCommentsCitation, (comment: services.IApplicationResponseComment) => {
                        comment.createdDte = moment(comment.createdDate).toDate();
                    });
                }

                if (value.responseCommentsSuggestion && value.responseCommentsSuggestion.length > 0) {
                    _.each(value.responseCommentsSuggestion, (comment: services.IApplicationResponseComment) => {
                        comment.createdDte = moment(comment.createdDate).toDate();
                    });
                }

                if (value.responseCommentsFactResponse && value.responseCommentsFactResponse.length > 0) {
                    _.each(value.responseCommentsFactResponse, (comment: services.IApplicationResponseComment) => {
                        comment.createdDte = moment(comment.createdDate).toDate();
                    });
                }

                if (value.responseCommentsFactOnly && value.responseCommentsFactOnly.length > 0) {
                    _.each(value.responseCommentsFactOnly, (comment: services.IApplicationResponseComment) => {
                        comment.createdDte = moment(comment.createdDate).toDate();
                    });
                }

                if (value.responseCommentsRFI && value.responseCommentsRFI.length > 0) {
                    _.each(value.responseCommentsRFI, (comment: services.IApplicationResponseComment) => {
                        comment.createdDte = moment(comment.createdDate).toDate();
                    });
                }
                
            });

            //this.$scope.$watch('vm.section.questions', (newValues, oldValues) => {
            //    if (newValues !== oldValues) {
            //        if (this.isInit) {
            //            this.isInit = false;
            //        } else {
            //            this.hasChanges();
            //        }
            //    }
            //}, true);
            
        }

        hasChanges(): void {
            //if (!this.savePending) {
            //    this.$timeout(() => {
            //        this.save(true, false);
            //    }, this.autoSaveTimer);
            //    this.savePending = true;
            //}
        }

        getApplication(): ng.IPromise<void> {
            return this.applicationService.getApp(this.values.appUniqueId)
                .then((data: services.IApplication) => {
                    this.application = data;
                    this.organization = this.application.organizationName;
                    
                    this.isRfi = data.applicationStatusName === "RFI" || data.applicationStatusName === "RFI In Progress";

                    if (this.isUser == true && this.application.rfiDueDate !== "" && this.application.applicationStatusName !== "Post Committee Review ") {
                        const rfiDate = moment(this.application.rfiDueDate).add(1, "days").toDate();
                        const now = new Date();

                        if (rfiDate < now) this.readOnly = true;
                    }

                    //var rfiDate = new Date(this.application.rfiDueDate);
                    //var now = moment();
                    //var tomorrow = now.add(1, "days").toDate();

                    //if (this.application.rfiDueDate != "" && rfiDate < tomorrow && this.isUser == true && this.application.applicationStatusName !== "Post Committee Review ") {
                    //    this.readOnly = true;
                    //}

                    //this.common.activateController([this.getApplicationResponseStatus(), this.getCommentTypes(), this.getAccreditationRoleByUserId(), this.getAccessToken()], 'ApplicationAnswersReviewController');
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting application");
                });
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }


        findSection(requirements: services.IApplicationSection[], sectionId: string): services.IApplicationSection {
            for (var i = 0; i < requirements.length; i++) {
                var req = requirements[i];

                if (req.id === sectionId) {
                    return req;
                }

                if (req.children && req.children.length > 0) {
                    var item = this.findSection(req.children, sectionId);

                    if (item != null) {
                        return item;
                    }
                }
            }

            return null;
        }

        findSectionHierarchy(requirements: services.IApplicationSectionResponse[], sectionId: string): services.IApplicationSectionResponse {
            for (var i = 0; i < requirements.length; i++) {
                var req = requirements[i];

                if (req.applicationSectionId === sectionId) {
                    return req;
                }

                if (req.children && req.children.length > 0) {
                    var item = this.findSectionHierarchy(req.children, sectionId);

                    if (item != null) {
                        return item;
                    }
                }
            }

            return null;
        }

        processSection(section: services.IApplicationSection): services.IApplicationSectionResponse {

            var requirement: services.IApplicationSectionResponse = {
                applicationSectionName: section.name,
                applicationSectionId: section.id,
                questions: section.questions,
                applicationSectionUniqueIdentifier: section.uniqueIdentifier,
                sectionStatusName: section.circle,
                applicationSectionHelpText: section.helpText,
                hasQuestions: section.questions && section.questions.length > 0,
                isVisible: section.isVisible,
                hasFlag: false,
                hasRfiComment: false,
                hasCitationComment: false,
                hasFactOnlyComment: false,
                hasSuggestions: false
            };
            return requirement;
        }

        getAccessToken(): ng.IPromise<void> {
            return this.documentService.getAccessToken(this.organization)
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
        
        getCommentTypes(): ng.IPromise<void> {
            return this.applicationResponseCommentService.getCommentTypes()
                .then((data: Array<app.services.ICommentType>) => {
                    if (data != null) {
                        this.commentTypes = data.filter((commentType) => {
                            if (this.isFact) return true;

                            if (this.common.isInspector()) {
                                return commentType.name !== "FACT Response" && commentType.name !== "Coordinator";
                            } else {
                                return (commentType.name !== 'FACT Only' && commentType.name !== "Coordinator" && commentType.name !== "FACT Response");
                            }                           
                        });;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("No comment type found.");
                });
        }

        getAccreditationRoleByUserId(): ng.IPromise<void> {
            if (this.values && this.values.appUniqueId) {
                return this.inspectionScheduleService.getAccreditationRole(this.common.currentUser.userId, this.values.appUniqueId)
                    .then((data: services.IGenericServiceResponse<services.IAccreditationRole>) => {
                        if (data.item != null) {

                            if (data.item.accreditationRoleName.indexOf("Trainee") != -1)
                                this.isTrainee = true;
                            else
                                this.isTrainee = false;
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                    });
            }
            
        }

        onNext() {
            this.save(false, true);
        }

        save(isAutoSave: boolean, gotoNext: boolean): void {
            if (!this.savePending && isAutoSave) return;

            var validationResult = this.validateSave();
            this.savePending = false;
            var isCommentBoxOpen = false;

            _.each(this.section.questions, (q) => {
                if (!q.isCollapsed) {
                    isCommentBoxOpen = true;
                }
            });

            if (isCommentBoxOpen) {
                if (!confirm("You have an unsaved comment field open.  Select \"Ok\" to continue or \"Cancel\" to return to the form.")) {
                    return;
                }
            }

            if (validationResult) {
                this.common.showSplash();
                if (this.isTrainee) {
                    this.applicationService.saveApplicationSectionTrainee(this.organization, this.application.applicationTypeName, this.application.uniqueId, this.section)
                        .then((data: services.IServiceResponse) => {
                            this.common.hideSplash();
                            if (data.hasError) {
                                this.notificationFactory.error(data.message);
                            } else {
                                if (isAutoSave) {
                                    this.notificationFactory.success("Data auto saved successfully.");
                                    this.common.hideSplash();
                                    this.savePending = false;
                                } else if (!gotoNext) {
                                    this.common.$broadcast('AppSaved', { appUniqueId: this.appUniqueId, rowId: this.section.applicationSectionId });
                                    this.$uibModalInstance.close(this.origSection || this.section);
                                } else {
                                    if (!this.origSection) {
                                        this.origSection = this.section;
                                    }

                                    this.common.$broadcast('CheckStatus', { rowId: this.section.applicationSectionId, section: data });

                                    var nextSection = this.section.nextSection;

                                    this.$uibModalInstance.close(this.origSection || this.section);

                                    this.common.$broadcast('AppSaved', { appUniqueId: this.appUniqueId, rowId: this.section.applicationSectionId, nextSection: nextSection });

                                }
                            }
                        })
                        .catch(() => {
                            this.common.hideSplash();
                            this.notificationFactory.error("Error trying to save. Please contact support.");
                        });
                }
                else {
                    this.section.applicationSectionUniqueIdentifier = this.section.applicationSectionUniqueIdentifier || this.values.appUniqueId || this.$location.search().app;
                    this.applicationService.saveApplicationSection(this.organization, this.application.applicationTypeName, this.application.uniqueId, this.section, this.isRfi)
                        .then((data: services.IServiceResponse) => {
                            this.common.hideSplash();
                            if (data.hasError) {
                                this.notificationFactory.error(data.message);
                            } else {
                                if (isAutoSave) {
                                    this.notificationFactory.success("Data auto saved successfully.");
                                    this.common.hideSplash();
                                    this.savePending = false;
                                } else if (!gotoNext) {
                                    this.common.$broadcast('AppSaved', { appUniqueId: this.appUniqueId, rowId: this.section.applicationSectionId });
                                    this.$uibModalInstance.close(this.origSection || this.section);
                                } else {
                                    if (!this.origSection) {
                                        this.origSection = this.section;
                                    }

                                    var nextSection = this.section.nextSection;

                                    this.common.$broadcast('CheckStatus', { rowId: this.section.applicationSectionId, section: data });
                                    this.common.$broadcast('AppSaved', { appUniqueId: this.appUniqueId, rowId: this.section.applicationSectionId, nextSection: nextSection });

                                    this.$uibModalInstance.close(this.section);

                                    this.notificationFactory.success("Data saved successfully");
                                }
                            }
                        })
                        .catch(() => {
                            this.common.hideSplash();
                            this.notificationFactory.error("Error trying to save. Please contact support.");
                        });

                    if (!this.isUser && !isAutoSave) {

                        var notifyApplicant = false;
                        _.each(this.section.questions, (question: services.IQuestion) => {
                            if (question.answerResponseStatusId == 7) {
                                notifyApplicant = true;
                            }
                        });

                        if (notifyApplicant) {
                            this.applicationResponseCommentService.notifyApplicantAboutRFI(this.application.createdBy)
                                .catch(() => {
                                    this.notificationFactory.error("Error trying to notify applicant about RFI.");
                                });
                        }
                    }
                }
            }
        }

        validateSave(): boolean {
            var requireRFIComments = false;
            var requireCitationComment = false;
            
            _.each(this.section.questions, (question) => {
                if (question.answerResponseStatusName === this.config.applicationSectionStatuses.rfi || question.answerResponseStatusName === this.config.applicationSectionStatuses.rfiFollowUp) {
                    if (question.responseCommentsRFI.length == 0) {
                        if (this.common.isUser()) {
                            if (!this.rfiCommentAdded) {
                                requireRFIComments = true;
                            }
                        }
                    } else if (!this.common.isUser() && question.statusChanged != null && question.statusChanged != undefined) {
                        var now = new Date();
                        now.setHours(0, 0, 0, 0);

                        var found = _.find(question.responseCommentsRFI, (r) => {
                            var created = moment(r.createdDate).toDate();

                            return created > now;
                        });

                        if (!found) {
                            requireRFIComments = true;
                        }
                    }
                }               

                if (question.answerResponseStatusName === this.config.applicationSectionStatuses.notCompliant && question.responseCommentsCitation.length === 0) {
                    requireCitationComment = true;
                }
            });

            if(requireRFIComments) {
                this.notificationFactory.error('Questions with RFI require RFI comments');
                return false;
            }

            if (requireCitationComment) {
                this.notificationFactory.error("Questions with Not Compliant require Citation comment");
                return false;
            }

            return true;
        }
                
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.ApplicationAnswersReviewController',
        ApplicationAnswersReviewController);
} 