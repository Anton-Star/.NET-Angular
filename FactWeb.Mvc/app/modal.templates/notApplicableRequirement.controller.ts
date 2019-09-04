module app.modal.templates {
    'use strict';

    interface IValues {
        requirements: Array<services.IApplicationHierarchyData>;
        notApplicables: Array<services.ISiteApplicationVersionQuestionNotApplicable>;
    }

    class NotApplicableRequirementController {
        selectedAnswer: services.IAnswer = null;
        questionTypes: Array<services.IQuestionType>;
        scopeTypes: Array<services.IScopeType>;
        displayQuestion: services.IQuestion;
        newHides: Array<services.IQuestion> = null;
        isBusy = false;
        isNew: boolean;
        radioButtonAnswers = [{ id: '', active: true, order: 1, text: 'Yes', isExpectedAnswer: true, questionId: '', selected: true }, { id: '', active: true, order: 2, text: 'No', isExpectedAnswer: false, questionId: '', selected: false }];
        grid: kendo.ui.Grid;
        hidesGrid: kendo.ui.Grid;
        editingRow: services.IAnswer;
        showChildren = false;

        static $inject = [
            '$scope',
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'scopeTypeService',
            'questionService',
            'answerService',
            'values'
        ];

        constructor(
            private $scope: ng.IScope,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private scopeTypeService: services.IScopeTypeService,
            private questionService: services.IQuestionService,
            private answerService: services.IAnswerService,
            private values: IValues) {

            this.setNotApplicables(values.requirements);
        }

        setNotApplicables(reqs: Array<services.IApplicationHierarchyData>) {
            _.each(reqs, (req) => {
                req.isNotApplicable = false;
                if (req.questions != undefined && req.questions != null && req.questions.length > 0) {
                    _.each(req.questions, (question) => {
                        var found = _.find(this.values.notApplicables, (notApplicable) => {
                            return question.id === notApplicable.questionId;
                        });

                        if (found) {
                            question.isNotApplicable = true;
                            
                        } else {
                            question.isNotApplicable = false;
                        }
                    });

                    var hasApplicable = _.find(req.questions, (question) => {
                        return question.isNotApplicable === false;
                    });

                    if (!hasApplicable) {
                        req.isNotApplicable = true;
                    }
                }
            
                if (req.hasChildren) {
                    this.setNotApplicables(req.children);

                    var applicable = _.find(req.children, (child) => {
                        return child.isNotApplicable === false;
                    });

                    if (!applicable) {
                        req.isNotApplicable = true;
                    }
                } else if (!req.questions || req.questions.length === 0) {
                    req.isNotApplicable = true;
                }
            });
        }

        onNotApplicableRow = (row, event) => {
            event.stopPropagation();
            if (row.isNotApplicable == undefined) row.isNotApplicable = false;
            this.setItems(row, !row.isNotApplicable);
        }

        setItems = (item, value) => {
            if (item.isNotApplicable == undefined) item.isNotApplicable = false;

            item.isNotApplicable = value;
            if (item.questions != undefined && item.questions != null && item.questions.length > 0) {
                _.each(item.questions, (question) => {
                    question.isNotApplicable = item.isNotApplicable;
                });
            }

            if (item.children != undefined && item.children != null && item.children.length > 0) {
                _.each(item.children, (child) => {
                    this.setItems(child, value);
                });
            }
        }

        onSave(): void {
            this.$uibModalInstance.close(this.values.requirements);
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.NotApplicableRequirementController',
        NotApplicableRequirementController);
} 