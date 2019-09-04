module app.modal.templates {
    'use strict';

    interface ISiteSection {
        site: services.ISite;
        section: Eligibility.IApplicationHierarchyData;
    }

    class MultiviewInspectorController {
        isBusy = false;
        minDate = new Date(1970, 0, 1, 0, 0, 0);
        maxDate = new Date(2050, 0, 1, 0, 0, 0);
        users: Array<services.IUser>;
        isInit = true;
        savePending = false;
        autoSaveTimer = 20000;
        isReadOnly = false;
        isObserverOrAuditor: boolean = false;
        sections: ISiteSection[] = [];
        columnWidth = 12;
        sectionQuestions: services.IQuestion[];
        statusTypes: Array<services.IApplicationResponseStatusItem>;        

        vm = this;

        static $inject = [
            '$scope',
            '$timeout',
            '$uibModal',
            'cacheService',
            'notificationFactory',
            'common',
            'config',
            'currentUser',
            '$uibModalInstance',
            'localStorageService',
            'applicationService',
            'organizationService',
            'inspectionScheduleService',
            'section',
            'questions',
            'appType',
            'accessType',
            'organization',
            'appDueDate',
            'appUniqueId',
            'site',
            'multiSites',
            'trueVaultService'
        ];

        constructor(
            private $scope: ng.IScope,
            private $timeout: ng.ITimeoutService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private currentUser: services.IUser,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private localStorageService: any,
            private applicationService: services.IApplicationService,
            private organizationService: services.IOrganizationService,
            private inspectionScheduleService: services.IInspectionScheduleService,
            private section: Eligibility.IApplicationHierarchyData,
            private questions: Array<services.IQuestion>,
            private appType: string,
            private accessType: string,
            private organization: string,
            private appDueDate: string,
            private appUniqueId: string,
            private site: services.ISite,
            private multiSites: services.IComplianceApplicationSite[],
            private trueVaultService: services.ITrueVaultService) {

            if (this.localStorageService.get("sections") != null) {
                this.section = this.localStorageService.get('sections');
            }

            this.sections.push({
                site: site,
                section: section
            });

            this.sectionQuestions = section.questions;


            _.each(multiSites, (appSite: services.IComplianceApplicationSite) => {
                if (appSite.site.siteId !== site.siteId) {
                    var app = _.find(appSite.applications, (application) => {
                        return application.applicationTypeName === appType;
                    });

                    if (app) {
                        var appSection: Eligibility.IApplicationHierarchyData = null;

                        for (var i = 0; i < app.sections.length; i++) {
                            var found = this.findSection(app.sections[i], section.id);

                            if (found) {
                                appSection = found;
                                break;
                            }
                        }

                        if (appSection != null) {
                            this.sections.push({
                                site: appSite.site,
                                section: appSection
                            });

                            _.each(appSection.questions, (q: services.IQuestion) => {
                                var foundQ = _.find(this.sectionQuestions, (qq: services.IQuestion) => {
                                    return qq.id === q.id;
                                });

                                if (!foundQ) {
                                    this.sectionQuestions.push(q);
                                }
                            });
                        }
                        
                    }
                }
                
            });

            this.sectionQuestions = _.sortBy(this.sectionQuestions, (q) => {
                return q.order;
            });
            
            this.columnWidth = this.columnWidth / this.sections.length;

            this.getUsers();
            this.getAccreditationRole(this.appUniqueId);
            this.getApplicationResponseStatus();


            this.checkAccess();

            if (!this.common.currentUser.isImpersonation) {

            }
            
        }

        findSection(section: Eligibility.IApplicationHierarchyData, sectionId: string): Eligibility.IApplicationHierarchyData {
            if (section.id === sectionId) {
                return section;
            }

            if (section.children && section.children.length > 0) {
                for (var i = 0; i < section.children.length; i++) {
                    var found = this.findSection(section.children[i], sectionId);

                    if (found != null) {
                        return found;
                    }
                }
            }

            return null;
        }

        getSetting(): ng.IPromise<void> {
            return this.cacheService.getApplicationSettings()
                .then((data) => {
                    var timer = _.find(data, (setting: services.IApplicationSetting) => {
                        return setting.name === "Auto Save Timer(In Seconds)";
                    });

                    if (timer) {
                        this.autoSaveTimer = parseInt(timer.value) * 1000;
                    }

                    this.$scope.$watch('vm.section.questions', (newValues, oldValues) => {
                        if (newValues !== oldValues) {
                            if (this.isInit) {
                                this.isInit = false;
                            } else {
                                this.hasChanges();
                            }
                        }
                    }, true);
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data. Please contact support.");
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
            if (!this.savePending) {
                this.$timeout(() => {
                    this.save(true);
                }, this.autoSaveTimer);
                this.savePending = true;
            }
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

        cancel(): void {
            this.localStorageService.remove("sections");
            this.$uibModalInstance.dismiss('cancel');
        }

        checkAnswer(section: Eligibility.IApplicationHierarchyData, question: services.IQuestion) {
            for (var i = 0; i < section.questions.length; i++) {
                var q = section.questions[i];
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

        checkAnswers(index, question: services.IQuestion): void {
            _.each(this.sections, (siteSection: ISiteSection) => {
                _.each(siteSection.section.questions[index].answers, (answer: services.IAnswer) => {
                    var foundAnswer = _.find(question.answers, (a: services.IAnswer) => {
                        return a.id === answer.id;
                    });

                    if (foundAnswer) {
                        answer.selected = foundAnswer.selected;
                    }
                });

                this.checkAnswer(siteSection.section, question);
            });
        }

        onRadioSelected(index, question: services.IQuestion, answer: services.IAnswer): void {
            var thisQuestion = _.find(this.section.questions, (q: services.IQuestion) => {
                return q.id === question.id;
            });

            if (thisQuestion) {
                _.each(thisQuestion.answers, (a) => {
                    a.selected = false;
                });

                answer.selected = true;
            }

            this.checkAnswers(index, question);
        }

        onCheckboxSelected(index, question: services.IQuestion): void {
            this.checkAnswers(index, question);
        }

        onSetOtherText(index, answer, responseIndex) {
            responseIndex = responseIndex || 0;

            _.each(this.sections, (siteSection) => {
                if (!siteSection.section.questions[index].isHidden) {
                    siteSection.section.questions[index].questionResponses[responseIndex].otherText = answer;
                }
            });

            
        }

        save(isAutoSave: boolean): void {
            var errors = false;
            var message = "<p>If you add one value, you must answer the second part of the question.</p>Questions: ";
            var i = 0;
            var hasOther = "", hasUser = "", hasAnswer = "", hasRange = "", hasDoc = "";

            var sections = [];

            _.each(this.sections, (section) => {
                _.each(section.section.questions, (question) => {
                    i++;
                    //if (question.type === this.config.questionTypes.multiple) {
                    //    var other = _.find(question.questionResponses, (response) => {
                    //        return response.otherText != null && response.otherText !== "";
                    //    });

                    //    var user = _.find(question.questionResponses, (response) => {
                    //        return response.userId != null && response.userId !== "";
                    //    });

                    //    var answer = _.find(question.answers, (answer) => {
                    //        return answer.selected;
                    //    });

                    //    var range = _.find(question.questionResponses, (response) => {
                    //        return response.fromDate != null && response.toDate != null && (response.fromDate !== "" && response.toDate !== "");
                    //    });

                    //    var doc = _.find(question.questionResponses, (response) => {
                    //        return response.document != null && response.document.name !== "";
                    //    });

                    //    if (question.questionTypeFlags.textArea || question.questionTypeFlags.textBox || question.questionTypeFlags.date) {
                    //        hasOther = other ? "Y" : "N";
                    //    }

                    //    if (question.questionTypeFlags.peoplePicker) {
                    //        hasUser = user ? "Y" : "N";
                    //    }

                    //    if (question.questionTypeFlags.radioButtons || question.questionTypeFlags.checkboxes) {
                    //        hasAnswer = answer ? "Y" : "N";
                    //    }
                    //    if (question.questionTypeFlags.dateRange) {
                    //        hasRange = range ? "Y" : "N";
                    //    }
                    //    if (question.questionTypeFlags.documentUpload) {
                    //        hasDoc = doc ? "Y" : "N";
                    //    }

                    //    if ((hasOther === "Y" || hasUser === "Y" || hasAnswer === "Y" || hasRange === "Y" || hasDoc === "Y") &&
                    //        (hasOther === "N" || hasUser === "N" || hasAnswer === "N" || hasRange === "N" || hasDoc === "N")) {
                    //        errors = true;
                    //        message += i + " ";
                    //    }
                    //}
                });

                sections.push(section.section);
            });

            if (errors) {
                this.notificationFactory.error(message);
                return;
            }

            this.localStorageService.set("sections", sections);

            this.isBusy = true;
            this.applicationService.saveMultiviewResponseStatus(this.organization, this.appType, sections)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                        this.isBusy = false;
                    } else {

                        this.localStorageService.remove("sections");

                        if (!isAutoSave) {
                            var allAnswered = true;
                            _.each(this.section.questions, (question) => {
                                var answered = false;

                                if (question.isHidden) {
                                    answered = true;
                                } else {
                                    if (question.answers && question.answers.length > 0) {
                                        var found = _.find(question.answers, (answer: any) => {
                                            return answer.selected === true;
                                        });

                                        if (found) answered = true;
                                    } else {
                                        answered = question.questionResponses && question.questionResponses.length > 0;
                                    }
                                }

                                if (!answered) {
                                    allAnswered = false;
                                    return;
                                }
                            });

                            this.$uibModalInstance.close({
                                allAnswered: allAnswered,
                                section: this.section
                            });
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

        showDocumentLibrary(index: number, question: services.IQuestion, questionResponse: services.IQuestionResponse, responseIndex?: number): void {
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
                    }
                }
            });

            instance.result.then((document: services.IDocument) => {
                responseIndex = responseIndex || 0;

                questionResponse.document = document;

                _.each(this.section, (siteSection: ISiteSection) => {
                    siteSection.section.questions[index].questionResponses[responseIndex].document = document;
                });

            }, () => {
            });
        }

        flagClick(question: services.IQuestion): void {
            question.flag = !question.flag;
        }

        onFromDateChanged(index: number, q: services.IQuestion, questionResponse: services.IQuestionResponse, responseIndex?: number): void {
            responseIndex = responseIndex || 0;

            this.minDate = new Date(questionResponse.fromDate);

            _.each(this.sections, (siteSection: ISiteSection) => {
                siteSection.section.questions[index].questionResponses[responseIndex]
                    .fromDate = questionResponse.fromDate;
            });
        }

        onToDateChanged(index: number, q: services.IQuestion, questionResponse: services.IQuestionResponse, responseIndex?: number): void {
            this.maxDate = new Date(questionResponse.toDate);

            _.each(this.sections, (siteSection: ISiteSection) => {
                siteSection.section.questions[index].questionResponses[responseIndex]
                    .toDate = questionResponse.toDate;
            });
        }

        onUserChange(index: number, value: string, responseIndex?: number) {
            responseIndex = responseIndex || 0;

            _.each(this.sections, (siteSection: ISiteSection) => {
                if (!siteSection.section.questions[index].isHidden) {
                    siteSection.section.questions[index].questionResponses[responseIndex]
                        .userId = value;
                }
            });
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

            if (this.common.currentUser.role.roleName === this.config.roles.inspector || this.common.currentUser.role.roleName === this.config.roles.factConsultantCoordinator || this.common.currentUser.role.roleName == this.config.roles.factAdministrator) {
                this.isReadOnly = true;
                return;
            }

            var dueDate = new Date(this.appDueDate);
            var now = moment();
            var tomorrow = now.toDate();

            tomorrow.setHours(0, 0, 0, 0);

            if (dueDate < tomorrow) {
                this.isReadOnly = true;
            }
            else {
                this.isReadOnly = false;
            }
        }

        getApplicationResponseStatus(): ng.IPromise<void> {
            return this.applicationService.getApplicationResponseStatus()
                .then((data: Array<app.services.IApplicationResponseStatusItem>) => {
                    if (data != null) {
                        if (this.common.currentUser.role.roleName.indexOf("Inspector") != -1) {
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

        getSelectedAnswer(answers: Array<services.IAnswer>): services.IAnswer[] {
            var answerText = [];
            _.each(answers, (answer: services.IAnswer) => {
                if (answer.selected === true) {
                    answerText.push(answer.text);
                }
            });
            return answerText;
        }
        
        onStatusSelected(question: services.IQuestion, status: services.IApplicationResponseStatusItem): void {
            question.answerResponseStatusId = status.id;
            question.answerResponseStatusName = status.name;
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.MultiviewInspectorController',
        MultiviewInspectorController);
} 