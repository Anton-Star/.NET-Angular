module app.services {
    'use strict';

    export interface IApplicationService {
        sendToFact(app: string, leadName: string, orgName: string, appTypeName: string, coordinatorEmail: string, applicationTypeId: number, organizationId: number): ng.IPromise<IServiceResponse>;
        getApplicationSections(orgName: string, type: string): ng.IPromise<Array<IApplicationSection>>;        
        getAllApplications(): ng.IPromise<Array<IApplication>>;
        getApp(id: string): ng.IPromise<IApplication>;
        getStatusView(id: string): ng.IPromise<IApplicationStatusView>;
        getApplication(orgName: string, type: string): ng.IPromise<IApplication>;
        getApplicationsByComplianceId(complianceId: string): ng.IPromise<Array<IApplication>>;
        //getComplianceApplication(type: string): ng.IPromise<IApplication>;
        getEligibilityApplication(): ng.IPromise<Array<IApplicationSection>>;
        getAppSections(id: string): ng.IPromise<Array<IApplicationSection>>;
        getApplicationAccess(id: string): ng.IPromise<string>;
        getComplianceApplicationAccess(id: string): ng.IPromise<string>;
        getRFIView(type: string): ng.IPromise<IRfiViewItem>;
        getInspectionSchedules(organizationId?: string): ng.IPromise<Array<IInspectionSchedule>>;
        getApplicationTypes(): ng.IPromise<Array<IApplicationType>>;        
        getApplicationStatus(): ng.IPromise<Array<IApplicationStatus>>;
        getApplicationStatusHistory(applicationUniqueId?: string, compAppId?: string): ng.IPromise<Array<IApplicationStatusHistory>>;
        getCompAppApprovals(compAppId: string): ng.IPromise<Array<ICompAppApproval>>;
        getApplicationApprovals(appUniqueId: string): ng.IPromise<Array<ICompAppApproval>>;
        saveApplicationSection(orgName: string, type: string, appUniqueId: string, section, isRfi: boolean): ng.IPromise<IServiceResponse>;
        saveMultiviewSection(orgName: string, type: string, sections: Eligibility.IApplicationHierarchyData[]): ng.IPromise<IServiceResponse>;
        saveMultiviewResponseStatus(orgName: string, type: string, sections: Eligibility.IApplicationHierarchyData[]): ng.IPromise<IServiceResponse>;
        saveApplicationSectionTrainee(orgName: string, type: string, appUniqueId: string, section): ng.IPromise<IServiceResponse>;
        updateApplicationStatus(applicationTypeId: number, applicationStatusName: string, organizationId: number, dueDate: Date, emailHtml: string, includeAccreditationReport: boolean): ng.IPromise<IServiceResponse>;
        submitApplication(orgName: string, type: string): ng.IPromise<IServiceResponse>;
        getApplicationResponseStatus(): ng.IPromise<Array<IApplicationResponseStatusItem>>;    
            
        getComplianceApplication(name: string): ng.IPromise<IComplianceApplication[]>;
        getSubmittedComplianceApplication(eligibilityOrRenewalAppUniqueId: string): ng.IPromise<ISubmittedCompliance>;
        getComplianceApplicationById(complianceApplicationId: string, useCache: boolean): ng.IPromise<IComplianceApplication>;
        cancelComplianceApplication(id: string): ng.IPromise<IServiceResponse>;
        getComplianceApplicationServiceType(complianceApplicationId: string): ng.IPromise<IComplianceApplication>;
        showAccreditationReport(complianceApplicationId: string): ng.IPromise<boolean>;
        setupComplianceApplication(complianceApp: services.IComplianceApplication): ng.IPromise<IGenericServiceResponse<string>>;
        setComplianceApplicationApprovalStatus(compAppId: string, approvalStatus: string, serialNumber: string, rejectionComments: string): ng.IPromise<IServiceResponse>;
        copyApplication(model: ICopyComplianceApplicationModel): ng.IPromise<IGenericServiceResponse<IComplianceApplication>>;
        getCompApplication(compAppId: string, appId: string): ng.IPromise<ICompApplication>;

        approveEligibility(complianceApp: services.IComplianceApplication): ng.IPromise<IServiceResponse>;
        getApplications(orgName: string): ng.IPromise<Array<IApplication>>;
        getApplicationsSimple(orgName: string): ng.IPromise<Array<IApplication>>;
        createApplication(orgName: string, applicationTypeName: string, coordinator: string, dueDate: string): ng.IPromise<IGenericServiceResponse<IApplication>>;
        updateApplicationCoordinator(uniqueId: string, coordinator: string, applicationStatus: string,dueDate:string): ng.IPromise<IGenericServiceResponse<IUser>>;
        cancelApplication(uniqueId: string): ng.IPromise<IServiceResponse>;        
        getCoordinatorApplications(showAll: boolean): ng.IPromise<Array<ICoordinatorApplication>>;
        getInspectorApplications(): ng.IPromise<Array<IApplication>>;
        saveApplicationType(applicationType: IApplicationType): ng.IPromise<IGenericServiceResponse<IApplicationType>>;
        getInspectors(app: string): ng.IPromise<Array<IInspectionScheduleDetail>>;
        getInspectionCompletionStatus(uniqueId: string): ng.IPromise<boolean>;
        setInspectorComplete(applicationId: number): ng.IPromise<IServiceResponse>;
        sendForInspection(compId: string, orgName: string, coordinatorId: string, isCb: boolean): ng.IPromise<IServiceResponse>;
        SaveApplication(application: IApplication): ng.IPromise<IServiceResponse>;
        bulkUpdateApplicationResponseStatus(object: string): ng.IPromise<IServiceResponse>;
        submitCompliance(app: string): ng.IPromise<IServiceResponse>;
        getApplicationStatusByUniqueId(appUniqueId: string): ng.IPromise<IApplicationStatus>;
        getCbTotals(compId: string): ng.IPromise<ICbTotal[]>;
        getCtTotals(compId: string): ng.IPromise<ICtTotal[]>;
        getApplicationReport(compId: string, siteName: string): ng.IPromise<IApplicationReport[]>;
        getAppReport(appUniqueId: string): ng.IPromise<IApplicationReport[]>;

        getCompAppInspectionDetail(compAppId: string): ng.IPromise<ICompAppInspectionDetail>;
        saveCompAppInspectionDetail(detail: ICompAppInspectionDetail): ng.IPromise<IGenericServiceResponse<ICompAppInspectionDetail>>;
        compAppHasRfis(id: string): ng.IPromise<boolean>;
    }

    class ApplicationService implements IApplicationService {
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        sendToFact(app: string, leadName: string, orgName: string, appTypeName: string, coordinatorEmail: string, applicationTypeId: number, organizationId: number): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Application/SendToFact', { app: app, leadName: leadName, orgName: orgName, appTypeName: appTypeName, coordinatorEmail: coordinatorEmail, applicationTypeId: applicationTypeId, organizationId: organizationId })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAllApplications(): ng.IPromise<Array<IApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApp(id: string): ng.IPromise<IApplication> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application?id=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplication>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getStatusView(id: string): ng.IPromise<IApplicationStatusView> {
            var deferred = this.$q.defer();
            this.$http
                .get('api/Application/StatusView?id=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationStatusView>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplication(orgName: string, type: string): ng.IPromise<IApplication> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application?orgName=' + orgName + '&type=' + type)
                .success((response: ng.IHttpPromiseCallbackArg<IApplication>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        getApplicationsByComplianceId(complianceId): ng.IPromise<Array<IApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/ApplicationsByComplianceId?complianceId=' + complianceId )
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAppSections(id: string): ng.IPromise<Array<IApplicationSection>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Sections?id=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationSection>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getComplianceApplicationAccess(id: string): ng.IPromise<string> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Compliance/Access?id=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<string>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplicationAccess(id: string): ng.IPromise<string> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/AccessType?id=' + id)
                .success((response: ng.IHttpPromiseCallbackArg<string>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getEligibilityApplication(): ng.IPromise<Array<IApplicationSection>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Eligibility')
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationSection>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 
               
        getRFIView(type: string): ng.IPromise<IRfiViewItem> {
            var deferred = this.$q.defer();
            this.$http
                //TODO: This needs updated to allow any application type
                .get('/api/Application/RfiView?type=' + type)
                .success((response: ng.IHttpPromiseCallbackArg<IRfiViewItem>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getApplicationSections(orgName: string, type: string): ng.IPromise<Array<IApplicationSection>> {
            
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Sections?orgName=' + encodeURIComponent(orgName) + '&type=' + type)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationSection>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getComplianceApplication(name: string): ng.IPromise<IComplianceApplication[]> {
            name = encodeURIComponent(name);
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Compliance?name=' + name)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IComplianceApplication[]>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getSubmittedComplianceApplication(eligibilityOrRenewalAppUniqueId: string): ng.IPromise<ISubmittedCompliance> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Compliance/Submitted?app=' + eligibilityOrRenewalAppUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<string>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getComplianceApplicationById(complianceApplicationId: string, useCache: boolean): ng.IPromise<IComplianceApplication> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Compliance?complianceApplicationId=' + complianceApplicationId + "&useCache=" + (useCache ? "Y" : "N"))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationSection>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        cancelComplianceApplication(id: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Application/Compliance/Cancel?id=' + id, {})
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getComplianceApplicationServiceType(complianceApplicationId: string): ng.IPromise<IComplianceApplication> {

            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Compliance/ServiceType?complianceApplicationId=' + complianceApplicationId)
                .success((response: ng.IHttpPromiseCallbackArg<IComplianceApplication[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
            
        }

        showAccreditationReport(complianceApplicationId: string): ng.IPromise<boolean> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Compliance/AccreditationReport?complianceApplicationId=' + complianceApplicationId)
                .success((response: ng.IHttpPromiseCallbackArg<boolean>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveApplicationType(applicationType: IApplicationType): ng.IPromise<IGenericServiceResponse<IApplicationType>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Application/Type', {
                    applicationTypeId: applicationType.applicationTypeId,
                    applicationTypeName: applicationType.applicationTypeName,
                })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        submitApplication(orgName: string, type: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Application/Submit?orgName=' + encodeURIComponent(orgName) + '&applicationType=' + type, {})
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        createApplication(orgName: string, applicationTypeName: string, coordinator: string, dueDate: string): ng.IPromise<IGenericServiceResponse<IApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Application', {
                    organizationName: orgName,
                    applicationTypeName: applicationTypeName,
                    coordinator: coordinator,
                    dueDate: dueDate        
                })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        updateApplicationCoordinator(uniqueId: string, coordinator: string, applicationStatus: string, dueDate: string): ng.IPromise<IGenericServiceResponse<IUser>> {
            var deferred = this.$q.defer();
            this.$http
                .post('/api/Application/Coordinator', {
                    uniqueId: uniqueId,
                    coordinator: coordinator,
                    applicationStatus: applicationStatus,
                    dueDate: dueDate

                })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IUser>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        cancelApplication(uniqueId: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .delete('/api/Application?uniqueId=' + uniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IUser>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplications(orgName: string): ng.IPromise<Array<IApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application?orgName=" + encodeURIComponent(orgName))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getApplicationsSimple(orgName: string): ng.IPromise<Array<IApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Simple?orgName=" + encodeURIComponent(orgName))
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCoordinatorApplications(showAll: boolean): ng.IPromise<Array<ICoordinatorApplication>> {
            
            var deferred = this.$q.defer();
            var all = (showAll ? "Y" : "N");

            this.$http
                .get(
                "/api/Application/Coordinator?showAll=" + all)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getInspectorApplications(): ng.IPromise<Array<IApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Inspector")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        getInspectionSchedules(organizationId?: string): ng.IPromise<Array<IInspectionSchedule>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/InspectionSchedules/" + organizationId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IInspectionSchedule>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }      

        getApplicationTypes(): ng.IPromise<Array<IApplicationType>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Types")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationType>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  

        getApplicationStatus(): ng.IPromise<Array<IApplicationStatus>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Status")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationStatus>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  

        getApplicationStatusHistory(applicationUniqueId?: string, compAppId?: string): ng.IPromise<Array<IApplicationStatusHistory>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/StatusHistory?app=" + applicationUniqueId + "&compApp=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationStatusHistory>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  

        getCompAppApprovals(compAppId: string): ng.IPromise<Array<ICompAppApproval>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Compliance/Approvals?complianceApplicationId=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICompAppApproval>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getApplicationApprovals(appUniqueId: string): ng.IPromise<Array<ICompAppApproval>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Approvals?appUniqueId=" + appUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ICompAppApproval>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        saveEligibilityApplication(orgName: string, sections: Array<Eligibility.IApplicationHierarchyData>): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();            
            this.$http
                .put(
                "/api/Application/Eligibility", JSON.stringify({
                    orgName: orgName,
                    sections: sections
                    }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveApplicationSection(orgName: string, type: string, appUniqueId: string, section, isRfi: boolean): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            var sect = {
                questions: section.questions,
                id: section.applicationSectionId || section.id,
                applicationSectionId: section.applicationSectionId || section.id,
                isRfi: isRfi
            };

            this.$http
                .post(
                "/api/Application/Save", JSON.stringify({
                    orgName: orgName,
                    type: type,
                    appUniqueId: appUniqueId,
                    section: sect
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveMultiviewSection(orgName: string, type: string, sections: Eligibility.IApplicationHierarchyData[]): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/SaveMultiview", JSON.stringify({
                    orgName: orgName,
                    type: type,
                    sections: sections
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveMultiviewResponseStatus(orgName: string, type: string, sections: Eligibility.IApplicationHierarchyData[]): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/SaveMultiviewResponseStatus", JSON.stringify({
                    orgName: orgName,
                    type: type,
                    sections: sections
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveApplicationSectionTrainee(orgName: string, type: string, appUniqueId: string, section): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();

            var sect = {
                questions: section.questions,
                id: section.applicationSectionId
            };

            this.$http
                .post(
                "/api/Application/SaveSectionTrainee", JSON.stringify({
                    orgName: orgName,
                    type: type,
                    appUniqueId: appUniqueId,
                    section: sect
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        setupComplianceApplication(complianceApp: services.IComplianceApplication): ng.IPromise<IGenericServiceResponse<string>> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/Compliance/Setup", JSON.stringify(complianceApp))
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<string>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        setComplianceApplicationApprovalStatus(compAppId: string, approvalStatus: string, serialNumber: string, rejectionComments: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/Compliance/ApprovalStatus",
                    {
                        complianceApplication: {
                            id: compAppId,
                            approvalStatus: {
                                name: approvalStatus
                            },
                            rejectionComments: rejectionComments
                        },
                        serialNumber: serialNumber,
                        rejectionComments: rejectionComments
                    })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        copyApplication(model: ICopyComplianceApplicationModel): ng.
            IPromise<IGenericServiceResponse<IComplianceApplication>> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/Compliance/Copy", model)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IComplianceApplication>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCompApplication(compAppId: string, appId: string): ng.IPromise<ICompApplication> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Application/Compliance/Simple?complianceApplicationId=' + compAppId + "&appId=" + appId)
                .success((response: ng.IHttpPromiseCallbackArg<ICompApplication>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        
        approveEligibility(complianceApp: services.IComplianceApplication): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/Eligibility/Approve", JSON.stringify(complianceApp))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        updateApplicationStatus(applicationTypeId: number, applicationStatusName: string, organizationId: number, dueDate: Date, emailHtml: string, includeAccreditationReport: boolean): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .put(
                "/api/Application/Status", JSON.stringify({
                    applicationTypeId: applicationTypeId,
                    applicationStatusName: applicationStatusName,
                    organizationId: organizationId,
                    dueDate: dueDate,
                    template: emailHtml,
                    includeAccreditationReport: includeAccreditationReport
                }))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplicationResponseStatus(): ng.IPromise<Array<IApplicationResponseStatusItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/ResponseStatus")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationResponseStatusItem>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplicationStatusById(applicationUniqueId: string): ng.IPromise<Array<IApplicationResponseStatusItem>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/ResponseStatus")
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationResponseStatusItem>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getInspectors(app: string): ng.IPromise<Array<IInspectionScheduleDetail>> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Inspectors?app=" + app)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IInspectionScheduleDetail>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }  

        getInspectionCompletionStatus(uniqueId: string): ng.IPromise<boolean> {
            var deferred = this.$q.defer();
            this.$http
                .get(
                "/api/Application/Inspector/InspectionStatus?uniqueId=" + uniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<boolean>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        

        setInspectorComplete(applicationId: number): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/Inspector?app=" + applicationId, {})
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        sendForInspection(compId: string, orgName: string, coordinatorId: string, isCb: boolean): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/Inspection", {
                    compId: compId,
                    orgName: orgName,
                    coordinatorId: coordinatorId,
                    isCb: isCb
                })
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        SaveApplication(application: IApplication): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/SaveApplication", JSON.stringify(application))
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        bulkUpdateApplicationResponseStatus(object: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/BulkUpdateApplicationResponseStatus", object)
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        submitCompliance(app: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "/api/Application/Compliance/Submit?app=" + app,{})
                .success((response: ng.IHttpPromiseCallbackArg<IServiceResponse>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplicationStatusByUniqueId(appUniqueId: string): ng.IPromise<IApplicationStatus> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Application/ApplicationStatusByUniqueId?appUniqueId=" + appUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationStatus>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getCbTotals(compId: string): ng.IPromise<ICbTotal[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Application/CbTotals?id=" + compId)
                .success((response: ng.IHttpPromiseCallbackArg<ICbTotal[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCtTotals(compId: string): ng.IPromise<ICtTotal[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Application/CtTotals?id=" + compId)
                .success((response: ng.IHttpPromiseCallbackArg<ICtTotal[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getApplicationReport(compId: string, siteName: string): ng.IPromise<IApplicationReport[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Application/Report?id=" + compId + "&siteName=" + encodeURIComponent(siteName))
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationReport[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getAppReport(appUniqueId: string): ng.IPromise<IApplicationReport[]> {
            var deferred = this.$q.defer();
            this.$http
                .get("/api/Application/Report?appId=" + appUniqueId)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationReport[]>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getCompAppInspectionDetail(compAppId: string): ng.IPromise<ICompAppInspectionDetail> {
            var deferred = this.$q.defer();
            this.$http
                .get("api/Application/ComplianceApplication/CompInspectionDetail?id=" + compAppId)
                .success((response: ng.IHttpPromiseCallbackArg<ICompAppInspectionDetail>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        saveCompAppInspectionDetail(detail: ICompAppInspectionDetail): ng.IPromise<IGenericServiceResponse<ICompAppInspectionDetail>> {
            var deferred = this.$q.defer();
            this.$http
                .post(
                "api/Application/ComplianceApplication/CompInspectionDetail", detail)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<ICompAppInspectionDetail>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        compAppHasRfis(id: string): ng.IPromise<boolean> {
            var deferred = this.$q.defer();
            this.$http
                .get("api/Application/CompApp/HasRfis?id=" + id)
                .success((response: ng.IHttpPromiseCallbackArg<boolean>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
    }

    factory.$inject = [
        '$http',
        '$q',
        'config'
    ];
    function factory($http: ng.IHttpService,
        $q: ng.IQService,
        config: IConfig): IApplicationService {
        return new ApplicationService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('applicationService',
        factory);
} 