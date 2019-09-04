module app.admin {
    'use strict';

    class ModuleManagementController {
        results: Array<services.IApplicationType> = [];
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
            '$uibModal',
            'applicationService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private applicationService: services.IApplicationService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {
            common.activateController([this.getTypes()], '');
        }

        getTypes(): ng.IPromise<void> {
            return this.applicationService.getApplicationTypes()
                .then((data: Array<services.IApplicationType>) => {
                    if (data == null) {
                        this.notificationFactory.error('no items');
                    } else {
                        _.each(data, (type) => {
                            if (type.isManageable) {
                                this.results.push(type);
                            }
                        });

                        this.gridOptions.dataSource.data(this.results);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        onAdd(): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/editModuleManagement.html",
                controller: "app.modal.templates.EditModuleManagementController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    module: () => {
                        return null;
                    }
                }
            });

            instance.result.then((type: services.IApplicationType) => {
                this.results.push(type);
                this.gridOptions.dataSource.data(this.results);
            }, () => {
            });

        }

        onEdit(module: services.IApplicationType): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/editModuleManagement.html",
                controller: "app.modal.templates.EditModuleManagementController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    module: () => {
                        return module;
                    }
                }
            });

            instance.result.then((type: services.IApplicationType) => {
                var record = _.find(this.results, (row) => {
                    return row.applicationTypeId === type.applicationTypeId;
                });

                if (record) {
                    record.applicationTypeName = type.applicationTypeName;
                }

                this.gridOptions.dataSource.data(this.results);
            }, () => {
            });

        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.ModuleManagementController',
        ModuleManagementController);
} 