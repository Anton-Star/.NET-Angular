module app.account {
    'use strict';

    interface IOrgType {
        name: string;
        title: string;
    }

    interface IReminderScope {
        firstName: string;
        lastName: string;
        email: string;
        isValid: boolean;
        submit: () => void;
        checkFields: ()=>boolean;
    }

    class ReminderController implements IReminderScope {
        firstName = "";
        lastName = "";
        email = "";
        isValid = false;

        static $inject = [
            'accountService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private accountService: app.services.IAccountService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {

            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Password Reminder",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Password Reminder', isActive: true }
                ]
            });
        }

        checkFields(): boolean {
            if (this.email !== "") {
                return false;
            }

            if (this.firstName !== "" && this.lastName !== "") {
                return false;
            }

            return true;
        }

        submit(): void {
            this.common.showSplash();

            this.accountService.reminder(this.email, this.firstName, this.lastName)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("If your information was found, an email will be sent to you.");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to register. Please contact support.");
                    this.common.hideSplash();
                });
        }
    }

    angular
        .module('app.account')
        .controller('app.account.ReminderController',
        ReminderController);
} 