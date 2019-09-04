

module app.widgets {
    'use strict';

    export interface IRequirementListScope extends ng.IScope {
        rows: Array<services.IApplicationSection>;
        applicationtype: string;
        questionTypes: Array<services.IQuestionType>;
        id: string;
        addNew(): void;
        edit(): void;
        add: Function;
    }

    export class RequirementListController {
        public static RowSelected = "RowSelected";
        public static RowEdit = "RowEdit";

        static $inject = [
            '$scope'
        ];

        constructor() {
        }  
    }

    class RequirementList implements ng.IDirective {
        constructor(common: common.ICommonFactory) {
            RequirementList.prototype.link = (scope: IRequirementListScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
                scope.addNew = () => {
                    common.$broadcast(RequirementListController.RowSelected, {
                        rowId: scope.id
                    });
                };
            }
        }

        static factory(): ng.IDirectiveFactory {
            var directive = (common: common.ICommonFactory) => new RequirementList(common);
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
        controller: RequirementListController;
        templateUrl = "/app/widgets/requirementList.html";
        
    }
    angular
        .module('app.widgets')
        .directive('requirementList', RequirementList.factory());
}