module app.application {
    'use strict';

    interface IResponse {
        allAnswered: boolean;
        section: any;
    }

    class StatusViewController extends app.application.ApplicationBase {
        //All application specific sections = this.application.sections from base
        applicationSectionRootItems: Array<services.ISectionRootItem>; //Application specific root items and requirement sections
        filteredItems: Array<services.ISectionRootItem> = []; //Actual Display (handles showAll use case)
        filters: Array<services.IFilter> = [];
        showAll = true;
        showAttachments = false;
        showRfi = false;
        showCitation = false;
        isApplicant = false;
        isInspectorOrStaff = false;
        sites: Array<services.IApplication>;        
        selectedSite = "";
        requirementTemp: services.IApplicationSectionResponse;
        section: string;
        app: services.IAppResponse;
        siteResponse: services.IApplicationSiteResponse;

        static $inject = [
            '$scope',
            '$q',
            '$timeout',
            '$rootScope',
            '$location',
            '$uibModal',
            'applicationService',
            'notificationFactory',
            'modalHelper',
            'config',
            'common'
        ];
        constructor(
            $scope: ng.IScope,
            $q: ng.IQService,
            $timeout: ng.ITimeoutService,
            $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            applicationService: app.services.IApplicationService,
            notificationFactory: app.blocks.INotificationFactory,
            private modalHelper: common.IModalHelper,
            config: IConfig,
            common: app.common.ICommonFactory) {

            super($q, $timeout, $rootScope, $location, null, applicationService, config, common, notificationFactory);

            this.section = $location.search().sec;

            this.checkValues();
            if (this.common.currentUser)
                this.init();

            var appSaved = $rootScope.$on("AppSaved", (data: any, args: any) => {
                console.log('app saved');
                this.reload(args);

            });

            $scope.$on('$destroy', () => {
                appSaved();
            });

            $rootScope.$watch('vm.common.inspectorHasAccess', () => {
                this.isInspectorOrStaff = this.common.inspectorHasAccess || this.common.isFact();
            });
        }

        reload(args?: any) {
            this.common.showSplash();
            //debugger;
            if (this.compAppUniqueId != undefined && this.compAppUniqueId != null && this.compAppUniqueId !== "") {
                this.applicationService.getCompApplication(this.compAppUniqueId, this.appUniqueId)
                    .then((data) => {
                        if (this.common.compApp.complianceApplicationSites.length > 1) {
                            for (var i = 0; i < this.common.compApp.complianceApplicationSites.length; i++) {
                                if (this.common.compApp.complianceApplicationSites[i].siteId ===
                                    data.complianceApplicationSites[0].siteId) {
                                    this.common.compApp.complianceApplicationSites[i] = data
                                        .complianceApplicationSites[0];
                                    break;
                                }
                            }
                        } else {
                            this.common.compApp = data;
                        }

                        this.compApplication = this.common.compApp;

                        this.processSites(false);

                        this.processComp(data, args);

                        //_.each(this.filters, (f) => {
                        //    f.isChecked = false;
                        //});

                        this.onFilter(null);

                        this.showAll = true;

                        this.common.hideSplash();
                    })
                    .catch((ex) => {
                        this.notificationFactory.error("Cannot update application details. Please contact support.");
                        this.common.hideSplash();
                    });
            } else {
                this.applicationService.getAppSections(this.location.search().app)
                    .then((items: Array<services.IApplicationSection>) => {
                        this.common.applicationSections = items;
                        this.loadApplicationsSections();

                        //_.each(this.filters, (f) => {
                        //    f.isChecked = false;
                        //});

                        this.onFilter(null);

                        this.common.hideSplash();
                    })
                    .catch((e) => {
                        console.log(e);
                        this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                    });
            }
        }

        processApp(items: services.IApplicationSection[], args?: any) {
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

            //this.getStatusViewNonCompliance();

            if (args && args.nextSection) {
                if (this.isInspectorOrStaff) {
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
                                    section: args.nextSection,
                                    appUniqueId: this.appUniqueId,
                                    isUser: this.common.isUser(),
                                    organization: this.compApplication ? this.compApplication.organizationName : this.application.organizationName
                                };
                            }
                        }
                    });

                    instance.result.then(() => {
                        this.notificationFactory.success("Review saved successfully.");
                    }, () => {
                    });
                } else {
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
                
            }
        }

        processComp(data: services.ICompApplication, args?: any) {
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
                                foundApplication.applicationSectionResponses, args ? args.nextSection : null);
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

            //this.getStatusView();

            if (args && args.nextSection) {

                if (this.isInspectorOrStaff) {
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
                                    section: args.nextSection,
                                    appUniqueId: this.appUniqueId,
                                    isUser: this.common.isUser(),
                                    organization: this.compApplication ? this.compApplication.organizationName : this.application.organizationName
                                };
                            }
                        }
                    });

                    instance.result.then(() => {
                        this.notificationFactory.success("Review saved successfully.");
                    }, () => {
                    });
                } else {
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
                            return this.common.application.dueDateString;
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
                
            }
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

        findAppSect(id: string, sections: services.IApplicationSection[]): services.IApplicationSection {
            for (var i = 0; i < sections.length; i++) {
                if (sections[i].id === id) return sections[i];

                if (sections[i].children != null && sections[i].children != undefined && sections[i].children.length > 0) {
                    var sect = this.findAppSect(id, sections[i].children);

                    if (sect != null) {
                        return sect;
                    }
                }
            }

            return null;
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

        processRow(row: services.IHierarchyData, sections: services.IApplicationSection[]) {
            var sect = this.findAppSect(row.id, sections);

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
            
            if (this.common.isUser()) { this.isApplicant = true; }
            else { this.isInspectorOrStaff = this.common.inspectorHasAccess || this.common.isFact(); }



            //var promises = [];
            //if (this.isComplianceApplication) {
            //    promises.push(this.getCompApplication());
            //}
            //else {
            //    promises.push(this.getApplicationDetails(), this.getApplicationSections());                
            //}

            //this.common.activateController(promises, 'homeController');
        }

        onAccessGranted() {
        }

        onApplicationDetailsLoaded() {
            //this.application.sections = this.requirements;
            //this.filterRequirementOnlySectionsNonCompliance();
        }

        onApplicationSectionsLoaded() {
            this.application.sections = this.requirements;
            this.getStatusViewNonCompliance();
        }

        onCompApplicationLoaded() {
            this.processSites(true);
        }

        processSites(setSite: boolean) {
            if (setSite) {
                this.selectedSite = this.appUniqueId; //set the site dropdown value
                if (this.compApplication.complianceApplicationSites &&
                    this.compApplication.complianceApplicationSites.length > 0) {
                    if (this.compApplication.complianceApplicationSites[0].appResponses &&
                        this.compApplication.complianceApplicationSites[0].appResponses.length > 0) {
                        this.app = this.compApplication.complianceApplicationSites[0].appResponses[0];
                        this.selectedSite = this.compApplication.complianceApplicationSites[0].appResponses[0]
                            .applicationUniqueId;
                        this.appUniqueId = this.selectedSite;
                    }
                }
            } else {
                if (this.compApplication.complianceApplicationSites &&
                    this.compApplication.complianceApplicationSites.length > 0) {
                    if (this.compApplication.complianceApplicationSites[0].appResponses &&
                        this.compApplication.complianceApplicationSites[0].appResponses.length > 0) {
                        var f = _.find(this.compApplication.complianceApplicationSites, (s: services.IApplicationSiteResponse) => {
                            return s.appResponses[0].applicationUniqueId === this.selectedSite;
                        });

                        if (f) {
                            this.app = f.appResponses[0];
                        }
                    }
                }
            }

            console.log(this.compApplication);

            this.getStatusView();
        }
        
        onSiteChange(): void {
            if (this.selectedSite != "") {
                this.appUniqueId = this.selectedSite;
                this.filters = [];
                var selectedSite = _.find(this.compApplication.complianceApplicationSites, (site) => {
                    return site.appResponses[0].applicationUniqueId == this.appUniqueId;
                });
                this.siteResponse = selectedSite
                this.app = selectedSite.appResponses[0];
                this.getStatusView();
            }
        }

        onReloadSection(section: services.IHierarchyData) {
            this.init();
        }

        //Actual Display = this.filteredItems (handles showAll use case). Application specific root items and requirement sections = this.applicationSectionRootItem. All application specific sections = this.application.sections
        getStatusViewNonCompliance() {
            //this.common.$broadcast(this.config.events.coordinatorSet, { coordinator: this.application.coordinator });

            //this functions populates this.applicationSectionRootItems from this.application.sections

            this.filterRequirementOnlySectionsNonCompliance(null, null, null);

            var hasAttachments = false;
            var hasCitations = false;
            var hasFACTOnlyComments = false;
            var hasSuggestions = false;
            var hasRfi = false;
            var isFlagged = false;

            var sections = this.applicationSectionRootItems;

            _.each(sections, (root) => {
                _.each(root.items, (item) => {

                    if (item.hasAttachments) {
                        hasAttachments = true;
                    }

                    if (item.hasCitationNotes) {
                        hasCitations = true;
                    }

                    if (item.hasRFIComments) {
                        hasRfi = true;
                    }

                    if (item.hasFACTOnlyComments) {
                        hasFACTOnlyComments = true;
                    }

                    if (item.hasSuggestions && !this.common.isUser()) {
                        hasSuggestions = true;
                    }

                    if (item.isFlag) {
                        isFlagged = true;
                    }


                    if (this.isApplicant) {

                        switch (item.statusName) {
                            case this.config.applicationSectionStatuses.forReview:
                            case this.config.applicationSectionStatuses.reviewed:
                            case this.config.applicationSectionStatuses.compliant:
                            case this.config.applicationSectionStatuses.notCompliant:
                            case this.config.applicationSectionStatuses.notApplicable:
                            case this.config.applicationSectionStatuses.noResponseRequested:
                                item.statusName = this.config.applicationSectionStatuses.complete;
                        }
                    }

                    //make the following two adjustments for CSS application only. TODO: Find a more elegant way
                    if (item.statusName == this.config.applicationSectionStatuses.notApplicable) item.statusName = "Not Applicable";
                    if (item.statusName == this.config.applicationSectionStatuses.noResponseRequested) item.statusName = "No Response";
                    if (item.statusName === this.config.applicationSectionStatuses.notStarted) item.statusName = "Not Started";


                    this.checkField(item.statusName);
                });
            });

            this.filters = _.sortBy(this.filters, (filter) => {
                return filter;
            });

            if (hasAttachments) {
                this.checkField("Has Attachments");
            }

            if (hasCitations && this.common.application.hasOutcome === true) {
                this.checkField("Has Citation Notes");
            }

            if (hasFACTOnlyComments && !this.isApplicant) {
                this.checkField("Has FACT Only Comments");
            }

            if (hasSuggestions && !this.isApplicant) {
                this.checkField("Has Suggestions");
            }

            if (hasRfi) {
                if (this.isApplicant) {
                    if ((this.compApplication && this.compApplication.applicationStatus !== this.config.applicationStatuses.inProgress) || (this.application && this.application.applicantApplicationStatusName !== this.config.applicationStatuses.inProgress))
                        this.checkField("Has RFI Comments");
                }
                else this.checkField("Has RFI Comments");
            }

            if (isFlagged) {
                this.checkField("Has Flag");
            } else {
                for (var j = 0; j < this.filters.length; j++) {
                    if (this.filters[j].filterName === "Has Flag") {
                        this.filters.splice(j, 1);
                        break;
                    }
                }
            }

            var rfi = _.find(this.filters, (f) => {
                return f.filterName === this.config.applicationSectionStatuses.rfi;
            });

            if (this.compApplication && rfi && this.compApplication.applicationStatus !== this.config.applicationStatuses.rfiInProgress) {
                this.common.$broadcast(this.config.events.hasRfi, { hasRfi: true });
            } else {
                this.common.$broadcast(this.config.events.hasRfi, { hasRfi: false });
            }

            this.filteredItems = this.applicationSectionRootItems;

            if (this.section && this.section !== "") {
                for (var i = 0; i < this.filteredItems.length; i++) {
                    if (this.section.indexOf(this.filteredItems[i].uniqueIdentifier) > -1) {
                        var item = _.find(this.filteredItems[i].items, (item) => {
                            return item.uniqueIdentifier === this.section;
                        });

                        if (item) {
                            this.onRequirementClick(item);
                            break;
                        }
                    }
                }

                this.section = "";
            }
        }

        filterRequirementOnlySectionsNonCompliance(changedSectionId: string, isFlagged: boolean, hasAttachments: boolean) {
            this.applicationSectionRootItems = [];
            var isRoot = false;
            _.each(this.application.sections, (section) => {
                if (changedSectionId) {
                    if (section.id === changedSectionId) {
                        section.hasFlags = isFlagged;
                        
                    }
                }
                var sectionRootItem: services.ISectionRootItem = { applicationSectionId: section.id, name: section.name, uniqueIdentifier: section.uniqueIdentifier, items: [] };
                sectionRootItem.items = [];//TODO:

                if (section.children && section.children.length) {
                    _.each(section.children,
                        (child) => {
                            this.getSectionItemStatus2NonCompliance(child, sectionRootItem, changedSectionId, isFlagged, hasAttachments);
                        });
                } else {
                    this.getSectionItemStatusNonCompliance(section, sectionRootItem, changedSectionId, isFlagged, hasAttachments);
                }

                this.applicationSectionRootItems.push(sectionRootItem);
            });
        }

        getSectionItemStatusNonCompliance(section: services.IHierarchyData, sectionRootItem: services.ISectionRootItem, changedSectionId: string, isFlagged: boolean, hasAttachments: boolean) {

            var sectionItemStatus = {
                applicationSectionId: section.id,
                hasAttachments: false,
                hasCitationNotes: false,
                hasRFIComments: false,
                hasFACTOnlyComments: false,
                hasSuggestions: false,
                isFlag: false,
                statusName: section.circleStatusName,
                uniqueIdentifier: section.uniqueIdentifier
            }

            var visibleQuestions = section.questions.filter(q => q.isHidden != true);
            _.each(visibleQuestions, (q) => {
                if (q.questionResponses && q.questionResponses.length > 0) {
                    if (q.questionResponses[0].document) sectionItemStatus.hasAttachments = true
                }
                if (q.flag) sectionItemStatus.isFlag = true;

                if (q.responseCommentsRFI && q.responseCommentsRFI.length > 0) sectionItemStatus.hasRFIComments = true;
                if (q.responseCommentsCitation && q.responseCommentsCitation.length > 0) sectionItemStatus.hasCitationNotes = true;
                if (q.responseCommentsFactOnly && q.responseCommentsFactOnly.length > 0) sectionItemStatus.hasFACTOnlyComments = true;
                if (q.responseCommentsSuggestion && q.responseCommentsSuggestion.length > 0 && !this.common.isUser()) sectionItemStatus.hasSuggestions = true;

                if (changedSectionId) {
                    if (section.id === changedSectionId) {
                        sectionItemStatus.isFlag = isFlagged;
                        sectionItemStatus.hasAttachments = hasAttachments;
                    }
                }

            });

            sectionRootItem.items.push(sectionItemStatus);
        }

        getSectionItemStatus2NonCompliance(section: services.IHierarchyData, sectionRootItem: services.ISectionRootItem, changedSectionId: string, isFlagged: boolean, hasAttachments: boolean) {
            if (!section.isVisible) return;

            if (section.children && section.children.length > 0) {
                _.each(section.children, (child) => {
                    this.getSectionItemStatus2NonCompliance(child, sectionRootItem, changedSectionId, isFlagged, hasAttachments);
                });
            } else {
                this.getSectionItemStatusNonCompliance(section, sectionRootItem, changedSectionId, isFlagged, hasAttachments);
            }
        }

        getStatusView() {
            //this.common.$broadcast(this.config.events.coordinatorSet, { coordinator: this.application.coordinator });

            //this functions populates this.applicationSectionRootItems from this.application.sections

            this.filterRequirementOnlySections(null, null, null);

            var hasAttachments = false;
            var hasCitations = false;
            var hasFACTOnlyComments = false;
            var hasSuggestions = false;
            var hasRfi = false;
            var isFlagged = false;

            var sections = this.applicationSectionRootItems;

            _.each(sections, (root) => {
                _.each(root.items, (item) => {

                    if (item.hasAttachments) {
                        hasAttachments = true;
                    }

                    if (item.hasCitationNotes) {
                        hasCitations = true;
                    }

                    if (item.hasRFIComments) {
                        hasRfi = true;
                    }

                    if (item.hasFACTOnlyComments) {
                        hasFACTOnlyComments = true;
                    }

                    if (item.isFlag) {
                        isFlagged = true;
                    }

                    if (item.hasSuggestions && !this.common.isUser()) {
                        hasSuggestions = true;
                    }


                    if (this.isApplicant) {

                        switch (item.statusName) {
                            case this.config.applicationSectionStatuses.forReview:
                            case this.config.applicationSectionStatuses.reviewed:
                            case this.config.applicationSectionStatuses.compliant:
                            case this.config.applicationSectionStatuses.notCompliant:
                            case this.config.applicationSectionStatuses.notApplicable:
                            case this.config.applicationSectionStatuses.noResponseRequested:
                                item.statusName = this.config.applicationSectionStatuses.complete;
                        }
                    }

                    //make the following two adjustments for CSS application only. TODO: Find a more elegant way
                    if (item.statusName == this.config.applicationSectionStatuses.notApplicable) item.statusName = "Not Applicable";
                    if (item.statusName == this.config.applicationSectionStatuses.noResponseRequested) item.statusName = "No Response";
                    if (item.statusName === this.config.applicationSectionStatuses.notStarted) item.statusName = "Not Started";


                    this.checkField(item.statusName);
                });
            });

            this.filters = _.sortBy(this.filters, (filter) => {
                return filter;
            });

            if (hasAttachments) {
                this.checkField("Has Attachments");
            }

            if (hasCitations && this.common.application.hasOutcome === true) {
                this.checkField("Has Citation Notes");
            }

            if (hasFACTOnlyComments && !this.isApplicant) {
                this.checkField("Has FACT Only Comments");
            }

            if (hasSuggestions && !this.common.isUser()) {
                this.checkField("Has Suggestions");
            }

            if (hasRfi) {
                if (this.isApplicant) {
                    if (this.compApplication.applicationStatus !== this.config.applicationStatuses.inProgress)
                        this.checkField("Has RFI Comments");
                }
                else this.checkField("Has RFI Comments");
            }

            if (isFlagged) {
                this.checkField("Has Flag");
            } else {
                for (var j = 0; j < this.filters.length; j++) {
                    if (this.filters[j].filterName === "Has Flag") {
                        this.filters.splice(j, 1);
                        break;
                    }
                }
            }

            var rfi = _.find(this.filters, (f) => {
                return f.filterName === this.config.applicationSectionStatuses.rfi;
            });

            if (this.compApplication && rfi && this.compApplication.applicationStatus !== this.config.applicationStatuses.rfiInProgress) {
                this.common.$broadcast(this.config.events.hasRfi, { hasRfi: true });
            } else {
                this.common.$broadcast(this.config.events.hasRfi, { hasRfi: false });
            }

            this.filteredItems = this.applicationSectionRootItems;

            if (this.section && this.section !== "") {
                for (var i = 0; i < this.filteredItems.length; i++) {
                    if (this.section.indexOf(this.filteredItems[i].uniqueIdentifier) > -1) {
                        var item = _.find(this.filteredItems[i].items, (item) => {
                            return item.uniqueIdentifier === this.section;
                        });

                        if (item) {
                            this.onRequirementClick(item);
                            break;
                        }
                    }
                }

                this.section = "";
            }
        }

        //get sections with associated questions (a.k.a requirements)
        filterRequirementOnlySections(changedSectionId: string, isFlagged: boolean, hasAttachments: boolean) {
            this.applicationSectionRootItems = [];
            var isRoot = false;
            _.each(this.app.applicationSectionResponses, (section) => {
                var sectionRootItem: services.ISectionRootItem = { applicationSectionId: section.applicationSectionId, name: section.applicationSectionName, uniqueIdentifier: section.applicationSectionUniqueIdentifier, items: [] };
                sectionRootItem.items = [];//TODO:

                if (section.children && section.children.length) {
                    _.each(section.children,
                        (child) => {
                            this.getSectionItemStatus2(child, sectionRootItem, changedSectionId, isFlagged, hasAttachments);
                        });
                } else {
                    this.getSectionItemStatus(section, sectionRootItem, changedSectionId, isFlagged, hasAttachments);  
                }

                this.applicationSectionRootItems.push(sectionRootItem);
            });
        }

        processSection(section: services.IApplicationSection, isRoot: boolean, hasOutcome?: boolean, parentPart?: string, parentName?: string): services.IHierarchyData {
            if (this.isInspectorOrStaff) {
                return this.processSectionForReviewerView(section, isRoot, hasOutcome, parentPart, parentName);
            } else {
                return this.processSectionForChecklistView(section, isRoot, hasOutcome, parentPart, parentName);
            }
        }

        getSectionItemStatus(section: services.IApplicationSectionResponse, sectionRootItem: services.ISectionRootItem, changedSectionId: string, isFlagged: boolean, hasAttachments: boolean) {

            var sectionItemStatus = {
                applicationSectionId: section.applicationSectionId,
                hasAttachments: false,
                hasCitationNotes: section.hasCitationComment,
                hasRFIComments: section.hasRfiComment,
                hasFACTOnlyComments: section.hasFactOnlyComment,
                hasSuggestions: section.hasSuggestions,
                isFlag: section.hasFlag,
                statusName: section.sectionStatusName,
                uniqueIdentifier: section.applicationSectionUniqueIdentifier,
            }

            sectionRootItem.items.push(sectionItemStatus);            
        }

        getSectionItemStatus2(section: services.IApplicationSectionResponse, sectionRootItem: services.ISectionRootItem, changedSectionId: string, isFlagged: boolean, hasAttachments: boolean) {
            if (!section.isVisible) return;
            
            if (section.children && section.children.length > 0) {
                _.each(section.children, (child) => {
                    this.getSectionItemStatus2(child, sectionRootItem, changedSectionId, isFlagged, hasAttachments);
                });
            } else {
                this.getSectionItemStatus(section, sectionRootItem, changedSectionId, isFlagged, hasAttachments);  
            }
        }
        
        onRequirementClick(item) {
            if (this.isInspectorOrStaff) {
                this.openReviewerViewForRequirement(item);                
            } else {
                this.openApplicantViewForRequirement(item);
            }
        }
        //open the reviewer view
        openReviewerViewForRequirement(item) {
            var req;

            if (this.app) {
                req = this.findSectionOnly(this.app.applicationSectionResponses, item.uniqueIdentifier);
            } else {
                req = this.findSectionFromApp(this.common.applicationSections, item.uniqueIdentifier);
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
                            section: req,
                            appUniqueId: this.appUniqueId,
                            isUser: this.common.isUser(),
                            organization: this.compApplication ? this.compApplication.organizationName : this.application.organizationName
                        };
                    }
                }
            });

            instance.result.then(() => {
                this.notificationFactory.success("Review saved successfully.");
            }, () => {
            });
        }
        //open the applicant view
        openApplicantViewForRequirement(item) {
            var req;

            if (this.app) {
                req = this.findSectionOnly(this.app.applicationSectionResponses, item.uniqueIdentifier);
            } else {
                req = this.findSectionFromApp(this.common.applicationSections, item.uniqueIdentifier);
            }
            
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/application.html",
                controller: "app.modal.templates.ApplicationController",
                controllerAs: "vm",
                size: 'xxl',
                backdrop: false,
                keyboard: false,
                resolve: {
                    appId: () => {
                        return "";
                    },
                    reqId: () => {
                        return "";
                    },
                    section: () => {
                        return req;
                    },
                    questions: () => {
                        return req.questions;
                    },
                    appType: () => {
                        return this.app ? this.app.applicationTypeName : this.application.applicationTypeName;
                    },
                    accessType: () => {
                        return "";
                    },
                    organization: () => {
                        return this.compApplication ? this.compApplication.organizationName : this.application.organizationName;
                    },
                    appDueDate: () => {
                        return this.common.application.dueDateString;
                    },
                    appUniqueId: () => {
                        return this.appUniqueId;
                    },
                    submittedDate: () => {
                        return this.common.application.submittedDate;
                    },
                    site: () => {
                        if (!this.siteResponse) return this.common.application.site;

                        var site: services.ISite = {
                            siteId: this.siteResponse.siteId,
                            siteName: this.siteResponse.siteName
                        };
                        return site;
                    },
                    appStatus: () => {
                        return this.compApplication != null && this.compApplication != undefined
                            ? this.compApplication.applicationStatus
                            : this.common.application.applicantApplicationStatusName;
                    }
                }
            });

            instance.result.then((response: IResponse) => {
                if (response.section) {
                    console.log('section', response.section);

                    var hasAttachments = false;
                    var isFlagged = false;

                    _.each(response.section.questions, (q) => {
                        if (q.flag === true) {
                            isFlagged = true;
                        }

                        if (q.questionResponses && q.questionResponses.length > 0 && q.questionResponses[0].document) {
                            hasAttachments = true;
                        }
                    });

                    if (this.compAppUniqueId === undefined || this.compAppUniqueId == null || this.compAppUniqueId !== '') {
                        this
                            .filterRequirementOnlySectionsNonCompliance(response.section.applicationSectionId,
                            isFlagged,
                            hasAttachments);

                        this.getStatusViewNonCompliance();
                    } else {
                        this.filterRequirementOnlySections(response.section.applicationSectionId,
                            isFlagged,
                            hasAttachments);  

                        this.getStatusView();
                    }

                }
            }, () => {
            });
        }
        
        onShowAll() {
            this.showAll = true;
            this.filteredItems = this.applicationSectionRootItems;

            _.each(this.filters, (filter) => {
                filter.isChecked = false;
            });
        }

        filterData() {
            var filtered: Array<services.ISectionRootItem> = [];
            _.each(this.applicationSectionRootItems, (root) => {
                _.each(root.items, (item) => {
                    var found = _.find(this.filters, (filter) => {
                        switch (filter.filterName.trim()) {
                            case "Has Attachments":
                                return filter.isChecked ? item.hasAttachments : false;
                            case "Has Citation Notes":
                                return filter.isChecked ? item.hasCitationNotes : false;
                            case "Has FACT Only Comments":
                                return filter.isChecked ? item.hasFACTOnlyComments : false;
                            case "Has RFI Comments":
                                return filter.isChecked ? item.hasRFIComments : false;
                            case "Has Flag":
                                return filter.isChecked ? item.isFlag : false;
                            case "Has Suggestions":
                                return filter.isChecked ? item.hasSuggestions : false;
                            default:
                                return (filter.filterName.replace(" ", "") === item.statusName.trim() || (filter.filterName === item.statusName.trim())) && filter.isChecked;
                        }

                    });

                    if (found) {
                        var foundRoot = _.find(filtered, (filteredRoot) => {
                            return filteredRoot.applicationSectionId === root.applicationSectionId;
                        });

                        if (!foundRoot) {
                            var newRoot: services.ISectionRootItem = {
                                applicationSectionId: root.applicationSectionId,
                                name: root.name,
                                uniqueIdentifier: root.uniqueIdentifier,
                                items: [
                                    item
                                ]
                            }

                            filtered.push(newRoot);
                        } else {
                            foundRoot.items.push(item);
                        }
                    }
                });
            });

            this.filteredItems = filtered;
        }

        onFilter(filter: services.IFilter) {
            var found = _.find(this.filters, (f) => {
                return f.isChecked;
            });

            if (!found) {
                this.filteredItems = this.applicationSectionRootItems;
                this.showAll = true;
            } else {
                this.filterData();
                this.showAll = false;
            }
        }

        checkField(statusName: string): void {
            if (!statusName) return;

            switch (statusName.toLowerCase()) {
                case this.config.applicationSectionStatuses.rfiCompleted.toLowerCase().replace(" ", ""):
                    statusName = this.config.applicationSectionStatuses.rfiCompleted;
                    break;
                case this.config.applicationSectionStatuses.forReview.toLowerCase().replace(" ", ""):
                    statusName = this.config.applicationSectionStatuses.forReview;
                    break;
                case this.config.applicationSectionStatuses.notStarted.toLowerCase().replace(" ", ""):
                    statusName = this.config.applicationSectionStatuses.notStarted;
                    break;
                case this.config.applicationSectionStatuses.notCompliant.toLowerCase().replace(" ", ""):
                    statusName = this.config.applicationSectionStatuses.notCompliant;
                    break;
            }

            var found = _.find(this.filters, (filter) => {
                return filter.filterName === statusName.trim();
            });

            if (!found) {
                this.filters.push({
                    filterName: statusName,
                    isChecked: false
                });
            }
        }
        
        //finds a section from the actual application.sections based on the requirementid/sectionid
        findSectionOnly(requirements: services.IApplicationSectionResponse[], sectionId: string): services.IApplicationSectionResponse {
            for (var i = 0; i < requirements.length; i++) {

                var req = requirements[i];
                if (req.applicationSectionUniqueIdentifier === sectionId) {

                    this.requirementTemp = req;
                    return this.requirementTemp;
                }

                if (req.children && req.children.length > 0) {
                    var item = this.findSectionOnly(req.children, sectionId);

                    if (item != null) {
                        this.requirementTemp = item;
                        return this.requirementTemp;
                    }
                }
            }

            return null;
        }

        findSectionFromApp(requirements: services.IApplicationSection[], sectionId: string): services.IApplicationSectionResponse {
            for (var i = 0; i < requirements.length; i++) {

                var req = requirements[i];
                if (req.uniqueIdentifier === sectionId) {

                    var resp: services.IApplicationSectionResponse = {
                        applicationSectionId: req.id,
                        applicationSectionName: req.name,
                        applicationSectionHelpText: req.helpText,
                        applicationSectionUniqueIdentifier: req.uniqueIdentifier,
                        hasQuestions: req.questions && req.questions.length > 0,
                        isVisible: req.isVisible,
                        hasFlag: false,
                        sectionStatusName: req.circleStatusName,
                        hasRfiComment: false,
                        hasCitationComment: false,
                        hasFactOnlyComment: false,
                        hasSuggestions: false,
                        questions: req.questions,

                    };

                    this.requirementTemp = resp;
                    return this.requirementTemp;
                }

                if (req.children && req.children.length > 0) {
                    var item = this.findSectionFromApp(req.children, sectionId);

                    if (item != null) {
                        this.requirementTemp = item;
                        return this.requirementTemp;
                    }
                }
            }

            return null;
        }

        //REQUIRED ??
        findSection2(requirements: services.IApplicationSectionResponse[], sectionId: string): services.IApplicationSectionResponse {
            for (var i = 0; i < requirements.length; i++) {
                var req = requirements[i];

                if (req.applicationSectionUniqueIdentifier === sectionId) {
                    return req;
                }

                if (req.children && req.children.length > 0) {
                    var item = this.findSection2(req.children, sectionId);

                    if (item != null) {
                        return item;
                    }
                }
            }

            return null;
        }
        //REQUIRED ??
        doesSectionHaveStatus(req: services.IApplicationSectionResponse, rfi: boolean, rfiComplete: boolean): string {
            if (rfi) {
                var found = _.find(req.questions, (q) => {
                    return !q.isHidden &&
                        (q.answerResponseStatusName === "RFI" || !rfi);
                });

                if (found) {
                    return "RFI";
                }
            }


            if (rfiComplete) {
                var found2 = _.find(req.questions, (q) => {
                    return !q.isHidden &&
                        (q.answerResponseStatusName === "RFI Complete");
                });

                if (found2) {
                    return "RFI Complete";
                }
            }

            if (req.children) {
                for (var j = 0; j < req.children.length; j++) {
                    var result = this.doesSectionHaveStatus(req.children[j], rfi, rfiComplete);

                    if (result !== "") {
                        return result;
                    }
                }

            }

            return "";
        }
        //REQUIRED ??
        findSectionWithRfis(requirement: services.IApplicationSectionResponse): services.IApplicationSectionResponse {

            if (requirement.children) {
                for (var i = 0; i < requirement.children.length; i++) {
                    var child = requirement.children[i];

                    var found = _.find(child.questions, (q: services.IQuestion) => {
                        return q.answerResponseStatusName === "RFI";
                    });

                    if (found) {
                        return child;
                    }
                }
            } else {
                var found = _.find(requirement.questions, (q: services.IQuestion) => {
                    return q.answerResponseStatusName === "RFI";
                });

                if (found) {
                    return requirement;
                }
                else return null;
            }
        }

    }

    angular
        .module('app.application')
        .controller('app.application.StatusViewController',
        StatusViewController);
}