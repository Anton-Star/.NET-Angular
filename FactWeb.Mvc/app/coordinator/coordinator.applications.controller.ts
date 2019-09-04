module app.Coordinator {
    'use strict';

    class ApplicationsController {
        results: Array<services.ICoordinatorApplication>;
        inactiveResults: services.ICoordinatorApplication[];
        search = "";
        eligibility = true;
        compliance = true;
        annual = true;
        renewal = true;
        netcord = true;
        inactive = false;
        gridOptions = {
            sortable: true,
            selectable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
            ],
            pageable: {
                pageSize: 10
            }
        };
        isAll = false;

        static $inject = [
            '$location',
            '$uibModal',
            'applicationService',
            'organizationService',
            'notificationFactory',
            'config',
            'common'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private applicationService: app.services.IApplicationService,
            private organizationService: app.services.IOrganizationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private config: IConfig,
            private common: app.common.ICommonFactory) {

            var all = this.$location.search().a;
            this.isAll = all && all === "Y";
            var columns = [];

            if (this.isAll) {
                columns = [
                    { field: "organizationName", template: $("#organizationEdit").html(), title: "Applicant", width: "260px" },
                    { field: "location", title: "Location" },
                    { field: "applicationTypeName", template: $("#appType").html(), title: "Application" },
                    { field: "coordinator", title: "Coordinator" },
                    { field: "inspectionScheduleInspectionDate", title: "Inspection Date", template: "#= inspectionDateString#" },
                    { field: "applicationDueDate", title: "Due Date", template: "#= dueDateString#" },
                    { field: "outcomeStatusName", title: "Outcome" },
                    { template: $("#reports").html(), title: "Reports" },
                    { field: "applicationStatusName", template: $("#status").html(), title: "Status" }
                ];
            }
            else {
                columns = [
                    { field: "organizationName", template: $("#organizationEdit").html(), title: "Applicant", width: "260px" },
                    { field: "location", title: "Location" },
                    { field: "applicationTypeName", template: $("#appType").html(), title: "Application" },
                    { field: "inspectionScheduleInspectionDate", title: "Inspection Date", template: "#= inspectionDateString#" },
                    { field: "applicationDueDate", title: "Due Date", template: "#= dueDateString#" },
                    { field: "outcomeStatusName", title: "Outcome" },
                    { template: $("#reports").html(), title: "Reports" },
                    { field: "applicationStatusName", template: $("#status").html(), title: "Status" }
                ];
            }

            this.gridOptions.columns = columns;
            this.inactive = this.isAll;

            common.activateController([this.getApplications()], '');
        }

        getApplications(): ng.IPromise<void> {

            var all = this.$location.search().a;

            return this.applicationService.getCoordinatorApplications(this.isAll)
                .then((data: Array<app.services.ICoordinatorApplication>) => {
                    var applications = [];

                    _.each(data, (app) => {
                        //if (app.coordinator == null) {
                        //    app.coordinator = {
                        //        fullName: ""
                        //    };
                        //}

                        //if (app.primarySite == null) {
                        //    app.primarySite = {
                        //        siteCity: "",
                        //        siteState: {
                        //            id: 0,
                        //            name: ""
                        //        }
                        //    }
                        //} else {
                        //    if (app.primarySite.siteState == null) {
                        //        app.primarySite.siteState = {
                        //            id: 0,
                        //            name: ""
                        //        };
                        //    }
                        //}

                       // app.createdDateString = moment(app.createdDate).format("MM/dd/yyyy");
                        app.inspectionDateString = app.inspectionScheduleInspectionDate != null ? moment(app.inspectionScheduleInspectionDate).format("MM/DD/YYYY") : ""; //changed MM/dd/yyyy to MM/DD/YYYY after the latest version of moment.js
                        app.dueDateString = app.applicationDueDate != null ? moment(app.applicationDueDate).format("MM/DD/YYYY") : "";

                        applications.push(app);
                    });
                    this.results = applications;
                    this.gridOptions.dataSource.data(data);      
                    this.onSearch();              
                })
                .catch(() => {
                    this.notificationFactory.error("Error while getting applications.");
                });
        }

        onSearch() {
            var text = "";

            if (this.search.length >= 3) {
                text = this.search;
            }

            var apps: Array<services.ICoordinatorApplication> = [];

            _.each(this.results, (app) => {
                var appTypes = [];

                if (this.eligibility) appTypes.push(this.config.applicationTypeNames.eligibility);
                if (this.compliance) {
                    appTypes.push(this.config.applicationTypeNames.complianceCb);
                    appTypes.push(this.config.applicationTypeNames.complianceCt);
                    appTypes.push(this.config.applicationTypeNames.complianceCommon);
                    appTypes.push("Compliance Application");
                }
                if (this.annual) appTypes.push(this.config.applicationTypeNames.annual);
                if (this.renewal) appTypes.push(this.config.applicationTypeNames.renewal);
                if (this.netcord) appTypes.push(this.config.applicationTypeNames.netcord);

                var found = _.find(appTypes, (type) => {
                    return app.applicationTypeName === type;
                });

                var include = false;

                if (found) {
                    if (text !== "") {
                        if (app.organizationName.toLowerCase().indexOf(text.toLowerCase()) > -1 ||
                            app.applicationStatusName.toLowerCase().indexOf(text.toLowerCase()) > -1 ||
                            (app.location || "").toLowerCase().indexOf(text.toLowerCase()) > -1 ||
                            (app.coordinator || "").toLowerCase().indexOf(text.toLowerCase()) > -1 ||
                            app.applicationTypeName.toLowerCase().indexOf(text.toLowerCase()) > -1 ||
                            app.inspectionDateString.toLowerCase().indexOf(text.toLowerCase()) > -1 ||
                            app.dueDateString.toLowerCase().indexOf(text.toLowerCase()) > -1) {
                            include = true;

                        }
                    } else {
                        include = true;
                    }
                }

                if (this.isAll) {
                    if (include) {
                        apps.push(app);
                    }
                } else {
                    if (this.inactive) {
                        if (include) {
                            if ((app.applicationIsActive != null &&
                                app.applicationIsActive != undefined &&
                                !app.applicationIsActive) ||
                                app.applicationStatusName === "Cancelled" ||
                                app.applicationStatusName === "Complete") {
                                apps.push(app);
                            }
                        }
                    } else {
                        if (include) {
                            if ((app.applicationIsActive != null &&
                                app.applicationIsActive != undefined &&
                                app.applicationIsActive) &&
                                app.applicationStatusName !== "Cancelled" &&
                                app.applicationStatusName !== "Complete") {
                                apps.push(app);
                            }
                        }
                    }  
                }

                             
            });
            this.gridOptions.dataSource.page(1);
            this.gridOptions.dataSource.data(apps);
        }

        editOrganization(dataItem: app.services.IApplication)
        {  
            this.organizationService.getById(dataItem.organizationId)
                .then((data: app.services.IOrganization) => {
           
                    var instance = this.$uibModal.open({
                        animation: true,
                        templateUrl: "/app/modal.templates/editOrganization.html",
                        controller: "app.modal.templates.EditOrganizationController",
                        controllerAs: "vm",
                        size: 'lg',
                        backdrop: 'static',
                        windowClass: 'app-modal-window',
                        resolve: {
                            organizationid: () => {
                                return dataItem.organizationId;
                            },
                            organization: () => {
                                return data;
                            },
                            facilities: () => {
                                return null;
                            },
                            users: () => {
                                return null;
                            }
                        }
                    });
                });
                
        }

    }

    angular
        .module('app.coordinator')
        .controller('app.coordinator.ApplicationsController',
        ApplicationsController);
} 