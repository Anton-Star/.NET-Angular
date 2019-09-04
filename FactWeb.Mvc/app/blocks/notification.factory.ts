module app.blocks {
    export interface INotificationFactory {
        success: (text: string) => void;
        error: (text: string) => void;
        warning: (text: string) => void;
    }
}

((): void => {
    'use strict';

    angular
        .module('app.blocks')
        .factory('notificationFactory', ['$rootScope', notificationFactory]);

    function notificationFactory($rootScope): app.blocks.INotificationFactory {
        return {
            success: (text: string): void => {
                toastr.success(text, "Success");
            },
            error: (text: string): void => {
                if ($rootScope.is403 !== true || text === "Access Denied")
                    toastr.error(text, "Error");
            },
            warning: (text: string): void => {
                toastr.warning(text, "Warning");
            }
        };
    }
})();