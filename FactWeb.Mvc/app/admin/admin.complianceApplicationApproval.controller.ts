module app.admin {
    'use strict';

    interface IComplianceApplicationApprovalScope {
        complianceApplicationId: string;
        comment: string;        
        updateStatus: (isApproved: boolean) => void;
        complianceApplication: app.services.IComplianceApplication;
        approvalStatus: app.services.IComplianceApplicationApprovalStatus;
    }

    class ComplianceApplicationApproval implements IComplianceApplicationApprovalScope {
        complianceApplicationId: string;
        comment: string;        
        complianceApplication: app.services.IComplianceApplication;
        approvalStatus: app.services.IComplianceApplicationApprovalStatus;
        isCoordinator: boolean = false;
        serialNumber: string;
        appId = "";

        static $inject = [
            '$rootScope',
            '$window',
            '$location',
            'applicationService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(   
            private $rootScope: ng.IRootScopeService,         
            private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            private applicationService: app.services.IApplicationService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {        
                
            this.complianceApplicationId = $location.search().complianceApplicationId;
            this.serialNumber = $location.search().sn;

            if (!this.common.currentUser) {
                this.$rootScope.$on(this.config.events.userLoggedIn,
                    (data: any, args: any) => {
                        this.isCoordinator = this.common.currentUser.role
                            .roleName ===
                            this.config.roles.factCoordinator;
                    });
            } else {
                this.isCoordinator = this.common.currentUser.role
                    .roleName ===
                    this.config.roles.factCoordinator;
            }

            this.common.activateController([this.getComplianceApplicationById()], '');
          
        }

        getComplianceApplicationById(): ng.IPromise<void> {            
            return this.applicationService.getComplianceApplicationById(this.complianceApplicationId, false)
                .then((data: services.IComplianceApplication) => {
             
                    this.complianceApplication = data;

                    this.appId = this.complianceApplication.applications.length > 0 ? this.complianceApplication.applications[0].uniqueId : "";
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting data");                    
                });
        }
        
        updateStatus(isApproved: boolean): void {            
            this.common.showSplash();

            this.complianceApplication.id = this.complianceApplicationId;
            this.approvalStatus = { id:"", name:"" };            
            this.approvalStatus.name = isApproved ? "Approved / Active" : "Reject";
            this.complianceApplication.approvalStatus = this.approvalStatus;
            
            //this.applicationService.setComplianceApplicationApprovalStatus(this.complianceApplication, this.serialNumber, "")
            //    .then((data: app.services.IServiceResponse) => {
            //        if (data.hasError) {
            //            this.notificationFactory.error(data.message);
            //        } else {
            //            this.notificationFactory.success("Status updated successfully");
            //        }
            //        this.common.hideSplash();
            //    })
            //    .catch(() => {
            //        this.notificationFactory.error("Error.");
            //        this.common.hideSplash();
            //    });
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.ComplianceApplicationApproval',
        ComplianceApplicationApproval);
} 