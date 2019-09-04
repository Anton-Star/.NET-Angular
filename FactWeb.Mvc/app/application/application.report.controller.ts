module app.Application {
    'use strict';

    class ApplicationReportController {
        uniqueId: string;
        compAppId: string;
        isComplianceApplication = true;
        sites: Array<services.ISite> = [];
        site: string = "";
        encodedSite = "";
        reportData: services.IApplicationReport[];

        static $inject = [
            '$location',
            '$q',
            '$window',
            'applicationService',
            'siteService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $q: ng.IQService,
            private $window: ng.IWindowService,
            private applicationService: services.IApplicationService,
            private siteService: services.ISiteService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            this.uniqueId = $location.search().app;
            this.compAppId = $location.search().c;

            if (this.compAppId) {
                this.common.activateController([this.getSites()], '');
            } else {
                this.common.activateController([this.getAppReportData()], '');
            }
            

        }

        getSites(): ng.IPromise<void> {
            return this.siteService.getSitesByCompliance(this.compAppId)
                .then((data: Array<app.services.ISite>) => {
                    if (data != null) {
                        this.sites = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting sites.");
                });
        }

        getAppReportData(): ng.IPromise<void> {
            return this.applicationService.getAppReport(this.uniqueId)
                .then((data) => {
                    console.log(data);
                    this.buildReportData(data);
                });
        }

        getReportData(): ng.IPromise<void> {
            return this.applicationService.getApplicationReport(this.compAppId, this.site)
                .then((data) => {
                    console.log(data);

                    this.buildReportData(data);
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting report data.");
                });
        }

        buildReportData(data: services.IApplicationReport[]) {
            var items: any = {
                applicationId: 0,
                applicationSectionId: null,
                comments: null,
                hasSection: "0",
                order: 0,
                order2: 1,
                order3: 0,
                order4: 0,
                order5: 0,
                response: null,
                text: "",
                type: "Header"
            };

            for (var i = 0; i < data.length; i++) {
                if (data[i].type === "App") {
                    data.splice(i + 1, 0, items);
                    i++;
                } else if (data[i].type === "Org" && i > 0) {
                    data.splice(i, 1);
                    i--;
                }
            }

            this.reportData = data;
        }

        onExport() {
            let rpt = $("#report");

            kendo.drawing.drawDOM(rpt, {})
                .then((group) => {
                    console.log('dom');
                    // Render the result as a PDF file
                    return kendo.drawing.exportPDF(group, {});
                })
                .done((data) => {
                    // Save the PDF file
                    kendo.saveAs({
                        dataURI: data,
                        fileName: "ApplicationReport.pdf"
                    });
                });
        }

        onSiteChange() {
            if (this.site != null && this.site !== "") {
                this.encodedSite = encodeURIComponent(this.site);
                this.common.activateController([this.getReportData()], '');    
            }
            
        }

    }

    angular
        .module('app.application')
        .controller('app.application.ApplicationReportController',
        ApplicationReportController);
}