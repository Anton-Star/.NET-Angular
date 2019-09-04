module app.widgets {
    'use strict';

    class Report implements ng.IDirective {
        static instance(): ng.IDirective {
            return new Report;
        }

        restrict = 'EA';
        controller = Report;
        
        scope = {
            name: '@',
            parameters: '@',
            height: '@'
        };

        link(scope: any, element: ng.IAugmentedJQuery, attributes: ng.IAttributes): void {
            var viewer: any = $("#reportViewer1");
            var reportViewer = $("#reportViewer1").data("telerik_ReportViewer");
            if (!reportViewer) {

                $("#reportViewer1").show();

                $(document).ready(() => {
                    viewer.telerik_ReportViewer({
                        error: (e, args) => {
                            alert('Error from report directive:' + args);
                        },
                        reportSource: {
                            report: scope.name,
                            parameters: JSON.parse(scope.parameters)
                        },
                        reportServer: {
                            url: "https://reports.factwebsite.org",
                            templateUrl: "/ReportViewer/templates/telerikReportViewerTemplate-FA-10.1.16.615.html"
                        },
                        scale: 1.0,
                        //viewMode: "PRINT_PREVIEW",
                        scaleMode: "FIT_PAGE_WIDTH",
                        ready: () => {
                            reportViewer = $("#reportViewer1").data("telerik_ReportViewer");

                            reportViewer.reportSource({
                                report: scope.name,
                                parameters: JSON.parse(scope.parameters),
                            });
                        }
                    });

                    if (scope.height) {
                        $("#reportViewer1").height(scope.height);
                    }
                    
                });
            }
            //on state change update the report source
            scope.$watch('name', ()=> {

                var reportViewer = $("#reportViewer1").data("telerik_ReportViewer");

                if (reportViewer) {
                    var rs = reportViewer.reportSource();
                    if (rs && rs.report)
                        if (rs.report !== scope.name &&
                            rs.parameters !== scope.parameters) {

                            reportViewer.reportSource({
                                report: scope.name,
                                parameters: JSON.parse(scope.parameters),
                            });
                        }
                }
            });

            scope.$watch('parameters', () => {
                var reportViewer = $("#reportViewer1").data("telerik_ReportViewer");

                if (reportViewer) {
                    
                    var rs = reportViewer.reportSource();
                    if (rs && rs.report)
                        if (rs.report !== scope.name ||
                            rs.parameters !== scope.parameters) {

                            reportViewer.reportSource({
                                report: scope.name,
                                parameters: JSON.parse(scope.parameters),
                            });
                        }
                }
            });
        }
    }

    angular
        .module('app.widgets')
        .directive('report', Report.instance);
}