module app.admin {
    'use strict';

    interface IAuditLogScope {
        getAuditLog: () => void;
    }

    class AuditLogController implements IAuditLogScope {
        results: Array<services.IAuditLog>;
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
            'auditLogService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private auditLogService: app.services.IAuditLogService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Audit Log",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Audit Log', isActive: true }
                ]
            });
            common.activateController([this.getAuditLog()], 'auditLogController');
        }

        getAuditLog(): ng.IPromise<void> {
            return this.auditLogService.getAuditLog()
                .then((data: Array<app.services.IAuditLog>) => {
                        if (data == null) {
                            this.notificationFactory.error('no items');
                        } else {
                            this.results = data;
                            this.gridOptions.dataSource.data(data);
                        }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.AuditLogController',
        AuditLogController);
} 