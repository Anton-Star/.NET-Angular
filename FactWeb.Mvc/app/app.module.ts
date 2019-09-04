((): void => {
    'use strict';

    angular
        .module('app', [
            'ngAnimate',
            'app.application',
            'app.core',
            'app.layout',
            'app.services',
            'app.widgets',
            'app.blocks',
            'app.common',
            'app.modal.templates',
            'app.eligibility',
            'app.compliance',
            'app.inspector',
            'app.reviewer',
            'app.renewal',
            'app.annual',
            'app.home',
            'app.account',
            'app.admin',
            'app.requirement',
            'app.coordinator',
            'app.reporting',
            'app.inspection',
            'ui.bootstrap',
            'kendo.directives',
            'LocalStorageModule',
            'vcRecaptcha',
            'ui.select',
            'textAngular',
            //'froala'
        ]);
})();