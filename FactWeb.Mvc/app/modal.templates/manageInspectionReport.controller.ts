module app.modal.templates {
    'use strict';

    class ManageInspectionReportController {
        comment: string;
        schedules: services.IInspectionSchedule[];
        detail: services.ICompAppInspectionDetail;
        isCb = false;

        static $inject = [
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'inspectionScheduleService',
            'applicationService'
        ];

        constructor(
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private inspectionScheduleService: services.IInspectionScheduleService,
            private applicationService: services.IApplicationService) {

            this.common.activateController([this.getInspectionSchedules(), this.getInspectionDetail()], '');

            var found = null;
            if (this.common.compApp) {
                found = _.find(this.common.compApp.complianceApplicationSites, (site) => {
                    var find = _.find(site.appResponses, (r) => {
                        return r.applicationTypeName.indexOf("CB") > -1;
                    });

                    if (find) {
                        return true;
                    } else {
                        return false;
                    }
                });

                if (found) {
                    this.isCb = true;
                }
            }
        }

        getInspectionSchedules(): ng.IPromise<void> {
            return this.inspectionScheduleService.getSchedulesForCompliance(this.common.compApp.id)
                .then((data) => {
                    this.schedules = data;
                });
        }

        getInspectionDetail(): ng.IPromise<void> {
            return this.applicationService.getCompAppInspectionDetail(this.common.compApp.id)
                .then((data) => {
                    if (data == null) {
                        this.detail = {
                            complianceApplicationId: this.common.compApp.id
                        };
                    } else {
                        this.detail = data;
                        this.detail
                            .inspectionScheduleIdString = this.detail.inspectionScheduleId != null
                            ? this.detail.inspectionScheduleId.toString()
                            : "";
                    }
                });
        }

        onCancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }

        onSave(): void {
            this.common.showSplash();

            if (this.detail.inspectionScheduleIdString && this.detail.inspectionScheduleIdString !== "") {
                this.detail.inspectionScheduleId = parseInt(this.detail.inspectionScheduleIdString);
            }

            this.applicationService.saveCompAppInspectionDetail(this.detail)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error('Error saving data: ' + data.message);
                    } else {
                        this.detail = data.item;
                        this.notificationFactory.success('Data saved successfully');
                    }

                    this.common.hideSplash();
                })
                .catch((e) => {
                    this.notificationFactory.error('Error saving data: ' + e);
                    this.common.hideSplash();
                });
        }

        onAdd() {
            this.detail = {
                complianceApplicationId: this.common.compApp.id
            };
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.ManageInspectionReportController',
        ManageInspectionReportController);
} 