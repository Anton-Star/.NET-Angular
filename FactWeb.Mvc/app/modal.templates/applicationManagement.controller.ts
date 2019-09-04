module app.modal.templates {
    'use strict';

    interface IApplicationType {
        id: number;
        name: string;
    }

    interface IValues {
        organizations: Array<services.IOrganization>;
        applications: Array<services.IApplication>;
        coordinators: Array<services.IUser>;
        application: services.ICoordinatorApplication;
    }

    class ApplicationManagement {
        applicationStatuses: Array<services.IApplicationStatus>;
        selectedApplicationType = "";
        selectedOrganization = "";
        coordinator = "";
        applicationStatus = "";
        dueDate: string;
        annualApplication: IApplicationType = {
            id: 5, name: "Annual Application"
        };
        renewalApplication: IApplicationType = {
            id: 6, name: "Renewal Application"
        };
        eligibilityApplication: IApplicationType = {
            id: 3, name: "Eligibility Application"
        };
        netcord: IApplicationType = {
            id: 8, name: "NETCORD"  
        };
        applicationTypes: Array<IApplicationType> = [];
        organizations: Array<services.IOrganization>;
        isEdit: boolean = false;

        static $inject = [
            '$q',
            '$window',
            '$uibModalInstance',
            'organizationService',
            'applicationService',
            'applicationStatusService',
            'notificationFactory',
            'common',
            'config',
            'values'
        ];
        constructor(
            private $q: ng.IQService,
            private $window: ng.IWindowService,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private organizationService: services.IOrganizationService,
            private applicationService: services.IApplicationService,
            private applicationStatusService: services.IApplicationStatusService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig,
            private values: IValues) {

            common.activateController([this.getApplicationStatus()], '');

            if (values.application != null) {
                this.applicationTypes.push({
                    id: 0, name: values.application.applicationTypeName
                });
                this.selectedApplicationType = values.application.applicationTypeName;
                this.selectedOrganization = values.application.organizationName;
                this.coordinator = values.application.coordinatorId || "";
                this.applicationStatus = values.application.applicationStatusId.toString();

                 if (values.application.applicationDueDate) {
                    var localDate = new Date(values.application.applicationDueDate.toString());
                     this.dueDate = (localDate.getMonth() + 1) + '/' + localDate.getDate() + '/' + localDate.getFullYear();
                }
                else {
                    var dueDateLocal = new Date();
                    if (this.selectedApplicationType == "Annual Application" || this.selectedApplicationType == "Renewal Application") {
                        dueDateLocal.setDate(dueDateLocal.getDate() + 60);                        
                        this.dueDate = (dueDateLocal.getMonth() + 1) + '/' + dueDateLocal.getDate() + '/' + dueDateLocal.getFullYear();
                    }
                    else if (this.selectedApplicationType == "Eligibility Application") {
                        dueDateLocal.setDate(dueDateLocal.getDate() + 150);
                        this.dueDate = (dueDateLocal.getMonth() + 1) + '/' + dueDateLocal.getDate() + '/' + dueDateLocal.getFullYear();
                    }
                }
                this.isEdit = true;
            }
            else {
                this.isEdit = false;
            }

            this.organizations = values.organizations.filter(function (el) {
                return el.accreditationStatusItem == null || el.accreditationStatusItem.id != "5";
            });;;
        }

        isValid(): boolean {
            return this.selectedOrganization !== "" && this.selectedApplicationType !== "";
        }

        getApplicationStatus(): ng.IPromise<void> {

            return this.applicationStatusService.getApplicationStatus()
                .then((data: Array<app.services.IApplicationStatus>) => {
                    if (data == null) {
                        this.notificationFactory.error('no items');
                    } else {
                        this.applicationStatuses = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        onSelectedOrganization(): void {
            this.applicationTypes = [];

            var hasRenewal = false;
            var hasAnnual = false;
            var hasEligibility = false;
            var hasNetcord = false;

            _.each(this.values.applications, (app: services.IApplication) => {
                if (app.organizationName === this.selectedOrganization) {
                    if (app.applicationTypeName === this.annualApplication.name && app.applicationStatusName !== this.config.applicationStatuses.cancelled && app.applicationStatusName !== this.config.applicationStatuses.complete) {
                        hasAnnual = true;
                    }

                    if (app.applicationTypeName === this.renewalApplication.name && app.applicationStatusName !== this.config.applicationStatuses.cancelled && app.applicationStatusName !== this.config.applicationStatuses.complete) {
                        hasRenewal = true;
                    }

                    if (app.applicationTypeName === this.eligibilityApplication.name && app.applicationStatusName !== this.config.applicationStatuses.cancelled && app.applicationStatusName !== this.config.applicationStatuses.complete) {
                        hasEligibility = true;
                    }

                    if (app.applicationTypeName === this.netcord.name && app.applicationStatusName !== this.config.applicationStatuses.cancelled && app.applicationStatusName !== this.config.applicationStatuses.complete) {
                        hasNetcord = true;
                    }
                }
            });

            if (!hasEligibility) {
                this.applicationTypes.push(this.eligibilityApplication);
            }

            if (!hasRenewal) {
                this.applicationTypes.push(this.renewalApplication);
            }

            if (!hasAnnual) {
                this.applicationTypes.push(this.annualApplication);
            }

            if (!hasNetcord) {
                this.applicationTypes.push(this.netcord);
            }
        }

        onSelectedType() {
            var org = _.find(this.organizations, (o: services.IOrganization) => {
                return o.organizationName === this.selectedOrganization;
            });

            if (this.selectedApplicationType === "Annual Application") {
                if (org && org.accreditationDate) {
                    var dte = moment(org.accreditationDate);
                    this.dueDate = dte.add(13, 'months').format("MM/DD/YYYY");
                    return;
                }
            }
            else if (this.selectedApplicationType === "Renewal Application") {
                if (org && org.accreditationDate) {
                    var dte = moment(org.accreditationDate);
                    this.dueDate = dte.add(24, 'months').format("MM/DD/YYYY");
                    return;
                }
            }
            else if (this.selectedApplicationType === "Eligibility Application" || this.selectedApplicationType === "NETCORD") {
                var dte = moment();
                this.dueDate = dte.add(5, 'months').format("MM/DD/YYYY");
            }
        }

        onClear(): void {
            this.selectedOrganization = "";
            this.selectedApplicationType = "";
            this.applicationTypes = [];
        }

        onSave(): void {
            this.common.showSplash();

            if (this.values.application == null) {

                this.applicationService.createApplication(this.selectedOrganization, this.selectedApplicationType, this.coordinator, this.dueDate)
                    .then((data: services.IGenericServiceResponse<services.IApplication>) => {

                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.notificationFactory.success("Application created successfully.");

                            var app: services.ICoordinatorApplication = {
                                organizationId: data.item.organizationId,
                                organizationName: data.item.organizationName,
                                location: "",
                                applicationTypeId: data.item.applicationTypeId,
                                applicationTypeName: data.item.applicationTypeName,
                                coordinatorId: data.item.coordinator.userId,
                                coordinator: data.item.coordinator.fullName,
                                applicationDueDate: data.item.dueDate,
                                outcomeStatusName: "",
                                applicationStatusId: data.item.applicationStatusId,
                                applicationStatusName: data.item.applicationStatusName,
                                applicationId: data.item.applicationId,
                                complianceApplicationId: data.item.complianceApplicationId,
                                applicationVersionTitle: data.item.applicationVersionTitle,
                                applicationUniqueId: data.item.uniqueId
                            }

                            this.$uibModalInstance.close(app);
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error creating Application");
                        this.common.hideSplash();
                    });
            } else {

                this.applicationService.updateApplicationCoordinator(this.values.application.applicationUniqueId, this.coordinator, this.applicationStatus, this.dueDate)
                    .then((data: services.IGenericServiceResponse<services.IUser>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.values.application.coordinatorId = data.item.userId;
                            this.values.application.coordinator = data.item.fullName;
                            this.notificationFactory.success("Application updated successfully.");
                            this.values.application.applicationStatusId = parseInt(this.applicationStatus);
                            this.values.application.applicationDueDate = new Date(this.dueDate);

                            var changedStatus = _.find(this.applicationStatuses, (status) => {
                                return status.id === parseInt(this.applicationStatus);
                            });

                            this.values.application.applicationStatusName = changedStatus.name;

                            this.$uibModalInstance.close(this.values.application);
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error creating Application");
                        this.common.hideSplash();
                    });
            }

        }


        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }

    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.ApplicationManagementController',
        ApplicationManagement);
} 