module app.Coordinator {
    'use strict';
    
    class ViewController {
        application: services.IApplication;
        coordinatorView: services.ICoordinatorViewItem;  
        allSection: Array<services.IApplicationSection> = [];    
        
        toAddresses = "";
        ccAddresses = "";
        bodyEmail = "";
        mailTo = "";
        appSettings: services.IApplicationSetting[];
        postInspectionDocuments: services.IDocument[];
        accessToken: services.IAccessToken;
        cbTotals: services.ITotal[] = [];
        ctTotals: services.ICtTotal[] = [];
        categories: string[] = [];
        appUniqueId: string = "";
        compAppUniqueId: string = "";
        isComplianceApplication: boolean = false;
        deleteMe = null;
        commentTypes: services.ICommentType[];
        staff: Array<services.IUser>;
        inspectionDetails: services.IInspectionOverallDetail;
        jobFunctions: services.IJobFunction[];

        static $inject = [
            '$location',
            '$uibModal',
            'jobFunctionService',
            'coordinatorService',
            'applicationService',
            'applicationResponseCommentService', 
            'documentService', 
            'cacheService',        
            'emailService',  
            'trueVaultService',
            'inspectionService',
            'userService',
            'notificationFactory',
            'config',
            'common'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private jobFunctionService: services.IJobFunctionService,
            private coordinatorService: services.ICoordinatorService,            
            private applicationService: services.IApplicationService,  
            private applicationResponseCommentService: services.IApplicationResponseCommentService,
            private documentService: services.IDocumentService,
            private cacheService: services.ICacheService,  
            private emailService: services.IEmailService,       
            private trueVaultService: services.ITrueVaultService,
            private inspectionService: services.IInspectionService, 
            private userService: services.IUserService,
            private notificationFactory: blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory) {

            this.appUniqueId = this.$location.search().app;
            this.compAppUniqueId = this.$location.search().c;

            if (this.compAppUniqueId !== null && this.compAppUniqueId !== "")
                this.isComplianceApplication = true;

            this.common.checkItemValue(this.config.events.applicationSettingsLoaded, this.common.applicationSettings, false)
                .then(() => {
                    this.appSettings = this.common.applicationSettings;
                });

            this.common.checkItemValue(this.config.events.applicationLoaded, this.common.application, false)
                .then(() => {
                    this.application = this.common.application;
                });

            this.common.checkItemValue(this.config.events.postInspectionDocumentsLoaded, this.common.postInspectionDocuments, false)
                .then(() => {
                    this.postInspectionDocuments = this.common.postInspectionDocuments;
                });

            this.common.checkItemValue(this.config.events.accessTokenLoaded, this.common.accessToken, false)
                .then(() => {
                    this.accessToken = this.common.accessToken;
                });

            this.common.checkItemValue(this.config.events.userLoggedIn, this.common.currentUser, false)
                .then(() => {
                    this.getCommentTypes();
                });

            if (this.common.ctTotals != null) {
                this.ctTotals = this.common.ctTotals;
            } else {
                this.getCtTotals();
            }

            if (this.common.cbTotals != null) {
                this.cbTotals = this.common.cbTotals;
                this.categories = this.common.cbCategories;
            } else {
                this.getCbTotals();
            }
                
            common.activateController([this.getCoordinatorView(), this.getUsers()], '');
            
        }

        getUsers(): ng.IPromise<void> {
            return this.userService.getFactStaff()
                .then((data: Array<services.IUser>) => {
                    this.staff = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getJobFunctions(): ng.IPromise<void> {
            return this.jobFunctionService.getAll()
                .then((data) => {
                    this.jobFunctions = data;
                });
        }

        getCommentTypes(): ng.IPromise<void> {
            return this.applicationResponseCommentService.getCommentTypes()
                .then((data: Array<app.services.ICommentType>) => {
                    if (data != null) {
                        this.commentTypes = data.filter((commentType) => {
                            if (this.common.isFact()) return true;

                            if (this.common.isInspector()) {
                                return commentType.name !== "FACT Response" && commentType.name !== "Coordinator";
                            } else {
                                return (commentType.name !== 'FACT Only' && commentType.name !== "Coordinator" && commentType.name !== "FACT Response");
                            }
                        });

                        console.log('comments loaded');
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("No comment type found.");
                });
        }

        getCtTotals(): ng.IPromise<void> {
            
            return this.applicationService.getCtTotals(this.compAppUniqueId)
                .then((data) => {
                    this.common.ctTotals = data;
                    this.ctTotals = data;
                });
        }

        getCbTotals(): ng.IPromise<void> {
            
            return this.applicationService.getCbTotals(this.compAppUniqueId)
                .then((data) => {
                    var items: services.ITotal[] = [];

                    _.each(data, (r) => {
                        var site: services.ITotal = _.find(items, (i) => {
                            return i.siteName === r.siteName;
                        });

                        if (!site) {
                            site = {
                                siteName: r.siteName,
                                unitType: r.unitType,
                                categories: []
                            };

                            items.push(site);
                        }

                       var found = _.find(this.categories, (c) => {
                           return c === r.category;
                        });

                        if (!found) {
                            this.categories.push(r.category);
                        }
                    });

                    _.each(items, (s) => {
                        var records = _.filter(data, (r) => {
                            return r.siteName === s.siteName;
                        });

                        _.each(this.categories, (c) => {
                            var found = _.find(records, (re) => {
                                return re.category === c;
                            });

                            if (found) {
                                var cat = _.find(s.categories, (ca) => {
                                    return ca.category === found.category;
                                });

                                if (cat) {
                                    cat.numberOfUnits += found.numberOfUnits;
                                } else {
                                    s.categories.push({
                                        category: found.category,
                                        numberOfUnits: found.numberOfUnits,
                                        asOfDate: found.asOfDate
                                    });
                                }                               
                            }
                        });
                    });

                    this.common.cbCategories = this.categories;
                    this.common.cbTotals = items;

                    this.cbTotals = items;
                });
        }

        getUrl(): string {
            var setting = _.find(this.appSettings, (setting: services.IApplicationSetting) => {
                return setting.name === "System Base Url";
            });

            return setting ? setting.value : "";
        }

        getCoordinatorView(): ng.IPromise<void> {
            console.log('start', new Date());
            //currently we are only retrieving coordinator view for compliance applications.
            return this.coordinatorService.getCoordinatorView(this.compAppUniqueId)
                .then((data: app.services.ICoordinatorViewItem) => {
                    console.log('end', new Date());
                    if (data == null) {
                        this.notificationFactory.error('No Coordinator View Item found.');
                    } else {
                        _.each(data.inspectionTeamMembers, (teamMember) => {
                            if (teamMember.role.indexOf("Inspector") > -1 && this.toAddresses.indexOf(teamMember.email) == -1)
                                this.toAddresses += teamMember.email + ";";
                            else if (teamMember.role == "Auditors" || teamMember.role == "Observers")
                                this.ccAddresses += teamMember.email + ";";
                        });
                        var url = this.getUrl();

                        this.bodyEmail = url + "#/Application?app=" + this.$location.search().app;

                        this.mailTo = "mailto:" + this.toAddresses;

                        if (this.ccAddresses.length > 0) {
                            this.mailTo += "?cc=" + this.ccAddresses;
                        }

                        this.mailTo += "&body=" + encodeURIComponent(this.bodyEmail);
                        if (data.overview.accreditedSince != null && data.overview.accreditedSince != undefined) {
                            data.overview.accreditedSince = moment(data.overview.accreditedSince).toDate();
                        }
                        

                        this.coordinatorView = data;
                        console.log('coord', data);

                        this.getInspectionDetails();
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error while getting coordinator view details.");
                });
        }

        //Use this function to load appplication level details on demand
        onSiteAppView(application: services.IApplication) {
            if (application.sections != null && application.sections != undefined) return;
                this.common.activateController([this.getApp(application)], '');                 
        }

        getInspectionDetails(): ng.IPromise<void> {
            return this.inspectionService.getInspectionDetails(this.compAppUniqueId)
                .then((data) => {
                    this.inspectionDetails = data;

                    console.log('coord view', this.coordinatorView);
                });
        }

        getApp(application: services.IApplication): ng.IPromise<void> {
            
            return this.coordinatorService.getAppSections(application.uniqueId)
                .then((items: Array<services.IApplicationSection>) => {
                    var data = [];
                    
                    _.each(items, (value: services.IApplicationSection) => {
                        data.push(this.processSection(value));
                    });

                    console.log('sections', data);

                    application.sections = data;                    
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support.<br><br>" + e.exceptionMessage);
                });
        }

        onSaveSiteDetails(site: services.IComplianceApplicationSite) {
            this.common.showSplash();
            this.inspectionService.saveInspectionDetailFromCoordinator(site.inspectionDetails)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Site details saved successfully.");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving site details. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onSaveDetails() {
            this.common.showSplash();
            this.inspectionService.saveDetail(this.inspectionDetails)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Site details saved successfully.");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving site details. Please contact support.");
                    this.common.hideSplash();
                });
        }

        processSection(section: services.IApplicationSection, parentPart?: string, parentName?: string): services.IHierarchyData {
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
                appUniqueId: section.appUniqueId
            };
            var showInCoordinatorView = false;
            if (section.questions && section.questions.length > 0) {
                var show = false;
                _.each(section.questions, (question: services.IQuestion) => {
                    if (!question.isHidden) {
                        if (question.applicationResponseComments != undefined && question.applicationResponseComments != null && question.applicationResponseComments.length != 0) {

                            if (question.responseCommentsCitation && question.responseCommentsCitation.length > 0) {
                                _.each(question.responseCommentsCitation, (comment: services.IApplicationResponseComment) => {
                                    comment.createdDte = moment(comment.createdDate).toDate();
                                    if (comment.commentOverride) {
                                        comment.comment = comment.commentOverride;
                                    }
                                });

                                showInCoordinatorView = true;
                            }

                            if (question.responseCommentsSuggestion && question.responseCommentsSuggestion.length > 0) {
                                _.each(question.responseCommentsSuggestion, (comment: services.IApplicationResponseComment) => {
                                    comment.createdDte = moment(comment.createdDate).toDate();
                                    if (comment.commentOverride) {
                                        comment.comment = comment.commentOverride;
                                    }
                                });
                                showInCoordinatorView = true;
                            }

                            if (question.responseCommentsFactResponse && question.responseCommentsFactResponse.length > 0) {
                                _.each(question.responseCommentsFactResponse, (comment: services.IApplicationResponseComment) => {
                                    comment.createdDte = moment(comment.createdDate).toDate();
                                    if (comment.commentOverride) {
                                        comment.comment = comment.commentOverride;
                                    }
                                });
                                showInCoordinatorView = true;
                            }

                            if (question.responseCommentsFactOnly && question.responseCommentsFactOnly.length > 0) {
                                _.each(question.responseCommentsFactOnly, (comment: services.IApplicationResponseComment) => {
                                    comment.createdDte = moment(comment.createdDate).toDate();
                                    if (comment.commentOverride) {
                                        comment.comment = comment.commentOverride;
                                    }
                                });
                                showInCoordinatorView = true;
                            }

                            if (question.responseCommentsRFI && question.responseCommentsRFI.length > 0) {
                                _.each(question.responseCommentsRFI, (comment: services.IApplicationResponseComment) => {
                                    comment.createdDte = moment(comment.createdDate).toDate();
                                    if (comment.commentOverride) {
                                        comment.comment = comment.commentOverride;
                                    }
                                });

                                if (this.coordinatorView.overview.inspectionDate &&
                                    this.coordinatorView.overview.inspectionDate !== '') {
                                    var inspectionDates: string[] = [];
                                    if (this.coordinatorView.overview.inspectionDate.indexOf(',')) {
                                        inspectionDates = this.coordinatorView.overview.inspectionDate.split(',');
                                    } else {
                                        inspectionDates.push(this.coordinatorView.overview.inspectionDate);
                                    }

                                    var inspectionDate: moment.Moment;

                                    _.each(inspectionDates, (m) => {
                                        var dte = moment(m);
                                        if (inspectionDate == null) {
                                            inspectionDate = dte;
                                        } else {
                                            if (dte < inspectionDate) {
                                                inspectionDate = dte;
                                            }
                                        }
                                    });

                                    for (var i = 0; i < question.responseCommentsRFI.length; i++) {
                                        var dt = moment(question.responseCommentsRFI[i].createdDate);
                                        if (dt < inspectionDate) {
                                            question.responseCommentsRFI.splice(i, 1);
                                            i--;
                                        }
                                    }
                                }

                                if (question.responseCommentsRFI.length > 0) {
                                    showInCoordinatorView = true;
                                }
                            }
                        }
                    }
                    
                    //if (question.questionResponses == undefined || question.questionResponses == null || question.questionResponses.length === 0) {
                    //    question.questionResponses = [{}];
                    //}
                        
                    //if (question.applicationResponseComments != undefined && question.applicationResponseComments != null && question.applicationResponseComments.length != 0) {

                    //    var found = $.grep(this.allSection, function (allsect) {
                    //        return allsect.id == section.id;
                    //    })[0];

                    //    if (found == null || found == undefined) {
                    //        this.allSection.push(section);
                    //    }
                    //}
                });
                row.isVisible = showInCoordinatorView;
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
                
                _.each(section.children, (value: services.IApplicationSection) => {
                    var thisChild = this.processSection(value, row.partNumber, row.partNumber + ": " + row.name)
                    if (thisChild.isVisible === true) {
                       row.children.push(thisChild);
                    }               
                });
            }
            if (showInCoordinatorView || (row.children && row.children.length > 0))
                row.isVisible = true;
            else
                row.isVisible = false;
            
            return row;
        }

        onTotals(siteName: string): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/siteTotals.html",
                controller: "app.modal.templates.SiteTotalsController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            site: null,
                            siteName: siteName
                        };
                    }
                }
            });

            instance.result.then(() => {
                this.common.activateController([this.getCtTotals(), this.getCbTotals()], '');
            });
        }

        onSave() {
            this.common.showSplash();

            this.coordinatorService.save(this.compAppUniqueId,
                this.coordinatorView.overview.accreditationGoal,
                this.coordinatorView.overview.inspectionScope,
                this.coordinatorView.overview.accreditedSince,
                "", "", "", this.coordinatorView.complianceApplication.typeDetail)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Data saved successfully");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving data. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onSavePersonnel() {
            this.common.showSplash();

            this.coordinatorService.savePersonnel(this.application.organizationId, this.coordinatorView.personnel)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Data saved successfully.");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving data. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onManageApp() {
            var orgs = [
                { organizationId: this.application.organizationId, organizationName: this.application.organizationName }
            ];

            var apps = [
                this.application
            ];

            var app: services.ICoordinatorApplication = {
                organizationId: this.application.organizationId,
                organizationName: this.application.organizationName,
                location: "",
                applicationTypeId: this.application.applicationTypeId,
                applicationTypeName: this.application.applicationTypeName,
                coordinator: this.application.coordinator ? this.application.coordinator.fullName : "",
                coordinatorId: this.application.coordinator ? this.application.coordinator.userId : "",
                applicationDueDate: this.application.dueDate,
                outcomeStatusName: this.application.outcomeStatusName,
                applicationStatusId: this.application.applicationStatusId,
                applicationStatusName: this.application.applicationStatusName,
                applicationId: this.application.applicationId,
                complianceApplicationId: this.application.complianceApplicationId,
                applicationVersionTitle: this.application.applicationVersionTitle,
                applicationUniqueId: this.application.uniqueId

            }

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/applicationManagement.html",
                controller: "app.modal.templates.ApplicationManagementController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            organizations: orgs,
                            applications: apps,
                            coordinators: this.staff,
                            application: app
                        };
                    }
                }
            });

            instance.result.then((application: services.ICoordinatorApplication) => {
                if (this.application.coordinator) {
                    this.application.coordinator.userId = application.coordinatorId;
                    this.application.coordinator.fullName = application.coordinator;
                } else {
                    this.application.coordinator = {
                        userId: application.coordinatorId,
                        fullName: application.coordinator
                    };
                }
                
                this.application.applicationStatusId = application.applicationStatusId;
                this.application.applicationStatusName = application.applicationStatusName;
            }, () => {
            });
        }

        onDownload(document: services.IDocument): void {
            this.trueVaultService.getBlob(this.accessToken.vaultId,
                document.requestValues,
                this.common.currentUser.documentLibraryAccessToken,
                this.config.factKey)
                .then((data: any) => {
                    var fileType = this.trueVaultService.getFileType(document.name);
                    var file = new Blob([data.response], { type: fileType });
                    saveAs(file, document.name);
                })
                .catch((e) => {
                    this.notificationFactory.error("Cannot get document from True Vault. " + e);
                });
        }

        onCheck(document: services.IDocument) {
            var doc = _.find(this.postInspectionDocuments, (d: services.IDocument) => {
                return d.id === document.id;
            });

            if (doc) {
                doc.includeInReporting = document.includeInReporting;
            }
        }

        onSaveDocuments() {
            this.common.showSplash();

            this.documentService.saveIncludeInReporting(this.application.organizationName, this.postInspectionDocuments)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Data saved successfully.");
                    }
                    this.common.hideSplash();
                })
                .catch((e) => {
                    this.notificationFactory.error(e);
                    this.common.hideSplash();
                });
        }

        onSendToInspector() {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/buildEmail.html",
                controller: "app.modal.templates.BuildEmailController",
                controllerAs: "vm",
                size: 'xl',
                backdrop: false,
                keyboard: false,
                resolve: {
                    values: () => {
                        return {
                            to: this.toAddresses,
                            cc: this.ccAddresses,
                            subject: "",
                            html: "",
                            type: this.config.emailTypes.sendToInspector
                        };
                    }
                }
            });

            instance.result.then((data) => {
                this.common.showSplash();

                this.emailService.send(data.to, data.cc, data.subject, data.html, data.includeAccreditationReport, this.application.cycleNumber, this.application.organizationName, this.application.complianceApplicationId)
                    .then(() => {
                        this.notificationFactory.success("Email sent successfully.");
                        this.common.hideSplash();
                    })
                    .catch((e) => {
                        this.notificationFactory.error("Error sending email. Please contact support." + e);
                        this.common.hideSplash();
                    });

            });
        }

        showQuestionInCoordinatorView(question: services.IQuestion) {
            if (question.isHidden == false) {
                if (question.applicationResponseComments && question.applicationResponseComments.length > 0) //if there are any response comments against this question.
                    return true;
            }
            return false;
        }

        saveApplicationSections(application: services.IApplication) {
            var sectionsToSave = []; //array to keep only those sections which needs saving.
            var anyValidationFailed = false;

            _.each(application.sections, (section) => {
                if (section.isVisible) {
                    if (!this.validateSave(section, sectionsToSave)) {
                        //if validation on any section within this application fails.
                        anyValidationFailed = true;
                        return;
                    }                    
                }
            });

            if (anyValidationFailed) return;

            var promises = [];
            _.each(sectionsToSave, (section) => {
                promises.push(this.saveApplicationSection(application, section));
            });
            this.common.activateController(promises, '');
        }

        saveApplicationSection(application: services.IApplication, section: services.IHierarchyData): ng.IPromise<void> {
            return this.applicationService.saveApplicationSection(application.organizationName, application.applicationTypeName, application.uniqueId, section, false)
                .then((data: services.IServiceResponse) => {
                    this.common.hideSplash();
                    if (data.hasError) {
                        this.notificationFactory.error("Section " + section.uniqueIdentifier + ": " + data.message);
                    } else {
                        this.notificationFactory.success("Section " + section.uniqueIdentifier + " saved successfully.");
                    }
                })
                .catch(() => {
                    this.common.hideSplash();
                    this.notificationFactory.error("Error in saving Section. Please contact support.");
                });
        }

        validateSave(section: services.IHierarchyData, sectionsToSave: services.IHierarchyData[]): boolean {
            var validated = true;
            var requireRFIComments = false;
            var requireCitationComment = false;
            var sectionToSave = false;

            _.each(section.questions, (question) => {
                if (this.showQuestionInCoordinatorView(question)) { //if the question was visible in coordinator view
                    sectionToSave = true;
                    if (question.answerResponseStatusName === this.config.applicationSectionStatuses.rfi || question.answerResponseStatusName === this.config.applicationSectionStatuses.rfiFollowUp) {
                        if (question.responseCommentsRFI.length == 0) {
                            requireRFIComments = true;
                        }
                    }

                    if (question.answerResponseStatusName === this.config.applicationSectionStatuses.notCompliant && question.responseCommentsCitation.length === 0) {
                        requireCitationComment = true;
                    }
                }                
            });
            if (requireRFIComments) {
                this.notificationFactory.error('Questions with RFI require RFI comments');
                return false;
            }

            if (requireCitationComment) {
                this.notificationFactory.error("Questions with Not Compliant require Citation comment");
                return false;
            }

            if (sectionToSave) {
                for (var i = 0; i < sectionsToSave.length; i++) {
                    if (sectionsToSave[i].uniqueIdentifier === section.uniqueIdentifier) {
                        sectionsToSave.splice(i, 1);
                        break;
                    }
                }
                sectionsToSave.push(section);
            }

            _.each(section.children, (child) => {
                validated = this.validateSave(child, sectionsToSave);
            });

            return validated;            
        }

        
    }

    angular
        .module('app.eligibility')
        .controller('app.coordinator.ViewController',
        ViewController);
} 