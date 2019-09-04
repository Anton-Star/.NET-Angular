module app.widgets {
    'use strict';

    class CcSidebar implements ng.IDirective {
        static instance(): ng.IDirective {
            return new CcSidebar;
        }

        restrict = 'A';
        controller = CcSidebar;

        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attributes: ng.IAttributes): void {
            var $sidebarInner = element.find('.sidebar-inner');
            var $dropdownElement = element.find('.sidebar-dropdown a');
            element.addClass('sidebar');
            $dropdownElement.click((e): any => {
                var dropClass = 'dropy';
                e.preventDefault();
                if (!$dropdownElement.hasClass(dropClass)) {
                    hideAllSidebars();
                    $sidebarInner.slideDown(350);
                    $dropdownElement.addClass(dropClass);
                } else if ($dropdownElement.hasClass(dropClass)) {
                    $dropdownElement.removeClass(dropClass);
                    $sidebarInner.slideUp(350);
                }

                function hideAllSidebars() {
                    $sidebarInner.slideUp(350);
                    $('.sidebar-dropdown a').removeClass(dropClass);
                }
            });
        }
    }

    angular
        .module('app.widgets')
        .directive('ccSidebar', CcSidebar.instance);
}