module app.admin {
    'use strict';

    interface IApplicationStatusScope {
        saveStatus(): void;
        editStatus: (rowData) => void;
        saveMode: boolean;
        selectedFACTStatus: string;
        selectedApplicantStatus: string;
    }

    class ApplicationStatusController implements IApplicationStatusScope {
        applicationStatuses: Array<services.IApplicationStatus>;
        applicationStatusId: number;
        selectedFACTStatus: string;
        selectedApplicantStatus: string;
        results: Array<services.IApplicationStatus>;
        saveMode: boolean;
        gridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$window',
            'applicationStatusService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private applicationStatusService: app.services.IApplicationStatusService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.saveMode = false;
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Application Status",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Application Status', isActive: true }
                ]
            });

            common.activateController([this.getApplicationStatus()], 'applicationStatusController');
        }

        getApplicationStatus(): ng.IPromise<void> {
            return this.applicationStatusService.getApplicationStatus()
                .then((data: Array<app.services.IApplicationStatus>) => {
                    if (data == null) {
                        this.notificationFactory.error('no items');
                    } else {
                        this.applicationStatuses = data;
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        saveStatus(): void {
            this.common.showSplash();

            this.applicationStatusService.saveStatus(this.applicationStatusId, this.selectedFACTStatus, this.selectedApplicantStatus)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item == true) {
                            this.getApplicationStatus();
                            this.clearForm();
                            this.saveMode = false;
                            if (data.message != '' && data.message != null)
                                this.notificationFactory.success(data.message);

                        } else {
                            this.notificationFactory.error("Error.");
                        }
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }

        editStatus(rowData): void {
            this.applicationStatusId = rowData.id;
            this.selectedFACTStatus = rowData.name;
            this.selectedApplicantStatus = rowData.nameForApplicant;
            this.saveMode = true;
        }

        cancel(): void {
            this.clearForm();
            this.saveMode = false;
        }

        clearForm(): void {
            this.selectedFACTStatus = "";
            this.selectedApplicantStatus = "";
            this.applicationStatusId = 0;
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.ApplicationStatusController',
        ApplicationStatusController);
} 