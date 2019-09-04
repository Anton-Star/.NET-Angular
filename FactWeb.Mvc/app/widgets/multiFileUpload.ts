module app.widgets {
    'use strict';

    class MultiFileModel implements ng.IDirective {

        constructor(private $parse: any) {

        }

        static factory(): ng.IDirectiveFactory {
            var directive = ($parse: any) => new MultiFileModel($parse);
            directive.$inject = ['$parse'];
            return directive;
        }

        restrict = 'A';

        link = (scope: ng.IScope, element: any, attributes: any) => {
            var model = this.$parse(attributes.multiFileModel);
            var isMultiple = attributes.multiple;
            var modelSetter = model.assign;
            element.bind('change', function () {
                var values = [];
                angular.forEach(element[0].files, function (item) {
                    var value = {
                        // File Name 
                        name: item.name,
                        //File Size 
                        size: item.size,
                        //File URL to view 
                        url: URL.createObjectURL(item),
                        // File Input Value 
                        _file: item
                    };
                    values.push(value);
                });
                scope.$apply(function () {
                    if (isMultiple) {
                        modelSetter(scope, values);
                    } else {
                        modelSetter(scope, values[0]);
                    }
                });
            });
            //element.bind('change', () => {
            //    scope.$apply(() => {
            //        modelSetter(scope, element[0].files[0]);
            //    });
            //});
        }
    }

    angular
        .module('app.widgets')
        .directive('multiFileModel', MultiFileModel.factory());
}