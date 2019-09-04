module app.modal.templates {
    'use strict';

    interface IValues {
        record: services.ICibmtrDataMgmt;
        cibmtr: services.ICibmtr;
        type: string;
    }

    class CibmtrDataMgmtEditController {
        selectedCibmtr: services.ICibmtrDataMgmt;
        showCer = false;
        showRandom = false;
        showOverall = false;
        cer: string = null;
        random: string = null;
        overall: string = null;
        dateOptions = {
            open: this.onOpen,
            format: "MM/dd/yyyy"
        };
        dateObject: Date = null;
        dateObject2: Date = null;
        auditDate = "";
        cpiLetterDate = "";
        cpiTypes: services.ICpiType[];

        static $inject = [
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'config',
            'facilityService',
            'cpiTypeService',
            'values'
        ];

        constructor(
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private config: IConfig,
            private facilityService: services.IFacilityService,
            private cpiTypeService: services.ICpiTypeService,
            private values: IValues) {

            this.selectedCibmtr = values.record;

            if (this.selectedCibmtr) {
                if (this.selectedCibmtr.auditDate) {
                    this.dateObject = moment(this.selectedCibmtr.auditDate).toDate();
                }

                if (this.selectedCibmtr.cpiLetterDate) {
                    this.dateObject2 = moment(this.selectedCibmtr.cpiLetterDate).toDate();
                }
            }

            this.onBlur("cer");
            this.onBlur("random");
            this.onBlur("overall");

            this.getCpiTypes();
        }

        getCpiTypes(): ng.IPromise<void> {
            return this.cpiTypeService.getAll()
                .then((data) => {
                    this.cpiTypes = data;
                });
        }

        formatField(field: number) {
            if (field) {
                return field.toString() + "%";
            }

            return null;
        }

        onOpen(e) {
            var id = e.sender.element[0].id;

            setTimeout(() => {
                var pos = $('#' + id).offset().top + 40;
                $(".k-animation-container").css("top", pos);
            },
                200);

        }

        blurDate() {
            if (this.auditDate !== "") {
                this.auditDate = moment(this.auditDate).format("MM/DD/YYYY");
            }
        }

        onBlur(field) {
            switch (field) {
                case "cer":
                    this.showCer = false;
                    this.cer = this.formatField(this.selectedCibmtr.criticalFieldErrorRate);
                    break;
                case "random":
                    this.showRandom = false;
                    this.random = this.formatField(this.selectedCibmtr.randomFieldErrorRate);
                    break;
                case "overall":
                    this.showOverall = false;
                    this.overall = this.formatField(this.selectedCibmtr.overallFieldErrorRate);
                    break;
            }
        }

        onSaveCibmtr() {
            this.common.showSplash();

            this.selectedCibmtr.auditDate = this.dateObject;
            this.selectedCibmtr.cpiLetterDate = this.dateObject2;

            this.facilityService.saveCibmtrData(this.selectedCibmtr)
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
        .controller('app.modal.templates.CibmtrDataMgmtEditController',
        CibmtrDataMgmtEditController);
}