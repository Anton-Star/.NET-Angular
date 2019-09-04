

module app.widgets {
    'use strict';

    export class ApplicationRequirementListController {
        public static RowSelected = "RowSelected";
        public static RowEdit = "RowEdit";

        static $inject = [
            '$scope'
        ];

        constructor() {
        }        
    }

    class ApplicationRequirementList implements ng.IDirective {
        static factory(): ng.IDirectiveFactory {
            var directive = () => new ApplicationRequirementList();
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
            currentVersion: "=currentVersion",
            appType: "@appType",
            accessType: "@accessType",
            organization: "@organization",
            dueDate: "@duedate",
            submittedDate: "@submitteddate",
            appUniqueId: "@appUniqueId",
            isReview: "@isReview",
            isMultisite: "@isMultisite",
            site: "=site",
            multiSites: "=multiSites",
            accessToken: "=accessToken",
            level: "@level",
            appStatus: "@appStatus"
        };
        
        link(scope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes): void {
            scope.nextLevel = parseInt(scope.level || "0") + 1;
        };
        controller: RequirementListController;
        templateUrl = "/app/widgets/applicationRequirementList.html";

    }
    angular
        .module('app.widgets')
        .directive('applicationRequirementList', ApplicationRequirementList.factory());
}