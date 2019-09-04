module app.widgets {
    'use strict';

    class Focus implements ng.IDirective {

        constructor(public $parse: ng.IParseService, public $timeout: ng.ITimeoutService) {

        }

        static factory(): ng.IDirectiveFactory {
            var directive = ($parse: ng.IParseService, $timeout: ng.ITimeoutService) => new Focus($parse, $timeout);
            directive.$inject = ['$parse', '$timeout'];
            return directive;
        }

        restrict = 'A';

        link = (scope: ng.IScope, element: any, attributes: any) => {
            var model = this.$parse(attributes.focus);
            scope.$watch(model, (value) => {
                if (value === true) {
                    this.$timeout(() => {
                        element[0].focus();
                    });
                }
            });
            // to address @blesh's comment, set attribute value to 'false'
            // on blur event:
            element.bind('blur', () => {
                scope.$apply(model.assign(scope, false));
            });
        }
    }

    angular
        .module('app.widgets')
        .directive('focus', Focus.factory());
}