module app.admin {
    'use strict';

    interface IRelationType {
        id: string;
        name: string;
        title: string;        
    }

    interface IOrganizationFacilityScope {
        saveRelation(): void;        
        deleteRelation: (organizationFacilityId) => void;
        editRelation: (rowData) => void;
        relationTypes: Array<IRelationType>;
        selectedRelationType: IRelationType;
        selectedOrganization: number;
        saveMode: boolean;
        selectedFacility: number;
        onRelationSelected: (relationType: IRelationType) => void;
    }

    class OrganizationFacilityController implements IOrganizationFacilityScope {
        organizations: Array<services.IOrganization>;
        facilities: Array<services.IFacility>;
        organizationFacilityId: number;
        selectedOrganization: number;
        selectedFacility: number;
        relationTypes = [
            { id:"rdbStrong", name: "strong", title: "Strong" },
            { id: "rdbWeak", name: "weak", title: "Weak"}];
        selectedRelationType: IRelationType;
        results: Array<services.IOrganizationFacility>;
        saveMode: boolean;
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
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$window',
            'organizationFacilityService',
            'organizationService',
            'facilityService',
            'notificationFactory',
            'common',
            'config'
        ];

        constructor(
            private $window: ng.IWindowService,
            private organizationFacilityService: app.services.IOrganizationFacilityService,
            private organizationService: services.IOrganizationService,
            private facilityService: services.IFacilityService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.saveMode = false;
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Organization Facility Relationship",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Organization Facility Relationship', isActive: true }
                ]
            });

            common.activateController([this.getOrganizations(), this.getFacilities(), this.getOrganizationFacility()], 'organizationFacilityController');            
        }

        getOrganizations(): ng.IPromise<void> {
            return this.organizationService.getFlatOrganizations()
                .then((data: Array<app.services.IOrganization>) => {
                    console.log('orgs', data);
                    if (data == null) {
                        this.notificationFactory.error('No Organizations found');
                    } else {
                        this.organizations = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organizations.");
                });
        }

        getFacilities(): ng.IPromise<void> {
            return this.facilityService.getAllFlat()
                .then((data: Array<app.services.IFacility>) => {
                    console.log('fac', data);
                    if (data == null) {
                        this.notificationFactory.error('No Facilities found');
                    } else {
                        this.facilities = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting facilities.");
                });
        }

        getOrganizationFacility(): ng.IPromise<void> {
            return this.organizationFacilityService.getOrganizationFacility()
                .then((data: Array<services.IOrganizationFacility>) => {
                    console.log('orgfac', data);
                    this.results = data;
                    this.gridOptions.dataSource.data(data);
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organization facilities.");
                });
        }

        saveRelation(): void {
            this.common.showSplash();
            var relation = true;
            
            switch (this.selectedRelationType.name) {
                case "strong":
                    relation = true;
                    break;
                case "weak":
                    relation = false;
            }
            
            this.organizationFacilityService.saveRelation(this.organizationFacilityId, this.selectedOrganization, this.selectedFacility, relation)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {                        
                        if (data.item == true) {
                            this.getOrganizationFacility();
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

        deleteRelation(organizationFacilityId): void {
            var confirmation = confirm("Are you sure you want to delete this relation ?");
            if (confirmation) {
                this.common.showSplash();
                this.organizationFacilityService.deleteRelation(organizationFacilityId)
                    .then((data: app.services.IGenericServiceResponse<boolean>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {

                            if (data.item == true) {
                                this.getOrganizationFacility();
                                this.notificationFactory.success(data.message);
                            } else {
                                this.notificationFactory.error("Error.");
                            }
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                    });
            }
        }

        editRelation(rowData): void {
            this.selectedOrganization = rowData.organizationId.toString();
            this.selectedFacility = rowData.facilityId.toString();
            this.organizationFacilityId = rowData.organizationFacilityId;
            
            if (rowData.relation == "Strong") {
                this.selectedRelationType = this.relationTypes[0];
                $("#rdbStrong").parent('label').addClass("active");
                $("#rdbWeak").parent('label').removeClass("active");
            } else {
                this.selectedRelationType = this.relationTypes[1];
                $("#rdbWeak").parent('label').addClass("active");
                $("#rdbStrong").parent('label').removeClass("active");
            } 
            this.saveMode = true;
        }

        cancel(): void {
            this.clearForm();
            this.saveMode = false;    
        }

        clearForm(): void {
            this.selectedOrganization = 0;
            this.selectedFacility = 0;
            this.organizationFacilityId = 0;
            this.selectedRelationType = null;
            $("#rdbWeak").parent('label').removeClass("active");
            $("#rdbStrong").parent('label').removeClass("active");
        }

        onRelationSelected(relationType: IRelationType): void {
            this.selectedRelationType = relationType;
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.OrganizationFacilityController',
        OrganizationFacilityController);
} 