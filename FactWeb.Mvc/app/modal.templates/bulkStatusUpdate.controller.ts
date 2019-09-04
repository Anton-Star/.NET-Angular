module app.modal.templates {
    'use strict';

    interface IBulkStatusUpdate {
        section: services.IApplicationSection;
        fromStatusesList: Array<services.IApplicationResponseStatusItem>;
        toStatusesList: Array<services.IApplicationResponseStatusItem>;
        updateStatus: () => void;
        cancel: () => void;
    }

    class BulkStatusUpdateController implements IBulkStatusUpdate {
        section: services.IApplicationSection;
        fromStatusesList: Array<services.IApplicationResponseStatusItem>;
        toStatusesList: Array<services.IApplicationResponseStatusItem>;
        toStatus: number;
        fromStatus: number;

        static $inject = [
            '$uibModal',
            'applicationResponseStatusService',
            'applicationService',
            'notificationFactory',
            'common',
            'config',
            'parentSection',
            'appType',
            'organization',
            'appUniqueId',
            '$uibModalInstance'
        ];

        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private applicationResponseStatusService: services.IApplicationResponseStatusService,
            private applicationService: services.IApplicationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private parentSection: services.IApplicationSection,
            private appType: string,
            private organization: string,
            private appUniqueId: string,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            console.log(this);

            if (parentSection != null) {
                this.section = parentSection;
            }

            common.activateController([this.getStatusesList()], 'bulkStatusUpdateController');
        }

        getStatusesList(): ng.IPromise<void> {
            return this.applicationResponseStatusService.getApplicationResponseStatus()
                .then((data: Array<app.services.IApplicationResponseStatusItem>) => {
                    if (data == null) {
                        this.notificationFactory.error("No Application Response Statuses found");
                    } else {
                        this.fromStatusesList = data.filter(function (statusType) {
                            return (statusType.name != 'No Response Requested');
                        });

                        this.toStatusesList = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting application response statuses. Please contact support.");
                });
        }

        isUpdateValid(): boolean {
            if (this.toStatus == this.fromStatus) {
                return false;
            }
            else {
                return true;
            }
        }

        getSmallerChildObject(child: services.IApplicationSection) {
            var obj = {
                id: child.applicationSectionId,
                questions: child.questions,
                children: []
            }

            //if (child.questions && child.questions.length > 0) {
            //    _.each(child.questions, (q) => {
            //        if (q.answerResponseStatusId === this.fromStatus ||
            //            (this.toStatus === 3 && q.answerResponseStatusId == null)) {
            //            q.answerResponseStatusId = this.toStatus;
            //        }
            //    });
            //}

            if (child.children && child.children.length > 0) {
                _.each(child.children, (c) => {
                    obj.children.push(this.getSmallerChildObject(c));
                });
            }

            return obj;
        }

        updateStatus(): void {
            var obj = {
                section: {
                    questions: this.section.questions,
                    id: this.section.applicationSectionId || this.section.id, 
                    children: []

                },
                fromStatus: this.fromStatus,
                toStatus: this.toStatus,
                appType: this.appType,
                organization: this.organization,
                appUniqueId: this.appUniqueId
            }

            if (this.section.questions && this.section.questions.length > 0) {
                _.each(this.section.questions, (q) => {
                    if (q.answerResponseStatusId === this.fromStatus ||
                        (this.toStatus === 3 && q.answerResponseStatusId == null)) {
                        q.answerResponseStatusId = this.toStatus;
                    }
                });
            }

            if (this.section.children && this.section.children.length > 0) {
                _.each(this.section.children, (c) => {
                    obj.section.children.push(this.getSmallerChildObject(c));
                });
            }

            var str = JSON.stringify(obj);
            this.common.showSplash();
            

            this.applicationService.bulkUpdateApplicationResponseStatus(str)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Statuses updated successfully.");
                    }

                    var fromStatusObj = _.find(this.toStatusesList, (t: services.IApplicationResponseStatusItem) => {
                        return t.id === this.fromStatus;
                    });

                    var toStatusObj = _.find(this.toStatusesList, (t: services.IApplicationResponseStatusItem) => {
                        return t.id === this.toStatus;
                    });

                    this.common.$broadcast('AppSaved', { appUniqueId: this.appUniqueId, rowId: this.section.applicationSectionId });
                    this.$uibModalInstance.close({ section: this.section, fromStatus: fromStatusObj, toStatus: toStatusObj });

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to update status. Please contact support.");
                    this.common.hideSplash();
                });
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.BulkStatusUpdateController',
        BulkStatusUpdateController);
}    