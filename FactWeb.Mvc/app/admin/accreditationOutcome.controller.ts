module app.admin {
    'use strict';

    interface IEmailResult {
        templateHtml: string;
        documents: Array<services.IDocument>;
        to?: string;
        cc?: string;
        subject?: string;
        includeAccreditationReport?: boolean
    }

    class AccreditationOutcomeController {
        organizations: Array<services.IOrganization>;
        applications: Array<services.IApplication>;
        outcomeStatuses: Array<services.IOutcomeStatus>;
        reportReviewStatuses: Array<services.IReportReviewStatus>;
        accreditationOutcomes: Array<services.IAccreditationOutcome>;
        accreditationOutcomeId: number;
        selectedOrganization: string;
        fullOrg: services.IOrganization;
        selectedApplication: string;
        selectedOutcomeStatus: number;
        selectedReportReviewStatus: number;
        committeeDate: string;
        dueDate: string;
        kCommitteeDate: string;
        results: Array<services.IAccreditationOutcome>;
        saveMode: boolean;
        useTwoYearCycle: boolean;
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
                pageSize: 15
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$location',
            '$window',
            '$uibModal',
            'cacheService',
            'applicationService',
            'organizationService',
            'outcomeStatusService',
            'reportReviewStatusService',
            'accreditationOutcomeService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private cacheService: services.ICacheService,
            private applicationService: services.IApplicationService,
            private organizationService: services.IOrganizationService,
            private outcomeStatusService: services.IOutcomeStatusService,
            private reportReviewStatusService: services.IReportReviewStatusService,
            private accreditationOutcomeService: app.services.IAccreditationOutcomeService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.saveMode = false;

            if (this.$location.search().c && this.$location.search().c !== "") {
                if (this.common.organization) {
                    console.log('comp', this.common.compApp, this.common.organization);
                    this.organizations = [
                        this.common.organization
                    ];
                    this.selectedOrganization = this.common.organization.organizationName;
                    this.add();
                } else {
                    this.common.checkItemValue(this.config.events.organizationLoaded, this.common.organization, false)
                        .then(data => {
                            console.log('comp', this.common.compApp, this.common.organization);
                            this.organizations = [
                                this.common.organization
                            ];
                            this.selectedOrganization = this.common.organization.organizationName;
                            this.add();
                        });
                }
            } else {
                console.log('no comp');
                common.activateController([this.getOrganizations()], 'accreditationOutcomeController');
            }
            
        }

        getOrganizations(): ng.IPromise<void> {
            return this.organizationService.getFlatOrganizations()
                .then((data: Array<app.services.IOrganization>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Organizations found');
                    } else {
                        this.organizations = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organizations. Please contact system admin.");
                });
        }

        selectedOrganizationChange() {

            if (this.common.organization) {
                this.fullOrg = this.common.organization;
                this.useTwoYearCycle = this.common.organization.useTwoYearCycle;
            } else {
                var selOrg = this.selectedOrganization;
                var selOrganization = this.organizations.filter(function (org) {
                    return org.organizationName === selOrg;
                })[0];

                this.fullOrg = selOrganization;
                this.useTwoYearCycle = selOrganization.useTwoYearCycle;
            }
            

            this.common.showSplash();

            this.applicationService.getApplicationsSimple(this.fullOrg.organizationName)
                .then((data: Array<app.services.IApplication>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Applications found');
                    } else {
                        this.applications = data;
                    }
                    this.common.hideSplash();

                    if (this.common.organization) {
                        var f = _.find(this.applications, (a) => {
                            return a.applicationTypeName === "Compliance Application";
                        });

                        if (f) {
                            this.selectedApplication = f.applicationId.toString();
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting applications. Please contact system admin.");
                    this.common.hideSplash();
                });
        }

        add() {
            this.selectedOrganizationChange();
            this.getOutcomeStatus();
            this.getReportReviewStatus();
            this.saveMode = true;
        }

        getOutcomeStatus(): ng.IPromise<void> {
            return this.outcomeStatusService.getAll()
                .then((data: Array<app.services.IOutcomeStatus>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Outcome Statuses found');
                    } else {
                        this.outcomeStatuses = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting Outcome Statuses. Please contact system admin.");
                });
        }

        getReportReviewStatus(): ng.IPromise<void> {
            return this.reportReviewStatusService.getAll()
                .then((data: Array<app.services.IReportReviewStatus>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Report Review Statuses found');
                    } else {
                        this.reportReviewStatuses = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting Report Review Status. Please contact system admin.");
                });
        }

        view() {
            this.common.showSplash();
            var org = _.find(this.organizations, (o: services.IOrganization) => {
                return o.organizationName === this.selectedOrganization;
            });
            this.accreditationOutcomeService.getAccreditationOutcome(org.organizationId)
                .then((data: Array<app.services.IAccreditationOutcome>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Accreditation Outcome items found.');
                    } else {
                        this.accreditationOutcomes = data;
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting Accreditation Outcome. Please contact system admin.");
                    this.common.hideSplash();
                });
        }

        isDate = function (x) {
            return x instanceof Date;
        };

        isDataValid(): boolean {
            var isValid = false;

            if ((this.committeeDate !== "" && this.committeeDate != null)
                && this.selectedApplication != null
                && this.selectedOutcomeStatus != null
                && this.selectedReportReviewStatus != null
                && (!this.showDueDate() || (this.dueDate !== "" && this.dueDate != null))) {
                isValid = true;
            }

            return isValid;
        }

        showDueDate() {
            var f = _.find(this.outcomeStatuses, (o) => {
                return o.id == this.selectedOutcomeStatus && (o.name === "Level 2" || o.name === "Level 3" || o.name === "Level 4" || o.name === "Level 5");
            });

            if (f) {
                return true;
            } else {
                return false;
            }
        }

        save(sendEmail: boolean): void {
            var app = _.find(this.applications, (a: services.IApplication) => {
                return a.applicationId.toString() === this.selectedApplication;
            });

            var outcome = _.find(this.outcomeStatuses, (out) => {
                return out.id == this.selectedOutcomeStatus;
            });

            if (!this.showDueDate()) {
                this.dueDate = null;
            }

            if (app && outcome.name !== "Level 1") {
                this.common.showSplash();

                this.applicationService.compAppHasRfis(app.complianceApplicationId)
                    .then(data => {
                        console.log('has rfis', data);
                        this.common.hideSplash();
                        if (data === false) {
                            var yn = this.$uibModal.open({
                                animation: true,
                                templateUrl: "/app/modal.templates/yesNoBox.html",
                                controller: "app.modal.templates.YesNoController",
                                controllerAs: "vm",
                                size: 'xxl',
                                windowClass: 'app-modal-window',
                                resolve: {
                                    message: () => {
                                        return "There are no RFIs for the applicant and the application status will be changed to \"Applicant Response\".  Would you like to proceed?";
                                    }
                                }
                            });

                            yn.result.then((result: boolean) => {
                                    console.log('result', result);
                                    if (result) {
                                        this.onContinueSave(sendEmail);
                                    }
                                },
                                () => {
                                });
                        } else {
                            this.onContinueSave(sendEmail);
                        }
                    })
                    .catch(e => {
                        this.notificationFactory.error('Error checking for RFIs. ' + e);
                        this.common.hideSplash();
                        return;
                    });
            } else {
                this.onContinueSave(sendEmail);
            }

            
        }

        onContinueSave(sendEmail: boolean) {
            if (sendEmail) {
                var application = _.find(this.applications, (app) => {
                    return app.applicationId.toString() === this.selectedApplication;
                });

                var outcome = _.find(this.outcomeStatuses, (out) => {
                    return out.id == this.selectedOutcomeStatus;
                });

                var reportViewStatus = _.find(this.reportReviewStatuses, (status) => {
                    return status.id == this.selectedReportReviewStatus;
                });

                var instance = this.$uibModal.open({
                    animation: true,
                    templateUrl: "/app/modal.templates/accreditation.html",
                    controller: "app.modal.templates.AccreditationController",
                    controllerAs: "vm",
                    size: 'xxl',
                    windowClass: 'app-modal-window',
                    resolve: {
                        values: () => {
                            return {
                                org: this.fullOrg,
                                application: application,
                                outcome: outcome,
                                reportViewStatus: reportViewStatus
                            };
                        }
                    }
                });

                instance.result.then((result: IEmailResult) => {
                    this.saveData(result.templateHtml, result.documents, result.to, result.cc, result.subject, result.includeAccreditationReport);
                }, () => {
                    this.notificationFactory.error("Save Cancelled");
                });
            } else {
                this.saveData("", [], "", "", "", false);
            }
        }

        saveData(email: string, documents: Array<services.IDocument>, to: string, cc: string, subject: string, includeAccreditationReport: boolean) {
            var org = _.find(this.organizations, (o: services.IOrganization) => {
                return o.organizationName === this.selectedOrganization;
            });

            if (org == null && this.organizations.length === 1) {
                org = this.organizations[0];
            }

            this.common.showSplash();

            this.accreditationOutcomeService.save(org.organizationId, this.selectedApplication, this.selectedOutcomeStatus, this.selectedReportReviewStatus,
                this.committeeDate, this.useTwoYearCycle, email, documents, to, cc, subject, includeAccreditationReport, this.dueDate)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item == true) {
                            this.clearForm();
                            this.saveMode = false;
                            if (data.message != '' && data.message != null)
                                this.notificationFactory.success(data.message);

                        } else {
                            this.notificationFactory.error("Error.");
                        }
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }

        delete(id: number) {
            this.common.showSplash();

            this.accreditationOutcomeService.remove(id)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item == true) {
                            this.notificationFactory.success(data.message);
                        }
                    }
                    this.common.hideSplash();
                    this.view();
                })
                .catch(() => {
                    this.notificationFactory.error("Error while deleting Accreditation Outcome.");
                    this.common.hideSplash();
                });
        }

        cancel(): void {
            this.clearForm();
            this.saveMode = false;
        }

        clearForm(): void {
            this.selectedOrganization = null;
            this.selectedApplication = null;
            this.applications = null;
            this.accreditationOutcomeId = 0;
            this.selectedOutcomeStatus = null;
            this.selectedReportReviewStatus = null;
            this.committeeDate = null;
            this.results = null;
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.AccreditationOutcomeController',
        AccreditationOutcomeController);
}  