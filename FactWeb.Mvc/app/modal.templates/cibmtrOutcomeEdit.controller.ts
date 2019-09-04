module app.modal.templates {
    'use strict';

    interface IValues {
        record: services.ICibmtrOutcomeAnalysis;
        cibmtr: services.ICibmtr;
        type: string;
    }

    class CibmtrOutcomeEditController {
        selectedCibmtr: services.ICibmtrOutcomeAnalysis;
        allogeneicGridOptions = {
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "reportYear", title: "Report Date" },
                { template: "<button class='k-button' ng-click='vm.onEditCibmtr(dataItem)'>Edit</button>" }
            ],
            pageable: {
                pageSize: 10
            }
        };
        years: number[] = [];
        showActual = false;
        showPredicted = false;
        showLower = false;
        showUpper = false;
        actual: string = null;
        predicted: string = null;
        lower: string = null;
        upper: string = null;
        reportYear: string;
        survivalScore: string;

        static $inject = [
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'config',
            'facilityService',
            'values'
        ];

        constructor(
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private config: IConfig,
            private facilityService: services.IFacilityService,
            private values: IValues) {

            this.selectedCibmtr = values.record;
            this.reportYear = this.selectedCibmtr.reportYear ? this.selectedCibmtr.reportYear.toString() : "";
            this.survivalScore = this.selectedCibmtr.survivalScore != undefined ? this.selectedCibmtr.survivalScore.toString() : "";

            this.onBlur("actual");
            this.onBlur("predicted");
            this.onBlur("lower");
            this.onBlur("upper");

            var date = new Date().getFullYear();

            for (var i = date; i >= 2012; i--) {
                this.years.push(i);
            }
        }

        formatField(field: number) {
            if (field) {
                return field.toString() + "%";
            }

            return null;
        }

        onBlur(field) {
            switch (field) {
                case "actual":
                    this.showActual = false;
                    this.actual = this.formatField(this.selectedCibmtr.actualPercent);
                    break;
                case "predicted":
                    this.showPredicted = false;
                    this.predicted = this.formatField(this.selectedCibmtr.predictedPercent);
                    break;
                case "lower":
                    this.showLower = false;
                    this.lower = this.formatField(this.selectedCibmtr.lowerPercent);
                    break;
                case "upper":
                    this.showUpper = false;
                    this.upper = this.formatField(this.selectedCibmtr.upperPercent);
                    break;
            }
        }

        onSaveCibmtr() {
            this.common.showSplash();

            this.selectedCibmtr.survivalScore = this.survivalScore !== "" ? parseInt(this.survivalScore) : null;
            this.selectedCibmtr.reportYear = this.reportYear !== "" ? parseInt(this.reportYear) : null;

            this.facilityService.saveCibmtrOutcome(this.selectedCibmtr)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.selectedCibmtr.id = this.selectedCibmtr.id || data.item;
                        this.$uibModalInstance.close(this.selectedCibmtr);
                        this.common.hideSplash();
                    }
                })
                .catch((e) => {
                    this.notificationFactory.error("Error saving CIBMTR: " + e + "Please contact support.");
                    this.common.hideSplash();
                });
        }

        onCancel() {
            this.$uibModalInstance.dismiss();
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.CibmtrOutcomeEditController',
        CibmtrOutcomeEditController);
}