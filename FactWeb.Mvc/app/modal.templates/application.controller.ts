module app.modal.templates {
    'use strict';

    interface IApplication {
        onRadioSelected(question: services.IQuestion, answer: services.IAnswer): void;
        save(isAutoSave: boolean, gotoNext: boolean): void;
        cancel(): void;
        flagClick(question: services.IQuestion): void;
        isBusy: boolean;
        isReadOnly: boolean;
        minDate: Date;
        maxDate: Date;
    }

    class ApplicationController implements IApplication {
        isBusy = false;
        minDate = new Date(1970, 0, 1, 0, 0, 0);
        maxDate = new Date(2050, 0, 1, 0, 0, 0);
        users: Array<services.IUser>;
        isInit = true;
        savePending = false;
        autoSaveTimer = 20000;
        isReadOnly = false;
        isObserverOrAuditor: boolean = false;
        accessToken: services.IAccessToken;
        allowSave = true;
        isUser: boolean;
        requirements: Array<IApplicationHierarchyData>;
        dateOptions = {
            open: this.onOpen
        };
        isStatusRfi = false;
        isPostInspection = false;
        isRfiSaved = false;

        vm = this;

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
            'localStorageService',
            'applicationService',
            'organizationService',
            'inspectionScheduleService',
            'questionService',
            'section',
            'questions',
            'appType',
            'accessType',
            'organization',
            'appDueDate',
            'submittedDate',
            'appUniqueId',
            'trueVaultService',
            '$location',
            'site',
            'requirementService',
            'reqId',
            'appId',
            'appStatus'
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
            private localStorageService: any,
            private applicationService: services.IApplicationService,
            private organizationService: services.IOrganizationService,
            private inspectionScheduleService: services.IInspectionScheduleService,
            private questionService: services.IQuestionService,
            private section: services.IApplicationSectionResponse,
            private questions: Array<services.IQuestion>,
            private appType: string,
            private accessType: string,
            private organization: string,
            private appDueDate: string,
            private submittedDate: string,
            private appUniqueId: string,
            private trueVaultService: services.ITrueVaultService,
            private $location: ng.ILocationService,
            private site: services.ISite,
            private requirementService: services.IRequirementService,
            private reqId: string,
            private appId: string,
            private appStatus: string) {

            console.log('sec', section, submittedDate);

            $(".root-sect").css("max-height", "600px");
            $(".root-sect").css("overflow", "hidden");
            //$(".application").css("max-height", "600px");
           
            if (this.common.isUser() || accessType === "") {
                this.isUser = true;
            }

            if (this.appUniqueId == null || this.appUniqueId == "")
                this.appUniqueId = this.$location.search().app;


            if (this.localStorageService.get("section") != null) {
                this.section = this.localStorageService.get('section');
            }

            this.getUsers();
            this.getAccreditationRole(this.appUniqueId);

            var inspectionDate = moment().add('d', 1);
            var today = moment();

            if (this.common.application && this.common.application.inspectionDate != null && this.common.application.inspectionDate != undefined) {
                inspectionDate = moment(this.common.application.inspectionDate);
            }

            if (inspectionDate < today) {
                this.isPostInspection = true;
            }

            if (this.appStatus != null && this.appStatus != undefined) {
                this.isStatusRfi = (this.appStatus === this.config.applicationSectionStatuses.rfi || this.appStatus === this.config.applicationStatuses.rfiInProgress);
                if (this.appStatus !== this.config.applicationSectionStatuses.inProgress &&
                    this.appStatus !== this.config.applicationStatuses.rfiInProgress &&
                    this.appStatus !== this.config.applicationSectionStatuses.rfi &&
                    this.appStatus !== "Applicant Response" &&
                    this.appStatus !== "Post Committee RFI") {
                    this.allowSave = false; 
                    this.isReadOnly = true;
                }
            } else {
                this.getapplicationStatusByUniqueId(this.appUniqueId);    
            }
            

            this.checkAccess();

            if (!this.common.currentUser.isImpersonation) {
                //this.getSetting();
            }

            if (this.section === null) {
                ////section can be null if modal template is opened from doc lib
                if (this.common.compApp != null) {
                    for (var i = 0; i < this.common.compApp.complianceApplicationSites.length; i++) {
                        var compAppSite = this.common.compApp.complianceApplicationSites[i];

                        for (var j = 0; j < compAppSite.appResponses.length; j++) {
                            if (compAppSite.appResponses[j].applicationUniqueId === this.appId) {
                                this.section = this.findSectionHierarchy(compAppSite.appResponses[j].applicationSectionResponses,
                                    this.reqId);
                                break;
                            }
                        }
                    }
                } else {
                    this.section = {
                        applicationSectionId: this.reqId,
                        applicationSectionName: "",
                        applicationSectionHelpText: "",
                        applicationSectionUniqueIdentifier: "",
                        hasQuestions: true,
                        isVisible: true,
                        hasFlag: false,
                        hasRfiComment: false,
                        hasCitationComment: false,
                        hasFactOnlyComment: false,
                        hasSuggestions: false,
                        sectionStatusName: ""
                    };
                }  

            } else {
                this.setHeight();
            }

            if (this.common.accessToken != null) {
                this.accessToken = this.common.accessToken;
            } else {
                this.getAccessToken();
            }

            common.activateController([this.getSectionQuestions()], '');           
        }

        getSectionQuestions(): ng.IPromise<void> {
            return this.questionService.getSectionQuestions(this.appId || this.appUniqueId, this.section.applicationSectionId || this.section.id)
                .then((questions) => {
                    this.section.questions = questions;
                    this.handleMultiple();
                    this.setHeight();
                    this.processComments();
                    console.log('questions', questions);
                });
        }

        processComments() {
            _.each(this.section.questions,(q) => {
                q.orgComment = q.comments;
            });
        }

        handleMultiple() {
            _.each(this.section.questions, (q) => {
                if (q.type === "Multiple" && (!q.questionResponses || q.questionResponses.length === 0)) {
                    this.addResponse(q);
                }
                else if (q.type === "Checkboxes") {
                    q.leftAnswers = [];
                    q.rightAnswers = [];

                    for (var i = 0; i < q.answers.length; i++) {
                        if (i % 2 === 0) {
                            q.leftAnswers.push(q.answers[i]);
                        } else {
                            q.rightAnswers.push(q.answers[i]);
                        }
                    }
                }

            });
        }

        setHeight() {
            this.$timeout(() => {
                var height = $(".app-modal").height();
                var windowHeight = $(window).height();

                if (height > windowHeight) {
                    $(".app-modal").height(windowHeight - 20);
                }
            }, 500);
            

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

        findSection(sections: services.IApplicationSection[], id: string): services.IApplicationSection {
            var sect = _.find(sections, (s) => {
                return s.id === id;
            });

            if (sect) return sect;

            for (var i = 0; i < sections.length; i++) {
                if (sections[i].children && sections[i].children.length > 0) {
                    sect = this.findSection(sections[i].children, id);

                    if (sect) return sect;
                }
            }

            return null;
        }

        processSection(section: services.IApplicationSection): Eligibility.IApplicationHierarchyData {

            var requirement: Eligibility.IApplicationHierarchyData = {
                partNumber: section.partNumber.toString(),
                name: section.name,
                hasChildren: false,
                id: section.id,
                questions: section.questions,
                uniqueIdentifier: section.uniqueIdentifier,
                status: section.status,
                helpText: section.helpText,
                appUniqueId: ""
            };
            return requirement;
        }

        onOpen(e) {
            var id = e.sender.element[0].id;

            setTimeout(() => {
                    var pos = $('#' + id).offset().top + 40;
                    $(".k-animation-container").css("top", pos);
                },
                200);

        }

        //getSetting(): ng.IPromise<void> {
        //    return this.cacheService.getApplicationSettings()
        //        .then((data) => {
        //            var timer = _.find(data, (setting: services.IApplicationSetting) => {
        //                return setting.name === "Auto Save Timer(In Seconds)";
        //            });

        //            if (timer) {
        //                this.autoSaveTimer = parseInt(timer.value) * 1000;
        //            }

        //            //this.$scope.$watch('vm.section.questions', (newValues, oldValues) => {
        //            //    if (newValues !== oldValues) {
        //            //        if (this.isInit) {
        //            //            this.isInit = false;
        //            //        } else {
        //            //            this.hasChanges();
        //            //        }
        //            //    }
        //            //}, true);
        //        })
        //        .catch(() => {
        //            this.notificationFactory.error("Error getting data. Please contact support.");
        //        });
        //}

        getAccessToken(): ng.IPromise<void> {
            return this.documentService.getAccessToken(this.organization)
                .then((data: services.IAccessToken) => {
                    this.common.accessToken = data;
                    this.accessToken = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to get access to document Library. Please contact support.");
                });
        }

        getapplicationStatusByUniqueId(appId: string): ng.IPromise<void> {
            return this.applicationService.getApplicationStatusByUniqueId(appId)
                .then((data: services.IApplicationStatus) => {
                    if (data != null) {
                        this.appStatus = data.nameForApplicant;
                        this.isStatusRfi = data.nameForApplicant === this.config.applicationSectionStatuses.rfi;
                        if (data.nameForApplicant !== this.config.applicationSectionStatuses.inProgress && data.nameForApplicant !== this.config.applicationSectionStatuses.rfi) {                            
                            this.allowSave = false;
                            this.isReadOnly = true;
                        }

                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error while application status by Id. Please contact support.");
                });
        }
        
        getAccreditationRole(appId: string): ng.IPromise<void> {
            return this.inspectionScheduleService.getAccreditationRole(null, appId)
                .then((data: services.IGenericServiceResponse<services.IAccreditationRole>) => {
                    if (data.item != null) {
                        this.isObserverOrAuditor = (data.item.accreditationRoleName.indexOf("Trainee") !== -1 || data.item.accreditationRoleName.indexOf("Auditor") !== -1);
                    }
                });
        }

        hasChanges(): void {
            //if (!this.savePending) {
            //    this.$timeout(() => {
            //        this.save(true, false);
            //    }, this.autoSaveTimer);
            //    this.savePending = true;
            //}
        }

        getUsers(): ng.IPromise<void> {
            if (this.organization != null && this.organization != undefined && this.organization !== "") {
                return this.organizationService.getOrgUsers(this.organization, false)
                    .then((users: Array<services.IUser>) => {
                        this.users = users;
                    })
                    .catch(() => {
                        this.notificationFactory.error("An error occurred. Please contact support.");
                    });
            } else {
                return this.organizationService.getOrgUsers(this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "", false)
                    .then((users: Array<services.IUser>) => {
                        this.users = users;
                    })
                    .catch(() => {
                        this.notificationFactory.error("An error occurred. Please contact support.");
                    });
            }


        }
        
        onShowRfi(item: services.IQuestion) {
            var sec: Eligibility.IApplicationHierarchyData = {
                partNumber: this.section.applicationSectionUniqueIdentifier,
                name: this.section.name,
                hasChildren: this.section.children && this.section.children.length > 0,
                status: this.section.sectionStatusName,
                helpText: this.section.applicationSectionHelpText,
                children: [],
                questions: [],
                uniqueIdentifier: this.section.applicationSectionUniqueIdentifier || this.section.uniqueIdentifier,
                id: this.section.applicationSectionId || this.section.id,
                statusName: this.section.sectionStatusName,
                appUniqueId: this.appId || this.appUniqueId
            };
            
            for (var i = 0; i < this.section.questions.length; i++) {
                if (this.section.questions[i].id === item.id) {
                    sec.questions.push(this.section.questions[i]);
                }
            }


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
                            section: sec,
                            appUniqueId: this.appId || this.appUniqueId,
                            isUser: this.common.currentUser.role.roleId === 3 || this.accessType === "" || this.accessType === "User",
                            questionId: item.id,
                            organization: this.organization
                        };
                    }

                }
            });

            instance.result.then(() => {
                //this.isRfiSaved = true;
                this.savePending = true;
                this.save(true, false, true);
                
                this.notificationFactory.success("Review saved successfully.");
            }, () => {
            });
        }

        cancel(): void {
            this.localStorageService.remove("section");
            $(".root-sect").css("max-height", "100%");
            $(".root-sect").css("overflow", "auto");
            //$(".application").css("max-height", "100%");
            this.$uibModalInstance.dismiss('cancel');
        }

        checkAnswers(question: services.IQuestion): void {
            for (var i = 0; i < this.section.questions.length; i++) {
                var q = this.section.questions[i];
                q.isHidden = false;
                if (q.hiddenBy && q.hiddenBy.length > 0) {
                    for (var j = 0; j < q.hiddenBy.length; j++) {
                        var hidden = q.hiddenBy[j];
                        if (hidden.questionId === question.id) {
                            var answer = _.find(question.answers, (a: services.IAnswer) => {
                                return hidden.answerId === a.id && a.selected;
                            });
                            if (answer) {
                                q.isHidden = true;
                                break;
                            }
                        } else {
                            var found = _.find(this.section.questions, (q: services.IQuestion) => {
                                return q.id === hidden.hiddenByQuestionId;
                            });

                            if (!found) {
                                found = _.find(this.questions, (q: services.IQuestion) => {
                                    return q.id === hidden.hiddenByQuestionId;
                                });
                            }

                            if (found) {
                                _.each(found.answers, (a: services.IAnswer) => {
                                    if (hidden.answerId === a.id && a.selected) {
                                        q.isHidden = true;
                                    }
                                });
                            }
                        }
                    }
                }
            }
        }

        onRadioSelected(question: services.IQuestion, answer: services.IAnswer): void {
            //if (this.isPostInspection || this.appStatus === "Applicant Response" || this.appStatus === "Post Committee RFI") return;

            if (this.isReadOnly || (this.isStatusRfi && !this.common.isQuestionRFI(question, true, this.appStatus))) {
                return;
            }

            var thisQuestion = _.find(this.section.questions, (q: services.IQuestion) => {
                return q.id === question.id;
            });

            if (thisQuestion) {
                _.each(thisQuestion.answers, (a) => {
                    a.selected = false;
                });

                answer.selected = true;
            }

            this.checkAnswers(question);
        }

        onCheckboxSelected(question: services.IQuestion): void {
            question.answers = [];

            _.each(question.leftAnswers, (a) => {
                question.answers.push(a);
            });

            _.each(question.rightAnswers, (a) => {
                question.answers.push(a);
            });

            this.checkAnswers(question);
        }

        onNext() {
            this.save(false, true);
        }

        save(isAutoSave: boolean, gotoNext: boolean, reloadQuestions?: boolean): void {
            if (!this.savePending && isAutoSave) return;

            if (!reloadQuestions) reloadQuestions = false;

            var errors = false;
            var message = "<p>If you add one value, you must answer the second part of the question.</p>Questions: ";
            var i = 0;

            _.each(this.section.questions, (question) => {
                i++;

                if (question.comments !== question.orgComment ||
                    (question.comments != null && question.orgComment == null) ||
                    (question.comments == null && question.orgComment != null)) {
                    question.commentLastUpdatedBy = this.common.currentUser.firstName + " " + this.common.currentUser.lastName;
                    question.commentDate = new Date();
                }

                var hasOther = "", hasUser = "", hasAnswer = "", hasRange = "", hasDoc = "";
                if (question.type === this.config.questionTypes.multiple) {
                    var other = _.find(question.questionResponses, (response) => {
                        return response.otherText != null && response.otherText !== "";
                    });

                    var user = _.find(question.questionResponses, (response) => {
                        return response.userId != null && response.userId !== "";
                    });

                    var answer = _.find(question.answers, (answer) => {
                        return answer.selected;
                    });

                    var range = _.find(question.questionResponses, (response) => {
                        return response.fromDate != null && response.toDate != null && (response.fromDate !== "" && response.toDate !== "");
                    });

                    var doc = _.find(question.questionResponses, (response) => {
                        return response.document != null && response.document.name !== "";
                    });

                    if (question.questionTypeFlags.textArea || question.questionTypeFlags.textBox || question.questionTypeFlags.date) {
                        hasOther = other ? "Y" : "N";
                    }

                    if (question.questionTypeFlags.peoplePicker) {
                        hasUser = user ? "Y" : "N";
                    }

                    if (question.questionTypeFlags.radioButtons || question.questionTypeFlags.checkboxes) {
                        hasAnswer = answer ? "Y" : "N";
                    }
                    if (question.questionTypeFlags.dateRange) {
                        hasRange = range ? "Y" : "N";
                    }
                    if (question.questionTypeFlags.documentUpload) {
                        hasDoc = doc ? "Y" : "N";
                    }

                    if ((hasOther === "Y" || hasUser === "Y" || hasAnswer === "Y" || hasRange === "Y" || hasDoc === "Y") &&
                        (hasOther === "N" || hasUser === "N" || hasAnswer === "N" || hasRange === "N" || hasDoc === "N")) {
                        errors = true;
                        message += i + " ";
                    }
                }
            });

            //if (this.section.appUniqueId == null || this.section.appUniqueId === "") {
            //    this.section.appUniqueId = this.appUniqueId;
            //}

            if (errors) {
                this.notificationFactory.error(message);
                return;
            }


            //this.localStorageService.set("section", {
            //    partNumber: this.section.partNumber,
            //    name: this.section.name,
            //    hasChildren: this.section.hasChildren,
            //    status: this.section.status,
            //    helpText: this.section.helpText,
            //    children: this.section.children,
            //    items: this.section.items,
            //    questions: this.section.questions,
            //    parentName: this.section.parentName,
            //    uniqueIdentifier: this.section.uniqueIdentifier,
            //    id: this.section.id,
            //    statusName: this.section.statusName,
            //    appUniqueId: this.section.appUniqueId
            //});

            this.isBusy = true;
            this.applicationService.saveApplicationSection(this.organization, this.appType, this.appUniqueId, this.section, false)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                        this.isBusy = false;
                    } else {

                        if (reloadQuestions) {
                            this.common.activateController([this.getSectionQuestions()], '');
                        }

                        this.localStorageService.remove("section");

                        if (!isAutoSave) {
                            var allAnswered = true;
                            _.each(this.section.questions, (question) => {
                                var answered = false;

                                if (question.isHidden) {
                                    answered = true;
                                } else {

                                    if (question.questionResponses && question.questionResponses.length > 0) {
                                        answered = true;
                                    }

                                    if (question.answers && question.answers.length > 0) {
                                        var found = _.find(question.answers, (answer: any) => {
                                            return answer.selected === true;
                                        });

                                        if (found) answered = true;
                                    }
                                }

                                if (!answered) {
                                    allAnswered = false;
                                    return;
                                }
                            });

                            if (!gotoNext) {
                                $(".root-sect").css("max-height", "100%");
                                $(".root-sect").css("overflow", "auto");
                                //$(".application").css("max-height", "100%");
                                this.section.questions = [];
                                this.$uibModalInstance.close({
                                    allAnswered: allAnswered,
                                    section: this.section
                                });

                                this.common.$broadcast('AppSaved', { rowId: this.section.applicationSectionId });
                            } else {
                                if (this.section.sectionStatusName === this.config.applicationSectionStatuses.rfi && this.section.questions && this.section.questions.length > 0) {
                                    var isComplete = true;
                                    var appDate = new Date(this.submittedDate);

                                    for (var i = 0; i < this.section.questions.length; i++) {
                                        var q = this.section.questions[i];

                                        var found = _.find(q.applicationResponseComments, (c) => {
                                            var created = new Date(c.createdDate);

                                            return created > appDate &&
                                                c.commentType.name === this.config.applicationSectionStatuses.rfi;
                                        });

                                        if (!found) {
                                            isComplete = false;
                                            break;
                                        }
                                    }

                                    if (isComplete) {
                                        this.section.sectionStatusName = this.config.applicationSectionStatuses.rfiCompleted;
                                    }
                                }

                                this.section.sectionStatusName = allAnswered ? this.config.applicationSectionStatuses.complete : this.config.applicationSectionStatuses.partial;

                                this.common.$broadcast('AppChanged', { rowId: this.section.applicationSectionId, setFlag: allAnswered });
                                this.common.$broadcast('CheckQuestions', { section: this.section, rowId: this.section.applicationSectionId });
                                
                                var nextSection = this.section.nextSection;

                                $(".root-sect").css("max-height", "100%");
                                $(".root-sect").css("overflow", "auto");
                                this.section.questions = [];
                                this.$uibModalInstance.close({
                                    allAnswered: allAnswered,
                                    section: this.section
                                });

                                this.common.$broadcast('AppSaved', { appUniqueId: this.appUniqueId, rowId: this.section.applicationSectionId, nextSection: nextSection });
                                
                                this.notificationFactory.success("Data saved successfully");
                                this.isBusy = false;
                            }
                            
                        } else {
                            this.notificationFactory.success("Auto Save completed successfully.");
                            this.isBusy = false;
                        }
                    }

                    this.savePending = false;
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.isBusy = false;
                    this.savePending = false;
                });
        }

        showDocumentLibrary(question: services.IQuestion, questionResponse: services.IQuestionResponse): void {
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
                         return question.type == 'Multiple';
                    },
                    isReadOnly: () => {
                        return this.isReadOnly;
                    },
                    organization: () => {
                        return this.organization;
                    },
                    accessToken: () => {
                        return this.accessToken;
                    },
                    appUniqueId: () => {
                        return this.appUniqueId;
                    }
                }
            });

            instance.result.then((documents: Array<services.IDocument>) => {
                if (documents.length > 0) {
                    if (question.type === "Multiple") {
                        questionResponse.document = documents[0];
                        if (documents.length > 1) {
                            for (var i = 1; i < documents.length; i++) {
                                question.questionResponses.push({
                                    document: documents[i]
                                });    
                            }
                        }
                    } else {
                        question.questionResponses = [];

                        _.each(documents, (doc) => {
                            question.questionResponses.push({
                                document: doc
                            });
                        });
                    }
                }               

            }, () => {
            });

        }

        onDocumentDownload(document: services.IDocument): void {
            this.trueVaultService.getBlob(this.accessToken.vaultId, document.requestValues, this.common.currentUser.documentLibraryAccessToken, this.config.factKey)
                .then((data: any) => {
                    var fileType = this.trueVaultService.getFileType(document.name);
                    var file = new Blob([data.response], { type: fileType });
                    saveAs(file, document.name);
                })
                .catch((e) => {
                    this.notificationFactory.error("Cannot get document from True Vault. " + e);
                });
        }

        flagClick(question: services.IQuestion): void {
            question.flag = !question.flag;
        }

        onDisplayText(title: string, text: string) {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/display.html",
                controller: "app.modal.templates.DisplayController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            title: title,
                            text: text
                        }
                    }
                }
            });

            instance.result.then(() => {
                $(".form-group").css("margin-left", "15px");
            });
        }

        onFromDateChanged(q: services.IQuestion, questionResponse: services.IQuestionResponse): void {
            this.minDate = new Date(questionResponse.fromDate);
        }

        onToDateChanged(q: services.IQuestion, questionResponse: services.IQuestionResponse): void {
            this.maxDate = new Date(questionResponse.toDate);
        }

        addResponse(q: services.IQuestion) {
            q.questionResponses.push({});
        }

        onRemoveResponse(question: services.IQuestion, index: number) {
            question.questionResponses.splice(index, 1);
        }

        checkAccess() {
            
            if (this.isObserverOrAuditor == true) {
                this.isReadOnly = true;
                return;
            }

            if ((this.common.currentUser.role.roleName === this.config.roles.inspector && this.accessType === "Inspector") || this.common.currentUser.role.roleName === this.config.roles.factConsultantCoordinator || this.common.currentUser.role.roleName == this.config.roles.factAdministrator) {
                this.isReadOnly = true;
                return;
            }

            if (this.common.isConsultantCoordinator()) {
                this.isReadOnly = true;
                return;
            }

            if (this.appDueDate != null && this.appDueDate !== "" && (this.common.application.rfiDueDate === "" || this.common.application.rfiDueDate == null)) {
                var dueDate = new Date(this.appDueDate);
                var momentDte = moment();
                var today = momentDte.toDate();

                today.setHours(0, 0, 0, 0);

                if (dueDate < today) {
                    this.isReadOnly = true;
                }
            }

            if (this.isUser == true && this.common.application.rfiDueDate !== "") {
                var rfiDate = moment(this.common.application.rfiDueDate).add(1, "days").toDate();
                var now = new Date();
                if (rfiDate < now)
                    this.isReadOnly = true;
            }
        }
        
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.ApplicationController',
        ApplicationController);
}  