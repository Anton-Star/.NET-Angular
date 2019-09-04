module app.modal.templates {
    'use strict';

    interface IApplication {
        application: services.IApplication;
        sections: kendo.data.HierarchicalDataSource;
        getApplication(): ng.IPromise<void>;
        onSelect(e: any): void;
    }

    export interface IApplicationHierarchyData {
        partNumber: string;
        name: string;
        hasChildren: boolean;
        status?: string;
        helpText?: string;
        children?: Array<IApplicationHierarchyData>;
        items?: Array<IApplicationHierarchyData>;
        questions: Array<services.IQuestion>;
        parentName?: string;
        uniqueIdentifier: string;
        id: string;
        statusName?: string;        
        isVisible: boolean;
    }

    class FullApplicationController implements IApplication {
        application: services.IApplication;
        sections = new kendo.data.HierarchicalDataSource(); 
        selectedItem: IApplicationHierarchyData;
        requirements: Array<IApplicationHierarchyData>;
        applicationType = "";
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
        accessType: string = "";
        organization: string = "";
        currentVersion: string = "";
        selectedApplicationType: string = "";

        static $inject = [
            '$location',
            '$uibModal',
            '$uibModalInstance',
            'config',
            'applicationService',
            'notificationFactory',
            'common',
            'applicationId'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private config: IConfig,
            private applicationService: services.IApplicationService,
            private notificationFactory: blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private applicationId: string) {
            
            this.checkForAccess();
        }

        checkForAccess(): void {
            this.applicationService.getApplicationAccess(this.applicationId)
                .then((accessType: string) => {
                    if (accessType != null && accessType !== "") {
                        this.accessType = accessType;
                        this.common.activateController([this.getApp(), this.getApplication()], 'ApplicationController');
                    } else {
                        this.$location.path('/').search({ x: 'u', url: this.$location.url() });
                    }
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        getApplication(): ng.IPromise<void> {
            return this.applicationService.getApp(this.applicationId)
                .then((app: services.IApplication) => {
                    
                    this.application = app;
                    this.organization = app.organizationName;
                    this.applicationType = app.applicationTypeName;
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        getApp(): ng.IPromise<void> {
            this.isFlagged = false;
            return this.applicationService.getAppSections(this.applicationId)
                .then((items: Array<services.IApplicationSection>) => {
                    var data = [];
                    this.isComplete = true;
                    _.each(items, (value: services.IApplicationSection) => {
                        data.push(this.processSection(value, true));
                    });
                    this.requirements = data;
                    
                    this.sections.data(data);
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        checkQuestionShown(): boolean {


            return true;
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }

        processSection(section: services.IApplicationSection, isRoot: boolean, parentPart?: string, parentName?: string): IApplicationHierarchyData {
            var row: IApplicationHierarchyData = {
                partNumber: section.partNumber.toString(),
                name: section.name,
                hasChildren: false,
                id: section.id,
                questions: section.questions,
                uniqueIdentifier: section.uniqueIdentifier,
                statusName: section.status,
                helpText: section.helpText,
                isVisible: true                
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
                row.items = [];
                _.each(section.children, (value: services.IApplicationSection) => {
                    row.children.push(this.processSection(value, false,row.partNumber, row.partNumber + ": " + row.name));
                    row.items.push(this.processSection(value, false,row.partNumber, row.partNumber + ": " + row.name));
                });
            }

            if (this.shouldBeRemoved(row)) {
                row.isVisible = false;
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

        shouldBeRemoved(section: IApplicationHierarchyData): boolean {
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

        hasQuestionsVisible(section: IApplicationHierarchyData): boolean {

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

        onSelect(e: any): void {
            var item: IApplicationHierarchyData = e.sender.dataItem(e.sender.select());
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
                this.common.$q.all([this.getApplication()]).then(() => {
                    this.notificationFactory.success("Section saved successfully.");
                    this.common.hideSplash();
                });
            }, () => {
            });
        }

        submit(): void {

            if (this.isFlagged || !this.isComplete) {
                this.notificationFactory.error("You must remove all flags and complete the application before submitting.");
                return;
            }

            this.common.showSplash();

            this.applicationService.submitApplication(this.organization, this.applicationType)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application submitted successfully.");
                        this.application.applicationStatusName = "For Review"; // replaced by Applied
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
        .module('app.modal.templates')
        .controller('app.modal.templates.FullApplicationController',
        FullApplicationController);
}  