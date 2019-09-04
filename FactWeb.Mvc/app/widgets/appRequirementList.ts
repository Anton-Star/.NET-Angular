

module app.widgets {
    'use strict';

    class AppRequirementList implements ng.IDirective {
        static factory(): ng.IDirectiveFactory {
            var directive = () => new AppRequirementList();
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
        templateUrl = "/app/widgets/appRequirementList.html";

    }
    angular
        .module('app.widgets')
        .directive('appRequirementList', AppRequirementList.factory());
}