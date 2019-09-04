

module app.widgets {
    'use strict';

    interface IQuestionCount {
        total: number;
        complete: number;
        isApplicant: boolean;
    }

    interface IBulkResponse {
        section: services.IApplicationSection,
        fromStatus: services.IApplicationResponseStatusItem;
        toStatus: services.IApplicationResponseStatusItem;
    }

    class ApplicationRequirement implements ng.IDirective {

        constructor(
            private $window: ng.IWindowService,
            private $rootScope: ng.IRootScopeService,
            private $timeout: ng.ITimeoutService,
            private $compile: ng.ICompileService,
            private $templateRequest: ng.ITemplateRequestService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private config: IConfig,
            private common: common.ICommonFactory,
            private modalHelper: common.IModalHelper,
            private notificationFactory: blocks.INotificationFactory,
            private requirementService: services.IRequirementService,
            private questionService: services.IQuestionService) {
            
        }

        static factory(): ng.IDirectiveFactory {
            var directive = (
                $window: ng.IWindowService,
                $rootScope: ng.IRootScopeService,
                $timeout: ng.ITimeoutService,
                $compile: any,
                $templateRequest,
                $uibModal: ng.ui.bootstrap.IModalService,
                config: IConfig,
                common: common.ICommonFactory,
                modalHelper: common.IModalHelper,
                notificationFactory: blocks.INotificationFactory,
                requirementService: services.IRequirementService,
                questionService: services.IQuestionService) =>
                new ApplicationRequirement($window, $rootScope, $timeout, $compile, $templateRequest, $uibModal, config, common, modalHelper, notificationFactory, requirementService, questionService);

            directive.$inject = [
                '$window',
                '$rootScope',
                '$timeout',
                '$compile',
                '$templateRequest',
                '$uibModal',
                'config',
                'common',
                'modalHelper',
                'notificationFactory',
                'requirementService',
                'questionService'
            ];

            return directive;
        }

        terminal = true;
        restrict = 'E';

        scope = {
            row: "=ngModel",
            id: "@",
            applicationtype: "@",
            questionTypes: "=questionTypes",
            scopeTypes: "=scopeTypes",
            isRequirementSet: "@isRequirementSet",
            parent: "=parent",
            questions: "=questions",
            currentVersion: "=currentVersion",
            appType: "@appType",
            accessType: "@accessType",
            organization: "@organization",
            dueDate: "@duedate",
            appSubmittedDate: "@appsubmitteddate",
            appUniqueId: "@appUniqueId",
            isReview: "@isReview",
            isMultisite: "@isMultisite",
            multiSites: "=multiSites",
            site: "=site",
            accessToken: "=accessToken",
            level: "@level",
            appStatus: "@appStatus"
        };

        processBulkChange(row: services.IApplicationSection, fromStatus: services.IApplicationResponseStatusItem, toStatus: services.IApplicationResponseStatusItem): boolean {
            var hasChanges = false;
            if (row.questions && row.questions.length > 0) {
                _.each(row.questions, (q) => {
                    if (q.answerResponseStatusId === fromStatus.id ||
                        (fromStatus.name === this.config.applicationSectionStatuses.forReview && (q.answerResponseStatusId == null || q.answerResponseStatusId === 0))) {
                        q.answerResponseStatusId = toStatus.id;
                        q.answerResponseStatusName = toStatus.name;
                        hasChanges = true;
                    }
                });
            }

            //if (row.children && row.children.length > 0) {
            //    _.each(row.children, (c) => {
            //        if (this.processBulkChange(c, fromStatus, toStatus)) {
            //            hasChanges = true;
            //        }
            //    });
            //}

            return hasChanges;
        }

        includeInTotal(child) {

            if (child.isVisible === false) {
                return false;
            }

            //if (child.questions && child.questions.length > 0) {
            //    var found = _.find(child.questions, (q: services.IQuestion) => {
            //        return !q.isHidden;
            //    });

            //    if (!found) {
            //        return false;
            //    }
            //}

            if (child.children && child.children.length > 0) {
                var visible = _.find(child.children, (item) => {
                        var result = this.includeInTotal(item);

                        return result;
                    });

                if (visible) {
                    return true;
                }
            } else {
                return true;
            }

            return false;
        }

        link = (scope: any, element: any, attributes: any) => {
            scope.common = this.common;
            scope.nextLevel = parseInt(scope.level || "0") + 1;
            scope.row.isEdit = false;
            scope.row.applicationTypeName = scope.applicationtype;
            scope.isApplicant = false;
            scope.status = {
                open: parseInt(scope.level || "0") < 3
            };
            scope.isInit = true;
            scope.windowInit = true;

            scope.$watch("status.open", () => {
                if (scope.isInit) {
                    scope.isInit = false;
                } else {
                    this.$timeout(() => {
                        this.common.onResize(scope.row.id);    
                    }, 100);
                }
                
            });

            var w = angular.element(this.$window);
            scope.getWindowDimensions = () => {
                return {
                    'h': w.height(),
                    'w': w.width()
                };
            };

            scope.$watch(scope.getWindowDimensions, () => {
                if (scope.windowInit) {
                    scope.windowInit = false;
                } else {
                    this.$timeout(() => {
                        this.common.onResize();
                    }, 100);

                }
            }, true);

            scope.isApplicant = this.common.isUser();

            scope.complete = 0;
            scope.total = 0;
            _.each(scope.row.children, (child) => {
                if (this.includeInTotal(child)) {
                    scope.total++;

                    if ((child.statusName === "Complete" || child.statusName === "RFI Complete" || child.statusName === "For Review" || child.statusName === "Reviewed" ||
                        child.circle === "Complete" || child.circle === "RFI Complete" || child.circle === "For Review" || child.circle === "Reviewed")
                        && child.isVisible) {
                        scope.complete++;
                    }
                }
            });

            if (angular.isArray(scope.row.children) && scope.row.children.length > 0) {
                this.$templateRequest("/app/widgets/applicationRequirementChildren.html").then((html) => {
                    var template = angular.element(html);
                    element.append(template);
                    this.$compile(template)(scope);
                });
            } else {
                this.$templateRequest("/app/widgets/applicationRequirement.html").then((html) => {
                    var template = angular.element(html);
                    element.append(template);
                    this.$compile(template)(scope);
                });
            }

            scope.onEdit = (row) => {
                //if (event != null) { event.stopPropagation(); } //Checking for null/undefined is still not working for FF.
                if (scope.isMultisite == "true") {
                    if (this.common.isReviewer()) {
                        var instance = this.$uibModal.open({
                            animation: true,
                            templateUrl: "/app/modal.templates/multiviewInspector.html",
                            controller: "app.modal.templates.MultiviewInspectorController",
                            controllerAs: "vm",
                            size: 'xxl',
                            backdrop: false,
                            keyboard: false,
                            resolve: {
                                section: () => {
                                    return row;
                                },
                                questions: () => {
                                    return scope.questions;
                                },
                                appType: () => {
                                    return scope.appType;
                                },
                                accessType: () => {
                                    return scope.accessType;
                                },
                                organization: () => {
                                    return scope.organization;
                                },
                                appDueDate: () => {
                                    return scope.dueDate;
                                },
                                appUniqueId: () => {
                                    return scope.appUniqueId;
                                },
                                multiSites: () => {
                                    return scope.multiSites;
                                },
                                site: () => {
                                    return scope.site;
                                }
                            }
                        });

                        instance.result.then(() => {
                            //use reloadSections if you want to reload the section from db
                            this.common.$broadcast(this.config.events.reloadSections, { section: scope.row });
                            this.notificationFactory.success("Review saved successfully.");
                        }, () => {
                        });
                    }
                    else {
                        var instance = this.$uibModal.open({
                            animation: true,
                            templateUrl: "/app/modal.templates/multiviewAnswers.html",
                            controller: "app.modal.templates.MultiviewAnswersController",
                            controllerAs: "vm",
                            size: 'xxl',
                            backdrop: false,
                            keyboard: false,
                            resolve: {
                                section: () => {
                                    return row;
                                },
                                questions: () => {
                                    return scope.questions;
                                },
                                appType: () => {
                                    return scope.appType;
                                },
                                accessType: () => {
                                    return scope.accessType;
                                },
                                organization: () => {
                                    return scope.organization;
                                },
                                appDueDate: () => {
                                    return scope.dueDate;
                                },
                                appUniqueId: () => {
                                    return scope.appUniqueId;
                                },
                                multiSites: () => {
                                    return scope.multiSites;
                                },
                                site: () => {
                                    return scope.site;
                                }
                            }
                        });

                        instance.result.then(() => {
                            //use reloadSections if you want to reload the section from db
                            this.common.$broadcast(this.config.events.reloadSections, { section: scope.row });
                            this.notificationFactory.success("Review saved successfully.");
                        }, () => {
                        });
                    }
                }
                else if (scope.isReview) {
                    var selectedItem = scope.row;

                    var roleName = this.common.currentUser.role.roleName.toLowerCase();
                    var isUser: boolean = false;

                    if (roleName === "user") {
                        isUser = true;
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
                                    section: selectedItem,
                                    appUniqueId: scope.appUniqueId,
                                    isUser: isUser,
                                    organization: scope.organization
                                };
                            }
                        }
                    });

                    instance.result.then((data: any) => {
                        scope.row = data;
                        //use reloadSections if you want to reload the section from db
                        //this.common.$broadcast(this.config.events.reloadSections, {section: scope.row});
                        scope.setCircle();
                        this.common.$broadcast('CheckStatus', { rowId: row.id, section: data });
                        this.notificationFactory.success("Review saved successfully.");
                    }, () => {
                    });
                }
                else {

                    //if (row.questions.length === 0) {
                    //    return;
                    //}
                    var values = {
                        section: () => {
                            return row;
                        },
                        questions: () => {
                            return scope.questions;
                        },
                        appType: () => {
                            return scope.appType;
                        },
                        accessType: () => {
                            return scope.accessType;
                        },
                        organization: () => {
                            return scope.organization;
                        },
                        appDueDate: () => {
                            return scope.dueDate;
                        },
                        appUniqueId: () => {
                            return scope.appUniqueId;
                        },
                        submittedDate: () => {
                            return scope.appSubmittedDate;
                        },
                        site: () => {
                            return scope.site;
                        },
                        reqId: () => {
                            return "";
                        },
                        appId: () => {
                            return "";
                        },
                        appStatus: () => {
                            return scope.appStatus;
                        }

                    };
                    
                    this.modalHelper.showModal("/app/modal.templates/application.html", "app.modal.templates.ApplicationController", values)
                        .then((data: any) => {
                            row.statusName = data.allAnswered === true ? this.config.applicationSectionStatuses.complete : this.config.applicationSectionStatuses.partial;

                            var isComplete = true;
                            var appDate = new Date(scope.appSubmittedDate);

                            for (var i = 0; i < row.questions.length; i++) {
                                var q = row.questions[i];

                                if (q.answerResponseStatusName !== this.config.applicationSectionStatuses.rfi) continue;

                                var found = _.find(q.applicationResponseComments, (c: services.IApplicationResponseComment) => {
                                    var created = new Date(c.createdDate);

                                    return created > appDate &&
                                        c.commentType.name === this.config.applicationSectionStatuses.rfi;
                                });

                                if (!found) {
                                    isComplete = false;
                                    break;
                                }
                            }

                            if (isComplete && row.circle === this.config.applicationSectionStatuses.rfi) {
                                console.log('complete');
                                row.circle = this.config.applicationSectionStatuses.rfiCompleted;
                                row.circleStatusName = this.config.applicationSectionStatuses.rfiCompleted.replace(" ", "");
                                row.statusName = this.config.applicationSectionStatuses.rfiCompleted;
                                scope.statusName = this.config.applicationSectionStatuses.rfiCompleted;
                            }

                            //use reloadSections if you want to reload the section from db
                            //this.common.$broadcast(this.config.events.reloadSections, { section: scope.row });
                            this.common.$broadcast('AppChanged', { rowId: row.id, setFlag: data.allAnswered });
                            this.common.$broadcast('CheckQuestions', { section: data.section, rowId: row.id });

                            this.notificationFactory.success("Section saved successfully.");
                        })
                        .catch(() => {
                            //do this when modal cancelled.
                        });
                    
                }
            }

            scope.onBulkUpdate = (row, event) => {
                event.stopPropagation();

                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/bulkStatusUpdate.html",
                    controller: "app.modal.templates.BulkStatusUpdateController",
                    controllerAs: "vm",
                    size: 'lg',
                    windowClass: 'app-modal-window',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        parentSection: () => {
                            return row;
                        },
                        appType: () => {
                            return scope.appType;
                        },
                        organization: () => {
                            return scope.organization;
                        },
                        appUniqueId: () => {
                            return scope.appUniqueId;
                        }
                    }
                });

                instance.result.then((response: IBulkResponse) => {
                    if (response) {
                        row = response.section;

                        this.common.$broadcast('BulkChanges', { section: response.section, rowId: row.id, fromStatus: response.fromStatus, toStatus: response.toStatus });
                    }
                }, () => {
                });
            }

            scope.checkAll = (section: app.services.IApplicationHierarchyData) => {
                var changed = false;

                if (section.questions && section.questions.length > 0) {
                    _.each(section.questions, (question) => {
                        if (scope.checkAnswers(question)) {
                            changed = true;
                        }
                    });
                }

                return changed;
            }

            scope.checkAnswers = (question: app.services.IQuestion) => {
                var changed = false;
                var isChanged = false;

                if (question.answers == null || question.answers == undefined || question.answers.length === 0) return changed;

                var isAnswered = _.find(question.answers, (a) => {
                    return a.selected;
                });

                if (!isAnswered) {
                    return changed;
                }

                if (scope.row.questions) { 
                    for (var i = 0; i < scope.row.questions.length; i++) {
                        var q: services.IQuestion = scope.row.questions[i];

                        if (q.id === "08970f36-6b35-4892-8cab-4e9fb8024928") {
                            debugger;
                        }

                        if (q.hiddenBy && q.hiddenBy.length > 0) {
                            changed = true;
                            var item = _.find(q.hiddenBy, (i) => {
                                return i.hiddenByQuestionId === question.id;
                            });

                            if (!item) {
                                continue;
                            }

                            q.isHidden = false;

                            for (var j = 0; j < q.hiddenBy.length; j++) {
                                var hidden = q.hiddenBy[j];
                                if (hidden.hiddenByQuestionId === question.id) {
                                    var find = _.find(question.answers, (a) => {
                                        return a.id === hidden.answerId && a.selected;
                                    });

                                    if (find) {
                                        q.isHidden = true;
                                        isChanged = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                

                if (isChanged) {
                    console.log(scope.row); }

                return changed;
            }

            scope.setCircle = () => {
                this.common.setCircleReviewer(scope.row);
                this.common.$broadcast('CircleChanged', {});
            }

            scope.setCircleChange = () => {
                if (scope.row.children && scope.row.children.length > 0) {
                    if (this.common.isReviewer()) {
                        this.common.setCircleReviewerFromChild(scope.row);
                    } else {
                        this.common.setCircleApplicantFromChild(scope.row);
                    }
                } else {
                    if (this.common.isReviewer()) {
                        this.common.setCircleReviewer(scope.row);
                    } else {
                        this.common.setCircleApplicantFromQuestions(scope.row);
                    }
                }

                this.common.$broadcast('CircleChanged', {});
            }

            this.$rootScope.$on("BulkChanges", (data: any, args: any) => {
                if (args.section && this.common.containsRow(args.section, scope.row.id, false)) {
                    if (this.processBulkChange(scope.row, args.fromStatus, args.toStatus) || scope.row.id === args.section.id) {
                        scope.setCircleChange();
                        this.common.$broadcast('CheckStatus', { rowId: scope.row.id, section: scope.row });
                    }
                }
            });

            this.$rootScope.$on("CheckStatus", (data: any, args: any) => {
                if (args.section && this.common.containsRow(scope.row, args.rowId, false) && scope.row.id !== args.section.id) {
                    if (scope.row.children && scope.row.children.length > 0) {
                        this.common.setCircleReviewerFromChild(scope.row);
                        this.common.$broadcast('CheckStatus', { rowId: scope.row.id, section: scope.row });
                        //scope.setCircleWithChildren();
                    }
                }
            });

            this.$rootScope.$on('CheckQuestions', (data: any, args: any) => {
                if (args.section) {
                    var changed = scope.checkAll(args.section);

                    if (changed) {
                        var found = _.find(scope.row.questions, (q: services.IQuestion) => {
                            return !q.isHidden;
                        });

                        if (found && !scope.row.isVisible) {
                            scope.row.isVisible = true;
                            this.common.$broadcast('AppChanged', { rowId: scope.row.id, setFlag: false });
                        } else if (!found && scope.row.isVisible) {
                            scope.row.isVisible = false;
                            this.common.$broadcast('AppChanged', { rowId: scope.row.id, setFlag: false });
                        }
                    }
                }
            });

            this.$rootScope.$on('SectComplete', (data: any, args: any) => {
                if (this.common.containsRow(scope.row, args.rowId, args.setFlag)) {
                    scope.complete = 0;
                    scope.total = 0;
                    scope.includes = [];
                    _.each(scope.row.children, (child) => {
                        if ((child.statusName === "Complete" || child.statusName === "RFI Completed") && child.isVisible) {
                            scope.complete++;
                        } else if (child.uniqueIdentifier === args.uniqueIdentifier && child.isVisible) {
                            scope.complete++;
                        }

                        var status = scope.row.circleStatusName;
                        scope.setCircleChange();

                        if (scope.row.circleStatusName !== status) {
                            this.common.$broadcast('SectComplete', { rowId: scope.row.id, uniqueIdentifier: scope.row.uniqueIdentifier });
                        }

                        if (this.includeInTotal(child)) {
                            var included = _.find(scope.includes, (i) => {
                                return i === child.uniqueIdentifier;
                            });

                            if (!included) {
                                scope.total++;
                                scope.includes.push(child.uniqueIdentifier);    
                            }
                        }
                    });
                }
            });

            this.$rootScope.$on('SectNotComplete', (data: any, args: any) => {
                if (this.common.containsRow(scope.row, args.rowId, args.setFlag)) {
                    scope.complete = 0;
                    scope.total = 0;
                    _.each(scope.row.children, (child) => {
                        if (child.statusName === "Complete" || child.statusName === "RFI Complete") {
                            if (child.uniqueIdentifier === args.uniqueIdentifier) {
                                child.statusName = "Partial";
                            } else {
                                scope.complete++;
                            }
                        }

                        if (this.includeInTotal(child)) {
                            scope.total++;
                        }
                    });
                }
            });
            
            this.$rootScope.$on('AppChanged', (data: any, args: any) => {
                if (this.common.containsRow(scope.row, args.rowId, args.setFlag)) {
                    if (scope.statusName !== "Complete" && scope.statusName !== "RFI Completed") {
                        var found = _.find(scope.row.children,
                            (c: any) => {
                                return c.statusName !== "Complete" && c.statusName !== "RFI Completed";
                            });

                        if (!found) {
                            scope.statusName = "Complete";
                            scope.setCircleChange();
                            //this.common.$broadcast('AppChanged', { rowId: scope.row.id });
                            this.common.$broadcast('SectComplete', { rowId: scope.row.id, uniqueIdentifier: scope.row.uniqueIdentifier });
                        } else {
                            this.common.$broadcast('SectNotComplete', { rowId: scope.row.id, uniqueIdentifier: scope.row.uniqueIdentifier });
                        }
                    } else {
                        this.common.$broadcast('SectComplete', { rowId: scope.row.id });
                    }
                    scope.complete = 0;
                    scope.total = 0;
                    _.each(scope.row.children, (child) => {
                        if (child.statusName === "Complete" || child.statusName === "RFI Completed") {
                            scope.complete++;
                        }

                        if (this.includeInTotal(child)) {
                            scope.total++;
                        }
                    });

                    var items = _.find(scope.row.children, (child: any) => {
                        return child.isVisible;
                    });

                    if (!items) {
                        items = _.find(scope.row.questions, (q: services.IQuestion) => {
                            return !q.isHidden;
                        });

                        if (!items && scope.row.isVisible) {
                            scope.row.isVisible = false;
                            this.common.$broadcast('AppChanged', { rowId: scope.row.id, setFlag: false });
                        }

                    } else if (!scope.row.isVisible) {
                        scope.row.isVisible = true;
                        this.common.$broadcast('AppChanged', { rowId: scope.row.id, setFlag: false });
                    }
                }

            });
        }
    }

    angular
        .module('app.widgets')
        .directive('applicationRequirement', ApplicationRequirement.factory());
}