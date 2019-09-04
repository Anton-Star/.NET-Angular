module app.Application {
    'use strict';

    interface IPersonnelScope {
    }

    export interface ILocalUser {
        userId?: string;
        emailAddress?: string;
        fullName?: string;
        roleName?: string;
        jobFunctions?: string;
        fullUser?: services.IUser;
    }

    class PersonnelController implements IPersonnelScope {
        organization: services.IOrganization;
        application: services.IApplication;
        uniqueId: string;
        compAppId: string;
        coordinatorCredentials: string;
        coordinatorFullName: string;
        isEligibilityApplication = false;
        isStaff = true;
        canManage = false;
        isInspectorPresent = false;
        orgInspectors: ILocalUser;
        inspectors: Array<services.IInspectionScheduleDetail> = [];
        inspectorEmails = "";
        leadInspectorEmail = "";
        orgDirectorEmail = "";
        staffEmail = "";
        primaryUserEmail = "";
        isComplianceApplication: boolean;
        complianceApplication: services.IComplianceApplication;
        appDueDate = "";
        submittedDate = "";
        staffOverview = "";

        orgStaff: Array<ILocalUser>;
        gridOrgStaff = {
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
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };
        isObserverOrAuditor = false;
        isFact = false;

        orgConsultants: Array<ILocalUser>;
        gridOrgConsultants = {
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
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        gridOptions = {
            sortable: true,
            filterable: false,
            selectable: "row",
            editable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$location',
            '$q',
            '$window',
            '$uibModal',
            'inspectionService',
            'organizationService',
            'applicationService',
            'applicationSettingService',
            'userService',
            'notificationFactory',
            'common',
            'config',
            'inspectionScheduleService'
        ];
        constructor(
            private $location: ng.ILocationService,
            private $q: ng.IQService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private inspectionService: services.IInspectionService,
            private organizationService: services.IOrganizationService,
            private applicationService: services.IApplicationService,
            private applicationSettingService: services.IApplicationSettingService,
            private userService: services.IUserService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig,
            private inspectionScheduleService: services.IInspectionScheduleService) {
            
            this.uniqueId = $location.search().app;
            this.compAppId = $location.search().c;

            if (this.compAppId != "") {
                this.isComplianceApplication = true;
            }
            else {
                this.isComplianceApplication = false;
            }

            if (this.isComplianceApplication) {
                this.common.checkItemValue(this.config.events.complianceApplicationLoaded, this.common.compApplication, true)
                    .then(() => {
                        this.complianceApplication = this.common.compApplication;

                        _.each(this.common.compApplication.complianceApplicationSites, (appSite: services.IComplianceApplicationSite) => {
                            _.each(appSite.applications, (app: services.IApplication) => {

                                if (app.submittedDateString && this.submittedDate === "")
                                    this.submittedDate = app.submittedDateString;

                                if (app.dueDateString && this.appDueDate === "")
                                    this.appDueDate = app.dueDateString;
                            });
                        });

                        this.common.hideSplash();
                    });

                this.common.checkItemValue(this.config.events.complianceApplicationInspectorsLoaded, this.common.compAppInspectors, false)
                    .then(() => {
                        if (this.common.compAppInspectors) {
                            this.isInspectorPresent = true;

                            _.each(this.common.compAppInspectors, (row: services.IInspectionScheduleDetail) => {
                                if (row.isLead) {
                                    if (this.leadInspectorEmail.indexOf(row.user.emailAddress) === -1) {
                                        this.leadInspectorEmail = row.user.emailAddress;
                                        row.roleName = "Team Leader";

                                    }
                                } else if (this.inspectorEmails.indexOf(row.user.emailAddress) === -1) {
                                    this.inspectorEmails += row.user.emailAddress + ",";
                                }
                            });
                            this.inspectors = this.common.compAppInspectors;

                            this.gridOptions.dataSource.data(this.inspectors);

                            console.log(this.inspectors);

                        } else {
                            this.isInspectorPresent = false;
                        } 
                    });
            } else {
                this.common.checkItemValue(this.config.events.applicationInspectorsLoaded, this.common.applicationInspectors, false)
                    .then(() => {
                        this.inspectors = this.common.applicationInspectors;
                    });
            }

            this.common.checkItemValue(this.config.events.applicationLoaded, this.common.application, false)
                .then(() => {
                    this.getApplication(this.common.application);
                });

            this.common.checkItemValue(this.config.events.applicationSettingsLoaded, this.common.applicationSettings, false)
                .then(() => {
                    var record = _.find(this.common.applicationSettings, (s) => {
                        return s.name === "Application Staff Overview";
                    });

                    if (record) {
                        this.staffOverview = record.value;
                    }
                });

            this.common.checkItemValue(this.config.events.organizationLoaded, this.common.organization, false)
                .then(() => {
                    this.organization = this.common.organization;
                    this.primaryUserEmail = this.organization.primaryUser ? this.organization.primaryUser.emailAddress : "";
                    if (this.orgDirectorEmail === "") {
                        this.orgDirectorEmail = this.primaryUserEmail;
                    }
                });
        }

        getOrgStaff(): ng.IPromise<void> {
            return this.organizationService.getOrgUsers(this.application.organizationName, true)
                .then((data: Array<services.IUser>) => {
                    var localUsers: Array<ILocalUser> = [];
                    this.orgDirectorEmail = "";
                    
                    _.each(data, (user: services.IUser) => {
                        var tempLocalUser: ILocalUser = {};
                        tempLocalUser.fullName = user.firstName + ' ' + user.lastName;
                        var tempCredentials = _.map(user.credentials, function (cred) { return cred.name; }).join(', ');

                        if (tempCredentials.length > 0) {
                            tempLocalUser.fullName = tempLocalUser.fullName + ', ' + tempCredentials;
                        }

                        tempLocalUser.roleName = user.role.roleName;

                        var tempOrgName = this.application.organizationName;

                        tempLocalUser.jobFunctions = "";
                        _.each(user.organizations, (o) => {
                           if (o.organization.organizationName === tempOrgName) {
                               tempLocalUser.jobFunctions += o.jobFunction.name + ", ";
                           } 
                        });

                        if (tempLocalUser.jobFunctions !== "") {
                            tempLocalUser.jobFunctions = tempLocalUser.jobFunctions.substr(0, tempLocalUser.jobFunctions.length - 2);
                        }
                        

                        tempLocalUser.emailAddress = user.emailAddress;
                        tempLocalUser.fullUser = user;

                        if ((user.isAuditor || user.isObserver) && this.inspectorEmails.indexOf(user.emailAddress) === -1) {
                            this.inspectorEmails += user.emailAddress + ",";
                        }

                        if (this.organization.organizationDirectors) {
                            var dir = _.find(this.organization.organizationDirectors, (d) => {
                                return user.userId == d.userId;
                            });

                            if (dir) {
                                this.orgDirectorEmail = dir.emailAddress;
                            } else {
                                this.staffEmail += user.emailAddress + ",";
                            }                            
                        } else {
                            this.staffEmail += user.emailAddress + ",";
                        }

                        localUsers.push(tempLocalUser);
                    });
                    this.orgStaff = localUsers;
                    console.log('users', localUsers, data);
                    this.gridOrgStaff.dataSource.data(localUsers);

                    if (this.orgDirectorEmail === "") {
                        this.orgDirectorEmail = this.primaryUserEmail;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organization staff.");
                });
        }

        getOrgInspectors(): ng.IPromise<void> {
            if (!this.isEligibilityApplication) {
                return this.organizationService.getOrgInspectors(this.uniqueId)
                    .then((data: services.IInspection) => {

                        if (data != null) {
                            var tempLocalUser: ILocalUser = {};
                            tempLocalUser.fullName = data.user.firstName + ' ' + data.user.lastName
                            var tempCredentials = _.map(data.user.credentials, function (cred) { return cred.name; }).join(', ')

                            if (tempCredentials.length > 0) {
                                tempLocalUser.fullName = tempLocalUser.fullName + ', ' + tempCredentials;
                            }

                            tempLocalUser.roleName = data.user.role.roleName;
                            tempLocalUser.jobFunctions = _.map(data.user.organizations, function (jF) { return jF.jobFunction.name; }).join(', ');
                            tempLocalUser.emailAddress = data.user.emailAddress;

                            this.orgInspectors = tempLocalUser;
                            
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error getting organization inspectors.");
                    });
            }
            else {
                this.isInspectorPresent = false;
            }
        }

        getOrgConsultants(): ng.IPromise<void> {
            return this.organizationService.getOrgConsultants(this.application.organizationId)
                .then((data: Array<services.IOrganizationConsultant>) => {
                    var localUsers: Array<ILocalUser> = [];
                    _.each(data, (orgCon: services.IOrganizationConsultant) => {
                        var tempLocalUser: ILocalUser = {};
                        tempLocalUser.fullName = orgCon.user.firstName + ' ' + orgCon.user.lastName;
                        var tempCredentials = _.map(orgCon.user.credentials, function (cred) { return cred.name; }).join(', ');

                        if (tempCredentials.length > 0) {
                            tempLocalUser.fullName = tempLocalUser.fullName + ', ' + tempCredentials;
                        }

                        tempLocalUser.roleName = orgCon.user.role.roleName;
                        tempLocalUser.jobFunctions = _.map(orgCon.user.organizations, function (jF) { return jF.jobFunction.name; }).join(', ');
                        tempLocalUser.emailAddress = orgCon.user.emailAddress;

                        localUsers.push(tempLocalUser);
                    });
                    this.orgConsultants = localUsers;
                    this.gridOrgConsultants.dataSource.data(localUsers);
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organization consultants.");
                });
        }

        getApplication(data: services.IApplication) {
            this.application = data;

            if (this.application.applicationTypeId === 3) {
                this.isEligibilityApplication = true;
            }

            this.getAccreditationRole(this.uniqueId);

            if (this.inspectorEmails.indexOf(this.application.coordinator.emailAddress) === -1) {
                this.inspectorEmails += this.application.coordinator.emailAddress + ",";
            }

            if (this.staffEmail.indexOf(this.application.coordinator.emailAddress) === -1) {
                this.staffEmail += this.application.coordinator.emailAddress + ",";
            }

            this.coordinatorFullName = this.application.coordinator.firstName + ' ' + this.application.coordinator.lastName;
            this.coordinatorCredentials = _.map(this.application.coordinator.credentials, function (cred) { return cred.name; }).join(', ');

            if (this.coordinatorCredentials.length > 0) {
                this.coordinatorFullName = this.coordinatorFullName + ', ' + this.coordinatorCredentials;
            }

            if (!this.common.currentUser.isImpersonation) {
                this.common.activateController([this.getOrgStaff(), this.getOrgInspectors(), this.getOrgConsultants()], '');
            } else {
                this.common.activateController([this.getOrgStaff(), this.getOrgInspectors()], '');
            }
        }

        getAccreditationRole(appId: string) {
            this.inspectionScheduleService.getAccreditationRole(null, appId)
                .then((data: services.IGenericServiceResponse<services.IAccreditationRole>) => {
                    if (data.item != null) {                        
                        this.isObserverOrAuditor = (data.item.accreditationRoleName.indexOf("Trainee") !== -1 || data.item.accreditationRoleName.indexOf("Auditor") !== -1);
                    }
                    this.isFact = this.common.currentUser.role.roleName === this.config.roles.factAdministrator || this.common.currentUser.role.roleName === this.config.roles.factCoordinator;
                    this.isStaff = this.common.currentUser.role.roleName === this.config.roles.factCoordinator ||
                        this.common.currentUser.role.roleName === this.config.roles.factQualityManager;
                    this.canManage = this.common.currentUser.canManageUsers && !this.common.isConsultantCoordinator();
                });
        }

        addUser(): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/editUser.html",
                controller: "app.modal.templates.EditUserController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    parentUser: () => {
                        return null;
                    },
                    isNewUser: () => {
                        return true;
                    },
                    currentOrganization: () => {
                        return this.organization;
                    },
                    role: () => {
                        return "";
                    },
                    isPersonnel: () => {
                        return false;
                    }
                }
            });

            instance.result.then(() => {
                this.common.showSplash();
                this.$q.all([this.getOrgStaff()])
                    .then(() => {
                        this.common.hideSplash();
                    });
            });
        }

        editUser(rowData): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/editUser.html",
                controller: "app.modal.templates.EditUserController",
                controllerAs: "vm",
                size: 'lg',
                windowClass: 'app-modal-window',
                resolve: {
                    parentUser: () => {
                        return rowData.fullUser;
                    },
                    isNewUser: () => {
                        return false;
                    },
                    currentOrganization: () => {
                        return this.common.currentUser.userId === rowData.fullUser.userId ? null : this.organization;
                    },
                    role: () => {
                        return "";
                    },
                    isPersonnel: () => {
                        return true;
                    }
                }
            });
        }
    }

    angular
        .module('app.application')
        .controller('app.application.PersonnelController',
        PersonnelController);
}