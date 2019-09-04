module app.common {
    'use strict';

    export interface IModalHelper {
        showModal: (templateUrl: string, controller: string, resolveValues?: Object) => ng.IPromise<{}>;
    }

    class ModalHelper implements IModalHelper {
        currentUser: services.IUser;
        isShowing = false;


        constructor(private $q: ng.IQService,
            private $uibModal: ng.ui.bootstrap.IModalService) {
        }

        showModal(templateUrl: string, controller: string, resolveValues?: Object): ng.IPromise<{}> {
            var deferred = this.$q.defer();

            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: templateUrl,
                controller: controller,
                controllerAs: "vm",
                size: 'xxl',
                backdrop: false,
                keyboard: false,
                resolve: resolveValues || {}
            });

            instance.result.then((data) => {
                deferred.resolve(data);
            }, () => {
                deferred.reject();
            });

            return deferred.promise;
        }
    }

    factory.$inject = [
        '$q',
        '$uibModal'
    ];
    function factory($q: ng.IQService,
        $uibModal: ng.ui.bootstrap.IModalService): IModalHelper {

        return new ModalHelper($q, $uibModal);
    }

    angular
        .module('app.common')
        .factory('modalHelper',
        factory);
} 