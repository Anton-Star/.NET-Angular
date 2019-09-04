((): void => {
    'use strict';

    angular
        .module('app.core', [
            'ngRoute',
            'ngSanitize',
            'ngCookies',
        ]);
})();