module app.modal.templates {
    'use strict';

    interface IOrganizationSearch {
        searchText: string;
        city: string;
        state: string;
        isBusy: boolean;
        selected: services.IOrganization;
        results: Array<services.IOrganization>;
        search: () => void;
        rowSelected: (event) => void;
        ok: () => void;
        cancel: () => void;
    }

    class OrganizationSearchController implements IOrganizationSearch {
        searchText: string;
        city: string;
        state: string;
        isBusy = false;
        selected: services.IOrganization;
        results: Array<services.IOrganization>;
        gridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            change: (e) => {
                return this.rowSelected(e);
            },
            selectable: "row",
            dataSource: new kendo.data.DataSource({ data: [], pageSize: 10 }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            'organizationService',
            'notificationFactory',
            'common',
            '$uibModalInstance'
        ];

        constructor(
            private organizationService: services.IOrganizationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {
        }

        search(): void {
            this.isBusy = true;
            this.organizationService.search(this.searchText, this.city, this.state)
                .then((data: Array<app.services.IOrganization>) => {
                    this.results = data;
                    this.gridOptions.dataSource.data(data);
                    this.isBusy = false;
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to search. Please contact support.");
                    this.isBusy = false;
                });
        }

        rowSelected(event): void {
            var grid = event.sender;
            var selectedItem = grid.dataItem(grid.select());
            this.selected = angular.copy(selectedItem);
        }

        ok(): void {
            this.$uibModalInstance.close(this.selected);
        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.OrganizationSearchController',
        OrganizationSearchController);
} 