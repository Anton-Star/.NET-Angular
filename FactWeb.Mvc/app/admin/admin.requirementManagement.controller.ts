module app.admin {
    'use strict';

    interface IRequirementManagementScope {
        applicationTypes: Array<services.IApplicationType>;
        questionTypes: Array<services.IQuestionType>;
        selectedApplicationType: string;
        requirements: Array<services.IApplicationSection>;
        addSet(): void;
    }

    interface IVersionNames {
        id: string;
        isShown: boolean;
        versionNumber: string;
        isActive: boolean;
    }

    class RequirementManagementController implements IRequirementManagementScope {
        applicationTypes: Array<services.IApplicationType>;
        questionTypes = [];
        selectedApplicationType = "";
        requirements = [];
        scopeTypes: Array<services.IScopeType>;
        requirementSettingName: string;
        private versions: Array<services.IApplicationVersion>;
        showResults = false;
        currentVersion: services.IApplicationVersion;
        questions: Array<services.IQuestion>;

        versionNames: Array<IVersionNames> = [];

        static $inject = [
            '$rootScope',
            '$window',
            '$uibModal',
            'scopeTypeService',
            'applicationService',
            'requirementService',
            'questionTypeService',
            'cacheService',
            'versionService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            $rootScope: ng.IRootScopeService,
            private $window: ng.IWindowService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private scopeTypeService: services.IScopeTypeService,
            private applicationService: services.IApplicationService,
            private requirementService: services.IRequirementService,
            private questionTypeService: services.IQuestionTypeService,
            private cacheService: services.ICacheService,
            private versionService: services.IVersionService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            $rootScope.$on(this.config.events.requirementSaved, () => {
                this.onApplicationTypeChange();
            });

            this.cacheService.getApplicationSettings()
                .then((settings) => {
                    this.loadRequirementSetting(settings);
                });

            common.activateController([this.getApplicationTypes(), this.getQuestionTypes(), this.getScopeTypes(), this.getAppSettings()], 'RequirementManagementController');
        }

        getAppSettings(): ng.IPromise<void> {
            return this.cacheService.getApplicationSettings()
                .then((data) => {
                    var setting = _.find(data, (setting: services.IApplicationSetting) => {
                        return setting.name === "Requirement Management Set Name";
                    });

                    if (setting) {
                        this.requirementSettingName = setting.value;
                    }
                });
        }

        loadRequirementSetting(applicationSettings: Array<services.IApplicationSetting>): void {
            var setting = _.find(applicationSettings, (setting: services.IApplicationSetting) => {
                return setting.name === "Requirement Management Set Name";
            });

            if (setting) {
                this.requirementSettingName = setting.value;
            }
        }

        getScopeTypes(): ng.IPromise<void> {
            return this.scopeTypeService.getAllActiveNonArchivedAsync()
                .then((data: Array<services.IScopeType>) => {
                    this.scopeTypes = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error occurred. Please contact support");
                });
        }

        search(requirements: Array<services.IApplicationSection>, rowId): services.IApplicationSection {
            
            var req: services.IApplicationSection = null;
            for (var j = 0; j < requirements.length; j++) {
                if (requirements[j].id === rowId) {
                    req = requirements[j];
                    break;
                }

                if (requirements[j].children != null &&
                    requirements[j].children != undefined &&
                    requirements[j].children.length > 0) {
                    req = this.search(requirements[j].children, rowId);
                }

                if (req !== null) {
                    break;
                }
            }

            return req;
        }

        getApplicationTypes(): ng.IPromise<void> {
            return this.applicationService.getApplicationTypes()
                .then((data: Array<services.IApplicationType>) => {
                    this.applicationTypes = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting types. Please contact support.");
                });
        }

        getQuestionTypes(): ng.IPromise<void> {
            return this.questionTypeService.getAll()
                .then((data: Array<services.IQuestionType>) => {
                    this.questionTypes = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting types. Please contact support.");
                });
        }

        onApplicationTypeChange(): void {
            this.common.showSplash();

            this.requirementService.getRequirements(this.selectedApplicationType)
                .then((items: Array<services.IApplicationVersion>) => {
                    var data = [];
                    this.versionNames = [];

                    this.currentVersion = _.find(items, (v: services.IApplicationVersion) => {
                        return v.isActive;
                    });

                    if (this.currentVersion) {
                        this.currentVersion.isShown = true;
                        _.each(this.currentVersion.applicationSections, (section: services.IApplicationSection) => {
                            data.push(this.requirementService.processSection(section));
                        });
                    }

                    console.log(this.currentVersion);

                    _.each(items, (v) => {
                        this.versionNames.push({
                            id: v.id,
                            isShown: v.isShown,
                            versionNumber: v.versionNumber,
                            isActive: v.isActive
                        });
                    });
                    
                    this.questions = this.requirementService.questions;
                    this.requirements = data;
                    this.versions = items;
                    this.showResults = true;

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to get requirements. Please contact support.");
                    this.common.hideSplash();
                });
        }

        onChangeVersion(v: IVersionNames): void {
            _.each(this.versionNames, (v: services.IApplicationVersion) => {
                v.isShown = false;
            });

            var version = _.find(this.versions, (ver: services.IApplicationVersion) => {
                return ver.id === v.id;
            });

            this.currentVersion = version;

            v.isShown = true;
            var data = [];

            this.requirementService.questions = [];

            if (version.applicationSections && version.applicationSections.length > 0) {
                _.each(version.applicationSections, (section: services.IApplicationSection) => {
                    data.push(this.requirementService.processSection(section));
                });

                this.questions = this.requirementService.questions;
                this.requirements = data;
            } else {
                this.common.showSplash();
                this.requirementService.getRequirementsByVersion(this.selectedApplicationType, version.id)
                    .then((rec) => {
                        console.log(rec);
                        version.applicationSections = rec.applicationSections;
                        if (rec && rec.applicationSections) {
                            _.each(rec.applicationSections, (section: services.IApplicationSection) => {
                                data.push(this.requirementService.processSection(section));
                            }); 

                            this.questions = this.requirementService.questions;
                            this.requirements = data;
                        }

                        this.common.hideSplash();
                    })
                    .catch(e => {
                        this.notificationFactory.error("Error getting version: " + e);
                        this.common.hideSplash();
                    });
            }
             
            
        }

        onAddNewVersion(): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/version.html",
                controller: "app.modal.templates.VersionController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                resolve: {
                    applicationTypeName: () => {
                        return this.selectedApplicationType;
                    },
                    versions: () => {
                        return this.versions;
                    }
                }
            });

            instance.result.then((version: services.IApplicationVersion) => {
                if (version.isActive) {
                    _.each(this.versions, (v: services.IApplicationVersion) => {
                        v.isActive = false;
                    });
                }

                this.versions.push(version);

                var name = {
                    id: version.id,
                    isShown: true,
                    versionNumber: version.versionNumber,
                    isActive: version.isActive
                };

                this.versionNames.push(name);

                this.onChangeVersion(name);

            }, () => {
            });
        }

        onRebuildCache() {
            this.common.showSplash();

            this.versionService.buildCache(this.currentVersion.applicationType.applicationTypeId,
                    this.currentVersion.id)
                .then(() => {
                    this.notificationFactory.success('Cache Rebuilt successfully.');
                    this.common.hideSplash();
                    this.onApplicationTypeChange();
                })
                .catch(e => {
                    this.notificationFactory.error('Error Rebuilding cache: ' + e);
                    this.common.hideSplash();
                });
        }

        onExport() {
            this.common.showSplash();

            this.requirementService.export(this.currentVersion.id)
                .then((data) => {
                    console.log('data', data);

                    var record = "";

                    _.each(data, (d) => {
                        record += d.row + "\r\n";
                    });

                    var blob = new Blob([record], { type: "text/plain; charset=utf-8" });
                    saveAs(blob, this.currentVersion.title + ".csv");
                    this.common.hideSplash();
                });
        }

        makeActive(): void {
            if (confirm("Are you sure you want to make this version active? All preexisting applications will stay on the current version, however new applications will switch to this version.")) {
                this.common.showSplash();

                this.versionService.makeVersionActive(this.currentVersion)
                    .then((data: services.IServiceResponse) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                            this.common.hideSplash();
                        } else {
                            this.notificationFactory.success("Version Saved successfully.");
                            _.each(this.versions, (v: services.IApplicationVersion) => {
                                v.isActive = false;
                            });
                            this.currentVersion.isActive = true;

                            this.common.hideSplash();
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Unable to connect with web service. Please contact support.");
                        this.common.hideSplash();
                    });
            }
        }

        addSet(): void {            
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/requirement.html",
                controller: "app.modal.templates.RequirementController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                keyboard: false,
                resolve: {
                    requirement: () => {
                        return {
                            applicationTypeName: this.selectedApplicationType
                        };
                    },
                    isNew: () => {
                        return true;
                    },
                    parent: () => {
                        return undefined;
                    },
                    isRequirementSet: () => {
                        return true;
                    },
                    scopeType: () => {
                        return this.scopeTypes;
                    },
                    currentVersion: () => {
                        return this.currentVersion;
                    }
                }
            });

            instance.result.then(() => {
                this.common.$broadcast(this.config.events.requirementSaved);
            }, () => {
            });
        }

        onRemoveVersion(): void {
            if (!confirm("Are you sure you want to delete this version?")) {
                 return;
            }

            if (this.currentVersion.isActive) {
                this.notificationFactory.error("Active version cannot be deleted");
            }

            this.common.showSplash();

            this.versionService.remove(this.currentVersion.id)
                .then(() => {
                    for (var i = 0; i < this.versionNames.length; i++) {
                        if (this.versionNames[i].id === this.currentVersion.id) {

                            if (i === 0) {
                                this.onChangeVersion(this.versionNames[i + 1]);
                            } else {
                                this.onChangeVersion(this.versionNames[i - 1]);
                            }

                            this.versionNames.splice(i, 1);

                            this.versions.splice(i, 1);
                            
                            break;
                        }
                    }

                    this.notificationFactory.success("Version removed successfully.");

                    this.common.hideSplash();
                })
                .catch((e) => {
                    this.notificationFactory.error("An error occurred." + e);
                    this.common.hideSplash();
                });
        }
        
    }

    angular
        .module('app.admin')
        .controller('app.admin.RequirementManagementController',
        RequirementManagementController);
} 