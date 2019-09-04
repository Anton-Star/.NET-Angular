module app.Renewal {
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
        helpText?:string;   
        children?: Array<IApplicationHierarchyData>;
        items?: Array<IApplicationHierarchyData>;
        questions: Array<services.IQuestion>;
        parentName?: string;
        uniqueIdentifier: string;
        id: string;
        statusName?: string;
    }

    class ApplicationController implements IApplication {
        application: services.IApplication;
        sections = new kendo.data.HierarchicalDataSource();
        selectedItem: IApplicationHierarchyData;
        requirements: Array<IApplicationHierarchyData>;
        tree: any;
        isFlagged = false;
        isComplete = false;
        agreeTerms = false;
        questions: Array<services.IQuestion> = [];
        applicationType = "Renewal Application";
        treeOptions = {
            select: (e) => {
                this.onSelect(e);
            }
        };

        static $inject = [
            '$uibModal',
            'config',
            'applicationService',
            'notificationFactory',
            'common'
        ];
        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private config: IConfig,
            private applicationService: services.IApplicationService,
            private notificationFactory: blocks.INotificationFactory,
            private common: app.common.ICommonFactory) {        
            
            this.common.activateController([this.getApp(), this.getApplication()], 'ApplicationController');
        }

        getApplication(): ng.IPromise<void> {
            return this.applicationService.getApplication(this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "", "Renewal Application")
                .then((app: services.IApplication) => {
                    this.application = app;
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        getApp(): ng.IPromise<void> {
            this.isFlagged = false;
            return this.applicationService.getApplicationSections(this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "", "Renewal Application")
                .then((items: Array<services.IApplicationSection>) => {
                    var data = [];
                    this.isComplete = true;
                    _.each(items, (value: services.IApplicationSection) => {
                        data.push(this.processSection(value));
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

        processSection(section: services.IApplicationSection, parentPart?: string, parentName?: string): IApplicationHierarchyData {
            var row: IApplicationHierarchyData = {
                partNumber: section.partNumber.toString(),
                name: section.name,                
                hasChildren: false,
                id: section.id,
                questions: section.questions,
                uniqueIdentifier: section.uniqueIdentifier,
                statusName: section.status,
                helpText: section.helpText
            };

            if (section.questions && section.questions.length > 0) {
                _.each(section.questions, (question: services.IQuestion) => {
                    if (question.type === "Radio Buttons" || question.type === "Checkboxes")
                        this.questions.push(question);

                    if (question.questionResponses == undefined || question.questionResponses == null || question.questionResponses.length === 0) {
                        question.questionResponses = [{ }];
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
                    row.children.push(this.processSection(value, row.uniqueIdentifier, row.uniqueIdentifier + ": " + row.name));
                    row.items.push(this.processSection(value, row.uniqueIdentifier, row.uniqueIdentifier + ": " + row.name));
                });
            }
            
            return row;
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
            
            this.applicationService.submitApplication(this.common.currentUser.organizations != null && this.common.currentUser.organizations.length === 1 ? this.common.currentUser.organizations[0].organization.organizationName : "", "Renewal Application")
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
        .module('app.renewal')
        .controller('app.renewal.ApplicationController',
        ApplicationController);
}  