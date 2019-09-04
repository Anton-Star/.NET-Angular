module app.modal.templates {
    'use strict';

    interface IInspectionScheduleDetail {
        selectedUser: string;
        selectedRole: string;
        selectedCategory: string;
        isLead: boolean;
        isMentor: boolean;
        inspectionScheduleDetailId: string;        
        startDate: string;
        endDate: string;        
        selectedSite: string;
        selectedSiteId: string;
        selectedSiteList :string;
        divNewStaffShow: boolean;
        selectedUserEnable: boolean;
        get: (isReload: boolean) => void;
        saveChanges: () => void;
        saveSchedule: () => void;
        cancelSchedule: () => void;
        cancelStaff: () => void;
        editStaff: (inspectionScheduleDetailId, userId, roleId, inspectionCategoryId, isLead, isMentor, siteId) => void;
        deleteStaff: (inspectionScheduleDetailId) => void;
        getRelationClass: (site: services.IFacilitySite) => string;        
    }

    class InspectionScheduleDetailController implements IInspectionScheduleDetail {
        selectedUser: string;
        selectedRole: string;
        selectedCategory: string;
        isLead: boolean;
        isMentor: boolean;
        inspectionScheduleDetailId: string;        
        startDate: string;
        endDate: string;        
        selectedSiteList: string;
        selectedSiteId: string;
        selectedSite: string;
        divNewStaffShow: boolean;
        selectedUserEnable:boolean                
        gridSites: Array<services.IFacilitySite>;
        dropdownSites: Array<services.IFacilitySite>;
        allSites: services.IFacilitySite[];
        users: Array<services.IUser>;
        roles: Array<services.IAccreditationRole>;
        inspectionCategories: Array<services.IInspectionCategory>;
        newInspectionDate: string;
        selectedNewSite: string;
        defaultDate: Date;
        defaultDateString: string;
        orgDescription = "";
        orgSpatialRelationship = "";
        archiveExist = false;

        siteGridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: false,
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 100
            }),
            pageable: {
                pageSize: 10
            },
            columns: [
                { field: "siteName", title: "Site" },
                { field: "relation", title: "Relation" }
            ]
        };

        results: Array<services.IInspectionScheduleDetail>;
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
            'inspectionScheduleService',
            'userService',
            'applicationService',
            'organizationFacilityService',
            'notificationFactory',
            'common',
            '$uibModalInstance',
            'inspectionScheduleId',
            'organizationId',
            'applicationId',
            'startDateSaved',
            'endDateSaved',
            'organization',
            'isReinspect',
            'config'
        ];

        constructor(
            private inspectionScheduleService: services.IInspectionScheduleService,
            private userService: services.IUserService,
            private applicationService: services.IApplicationService,
            private organizationFacilityService: services.IOrganizationFacilityService,            
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private inspectionScheduleId: string,
            private organizationId: string,
            private applicationId: string,            
            private startDateSaved: string,
            private endDateSaved: string,     
            private organization: services.IOrganization,    
            private isReinspect: boolean,   
            private config: IConfig) {
            this.isLead = false;
            this.isMentor = false;
            this.divNewStaffShow = true;            
            this.startDate = this.startDateSaved;
            this.endDate = this.endDateSaved;
            this.selectedUserEnable = true;

            
            this.defaultDate = new Date();
            this.defaultDate.setDate(this.defaultDate.getDate() + 30);
            this.defaultDateString = (this.defaultDate.getMonth() + 1) + '/' + this.defaultDate.getDate() + '/' + this.defaultDate.getFullYear();

            if (this.inspectionScheduleId == "0")
            {                
                this.startDate = this.defaultDateString;
                this.endDate = this.defaultDateString;                
            }
            this.newInspectionDate = this.defaultDateString;

            this.orgDescription = organization.description;
            this.orgSpatialRelationship = organization.spatialRelationship;

            common.activateController([this.getAccreditationRoles(), this.getInspectionCategories(), this.get(false)], 'inspectionScheduleDetailController');                          

        }

        get(isReload: boolean): ng.IPromise<void> {
            return this.inspectionScheduleService.getInspectionScheduleDetail(this.organizationId, this.applicationId, this.inspectionScheduleId)
                .then((data: app.services.IGenericServiceResponse<app.services.IInspectionScheduleDetailPage>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        console.log(data);
                        if (data.item != null) {
                            this.getDropdownSites(this.organizationId);
                            this.results = data.item.inspectionScheduleDetailItems;
                            this.gridOptions.dataSource.data(data.item.inspectionScheduleDetailItems);
                            this.inspectionScheduleDetailId = "0";                            
                            this.archiveExist = data.item.archiveExist;

                            _.each(data.item.facilitySites, (s) => {
                                if (this.inspectionScheduleId === "0" || s.inspectionDate != null) {
                                    s.isOverridden = false;
                                } else {
                                    s.isOverridden = true;
                                }
                            });

                            this.gridSites = data.item.facilitySites;
                            
                            if (this.archiveExist == true)
                                this.inspectionScheduleId = data.item.inspectionScheduleId.toString();
                        }
                    }                    
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }

        getAllUsers(){
            this.common.showSplash();            
            this.userService.getUsersNearSite(this.selectedSiteId)
                .then((data: Array<app.services.IUser>) => {                    
                    if (data == null) {
                        this.notificationFactory.error("No user records");
                    } else {
                        this.users = data;
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }

        getAllSites(organizationId: string, inspectionScheduleId: string): ng.IPromise<void> {
            return this.inspectionScheduleService.getSites(organizationId, inspectionScheduleId)
                .then((data: Array<app.services.IFacilitySite>) => {
                    if (data == null || data.length == 0) return;

                    console.log('sites', data);

                    this.allSites = data;

                    this.siteGridOptions.dataSource.data(data);

                    if (inspectionScheduleId != "0") // update case
                    {
                        //if (this.archiveExist == false) {
                        //    //this.gridSites = data;

                        //    for (var i = 0; i < data.length; i++) {
                        //        var facId = data[i].facilityId;
                        //        var facObject = $.grep(this.dropdownSites, function (fromFac) {
                        //            return fromFac.facilityId == facId;
                        //        })[0];

                        //        var idx = this.dropdownSites.indexOf(facObject);
                        //        if (idx != -1) {
                        //            this.dropdownSites.splice(idx, 1);

                        //        }
                        //    }
                        //}
                        //else {
                        //    this.gridSites = data;
                        //    for (i = 0; i < this.gridSites.length; i++) {
                        //        this.gridSites[i].inspectionDate = this.defaultDateString;
                        //    }

                        //    for (var i = 0; i < data.length; i++) {
                        //        var facId = data[i].facilityId;
                        //        var facObject = $.grep(this.dropdownSites, function (fromFac) {
                        //            return fromFac.facilityId == facId;
                        //        })[0];

                        //        var idx = this.dropdownSites.indexOf(facObject);
                        //        if (idx != -1) {
                        //            this.dropdownSites.splice(idx, 1);

                        //        }
                        //    }
                        //}
                    }
                    else // new case
                    {
                        //if (this.isReinspect) {
                        //    //All sites are weak
                        //} else if (this.archiveExist == false) {
                        //    this.gridSites = data.filter(function (site) {
                        //        return site.relation == "Strong";
                        //    });

                        //    for (i = 0; i < this.gridSites.length; i++) {
                        //        this.gridSites[i].inspectionDate = this.defaultDateString;
                        //    }

                        //    this.dropdownSites = this.dropdownSites.filter(function (site) {
                        //        return site.relation == "Weak";
                        //    });
                        //}
                        //else {
                        //    this.gridSites = data;
                        //    for (i = 0; i < this.gridSites.length; i++) {
                        //        this.gridSites[i].inspectionDate = this.defaultDateString;
                        //    }

                        //    for (var i = 0; i < data.length; i++) {
                        //        var facId = data[i].facilityId;
                        //        var facObject = $.grep(this.dropdownSites, function (fromFac) {
                        //            return fromFac.facilityId == facId;
                        //        })[0];

                        //        var idx = this.dropdownSites.indexOf(facObject);
                        //        if (idx != -1) {
                        //            this.dropdownSites.splice(idx, 1);

                        //        }
                        //    }

                        //}
                    }                   
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        showRemove(site: services.IFacilitySite): boolean {            
            if (this.archiveExist == true) {
                return true;
            }
            else 
            {
                return site.relation == 'Weak'
            } 
        }

        getAccreditationRoles(): ng.IPromise<void> {

            return this.userService.getAccreditationRoles()
                .then((data: Array<app.services.IAccreditationRole>) => {
                    if (data == null) {
                        this.notificationFactory.error("No user records");
                    } else {
                        this.roles = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getInspectionCategories(): ng.IPromise<void> {
            return this.inspectionScheduleService.getAllInspectionCategories()
                .then((data: Array<app.services.IInspectionCategory>) => {
                    if (data == null) {
                        this.notificationFactory.error("No inspection category records");
                    } else {
                        this.inspectionCategories = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }        
       
        getDropdownSites(organizationId: string): ng.IPromise<void> {
            return this.organizationFacilityService.getSitesByOrganization(parseInt(organizationId))
                .then((data: Array<app.services.IFacilitySite>) => {                   
                    if (data == null) {
                        this.notificationFactory.error("no sites");
                    } else {                        
                        this.dropdownSites = data;
                        this.getAllSites(this.organizationId, this.inspectionScheduleId);
                    }  
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }
        
        
        saveChanges(): void {
            //this.common.showSplash();
            var inspectionScheduleDetailId = this.inspectionScheduleDetailId;
            var selectedUserId = this.selectedUser;
            var selectedRoleId = this.selectedRole;
            var selectedCategoryId = this.selectedCategory;
            var lead = this.isLead;
            var mentor = this.isMentor;            
            var startDate = this.startDate;
            var endDate = this.endDate;

            var sites = [];

            var inspectHasDate = true;
            _.each(this.gridSites, (s) => {
                if (!s.isOverridden) {
                    sites.push(s);

                    if (!s.inspectionDate || s.inspectionDate === "") {
                        inspectHasDate = false;
                    }
                }
            });

            if (!inspectHasDate) {
                this.notificationFactory.error("You must have an inspection date for sites that are inspected.");
                return;
            }

            if (sites.length === 0) {
                this.notificationFactory.error("You must inspect atleast 1 site.");
                return;
            }

            this.inspectionScheduleService.saveInspectionSchedule(inspectionScheduleDetailId, this.inspectionScheduleId, this.organizationId, this.applicationId, selectedUserId, selectedRoleId, selectedCategoryId, lead, mentor, startDate, endDate, sites, this.selectedSiteId)
                .then((data: app.services.IGenericServiceResponse<number>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item != null) {                            
                            this.inspectionScheduleId = data.item.toString();
                            this.get(true);
                            this.clearControls();
                        }
                        else {
                            this.notificationFactory.error("Error.");
                        }
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });

            this.divNewStaffShow = true;
        }

        cancelStaff(): void {
            this.divNewStaffShow = true;
            this.clearControls();
        }

        clearControls(): void {
            this.selectedUser = null;
            this.selectedRole = null;
            this.selectedCategory = null;
            this.selectedSiteId = null
            this.isLead = false;
            this.isMentor = false;
            this.inspectionScheduleDetailId = "0";
        }

        editStaff(inspectionScheduleDetailId, userId, roleId, inspectionCategoryId, isLead, isMentor, siteId): void {                
            this.divNewStaffShow = false;
            this.inspectionScheduleDetailId = inspectionScheduleDetailId;
            this.selectedUser = userId;
            this.selectedRole = roleId.toString();
            this.selectedCategory = inspectionCategoryId.toString();
            this.isLead = isLead;
            this.isMentor = isMentor;
            this.selectedSiteId = siteId.toString();
            this.getAllUsers();
            this.selectedUserEnable = false;            
        }

        deleteStaff(inspectionScheduleDetailId): void {
            var confirmation = confirm("Are you sure you want to delete this staff ?");
            if (confirmation) {
                this.common.showSplash();
                this.inspectionScheduleService.deleteStaff(inspectionScheduleDetailId)
                    .then((data: app.services.IGenericServiceResponse<boolean>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (data.item == true) {
                                this.get(true);
                                this.common.hideSplash();
                            }
                            else {
                                this.notificationFactory.error("Error.");
                            }
                        }

                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                        this.common.hideSplash();
                    });
            }
        }

        saveSchedule(): void {                                    
            var startDate = this.startDate;
            var endDate = this.endDate;  

            var sites = [];

            _.each(this.gridSites, (s) => {
                if (!s.isOverridden) {
                    sites.push(s);
                }
            });

            if (sites.length === 0) {
                this.notificationFactory.error("You must inspect atleast 1 site.");
                return;
            }
            
            if (this.validateForm()) {
                this.inspectionScheduleService.saveInspectionSchedule("", this.inspectionScheduleId, this.organizationId, this.applicationId, "", "", "", this.isLead, this.isMentor, startDate, endDate, sites, this.selectedSiteId)
                    .then((data: app.services.IGenericServiceResponse<number>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (data.item != null) {
                                this.inspectionScheduleId = data.item.toString();                                
                                this.$uibModalInstance.close();
                            } else {
                                this.notificationFactory.error("Error.");
                            }
                        }                       
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");                       
                    });
            } 
        }

        validateForm(): boolean {
            
            if (this.startDate == null) {
                this.notificationFactory.error("Enter valid Start date.");
                return false;
            }
            if (this.endDate == null) {
                this.notificationFactory.error("Enter valid End date.");
                return false;
            }

            var startDate = new Date(this.startDate);
            var endDate = new Date(this.endDate); 
            var timeDiff = Math.abs(endDate.getTime() - startDate.getTime());
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)); 
            
            if (startDate <= new Date()) {
                this.notificationFactory.error("Start date should be a future date.");
                return false;
            }

            if (endDate <= new Date()) {
                this.notificationFactory.error("End date should be a future date.");
                return false;
            }

            if (diffDays > 365) {
                this.notificationFactory.error("Start and End date should be between one year.");
                return false;
            }

            if (this.selectedSiteList == "") {
                this.notificationFactory.error("Select at least on facility");
                return false;
            }

            var result = $.grep(this.results, function (e) { return e.roleName === "Inspector" && e.isLead == true; });
            if (result.length < 1) {
                this.notificationFactory.error("There should be one inspector as the lead inspector.");
                return false;
            }

            var result1 = $.grep(this.results, function (e) { return e.roleName === "Inspector Trainee"; });
            if (result1.length > 0) {
                var result2 = $.grep(this.results, function (e) { return e.roleName === "Inspector" && e.isMentor == true;});
                if (result2.length < 1) {
                    this.notificationFactory.error("One of the inspectors assigned to the application must be marked as the mentor for the trainee.");
                    return false;
                }
            }
            return true;
        }

        cancelSchedule(): void {
            this.$uibModalInstance.dismiss('cancel');
        }

        getRelationClass(site: services.IFacilitySite): string {
            return (site.relation == "Strong") ? "strong" : "";
        }

        addSite() {            
            if (this.newInspectionDate == null)
            {
                this.notificationFactory.error("Enter valid Inspection date.");
                return;
            }         
            var siteId = this.selectedNewSite;
            var date = this.newInspectionDate;
                        
            var facObject = $.grep(this.dropdownSites, function (fromFac) {
                return fromFac.siteId.toString() == siteId;
                //return fromFac.facilityId.toString() == facId;
            })[0];

            var idx = this.dropdownSites.indexOf(facObject);
            if (idx != -1) {
                this.dropdownSites.splice(idx, 1);
                facObject.inspectionDate = date;
                this.gridSites.push(facObject);
            }

            this.newInspectionDate = this.defaultDateString;
            this.selectedNewSite = null;            
        }

        removeSite(siteObject:services.IFacilitySite)
        {            
            var idx = this.gridSites.indexOf(siteObject);
            if (idx != -1) {
                this.gridSites.splice(idx, 1);
                this.dropdownSites.push(siteObject);
            }
        }

        addNewStaff() {
            this.divNewStaffShow = false;
        }

        selectedSiteChange() {            
            if (this.selectedSiteId != "") {
                this.getAllUsers();
                this.selectedUserEnable = false;
            }
            else {
                this.selectedUserEnable = true;
            }
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.InspectionScheduleDetailController',
        InspectionScheduleDetailController);
} 