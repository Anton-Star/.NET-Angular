

module app.widgets {
    'use strict';

    class ApplicationRequirement implements ng.IDirective {

        constructor(
            private $location: ng.ILocationService,
            private $rootScope: ng.IRootScopeService,
            private $compile: ng.ICompileService,
            private $templateRequest: ng.ITemplateRequestService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private config: IConfig,
            private common: common.ICommonFactory) {
        }

        static factory(): ng.IDirectiveFactory {
            var directive = (
                $location: ng.ILocationService,
                $rootScope: ng.IRootScopeService,
                $compile: any,
                $templateRequest,
                $uibModal: ng.ui.bootstrap.IModalService,
                config: IConfig,
                common: common.ICommonFactory) =>
                new ApplicationRequirement($location, $rootScope, $compile, $templateRequest, $uibModal, config, common);

            directive.$inject = [
                '$location',
                '$rootScope',
                '$compile',
                '$templateRequest',
                '$uibModal',
                'config',
                'common'
            ];

            return directive;
        }

        terminal = true;
        restrict = 'E';

        scope = {
            application: "=application",
            complianceApplication: "=compliance",
            isComplianceApplication: "=isComplianceApplication"
        };
        
        link = (scope: any, element: any, attributes: any) => {
            scope.location = this.$location;
            this.$templateRequest("/app/widgets/applicationHeader.html").then((html) => {
                var template = angular.element(html);
                element.append(template);
                this.$compile(template)(scope);
            });
        }
    }

    angular
        .module('app.widgets')
        .directive('applicationHeader', ApplicationRequirement.factory());
}