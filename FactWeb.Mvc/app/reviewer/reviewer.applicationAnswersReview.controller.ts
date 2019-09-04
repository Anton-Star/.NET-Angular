module app.reviewer {
    'use strict';

    /*TODO: REMOVE THIS INTERFACT*/
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
        appUniqueId: string;
        hasFlags?: boolean;
        hasRfi?: boolean;
        circleStatusName?: string;
        circle?: string;
    }

    class ApplicationAnswersReviewController extends app.application.ApplicationBase {
        allSections: Array<Eligibility.IApplicationHierarchyData>;
        user: services.IUser;
        complianceApps: services.ICompApplication[] = [];
        apps: services.IApp[] = [];
        appDueDate = "";
        submittedDate = "";
        inspectionDate: Date;
        isInit = true;

        dateOptions = {
            open: this.onOpen
        };

        static $inject = [
            '$scope',
            '$q',
            '$timeout',
            '$window',
            '$rootScope',
            '$uibModal',
            '$location',
            'config',
            '$route',
            'modalHelper',
            'documentService',
            'accountService',
            'applicationService',
            'applicationResponseCommentService',
            'applicationSettingService',
            'inspectionService',
            'emailTemplateService',
            'cacheService',
            'notificationFactory',
            'common',
            'coordinatorService',
            'applicationStatusService',
            'organizationService'
        ];
        constructor(
            $scope: ng.IScope,
            $q: ng.IQService,
            $timeout: ng.ITimeoutService,
            private $window: ng.IWindowService,
            private $rootScope: ng.IRootScopeService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            $location: ng.ILocationService,
            config: IConfig,
            private $route: ng.route.IRouteService,
            private modalHelper: common.IModalHelper,
            documentService: services.IDocumentService,
            private accountService: services.IAccountService,
            applicationService: services.IApplicationService,
            private applicationResponseCommentService: services.IApplicationResponseCommentService,
            private applicationSettingService: services.IApplicationSettingService,
            private inspectionService: services.IInspectionService,
            private emailTemplateService: services.IEmailTemplateService,
            private cacheService: services.ICacheService,
            notificationFactory: blocks.INotificationFactory,
            common: app.common.ICommonFactory,
            private coordinatorService: services.ICoordinatorService,
            private applicationStatusService: services.IApplicationStatusService,
            private organizationService: services.IOrganizationService) {

            super($q, $timeout, $rootScope, $location, documentService, applicationService, config, common, notificationFactory);

            //register bulk update event.
            this.scanForEvents();

            this.checkValues();

            if (this.common.currentUser) {
                this.init();
            }        

            var appSaved = this.$rootScope.$on("AppSaved", (data: any, args: any) => {
                this.common.showSplash();
                if (this.compAppUniqueId != undefined && this.compAppUniqueId != null && this.compAppUniqueId !== "") {
                    this.applicationService.getCompApplication(this.compAppUniqueId, args.appUniqueId)
                        .then((data) => {
                            console.log(data);
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

                                        if (args.nextSection && foundApplication.applicationUniqueId === args.appUniqueId) {
                                            var nextSect = this.findSect(application.applicationSectionResponses, args.nextSection.applicationSectionId);

                                            if (nextSect != null) {
                                                args.nextSection = nextSect;
                                            }
                                        }
                                    });
                                }
                            });

                            if (args.nextSection) {

                                this.$uibModal.open({
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
                                                appUniqueId: args.appUniqueId,
                                                isUser: false,
                                                organization: this.organization.organizationName
                                            };
                                        }
                                    }
                                });
                            }

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
                            var nextSectionProcessed = false;

                            _.each(this.requirements, (r) => {
                                this.processRow(r, items);

                                if (args.nextSection && !nextSectionProcessed) {
                                    var nextSect = this.findAppSection(r, args.nextSection.applicationSectionId);

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
                                this.$uibModal.open({
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
                                                appUniqueId: args.appUniqueId,
                                                isUser: false,
                                                organization: this.organization.organizationName
                                            };
                                        }
                                    }
                                });
                            }

                            this.common.hideSplash();
                        })
                        .catch((e) => {
                            console.log(e);
                            this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                        });
                }
                
            });

            $scope.$on('$destroy', () => {
                appSaved();
            });
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

        processRow(row: services.IHierarchyData, sections: services.IApplicationSection[]) {
            var sect = this.findSect2(row.id, sections);

            if (sect != null) {
                row.isVisible = sect.isVisible;
                row.circleStatusName = sect.circleStatusName;
                row.circle = sect.circle;

                if (row.children && row.children.length > 0) {
                    _.each(row.children, (c) => {
                        this.processRow(c, sections);
                    });
                }
            }
        }

        findSect2(id: string, sections: services.IApplicationSection[]): services.IApplicationSection {
            for (var i = 0; i < sections.length; i++) {
                if (sections[i].id === id) return sections[i];

                if (sections[i].children != null && sections[i].children != undefined && sections[i].children.length > 0) {
                    var sect = this.findSect2(id, sections[i].children);

                    if (sect != null) {
                        return sect;
                    }
                }
            }

            return null;
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

        onOpen(e) {
            var id = e.sender.element[0].id;

            setTimeout(() => {
                var pos = $('#' + id).offset().top + 40;
                $(".k-animation-container").css("top", pos);
            },
                200);

        }
        
        scanForEvents() {
            //this.rootScope.$on(this.config.events.bulkUpdate, (data: any, args: any) => {
            //    this.$window.location.reload();
            //});

            this.rootScope.$on('CheckStatus', (data: any, args: any) => {
                _.each(args.section.questions, (q: services.IQuestion) => {
                   if (q.answerResponseStatusName == this.config.applicationSectionStatuses.rfi) {
                       this.hasRfi = true;
                   }
                });
            });

            this.checkStatus();
        }

        init() {
            this.user = this.common.currentUser;
        }

        onAccessGranted() { }

        onApplicationDetailsLoaded() {
            this.application.applicantApplicationStatusName = this.application.applicationStatusName;
        }

        onApplicationSectionsLoaded() {
        }

        onCompApplicationLoaded() {
            if (this.application && this.application.submittedDateString && this.submittedDate === "")
                this.submittedDate = this.application.submittedDateString;

            if (this.application && this.application.dueDateString && this.appDueDate === "")
                this.appDueDate = this.application.dueDateString;

            if (this.application && this.application.inspectionDate && !this.inspectionDate) {
                this.inspectionDate = this.application.inspectionDate;
            }

            this.hasRfi = this.compApplication.hasRfi;

            this.common.onResize();
        }

        onReloadSection(section: services.IHierarchyData) {
            var promises = [];
            //if (this.isComplianceApplication)
            //    promises.push(this.getCompApplication()); //fetch all sites and their associated sections.
            //else
            //    promises.push(this.getApplicationSections()); //fetch only current application.

            this.common.activateController(promises, "ApplicationAnswersReviewController");            
        }

        //override the processSection here because the reviewer view needs some extra processing on sections.
        processSection(section: services.IApplicationSection, isRoot: boolean, hasOutcome?: boolean, parentPart?: string, parentName?: string): services.IHierarchyData {
            if (!this.isComplianceApplication) {
                return this.processSectionForReviewerView(section, isRoot, hasOutcome, parentPart, parentName);
            } else {
                return null;
            }
        }

        checkStatus() {
            this.$rootScope.$on("CheckStatus", (data: any, args: any) => {
                if (this.compApplication && this.compApplication.complianceApplicationSites) {
                    _.each(this.compApplication.complianceApplicationSites, (s: services.IComplianceApplicationSite) => {
                        _.each(s.applications, (a: services.IApplication) => {
                            a.circle = this.common.getCircleColorForSections(a.sections, true);
                        });

                        s.circle = this.common.getCircleForSite(s.applications, true);
                    });
                }
            });
        }
        
        sendToFact(): void {
            this.common.showSplash();
            
            this.applicationService.sendToFact(this.appUniqueId,
                this.common.currentUser.firstName + " " + this.common.currentUser.lastName + " " + this.common.currentUser.credentials,
                this.application.organizationName,
                this.application.applicationTypeName,
                this.application.coordinator && this.application.coordinator.emailAddress ? this.application.coordinator.emailAddress : "", this.application.applicationTypeId, this.application.organizationId)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("RFI sent to FACT");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to send to FACT. Please contact support.");
                    this.common.hideSplash();
                });
        }

        backForRFI(): void {
            if (this.common.isInspector() || this.common.isFact())
            {            
                if (!this.application.rfiDueDate || this.application.rfiDueDate === "") {
                    this.notificationFactory.error("RFI Due Date is required before sending back for RFI.");
                    return;
                }

                // check is there is any question with RFI status
                var date = new Date(this.application.rfiDueDate);
                var options = { month: 'long', day: 'numeric', year: 'numeric' };
                var rfiDueDate = date.toLocaleDateString('en-US', options);

                var html = this.template.html;
                html = html.replace("{Org Name}", this.application.organizationName);
                html = html.replace("{RFI Due Date}", rfiDueDate);
                if (this.application.complianceApplicationId && this.application.complianceApplicationId !== "") {
                    html = html.replace("{URL}", this.urlSetting.value + "#/Compliance?app=" + this.application.uniqueId + "&c=" + this.application.complianceApplicationId);
                } else {
                    html = html.replace("{URL}", this.urlSetting.value + "#/Application?app=" + this.application.uniqueId);
                }
                if (this.application.coordinator != null) {
                    html = html.replace("{CoordinatorName}", this.application.coordinator.firstName + " " + this.application.coordinator.lastName);

                    var creds = "";

                    _.each(this.application.coordinator.credentials, (c) => {
                        creds += ", " + c.name;
                    });

                    if (creds !== "") {
                        html = html.replace(", {CoordinatorCredentials}", creds);
                    } else {
                        html = html.replace(", {CoordinatorCredentials}", "");
                    }
                    
                    html = html.replace("{CoordinatorTitle}", this.application.coordinator.title);
                    html = html.replace("{CoordinatorPhoneNumber}", this.application.coordinator.preferredPhoneNumber);
                    html = html.replace("{CoordinatorEmailAddress}", this.application.coordinator.emailAddress);
                } else {
                    html = html.replace("{CoordinatorName}", "");
                    html = html.replace("{CoordinatorCredentials}", "");
                    html = html.replace("{CoordinatorTitle}", "");
                    html = html.replace("CoordinatorPhoneNumber", "");
                    html = html.replace("{CoordinatorEmailAddress}", "");
                }

                var tos = "";

                if (this.organization.organizationDirectors) {
                    _.each(this.organization.organizationDirectors, (d) => {
                        tos += d.emailAddress + ",";
                    });

                    if (tos.length > 0) {
                        tos = tos.substring(0, tos.length - 1);
                    }
                }

                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/buildEmail.html",
                    controller: "app.modal.templates.BuildEmailController",
                    controllerAs: "vm",
                    size: 'lg',
                    windowClass: 'app-modal-window',
                    resolve: {
                        values: () => {
                            return {
                                to: this.organization.primaryUser.emailAddress + "," + tos,
                                cc: this.application.coordinator.emailAddress + (this.organization.facilityDirectors != null ? "," + this.organization.facilityDirectors : ""),
                                subject: "FACT requires additional information for your application",
                                html: html,
                                type: this.config.emailTypes.backToRfi
                            }
                        }
                    }
                });

                instance.result.then((data: any) => {
                    this.common.showSplash();

                    //this.hasRfi = false;
                    //this.parseForRFI();

                    //bug 760 show error message if there is no rfi
                    if (!this.hasRfi) {
                        this.notificationFactory.error("There are no question(s) having status RFI");
                        return;
                    }

                    // User Story - 292
                    var newApplicationStatus = "RFI In Progress";

                    if (this.application.hasOutcome)
                    {
                        newApplicationStatus = "Applicant Response";
                    }

                    var dueDate: Date = null;
                    if (this.application.rfiDueDate) {
                        dueDate = new Date(this.application.rfiDueDate);
                    }

                    this.applicationService.updateApplicationStatus(this.application.applicationTypeId, newApplicationStatus, this.application.organizationId, dueDate, data.html, data.includeAccreditationReport) //duplicate
                        .then((data: services.IServiceResponse) => {
                            if (data.hasError) {
                                this.notificationFactory.error(data.message);
                            }
                            else {
                                this.application.applicantApplicationStatusName = "RFI";

                                this.notificationFactory.success("Success");
                            }

                            this.common.hideSplash();

                        })
                        .catch(() => {
                            this.notificationFactory.error("Error trying to update application status. Please contact support.");
                            this.common.hideSplash();
                        });
                        
                }, () => {
                    this.notificationFactory.error("Save has been cancelled");
                });

                
            }

        }

        parseForRFI(): void {
            if (this.isComplianceApplication) {
                
                _.each(this.compApplication.complianceApplicationSites, (appSite: services.IComplianceApplicationSite) => {
                    _.each(appSite.applications, (app: services.IApplication) => {
                        _.each(app.sections, (section: services.IApplicationSection) => {
                            this.processSection(section, true);
                        });
                    });
                });
            }
            else {
                //check the model values for current application only
                _.each(this.requirements, (section: services.IApplicationSection) => {
                    this.processSection(section, true);                    
                });
            }
            
        }

        markComplete(): void {    
            var html = this.annualAppCompleteTemplate.html;
            html = html.replace("{ApplicationType}", this.application.applicationTypeName);
            html = html.replace("{URL}", this.urlSetting.value + "#/Application?app=" + this.application.uniqueId);

            var rfiContent = "";

            if (this.rfis.length > 0) {
                rfiContent = "<p>The following items require additional information at the time of your renewal report:</p><ul>";
                _.each(this.rfis, (rfi: services.IRfi) => {
                    rfi.comment = rfi.comment.replace("<p>", "");
                    rfi.comment = rfi.comment.replace("</p>", "");
                    rfiContent += "<li>" + rfi.requirementNumber + " - " + rfi.questionNumber + " - " + rfi.comment + "</li>";
                });

                rfiContent += "</ul>";

            }

            html = html.replace("{rfis}", rfiContent);

            html = html.replace("{Org Name}", this.application.organizationName);
            if (this.application.coordinator != null) {
                html = html.replace("{Coordinator Name}", this.application.coordinator.firstName + " " + this.application.coordinator.lastName);

                var creds = "";

                _.each(this.application.coordinator.credentials, (c) => {
                    creds += ", " + c.name;
                });

                if (creds !== "") {
                    html = html.replace(", {Coordinator Credentials}", creds);
                } else {
                    html = html.replace(", {Coordinator Credentials}", "");
                }

                html = html.replace("{CoordinatorTitle}", this.application.coordinator.title);
                html = html.replace("{Coordinator Phone}", this.application.coordinator.preferredPhoneNumber);
                html = html.replace("{Coordinator Email Address}", this.application.coordinator.emailAddress);
            } else {
                html = html.replace("{Coordinator Name}", "");
                html = html.replace("{Coordinator Credentials}", "");
                html = html.replace("{CoordinatorTitle}", "");
                html = html.replace("Coordinator Phone", "");
                html = html.replace("{Coordinator Email Address}", "");
            }
                 
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/buildEmail.html",
                controller: "app.modal.templates.BuildEmailController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            to: "",
                            cc: "",
                            subject: "",
                            html: html,
                            type: this.config.emailTypes.annualComplete
                        }
                    }
                }
            });

            instance.result.then((data: any) => {
                this.common.showSplash();
                this.applicationService.updateApplicationStatus(this.application.applicationTypeId, "Complete", this.application.organizationId, null, data.html, false)
                    .then((data: services.IServiceResponse) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        }
                        else {
                            this.notificationFactory.success("Success");
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to update application status. Please contact support.");
                        this.common.hideSplash();
                    });
            });
               
                               

            
        }

        validateFlag(): boolean {
            var validation = true;

            if (this.user.role.roleName.toLowerCase() == "user") {
                _.each(this.allSections, (section: Eligibility.IApplicationHierarchyData) => {
                    _.each(section.questions, (question: services.IQuestion) => {
                        if (question.flag == true)
                            validation = false;
                    });
                });


            }
            return validation;
        }

        save(): void {
            //override due date with rfi date

            if (this.application.rfiDueDate) {
                this.application.dueDate = new Date(this.application.rfiDueDate);
            }

            //save application due date           
            this.applicationResponseCommentService.SaveApplication(this.application)
                .then((result: services.IServiceResponse) => {
                    this.notificationFactory.success("Due date saved successfully");
                })
                .catch(() => {
                    this.notificationFactory.error("Cannot save application. Please contact support");
                });
        }
                
        sendToInspection(): void {

            if (!confirm("Are you sure you want to send this application for inspection?")) {
                return;
            }

            var found = null;
            if (this.compApplication) {
                found = _.find(this.compApplication.complianceApplicationSites, (site) => {
                    var find = _.find(site.appResponses, (r) => {
                        return r.applicationTypeName.indexOf("CB") > -1;
                    });

                    if (find) {
                        return true;
                    } else {
                        return false;
                    }
                });    
            }

            this.common.showSplash();

            this.applicationService.sendForInspection(this.compAppUniqueId, this.application.organizationName, this.application.coordinator.userId, found != null)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Application submitted for inspection successfully.");

                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.common.hideSplash();
                });
        }
    }

    angular
        .module('app.reviewer')
        .controller('app.reviewer.ApplicationAnswersReviewController',
        ApplicationAnswersReviewController);
}    