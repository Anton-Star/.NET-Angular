module app.admin {
    'use strict';

    interface IRFIView {
        application: services.IApplication;
    }


    class RFIViewController implements IRFIView {
        application: services.IApplication;
        requirementSetName: string;
        allSiteApplicationSection: Array<services.ISiteApplicationSection>;
        rfiViewItem: services.IRfiViewItem;
        allQuestions: Array<services.IQuestion>;
        sectionUniqueIdTemp: string;
        sectionNameTemp: string;
        rfisBeforeInspection: number = 0;
        rfisAfterInspection: number = 0;
        totalRFIStandards: number = 0;
        totalRFIs: number = 0;
        totalSites: number = 0;
        rfiInSiteCounter: number = 0;
        siteInspectionDate: string;
        compAppId: string;
        appId: string;
        isFact = false;
        isInspector = false;
        fullSections: Array<services.ISiteApplicationSection>;
        

        static $inject = [
            '$uibModal',
            '$location',
            'config',
            'applicationService',
            'coordinatorService',
            'applicationSettingService',
            'trueVaultService',
            'notificationFactory',
            'common'
        ];
        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $location: ng.ILocationService,
            private config: IConfig,
            private applicationService: services.IApplicationService,
            private coordinatorService: services.ICoordinatorService,
            private applicationSettingService: services.IApplicationSettingService,
            private trueVaultService: services.ITrueVaultService,
            private notificationFactory: blocks.INotificationFactory,
            private common: app.common.ICommonFactory) {

            this.compAppId = $location.search().c;
            this.appId = $location.search().app;           

            this.common.checkItemValue(this.config.events.applicationLoaded, this.common.application, false)
                .then(() => {
                    this.application = this.common.application;
                });

            this.common.activateController([this.getApplication()], 'RFIViewController');
        }

        onDocumentDownload = (document: services.IDocument): void => {
            this.trueVaultService.onDocumentDownload(document, this.application.organizationName, this.common.accessToken);
        }

        getApplication(): ng.IPromise<void> {
            console.log('getting rfi view', new Date());       
            return this.coordinatorService.getRFIView(this.appId, this.compAppId)
                .then((items: services.IRfiViewItem) => {
                    console.log('got rfi view', new Date(), items);
                    this.rfiViewItem = items;
                    this.allSiteApplicationSection = [];

                    if (items) {
                        _.each(items.siteApplicationSection, (siteApplicationSection: services.ISiteApplicationSection) => {
                            if (siteApplicationSection.applicationSectionItem.length > 0) {

                                var siteApplicationSectionLocal: services.ISiteApplicationSection = {
                                    applicationSectionItem: [],
                                    siteItem: siteApplicationSection.siteItem
                                };

                                _.each(siteApplicationSection.applicationSectionItem, (section: services.IApplicationSection) => {

                                    var sectionLocal: services.IApplicationSection = {
                                        id: section.id,
                                        name: "",
                                        uniqueIdentifier: "",
                                        order: section.order,
                                        questions: [],
                                        appUniqueId: section.appUniqueId
                                    };
                                    this.allQuestions = [];

                                    if (this.application.applicationStatusName !== "In Progress") {
                                        this.processSection(section, siteApplicationSection.siteItem);
                                    }

                                    if (!this.common.isFact() && this.common.currentUser.role.roleName !== this.config.roles.inspector) {
                                        var comments: services.IApplicationResponseComment[] = [];

                                        _.each(this.allQuestions, (q) => {
                                            if (q.applicationResponseComments) {
                                                _.each(q.applicationResponseComments, (r) => {
                                                    r.updatedDt = moment(r.updatedDate).toDate();

                                                    if ((r.visibleToApplicant && r.commentType.name === "RFI") ||
                                                        r.commentType.name !== "RFI" ||
                                                        r.commentFrom.userId === this.common.currentUser.userId ||
                                                        r.commentFrom.role.roleName === this.config.roles.user) {
                                                        comments.push(r);
                                                    }

                                                });

                                                //comments = _.orderBy(comments, (c) => {
                                                //    return moment(c.createdDate).toDate();
                                                //}, 'desc');

                                                q.applicationResponseComments = comments;
                                                comments = [];
                                            }
                                        });
                                    } else {
                                        _.each(this.allQuestions, (q) => {
                                            if (q.applicationResponseComments) {
                                                _.each(q.applicationResponseComments, (r) => {
                                                    r.updatedDt = moment(r.updatedDate).toDate();
                                                });
                                            }
                                        });
                                    }

                                    sectionLocal.questions = this.allQuestions;
                                    sectionLocal.uniqueIdentifier = this.sectionUniqueIdTemp;
                                    sectionLocal.name = this.sectionNameTemp;

                                    if (this.allQuestions.length > 0) {
                                        siteApplicationSectionLocal.applicationSectionItem.push(sectionLocal);
                                    }

                                });

                                if (siteApplicationSectionLocal.siteItem) {
                                    siteApplicationSectionLocal.siteItem.rfiInSite = this.rfiInSiteCounter;
                                }
                                this.rfiInSiteCounter = 0;
                                this.allSiteApplicationSection.push(siteApplicationSectionLocal);
                            }
                        });
                    }

                    _.each(this.allSiteApplicationSection, (s) => {
                        _.each(s.applicationSectionItem, (i) => {
                            _.each(i.questions, (q) => {
                                console.log(q.applicationResponseComments);
                                q.applicationResponseComments = _.orderBy(q.applicationResponseComments, (c) => {
                                    return moment(c.createdDate).toDate();
                                }, 'desc');
                            });
                        });
                    });

                    console.log('all', this.allSiteApplicationSection);

                    this.fullSections = this.allSiteApplicationSection;
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred. Please contact support:<br>" + e.exceptionMessage);
                });

        }

        onViewRfis(isBefore?: boolean) {

            if (isBefore == null) {
                this.allSiteApplicationSection = this.fullSections;
                return;
            }

            var sites: services.ISiteApplicationSection[] = [];

            _.each(this.fullSections, (siteApp) => {

                var siteApplication: services.ISiteApplicationSection = {
                    siteItem: siteApp.siteItem,
                    applicationSectionItem: []
                };

                _.each(siteApp.applicationSectionItem, (sect) => {
                    var section: services.IApplicationSection = {
                        order: sect.order,
                        id: sect.id,
                        isActive: sect.isActive,
                        name: sect.name,
                        comments: sect.comments,
                        partNumber: sect.partNumber,
                        uniqueIdentifier: sect.uniqueIdentifier,
                        status: sect.status,
                        isVariance: sect.isVariance,
                        helpText: sect.helpText,
                        version: sect.version,
                        questions: [],
                        scopeTypes: sect.scopeTypes,
                        parentId: sect.parentId,
                        applicationTypeName: sect.applicationTypeName,
                        versionId: sect.versionId,
                        appUniqueId: sect.appUniqueId,
                        isVisible: sect.isVisible,
                        circleStatusName: sect.circleStatusName,
                        circle: sect.circle,
                        statusName: sect.statusName
                    };

                    _.each(sect.questions, (question) => {

                        if (question.responseCommentsRFI != undefined &&
                            question.responseCommentsRFI != null &&
                            question.responseCommentsRFI.length > 0) {
                            
                            //question.applicationResponseComments = _.orderBy(question.responseCommentsRFI, (c) => {
                            //    return moment(c.createdDate).toDate();
                            //}, 'desc');

                            var rfis = _.filter(question.responseCommentsRFI, (ctx) => {
                                if (!ctx.createdDate) {
                                    return false;
                                }

                                var dte = moment(ctx.createdDate);
                                var appDte = this.application.updatedDate ? moment(this.application.updatedDate) : moment();
                                return ctx.createdDate && ctx.commentType.name === "RFI" && (!this.application.updatedDate || dte < appDte);
                            });

                            var first = rfis.length > 0 ? rfis[rfis.length - 1] : null;

                            if (first) {
                                var rfiDate = moment(first.createdDate).toDate();
                                var inspectDate = this.application.inspectionDate
                                    ? moment(this.application.inspectionDate.toString()).toDate()
                                    : moment().toDate();
                                
                                if (isBefore) {
                                    if (!this.application.inspectionDate || rfiDate <= inspectDate) {
                                        section.questions.push(question);
                                    }
                                } else {
                                    if (rfiDate > inspectDate) {
                                        section.questions.push(question);
                                    }
                                }
                            }
                        }
                    });

                    if (section.questions.length > 0) {
                        siteApplication.applicationSectionItem.push(section);
                    }
                });

                if (siteApplication.applicationSectionItem.length > 0) {
                    var count = 0;
                    _.each(siteApplication.applicationSectionItem, (si) => {
                        count += si.questions.length;
                    });

                    siteApplication.siteItem.rfiInSite = count;
                    sites.push(siteApplication);
                }
            });

            this.allSiteApplicationSection = sites;
        }

        processSection(section: services.IApplicationSection, site:services.ISite): void {

            if (section.questions && section.questions.length > 0 && (!section.children || section.children.length === 0)) {
                
                _.each(section.questions, (question: services.IQuestion) => {

                    if (question.applicationResponseComments != undefined && question.applicationResponseComments != null && question.applicationResponseComments.length > 0) {
                        //question.applicationResponseComments = _.orderBy(question.applicationResponseComments, (c) => {
                        //    return moment(c.createdDate).toDate();
                        //}, 'desc');
                        
                        var rfis = _.filter(question.applicationResponseComments, (ctx) => {
                            if (!ctx.createdDate) {
                                return false;
                            }

                            var dte = moment(ctx.createdDate);
                            var appDte = this.application.updatedDate ? moment(this.application.updatedDate) : moment();

                            return ctx.createdDate && ctx.commentType.name === "RFI" && (this.common.isFact() || !this.application.updatedDate || dte < appDte);
                        });

                        
                        var first = rfis.length > 0 ? rfis[rfis.length - 1] : null;
                        
                        //var first = _.minBy(question.applicationResponseComments, (ctx) => {
                        //    if (!ctx.createdDate) {
                        //        return false;
                        //    }

                        //    var dte = moment(ctx.createdDate);
                        //    var appDte = this.application.updatedDate ? moment(this.application.updatedDate) : moment();
                        //    return ctx.createdDate && ctx.commentType.name === "RFI" && (!this.application.updatedDate || dte < appDte);
                        //});

                        if (first) {
                            var rfiDate = moment(first.createdDate);
                            var inspectDate = this.application.inspectionDate
                                ? moment(this.application.inspectionDate)
                                : moment();

                            if (!this.application.inspectionDate || rfiDate <= inspectDate) {
                                this.rfisBeforeInspection++;
                            }
                            else {
                                this.rfisAfterInspection++;
                            }
                            
                            this.rfiInSiteCounter++;
                            this.allQuestions.push(question);
                            this.sectionUniqueIdTemp = section.uniqueIdentifier;
                            this.sectionNameTemp = section.name;

                        }

                        _.each(question.applicationResponseComments, (comment: services.IApplicationResponseComment) => {
                            comment.createdDte = moment(comment.createdDate).toDate();
                        });

                        if (question.responseCommentsCitation && question.responseCommentsCitation.length > 0) {
                            _.each(question.responseCommentsCitation, (comment: services.IApplicationResponseComment) => {
                                comment.createdDte = moment(comment.createdDate).toDate();
                            });
                        }

                        if (question.responseCommentsSuggestion && question.responseCommentsSuggestion.length > 0) {
                            _.each(question.responseCommentsSuggestion, (comment: services.IApplicationResponseComment) => {
                                comment.createdDte = moment(comment.createdDate).toDate();
                            });
                        }

                        if (question.responseCommentsFactResponse && question.responseCommentsFactResponse.length > 0) {
                            _.each(question.responseCommentsFactResponse, (comment: services.IApplicationResponseComment) => {
                                comment.createdDte = moment(comment.createdDate).toDate();
                            });
                        }

                        if (question.responseCommentsFactOnly && question.responseCommentsFactOnly.length > 0) {
                            _.each(question.responseCommentsFactOnly, (comment: services.IApplicationResponseComment) => {
                                comment.createdDte = moment(comment.createdDate).toDate();
                            });
                        }

                        if (question.responseCommentsRFI && question.responseCommentsRFI.length > 0) {
                            _.each(question.responseCommentsRFI, (comment: services.IApplicationResponseComment) => {
                                comment.createdDte = moment(comment.createdDate).toDate();
                            });
                        }
                        
                    }                    
                });
            }

            //if (section.children && section.children.length > 0) {

            //    _.each(section.children, (value: services.IApplicationSection) => {
            //        this.processSection(value, site);
            //    });
            //}
        }

        findSect(sections: services.ISiteApplicationSection[], appUniqueId: string, sectionUniqueId: string) {
            for (var i = 0; i < sections.length; i++) {
                var siteApplicationSection: services.ISiteApplicationSection = sections[i];

                for (var j = 0; j < siteApplicationSection.applicationSectionItem.length; j++) {
                    var section: services.IApplicationSection = siteApplicationSection.applicationSectionItem[j];

                    if (section.uniqueIdentifier === sectionUniqueId && section.appUniqueId === appUniqueId) {
                        return section;
                    }

                    if (section.children && section.children.length > 0) {
                        var sect = this.findSect2(section.children, appUniqueId, sectionUniqueId);

                        if (sect != null) {
                            return sect;
                        }
                    }
                }
            }

            return null;
        }

        findSect2(sections: services.IApplicationSection[], appUniqueId: string, sectionUniqueId: string) {
            for (var i = 0; i < sections.length; i++) {
                var section: services.IApplicationSection = sections[i];

                if (section.uniqueIdentifier === sectionUniqueId && section.appUniqueId === appUniqueId) {
                    return section;
                }

                if (section.children && section.children.length > 0) {
                    var sect = this.findSect2(section.children, appUniqueId, sectionUniqueId);

                    if (sect != null) return sect;
                }
            }

            return null;
        }

        onShowRfi(item: services.IApplicationSection) {
            var sect = this.findSect(this.rfiViewItem.siteApplicationSection, item.appUniqueId || this.appId, item.uniqueIdentifier);
             
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
                            section: sect || item,
                            appUniqueId: item.appUniqueId || this.appId,
                            isUser: this.common.isUser(),
                            organization: this.application.organizationName
                        };
                    }
                }
            });

            instance.result.then(() => {
                this.notificationFactory.success("Review saved successfully.");
            }, () => {
            });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.RFIViewController',
        RFIViewController);
}    