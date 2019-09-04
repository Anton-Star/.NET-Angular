module app.home {
    'use strict';

    class RequestAccessController {
        model: services.IRequestAccess;
        serviceTypes: services.IMasterServiceTypeItem[];
        confirm: string;

        static $inject = [
            'facilityService',
            'accountService',
            'notificationFactory',
            'config',
            'common'
        ];
        constructor(
            private facilityService: services.IFacilityService,
            private accountService: services.IAccountService,
            private notificationFactory: blocks.INotificationFactory,
            private config: IConfig,
            private common: common.ICommonFactory) {
            this.getServiceTypes();
        }

        getServiceTypes(): ng.IPromise<void> {
            return this.facilityService.getMasterServiceTypes()
                .then((data: services.IMasterServiceTypeItem[]) => {
                    data.push({
                        id: 0,
                        name: "Other",
                        shortName: ""
                    });

                    this.serviceTypes = data;
                });
        }

        isComplete() {
            if (this.model.serviceType === "Other" && (!this.model.masterServiceTypeOtherComment || this.model.masterServiceTypeOtherComment === "")) {
                return true;
            }

            return false;
        }

        onSubmit(): void {
            this.common.showSplash();

            this.accountService.requestAccess(this.model)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Access Request Successfully submitted. We will be in contact with next steps shortly.");
                    }
                    this.common.hideSplash();
                    this.confirm = 'success';
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to log in. Please contact support.");
                    this.common.hideSplash();
                });
        }
    }

    angular
        .module('app.home')
        .controller('app.home.RequestAccessController',
        RequestAccessController);
} 