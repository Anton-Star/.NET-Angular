module app.modal.templates {
    'use strict';

    interface IValues {
        facilityName: string;
    }

    class CibmtrController {
        grid: kendo.ui.Grid;
        cibmtrGridOptions = {
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "centerNumber", title: "Center Name / FACT Identifier" },
                { field: "displayName", title: "Display Name" },
                { field: "isNonCibmtr", title: "Non-CIBMTR Flag" },
                { field: "isActive", title: "Active" },
                { template: "<button class='k-button' ng-click='vm.onEditCibmtr(dataItem)'>Edit</button>" }
            ],
            pageable: {
                pageSize: 10
            }
        };
        cibmtrData: services.ICibmtr[];

        static $inject = [
            '$uibModal',
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'config',
            'facilityService',
            'values'
        ];

        constructor(
            private $uibModal: ng.ui.bootstrap.IModalService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private config: IConfig,
            private facilityService: services.IFacilityService,
            private values: IValues) {

            this.common.activateController([this.onGetData()], '');
        }

        onGetData(): ng.IPromise<void> {
            return this.facilityService.getCibmtrData(this.values.facilityName)
                .then((data) => {
                    this.cibmtrData = data;
                    this.cibmtrGridOptions.dataSource.data(data);
                });
        }

        onAddCibmtr() {
            var record = {
                id: null,
                isActive: true,
                displayName: this.values.facilityName,
                isNonCibmtr: false,
                facilityName: this.values.facilityName
            };

            this.onShowModal(record, true);
        }

        onShowModal(record: services.ICibmtr, isNew: boolean) {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/cibmtrEdit.html",
                controller: "app.modal.templates.CibmtrEditController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                backdrop: false,
                keyboard: false,
                resolve: {
                    values: () => {
                        return {
                            record: record
                        };
                    }
                }
            });

            instance.result.then((row: services.ICibmtr) => {
                if (isNew) {
                    this.cibmtrData.push(row);
                } else {
                    for (var i = 0; i < this.cibmtrData.length; i++) {
                        if (this.cibmtrData[i].id === record.id) {
                            this.cibmtrData.splice(i, 1, row);
                        }
                    }
                }

                this.cibmtrGridOptions.dataSource.data(this.cibmtrData);
            }, () => {
            });
        }

        onEditCibmtr(record: services.ICibmtr) {
            this.onShowModal(record, false);
        }

        cancel() {
            this.$uibModalInstance.dismiss();
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.CibmtrController',
        CibmtrController);
}