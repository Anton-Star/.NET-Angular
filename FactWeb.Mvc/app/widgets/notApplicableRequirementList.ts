

module app.widgets {
    'use strict';

    export interface INotApplicableRequirementListScope extends ng.IScope {
        rows: Array<services.IApplicationSection>;
        applicationtype: string;
        questionTypes: Array<services.IQuestionType>;
        id: string;
        addNew(): void;
        edit(): void;
        add: Function;
    }

    export class NotApplicableRequirementListController {
        public static RowSelected = "RowSelected";
        public static RowEdit = "RowEdit";

        static $inject = [
            '$scope'
        ];

        constructor() {
        }
    }

    class NotApplicableRequirementList implements ng.IDirective {
        constructor(common: common.ICommonFactory) {
            NotApplicableRequirementList.prototype.link = (scope: IRequirementListScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
                scope.addNew = () => {
                    common.$broadcast(RequirementListController.RowSelected, {
                        rowId: scope.id
                    });
                };
            }
        }

        static factory(): ng.IDirectiveFactory {
            var directive = (common: common.ICommonFactory) => new NotApplicableRequirementList(common);
            directive.$inject = ['common'];
            return directive;
        }

        restrict = 'E';

        scope = {
            rows: "=ngModel",
            applicationtype: "@",
            edit: "&",
            questionTypes: "=questionTypes",
            scopeTypes: "=scopeTypes",
            isRequirementSet: "@isRequirementSet",
            parent: "=parent",
            questions: "=questions",
            currentVersion: "=currentVersion"
        };
        public link: (scope: IRequirementListScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => void;
        controller: NotApplicableRequirementListController;
        templateUrl = "/app/widgets/notApplicableRequirementList.html";

    }
    angular
        .module('app.widgets')
        .directive('notApplicableRequirementList', NotApplicableRequirementList.factory());
}