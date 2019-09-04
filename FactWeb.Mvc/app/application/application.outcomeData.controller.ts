module app.Application {
    'use strict';

    interface ILocalCibmtr extends services.ICibmtr {
        latestAnalysis: services.ICibmtrOutcomeAnalysis;
        latestData: services.ICibmtrDataMgmt;
        cpiData: services.ICibmtrDataMgmt[];
        dataManagementData: Object;
        analysisOpen: boolean;
    }

    class OutcomeDataController {
        application: services.IApplication;
        isComplianceApplication: boolean;
        complianceApplication: services.IComplianceApplication;
        cibmtrData: ILocalCibmtr[] = [];
        inspectors: services.IInspectionScheduleDetail[] = [];
        uniqueId = "";
        compAppId = "";
        isReadOnly = false;
        isTrainee = false;
        isReviewed = false;
        isClinical = false;
        dataManagementRequirementNumber = "";
        outcomeAnalysisRequirementNumber = "";
        isNotRequired = false;

        static $inject = [
            '$rootScope',
            '$location',
            '$q',
            '$uibModal',
            'applicationSettingService',
            'applicationService',
            'facilityService',
            'inspectionService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private $q: ng.IQService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private applicationSettingService: services.IApplicationSettingService,
            private applicationService: services.IApplicationService,
            private facilityService: services.IFacilityService,
            private inspectionService: services.IInspectionService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {

            this.common.showSplash();

            this.getAppSettings();

            this.uniqueId = $location.search().app;
            this.compAppId = $location.search().c;

            if (this.compAppId !== "") {
                this.isComplianceApplication = true;
                //this.getComplianceApplication();
            }
            else {
                this.isComplianceApplication = false;
            }

            this.common.checkItemValue(this.config.events.userLoggedIn, this.common.currentUser, false)
                .then(() => {
                    this.isReadOnly = this.common.isFact();
                });

            this.common.checkItemValue(this.config.events.organizationLoaded, this.common.organization, false)
                .then(() => {
                    var found = _.find(this.common.organization.facilities, (org) => {
                        return org.serviceTypeName.indexOf("Clinical Program") > -1;
                    });

                    if (found) {
                        this.isClinical = true;
                    } else {
                        this.isClinical = false;
                    }
                });

            this.common.checkItemValue(this.config.events.complianceApplicationInspectorsLoaded, this.common.compAppInspectors, false)
                .then(() => {
                    console.log(this.inspectors);
                    this.inspectors = this.common.compAppInspectors;

                    var found = _.find(this.inspectors, (i) => {
                        return i.user.userId === this.common.currentUser.userId && i.roleName === "Inspector Trainee";
                    });

                    if (found) {
                        this.isTrainee = true;
                        this.isReadOnly = true;
                    }

                    for (var i = 0; i < this.inspectors.length; i++) {
                        var d = this.inspectors[i];

                        if (d.user.userId === this.common.currentUser.userId) {
                            if (d.reviewedOutcomesData) {
                                this.isReviewed = true;
                            }
                        }
                    } 
                });

            this.getApplication();
        }

        getAppSettings() {
            this.applicationSettingService.getAll()
                .then((data) => {
                    var req = _.find(data, (r) => {
                        return r.name === "Data Management Requirement Number";
                    });

                    if (req) {
                        this.dataManagementRequirementNumber = req.value;
                    }

                    var reqs = _.find(data, (r) => {
                        return r.name === "Outcome Analysis Requirement Number";
                    });

                    if (reqs) {
                        this.outcomeAnalysisRequirementNumber = reqs.value;
                    }
                });
        }
        
        getApplication(): ng.IPromise<void> {
            return this.applicationService.getApp(this.uniqueId)
                .then((data: services.IApplication) => {
                    this.application = data;
                    this.application.applicantApplicationStatusName = this.application.applicationStatusName;

                    this.$q.all([this.getCibmtrData()])
                        .then(() => {
                            this.common.hideSplash();
                        });
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting application");
                });
        }

        getCibmtrData(): ng.IPromise<void> {
            return this.facilityService.getCibmtrDataForOrg(this.application.organizationName)
                .then((data: services.ICibmtr[]) => {
                    var fd = _.find(data, (d) => {
                        var f = _.find(d.cibmtrOutcomeAnalyses, (o: services.ICibmtrOutcomeAnalysis) => {
                            return o.isNotRequired === true;
                        });

                        if (f) {
                            return true;
                        } else {
                            return false;
                        }
                    });

                    if (fd) {
                        this.isNotRequired = true;
                    }

                    console.log('isnotrequired', this.isNotRequired);

                    _.each(data, (cibmtr) => {
                        var rows = _.orderBy(cibmtr.cibmtrOutcomeAnalyses, ['reportYear'], ['desc']);

                        if (rows.length > 5) {
                            rows = rows.splice(0, 5);
                        }

                        var dataRows = _.filter(cibmtr.cibmtrDataMgmts, (cib) => {
                            return cib.auditDate != null;
                        });

                        var datas = _.orderBy(dataRows, ['auditDate'], ['desc']);

                        if (datas.length > 3) {
                            datas = datas.splice(0, 3);
                        }

                        _.each(cibmtr.cibmtrDataMgmts, (c) => {
                            c.orderDate = c.auditDate != null ? c.auditDate : c.cpiLetterDate;
                        });

                        var cpis = _.filter(cibmtr.cibmtrDataMgmts, (cib) => {
                            return cib.orderDate != null;
                        });

                        var cpiData = _.orderBy(cpis, ['orderDate'], ['desc']);

                        let dataManagementData = {
                            correctiveActions: '',
                            determination: '',
                            recommendation: ''
                        };

                        if (cpiData.length > 0) {
                            

                            for (var i = 0; i < cpiData.length; i++) {
                                if (cpiData[i].correctiveActions && cpiData[i].correctiveActions != null && cpiData[i].correctiveActions !== '') {
                                    dataManagementData.correctiveActions = cpiData[i].correctiveActions;
                                    dataManagementData.determination = cpiData[i].factProgressDetermination;
                                    dataManagementData.recommendation = cpiData[i].additionalInformation;
                                    break;
                                }
                            }

                        }

                        if (cpiData.length > 3) {
                            cpiData = cpiData.splice(0, 3);
                        }

                        //_.each(datas, (d) => {
                        //    d.criticalFieldErrorRate = !d.criticalFieldErrorRate || d.criticalFieldErrorRate < 1
                        //        ? d.criticalFieldErrorRate
                        //        : (d.criticalFieldErrorRate / 100);

                        //    d.randomFieldErrorRate = !d.randomFieldErrorRate || d.randomFieldErrorRate < 1
                        //        ? d.randomFieldErrorRate
                        //        : (d.randomFieldErrorRate / 100);

                        //    d.overallFieldErrorRate = !d.overallFieldErrorRate || d.overallFieldErrorRate < 1
                        //        ? d.overallFieldErrorRate
                        //        : (d.overallFieldErrorRate / 100);
                        //});

                        var row: ILocalCibmtr = {
                            id: cibmtr.id,
                            centerNumber: cibmtr.centerNumber,
                            ccnName: cibmtr.ccnName,
                            transplantSurvivalReportName: cibmtr.transplantSurvivalReportName,
                            displayName: cibmtr.displayName,
                            isNonCibmtr: cibmtr.isNonCibmtr,
                            facilityName: cibmtr.facilityName,
                            cibmtrOutcomeAnalyses: rows,
                            cibmtrDataMgmts: datas,
                            latestAnalysis: rows.length > 0 ? rows[0] : null,
                            latestData: datas.length > 0 ? datas[0] : null,
                            cpiData: cpiData,
                            isActive: cibmtr.isActive,
                            dataManagementData: dataManagementData,
                            analysisOpen: rows.length > 0 ? !rows[0].isNotRequired : false
                        };

                        console.log(row);

                        this.cibmtrData.push(row);
                    });
                })
                .catch((e) => {
                    this.notificationFactory.error("Cannot get Outcomes/Data. Please contact support." + e);
                });
        }

        onSaveReview() {
            this.common.showSplash();

            this.inspectionService.setReviewOutcome(this.compAppId)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("Outcomes/Data successfully marked as reviewed.");
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error marking Outcomes/Data as reviewed. Please contact support.");
                    this.common.hideSplash();
                });
        }

        canMarkReviewed(): boolean {
            var canMark = true;

            _.each(this.cibmtrData, (c) => {
                if (c.latestAnalysis && c.latestAnalysis.id && c.latestAnalysis.id !== "" && !c.latestAnalysis.isNotRequired) {
                    if (c.latestAnalysis.progressOnImplementation === null ||
                        c.latestAnalysis.progressOnImplementation === "" ||
                        c.latestAnalysis.inspectorInformation == null ||
                        c.latestAnalysis.inspectorInformation === "" ||
                        c.latestAnalysis.inspectorCommendablePractices == null ||
                        c.latestAnalysis.inspectorCommendablePractices === "" ||
                        c.latestAnalysis.inspector100DaySurvival == null ||
                        c.latestAnalysis.inspector100DaySurvival === "" ||
                        c.latestAnalysis.inspector1YearSurvival == null ||
                        c.latestAnalysis.inspector1YearSurvival === "") {
                        canMark = false;
                        return false;
                    }
                }

                if (c.latestData && c.latestData.id && c.latestData.id !== "") {
                    if (
                        c.latestData.progressOnImplementation == null ||
                        c.latestData.progressOnImplementation === "" ||
                        c.latestData.inspectorInformation == null ||
                        c.latestData.inspectorInformation === "" ||
                        c.latestData.inspectorCommendablePractices == null ||
                        c.latestData.inspectorCommendablePractices === "") {
                        canMark = false;
                        return false;
                    }
                }
            });

            return canMark;
        }

        onSubmit() {
            var outcomes = [];
            var mgmts = [];

            _.each(this.cibmtrData, (c) => {
                if (c.latestAnalysis && c.latestAnalysis.id && c.latestAnalysis.id !== "") {
                    outcomes.push(c.latestAnalysis);
                }
                
                if (c.latestData && c.latestData.id && c.latestData.id !== "") {
                    mgmts.push(c.latestData);
                }
            });

            this.common.showSplash();

            this.facilityService.updateCibmtrs(outcomes, mgmts)
                .then((data) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success("CIBMTR Outcome/Data saved successfully.");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving CIBMTR data. Please contact support.");
                    this.common.hideSplash();
                });
        }
        
    }

    angular
        .module('app.application')
        .controller('app.application.OutcomeDataController',
        OutcomeDataController);
}