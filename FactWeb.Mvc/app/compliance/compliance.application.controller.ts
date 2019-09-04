module app.Compliance {
    'use strict';

    interface IAppQuestions {
        applicationId: number;
        questions: Array<services.IQuestion>;
    }

    class ApplicationController extends app.application.ApplicationBase {
        applicationQuestions: Array<IAppQuestions> = [];
        currentQuestions: Array<services.IQuestion> = [];
        
        appDueDate = "";
        submittedDate = "";
        inspectionDate: Date;
        status = "";
        isDirector = false;
        approvalOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            },
            columns: [
                { field: "firstName", title: "First Name" },
                { field: "lastName", title: "Last Name" },
                { field: "emailAddress", title: "EmailAddress", width: "300px" },
                { field: "isApproved", title: "Approved", template: "<div> #= isApproved ? 'Yes' : 'No' #" },
                { field: "approvalDate", title: "Approved Date", template: `<div class="pull-left"> #= approvalDate ? kendo.toString(kendo.parseDate(approvalDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') : ""   #</div>` }
            ]
        };
        approvals: services.ICompAppApproval[];
        isApproved = true;
        
        static $inject = [
            '$rootScope',
            '$scope',
            '$q',
            '$location',
            '$uibModal',
            '$timeout',
            'modalHelper',
            'config',
            'applicationService',
            'notificationFactory',
            'common'            
        ];
        constructor(
            private $rootScope: ng.IRootScopeService,
            $scope: ng.IScope,
            $q: ng.IQService,
            $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $timeout: ng.ITimeoutService,
            private modalHelper: common.IModalHelper,
            config: IConfig,
            applicationService: services.IApplicationService,
            notificationFactory: blocks.INotificationFactory,
            common: common.ICommonFactory
            ) {        

            super($q, $timeout, $rootScope, $location, null, applicationService, config, common, notificationFactory);

            this.applicationType = "Compliance Application";

            this.checkValues();
                

            var appSaved = this.$rootScope.$on("AppSaved", (data: any, args: any) => {
                this.common.showSplash();
                this.applicationService.getCompApplication(this.compAppUniqueId, args.appUniqueId)
                    .then((data) => {
                        this.isFlagged = data.hasFlag;
                        _.each(this.compApplication.complianceApplicationSites, (site) => {
                            var foundSite: services.IApplicationSiteResponse = _.find(data.complianceApplicationSites, (updatedSite) => {
                                return updatedSite.siteName === site.siteName;
                            });

                            if (foundSite) {
                                site.statusName = foundSite.statusName;

                                _.each(site.appResponses, (application) => {
                                    var foundApplication = _.find(foundSite.appResponses, (app) => {
                                        return application.applicationTypeName === app.applicationTypeName;
                                    });

                                    if (foundApplication) {
                                        application.applicationTypeStatusName = foundApplication.applicationTypeStatusName;

                                        application.applicationSectionResponses = this.processResponseUpdates(application.applicationSectionResponses,
                                                foundApplication.applicationSectionResponses, args.nextSection);
                                    }

                                    if (args.nextSection && foundApplication && foundApplication.applicationUniqueId === args.appUniqueId) {
                                        var nextSect = this.findSect(application.applicationSectionResponses, args.nextSection.applicationSectionId);

                                        while (nextSect != null && !nextSect.isVisible) {
                                            nextSect = this
                                                .findSect(application.applicationSectionResponses,
                                                nextSect.nextSection.applicationSectionId);
                                        }

                                        if (nextSect != null) {
                                            args.nextSection = nextSect;
                                        }
                                    }
                                });
                            }
                        });

                        if (args.nextSection) {
                            var values = {
                                section: () => {
                                    return args.nextSection;
                                },
                                questions: () => {
                                    return null;
                                },
                                appType: () => {
                                    return this.application.applicationTypeName;
                                },
                                accessType: () => {
                                    return this.accessType;
                                },
                                organization: () => {
                                    return this.organization.organizationName;
                                },
                                appDueDate: () => {
                                    return this.appDueDate;
                                },
                                appUniqueId: () => {
                                    return args.appUniqueId;
                                },
                                submittedDate: () => {
                                    return this.application.submittedDate;
                                },
                                site: () => {
                                    return null;
                                },
                                reqId: () => {
                                    return "";
                                },
                                appId: () => {
                                    return "";
                                },
                                appStatus: () => {
                                    return this.application.applicationStatusName;
                                }

                            };

                            this.modalHelper.showModal("/app/modal.templates/application.html", "app.modal.templates.ApplicationController", values)
                                .then((data: any) => {
                                    this.notificationFactory.success("Section saved successfully.");
                                })
                                .catch(() => {
                                    //do this when modal cancelled.
                                });
                        }

                        this.common.hideSplash();
                    })
                    .catch((ex) => {
                        this.notificationFactory.error("Cannot update application details. Please contact support.");
                        this.common.hideSplash();
                    });

                //debugger;
                //_.each(this.compApplication.complianceApplicationSites, (s: services.IComplianceApplicationSite) => {
                //    _.each(s.applications, (a: services.IApplication) => {
                //        a.circle = this.common.getCircleColorForSections(a.sections, true);
                //    });

                //    s.circle = this.common.getCircleForSite(s.applications, true);
                //});

                //var found = _.find(this.compApplication.complianceApplicationSites, (s) => {
                //    return s.statusName !== this.config.applicationSectionStatuses.complete &&
                //        s.statusName !== this.config.applicationSectionStatuses.rfiCompleted;
                //});

                //if (!found) {
                //    this.isComplete = true;
                //} else {
                //    this.isComplete = false;
                //}
            });

            $scope.$on('$destroy', () => {
                appSaved();
            });
        }

        findSect(responses: services.IApplicationSectionResponse[], sectionId: string): services.IApplicationSectionResponse {

            for (var i = 0; i < responses.length; i++) {
                if (responses[i].applicationSectionId === sectionId) {
                    return responses[i];
                }

                if (responses[i].children != null && responses[i].children != undefined && responses[i].children.length > 0) {
                    var sect = this.findSect(responses[i].children, sectionId);

                    if (sect != null) return sect;
                }
            }

            return null;
        }

        init() {
            //this.checkForAccess();
            //onAccessGranted will be called if access is granted.
        }

        onAccessGranted() {
            //this.common.activateController([this.getCompApplication()], 'ApplicationController');
        }

        onApplicationDetailsLoaded() {
            if (this.application && this.application.submittedDateString && this.submittedDate === "")
                this.submittedDate = this.application.submittedDateString;

            if (this.application && this.application.dueDateString && this.appDueDate === "")
                this.appDueDate = this.application.dueDateString;

            if (this.application && this.application.inspectionDate && !this.inspectionDate) {
                this.inspectionDate = this.application.inspectionDate;
            }

            if (this.common.isDirector(this.application.organizationName)) {
                this.getApprovals(false);
            } else {
                this.$rootScope.$on(this.config.events.organizationLoaded, (data: any, args: any) => {
                    if (this.common.isDirector(this.application.organizationName)) {
                        this.getApprovals(false);
                    }
                });
            }
        }

        getApprovals(showSplash?: boolean) {
            if (showSplash == undefined || showSplash == null) showSplash = true;

            if (showSplash) {
                this.common.showSplash();
            }
            
            this.applicationService.getCompAppApprovals(this.compAppUniqueId)
                .then((data) => {
                    console.log('approvals');
                    if (showSplash) this.common.hideSplash();
                    this.approvals = data;

                    var found = _.find(data, (d) => {
                        return d.emailAddress === this.common.currentUser.emailAddress;
                    });

                    if (found) {
                        this.isApproved = found.isApproved;
                    } else {
                        this.isApproved = true;
                    }

                    this.approvalOptions.dataSource.data(data);
                });
        }

        onApplicationSectionsLoaded() {

            console.log(this.compApplication);
        }
        

        onCompApplicationLoaded() {
            if (this.application && this.application.submittedDateString && this.submittedDate === "")
                this.submittedDate = this.application.submittedDateString;

            if (this.application && this.application.dueDateString && this.appDueDate === "")
                this.appDueDate = this.application.dueDateString;

            if (this.application && this.application.inspectionDate && !this.inspectionDate) {
                this.inspectionDate = this.application.inspectionDate;
            }

            this.common.onResize();
        }

        onReloadSection(section: services.IHierarchyData) {
            this.onAccessGranted();
        }
        
        processSection(section: services.IApplicationSection, isRoot: boolean, hasOutcome?: boolean, parentPart?: string, parentName?: string): services.IHierarchyData {
            return null;
            //return this.processSectionForChecklistView(section, isRoot, hasOutcome, parentPart, parentName);            
        }

        //onSelect(e: any): void {
        //    var item: services.IHierarchyData = e.sender.dataItem(e.sender.select());
        //    if (item.questions.length === 0) {
        //        return;
        //    }

        //    this.selectedItem = item;

        //    var instance = this.$uibModal.open({
        //        animation: true,
        //        templateUrl: "/app/modal.templates/application.html",
        //        controller: "app.modal.templates.ApplicationController",
        //        controllerAs: "vm",
        //        size: 'xxl',
        //        backdrop: false,
        //        keyboard: false,
        //        resolve: {
        //            section: () => {
        //                return this.selectedItem;
        //            },
        //            questions: () => {
        //                return this.questions;
        //            }
        //        }
        //    });

        //    instance.result.then(() => {
        //        this.common.showSplash();
        //        this.common.$q.all([this.getCompApplication()]).then(() => {
        //            this.notificationFactory.success("Section saved successfully.");                    
        //            this.common.hideSplash();
        //        });
        //    }, () => {
        //    });
        //}

        submit(): void {
            
            if (!this.isCompComplete(this.application.applicationStatusName === this.config.applicationSectionStatuses.rfi)) {
                this.notificationFactory.error("You must remove all flags and complete the application before submitting.");
                return;
            }
            
            this.common.showSplash();

            this.applicationService.submitCompliance(this.compAppUniqueId)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application submitted successfully.");
                        this.application.applicationStatusName = "For Review"; // replaced by Applied
                        this.application.applicantApplicationStatusName = "Application Submitted";
                        this.application.submittedDate = new Date();
                        //this.getCompApplication();
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save application. Please contact support.");
                    this.common.hideSplash();
                });
        }

        updateStatus(isApproved: boolean): void {
            this.common.showSplash();

            this.applicationService.setComplianceApplicationApprovalStatus(this.compApplication.id, isApproved ? "Approved / Active" : "Reject", "X", "")
                .then((data: app.services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Status updated successfully");
                        this.isApproved = true;

                        for (var i = 0; i < this.approvals.length; i++) {
                            if (this.approvals[i].emailAddress === this.common.currentUser.emailAddress) {
                                this.approvals[i].isApproved = true;
                                break;
                            }
                        }

                        this.approvalOptions.dataSource.data(this.approvals);
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }
        
    }

    angular
        .module('app.compliance')
        .controller('app.compliance.ApplicationController',
        ApplicationController);
}    