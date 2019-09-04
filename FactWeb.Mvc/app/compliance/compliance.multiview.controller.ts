module app.Compliance {
    'use strict';

    interface IAppQuestions {
        applicationId: number;
        questions: Array<services.IQuestion>;
    }

    class MultiViewController {
        application: services.IComplianceApplication;
        sections = new kendo.data.HierarchicalDataSource();
        selectedItem: services.IHierarchyData;
        requirements: Array<services.IHierarchyData>;
        applicationType = "Compliance Application";
        accessType = "";
        tree: any;
        isFlagged = false;
        isComplete = false;
        agreeTerms = false;
        questions: Array<services.IQuestion> = [];
        treeOptions = {
            select: (e) => {
                this.onSelect(e);
            }
        };
        applicationQuestions: Array<IAppQuestions> = [];
        currentQuestions: Array<services.IQuestion> = [];
        compAppId = "";
        submittedDate: Date;
        dueDate: Date;
        inspectionDate: Date;
        status = "";

        static $inject = [
            '$location',
            '$uibModal',
            'config',
            'applicationService',
            'notificationFactory',
            'common'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private config: IConfig,
            private applicationService: services.IApplicationService,
            private notificationFactory: blocks.INotificationFactory,
            private common: app.common.ICommonFactory) {

            this.compAppId = $location.search().c;

            this.getAccess();

        }

        getApplicationById(): ng.IPromise<void> {
            return this.applicationService.getComplianceApplicationServiceType(this.compAppId)
                .then((data: services.IComplianceApplication) => {
                    this.application = data;
                    var sub = _.find(this.application.applications, (a) => {
                        return a.submittedDateString !== "";
                    });

                    if (sub) {
                        this.submittedDate = sub.submittedDate;
                        this.dueDate = sub.dueDate;
                        this.inspectionDate = sub.inspectionDate;
                    }

                    this.status = data.applicationStatus;

                    this.common.$broadcast(this.config.events.coordinatorSet,
                        { coordinator: this.application.coordinator });
                    this.common.$broadcast(this.config.events.orgSet, { organization: this.application.organizationName, orgId: this.application.organizationId });
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data");
                });
        }

        getAccess(): ng.IPromise<void> {
            return this.applicationService.getComplianceApplicationAccess(this.compAppId)
                .then((accessType: string) => {
                    if (accessType != null && accessType !== "") {
                        this.accessType = accessType;
                        this.common.activateController([this.getApplicationById()], '');
                    } else {
                        this.$location.path('/').search({ x: 'u', url: this.$location.url() });
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting access");
                    this.$location.path('/');
                });
        }

        checkQuestionShown(): boolean {


            return true;
        }

        hasQuestionsVisible(section: services.IHierarchyData): boolean {

            if (section.questions == null) return false;

            var found = _.find(section.questions, (question) => {
                return !question.isHidden;
            });

            if (found) {
                return true;
            } else {
                return false;
            }
        }

        shouldBeRemoved(section: services.IHierarchyData): boolean {
            var sect = _.cloneDeep(section);

            if (sect.children != null && sect.children.length > 0) {
                for (var i = 0; i < sect.children.length; i++) {
                    if (this.shouldBeRemoved(sect.children[i])) {
                        sect.children.splice(i, 1);
                        i--;
                    }
                }
            }

            if ((sect.children == null || sect.children.length === 0) && !this.hasQuestionsVisible(sect)) {
                return true;
            }

            return false;
        }

        processSection(section: services.IApplicationSection, isRoot: boolean, parentPart?: string, parentName?: string): services.IHierarchyData {
            var row: services.IHierarchyData = {
                partNumber: section.partNumber.toString(),
                name: section.name,
                hasChildren: false,
                id: section.id,
                questions: section.questions,
                uniqueIdentifier: section.uniqueIdentifier,
                statusName: section.status,
                helpText: section.helpText,
                isVisible: true,
                appUniqueId: section.appUniqueId,
                circle: section.circle,
                circleStatusName: section.circleStatusName
            };

            if (section.questions && section.questions.length > 0) {
                _.each(section.questions, (question: services.IQuestion) => {
                    if (question.type === "Radio Buttons" || question.type === "Checkboxes")
                        this.questions.push(question);

                    if (question.questionResponses == undefined || question.questionResponses == null || question.questionResponses.length === 0) {
                        question.questionResponses = [{}];
                    }
                });
            }

            var anyflags = _.some(row.questions, (question: services.IQuestion) => {
                return question.flag;
            });

            if (anyflags) {
                this.isFlagged = true;
            }

            switch (section.status) {
                case this.config.applicationSectionStatuses.complete:
                    row.status = "glyphicon-ok";
                    break;
                case this.config.applicationSectionStatuses.partial:
                    row.status = "glyphicon-adjust";
                    this.isComplete = false;
                    break;
                case this.config.applicationSectionStatuses.notStarted:
                    row.status = "glyphicon-info-sign";
                    this.isComplete = false;
                    break;
            }

            if (parentPart) {
                row.partNumber = parentPart + "." + row.partNumber;
            }

            if (parentName) {
                row.parentName = parentName;
            }

            if (section.children && section.children.length > 0) {
                row.hasChildren = true;
                row.children = [];
                //row.items = [];
                _.each(section.children, (value: services.IApplicationSection) => {
                    var child = this.processSection(value, false, row.partNumber, row.uniqueIdentifier + ": " + row.name);

                    if (child != null) {
                        row.children.push(child);
                    }

                    //row.items.push(this.processSection(value, row.partNumber, row.partNumber + ": " + row.name));
                });
            }

            if (this.shouldBeRemoved(row)) {
                row.isVisible = false;
            } else {
                if (section.children && section.children.length > 0) {
                    if (section.status !== this.config.applicationSectionStatuses.complete) {
                        var nonComplete = _.find(row.children, (child) => {
                            return child.isVisible && child.statusName !== this.config.applicationSectionStatuses.complete;
                        });

                        if (!nonComplete) {
                            section.status = this.config.applicationSectionStatuses.complete;
                            row.statusName = this.config.applicationSectionStatuses.complete;
                        }
                    }

                     
                } else {
                    //this.setCircle(row);
                }
            }

            if (row.questions && row.questions.length > 0 && ((row.children && row.children.length > 0) || isRoot)) {
                if (!row.children) {
                    row.children = [];
                }

                var newSection = _.clone(row);
                newSection.children = [];

                _.each(newSection.questions, (question) => {
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

                    newSection.statusName = answered ? this.config.applicationSectionStatuses.complete : this.config.applicationSectionStatuses.partial;
                });

                row.children.splice(0, 0, newSection);
            }

            return row;
        }

        checkComplete(reqs: services.IHierarchyData[], isRoot: boolean): boolean {
            if (!reqs) return false;

            for (var i = 0; i < reqs.length; i++) {

                var req = reqs[i];

                if (!req.isVisible) {
                    continue;
                }

                if (req.children && req.children.length > 0) {
                    if (!this.checkComplete(req.children, false)) {
                        return false;
                    }
                }

                if (req.questions && req.questions.length > 0 && !isRoot) {
                    for (var j = 0; j < req.questions.length; j++) {
                        var question = req.questions[j];

                        var answered = true;

                        if (!question.isHidden) {
                            if (question.answers && question.answers.length > 0) {
                                var found = _.find(question.answers, (answer: any) => {
                                    return answer.selected === true;
                                });

                                if (found) answered = true;
                            } else {
                                answered = question.questionResponses && question.questionResponses.length > 0;
                            }
                        }

                        if (!answered || (question.flag && !question.isHidden)) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        onSiteAppView(application: services.IApplication, sites: services.IComplianceApplicationSite) {
            if (application.sections != null && application.sections != undefined) return;

            var promises = [
                this.checkForAccess(application), this.getApp(application)
            ];

            _.each(sites, (appSite: services.IComplianceApplicationSite) => {
                if (appSite.site.siteId !== application.site.siteId) {
                    var app = _.find(appSite.applications, (myApp: services.IApplication) => {
                        return myApp.applicationTypeName === application.applicationTypeName;
                    });

                    if (app) {
                        this.getApp(app);
                    }
                }
            });

            this.common.activateController(promises, '');
        }

        getApp(application: services.IApplication): ng.IPromise<void> {
            return this.applicationService.getAppSections(application.uniqueId)
                .then((items: Array<services.IApplicationSection>) => {
                    console.log('app', items);
                    var data = [];
                    this.isComplete = true;
                    this.currentQuestions = [];
                    _.each(items, (value: services.IApplicationSection) => {
                        data.push(this.processSection(value, true));
                    });
                    application.allQuestions = this.currentQuestions;
                    application.sections = data;
                    this.sections.data(data);
                    console.log('finished app', data);
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        checkForAccess(application: services.IApplication): ng.IPromise<void> {
            return this.applicationService.getApplicationAccess(application.uniqueId)
                .then((accessType: string) => {
                    application.accessType = accessType;
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        onSelect(e: any): void {
            var item: services.IHierarchyData = e.sender.dataItem(e.sender.select());
            if (item.questions.length === 0) {
                return;
            }

            this.selectedItem = item;

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/application.html",
                controller: "app.modal.templates.ApplicationController",
                controllerAs: "vm",
                size: 'xxl',
                backdrop: false,
                keyboard: false,
                resolve: {
                    section: () => {
                        return this.selectedItem;
                    },
                    questions: () => {
                        return this.questions;
                    }
                }
            });

            instance.result.then(() => {
                this.common.showSplash();
                this.common.$q.all([this.getApplicationById()]).then(() => {
                    this.notificationFactory.success("Section saved successfully.");
                    this.common.hideSplash();
                });
            }, () => {
            });
        }

        submit(): void {

            if (!this.checkComplete(this.requirements, true)) {
                this.notificationFactory.error("You must remove all flags and complete the application before submitting.");
                return;
            }

            this.common.showSplash();

            this.applicationService.submitApplication(this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "", "Eligibility Application")
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application submitted successfully.");
                        //this.application.applicationStatusName = "For Review"; // replaced by Applied
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save application. Please contact support.");
                    this.common.hideSplash();
                });
        }

    }

    angular
        .module('app.compliance')
        .controller('app.compliance.MultiViewController',
        MultiViewController);
}  