module app.modal.templates {
    'use strict';

    interface IEditUser {
        user: services.IUser;
        jobFunctions: Array<services.IJobFunction>;
        save: () => void;
        cancel: () => void;
        passwordsMatch(): boolean;
        isInvalidPassword(): boolean;
        isNonSystemUser: boolean;
    }

    class EditUserController implements IEditUser {
        user: services.IUser;
        jobFunction: Array<services.IJobFunction>;
        jobFunctions: Array<services.IJobFunction>;
        languages: Array<services.ILanguage>;
        memberships: Array<services.IMembership>;
        organizations: Array<services.IOrganization>;
        roles: Array<services.IRole>;
        credential: Array<services.ICredential>;
        credentials: Array<services.ICredential>;
        password: string = "";
        confirmPassword: string = "";
        isNonSystemUser = false;
        file: any;
        isIsctItem: boolean = false;
        isAsbmtItem: boolean = false;
        isctItem: string = "";
        asbmtItem: string = "";
        showRole = true;
        isFact = false;

        newOrg: services.IOrganization;
        newJobFunction: services.IJobFunction;
        grid: kendo.ui.Grid;
        gridOptions = {
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "organization.organizationName", title: "Organization" },
                { field: "jobFunction.name", title: "Job Function" },
                { template: "<button class='k-button' ng-click='vm.removeOrg(dataItem)'>Remove</button>" }
            ],
            pageable: {
                pageSize: 10
            }
        };
        emailDisabled = false;

        static $inject = [
            '$scope',
            'userService',
            'roleService',
            'credentialService',
            'organizationService',
            'accountService',
            'jobFunctionService',
            'languageService',
            'membershipService',
            'trueVaultService',
            'documentService',
            'notificationFactory',
            'common',
            'parentUser',
            'isNewUser',
            'currentOrganization',
            'role',
            'isPersonnel',
            '$uibModalInstance',
            'config'
        ];

        constructor(
            $scope: ng.IScope,
            private userService: services.IUserService,
            private roleService: services.IRoleService,
            private credentialService: services.ICredentialService,
            private organizationService: services.IOrganizationService,
            private accountService: services.IAccountService,
            private jobFunctionService: services.IJobFunctionService,
            private languageService: services.ILanguageService,
            private membershipService: services.IMembershipService,
            private trueVaultService: services.ITrueVaultService,
            private documentService: services.IDocumentService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private parentUser: services.IUser,
            private isNewUser: boolean,
            private currentOrganization: services.IOrganization,
            private role: string,
            private isPersonnel: boolean,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private config: IConfig) {
            this.credentials = this.credential;
            this.jobFunctions = this.jobFunction;
            this.emailDisabled = isPersonnel;

            this.isFact = this.common.isFact();
            if (parentUser != null && !isNewUser) {
                console.log(parentUser);
                parentUser.medicalLicenseExpiry = moment(parentUser.medicalLicenseExpiry).toDate();
                this.user = parentUser;
                this.gridOptions.dataSource.data(this.user.organizations || []);
                this.isNonSystemUser = this.user.role.roleName === "Non-System User";
            } else {
                this.user = {
                organizations:[]
                };
                this.gridOptions.dataSource.data([]);
            }

            $scope.$watch("vm.file", (newVal: any, oldVal) => {
                if (newVal != "" && newVal)
                    this.onSaveFile();

            });
            
            if (this.currentOrganization) {
                var orgs: services.IOrganization[] = [this.currentOrganization];

                this.organizations = orgs;
                this.newOrg = this.currentOrganization;

                this.showRole = !this.common.isUser() ||
                    isNewUser || !this.user.role ||
                    (this.user.role &&
                        this.user.role.roleName !== this.config.roles.factConsultant &&
                        this.user.role.roleName !== this.config.roles.inspector);

                common.activateController([this.getRoles(), this.getJobFunctions(), this.getCredentials(), this.getLanguages(), this.getMemberships()], 'editUserController');
            } else {
                common.activateController([this.getOrganizations(), this.getRoles(), this.getJobFunctions(), this.getCredentials(), this.getLanguages(), this.getMemberships()], 'editUserController');
            }
        }

        getOrganizations(): ng.IPromise<void> {
            return this.organizationService.getFlatOrganizations()
                .then((data: Array<app.services.IOrganization>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Organizations found');
                    } else {
                        data = _.sortBy(data, 'organizationName');

                        if (!this.isFact && !this.currentOrganization) {
                            this.organizations = [];
                            _.each(this.user.organizations, (o) => {
                                var org = _.find(data, (or) => {
                                    return o.organization.organizationId === or.organizationId;
                                });

                                if (org) {
                                    this.organizations.push(org);
                                }
                            });
                        } else {
                            if (this.currentOrganization != null && this.common.currentUser.role.roleId === this.config.roleIds.user) {
                                var currentOrganizationId = this.currentOrganization.organizationId;
                                this.organizations = data.filter((el) => {
                                    return el.organizationId === currentOrganizationId;
                                });
                            } else {
                                this.organizations = data;
                            }
                        }
                            
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organizations. Please contact system admin.");
                });
        }

        getRoles(): ng.IPromise<void> {
            return this.roleService.getRolesByRole(this.common.currentUser.role.roleId)
                .then((data: Array<app.services.IRole>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Roles found');
                    } else {
                        if (this.common.currentUser.role.roleName === this.config.roles.inspector || this.common.isUser()) {
                            this.roles = _.filter(data,
                                (d) => {
                                    return d.roleName !== this.config.roles.factAdministrator &&
                                        d.roleName !== this.config.roles.factConsultant &&
                                        d.roleName !== this.config.roles.factConsultantCoordinator &&
                                        d.roleName !== this.config.roles.factCoordinator &&
                                        d.roleName !== this.config.roles.inspector &&
                                        d.roleName !== this.config.roles.qualityManager;
                                });
                        } else {
                            this.roles = data;
                        }

                        if (!this.isNewUser && this.user.role.roleName === this.config.roles.inspector) {
                            this.showRole = false;
                            this.roles.push({
                                roleId: 32,
                                roleName: this.config.roles.inspector
                            });
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting roles. Please contact system admin.");
                });
        }

        getJobFunctions(): ng.IPromise<void> {
            return this.jobFunctionService.getAll()
                .then((data: Array<services.IJobFunction>) => {
                    this.jobFunctions = data.filter(function (el) {
                        return el.name.toLowerCase().trim() !== "organization director" && el.name.toLowerCase().trim() !== "primary contact" && el.name.toLowerCase().trim() !== "organization admin";
                    });;

                    if (!this.isNewUser) {
                        _.forEach(this.jobFunctions, (jobFunction: services.IJobFunction) => {
                            var found = _.find(this.user.jobFunctions, (type: services.IJobFunction) => {
                                return jobFunction.id === type.id;
                            });

                            if (found) {
                                jobFunction.selected = true;
                            } else {
                                jobFunction.selected = false;
                            }
                        });
                    }

                    if (this.role) {
                        var jobFunc = _.find(this.jobFunctions, (jf: services.IJobFunction) => {
                            if (this.role === "contact") {
                                return jf.name === "Organization Admin";
                            }

                            return false;
                        });

                        if (jobFunc) {
                            this.newJobFunction = jobFunc;
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to get Job Functions. Please contact support.");
                });
        }

        getLanguages(): ng.IPromise<void> {
            return this.languageService.getAll()
                .then((data: Array<services.ILanguage>) => {
                    this.languages = data;

                    if (!this.isNewUser) {
                        _.forEach(this.languages, (language: services.ILanguage) => {
                            var found = _.find(this.user.languages, (type: services.ILanguage) => {
                                return language.id === type.id;
                            });

                            if (found) {
                                language.isSelected = found.isSelected != null? found.isSelected : true;
                            } else {
                                language.isSelected = false;
                            }
                        });
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to get languages. Please contact support.");
                });
        }

        getMemberships(): ng.IPromise<void> {
            return this.membershipService.getAll()
                .then((data: Array<services.IMembership>) => {
                    this.memberships = data;

                    if (!this.isNewUser) {
                        _.forEach(this.memberships, (membership: services.IMembership) => {
                            var found = _.find(this.user.memberships, (type: services.IUserMembership) => {
                                return membership.id === type.membership.id;
                            });

                            if (found) {
                                membership.isSelected = true;

                                if (membership.name === "ISCT" || membership.name === "ISCT-Europe") {
                                    this.isctItem = found.membershipNumber;
                                    this.isIsctItem = true;
                                }

                                if (membership.name === "ASBMT") {
                                    this.asbmtItem = found.membershipNumber;
                                    this.isAsbmtItem = true;
                                }

                            } else {
                                membership.isSelected = false;
                            }
                        });
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to get memberships. Please contact support.");
                });
        }

        getCredentials(): ng.IPromise<void> {
            return this.credentialService.getAll()
                .then((data: Array<app.services.ICredential>) => {
                    if (data == null) {
                        this.notificationFactory.error('No Credentials found');
                    } else {
                        this.credentials = data;

                        if (!this.isNewUser) {
                            _.forEach(this.credentials, (credential: services.ICredential) => {
                                var found = _.find(this.user.credentials, (type: services.ICredential) => {
                                    return credential.id === type.id;
                                });

                                if (found) {
                                    credential.isSelected = found.isSelected != null ? found.isSelected : true;
                                } else {
                                    credential.isSelected = false;
                                }
                            });
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting credentials. Please contact system admin.");
                });
        }

        isInvalid(): boolean {
            return !this.user.role ||
                !this.user.role.roleId ||
                this.user.role.roleId == null ||
                this.user.firstName == null ||
                this.user.firstName === "" ||
                this.user.lastName == null ||
                this.user.lastName === "" ||
                (!this.isNonSystemUser && (this.user.emailAddress == null || this.user.emailAddress === "")) ||
                (!this.isNonSystemUser && this.user.isActive == null);
        }

        save(): void {
            this.user.credentials = this.credentials;
            this.user.languages = this.languages;
            this.user.jobFunctions = this.jobFunctions;
            this.user.memberships = [];

            if (!this.common.isFact() && (!this.user.organizations || this.user.organizations.length === 0)) {
                this.notificationFactory.error("You must add at least one Job Function.");
                return;
            }

            _.forEach(this.memberships, (membership: services.IMembership) => {
                if (membership.isSelected) {
                    var userMembership: services.IUserMembership = {
                        membership: membership,
                        membershipNumber: "",
                        isSelected: true
                    };

                    if (membership.name === "ISCT" || membership.name === "ISCT-Europe") {
                        userMembership.membershipNumber = this.isctItem;
                    }

                    if (membership.name === "ASBMT") {
                        userMembership.membershipNumber = this.asbmtItem;
                    }

                    this.user.memberships.push(userMembership);
                }
            });

            this.onSave(true);
        }

        onSave(checkForExisting: boolean) {
            this.common.showSplash();
            this.userService.save(this.user, this.password, this.confirmPassword, this.isNewUser, this.isNewUser && !checkForExisting)
                .then((data: app.services.IGenericServiceResponse<app.services.IUser>) => {
                    if (data.hasError) {
                        if (checkForExisting && data.message.indexOf("User with Email Address ") > -1 &&
                            data.message.indexOf("already exists.") > -1) {
                            this.common.hideSplash();
                            if (confirm("An account already exists for this user.  Would you like to add this user to your organization?")) {
                                this.onSave(false);
                            }
                        } else {
                            this.notificationFactory.error(data.message);
                            this.common.hideSplash();
                        }

                    } else {
                        this.notificationFactory.success("User saved successfully.");
                        this.$uibModalInstance.close(data.item);
                        this.common.hideSplash();
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save user. Please contact support.");
                    this.common.hideSplash();
                });
        }

        passwordsMatch(): boolean {
            return this.password === this.confirmPassword;
        }

        isInvalidPassword(): boolean {
            var hasError = false;

            if (this.isNewUser && this.user.role.roleId != 19) {
                if (this.common.isValidPassword(this.password) == true) {
                    hasError = true;                   
                }
            }

            return hasError || !this.passwordsMatch();
        }

        validatePassword(): void {             
            if (this.common.isValidPassword(this.password) == true){
                this.notificationFactory.warning("Passwords must be at least 8 characters in length and include at least 1 numeric, one English alphabet character, and one special character");
            }               
        }
        
        validateConfirmPassword(): void {
            if (this.password !== this.confirmPassword) {
                this.notificationFactory.warning("Passwords do not match.  Please re-enter");
            }
        }


        selectedRoleChange() {
            var selRole = this.user.role.roleId;

            if (selRole == 19) {
                this.isNonSystemUser = true;
            }
            else {
                this.isNonSystemUser = false;
            }
        }

        onSaveFile(): void {
            var fileName = this.file.name.replace(/\.[^/.]+$/, "") + " - " + this.user.firstName + this.user.lastName + this.file.name.substring(this.file.name.indexOf("."));

            this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.config.genericKey, this.file, fileName, "")
                .then((data: services.ITrueVaultBlobResponse) => {
                    if (data.result !== "success") {
                        this.notificationFactory.error("Error trying to save document. Please contact support.");
                        this.common.hideSplash();
                    } else {
                        this.user.medicalLicensePath = fileName + ";" + data.blob_id;
                        this.notificationFactory.success("License uploaded successfully.");
                    }
                })
                .catch((e) => {
                    if (typeof (e) === 'string' && e.indexOf("404.13") != -1) {
                        this.notificationFactory.error("Error trying to save document. Maximum allowed upload file size is 30MB.");
                    }
                    else {
                        this.notificationFactory.error("Error trying to save to True Vault. Please contact support.");
                    }

                    this.common.hideSplash();
                });
        }

        onDownload(): void {
            var vals = this.user.medicalLicensePath.split(';');
            this.trueVaultService.getBlob(this.config.genericKey,
                vals[1],
                this.common.currentUser.documentLibraryAccessToken,
                this.config.factKey)
                .then((data: any) => {
                    var fileType = this.trueVaultService.getFileType(vals[0]);
                    var file = new Blob([data.response], { type: fileType });
                    saveAs(file, vals[0]);
                })
                .catch((e) => {
                    this.notificationFactory.error("Cannot get document from True Vault. " + e);
                });
        }

        addOrg(): void {

            if (this.user.organizations == null || this.user.organizations == undefined)            
                this.user.organizations = [];

            
            var found = _.find(this.user.organizations, (userOrg: services.IUserOrganization) => {
                return userOrg.organization.organizationId == this.newOrg.organizationId && userOrg.jobFunction.id == this.newJobFunction.id;
            });

            if (found) {
                this.notificationFactory.error('This Organization and Job Function already exist.');
            }
            else {
                this.user.organizations.push({
                    organization: this.newOrg,
                    jobFunction: this.newJobFunction
                });
            }

            this.gridOptions.dataSource.data(this.user.organizations);
            this.newOrg = null;
            this.newJobFunction = null;
        }

        removeOrg(row: services.IUserOrganization): void {            
            _.remove(this.user.organizations, (o) => {
                return o.organization.organizationId === row.organization.organizationId && o.jobFunction.id === row.jobFunction.id;
            });
            this.gridOptions.dataSource.data(this.user.organizations);

        }

        cancel(): void {
            this.$uibModalInstance.dismiss('cancel');
        }

        onMembershipSelected(membership: services.IMembership): void {
            var isct = _.find(this.memberships, (member: services.IMembership) => {
                return (member.name === "ISCT" && member.isSelected) || (member.name === "ISCT-Europe" && member.isSelected);
            });

            if (isct) {
                this.isIsctItem = true;
            } else {
                this.isIsctItem = false;
                this.isctItem = "";
            }

            if (membership.name === "ASBMT") {
                if (membership.isSelected) {
                    this.isAsbmtItem = true;
                } else {
                    this.isAsbmtItem = false;
                    this.asbmtItem = "";
                }
            }
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.EditUserController',
        EditUserController);
}