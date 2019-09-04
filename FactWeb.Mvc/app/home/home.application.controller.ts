module app.Home {

    //Handles Site CheckList View
    class ApplicationController extends app.application.ApplicationBase {
        
        complianceApps: services.ICompApplication[] = [];
        apps: services.IApp[] = [];
        isReadOnly = false;
        approvals: services.ICompAppApproval[];
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
        isApproved = true;

        static $inject = [
            '$scope',
            '$rootScope',
            '$q',
            '$timeout',
            '$rootScope',
            '$location',
            '$uibModal',            
            'applicationService',
            'notificationFactory',            
            'modalHelper',
            'common',
            'config'            
        ];
        constructor(
            $scope: ng.IScope,
            private $rootScope: ng.IRootScopeService,
            $q: ng.IQService,
            $timeout: ng.ITimeoutService,
            rootScope: ng.IRootScopeService,
            location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,            
            applicationService: services.IApplicationService,
            notificationFactory: blocks.INotificationFactory,
            private modalHelper: common.IModalHelper,
            common: app.common.ICommonFactory,
            config: IConfig            
            ) {

            //passing null for documentService as this instance does not need it.
            super($q, $timeout, rootScope, location, null, applicationService, config, common, notificationFactory); 
            
            this.common.$broadcast(this.config.events.hasRfi, { hasRfi: false });
            
            if (this.common.currentUser) this.init();

            this.$rootScope.$on("CircleChanged", (data: any, args: any) => {
                this.application.circle = this.common.getCircleColorForSections(this.application.sections, true);

                this.isComplete = this.application.circle !== this.config.applicationSectionStatuses.complete &&
                    this.application.circle !== this.config.applicationSectionStatuses.rfiCompleted;
            });

            var appSaved = this.$rootScope.$on("AppSaved", (data: any, args: any) => {
                this.common.showSplash();
                this.applicationService.getAppSections(this.location.search().app)
                    .then((items: Array<services.IApplicationSection>) => {
                        console.log('app', items);
                        this.common.applicationSections = items;
                        var nextSectionProcessed = false;

                        this.isFlagged = false;
                        _.each(this.requirements, (r) => {
                            this.processRow(r, items);

                            if (args.nextSection && !nextSectionProcessed) {
                                var sec = _.find(items, (i) => {
                                    return i.id === r.id;
                                });

                                var nextSect;

                                if (sec) {
                                    nextSect = this.findSec(sec, args.nextSection.id);
                                    var secondarySect = this.findAppSection(r, args.nextSection.id);   

                                    if (secondarySect) {
                                        nextSect.nextSection = secondarySect.nextSection;
                                    }
                                } else {
                                    nextSect = this.findAppSection(r, args.nextSection.applicationSectionId || args.nextSection.id);    
                                }

                                console.log('nextSec', nextSect);

                                if (nextSect != null) {
                                    args.nextSection = nextSect;
                                    nextSectionProcessed = true;
                                }

                                if (!args.nextSection.isVisible) {
                                    args.nextSection = this.common.findNextSection(args.nextSection.id, items, args.nextSection.parent);
                                }
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
                                    return this.application.dueDate;
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
                    .catch((e) => {
                        console.log(e);
                        this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                    });
            });

            $scope.$on('$destroy', () => {
                appSaved();
            });

            if (this.application && this.common.isDirector(this.application.organizationName)) {
                this.getApprovals();
            }
            
        }

        findSec(section: services.IApplicationSection, sectionId: string): services.IApplicationSection {
            if (section.id === sectionId) {
                return section;
            }

            if (section.children) {
                for (var i = 0; i < section.children.length; i++) {
                    var sec = this.findSec(section.children[i], sectionId);

                    if (sec != null) {
                        return section.children[i];
                    }
                }
            }

            return null;
        }

        getApprovals() {
            this.common.showSplash();
            this.applicationService.getApplicationApprovals(this.appUniqueId)
                .then((data) => {
                    this.common.hideSplash();
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

        findSect(id: string, sections: services.IApplicationSection[]): services.IApplicationSection {
            for (var i = 0; i < sections.length; i++) {
                if (sections[i].id === id) return sections[i];

                if (sections[i].children != null && sections[i].children != undefined && sections[i].children.length > 0) {
                    var sect = this.findSect(id, sections[i].children);

                    if (sect != null) {
                        return sect;
                    }
                }
            }

            return null;
        }

        processRow(row: services.IHierarchyData, sections: services.IApplicationSection[]) {
            var sect = this.findSect(row.id, sections);

            if (sect != null) {
                row.isVisible = sect.isVisible;
                row.circleStatusName = sect.circleStatusName;
                row.circle = sect.circle;

                var anyflags = _.some(sect.questions, (question: services.IQuestion) => {
                    return question.flag && !question.isHidden;
                });

                if (anyflags) {
                    this.isFlagged = true;
                }

                if (row.children && row.children.length > 0) {
                    _.each(row.children, (c) => {
                        this.processRow(c, sections);
                    });
                }
            }
        }
        
        init() {
            this.checkForAccess();//onAccessGranted will be called if access is granted.

            //these statuses will default to true for checklist view
            this.isComplete = true; 
            this.hasRfiResponse = true;
        }

        onAccessGranted() {
            this.checkValues();
            //this.common.activateController([this.getApplicationDetails(), this.getApplicationSections()], 'ApplicationController');
        }

        onApplicationDetailsLoaded() {
            if (this.application && this.common.isDirector(this.application.organizationName)) {
                this.getApprovals();
            }

            //populate RFIs
            var rfis = [];

            if (this.application.applicationsWithRfis && this.application.applicationsWithRfis.length > 0) {
                _.each(this.application.applicationsWithRfis, (r: services.IApplicationWithRfi) => {
                    if (r.status === this.config.applicationSectionStatuses.rfi) {
                        rfis.push(r);
                    }
                });

                this.application.applicationsWithRfis = rfis;
            }

            if (this.common.currentUser.role.roleName === this.config.roles.factConsultantCoordinator || this.common.currentUser.role.roleName == this.config.roles.factAdministrator) {
                this.isReadOnly = true;
                return;
            }

            if (this.application.dueDate) {
                var due = new Date(this.application.dueDate.toString());
                var momentDte = moment();
                var today = momentDte.toDate();

                today.setHours(0, 0, 0, 0);

                if (due < today) {
                    this.isReadOnly = true;
                }
            }
        }

        onApplicationSectionsLoaded() {
            this.requirements = this.common.setNextSection(this.requirements, this.requirements, null);
        }

        onCompApplicationLoaded() { //not relevant for non-comp applications
        }

        onReloadSection(section: services.IHierarchyData) {
            //this.common.activateController([this.getApplicationSections()], '');
        }

        onShowRfi(item: services.IApplicationWithRfi) {
            this.common.showSplash();

            if (item.complianceAppId != null) {
                var compApp = _.find(this.complianceApps, (c: services.ICompApplication) => {
                    return c.id === item.complianceAppId;
                });

                if (compApp) {
                    var section = this.getSection(item, compApp);
                    this.showRfi(item, section);
                } else {
                    this.applicationService.getCompApplication(item.complianceAppId, null)
                        .then((data: services.ICompApplication) => {
                            _.each(data.complianceApplicationSites, (appSite: services.IComplianceApplicationSite) => {
                                _.each(appSite.applications, (app: services.IApplication) => {
                                    var appSections = [];
                                    _.each(app.sections, (section: services.IApplicationSection) => {
                                        appSections.push(this.processSection(section, true));
                                    });
                                    app.sections = appSections;
                                });
                            });
                            this.complianceApps.push(data);

                            var section = this.getSection(item, data);
                            this.showRfi(item, section);
                        })
                        .catch(() => {
                            this.notificationFactory.error("Error getting compliance application. Please contact support.");
                        });
                }
            } else {
                var app = _.find(this.apps, (c: services.IApp) => {
                    return c.uniqueId === item.uniqueId;
                });

                if (app) {
                    var sect = this.getAppSection(item, app.sections);
                    this.showAppRfi(item, sect);
                } else {
                    this.applicationService.getAppSections(item.uniqueId)
                        .then((items: Array<services.IApplicationSection>) => {
                            var data: services.IHierarchyData[] = [];
                            _.each(items, (value: services.IApplicationSection) => {

                                var row = this.processSection(value, true);

                                if (row != null) {
                                    data.push(row);
                                }
                            });

                            this.apps.push({
                                uniqueId: item.uniqueId,
                                sections: data
                            });

                            var sect = this.getAppSection(item, data);
                            this.showAppRfi(item, sect);

                        })
                        .catch((e) => {
                            this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                        });
                }
            }
        }

        showRfi(item: services.IApplicationWithRfi, section: services.IApplicationSectionResponse) {
            var values = {
                values: () => {
                    return {
                        section: section,
                        organization: this.application.organizationName, 
                        appUniqueId: item.uniqueId,
                        isUser: this.common.isUser()
                    };
                }
            };

            this.modalHelper.showModal("/app/modal.templates/applicationAnswersReview.html", "app.modal.templates.ApplicationAnswersReviewController", values)
                .then(() => {
                    this.notificationFactory.success("Review saved successfully.");
                })
                .catch(() => {
                    //do this when modal cancelled.
                });
        }

        showAppRfi(item: services.IApplicationWithRfi, section: services.IHierarchyData) {
            var values = {
                values: () => {
                    return {
                        section: section,
                        organization: this.application.organizationName, 
                        appUniqueId: item.uniqueId,
                        isUser: this.common.isUser()
                    };
                }
            };

            this.modalHelper.showModal("/app/modal.templates/applicationAnswersReview.html", "app.modal.templates.ApplicationAnswersReviewController", values)
                .then(() => {
                    this.notificationFactory.success("Review saved successfully.");
                })
                .catch(() => {
                    //do this when modal cancelled.
                });
        }

        processSection(section: services.IApplicationSection, isRoot: boolean, hasOutcome?: boolean, parentPart?: string, parentName?: string): services.IHierarchyData {
            return this.processSectionForChecklistView(section, isRoot, hasOutcome, parentPart, parentName);
        }
        
        getAppSection(item: services.IApplicationWithRfi, sections: services.IHierarchyData[]): services.IHierarchyData {
            var section: services.IHierarchyData = null;

            for (var i = 0; i < sections.length; i++) {
                section = this.findAppSection(sections[i], item.applicationSectionId);

                if (section != null) {
                    break;
                }
            }

            return section;
        }

        findSectionByRequirement(section: services.IApplicationSectionResponse, requirementNumber: string): services.IApplicationSectionResponse {
            if (section.uniqueIdentifier === requirementNumber || section.applicationSectionUniqueIdentifier === requirementNumber) {
                return section;
            }

            if (section.children) {
                for (var i = 0; i < section.children.length; i++) {
                    var sec = this.findSectionByRequirement(section.children[i], requirementNumber);

                    if (sec != null) {
                        return section.children[i];
                    }
                }
            }

            return null;
        }

        getSection(item: services.IApplicationWithRfi, app: services.ICompApplication): services.IApplicationSectionResponse {
            var site = _.find(app.complianceApplicationSites, (s: services.IApplicationSiteResponse) => {
                return s.siteId === item.siteId;
            });

            var siteApp = _.find(site.appResponses, (a: services.IAppResponse) => {
                return a.applicationUniqueId === item.uniqueId;
            });

            var section: services.IApplicationSectionResponse = null;

            for (var i = 0; i < siteApp.applicationSectionResponses.length; i++) {
                section = this.findSection(siteApp.applicationSectionResponses[i], item.applicationSectionId);

                if (section != null) {
                    break;
                }
            }

            if (section != null && section.children && section.children.length > 0) {
                for (var i = 0; i < section.children.length; i++) {
                    var newSect = this.findSectionByRequirement(section.children[i], item.requirementNumber);

                    if (newSect != null) {
                        section = newSect;
                        break;
                    }
                }
            }

            return section;
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
                    organization: () => {
                        return this.orgName;
                    },
                    section: () => {
                        return this.selectedItem;
                    },
                    questions: () => {
                        return this.questions;
                    },
                    appStatus: () => {
                        return this.application.applicantApplicationStatusName;
                    }
                }
            });

            instance.result.then(() => {
                this.common.showSplash();
                this.common.$q.all([this.getApplicationDetails()]).then(() => {
                    this.notificationFactory.success("Section saved successfully.");
                    this.common.hideSplash();
                });
            }, () => {
            });
        }

        isAppComplete() {
            if (!this.requirements) {
                return false;
            }


            var found = _.find(this.requirements, (r) => {
                return r.circle !== "Complete" && r.circle !== "RFI Complete" && r.circle !== "RFI Completed" && r.isVisible;
            });

            if (found) {
                return false;
            } else {
                return true;
            }
        }

        submit(): void {

            if (!this.isAppComplete()) {
                this.notificationFactory.error("You must remove all flags and complete the application before submitting.");
                return;
            }

            this.common.showSplash();

            this.applicationService.submitApplication(this.orgName, this.applicationType)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {

                        if (!this.approvals) {
                            this.notificationFactory.success("Application submitted successfully.");
                        } else {
                            this.isApproved = true;
                            for (var i = 0; i < this.approvals.length; i++) {
                                if (this.approvals[i].emailAddress === this.common.currentUser.emailAddress) {
                                    this.approvals[i].isApproved = true;
                                    this.approvals[i].approvalDate = new Date();
                                    break;
                                }
                            }

                            this.approvalOptions.dataSource.data(this.approvals);

                            var found = _.find(this.approvals, (a) => {
                                return !a.isApproved;
                            });    

                            if (!found) {
                                this.notificationFactory.success("Application submitted successfully.");
                                this.application.applicationStatusName = "For Review"; // replaced by Applied
                                this.application.applicantApplicationStatusName = "Application Submitted"; // replaced by Applied
                                this.application.submittedDate = new Date();
                            } else {
                                this.notificationFactory.success("Status updated successfully");
                            }
                        }

                        
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
        .module('app.home')
        .controller('app.home.ApplicationController',
        ApplicationController);
}  