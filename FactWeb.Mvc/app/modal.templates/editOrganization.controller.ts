module app.modal.templates {
    'use strict';

    interface IEditOrganization {
        organizationDirector: Array<services.IUser>;
        organizationUsers: Array<services.IUser>;
        primarySite: services.ISite;
        accreditationStatus: Array<services.IAccreditationStatus>;
        accreditedServices: string;
        baaOwner: Array<services.IBAAOwner>;
        baaExecutionDate: string;
        baaVerifiedDate: string;
        accreditationDate: string;
        accreditationExtensionDate: string;
        accreditedSince: string;
        viewFacility: (facility: services.IFacility) => void;
        save: (closeForm: boolean) => void;
        cancel: () => void;
    }

    class EditOrganizationController implements IEditOrganization {
        organizationDirector: Array<services.IUser>;
        organizationUsers: Array<services.IUser>;
        primarySite: services.ISite;
        accreditationStatus: Array<services.IAccreditationStatus>;
        accreditedServices: string;
        baaOwner: Array<services.IBAAOwner>;
        baaExecutionDate = "";
        baaVerifiedDate = "";
        accreditationDate = "";
        accreditationExpirationDate = "";
        accreditationExtensionDate = "";
        accreditedSince = "";
        cycleEffectiveDate = "";    
        accredLevel = "";
        //membershipDate = "";
        file: any;    
        isCb = false;
        isBusy = false;
        netcordMembership: services.IOrgNetcordMembership;
        //netcordMembershipTypes: services.INetcordMembershipType[];
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
            },
            columns: [
                { field: "facilityName", title: "Facility Name", width: "500px" },
                {
                    template: "<button class=\" k-button\" ng-click=\"vm.viewFacility(dataItem)\">Edit</button>"
                }
            ]
        };
        gridOptionsBAA = {
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
            },
            columns: [
                { field: "documentItem.name", title: "Document Name", width: "500px" },
                {
                    template: "<button class=\" k-button\" ng-click=\"vm.onDownload(dataItem)\">Download</button> <button class=\"k-button\" ng-click=\"vm.onDeleteBAADocument(dataItem)\">Delete</button>"
                }
            ]
        };

        static $inject = [
            '$window',
            '$scope',
            '$uibModal',
            'organizationService',
            'accreditationOutcomeService',
            'documentService',
            'trueVaultService',
            'userService',
            'facilityService',
            'siteService',
            'cacheService',
            'notificationFactory',
            'common',
            'modalHelper',
            'organizationid',
            'organization',
            'facilities',
            'users',
            '$uibModalInstance',
            'config'
        ];

        constructor(
            private $window: ng.IWindowService,
            private $scope: ng.IScope,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private organizationService: services.IOrganizationService,
            private accreditationOutcomeService: services.IAccreditationOutcomeService,
            private documentService: services.IDocumentService,
            private trueVaultService: services.ITrueVaultService,
            private userService: services.IUserService,
            private facilityService: services.IFacilityService,
            private siteService: services.ISiteService,
            private cacheService: services.ICacheService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private modalHelper: common.IModalHelper,
            private organizationid: number,
            private organization: services.IOrganization,
            private facilities: services.IFacility[],
            private users: services.IUser[],
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private config: IConfig) {
            
            if (organization != null) {
                if (organization.baaOwnerItem) {
                    organization.baaOwnerItem.id = organization.baaOwnerItem.id.toString();
                }

                this.getFormattedOrganizationNumber();

                this.organization.baaExecutionDate = this.convertDate(this.organization.baaExecutionDate);
                this.organization.baaVerifiedDate = this.convertDate(this.organization.baaVerifiedDate);
                this.organization.accreditationDate = this.convertDate(this.organization.accreditationDate);
                this.organization.accreditationExtensionDate = this.convertDate(this.organization.accreditationExtensionDate);
                this.organization.accreditationExpirationDate = this.convertDate(this.organization.accreditationExpirationDate);
                this.organization.accreditedSince = this.convertDate(this.organization.accreditedSince);
                
                if (organization.facilityItems) {
                    this.gridOptions.dataSource.data(organization.facilityItems);
                }

                var found = _.find(organization.facilityItems, (fac: services.IFacility) => {
                    return fac.masterServiceType.shortName === "CB";
                });

                
                if (found) {
                    this.isCb = true;
                }

                if (organization
                    .accreditationStatusItem &&
                    organization.accreditationStatusItem.name === "Accredited") {
                    this.getOutcome();
                }

                common.activateController([
                    this.getOrganizationDirector(), this.getAccreditedServices(),
                    this.getAccreditationStatus(), this.getAddress(), this.getBAAOwner(), this.getBAADocuments()], 'editOrganizationController');
            } else {
                common.activateController([
                    this.getOrganizationDirector(), this.getOrg(), this.getAccreditedServices(),
                    this.getAccreditationStatus(), this.getBAAOwner()], 'editOrganizationController');
            }

            if (facilities == null) {
                this.getFacilities();
            }         
        }

        getOutcome() {
            this.accreditationOutcomeService.getAccreditationOutcome(this.organization.organizationId)
                .then((data) => {
                    if (data.length > 0) {
                        this.accredLevel = data[0].outcomeStatusName;
                    }
                    
                });
        }

        getFacilities(): ng.IPromise<void> {
            return this.facilityService.getAllFlat()
                .then((data: services.IFacility[]) => {
                    this.facilities = data;
                });
        }

        convertDate(date: string): string {
            if (date === "" || date ==  null) return "";

            var dte = moment(date);

            return dte.format('MM/DD/YYYY');
        }

        getFormattedOrganizationNumber() {
            if (this.organization && this.organization.organizationId) {
                this.organization.organizationFormattedId = "O00000".substr(0, 6 - this.organization.organizationId.toString().length) + this.organization.organizationId;
            }
        }

        //getNetcordMembershipTypes(): ng.IPromise<void> {
        //    return this.cacheService.getNetcordMembershipTypes()
        //        .then((data: services.INetcordMembershipType[]) => {
        //            this.netcordMembershipTypes = data;
        //        });
        //}

        getOrganizationDirector(): ng.IPromise<void> {
            if (this.users != null) {
                this.organizationDirector = this.users;
                this.organizationUsers = this.users;
                return null;
            }

            return this.userService.getAllUsers(false)
                .then((data: Array<app.services.IUser>) => {
                    
                    if (data == null) {
                        this.notificationFactory.error("No Users records found.");
                    } else {
                        data = this.sortUsers(data);

                        this.organizationDirector = data;
                        this.organizationUsers = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        sortUsers(data: Array<app.services.IUser>) {
            data.sort((a, b) => {
                var nameA = a.lastName.toLowerCase(), nameB = b.lastName.toLowerCase();
                if (nameA < nameB) //sort string ascending
                    return -1;
                if (nameA > nameB)
                    return 1;
                return 0; //default return value (no sorting)
            });

            return data;
        }

        getOrg(): ng.IPromise<void> {
            return this.organizationService.getById(this.organizationid)
                .then((data: services.IOrganization) => {
                    
                    this.organization = data;

                    this.getAddress();

                    if (this.organization.baaOwnerItem) {
                        this.organization.baaOwnerItem.id = this.organization.baaOwnerItem.id.toString();
                    }

                    this.getFormattedOrganizationNumber();

                    this.organization.baaExecutionDate = this.convertDate(this.organization.baaExecutionDate);
                    this.organization.baaVerifiedDate = this.convertDate(this.organization.baaVerifiedDate);
                    this.organization.accreditationDate = this.convertDate(this.organization.accreditationDate);
                    this.organization.accreditationExtensionDate = this.convertDate(this.organization.accreditationExtensionDate);
                    this.organization.accreditationExpirationDate = this.convertDate(this.organization.accreditationExpirationDate);
                    this.organization.accreditedSince = this.convertDate(this.organization.accreditedSince);
                    this.organization.cycleEffectiveDate = this.convertDate(this.organization.cycleEffectiveDate);

                    if (this.organization.accreditationStatusItem &&
                        this.organization.accreditationStatusItem.name === "Accredited") {
                        this.getOutcome();
                    }

                    if (this.organization.facilityItems) {
                        this.gridOptions.dataSource.data(this.organization.facilityItems);
                    }
                    
                    var found = _.find(data.facilityItems, (fac: services.IFacility) => {
                        return fac.masterServiceType.shortName === "CB";
                    });

                    if (found) {
                        this.isCb = true;                       
                    }

                    this.getBAADocuments();
                });
        }

        getOrgByName(): ng.IPromise<void> {
            return this.organizationService.getByName(this.organization.organizationName, true, true)
                .then((data: services.IOrganization) => {

                    this.organization = data;
                    this.organizationid = this.organization.organizationId;

                    if (this.organization.baaOwnerItem) {
                        this.organization.baaOwnerItem.id = this.organization.baaOwnerItem.id.toString();
                    }

                    this.getFormattedOrganizationNumber();

                    this.organization.baaExecutionDate = this.convertDate(this.organization.baaExecutionDate);
                    this.organization.baaVerifiedDate = this.convertDate(this.organization.baaVerifiedDate);
                    this.organization.accreditationDate = this.convertDate(this.organization.accreditationDate);
                    this.organization.accreditationExtensionDate = this.convertDate(this.organization.accreditationExtensionDate);
                    this.organization.accreditationExpirationDate = this.convertDate(this.organization.accreditationExpirationDate);
                    this.organization.accreditedSince = this.convertDate(this.organization.accreditedSince);
                    this.organization.cycleEffectiveDate = this.convertDate(this.organization.cycleEffectiveDate);

                    if (this.organization.facilityItems) {
                        this.gridOptions.dataSource.data(this.organization.facilityItems);
                    }

                    var found = _.find(data.facilityItems, (fac: services.IFacility) => {
                        return fac.masterServiceType.shortName === "CB";
                    });

                    if (found) {
                        this.isCb = true;                      
                    }
                });
        }

        getAccreditedServices(): ng.IPromise<void> {
            return this.organizationService.getAccreditedServices(this.organizationid || 0)
                .then((data: string) => {
                    if (data == null) {
                        this.notificationFactory.error("No accredited services found.");
                    } else {

                        this.accreditedServices = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getAccreditationStatus(): ng.IPromise<void> {
            return this.organizationService.getAccreditationStatus()
                .then((data: Array<app.services.IAccreditationStatus>) => {                    
                    if (data == null) {
                        this.notificationFactory.error("No accreditation status found.");
                    } else {
                        
                        this.accreditationStatus = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getAddress(): ng.IPromise<void> {
            if (this.organization && this.organization.organizationId) {
                return this.siteService.getAddress(this.organization.organizationId)
                    .then((data: app.services.ISite) => {
                        if (data != null) {
                            this.primarySite = data;
                            this.organization.streetAddress1 = data.siteStreetAddress1;
                            this.organization.streetAddress2 = data.siteStreetAddress2;
                            this.organization.city = data.siteCity;
                            this.organization.zip = data.siteZip;

                            if (data.siteProvince != null) {
                                this.organization.stateProvince = data.siteProvince;
                            }
                            else if (data.siteState != null) {
                                this.organization.stateProvince = data.siteState.name;
                            }

                            this.organization.country = data.siteCountry ? data.siteCountry.name : "";
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error getting Organization Address.");
                    });
            }
        }

        getBAAOwner(): ng.IPromise<void> {
            return this.organizationService.getBAAOwner()
                .then((data: Array<app.services.IBAAOwner>) => {
                    if (data == null) {
                        this.notificationFactory.error("No BAA owner found.");
                    } else {

                        this.baaOwner = data;
                    }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        getBAADocuments(): ng.IPromise<void> {
            if (this.organization && this.organization.organizationId) {
                return this.organizationService.getBAADocuments(this.organization.organizationId)
                    .then((data: Array<app.services.IOrganizationBAADocumentItem>) => {
                        if (data) {
                            this.organization.bAADocumentItems = data;
                            this.gridOptionsBAA.dataSource.data(this.organization.bAADocumentItems);
                        }
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                    });
            }
        }

        viewFacility(rowData): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/facilityDemography.html",
                controller: "app.modal.templates.FacilityDemographyController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                windowClass: 'app-modal-window',
                resolve: {
                    parentFacility: () => {
                        rowData.siteTotals = [];
                        return rowData;
                    },
                    isReadonly: () => {
                        return false;
                    },
                    facilityAccredidations: () => {
                        return [];
                    }
                }
            });

            instance.result.then(() => {
            }, () => {
            });
        }

        isDate = function (x) {
            return x instanceof Date;
        };

        isDateValid(): boolean {
            var isValid = false;

            if ((this.baaExecutionDate == ""
                || (this.organization.baaExecutionDate != "" && this.isDate(this.organization.baaExecutionDate)))
                && (this.baaVerifiedDate == ""
                    || (this.organization.baaVerifiedDate != "" && this.isDate(this.organization.baaVerifiedDate)))
                && (this.accreditationDate == ""
                    || (this.organization.accreditationDate != "" && this.isDate(this.organization.accreditationDate)))
                && (this.accreditationExtensionDate == ""
                    || (this.organization.accreditationExtensionDate != "" && this.isDate(this.organization.accreditationExtensionDate)))
                && ((this.organization.accreditedSince != "" && this.isDate(this.organization.accreditedSince)))
                && (this.isDate(this.organization.cycleEffectiveDate)))
            {
                isValid = true;
            }

            return isValid;
        }

        onSaveBaaDocument() {
            this.common.showSplash();

            this.trueVaultService.addFile(this.common.currentUser.documentLibraryAccessToken, this.config.genericKey, this.file, this.file.name, "")
                .then((data: services.ITrueValueResponse) => {
                    if (data.response.result !== "success") {
                        this.notificationFactory.error("Error trying to save document. Please contact support.");
                        this.common.hideSplash();
                    } else {
                        this.documentService.addBAA(this.organization.organizationName, this.file.name, "", this.common.currentUser.documentLibraryAccessToken, data.response.blob_id)
                            .then((data: services.IGenericServiceResponse<services.IDocument>) => {
                                if (data.hasError) {
                                    this.notificationFactory.error(data.message);
                                } else {
                                    this.getBAADocuments();
                                    this.notificationFactory.success("Document saved successfully.");
                                }
                                this.common.hideSplash();
                            })
                            .catch(() => {
                                this.notificationFactory.error("Error trying to save document. Please contact support.");
                                this.common.hideSplash();
                            });
                    }
                })
                .catch((e) => {
                    if (e.indexOf("404.13") != -1) {
                        this.notificationFactory.error("Error trying to save document. Maximum allowed upload file size is 30MB.");
                    }
                    else {
                        this.notificationFactory.error("Error trying to save to True Vault. Please contact support.");
                    }

                    this.common.hideSplash();
                });
        }

        onDownload(document: services.IOrganizationBAADocumentItem): void {
            this.trueVaultService.getBlob(this.config.genericKey,
                document.documentItem.requestValues,
                this.common.currentUser.documentLibraryAccessToken,
                this.config.factKey)
                .then((data: any) => {
                    var fileType = this.trueVaultService.getFileType(document.documentItem.name);
                    var file = new Blob([data.response], { type: fileType });
                    saveAs(file, document.documentItem.name);
                })
                .catch((e) => {
                    this.notificationFactory.error("Cannot get document from True Vault. " + e);
                });
        }

        onDeleteBAADocument(document: services.IOrganizationBAADocumentItem): void {
            if (!confirm("Are you sure you want to delete this document?")) {
                return;
            }

            this.common.showSplash();
            this.documentService.removeBAADocument(this.organization.organizationName, document.documentItem)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        var i = 0;
                        var found = false;

                        for (i = 0; i < this.organization.bAADocumentItems.length; i++) {
                            if (this.organization.bAADocumentItems[i].id === document.id) {
                                found = true;
                                break;
                            }
                        }

                        if (found) {
                            this.organization.bAADocumentItems.splice(i, 1);
                            this.gridOptionsBAA.dataSource.data(this.organization.bAADocumentItems);
                        }

                        this.notificationFactory.success("Document deleted successfully.");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving data.");
                    this.common.hideSplash();
                });
        }

        setAccredExp() {
            var years = this.organization.useTwoYearCycle ? 2 : 3;

            if (this.accreditationDate != undefined && this.accreditationDate != null && this.accreditationDate !== "") {
                var dte = moment(this.organization.accreditationDate);
                dte = dte.add(years, 'Y');
                this.accreditationExpirationDate = dte.format("MM/DD/YYYY");
                this.organization.accreditationExpirationDate = dte.format("MM/DD/YYYY");
            }
            
        }

        addUser(type: string): void {
            var values = {
                parentUser: () => {
                    return null;
                },
                isNewUser: () => {
                    return true;
                },
                currentOrganization: () => {
                    return null;
                },
                role: () => {
                    return type;
                },
                isPersonnel: () => {
                    return false;
                }
            };

            this.modalHelper.showModal("/app/modal.templates/editUser.html", "app.modal.templates.EditUserController", values)
                .then((data: services.IUser) => {
                    if (data == null) return;

                    if (type === "contact") {
                        this.organization.primaryUser = data;
                    } else {
                        //this.organization.organizationDirector = data;
                    }

                    this.organizationDirector.push(data);

                    this.organizationDirector = this.sortUsers(this.organizationDirector);

                    this.organizationUsers.push(data);

                    this.organizationUsers = this.sortUsers(this.organizationUsers);
                })
                .catch(() => {
                });

        }

        save(closeForm): void {
            this.isBusy = true;       
            if (this.organizationid) {
                if ((this.organization.cycleNumber != null &&
                    this.organization.cycleNumber != undefined &&
                    this.organization.cycleNumber !== "") && (this.organization.cycleEffectiveDate == null || this.organization.cycleEffectiveDate == undefined || this.organization.cycleEffectiveDate === "")) {
                    this.notificationFactory.error("You must enter a Cycle Effective Date when the Cycle Number is entered.");
                    this.isBusy = false;
                    return;
                }

                this.common.showSplash();

                this.organizationService.update(this.organization)
                    .then((data: services.IServiceResponse) => {
                        this.isBusy = false;
                        $(".app-modal-window").scrollTop(0); 
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.notificationFactory.success("Organization updated successfully.");

                            if (closeForm) {
                                this.$uibModalInstance.close({ facilities: this.facilities, users: this.organizationDirector, organization: this.organization });
                            }
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to update organization. Please contact support.");
                        this.common.hideSplash();
                    });
            } else {
                this.common.showSplash();
                this.organizationService.save(this.organization)
                    .then((data: services.IGenericServiceResponse<services.IOrganization>) => {
                        this.isBusy = false;
                        $(".app-modal-window").scrollTop(0); 
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.notificationFactory.success("Organization added successfully.");

                            if (closeForm) {
                                this.$uibModalInstance.close({ facilities: this.facilities, users: this.organizationDirector, organization: data.item });
                            }
                            else {
                                this.getOrgByName();
                            }
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to update organization. Please contact support.");
                        this.common.hideSplash();
                    });
            }
            //this.$window.scrollTo(0, 0);            
        }

        cancel(): void {
            this.$uibModalInstance.dismiss({ facilities: this.facilities, users: this.organizationDirector, organization: this.organization });
        }

        editPrimarySite(): void {
            var instance = this.$uibModal.open({
                animation: true,
                templateUrl: "/app/modal.templates/newSite.html",
                controller: "app.modal.templates.NewSiteController",
                controllerAs: "vm",
                size: 'lg',
                backdrop: false,
                windowClass: 'app-modal-window',
                resolve: {
                    values: () => {
                        return {
                            parentSite: this.primarySite,
                            isNewSite: false,
                            facilities: this.facilities,
                            siteName: this.primarySite.siteName
                        }
                    }
                }
            });
        }
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.EditOrganizationController',
        EditOrganizationController);
}  