module app.modal.templates {
    'use strict';

    interface IValues {
        record: services.ICibmtr;
    }

    class CibmtrEditController {
        selectedCibmtr: services.ICibmtr;
        isNotRequired = false;
        allogeneicGridOptions = {
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "reportYear", title: "Report Date" },
                { field: "comparativeDataSource", title: "Comparative Data Source" },
                { template: "<button class='k-button' ng-click='vm.onEdit(dataItem, \"allo\")'>Edit</button>" }
            ],
            pageable: {
                pageSize: 10
            }
        };
        dataMgmtGridOptions = {
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "auditDate", title: "Audit Date", template: "#= auditDate == null ? '' : kendo.toString(kendo.parseDate(auditDate, 'yyyy-MM-dd'),'MM/dd/yyyy') #" },
                { field: "cpiLetterDate", title: "CPI Letter Date", template: "#= cpiLetterDate == null ? '' : kendo.toString(kendo.parseDate(cpiLetterDate, 'yyyy-MM-dd'),'MM/dd/yyyy') #" },
                { template: "<button class='k-button' ng-click='vm.onEditDataMgmt(dataItem, \"allo\")'>Edit</button>" }
            ],
            pageable: {
                pageSize: 10
            }
        };
        isNew = false;
        wasSaved = false;

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

            this.selectedCibmtr = values.record;
            this.allogeneicGridOptions.dataSource.data(this.selectedCibmtr.cibmtrOutcomeAnalyses);
            this.dataMgmtGridOptions.dataSource.data(this.selectedCibmtr.cibmtrDataMgmts);

            this.isNew = this.selectedCibmtr.id == null;

            var found = _.find(this.selectedCibmtr.cibmtrOutcomeAnalyses, (c) => {
                return c.isNotRequired === true;
            });

            if (found) {
                this.isNotRequired = true;
            }
        }

        onAddDataMgmt() {
            var record: services.ICibmtrDataMgmt = {
                id: null,
                cibmtrId: this.selectedCibmtr.id
            };

            this.onShowDataModal(record, true);
        }

        onEditDataMgmt(record: services.ICibmtrDataMgmt) {
            this.onShowDataModal(record, false);
        }

        onShowDataModal(record: services.ICibmtrDataMgmt, isNew: boolean) {
            
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/cibmtrDataMgmtEdit.html",
                controller: "app.modal.templates.CibmtrDataMgmtEditController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                backdrop: false,
                keyboard: false,
                resolve: {
                    values: () => {
                        return {
                            record: record,
                            cibmtr: this.values.record
                        };
                    }
                }
            });

            instance.result.then((row: services.ICibmtrDataMgmt) => {
                if (isNew) {
                    this.selectedCibmtr.cibmtrDataMgmts.push(row);
                } else {
                    for (var i = 0; i < this.selectedCibmtr.cibmtrDataMgmts.length; i++) {
                        if (this.selectedCibmtr.cibmtrDataMgmts[i].id === record.id) {
                            this.selectedCibmtr.cibmtrDataMgmts.splice(i, 1, row);
                        }
                    }
                }

                this.dataMgmtGridOptions.dataSource.data(this.selectedCibmtr.cibmtrDataMgmts);
                this.notificationFactory.success("Data Management data saved successfully.");
            }, () => {
            });
        }

        onAddOutcome() {
            var record: services.ICibmtrOutcomeAnalysis = {
                id: null,
                cibmtrId: this.selectedCibmtr.id
            };

            this.onShowModal(record, "", true);
        }

        onShowModal(record: services.ICibmtrOutcomeAnalysis, type: string, isNew: boolean) {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/cibmtrOutcomeEdit.html",
                controller: "app.modal.templates.CibmtrOutcomeEditController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                backdrop: false,
                keyboard: false,
                resolve: {
                    values: () => {
                        return {
                            record: record,
                            cibmtr: this.values.record,
                            type: type
                        };
                    }
                }
            });

            instance.result.then((row: services.ICibmtrOutcomeAnalysis) => {
                if (isNew) {
                    this.selectedCibmtr.cibmtrOutcomeAnalyses.push(row);
                } else {
                    for (var i = 0; i < this.selectedCibmtr.cibmtrOutcomeAnalyses.length; i++) {
                        if (this.selectedCibmtr.cibmtrOutcomeAnalyses[i].id === record.id) {
                            this.selectedCibmtr.cibmtrOutcomeAnalyses.splice(i, 1, row);
                        }
                    }
                }

                this.allogeneicGridOptions.dataSource.data(this.selectedCibmtr.cibmtrOutcomeAnalyses);
                this.notificationFactory.success("Outcome Analysis data saved successfully.");
            }, () => {
            });
        }

        onSetRequired() {
            _.each(this.selectedCibmtr.cibmtrOutcomeAnalyses, (o) => {
                o.isNotRequired = this.isNotRequired;
            });

            this.common.showSplash();
            this.facilityService.saveOutcomes(this.selectedCibmtr.cibmtrOutcomeAnalyses)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error("Error saving outcomes: " + data.hasError);
                    } else {
                        this.notificationFactory.success("Successfully updated outcomes");
                    }

                    this.common.hideSplash();
                })
                .catch((e) => {
                    this.notificationFactory.error("Error saving outcomes: " + e);
                    this.common.hideSplash();
                });

        }

        onEdit(record: services.ICibmtrOutcomeAnalysis, type: string) {
            this.onShowModal(record, type, false);
        }

        onSaveCibmtr(close: boolean) {
            if (!this.selectedCibmtr.centerNumber || this.selectedCibmtr.centerNumber === "") {
                this.notificationFactory.error("You must enter a CIBMTR Center Number or FACT Identifier");
                return;
            }

            this.common.showSplash();

            this.facilityService.saveCibmtr(this.selectedCibmtr)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.selectedCibmtr.id = this.selectedCibmtr.id || data.item;
                        if (close) {
                            this.$uibModalInstance.close(this.selectedCibmtr);
                        }
                        this.notificationFactory.success("CIBMTR Data saved successfully.");
                        this.wasSaved = true;
                        this.common.hideSplash();
                    }
                })
                .catch((e) => {
                    this.notificationFactory.error("Error saving CIBMTR: " + e + "Please contact support.");
                    this.common.hideSplash();
                });
        }

        onCancel() {
            if (this.isNew && this.wasSaved) {
                this.$uibModalInstance.close(this.selectedCibmtr);
            } else {
                this.$uibModalInstance.dismiss();
            }
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.CibmtrEditController',
        CibmtrEditController);
}