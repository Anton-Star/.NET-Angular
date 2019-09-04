module app.modal.templates {
    'use strict';

    interface IValues {
        users: Array<services.IUser>;
        user?: services.IUser;
    }

    class AuditorObserverController {
        isBusy = false;

        vm = this;

        static $inject = [
            '$uibModal',
            'notificationFactory',
            'common',
            'config',
            'currentUser',
            '$uibModalInstance',
            'userService',
            'values'
        ];

        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private currentUser: services.IUser,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private userService: services.IUserService,
            private values: IValues) {

            if (values.user == null) {
                values.user = {
                    userId: "",
                    isObserver: false,
                    isAuditor: false
                };
            }
            
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
        
        save(): void {
            this.isBusy = true;

            this.userService.setAuditorObserver(this.values.user.userId, this.values.user.isAuditor, this.values.user.isObserver)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                        this.isBusy = false;
                    } else {
                        this.$uibModalInstance.close(this.values.user);
                    }
                })
                .catch((e) => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.isBusy = false;
                });
        }
        
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.AuditorObserverController',
        AuditorObserverController);
} 