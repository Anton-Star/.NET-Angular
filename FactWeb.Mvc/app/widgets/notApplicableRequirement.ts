

module app.widgets {
    'use strict';

    class NotApplicableRequirement implements ng.IDirective {

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
                new NotApplicableRequirement($compile, $templateRequest, $uibModal, config, common, notificationFactory, requirementService, questionService);

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
            scope.row.isEdit = false;
            scope.row.applicationTypeName = scope.applicationtype;

            if (angular.isArray(scope.row.children) && scope.row.children.length > 0 && !scope.row.isNotApplicable) {
                this.$templateRequest("/app/widgets/notApplicableRequirementChildren.html").then((html) => {
                    var template = angular.element(html);
                    element.append(template);
                    this.$compile(template)(scope);
                });
            } else {
                this.$templateRequest("/app/widgets/notApplicableRequirement.html").then((html) => {
                    var template = angular.element(html);
                    element.append(template);
                    this.$compile(template)(scope);
                });
            }

            scope.onNotApplicable = (q: services.IQuestion) => {
                event.stopPropagation(); 
                q.isNotApplicable = !q.isNotApplicable;
            };

            scope.onNotApplicableRow = (row, event) => {
                event.stopPropagation(); 
                if (row.isNotApplicable == undefined) row.isNotApplicable = false;
                scope.setItems(row, !row.isNotApplicable);
            }

            scope.setItems = (item, value) => {
                if (item.isNotApplicable == undefined) item.isNotApplicable = false;

                item.isNotApplicable = value;
                if (item.questions != undefined && item.questions != null && item.questions.length > 0) {
                    _.each(item.questions, (question) => {
                        question.isNotApplicable = item.isNotApplicable;
                    });
                }

                if (item.children != undefined && item.children != null && item.children.length > 0) {
                    _.each(item.children, (child) => {
                        scope.setItems(child, value);
                    });
                }
            }
        }
    }

    angular
        .module('app.widgets')
        .directive('notApplicableRequirement', NotApplicableRequirement.factory());
}