module app.widgets {
    'use strict';

    class CcWidgetHeader implements ng.IDirective {
        static instance(): ng.IDirective {
            return new CcWidgetHeader;
        }

        restrict = 'A';
        scope = {
            'title': '@',
            'subtitle': '@',
            'rightText': '@',
            'allowCollapse': '@'
        };
        templateUrl = "app/layout/widgetheader.html";

        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attributes: ng.IAttributes): void {
            attributes.$set('class', 'widget-head');
        }
    }

    angular
        .module('app.widgets')
        .directive('ccWidgetHeader', CcWidgetHeader.instance);
}