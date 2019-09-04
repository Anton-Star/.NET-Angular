module app.modal.templates {
    'use strict';

    interface IValues {
        requirement: services.IApplicationHierarchyData,
        question: services.IQuestion,
        isNew: boolean;
        questionTypes: Array<services.IQuestionType>;
        scopeTypes: Array<services.IScopeType>;
        radioButtonAnswers: Array<services.IAnswer>;
        parent: services.IApplicationHierarchyData;
        questions: Array<services.IQuestion>;
    }
    
    class QuestionController {
        selectedAnswer: services.IAnswer = null;
        questionTypes: Array<services.IQuestionType>;
        scopeTypes: Array<services.IScopeType>;
        displayQuestion: services.IQuestion;
        question: services.IQuestion;
        newHides: Array<services.IQuestion> = null;
        isBusy = false;
        isNew: boolean;        
        radioButtonAnswers = [{ id: '', active: true, order: 1, text: 'Yes', isExpectedAnswer: true, questionId: '', selected: true }, { id: '', active: true, order: 2, text: 'No', isExpectedAnswer: false, questionId: '', selected: false }];
        grid: kendo.ui.Grid;
        hidesGrid: kendo.ui.Grid;
        questionsGrid: kendo.ui.Grid;
        editingRow: services.IAnswer;        
        gridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: "row",
            toolbar: [{ name: "create", text: "Add" }],
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            schema: {
                model: {
                    id: "id",
                    fields: {
                        id: { type: "string" },
                        text: {
                            type: "string",
                            validation: {
                                required: true
                            }
                        },
                        order: {
                          type: "number"  
                        },
                        isExpectedAnswer: {
                            type: "boolean",
                            validation: {
                                required: true
                            },
                            defaultValue: false
                        }
                    }
                }
            },
            pageable: {
                pageSize: 10
            },
            edit: (e) => {
                this.editingRow = e.model;
                this.editingRow.isExpectedAnswer = this.editingRow.isExpectedAnswer || false;
                this.editingRow.questionId = this.displayQuestion.id;
            },
            save: () => {
                this.saveAnswer();
            },
            remove: (e) => {
                if (e.model.id) {
                    this.removeAnswer(e.model.id);
                }
            },
            change: (e) => {
                this.onRowSelected(e);
            }
        };

        hidesGridOptions = {
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
            schema: {
                model: {
                    id: "id",
                    fields: {
                        id: { type: "string" },
                        text: {
                            type: "string",
                            validation: {
                                required: true
                            }
                        }
                    }
                }
            },
            pageable: {
                pageSize: 10
            },
            remove: (e) => {
                if (e.model.id) {
                    this.removeHides(e.model.id);
                }
            }
        };
        filter = "";
        selectAll = false;
        questionsGridOptions = {
            sortable: true,
            selectable: "multiple",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            },
            change: (e) => {
                this.onQuestionRowSelected(e);
            }
        };

        static $inject = [
            '$scope',
            '$q',
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
            private $q: ng.IQService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private scopeTypeService: services.IScopeTypeService,
            private questionService: services.IQuestionService,
            private answerService: services.IAnswerService,
            private values: IValues) {
        
            this.isNew = this.values.isNew;
            this.questionTypes = this.values.questionTypes;
            this.scopeTypes = this.values.scopeTypes;

            console.log(values);
            
            if (this.values.isNew) { // new question
                this.displayQuestion = {
                    sectionId: this.values.requirement.id,
                    answers: this.radioButtonAnswers
                };

                var maxCompNumber = _.maxBy(this.values.requirement.questions, 'complianceNumber');

                this.displayQuestion.complianceNumber = (maxCompNumber ? maxCompNumber.complianceNumber : 0) + 1;
                
                if (this.values.parent != null) { // show current requirement's parent scope types
                    _.forEach(this.scopeTypes, (scopeType: services.IScopeType) => {
                        var found = _.find(this.values.parent.scopeTypes, (type: services.IScopeType) => {
                            return type != null && scopeType.scopeTypeId === type.scopeTypeId;
                        });

                        if (found) {
                            scopeType.isSelected = true;
                        } else {
                            scopeType.isSelected = false;
                        }
                    });
                }
                else { // show current requirement scope types
                    _.forEach(this.scopeTypes, (scopeType: services.IScopeType) => {
                        var found = _.find(this.values.requirement.scopeTypes, (type: services.IScopeType) => {
                            return type != null && scopeType.scopeTypeId === type.scopeTypeId;
                        });

                        if (found) {
                            scopeType.isSelected = true;
                        } else {
                            scopeType.isSelected = false;
                        }
                    });
                }
            } else { // show saved scope types in question       
                        
                this.displayQuestion = this.values.question;
                this.displayQuestion.sectionId = this.values.requirement.id;
                _.forEach(this.scopeTypes, (scopeType: services.IScopeType) => {
                    var found = _.find(this.displayQuestion.scopeTypes, (type: services.IScopeType) => {
                        return type != null && scopeType.scopeTypeId === type.scopeTypeId;
                    });

                    if (found) {
                        scopeType.isSelected = true;
                    } else {
                        scopeType.isSelected = false;
                    }
                });

                if (this.displayQuestion && this.displayQuestion.answers) {
                    var displays = _.find(this.displayQuestion.answers, (q) => {
                        return q.hidesQuestions != null;
                    });    

                    if (!displays) {
                        this.getDisplays();
                    }
                }
                

                _.forEach(this.questionTypes, (questionType: services.IQuestionType) => {
                    this.setQuestionTypeSelected(questionType);
                });
            }

            this.gridOptions.dataSource.data(this.displayQuestion.answers);
        }

        getDisplays() {
            this.questionService.getDisplays(this.displayQuestion.id)
                .then((data: services.IQuestionAnswerDisplay[]) => {
                    _.each(data, (d) => {
                        var answer = _.find(this.displayQuestion.answers, (a: services.IAnswer) => {
                            return a.id === d.answerId;
                        });

                        if (answer) {
                            answer.hidesQuestions = answer.hidesQuestions || [];
                            answer.hidesQuestions.push(d);
                        }
                    });
                });
        }

        responseTypeChange(): void {
        
            if (this.displayQuestion.type === "Radio Buttons") {
                this.displayQuestion.questionTypeFlags = null;
                this.displayQuestion.answers = this.radioButtonAnswers;
                this.gridOptions.dataSource.data(this.displayQuestion.answers);
            }
            else if (this.displayQuestion.type === "Multiple") {
                this.displayQuestion.questionTypeFlags = {
                    textArea: false,
                    radioButtons: false,
                    checkboxes: false,
                    documentUpload: false,
                    date: false,
                    dateRange: false,
                    peoplePicker: false,
                    textBox: false
                };
            }
            else if (this.values.isNew === false) {
                this.displayQuestion.questionTypeFlags = null;
                this.displayQuestion.answers = this.values.question.answers;
                this.gridOptions.dataSource.data(this.displayQuestion.answers);
            }
        }

        setQuestionTypeSelected(questionType: services.IQuestionType) {
            if (this.displayQuestion.questionTypeFlags == undefined || this.displayQuestion.questionTypeFlags == null) {
                questionType.isSelected = false;
                return;
            }

            switch (questionType.name) {
                case "Text Area":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.textArea || false;
                    break;
                case "Checkboxes":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.checkboxes || false;
                    break;
                case "Date":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.date || false;
                    break;
                case "People Picker":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.peoplePicker || false;
                    break;
                case "Radio Buttons":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.radioButtons || false;
                    break;
                case "Document Upload":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.documentUpload || false;
                    break;
                case "Date Range":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.dateRange || false;
                    break;
                case "Text Box":
                    questionType.isSelected = this.displayQuestion.questionTypeFlags.textBox || false;
                    break;
            }
        }

        onAnswerTypeSelected(questionType: services.IQuestionType): void {
            switch (questionType.name) {
                case "Text Area":
                    this.displayQuestion.questionTypeFlags.textArea = !this.displayQuestion.questionTypeFlags.textArea;
                    break;
                case "Checkboxes":
                    this.displayQuestion.questionTypeFlags.checkboxes = !this.displayQuestion.questionTypeFlags.checkboxes;
                    break;
                case "Date":
                    this.displayQuestion.questionTypeFlags.date = !this.displayQuestion.questionTypeFlags.date;
                    break;
                case "People Picker":
                    this.displayQuestion.questionTypeFlags.peoplePicker = !this.displayQuestion.questionTypeFlags.peoplePicker;
                    break;
                case "Radio Buttons":
                    this.displayQuestion.questionTypeFlags.radioButtons = !this.displayQuestion.questionTypeFlags.radioButtons;
                    break;
                case "Document Upload":
                    this.displayQuestion.questionTypeFlags.documentUpload = !this.displayQuestion.questionTypeFlags.documentUpload;
                    break;
                case "Date Range":
                    this.displayQuestion.questionTypeFlags.dateRange = !this.displayQuestion.questionTypeFlags.dateRange;
                    break;
                case "Text Box":
                    this.displayQuestion.questionTypeFlags.textBox = !this.displayQuestion.questionTypeFlags.textBox;
                    break;
            }
        }

        isExpectedValueEditor = (container) => {
            $('<input type="checkbox" ng-click="vm.expectedAnswerChanged()"/>').appendTo(container);
        }

        expectedAnswerChanged(): void {
            this.editingRow.isExpectedAnswer = !this.editingRow.isExpectedAnswer;
        }

        isInvalid(): boolean {
            var lengthValid = true;
            if (this.displayQuestion.type === "Radio Buttons" || this.displayQuestion.type === "Checkboxes") {
                if (this.gridOptions.dataSource.data.length === 0) {
                    lengthValid = false;
                }
            }

            return this.displayQuestion.complianceNumber == undefined || this.displayQuestion.complianceNumber.toString() === "" ||
                this.displayQuestion.text == undefined || this.displayQuestion.text === "" ||
                this.displayQuestion.type == undefined || this.displayQuestion.type === "" || !lengthValid;
        }

        updateQuestion(): void {
        }

        saveAnswer(): void {
            this.common.showSplash();
            
            this.answerService.save(this.editingRow)
                .then((data: services.IGenericServiceResponse<string>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        
                        if (!this.editingRow.id) {
                            this.editingRow.isExpectedAnswer = this.editingRow.isExpectedAnswer || false;
                            this.editingRow.id = data.item;
                            this.displayQuestion.answers.push(this.editingRow);
                        } else {
                            for (var i = 0; i < this.displayQuestion.answers.length; i++) {
                                if (this.displayQuestion.answers[i].id === this.editingRow.id) {
                                    this.displayQuestion.answers[i].order = this.editingRow.order;
                                    this.displayQuestion.answers[i].isExpectedAnswer = this.editingRow.isExpectedAnswer;
                                    this.displayQuestion.answers[i].text = this.editingRow.text;
                                    break;
                                }
                            }
                        }
                        
                        this.notificationFactory.success("Answer Saved successfully.");
                        this.grid.refresh();
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error occurred. Please contact support.");
                    this.common.hideSplash();
                });
        }

        removeAnswer(id: string): void {
            this.common.showSplash();

            this.answerService.remove(id)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                        this.gridOptions.dataSource.data(this.displayQuestion.answers);
                    } else {
                        for (var i = 0; i < this.displayQuestion.answers.length; i++) {
                            if (this.displayQuestion.answers[i].id === id) {
                                this.displayQuestion.answers.splice(i, 1);
                                break;
                            }
                        }

                        this.notificationFactory.success(data.message);
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error occurred. Please contact support.");
                    this.common.hideSplash();
                });
        }

        removeHides(id: string): void {
            this.common.showSplash();

            this.answerService.removeHides(id)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success(data.message);
                        _.remove(this.selectedAnswer.hidesQuestions, (display: services.IQuestionAnswerDisplay) => {
                            return display.id === id;
                        });
                    }
                    var items = angular.copy(this.selectedAnswer.hidesQuestions);
                    this.hidesGridOptions.dataSource.data(items || []);
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error occurred. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onRowSelected(event): void {
            var grid = event.sender;
            var selectedItem = grid.dataItem(grid.select());
            var answer = _.find(this.displayQuestion.answers, (a) => {
                return a.id === selectedItem.id;
            });
            this.selectedAnswer = answer;
            var items = angular.copy(this.selectedAnswer.hidesQuestions);
            this.hidesGridOptions.dataSource.data(items || []);
            this.$scope.$apply();
        }

        cancelAddHides(): void {
            this.newHides = null;
        }

        saveHides(): void {
            this.common.showSplash();

            this.answerService.addHides(this.selectedAnswer.id, this.newHides)
                .then((data: services.IGenericServiceResponse<Array<services.IQuestionAnswerDisplay>>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        _.each(data.item, (display: services.IQuestionAnswerDisplay) => {
                            this.selectedAnswer.hidesQuestions.push(display);
                        });
                        this.cancelAddHides();
                        this.notificationFactory.success(data.message);
                    }
                    var items = angular.copy(this.selectedAnswer.hidesQuestions);
                    this.hidesGridOptions.dataSource.data(items || []);
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error occurred. Please contact support.");
                    this.common.hideSplash();
                });
        }

        addHides(): void {
            this.newHides = [];
            var qs = _.filter(this.values.questions, (question: services.IQuestion) => {
                var isFound = _.find(this.selectedAnswer.hidesQuestions, (q: services.IQuestionAnswerDisplay) => {
                    return (q.questionId === question.id || question.id === this.values.question.id);
                });

                if (isFound) {
                    return false;
                } else {
                    return true;
                }
            });

            if (this.filter !== "") {
                qs = _.filter(qs, (question) => {
                    return question.sectionUniqueIdentifier.toLowerCase().indexOf(this.filter.toLowerCase()) > -1 ||
                        (question.complianceNumber && question.complianceNumber.toString().toLowerCase().indexOf(this.filter.toLowerCase()) > -1) ||
                        question.text.toLowerCase().indexOf(this.filter.toLowerCase()) > -1;
                });
            }

            this.questionsGridOptions.dataSource.data(qs);
        }

        closeHides(): void {
            this.selectedAnswer = null;
            this.cancelAddHides();
            this.hidesGrid.clearSelection();
        }

        onQuestionRowSelected(event): void {
            this.setHides();
        }

        setHides() {
            this.newHides = [];
            var items = this.questionsGrid.select();
            _.each(items, (i) => {
                var item: any = this.questionsGrid.dataItem($(i));
                this.newHides.push(item);
            });
            
        }

        onSelectAllHides() {
            var grid = $('#questionsGrid').data('kendoGrid');

            if (this.selectAll) {
                grid.select(grid.tbody.find(">tr"));
            } else {
                grid.refresh();
            }

            this.setHides();
        }

        save(): void {
            this.isBusy = true;
            
            var scopes = [];
            _.each(this.scopeTypes, (scopeType: services.IScopeType) => {
                if (scopeType.isSelected) {
                    scopes.push(scopeType);
                }
            });
            this.displayQuestion.scopeTypes = scopes;

            this.common.showSplash();

            this.questionService.save(this.displayQuestion)
                .then((data: services.IGenericServiceResponse<services.IQuestion>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        
                        this.question = data.item;
                        this.question.scopeTypes = scopes;
                                            
                        if (this.displayQuestion.type == "Radio Buttons" && this.values.isNew == true) {
                            var items = [];

                            _.each(this.radioButtonAnswers, (radioButtonAnswer: services.IAnswer) => {
                                radioButtonAnswer.questionId = data.item.id;
                                items.push(this.saveAnswerEnd(radioButtonAnswer));
                            });

                            this.$q.all(items).then(() => {
                                this.notificationFactory.success("Question Saved successfully.");
                                this.$uibModalInstance.close({ question: this.question, saved: true });
                            });
                        } else {
                            this.notificationFactory.success("Question Saved successfully.");
                            this.$uibModalInstance.close({ question: this.question, saved: true });
                        }
                    }
                    this.isBusy = false;
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to connect with web service. Please contact support.");
                    this.isBusy = false;
                    this.common.hideSplash();
                });
        }

        saveAnswerEnd(radioButtonAnswer: services.IAnswer): ng.IPromise<void> {
            return this.answerService.save(radioButtonAnswer)
                .then((answerData: services.IGenericServiceResponse<string>) => {
                    if (answerData.hasError) {
                        this.notificationFactory.error(answerData.message);
                    } else {
                        radioButtonAnswer.id = answerData.item;
                        this.question.answers.push(radioButtonAnswer);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error occurred. Please contact support.");
                });
        }

        cancel(): void {
            this.$uibModalInstance.close({ question: this.displayQuestion, saved: false });
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.QuestionController',
        QuestionController);
} 