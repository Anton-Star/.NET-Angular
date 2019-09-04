module app.widgets {
    'use strict';

    class FileModel implements ng.IDirective {

        constructor(private $parse: any) {

        }

        static factory(): ng.IDirectiveFactory {
            var directive = ($parse: any) => new FileModel($parse);
            directive.$inject = ['$parse'];
            return directive;
        }

        restrict = 'A';

        link = (scope: ng.IScope, element: any, attributes: any) => {
            var model = this.$parse(attributes.fileModel);
            var modelSetter = model.assign;

            element.bind('change', () => {
                scope.$apply(() => {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    }

    angular
        .module('app.widgets')
        .directive('fileModel', FileModel.factory());
}