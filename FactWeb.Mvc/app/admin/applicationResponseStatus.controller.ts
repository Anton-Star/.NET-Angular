module app.admin {
    'use strict';

    interface IApplicationResponseStatusScope {
        saveStatus(): void;
        editStatus: (rowData) => void;
        saveMode: boolean;
        selectedFACTStatus: string;
        selectedApplicantStatus: string;
    }

    class ApplicationResponseStatusController implements IApplicationResponseStatusScope {
        applicationResponseStatuses: Array<services.IApplicationResponseStatusItem>;
        applicationResponseStatusId: number;
        selectedFACTStatus: string;
        selectedApplicantStatus: string;
        results: Array<services.IApplicationResponseStatusItem>;
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
            'applicationResponseStatusService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private applicationResponseStatusService: app.services.IApplicationResponseStatusService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.saveMode = false;
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Application Response Status",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Application Response Status', isActive: true }
                ]
            });

            common.activateController([this.getApplicationResponseStatus()], 'applicationResponseStatusController');
        }

        getApplicationResponseStatus(): ng.IPromise<void> {
            return this.applicationResponseStatusService.getApplicationResponseStatus()
                .then((data: Array<app.services.IApplicationResponseStatusItem>) => {
                    if (data == null) {
                        this.notificationFactory.error('no items');
                    } else {
                        this.applicationResponseStatuses = data;
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

            this.applicationResponseStatusService.saveStatus(this.applicationResponseStatusId, this.selectedFACTStatus, this.selectedApplicantStatus)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item == true) {
                            this.getApplicationResponseStatus();
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
            this.applicationResponseStatusId = rowData.id;
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
            this.applicationResponseStatusId = 0;
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.ApplicationResponseStatusController',
        ApplicationResponseStatusController);
} 