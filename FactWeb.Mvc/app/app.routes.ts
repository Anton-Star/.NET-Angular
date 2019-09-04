interface IRouteConfig {
    templateUrl: string;
    controller: string;
    controllerAs: string;
}

interface IRoute {
    url: string;
    title?: string;
    isShown?: boolean;
    config: IRouteConfig;
    roles?: Array<string>;
}

((): void => {
    'use strict';

    var allRoutes = getRoutes();
    
    angular
        .module('app')
        .constant('routes', allRoutes);

    angular
        .module('app')
        .config(['$routeProvider', ($routeProvider: ng.route.IRouteProvider) => {
            allRoutes.forEach(r => {
                $routeProvider.when(r.url, {
                    templateUrl: r.config.templateUrl,
                    controller: r.config.controller,
                    controllerAs: r.config.controllerAs
                });                
                return $routeProvider;
            });
            $routeProvider.otherwise({ redirectTo: '/' });
        }]);

    function getRoutes(): Array<IRoute> {
        return [
            {
                url: '/',
                title: 'Login',
                isShown: false,
                config: {
                    templateUrl: 'app/home/login.html',
                    controller: 'app.home.LoginController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/RequestAccess',
                title: 'Request Access',
                isShown: false,
                config: {
                    templateUrl: 'app/home/request.html',
                    controller: 'app.home.RequestAccessController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Application',
                title: 'Login',
                isShown: false,
                config: {
                    templateUrl: 'app/home/application.html',
                    controller: 'app.home.ApplicationController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Coordinator", "FACT Consultant Coordinator", "FACT Consultant", "FACT Quality Manager"]
            },
            {
                url: '/Compliance',
                title: '',
                isShown: false,
                config: {
                    templateUrl: 'app/compliance/application.html',
                    controller: 'app.compliance.ApplicationController',
                    controllerAs: 'vm'
                }
            },
            //{
            //    url: '/Compliance/Multiview',
            //    title: '',
            //    isShown: false,
            //    config: {
            //        templateUrl: 'app/compliance/multiview.html',
            //        controller: 'app.compliance.MultiViewController',
            //        controllerAs: 'vm'
            //    }
            //},
            {
                url: '/Coordinator/View',
                title: 'Coordinator View',
                isShown: false,
                config: {
                    templateUrl: 'app/coordinator/view.html',
                    controller: 'app.coordinator.ViewController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Consultant Coordinator", "FACT Coordinator", "FACT Quality Manager"]
            },
            {
                url: '/Coordinator/Applications',
                title: 'Coordinator Applications',
                isShown: false,
                config: {
                    templateUrl: 'app/coordinator/applications.html',
                    controller: 'app.coordinator.ApplicationsController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Coordinator", "FACT Consultant Coordinator", "FACT Quality Manager"]
            },
            {
                url: '/Inspector/Inspections',
                config: {
                    templateUrl: 'app/inspector/inspections.html',
                    controller: 'app.inspector.InspectionsController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "Inspector", "FACT Quality Manager"]
            },
            {
                url: '/Inspector/Applications',
                config: {
                    templateUrl: 'app/eligibility/home.html',
                    controller: 'app.eligibility.HomeController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "Inspector", "FACT Quality Manager"]
            },
            {
                url: '/Register',
                title: 'Register',
                isShown: false,
                config: {
                    templateUrl: 'app/account/register.html',
                    controller: 'app.account.RegisterController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Application/StatusView',
                title: 'Status View',
                isShown: false,
                config: {
                    templateUrl: 'app/application/statusView.html',
                    controller: 'app.application.StatusViewController',
                    controllerAs: 'vm'
                },
                roles: ["Primary Contact", "FACT Consultant Coordinator"]
            },
            {
                url: '/Application/Personnel',
                title: 'Personnel',
                isShown: false,
                config: {
                    templateUrl: 'app/application/personnel.html',
                    controller: 'app.application.PersonnelController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Application/OutcomeData',
                title: 'OutcomeData',
                isShown: false,
                config: {
                    templateUrl: 'app/application/outcomeData.html',
                    controller: 'app.application.OutcomeDataController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Application/History',
                title: 'Application History',
                isShown: false,
                config: {
                    templateUrl: 'app/application/history.html',
                    controller: 'app.application.HistoryController',
                    controllerAs: 'vm'
                },
                roles: ["Primary Contact", "FACT Consultant Coordinator"]
            },
            {
                url: '/RegisterStep2',
                title: 'RegisterStep2',
                isShown: false,
                config: {
                    templateUrl: 'app/account/registerStep2.html',
                    controller: 'app.account.RegisterStep2Controller',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/Settings',
                title: 'Settings',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/settings.html',
                    controller: 'app.admin.SettingsController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Quality Manager"]
            },
            {
                url: '/Account/PasswordReminder',
                title: 'Password Reminder',
                isShown: false,
                config: {
                    templateUrl: 'app/account/reminder.html',
                    controller: 'app.account.ReminderController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Account/PasswordReset',
                title: 'Password Reset',
                isShown: false,
                config: {
                    templateUrl: 'app/account/passwordReset.html',
                    controller: 'app.account.ResetController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/AuditorObserverManagement',
                title: 'Auditor/Observer Management',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/auditorObserverManagement.html',
                    controller: 'app.admin.AuditorObserverManagementController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "Quality Manager", "FACT Quality Manager"]
            },
            {
                url: '/Admin/OrgFacilityView',
                title: 'Org To Facility View',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/orgfacilityview.html',
                    controller: 'app.admin.OrgFacilityViewController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Quality Manager", "FACT Coordinator"]
            },
            {
                url: '/AppManagement',
                config: {
                    templateUrl: 'app/application/applicationManagement.html',
                    controller: 'app.application.ApplicationManagementController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Quality Manager", "FACT Coordinator"]
            },
            {
                url: '/Admin/Templates',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/template.html',
                    controller: 'app.admin.TemplateController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator","FACT Quality Manager"]
            },
            {
                url: '/Compliance/ManageCompliance',
                isShown: true,
                config: {
                    templateUrl: 'app/compliance/manageCompliance.html',
                    controller: 'app.compliance.ManageComplianceController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator","FACT Quality Manager", "FACT Coordinator"]
            },
            {
                url: '/Admin/RequirementManagement',
                title: 'Requirements Management',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/requirementManagement.html',
                    controller: 'app.admin.RequirementManagementController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator","Primary Contact", "FACT Quality Manager"]
            },
            {
                url: '/Admin/Requirement/Import',
                title: 'Requirements Import',
                isShown: true,
                config: {
                    templateUrl: 'app/requirement/import.html',
                    controller: 'app.requirement.ImportController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Quality Manager"]
            },
            {
                url: '/Admin/ModuleManagement',
                title: 'Module Management',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/moduleManagement.html',
                    controller: 'app.admin.ModuleManagementController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Quality Manager"]
            },
            {
                url: '/Overview',
                title: 'Overview',
                isShown: true,
                config: {
                    templateUrl: 'app/eligibility/home.html',
                    controller: 'app.eligibility.HomeController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Eligibility/Application',
                title: 'Application',
                isShown: true,
                config: {
                    templateUrl: 'app/eligibility/application.html',
                    controller: 'app.eligibility.ApplicationController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Quality Manager"]
            },
            {
                url: '/Renewal/Application',
                title: 'Application',
                isShown: true,
                config: {
                    templateUrl: 'app/renewal/application.html',
                    controller: 'app.renewal.ApplicationController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Annual/Application',
                title: 'Application',
                isShown: true,
                config: {
                    templateUrl: 'app/annual/application.html',
                    controller: 'app.annual.ApplicationController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/documentLibrary',
                title: 'Document Library',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/documentLibrary.html',
                    controller: 'app.admin.DocumentLibraryController',
                    controllerAs: 'vm'
                },
                roles: ["Primary Contact", "FACT Consultant Coordinator"]
            },
            {
                url: '/Reviewer/View',
                title: 'Application Answers Review',
                isShown: true,
                config: {
                    templateUrl: 'app/reviewer/applicaitonAnswersReview.html',
                    controller: 'app.reviewer.ApplicationAnswersReviewController',
                    controllerAs: 'vm'
                },
                roles: ["Primary Contact", "FACT Administrator", "User", "FACT Quality Manager"]
            },                  
            {
                url: '/Eligibility/Staff',
                title: 'Staff',
                isShown: true,
                config: {
                    templateUrl: 'app/eligibility/home.html',
                    controller: 'app.eligibility.HomeController',
                    controllerAs: 'vm'
                },
                roles: ["Primary Contact", "FACT Consultant Coordinator"]
            },
            {
                url: '/Eligibility/Documents',
                title: 'Documents',
                isShown: true,
                config: {
                    templateUrl: 'app/eligibility/home.html',
                    controller: 'app.eligibility.HomeController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Eligibility/Signatures',
                title: 'Signatures',
                isShown: true,
                config: {
                    templateUrl: 'app/eligibility/home.html',
                    controller: 'app.eligibility.HomeController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Eligibility/History',
                title: 'History',
                isShown: true,
                config: {
                    templateUrl: 'app/eligibility/home.html',
                    controller: 'app.eligibility.HomeController',
                    controllerAs: 'vm'
                },
                roles: ["Primary Contact", "FACT Consultant Coordinator"]
            },
            {
                url: '/Admin/organizationFacility',
                title: 'Organization Facility',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/organizationFacility.html',
                    controller: 'app.admin.OrganizationFacilityController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/documentLibrary',
                title: 'Document Library',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/documentLibrary.html',
                    controller: 'app.admin.DocumentLibraryController',
                    controllerAs: 'vm'
                }
            },
            //{
            //    url: '/Admin/facilitySite',
            //    title: 'Facility Site',
            //    isShown: true,
            //    config: {
            //        templateUrl: 'app/admin/facilitySite.html',
            //        controller: 'app.admin.FacilitySiteController',
            //        controllerAs: 'vm'
            //    }
            //},
            {
                url: '/InspectionScheduler',
                title: 'Inspection Scheduler',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/inspectionSchedule.html',
                    controller: 'app.admin.InspectionScheduleController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Coordinator", "FACT Quality Manager"]
            },
            {
                url: '/Admin/auditLog',
                title: 'Audit Log',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/auditLog.html',
                    controller: 'app.admin.AuditLogController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/manageOrganization',
                title: 'Manage Organization',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/manageOrganization.html',
                    controller: 'app.admin.ManageOrganizationController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/scopeType',
                title: 'Scope Types',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/scopeType.html',
                    controller: 'app.admin.ScopeTypeController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/applicationStatus',
                title: 'Application Status',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/applicationStatus.html',
                    controller: 'app.admin.ApplicationStatusController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/applicationResponseStatus',
                title: 'Application Response Status',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/applicationResponseStatus.html',
                    controller: 'app.admin.ApplicationResponseStatusController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/facilityDemography',
                title: 'Facility Demography',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/facilityDemography.html',
                    controller: 'app.admin.FacilityDemographyController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/manageUsers',
                title: 'Manage Users',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/manageUsers.html',
                    controller: 'app.admin.ManageUsersController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/complianceApplicationApproval',
                title: 'Compliance Application Approval',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/complianceApplicationApproval.html',
                    controller: 'app.admin.ComplianceApplicationApproval',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/AccreditationOutcome',
                title: 'Accreditation Outcome',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/accreditationOutcome.html',
                    controller: 'app.admin.AccreditationOutcomeController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/OrganizationConsultant',
                title: 'Organization Consultants',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/organizationConsultant.html',
                    controller: 'app.admin.OrganizationConsultantController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/rfiView',
                title: 'RFI View',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/rfiView.html',
                    controller: 'app.admin.RFIViewController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Inspector/Overview',
                title: 'Inspection',
                isShown: true,
                config: {
                    templateUrl: 'app/inspector/overview.html',
                    controller: 'app.inspector.OverviewController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Admin/manageSites',
                title: 'Manage Sites',
                isShown: true,
                config: {
                    templateUrl: 'app/admin/manageSites.html',
                    controller: 'app.admin.ManageSitesController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/Reporting',
                title: 'Reporting',
                isShown: true,
                config: {
                    templateUrl: 'app/reporting/index.html',
                    controller: 'app.reporting.IndexController',
                    controllerAs: 'vm'
                }
            },
            {
                url: '/ScheduleInspection',
                title: 'Schedule Inspection',
                isShown: true,
                config: {
                    templateUrl: 'app/inspection/schedule.html',
                    controller: 'app.inspection.ScheduleController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", "FACT Quality Manager"]
            },
            {
                url: '/Admin/Impersonate',
                title: 'Impersonation',
                isShown: false,
                config: {
                    templateUrl: 'app/admin/impersonate.html',
                    controller: 'app.admin.ImpersonateController',
                    controllerAs: 'vm'
                },
                roles: ["FACT Administrator", , "FACT Coordinator", "FACT Quality Manager" ]
            },
            {
                url: '/Application/Report',
                title: 'ApplicationReport',
                isShown: false,
                config: {
                    templateUrl: 'app/application/report.html',
                    controller: 'app.application.ApplicationReportController',
                    controllerAs: 'vm'
                }
            }
        ];
    }
})();