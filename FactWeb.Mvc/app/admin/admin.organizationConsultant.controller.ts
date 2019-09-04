module app.admin {
    'use strict';

    interface IOrganizationConsultantScope {
        save(): void;
        delete: (organizationFacilityId) => void;  
        edit: (rowData) => void;      
        selectedOrganization: number;
        selectedConsultant: number;        
        saveMode: boolean;
    }

    class OrganizationConsultantController implements IOrganizationConsultantScope {

        organizations: Array<services.ISimpleOrganization>;        
        consultants: Array<services.IUser>;
        organizationConsultants: Array<services.IOrganizationConsultant>;
        organizationConsultantId: number;
        selectedOrganization: number;
        selectedConsultant: number;        
        results: Array<services.IOrganizationConsultant>;
        saveMode: boolean;
        startDate: string;
        endDate: string; 
        gridOptions = {
            sortable: true,
            filterable: true,
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
            'organizationService',
            'organizationConsultantService',
            'userService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private organizationService: services.IOrganizationService,
            private organizationConsultantService: app.services.IOrganizationConsultantService,
            private userService: app.services.IUserService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.saveMode = false;
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Organization Consultant",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Organization Consultants', isActive: true }
                ]
            });
            var defaultDate = new Date();
            this.startDate = (defaultDate.getMonth()+1) + '/' + defaultDate.getDate() + '/' + defaultDate.getFullYear();
            defaultDate.setDate(defaultDate.getDate() + 365);
            this.endDate = (defaultDate.getMonth() + 1) + '/' + defaultDate.getDate() + '/' + defaultDate.getFullYear();
            
            this.getConsultant();
            common.activateController([this.getConsultant(), this.getOrganizationConsultants(), this.getOrganization()], 'organizationConsultantController');
        }

        getOrganizationConsultants(): ng.IPromise<void> {        
            return this.organizationConsultantService.getOrganizationConsultants()
                .then((data: Array<services.IOrganizationConsultant>) => {
                    console.log('org consultants', data);                                 
                    if (data == null) {
                        this.notificationFactory.error('no record');
                    } else {                            
                        this.results = data;
                        this.gridOptions.dataSource.data(data);
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getOrganization(): ng.IPromise<void> {
            return this.organizationService.getSimpleOrganizations()
                .then((data: Array<services.ISimpleOrganization>) => {
                    console.log('orgs', data);
                    if (data == null) {
                        this.notificationFactory.error('no organizations');
                    } else {
                        this.organizations = data;                        
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getConsultant(): ng.IPromise<void> {
            return this.userService.getConsultants()
                .then((data: Array<services.IUser>) => {
                    console.log('consultants', data);                
                    if (data == null) {
                        this.notificationFactory.error('no users');
                    } else {
                        this.consultants = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }
        

        save(): void {
            this.common.showSplash();

            this.organizationConsultantService.save(this.organizationConsultantId, this.selectedOrganization, this.selectedConsultant, this.startDate, this.endDate)
                .then((data: app.services.IGenericServiceResponse<boolean>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item == true) {
                            this.getOrganizationConsultants();
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

        delete(organizationConsultantId): void {
            var confirmation = confirm("Are you sure you want to delete this association ?");
            if (confirmation) {
                this.common.showSplash();
                this.organizationConsultantService.delete(organizationConsultantId)
                    .then((data: app.services.IGenericServiceResponse<boolean>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (data.item == true) {
                                this.getOrganizationConsultants();
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

        edit(rowData): void {            
            this.selectedOrganization = rowData.organizationId.toString();
            this.selectedConsultant = rowData.consultantId.toString();
            this.organizationConsultantId = rowData.organizationConsultantId;
            this.startDate = rowData.startDate;
            this.endDate = rowData.endDate;
          
            this.saveMode = true;
        }
        
        cancel(): void {
            this.clearForm();        
            this.saveMode = false;      
        }

        clearForm(): void {
            this.selectedOrganization = null;
            this.selectedConsultant = null;
            this.startDate = "";
            this.endDate = "";
        }

        startDateChange(): void {                            
            var startDate = new Date(this.startDate);
            startDate.setDate(startDate.getDate() + 365);
            this.endDate = startDate.getMonth()+1 + '/' + startDate.getDate() + '/' + startDate.getFullYear();            
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.OrganizationConsultantController',
        OrganizationConsultantController);
} 