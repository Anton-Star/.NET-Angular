

module app.widgets {
    'use strict';

    class Requirement implements ng.IDirective {

        constructor(private $compile: ng.ICompileService,
            private $templateRequest: ng.ITemplateRequestService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private config: IConfig,
            private common: common.ICommonFactory,
            private notificationFactory: blocks.INotificationFactory,
            private requirementService: services.IRequirementService,
            private questionService: services.IQuestionService) {           
        }

        static factory(): ng.IDirectiveFactory {
            var directive = (
                $compile: any,
                $templateRequest,
                $uibModal: ng.ui.bootstrap.IModalService,
                config: IConfig,
                common: common.ICommonFactory,
                notificationFactory: blocks.INotificationFactory,
                requirementService: services.IRequirementService,
                questionService: services.IQuestionService) =>
                    new Requirement($compile, $templateRequest, $uibModal, config, common, notificationFactory, requirementService, questionService);

            directive.$inject = [
                '$compile',
                '$templateRequest',
                '$uibModal',
                'config',
                'common',
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
            currentVersion: "=currentVersion"
        };

        link = (scope: any, element: any, attributes: any) => {
            scope.isOpen = false;
            scope.row.isEdit = false;
            scope.isHidden = false;
            scope.row.applicationTypeName = scope.applicationtype;

            if (angular.isArray(scope.row.children) && scope.row.children.length > 0) {
                this.$templateRequest("/app/widgets/requirementChildren.html").then((html) => {
                    var template = angular.element(html);
                    element.append(template);
                    this.$compile(template)(scope);
                });
            } else {
                this.$templateRequest("/app/widgets/requirement.html").then((html) => {
                    var template = angular.element(html);
                    element.append(template);
                    this.$compile(template)(scope);
                });
            }

            scope.add = () => {
                event.stopPropagation();                
                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/requirement.html",
                    controller: "app.modal.templates.RequirementController",
                    controllerAs: "vm",
                    size: 'lg',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        requirement: () => {
                            return scope.row;
                        },
                        isNew: () => {
                            return true;
                        },
                        parent: () => {
                            return scope.parent;
                        },
                        isRequirementSet: () => {
                            return false;
                        },
                        scopeType: () => {
                            return scope.scopeTypes;
                        },
                        currentVersion: () => {
                            return scope.currentVersion;
                        }
                    }
                });

                instance.result.then((item) => {
                    if (item.saved) {
                        scope.row.children.push(item.section);
                        scope.row.children = _.sortBy(scope.row.children, (c: services.IApplicationSection) => {
                            return c.order;
                        });
                    }

                    this.common.$broadcast(this.config.events.requirementSaved);
                }, () => {
                    });
            };

            scope.edit = () => {
                scope.original = angular.copy(scope.row);
                event.stopPropagation();
                
                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/requirement.html",
                    controller: "app.modal.templates.RequirementController",
                    controllerAs: "vm",
                    size: 'lg',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        requirement: () => {
                            return scope.row;
                        },
                        isNew: () => {
                            return false;
                        },
                        parent: () => {
                            return scope.parent;
                        },
                        isRequirementSet: () => {
                            return false;
                        },
                        scopeType: () => {
                            return scope.scopeTypes;
                        },
                        currentVersion: () => {
                            return scope.currentVersion;
                        }
                    }
                });

                instance.result.then((item) => {
                    if (item.saved) {
                        scope.row.uniqueIdentifier = item.section.uniqueIdentifier;
                        scope.row.order = item.section.order;
                        scope.row.name = item.section.name;
                        scope.row.helpText = item.section.helpText;
                        scope.row.version = item.section.version;
                    }
                    

                    //this.common.$broadcast(this.config.events.requirementSaved);
                }, () => {
                    });

                scope.row.isEdit = true;
            };

            scope.delete = () => {
                event.stopPropagation();

                if (!confirm("Are you sure you want to remove this requirement?")) {
                    return;
                }

                this.common.showSplash();

                this.requirementService.remove(scope.id)
                    .then((data: services.IServiceResponse) => {
                        this.common.hideSplash();

                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            scope.isHidden = true;
                            this.notificationFactory.success("Requirement removed successfully.");
                            //this.common.$broadcast(this.config.events.requirementSaved);
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Unable to save. Please contact support.");
                        this.common.hideSplash();
                    });
            };

            scope.cancel = () => {
                event.stopPropagation();
                scope.row = scope.original;
                scope.row.isEdit = false;
            };

            scope.addQuestion = () => {
                scope.original = angular.copy(scope.row);
                event.stopPropagation();

                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/question.html",
                    controller: "app.modal.templates.QuestionController",
                    controllerAs: "vm",
                    size: 'lg',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        values: () => {
                            return {
                                requirement: scope.row,
                                isNew: true,
                                questionTypes: scope.questionTypes,
                                scopeTypes: scope.scopeTypes,
                                parent: scope.parent,
                                questions: scope.questions
                            };
                        }
                    }
                });

                instance.result.then((item) => {
                    if (item.saved) {
                        scope.row.questions.push(item.question);
                        scope.row.questions = _.sortBy(scope.row.questions, (q: services.IQuestion) => {
                            return q.complianceNumber;
                        });

                        //this.common.$broadcast(this.config.events.requirementSaved);
                    }
                    
                }, () => {
                    });

                scope.row.isEdit = true;
            };

            scope.deleteQuestion = (question: services.IQuestion) => {
                event.stopPropagation();

                if (!confirm("Are you sure you want to remove this question?")) {
                    return;
                }

                this.common.showSplash();

                this.questionService.remove(question.id)
                    .then((data: services.IServiceResponse) => {
                        this.common.hideSplash();

                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            for (var i = 0; i < scope.row.questions.length; i++) {
                                if (scope.row.questions[i].id === question.id) {
                                    scope.row.questions.splice(i, 1);
                                    break;
                                }
                            }

                            this.notificationFactory.success("Question removed successfully.");
                            this.common.$broadcast(this.config.events.requirementSaved);
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Unable to save. Please contact support.");
                        this.common.hideSplash();
                    });
            };

            scope.editQuestion = (question: services.IQuestion) => {
                scope.original = angular.copy(scope.row);
                event.stopPropagation();
                
                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/question.html",
                    controller: "app.modal.templates.QuestionController",
                    controllerAs: "vm",
                    size: 'lg',
                    backdrop: false,
                    keyboard: false,
                    resolve: {
                        values: () => {
                            return {
                                requirement: scope.row,
                                question: question,
                                isNew: false,
                                questionTypes: scope.questionTypes,
                                scopeTypes: scope.scopeTypes,
                                parent: scope.parent,
                                questions: scope.questions
                        };
                        }
                    }
                });

                $('.modal').on('shown', () => {
                    $(document).off('focusin.modal');
                });

                instance.result.then((item) => {
                    for (var i = 0; i < scope.row.questions.length; i++) {
                        if (scope.row.questions[i].id === item.question.id) {
                            
                            scope.row.questions[i] = item.question;
                            break;
                        }
                    }
                    //this.common.$broadcast(this.config.events.requirementSaved);
                }, () => {
                    });

                scope.row.isEdit = true;
            };
        }
    }

    angular
        .module('app.widgets')
        .directive('requirement', Requirement.factory());
}