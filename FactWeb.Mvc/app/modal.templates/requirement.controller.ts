module app.modal.templates {
    'use strict';

    interface IRequirementScope {
        save: () => void;
        cancel: () => void;
    }

    class RequirementController implements IRequirementScope {
        displayRequirement: services.IApplicationHierarchyData;
        isBusy = false;
        scopeTypes: Array<services.IScopeType>;
        requirementSettingName: string;

        static $inject = [
            'organizationService',
            'cacheService',
            'notificationFactory',
            'common',
            'config',
            '$uibModalInstance',
            'requirementService',
            'scopeTypeService',
            'requirement',
            'isNew',
            'parent',
            'isRequirementSet',
            'scopeType',
            'currentVersion'
        ];

        constructor(
            private organizationService: services.IOrganizationService,
            private cacheService: services.ICacheService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private requirementService: services.IRequirementService,
            private scopeTypeService : services.IScopeTypeService,
            private requirement: services.IApplicationHierarchyData,
            private isNew: boolean,
            private parent: services.IApplicationHierarchyData,
            private isRequirementSet: boolean,
            private scopeType: Array<services.IScopeType>,
            private currentVersion: services.IApplicationVersion) {            
            this.scopeTypes = this.scopeType;

            this.getSetting();
            
            if (this.isNew) { // new requirement
                if (this.isRequirementSet) { // new requirement set
                               
                    _.forEach(this.scopeTypes, (scopeType: services.IScopeType) => {
                            scopeType.isSelected = false;                        
                    });

                    this.displayRequirement = {
                        parentId: null,
                        isVariance: false,
                        order: null,
                        applicationTypeName: requirement.applicationTypeName,
                        uniqueIdentifier: "",
                        scopeTypes: this.scopeType,
                        versionId: this.currentVersion.id
                    };

                   

                } else { // new requiremenbt
                    
                    var uniqueIdentifier = "";
                    var order = requirement.children ? requirement.children.length + 1 : 1;
                    //second level
                    if (parent == undefined) { // no parent exist
                        uniqueIdentifier = requirement.uniqueIdentifier + order;

                        _.forEach(this.scopeType, (scopeType: services.IScopeType) => { // show current requirement's scope types
                            var found = _.find(this.requirement.scopeTypes, (type: services.IScopeType) => {
                                return scopeType.scopeTypeId === type.scopeTypeId;
                            });

                            if (found) {
                                scopeType.isSelected = true;
                            } else {
                                scopeType.isSelected = false;
                            }
                        });
                        this.scopeTypes = this.scopeType;

                    } else { // show parent scope types
                        uniqueIdentifier = requirement.uniqueIdentifier + "." + order;
                        this.scopeType = this.parent.scopeTypes;
                    }
                    this.displayRequirement = {
                        parentId: requirement.id,
                        isVariance: false,
                        order: order.toString(),
                        applicationTypeName: requirement.applicationTypeName,
                        uniqueIdentifier: uniqueIdentifier,
                        version: requirement.version,
                        scopeTypes: this.scopeType,
                        versionId: this.currentVersion.id
                    };
                }
                
            } else { //show saved scopeType types
                
                this.displayRequirement = angular.copy(this.requirement);
                if (parent != undefined) {                
                    this.displayRequirement.parentId = this.parent.id;
                }
                _.forEach(this.scopeType, (scopeType: services.IScopeType) => {
                    var found = _.find(this.displayRequirement.scopeTypes, (type: services.IScopeType) => {
                        return scopeType.scopeTypeId === type.scopeTypeId;
                    });

                    if (found) {
                        scopeType.isSelected = true;
                    } else {
                        scopeType.isSelected = false;
                    }
                });
                this.scopeTypes = this.scopeType;
            }
            
        }

        getSetting(): ng.IPromise<void> {
            return this.cacheService.getApplicationSettings()
                .then((data) => {
                    var setting = _.find(data, (setting: services.IApplicationSetting) => {
                        return setting.name === "Requirement Management Set Name";
                    });

                    if (setting) {
                        this.requirementSettingName = setting.value;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data. Please contact support.");
                });
        }

        isInvalid(): boolean {
            return this.displayRequirement.uniqueIdentifier === "" || this.displayRequirement.uniqueIdentifier === undefined ||
                this.displayRequirement.name === "" || this.displayRequirement.name === undefined ||
                this.displayRequirement.version === "" || this.displayRequirement.version === undefined;
        }

        onOrderChange(): void {
            if (this.parent == undefined) return;

            var index = this.displayRequirement.uniqueIdentifier.lastIndexOf(".");
            var id = "";
            if (index === -1) {
                var digit = this.displayRequirement.uniqueIdentifier.match(/\d/);
                if (digit != null && digit.length > 0) {
                    index = this.displayRequirement.uniqueIdentifier.indexOf(digit[0]);
                } else {
                    index = 0;
                }
            }

            id = this.displayRequirement.uniqueIdentifier.substr(0, index + 1);
            this.displayRequirement.uniqueIdentifier = id + this.displayRequirement.order;
        }

        save(): void {
            this.isBusy = true;
            this.displayRequirement.scopeTypes = this.scopeTypes;
            this.requirementService.save(this.displayRequirement)
                .then((data: services.IGenericServiceResponse<services.IApplicationSection>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                        this.isBusy = false;
                    } else {
                        this.notificationFactory.success("Requirement Saved successfully.");
                        this.$uibModalInstance.close({ section: data.item, saved: false });
                        this.isBusy = false;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Unable to connect with web service. Please contact support.");
                    this.isBusy = false;
                });
        }

        cancel(): void {            
            this.$uibModalInstance.close({ section: null, saved: false });         
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.RequirementController',
        RequirementController);
} 