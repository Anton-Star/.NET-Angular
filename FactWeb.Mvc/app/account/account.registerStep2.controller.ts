module app.account {
    'use strict';

    interface IStep2Scope {
        languages: Array<services.ILanguage>;
        jobFunctions: Array<services.IJobFunction>;
        memberships: Array<services.IMembership>;
        webPhoto: any;
        isctItem: services.IUserMembership;
        asbmtItem: services.IUserMembership;
        webOptions: any;
        save: () => void;
        user: services.IUser;
        onOptOutEmail(value: boolean): void;
        onJobSelected(jobFunction: services.IJobFunction): void
    }

    class RegisterStep2Controller implements IStep2Scope {
        languages: Array<services.ILanguage>;
        jobFunctions: Array<services.IJobFunction>;
        memberships: Array<services.IMembership>;
        webPhoto: any;
        user: services.IUser;
        webOptions = {
            select: (e) => {
                this.onSaveFile(e, "webphoto");
            }
        };
        resumeOptions = {
            select: (e) => {
                this.onSaveFile(e, "resume");
            }
        };
        complianceOptions = {
            select: (e) => {
                this.onSaveFile(e, "compliance");
            }
        };
        historyOptions = {
            select: (e) => {
                this.onSaveFile(e, "professionalhistory");
            }
        };
        licenseOptions = {
            select: (e) => {
                this.onSaveFile(e, "medicallicense");
            }
        }
        isctItem: services.IUserMembership = null;
        asbmtItem: services.IUserMembership = null;

        static $inject = [
            '$window',
            '$uibModal',
            'accountService',
            'languageService',
            'jobFunctionService',
            'membershipService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private accountService: services.IAccountService,
            private languageService: services.ILanguageService,
            private jobFunctionService: services.IJobFunctionService,
            private membershipService: services.IMembershipService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            if (this.common.currentUser == null) {
                this.common.activateController([this.getUser(), this.getLanguages(), this.getJobFunctions(), this.getMemberships()], 'RegisterStep2Controller');
            } else {
                this.user = this.common.currentUser;
                this.common.activateController([this.getLanguages(), this.getJobFunctions(), this.getMemberships()], 'RegisterStep2Controller');
            }
          
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Registration Step 2",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Register Step 2', isActive: true }
                ]
            });
        }

        getUser(): ng.IPromise<void> {
            return this.accountService.getCurrentUser()
                .then((user: services.IUser) => {
                    this.common.currentUser = user;
                    this.user = user;
                })
                .catch(() => {
                    this.notificationFactory.error("Cannot get user. Please contact support");
                });
        }

        getLanguages(): ng.IPromise<void> {
            return this.jobFunctionService.getAll()
                .then((data: Array<services.IJobFunction>) => {
                    this.jobFunctions = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to get data. Please contact support.");
                });
        }

        getJobFunctions(): ng.IPromise<void> {
            return this.languageService.getAll()
                .then((data: Array<services.ILanguage>) => {
                    this.languages = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to get data. Please contact support.");
                });
        }

        getMemberships(): ng.IPromise<void> {
            return this.membershipService.getAll()
                .then((data: Array<services.IMembership>) => {
                    this.memberships = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to get data. Please contact support.");
                });
        }

        save(): void {
            this.common.showSplash();

            this.accountService.update(this.user)
                .then((data: app.services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.$window.location.href = '#/Overview';
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to register. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onOptOutEmail(value: boolean): void {
            this.user.emailOptOut = value;
        }
        onOptOutMail(value: boolean): void {
            this.user.mailOptOut = value;
        }
        onAgree(): void {
            this.user.agreedToPolicyDate = new Date();
        }

        onSaveFile(e, type: string): void {
            if (e.files.length > 0) {
                var file = e.files[0];

                this.common.showSplash();

                this.accountService.addFile(file.rawFile, file.extension, type)
                    .then((data: services.IGenericServiceResponse<string>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            switch (type) {
                                case "webphoto":
                                    this.user.webPhotoPath = data.item;
                                    break;
                                case "resume":
                                    this.user.resumePath = data.item;
                                    break;
                                case "compliance":
                                    this.user.statementOfCompliancePath = data.item;
                                    break;
                                case "professionalhistory":
                                    this.user.annualProfessionHistoryFormPath = data.item;
                                    break;
                                case "medicallicense":
                                    this.user.medicalLicensePath = data.item;
                                    break;
                            }
                            
                        }

                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Unable to save file. Please contact support.");
                        this.common.hideSplash();
                    });
            }
        }

        onJobSelected(jobFunction: services.IJobFunction): void {
            var item = _.remove(this.user.jobFunctions, (job: services.IJobFunction) => {
                return job.id === jobFunction.id;
            });

            if (item == null || item.length === 0) {
                this.user.jobFunctions.push(jobFunction);
            }
        }

        onLanguageSelected(language: services.ILanguage): void {
            var item = _.remove(this.user.languages, (lang: services.ILanguage) => {
                return lang.id === language.id;
            });

            if (item == null || item.length === 0) {
                this.user.languages.push(language);
            }
        }

        onMembershipSelected(membership: services.IMembership): void {
            var item = _.remove(this.user.memberships, (member: services.IUserMembership) => {
                return member.membership.id === membership.id;
            });

            if (item == null || item.length === 0) {
                this.user.memberships.push({
                    membership: membership,
                    membershipNumber: ""
                });
            }

            var isct = _.find(this.user.memberships, (member: services.IUserMembership) => {
                return member.membership.name === "ISCT" || member.membership.name === "ISCT-Europe";
            });

            if (isct) {
                if (this.isctItem === null) {
                    this.isctItem = isct;
                }
            } else {
                this.isctItem = null;
            }

            var asbmt = _.find(this.user.memberships, (member: services.IUserMembership) => {
                return member.membership.name === "ASBMT";
            });

            if (asbmt) {
                if (this.asbmtItem === null) {
                    this.asbmtItem = asbmt;
                }
            } else {
                this.asbmtItem = null;
            }
        }
    }

    angular
        .module('app.account')
        .controller('app.account.RegisterStep2Controller',
        RegisterStep2Controller);
} 