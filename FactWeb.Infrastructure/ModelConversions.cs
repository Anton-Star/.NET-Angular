using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using static FactWeb.Infrastructure.Constants;

namespace FactWeb.Infrastructure
{
    class LocalDocument
    {
        public int ResponseId { get; set; }
        public DocumentItem Document { get; set; }
    }

    public static class ModelConversions
    {

        #region Facility

        public static List<FacilityItems> Convert(List<Facility> facilityList)
        {
            return facilityList == null ? null : facilityList.Select(x => Convert(x)).ToList();
        }

        public static FacilityItems Convert(Facility facility, bool includeChildData = true, bool includeServiceType = true)
        {
            var fac = new FacilityItems
            {
                FacilityId = facility.Id,
                FacilityName = facility.Name,
                FacilityNumber = facility.FacilityNumber,
                isActive = facility.IsActive,
                FacilityDirectorId = facility.FacilityDirectorId,
                PrimaryOrganizationId = facility.PrimaryOrganizationId,
                PrimaryOrganizationName = facility.PrimaryOrganization == null ? string.Empty : facility.PrimaryOrganization.Name,
                OtherSiteAccreditationDetails = facility.OtherSiteAccreditationDetails,
                MaxtrixMax = facility.MaxtrixMax == null ? "" : facility.MaxtrixMax,
                QMRestrictions = facility.QMRestrictions,
                NetCordMembership = facility.NetCordMembership,
                HRSA = facility.HRSA,
                MasterServiceTypeId = facility.MasterServiceTypeId,
                //FacilityAccreditationId = facility.FacilityAccreditationId,
                ServiceTypeId = facility.ServiceTypeId,
                NetcordMembershipTypeId = facility.NetcordMembershipTypeId,
                ProvisionalMembershipDate = facility.ProvisionalMembershipDate != null ? facility.ProvisionalMembershipDate.Value.ToShortDateString() : "",
                AssociateMembershipDate = facility.AssociateMembershipDate != null ? facility.AssociateMembershipDate.Value.ToShortDateString() : "",
                FullMembershipDate = facility.FullMembershipDate != null ? facility.FullMembershipDate.Value.ToShortDateString() : ""
            };

            if (includeServiceType)
            {
                fac.ServiceType = Convert(facility.ServiceType);
            }

            if (includeChildData)
            {
                fac.Sites = facility.FacilitySites == null ? null : ConvertToSiteItems(facility.FacilitySites.ToList());
                fac.FacilityAccreditation = facility.FacilityAccreditationMapping == null
                    ? null
                    : Convert(facility.FacilityAccreditationMapping.Select(x => x.FacilityAccreditation).ToList());
                fac.FacilityDirector = Convert(facility.FacilityDirector, false);
                fac.PrimaryOrganization = Convert(facility.PrimaryOrganization, false, false);
                fac.MasterServiceType = Convert(facility.MasterServiceType);
                fac.SiteTotals = ConvertToSiteTotal(facility);
            }

            return fac;

        }

        public static FacilityItems ConvertIncludeChild(Facility facility, bool includeChildData = true)
        {
            var fac = new FacilityItems
            {
                FacilityId = facility.Id,
                FacilityName = facility.Name,
                FacilityNumber = facility.FacilityNumber,
                isActive = facility.IsActive,
                FacilityDirectorId = facility.FacilityDirectorId,
                PrimaryOrganizationId = facility.PrimaryOrganizationId,
                PrimaryOrganizationName = facility.PrimaryOrganization == null ? string.Empty : facility.PrimaryOrganization.Name,
                OtherSiteAccreditationDetails = facility.OtherSiteAccreditationDetails,
                MaxtrixMax = facility.MaxtrixMax == null ? "" : facility.MaxtrixMax,
                QMRestrictions = facility.QMRestrictions,
                NetCordMembership = facility.NetCordMembership,
                NetcordMembershipTypeId = facility.NetcordMembershipTypeId,
                ProvisionalMembershipDate = facility.ProvisionalMembershipDate != null ? facility.ProvisionalMembershipDate.Value.ToShortDateString() : "",
                AssociateMembershipDate = facility.AssociateMembershipDate != null ? facility.AssociateMembershipDate.Value.ToShortDateString() : "",
                FullMembershipDate = facility.FullMembershipDate != null ? facility.FullMembershipDate.Value.ToShortDateString() : "",
                HRSA = facility.HRSA,
                MasterServiceTypeId = facility.MasterServiceTypeId,
                //FacilityAccreditationId = facility.FacilityAccreditationId,
                ServiceTypeId = facility.ServiceTypeId,

            };

            if (includeChildData)
            {
                fac.Sites = facility.FacilitySites == null ? null : ConvertToSiteItemsWithData(facility.FacilitySites.ToList());
                fac.FacilityAccreditation = facility.FacilityAccreditationMapping == null
                    ? null
                    : Convert(facility.FacilityAccreditationMapping.Select(x => x.FacilityAccreditation).ToList());
                fac.ServiceType = Convert(facility.ServiceType);
                fac.FacilityDirector = Convert(facility.FacilityDirector, false);
                fac.PrimaryOrganization = Convert(facility.PrimaryOrganization, false);
                fac.MasterServiceType = Convert(facility.MasterServiceType);
                fac.SiteTotals = ConvertToSiteTotal(facility);
            }

            return fac;

        }

        #endregion


        public static List<OrganizationConsultantItem> Convert(List<OrganizationConsutant> organizationConsutantList)
        {
            return organizationConsutantList == null ? null : organizationConsutantList.Select(Convert).ToList();
        }

        public static OrganizationConsultantItem Convert(OrganizationConsutant organizationConsultant)
        {
            if (organizationConsultant == null)
                return null;

            return new OrganizationConsultantItem
            {
                OrganizationConsultantId = organizationConsultant.Id,
                OrganizationId = organizationConsultant.Organization.Id,
                OrganizationName = organizationConsultant.Organization.Name,
                StartDate = organizationConsultant.StartDate == null ? "" : organizationConsultant.StartDate.Value.ToShortDateString(),
                EndDate = organizationConsultant.EndDate == null ? "" : organizationConsultant.EndDate.Value.ToShortDateString(),
                ConsultantId = organizationConsultant.User.Id.ToString(),
                ConsultantName = organizationConsultant.User.FirstName + " " + organizationConsultant.User.LastName,
                User = new UserItem
                {
                    UserId = organizationConsultant.User.Id,
                    FirstName = organizationConsultant.User.FirstName,
                    LastName = organizationConsultant.User.LastName,
                    EmailAddress = organizationConsultant.User.EmailAddress,
                    Role = Convert(organizationConsultant.User.Role),
                    JobFunctions = organizationConsultant.User.UserJobFunctions.Select(x => Convert(x.JobFunction)).ToList(),
                    Credentials = organizationConsultant.User.UserCredentials.Select(x => Convert(x.Credential)).ToList()
                }
            };
        }

        public static List<SiteTotalItem> ConvertToSiteTotal(Facility facility)
        {
            if (facility.FacilitySites == null)
                return null;

            int processingSiteCounter = 0;
            int sitesInPatient = 0;
            int sitesOutPatient = 0;
            int siteFixedCount = 0;
            int siteNonFixedCount = 0;
            int siteFixedNonFixedCount = 0;

            var sites = facility.FacilitySites.Select(x => x.Site.SiteClinicalTypes).ToList();
            var apheresisCollectionCount = facility.FacilitySites.Select(x => x.Site).Where(y => y.CollectionProductTypeId == 1).Count();
            var marrowCollectionCount = facility.FacilitySites.Select(x => x.Site).Where(y => y.CollectionProductTypeId == 2).Count();


            foreach (var site in facility.FacilitySites.Select(x => x.Site))
            {
                if (site.SiteProcessingTypes != null && site.SiteProcessingTypes.Count() > 0)
                    processingSiteCounter++;

                foreach (var siteClinicalType in site.SiteClinicalTypes)
                {
                    if (siteClinicalType.ClinicalTypeId == 1)
                        sitesInPatient++;
                    else if (siteClinicalType.ClinicalTypeId == 2)
                        sitesOutPatient++;
                }

                if (site.CBCollectionTypeId == 1)
                    siteFixedCount++;
                else if (site.CBCollectionTypeId == 2)
                    siteNonFixedCount++;
                else if (site.CBCollectionTypeId == 3)
                {
                    siteFixedNonFixedCount++;
                }
            }

            List<SiteTotalItem> listSiteTotal = new List<SiteTotalItem>();

            if (sites.Count() > 0)
            {
                listSiteTotal.Add(new SiteTotalItem
                {
                    InPatient = sitesInPatient,
                    OutPatient = sitesOutPatient,
                    MarrowCollection = marrowCollectionCount,
                    ApheresisCollection = apheresisCollectionCount,
                    Processing = processingSiteCounter,
                    Fixed = siteFixedCount,
                    NonFixed = siteNonFixedCount,
                    FixedNonFixed = siteFixedNonFixedCount
                });
                return listSiteTotal;
            }
            return listSiteTotal;
        }

        public static ComplianceApplicationApprovalStatusItem Convert(ComplianceApplicationApprovalStatus item)
        {
            if (item == null) return null;

            return new ComplianceApplicationApprovalStatusItem
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        public static MasterServiceTypeItem Convert(MasterServiceType masterServiceType)
        {
            if (masterServiceType == null)
                return null;

            return new MasterServiceTypeItem
            {
                Id = masterServiceType.Id,
                Name = masterServiceType.Name,
                ShortName = masterServiceType.ShortName
            };
        }
        public static List<FacilityAccreditationItem> Convert(List<FacilityAccreditation> facilityAccreditations)
        {
            return facilityAccreditations == null ? null : facilityAccreditations.Select(Convert).ToList();
        }

        public static FacilityAccreditationItem Convert(FacilityAccreditation facilityAccreditation)
        {
            if (facilityAccreditation == null)
                return null;

            return new FacilityAccreditationItem
            {
                Id = facilityAccreditation.Id,
                Name = facilityAccreditation.Name
            };
        }

        public static ServiceTypeItem Convert(ServiceType serviceType)
        {
            if (serviceType == null)
                return null;

            return new ServiceTypeItem
            {
                Id = serviceType.Id,
                Name = serviceType.Name,
                MasterServiceTypeId = serviceType.MasterServiceTypeId,
                MasterServiceTypeItem = Convert(serviceType.MasterServiceType)
            };
        }

        //

        //public static List<FacilityItems> Convert(List<OrganizationFacilityModel> facilityList)
        //{
        //    return facilityList == null ? null : facilityList.Select(Convert).ToList();
        //}

        //public static FacilityItems Convert(Facility facility)
        //{
        //    return new FacilityItems
        //    {
        //        FacilityId = facility.Id,
        //        FacilityName = facility.Name
        //    };
        //}

        #region Organziation
        public static OrganizationItem Convert(Organization org, bool includeFacilities = true, bool includeAll = true)
        {
            if (org == null) return null;

            var organization = new OrganizationItem
            {
                OrganizationId = org.Id,
                OrganizationNumber = org.Number,
                OrganizationName = org.Name,
                OrganizationTypeItem = Convert(org.OrganizationType),
                OrganizationPhone = org.Phone,
                OrganizationPhoneExt = org.PhoneExt,
                OrganizationPhoneUS = org.PhoneUS,
                OrganizationPhoneUSExt = org.PhoneUSExt,
                OrganizationFax = org.Fax,
                OrganizationFaxExt = org.FaxExt,
                OrganizationFaxUS = org.FaxUS,
                OrganizationFaxUSExt = org.FaxExt,
                OrganizationEmail = org.Email,
                OrganizationWebSite = org.WebSite,
                BAAExecutionDate = org.BAAExecutionDate.ToString(),
                BAADocumentVersion = org.BAADocumentVersion,
                BaaDocumentPath = org.BaaDocumentPath,
                BAAVerifiedDate = org.BAAVerifiedDate.ToString(),
                AccreditationDate = org.AccreditationDate?.ToShortDateString() ?? string.Empty,
                AccreditationExpirationDate = org.AccreditationExpirationDate?.ToShortDateString() ?? string.Empty,
                AccreditationExtensionDate = org.AccreditationExtensionDate?.ToShortDateString() ?? string.Empty,
                AccreditedSince = org.AccreditedSince?.ToShortDateString() ?? string.Empty,
                Comments = org.Comments,
                Description = org.Description,
                SpatialRelationship = org.SpatialRelationship,
                DocumentLibraryVaultId = org.DocumentLibraryVaultId,
                DocumentLibraryGroupId = org.DocumentLibraryGroupId,
                UseTwoYearCycle = org.UseTwoYearCycle.GetValueOrDefault(),
                CcEmailAddresses = org.CcEmailAddresses
            };

            if (includeFacilities)
            {
                organization.Facilities = org.OrganizationFacilities?.Select(Convert).ToList() ??
                                          new List<OrganizationFacilityItems>();
                organization.FacilityItems = ConvertToFacilityItems(org, false);                
            }

            organization.OrganizationDirectors =
                    org.Users.Where(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector)
                        .Select(x => Convert(x.User, false))
                        .ToList();

            if (includeAll)
            {
                
                organization.PrimaryUser = Convert(org.PrimaryUser, false);
                organization.AccreditationStatusItem = Convert(org.AccreditationStatus);
                organization.BAAOwnerItem = Convert(org.BAAOwner);
                if (org.OrganizationAccreditationCycles == null) return organization;

                var cycle = org.OrganizationAccreditationCycles.SingleOrDefault(x => x.IsCurrent);

                if (cycle != null)
                {
                    organization.CycleNumber = cycle.Number;
                    organization.CycleEffectiveDate = cycle.EffectiveDate;
                }

                if (organization.Facilities != null)
                {
                    organization.FacilityDirectors =
                        org.OrganizationFacilities?
                            .Where(
                                x =>
                                    x.Facility.FacilityDirector != null &&
                                    organization.OrganizationDirectors.All(y=>y.UserId != x.Facility.FacilityDirectorId) &&
                                    x.Facility.FacilityDirectorId != org.PrimaryUserId)
                            .Select(x => x.Facility.FacilityDirector?.EmailAddress).Distinct().ToList();
                }

                
            }
            else
            {
                if (org.PrimaryUserId.HasValue)
                {
                    organization.PrimaryUser = Convert(new User { Id = org.PrimaryUserId.Value }, false);
                }

                if (org.AccreditationStatusId.HasValue)
                {
                    organization.AccreditationStatusItem = Convert(new AccreditationStatus { Id = org.AccreditationStatusId.Value });
                }

                if (org.BAAOwnerId.HasValue)
                {
                    organization.BAAOwnerItem = Convert(new BAAOwner { Id = org.BAAOwnerId.Value });
                }
            }



            return organization;
        }

        //public static OrganizationNetcordMembershipItem Convert(OrganizationNetcordMembership item)
        //{
        //    if (item == null) return null;

        //    return new OrganizationNetcordMembershipItem
        //    {
        //        Id = item.Id,
        //        IsCurrent = item.IsCurrent,
        //        MembershipDate = item.MembershipDate,
        //        NetcordMembershipType = Convert(item.NetcordMembershipType)
        //    };
        //}

        public static NetcordMembershipTypeItem Convert(NetcordMembershipType item)
        {
            if (item == null) return null;

            return new NetcordMembershipTypeItem
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        public static OrganizationBAADocumentItem Convert(OrganizationBAADocument item)
        {
            if (item == null) return null;

            return new OrganizationBAADocumentItem
            {
                Id = item.Id,
                DocumentId = item.DocumentId,
                OrganizationId = item.OrganizationId,
                DocumentItem = Convert(item.Document, false)
            };
        }

        public static List<FacilityItems> ConvertToFacilityItems(Organization organization, bool includeChildData = true)
        {
            if (organization.OrganizationFacilities == null) return new List<FacilityItems>();

            var facilities = organization.OrganizationFacilities.Select(x => x.Facility);

            List<FacilityItems> listFacilityItems = new List<FacilityItems>();

            foreach (Facility facility in facilities)
            {
                var item = new FacilityItems
                {
                    FacilityId = facility.Id,
                    FacilityName = facility.Name,
                    FacilityNumber = facility.FacilityNumber,
                    isActive = facility.IsActive,
                    FacilityDirectorId = facility.FacilityDirectorId,
                    PrimaryOrganizationId = facility.PrimaryOrganizationId,
                    PrimaryOrganizationName = facility.PrimaryOrganization == null ? string.Empty : facility.PrimaryOrganization.Name,
                    OtherSiteAccreditationDetails = facility.OtherSiteAccreditationDetails,
                    MaxtrixMax = facility.MaxtrixMax,
                    QMRestrictions = facility.QMRestrictions,
                    NetCordMembership = facility.NetCordMembership,
                    HRSA = facility.HRSA,
                    MasterServiceTypeId = facility.MasterServiceTypeId,
                    ServiceTypeId = facility.ServiceTypeId,
                    Sites = ConvertToSiteItems(facility.FacilitySites.ToList()),

                    ServiceType = Convert(facility.ServiceType),
                    //FacilityDirector = Convert(facility.FacilityDirector),
                    MasterServiceType = Convert(facility.MasterServiceType)
                };

                if (includeChildData)
                {
                    item.FacilityAccreditation =
                        Convert(facility.FacilityAccreditationMapping.Select(x => x.FacilityAccreditation).ToList());

                    item.SiteTotals = ConvertToSiteTotal(facility);
                }

                listFacilityItems.Add(item);
            }

            return listFacilityItems;
        }

        public static OrganizationTypeItem Convert(OrganizationType organizationType)
        {
            if (organizationType == null) return null;

            return new OrganizationTypeItem
            {
                OrganizationTypeId = organizationType.Id,
                OrganizationTypeName = organizationType.Name
            };
        }
        #endregion

        public static List<SiteItems> ConvertToSiteItems(List<FacilitySite> facilitySites)
        {
            List<SiteItems> siteItemsList = new List<SiteItems>();

            foreach (var facilitySite in facilitySites)
            {
                siteItemsList.Add(Convert(facilitySite.Site, false));
            }
            return siteItemsList;
        }

        public static List<SiteItems> ConvertToSiteItemsWithData(List<FacilitySite> facilitySites)
        {
            List<SiteItems> siteItemsList = new List<SiteItems>();

            foreach (var facilitySite in facilitySites)
            {
                siteItemsList.Add(Convert(facilitySite.Site, true, false));
            }
            return siteItemsList;
        }

        public static List<FacilitySiteItems> ConvertToFacilitySites(List<OrganizationFacility> organizationFacilityList)
        {
            if (organizationFacilityList == null || organizationFacilityList.Count() == 0)
                return null;

            List<FacilitySiteItems> facilitySiteItemsList = new List<FacilitySiteItems>();

            foreach (var organizationFacility in organizationFacilityList)
            {
                var siteList = organizationFacility.Facility.FacilitySites.Select(x => x.Site).ToList();

                foreach (var site in siteList)
                {
                    var facilitySiteItem = new FacilitySiteItems();

                    facilitySiteItem.OrganizationId = organizationFacility.Organization.Id;
                    facilitySiteItem.OrganizationName = organizationFacility.Organization.Name;
                    facilitySiteItem.FacilityId = organizationFacility.Facility.Id;
                    facilitySiteItem.FacilityName = organizationFacility.Facility.Name;
                    facilitySiteItem.Relation = organizationFacility.StrongRelation == true ? "Strong" : "Weak";
                    facilitySiteItem.SiteId = site.Id;
                    facilitySiteItem.SiteName = site.Name;

                    facilitySiteItemsList.Add(facilitySiteItem);
                }
            }
            return facilitySiteItemsList;
        }

        public static List<FacilitySiteItems> ConvertToFacilitySites(List<OrganizationFacility> organizationFacilityList, List<InspectionScheduleSite> inspectionScheduleSiteList)
        {
            if (inspectionScheduleSiteList == null || inspectionScheduleSiteList.Count() == 0)
                return null;

            List<FacilitySiteItems> facilitySiteItemsList = new List<FacilitySiteItems>();

            foreach (var organizationFacility in organizationFacilityList)
            {
                var siteList = organizationFacility.Facility.FacilitySites.Select(x => x.Site).ToList();

                foreach (var site in siteList)
                {
                    var inspectionScheduleSite = inspectionScheduleSiteList.Where(x => x.SiteID == site.Id).ToList();

                    if (inspectionScheduleSite.Count() > 0)
                    {
                        var facilitySiteItem = new FacilitySiteItems();

                        facilitySiteItem.OrganizationId = organizationFacility.Organization.Id;
                        facilitySiteItem.OrganizationName = organizationFacility.Organization.Name;
                        facilitySiteItem.FacilityId = organizationFacility.Facility.Id;
                        facilitySiteItem.FacilityName = organizationFacility.Facility.Name;
                        facilitySiteItem.InspectionDate = inspectionScheduleSite[0].InspectionDate.ToShortDateString();
                        facilitySiteItem.Relation = organizationFacility.StrongRelation == true ? "Strong" : "Weak";
                        facilitySiteItem.SiteId = site.Id;
                        facilitySiteItem.SiteName = site.Name;

                        facilitySiteItemsList.Add(facilitySiteItem);
                    }
                }
            }
            return facilitySiteItemsList;
        }


        #region User
        public static List<UserItem> Convert(List<User> userList, bool includeOrganizations)
        {
            return userList == null ? null : userList.Select(x => ModelConversions.Convert(x, true, includeOrganizations)).ToList();
        }

        public static List<UserItem> Convert(List<User> userList)
        {
            return userList == null ? null : userList.Select(x => ModelConversions.Convert(x, true)).ToList();
        }

        public static UserItem Convert(User user, bool includeChildData = true, bool includeOrganizations = false, bool includeCreds = false)
        {
            if (user == null) return null;

            var userItem = new UserItem
            {
                UserId = user.Id,
                EmailAddress = user.RoleId == (int)Constants.Role.NonSystemUser ? null : user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = string.Format("{0}, {1}", user.LastName, user.FirstName),
                PreferredPhoneNumber = user.PreferredPhoneNumber,
                Extension = user.PhoneExtension,
                WorkPhoneNumber = user.WorkPhoneNumber,
                IsLocked = user.IsLocked,
                IsActive = user.IsActive,
                WebPhotoPath = user.WebPhotoPath,
                EmailOptOut = user.EmailOptOut ?? false,
                MailOptOut = user.MailOptOut ?? false,
                ResumePath = user.ResumePath,
                StatementOfCompliancePath = user.StatementOfCompliancePath,
                AgreedToPolicyDate = user.AgreedToPolicyDate,
                AnnualProfessionHistoryFormPath = user.AnnualProfessionHistoryFormPath,
                MedicalLicensePath = user.MedicalLicensePath,
                MedicalLicenseExpiry = user.MedicalLicenseExpiry,
                CompletedStep2 = user.CompletedStep2 ?? false,
                IsAuditor = user.IsAuditor.GetValueOrDefault(),
                IsObserver = user.IsObserver.GetValueOrDefault(),
                UserType = user.IsAuditor.GetValueOrDefault() ? "Auditor" : user.IsObserver.GetValueOrDefault() ? "Observer" : string.Empty,
                CanManageUsers = user.CanManageUsers.GetValueOrDefault(),
                DocumentLibraryUserId = user.DocumentLibraryUserId,
                Title = user.Title
            };

            if (includeChildData)
            {
                userItem.Role = Convert(user.Role);
                userItem.JobFunctions = user.UserJobFunctions?.Select(x => Convert(x.JobFunction)).ToList();
                userItem.Languages = user.UserLanguages?.Select(x => Convert(x.Language)).ToList();
                userItem.Memberships =
                    user.UserMemberships?.Select(
                        x =>
                            new UserMembershipItem
                            {
                                Membership = Convert(x.Membership),
                                MembershipNumber = x.MembershipNumber
                            }).ToList();
            }
            else
            {
                userItem.Role = new RoleItem { RoleId = user.RoleId };
            }

            if (includeChildData || includeCreds)
            {
                userItem.Credentials = user.UserCredentials?.Select(x => Convert(x.Credential)).ToList();
            }

            if (includeOrganizations)
            {
                userItem.Organizations = user.Organizations.Select(Convert).ToList();
            }

            return userItem;
        }

        #endregion

        public static UserOrganizationItem Convert(UserOrganization item)
        {
            if (item == null) return null;

            return new UserOrganizationItem
            {
                Organization = Convert(item.Organization, false, false),
                JobFunction = Convert(item.JobFunction),
                ShowOnAccReport = item.ShowOnAccReport
            };
        }

        #region Role

        public static List<RoleItem> Convert(List<Role> role)
        {
            return role == null ? null : role.Select(Convert).ToList();
        }

        public static RoleItem Convert(Role role)
        {
            if (role == null) return null;
            return new RoleItem
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
        }
        #endregion

        #region Organzation Facility

        public static List<OrganizationFacilityItems> Convert(List<OrganizationFacility> organizationFacilityList)
        {
            return organizationFacilityList == null ? null : organizationFacilityList.Select(Convert).ToList();
        }

        public static OrganizationFacilityItems Convert(OrganizationFacility item)
        {
            return new OrganizationFacilityItems
            {
                OrganizationFacilityId = item.Id,
                OrganizationId = item.Organization?.Id ?? 0,
                OrganizationName = item.Organization?.Name,
                FacilityId = item.Facility.Id,
                FacilityName = item.Facility.Name,
                Relation = item.StrongRelation == true ? "Strong" : "Weak",
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedDate.ToString(),
                MasterServiceTypeName = item.Facility.MasterServiceType?.Name,
                ServiceTypeName = item.Facility.ServiceType?.Name
            };
        }

        #endregion

        #region Inspection Scheduler

        public static InspectionScheduleItem Convert(InspectionSchedule inspectionSchedule)
        {
            return new InspectionScheduleItem
            {
                ApplicationTypeName = inspectionSchedule.Applications.ApplicationType.Name,
                ApplicationTypeId = inspectionSchedule.Applications.ApplicationTypeId.ToString(),
                CompletedDate = inspectionSchedule.CompletionDate.ToString(),
                InspectionDate = inspectionSchedule.InspectionDate.ToString(),
                InspectionScheduleId = inspectionSchedule.Id.ToString(),
                IsCompleted = inspectionSchedule.IsCompleted == true ? "Yes" : "No",
                OrganizationId = inspectionSchedule.Organizations.Id.ToString(),
                OrganizationName = inspectionSchedule.Organizations.Name,
                IsActive = inspectionSchedule.IsActive.ToString(),
                StartDate = inspectionSchedule.StartDate.ToShortDateString(),
                EndDate = inspectionSchedule.EndDate.ToShortDateString(),
                SiteId = inspectionSchedule.Applications.Site.Id,
                SiteName = inspectionSchedule.Applications.Site.Name
            };
        }

        public static List<InspectionScheduleItem> Convert(List<InspectionSchedule> inspectionScheduleList)
        {
            return inspectionScheduleList == null ? null : inspectionScheduleList.Select(Convert).ToList();
        }

        public static List<ApplicationTypeItem> Convert(List<ApplicationType> applicationType)
        {
            return applicationType == null ? null : applicationType.Select(Convert).ToList();
        }

        public static ApplicationTypeItem Convert(ApplicationType applicationType)
        {
            if (applicationType == null) return null;

            return new ApplicationTypeItem
            {
                ApplicationTypeId = applicationType.Id,
                ApplicationTypeName = applicationType.Name,
                IsManageable = applicationType.IsManageable
            };
        }

        public static List<ApplicationStatusItem> Convert(List<ApplicationStatus> applicationStatus)
        {
            return applicationStatus == null ? null : applicationStatus.Select(Convert).ToList();
        }

        public static ApplicationStatusItem Convert(ApplicationStatus applicationStatus)
        {
            return new ApplicationStatusItem
            {
                Id = applicationStatus.Id,
                Name = applicationStatus.Name,
                NameForApplicant = applicationStatus.NameForApplicant
            };
        }

        public static List<ApplicationStatusHistoryItem> Convert(List<ApplicationStatusHistory> applicationStatusHistory)
        {
            return applicationStatusHistory == null ? null : applicationStatusHistory.Select(Convert).ToList();
        }

        public static ApplicationStatusHistoryItem Convert(ApplicationStatusHistory applicationStatusHistory)
        {
            return new ApplicationStatusHistoryItem
            {
                Id = applicationStatusHistory.Id,
                ApplicationId = applicationStatusHistory.ApplicationId,
                ApplicationStatusOld = Convert(applicationStatusHistory.ApplicationStatusOld),
                ApplicationStatusNew = Convert(applicationStatusHistory.ApplicationStatusNew),
                CreatedBy = applicationStatusHistory.CreatedBy,
                CreatedDate = applicationStatusHistory.CreatedDate.Value.ToShortDateString()
            };
        }

        public static List<InspectionScheduleDetailItems> Convert(List<InspectionScheduleDetail> inspectionScheduleDetailList, bool cibmtrDataEntred = true)
        {
            return inspectionScheduleDetailList == null ? null : inspectionScheduleDetailList.Select(x=>Convert(x, cibmtrDataEntred)).ToList();
        }

        public static InspectionScheduleDetailItems Convert(InspectionScheduleDetail inspectionScheduleDetail, bool cibmtrDataEntred = true)
        {
            return new InspectionScheduleDetailItems
            {
                InspectionScheduleDetailId = inspectionScheduleDetail.Id,
                UserId = "'" + inspectionScheduleDetail.User.Id.ToString() + "'",
                FullName = string.Format("{0}, {1}", inspectionScheduleDetail.User.LastName, inspectionScheduleDetail.User.FirstName),
                RoleId = inspectionScheduleDetail.AccreditationRole.Id,
                RoleName = inspectionScheduleDetail.AccreditationRole.Name,
                InspectionCategoryId = inspectionScheduleDetail.InspectionCategory.Id,
                InspectionCategoryName = inspectionScheduleDetail.InspectionCategory.Name,
                IsLead = inspectionScheduleDetail.IsLead,
                IsMentor = inspectionScheduleDetail.IsMentor,
                SiteId = inspectionScheduleDetail.Site.Id,
                SiteName = inspectionScheduleDetail.Site.Name,
                IsClinical = inspectionScheduleDetail.Site.FacilitySites.Any(x=>x.Facility.ServiceType.Name == Constants.ServiceType.ClinicalProgramCT),
                IsInspectionComplete = inspectionScheduleDetail.IsInspectionComplete.GetValueOrDefault(),
                InspectionCompletionDate = inspectionScheduleDetail.InspectorCompletionDate,
                MentorFeedback = inspectionScheduleDetail.MentorFeedback,
                ReviewedOutcomesData = cibmtrDataEntred && inspectionScheduleDetail.ReviewedOutcomesData.GetValueOrDefault(),
                User = new UserItem
                {
                    UserId = inspectionScheduleDetail.UserId,
                    FirstName = inspectionScheduleDetail.User.FirstName,
                    LastName = inspectionScheduleDetail.User.LastName,
                    EmailAddress = inspectionScheduleDetail.User.EmailAddress
                }
            };
        }

        public static List<AccreditationRoleItem> Convert(List<AccreditationRole> accreditationRole)
        {
            return accreditationRole == null ? null : accreditationRole.Select(Convert).ToList();
        }

        public static AccreditationRoleItem Convert(AccreditationRole accreditationRole)
        {
            if (accreditationRole == null) return null;

            return new AccreditationRoleItem
            {
                AccreditationRoleId = accreditationRole.Id,
                AccreditationRoleName = accreditationRole.Name
            };
        }

        public static List<InspectionCategoryItem> Convert(List<InspectionCategory> inspectionCategory)
        {
            return inspectionCategory == null ? null : inspectionCategory.Select(Convert).ToList();
        }

        public static InspectionCategoryItem Convert(InspectionCategory inspectionCategory)
        {
            return new InspectionCategoryItem
            {
                InspectionCategoryId = inspectionCategory.Id,
                InspectionCategoryName = inspectionCategory.Name
            };
        }

        #endregion

        #region Inspection Schedule Facility
        //public static List<OrganizationFacilityItems> Convert(List<InspectionScheduleFacility> inspectionScheduleFacilityList)
        //{
        //    return inspectionScheduleFacilityList == null ? null : inspectionScheduleFacilityList.Select(Convert).ToList();
        //}

        //public static OrganizationFacilityItems Convert(InspectionScheduleFacility item)
        //{
        //    return new OrganizationFacilityItems
        //    {
        //        FacilityId = item.Facility.FacilityId,
        //        FacilityName = item.Facility.Name,
        //        Relation = item.StrongRelation == true ? "Strong" : "Weak",
        //        CreatedBy = item.CreatedBy,
        //        CreatedOn = item.CreatedDate.ToString()
        //    };
        //}

        #endregion

        #region Audit Log
        public static List<AuditLogItem> Convert(List<AuditLog> auditLogList)
        {
            return auditLogList == null ? null : auditLogList.Select(Convert).ToList();
        }

        public static AuditLogItem Convert(AuditLog auditLogList)
        {
            TimeZoneInfo cst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            return new AuditLogItem
            {
                AuditLogId = auditLogList.Id,
                UserName = auditLogList.UserName,
                IpAddress = auditLogList.IPAddress,
                DateTime = TimeZoneInfo.ConvertTime(auditLogList.Date.DateTime, cst).ToString(),
                Description = auditLogList.Description
            };
        }

        #endregion

        #region Site

        public static SiteItems Convert(Site site, bool includeLowLevelItems = true, bool includeFacilities = false, bool includeSiteAddress = true)
        {
            if (site == null)
            {
                return null;
            }

            SiteAddress sitePrimaryAddress = null;

            var siteItem = new SiteItems
            {
                SiteId = site.Id,
                SiteName = site.Name,
                SiteDescription = site.Description,
                SiteStartDate = site.StartDate.ToShortDateString(),
                SiteIsPrimarySite = site.IsPrimarySite,
                SiteUnitsBanked = site.CBUnitsBanked.GetValueOrDefault(),
                SiteUnitsBankedDate = site.CBUnitsBankDate.ToString(),
                SiteInspectionDate = site.InspectionScheduleSites?.Count > 0 ? site.InspectionScheduleSites.ElementAt(0).InspectionDate.ToShortDateString() : "",
            };

            if (includeSiteAddress)
            {
                sitePrimaryAddress = site.SiteAddresses?.FirstOrDefault(x => x.IsPrimaryAddress == true);

                siteItem.SiteStreetAddress1 = (sitePrimaryAddress == null) ? "" : sitePrimaryAddress.Address.Street1;
                siteItem.SiteStreetAddress2 = (sitePrimaryAddress == null) ? "" : sitePrimaryAddress.Address.Street2;
                siteItem.SiteCity = (sitePrimaryAddress == null) ? "" : sitePrimaryAddress.Address.City;
                siteItem.SiteZip = (sitePrimaryAddress == null) ? "" : sitePrimaryAddress.Address.ZipCode;
                siteItem.SiteProvince = (sitePrimaryAddress == null) ? "" : sitePrimaryAddress.Address.Province;
                siteItem.SitePhone = (sitePrimaryAddress == null) ? "" : sitePrimaryAddress.Address.Phone;
            }

            if (includeLowLevelItems)
            {
                siteItem.SiteState = (sitePrimaryAddress == null) ? null : Convert(sitePrimaryAddress.Address.State);
                siteItem.SiteCountry = (sitePrimaryAddress == null) ? null : Convert(sitePrimaryAddress.Address.Country);
                siteItem.SiteAddresses = site.SiteAddresses == null
                    ? null
                    : Convert(site.SiteAddresses.Where(x => x.IsPrimaryAddress != true).ToList());

                siteItem.ScopeTypes = site.SiteScopeTypes?.Select(x => x.ScopeType).ToList().Select(Convert).ToList();
                siteItem.ClinicalTypes = site.SiteClinicalTypes?.Select(x => x.ClinicalType).ToList().Select(Convert).ToList();
                siteItem.TransplantTypes = site.SiteTransplantTypes?.Select(x => x.TransplantType).ToList().Select(Convert).ToList();
                siteItem.ProcessingTypes = site.SiteProcessingTypes?.Select(x => x.ProcessingType).ToList().Select(Convert).ToList();

                //siteItem.SiteProcessingType = Convert(site.ProcessingType);
                siteItem.SiteCollectionProductType = Convert(site.CollectionProductType);
                siteItem.SiteCBCollectionType = Convert(site.CBCollectionType);
                siteItem.SiteCBUnitType = Convert(site.CBUnitType);
                siteItem.SiteClinicalPopulationType = Convert(site.ClinicalPopulationType);
                //siteItem.SiteAddresses = Convert(site.SiteAddresses.ToList());
                siteItem.SiteFacilityId = site.FacilitySites == null
                    ? 0
                    : site.FacilitySites.Count > 0 ? site.FacilitySites.First().FacilityId : 0;

                if (site.SiteCordBloodTransplantTotals != null)
                {
                    siteItem.SiteCordBloodTransplantTotals = site.SiteCordBloodTransplantTotals.OrderBy(x => x.AsOfDate).Select(Convert).ToList();
                }

                if (site.SiteTransplantTotals != null)
                {
                    siteItem.SiteTransplantTotals =
                        site.SiteTransplantTotals.OrderBy(x => x.StartDate).Select(Convert).ToList();
                }

                if (site.SiteCollectionTotals != null)
                {
                    siteItem.SiteCollectionTotals =
                        site.SiteCollectionTotals.OrderBy(x => x.StartDate).Select(Convert).ToList();
                }

                if (site.SiteProcessingTotals != null)
                {
                    siteItem.SiteProcessingTotals =
                        site.SiteProcessingTotals.OrderBy(x => x.StartDate).Select(Convert).ToList();
                }

                if (site.SiteProcessingMethodologyTotals != null)
                {
                    siteItem.SiteProcessingMethodologyTotals =
                        site.SiteProcessingMethodologyTotals.OrderBy(x => x.StartDate).Select(Convert).ToList();
                }
            }


            if (includeFacilities && site.FacilitySites != null)
            {
                siteItem.Facilities = site.FacilitySites.Select(x => Convert(x.Facility, false)).ToList();
            }

            return siteItem;
        }

        public static List<SiteAddressItem> Convert(List<SiteAddress> siteAddressList)
        {
            return siteAddressList == null ? null : siteAddressList.Select(Convert).ToList();
        }

        public static SiteAddressItem Convert(SiteAddress siteAddress)
        {
            var siteAddressItem = new SiteAddressItem
            {
                Id = siteAddress.Id,
                AddressId = siteAddress.AddressId,
                SiteId = siteAddress.SiteId,
                IsPrimaryAddress = siteAddress.IsPrimaryAddress,

                Address = ModelConversions.Convert(siteAddress.Address)
            };

            siteAddressItem.AddressType = siteAddressItem.Address.AddressType == null ? null : siteAddressItem.Address.AddressType.Name;

            return siteAddressItem;
        }

        public static List<AddressItem> Convert(List<Address> addressList)
        {
            return addressList == null ? null : addressList.Select(Convert).ToList();
        }

        public static AddressItem Convert(Address address)
        {
            return new AddressItem
            {
                Id = address.Id,
                AddressType = Convert(address.AddressType),
                Street1 = address.Street1,
                Street2 = address.Street2,
                City = address.City,
                State = Convert(address.State),
                Phone = address.Phone,
                Province = address.Province,
                ZipCode = address.ZipCode,
                Country = Convert(address.Country),
                Logitude = address.Logitude,
                Latitude = address.Latitude,
                LocalId = address.Id
            };
        }

        public static List<SiteItems> Convert(List<Site> siteList)
        {
            return siteList == null ? null : siteList.Select(x => ModelConversions.Convert(x, true, false)).ToList();
        }
        #endregion

        #region Facility Site

        public static List<FacilitySiteItems> Convert(List<FacilitySite> facilitySiteList)
        {
            return facilitySiteList == null ? null : facilitySiteList.Select(Convert).ToList();
        }

        public static FacilitySiteItems Convert(FacilitySite item)
        {
            return new FacilitySiteItems
            {
                FacilitySiteId = item.Id,
                SiteId = item.Site.Id,
                SiteName = item.Site.Name,
                FacilityId = item.Facility.Id,
                FacilityName = item.Facility.Name,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedDate.ToString()
            };
        }

        #endregion

        //public static SiteApplicationVersionItem Convert(SiteApplicationVersion item)
        //{
        //    var result = new SiteApplicationVersionItem
        //    {
        //        Id = item.Id,
        //        ApplicationVersion = Convert(item.ApplicationVersion)
        //    };

        //    if (item.NotApplicables != null)
        //    {
        //        result.QuestionsNotApplicable = item.NotApplicables.Select(Convert).ToList();
        //    }

        //    return result;
        //}

        //public static QuestionNotApplicable Convert(SiteApplicationVersionQuestionNotApplicable item)
        //{
        //    return new QuestionNotApplicable
        //    {
        //        Id = item.Id,
        //        QuestionId = item.ApplicationSectionQuestionId,
        //        SiteApplicationVersionId = item.SiteApplicationVersionId
        //    };
        //}

        public static QuestionNotApplicable Convert(ApplicationQuestionNotApplicable item)
        {
            return new QuestionNotApplicable
            {
                Id = item.Id,
                QuestionId = item.ApplicationSectionQuestionId,
                ApplicationId = item.ApplicationId
            };
        }

        public static ComplianceApplicationItem Convert(ComplianceApplication item)
        {
            return new ComplianceApplicationItem
            {
                Id = item.Id,
                AccreditationGoal = item.AccreditationGoal,
                ApplicationStatus = item.ApplicationStatus.Name,
                InspectionScope = item.InspectionScope,
                RejectionComments = item.RejectionComments,
                Coordinator = Convert(item.Coordinator),
                Applications = item.Applications.Select(x=>Convert(x, true)).ToList()
            };
        }

        public static QuestionTypeItem Convert(QuestionType item)
        {
            return new QuestionTypeItem
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        public static ApplicationVersionItem Convert(ApplicationVersion version, Guid? appUniqueId, bool onlyPullSectionsForActive, List<ApplicationSection> applicationSections = null, bool includeSections = true)
        {
            var row = new ApplicationVersionItem
            {
                Id = version.Id,
                ApplicationType = Convert(version.ApplicationType),
                IsActive = version.IsActive,
                Title = version.Title,
                VersionNumber = version.VersionNumber
            };

            if (applicationSections != null)
            {
                version.ApplicationSections = applicationSections.Where(x => x.ApplicationVersionId == version.Id).ToList();
            }

            if (includeSections)
            {
                if (onlyPullSectionsForActive)
                {
                    if (version.IsActive)
                    {
                        row.ApplicationSections =
                            version.ApplicationSections.Where(x => x.ParentApplicationSectionId == null && x.IsActive)
                                .OrderBy(x => System.Convert.ToInt32(string.IsNullOrEmpty(x.Order) ? "0" : x.Order))
                                .Select(x => Convert(x, appUniqueId, applicationSections))
                                .ToList();
                    }
                }
                else
                {
                    row.ApplicationSections =
                        version.ApplicationSections.Where(x => x.ParentApplicationSectionId == null && x.IsActive)
                            .OrderBy(x => System.Convert.ToInt32(string.IsNullOrEmpty(x.Order) ? "0" : x.Order))
                            .Select(x => Convert(x, appUniqueId, applicationSections))
                            .ToList();
                }
            }
            

            return row;
        }

        public static ApplicationSectionItem Convert(ApplicationSection sect, Guid? appUniqueId, List<ApplicationSection> applicationSections = null, bool loadChildren = true)
        {
            ApplicationSection section = sect;
            List<ApplicationSection> orderedChildren = null;

            if (applicationSections != null)
            {
                section = applicationSections.SingleOrDefault(x => x.Id == sect.Id) ?? sect;
            }

            if (section.Children != null || applicationSections == null)
            {
                orderedChildren = section.Children?.Where(x => x.IsActive).OrderBy(x => System.Convert.ToInt32(x.Order)).ToList() ?? new List<ApplicationSection>();
            }
            else
            {
                orderedChildren = applicationSections.Where(x => x.ParentApplicationSectionId == section.Id && x.IsActive).OrderBy(x => System.Convert.ToInt32(x.Order)).ToList();
            }
            

            return new ApplicationSectionItem
            {
                Id = section.Id,
                IsActive = section.IsActive,
                Status = Constants.ApplicationSectionStatusView.NotStarted,
                Name = section.Name,
                PartNumber = section.PartNumber,
                ApplicationTypeName = section.ApplicationType != null ? section.ApplicationType.Name : string.Empty,
                IsVariance = section.IsVariance.GetValueOrDefault(),
                Version = section.Version,
                Order = section.Order,
                HelpText = section.HelpText,
                Children = loadChildren ? orderedChildren.Select(x => Convert(x, appUniqueId, applicationSections)).ToList() : null,
                Questions = loadChildren ? section.Questions?.Where(x => x.IsActive).OrderBy(x => x.ComplianceNumber).ThenByDescending(x => System.Convert.ToInt32(x.Order)).Select(x => Convert(x, true)).ToList() : null,
                UniqueIdentifier = section.UniqueIdentifier,
                AppUniqueId = appUniqueId.GetValueOrDefault(),
                ScopeTypes = section.ApplicationSectionScopeTypes?.Where(x => x.IsDefault == true).Select(Convert).ToList()
            };
        }

        public static List<SectionDocument> ConvertToSectionDocumentsSectionWise(ApplicationSection section)
        {
            var responses = new List<LocalDocument>();
            var sites = new Dictionary<int, string>();


            if (section.Questions == null) return new List<SectionDocument>();

            foreach (var response in section.Questions.SelectMany(question => question.ApplicationResponses))
            {
                if (!response.DocumentId.HasValue) continue;

                response.Document.ApplicationId = response.ApplicationId;
                responses.Add(new LocalDocument {
                    ResponseId = response.Id,
                    Document = Convert(response.Document, true, true)
                });

                if (!sites.ContainsKey(response.Id))
                {
                    sites.Add(response.Id, response.Application.Site?.Name);
                }
            }

            ICollection<DocumentAssociationType> types = new List<DocumentAssociationType>
                    {
                        new DocumentAssociationType
                        {
                            AssociationType = new AssociationType
                            {
                                Id = Guid.Parse("bab89bbc-6d89-44dd-a9b6-dc4e8b414f41"),
                                Name = "RFI Response"
                            }
                        }
                    };

            foreach (var comment in section.Questions.SelectMany(question => question.ApplicationResponseComments))
            {
                if (!comment.DocumentId.HasValue && (comment.ApplicationResponseCommentDocuments == null || !comment.ApplicationResponseCommentDocuments.Any())) continue;

                if (comment.DocumentId.HasValue)
                {

                    comment.Document.ApplicationId = comment.ApplicationId;
                    comment.Document.AssociationTypes = types;

                    responses.Add(new LocalDocument
                    {
                        ResponseId = comment.Id,
                        Document = Convert(comment.Document, true, true)
                    });
                }

                if (comment.ApplicationResponseCommentDocuments != null)
                {

                    foreach (var doc in comment.ApplicationResponseCommentDocuments)
                    {
                        doc.Document.ApplicationId = comment.ApplicationId;
                        doc.Document.AssociationTypes = types;
                        responses.Add(new LocalDocument
                        {
                            ResponseId = comment.Id,
                            Document = Convert(doc.Document, true, true)
                        });
                    }
                }

                if (!sites.ContainsKey(comment.Id))
                {
                    sites.Add(comment.Id, comment.Application?.Site?.Name);
                }
            }

            return responses.Select(res => new SectionDocument
                {
                    SiteName = sites[res.ResponseId],
                    Requirement = $"{section.UniqueIdentifier} {section.Name}",
                    Document = res.Document,
                    RequirementId = $"{section.Id}",
                    AppId = res.Document.ApplicationId.ToString()
                })
                .ToList();
        }
        public static List<SectionDocument> ConvertToSectionDocuments(ApplicationSection section)
        {
            var responses = new Dictionary<Guid, DocumentItem>();
            

            if (section.Questions == null) return new List<SectionDocument>();
            //var appId = 0;//need to confirm
            foreach (var response in section.Questions.SelectMany(question => question.ApplicationResponses))
            {
                
                if (response.DocumentId.HasValue)
                {
                   
                    if (!responses.ContainsKey(response.DocumentId.Value))
                    {
                        //appId = response.ApplicationId;
                        //response.Document.ApplicationId = 0;
                        response.Document.ApplicationId = response.ApplicationId;
                        responses.Add(response.DocumentId.Value, Convert(response.Document, true));

                    }
                }
                
            }

            return responses.Values.Select(document => new SectionDocument
            {
                Requirement = $"{section.UniqueIdentifier} {section.Name}",
                Document = document,
                RequirementId = $"{section.Id}",
                //AppId = $"{appId}"
                AppId = document.ApplicationId.ToString()
            })
                .ToList();
        }

        public static ScopeTypeItem Convert(ApplicationSectionScopeType applicationSectionScopeType)
        {
            if (applicationSectionScopeType == null) return null;

            return new ScopeTypeItem
            {
                ScopeTypeId = applicationSectionScopeType.ScopeType.Id,
                ImportName = applicationSectionScopeType.ScopeType.ImportName,
                IsActive = applicationSectionScopeType.ScopeType.IsActive,
                IsArchived = applicationSectionScopeType.ScopeType.IsArchived,
                IsSelected = true,
                Name = applicationSectionScopeType.ScopeType.Name
            };
        }

        public static List<ApplicationResponseCommentItem> Convert(List<ApplicationResponseComment> applicationResponseCommentList)
        {
            return applicationResponseCommentList == null ? null : applicationResponseCommentList.Select(Convert).ToList();
        }

        public static ApplicationResponseCommentItem Convert(ApplicationResponseComment applicationResponseComment)
        {
            if (applicationResponseComment == null) return null;

            return new ApplicationResponseCommentItem
            {
                ApplicationResponseCommentId = applicationResponseComment.Id,
                ApplicationId = applicationResponseComment.ApplicationId,
                QuestionId = applicationResponseComment.QuestionId,
                //RFIComment = applicationResponseComment.RFIComment,
                //CitationComment = applicationResponseComment.CitationComment,
                //CoordinatorComment = applicationResponseComment.CoordinatorComment,
                FromUser = applicationResponseComment.FromUser,
                //ToUser = applicationResponseComment.ToUser,
                DocumentId = applicationResponseComment.DocumentId,
                //Comment = ""+applicationResponseComment.CitationComment+ applicationResponseComment.RFIComment + applicationResponseComment.CoordinatorComment,
                Comment = applicationResponseComment.Comment,
                CommentOverride = applicationResponseComment.CommentOverride,
                OverridenBy = applicationResponseComment.OverridenBy,
                IsOverridden = !string.IsNullOrWhiteSpace(applicationResponseComment.CommentOverride),
                CommentFrom = Convert(applicationResponseComment.CommentFrom, false),
                CommentTo = (applicationResponseComment.CommentFrom.Role.Name == Constants.Roles.User) ? Constants.Roles.FACTAdministrator : Constants.ResponseComments.Applicant,
                Document = Convert(applicationResponseComment.Document),                
                UpdatedBy = applicationResponseComment.UpdatedBy,
                UpdatedDate = applicationResponseComment.UpdatedDate.HasValue ? applicationResponseComment.UpdatedDate.Value.ToShortDateString() : null,
                CreatedBy = applicationResponseComment.CreatedBy,
                CreatedDate = applicationResponseComment.CreatedDate.HasValue ? applicationResponseComment.CreatedDate.Value.ToString() : null,
                CommentType = Convert(applicationResponseComment.CommentType),
                IncludeInReporting = applicationResponseComment.IncludeInReporting.GetValueOrDefault(),
                VisibleToApplicant = applicationResponseComment.VisibleToApplicant.GetValueOrDefault(),
                CommentDocuments = applicationResponseComment.ApplicationResponseCommentDocuments?.Select(x=> new CommentDocument
                {
                    CommentId = x.ApplicationResponseCommentId,
                    DocumentId = x.DocumentId,
                    Name = x.Document.Name,
                    OriginalName = x.Document.OriginalName,
                    RequestValues = x.Document.RequestValues
                }).ToList()
            };
        }

        public static List<ApplicationItem> Convert(List<Application> applicationList, bool includeInspectorApproval)
        {
            return applicationList == null ? null : applicationList.Select(x=>Convert(x, false, true, true, includeInspectorApproval)).ToList();
        }

        public static ApplicationItem Convert(Application application, bool includeNotApplicable = false, bool includeAllSite = true, bool includeFacilities = true, bool includeInspectorApproval = true, DateTime? inspectionDate = null)
        {

            if (application == null) return null;

            var item = new ApplicationItem
            {
                ApplicationId = application.Id,
                ApplicationTypeId = application.ApplicationTypeId,
                ApplicationTypeName =
                    application.ApplicationType != null ? application.ApplicationType.Name : string.Empty,
                ApplicantApplicationStatusName =
                    application.ApplicationStatus?.NameForApplicant ?? Constants.ApplicationStatus.InProgress,
                ApplicationStatusId = application.ApplicationStatusId,
                ApplicationStatusName = application.ApplicationStatus?.Name ?? Constants.ApplicationStatus.InProgress,
                OrganizationId = application.Organization.Id,
                OrganizationName = application.Organization.Name,
                CurrentCycleNumber =
                    application.Organization.OrganizationAccreditationCycles != null &&
                    application.Organization.OrganizationAccreditationCycles.Count > 0
                        ? application.Organization.OrganizationAccreditationCycles.First(x => x.IsCurrent == true)
                            .Number
                        : 0,
                CreatedBy = application.CreatedBy,
                CycleNumber = application.CycleNumber,
                //SiteApplications = application.SiteApplicationVersions.Select(Convert).ToList(),
                SubmittedDate = application.SubmittedDate,
                SubmittedDateString =
                    application.SubmittedDate == null ? "" : application.SubmittedDate.Value.ToShortDateString(),
                TypeDetail = application.TypeDetail,
                RFIDueDate = application.RFIDueDate == null ? "" : application.RFIDueDate.Value.ToShortDateString(),
                DueDate = application.DueDate,
                DueDateString = application.DueDate == null ? "" : application.DueDate.Value.ToShortDateString(),
                UpdatedDate = application.UpdatedDate,
                UniqueId = application.UniqueId,
                ApplicationVersionId = application.ApplicationVersionId.GetValueOrDefault(),
                ApplicationVersionTitle =
                    application.ApplicationVersion != null ? application.ApplicationVersion.Title : string.Empty,
                ComplianceApplicationId = application.ComplianceApplicationId,
                CreatedDate = application.CreatedDate.GetValueOrDefault(),
                CreatedDateString =
                    application.ComplianceApplication != null
                        ? application.ComplianceApplication.CreatedDate.GetValueOrDefault().ToShortDateString()
                        : application.CreatedDate.GetValueOrDefault().ToShortDateString()
            };

            if (includeInspectorApproval)
            {
                item.IsInspectorComplete =
                    application.ApplicationResponses?.Where(y => y.ApplicationSectionQuestion.QuestionTypeId != 4)
                        .All(
                            x =>
                                x.ApplicationResponseStatusId.HasValue &&
                                (x.ApplicationResponseStatus.Name == Constants.ApplicationResponseStatus.Compliant ||
                                 x.ApplicationResponseStatus.Name == Constants.ApplicationResponseStatus.NotCompliant ||
                                 x.ApplicationResponseStatus.Name ==
                                 Constants.ApplicationResponseStatus.NoResponseRequested)) ?? true;
            }

            if (includeNotApplicable && application.ApplicationQuestionNotApplicables != null)
            {
                item.NotApplicables = application.ApplicationQuestionNotApplicables.Select(Convert).ToList();
            }

            if (application.Site != null)
            {
                item.Site = Convert(application.Site, false, false, false);
            }

            if (includeFacilities && application.Organization.OrganizationFacilities != null)
            {
                var primarySite = (from orgFac in application.Organization.OrganizationFacilities
                                   select orgFac.Facility.FacilitySites.FirstOrDefault(x => x.Site.IsPrimarySite)
                                   into facSite
                                   where facSite != null && facSite.Site != null
                                   select facSite.Site).FirstOrDefault();

                item.PrimarySite = Convert(primarySite, false);
            }

            if (application.AccreditationOutcomes != null && application.AccreditationOutcomes.Count > 0)
            {
                var outcome = application.AccreditationOutcomes.OrderByDescending(ao => ao.CreatedDate).FirstOrDefault();

                if (outcome != null)
                {
                    item.OutcomeStatusName = outcome.OutcomeStatus.Name;
                }
            }

            if (application.ComplianceApplication != null)
            {
                item.Coordinator = Convert(application.ComplianceApplication.Coordinator, false);
            }
            else if (application.Coordinator != null)
            {
                item.Coordinator = Convert(application.Coordinator, false, false, true);
            }
            else if (application.CoordinatorId != null)
            {
                item.Coordinator = new UserItem
                {
                    UserId = application.CoordinatorId.Value
                };
            }

            if (inspectionDate.HasValue)
            {
                item.InspectionDate = inspectionDate.Value;
            }
            else if (application.InspectionSchedules != null)
            {

                item.InspectionDate = application.InspectionSchedules.LastOrDefault()?.StartDate;
            }



            return item;
        }

        public static List<ApplicationSectionResponse> SetStatuses(string appStatus, List<ApplicationSectionResponse> items,
            bool isReviewer)
        {
            foreach (var item in items)
            {
                if (item.Children != null && item.Children.Count > 0)
                {
                    SetStatuses(appStatus, item.Children, isReviewer);

                    if (isReviewer && appStatus != Constants.ApplicationStatus.InProgress)
                    {
                        item.SectionStatusName = SetStatusFromChildrenForReviewer(item);
                    }
                    else
                    {
                        item.SectionStatusName = SetStatusFromChildrenForApplicant(item);
                    }

                    item.IsVisible = HasVisibleChildren(item);
                }
            }

            return items;
        }

        private static bool HasVisibleChildren(ApplicationSectionResponse item)
        {
            if (item.Children != null && item.Children.Count > 0)
            {
                foreach (var child in item.Children)
                {
                    if (HasVisibleChildren(child))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return item.IsVisible;
            }

            return false;
        }

        private static string SetStatusFromChildrenForReviewer(ApplicationSectionResponse item, List<ApplicationSectionResponse> sections = null)
        {
            sections = sections ?? item.Children.Where(x => x.IsVisible).ToList();

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.ForReview))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (sections.All(x => string.IsNullOrEmpty(x.SectionStatusName)))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.New))
            {
                return Constants.ApplicationResponseStatus.New;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.Reviewed))
            {
                return Constants.ApplicationResponseStatus.Reviewed;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.NotCompliant))
            {
                return Constants.ApplicationResponseStatus.NotCompliant;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.RfiFollowup))
            {
                return Constants.ApplicationResponseStatus.RfiFollowup;
            }

            

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.Compliant))
            {
                return Constants.ApplicationResponseStatus.Compliant;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.NA))
            {
                return Constants.ApplicationResponseStatus.NA;
            }

            return
                sections.Any(
                    x => x.SectionStatusName == Constants.ApplicationResponseStatus.NoResponseRequested)
                    ? Constants.ApplicationResponseStatus.NoResponseRequested
                    : Constants.ApplicationResponseStatus.InProgress;
        }

        private static string SetStatusFromChildrenForApplicant(ApplicationSectionResponse item, List<ApplicationSectionResponse> sections = null)
        {
            sections = sections ?? item.Children.Where(x => x.IsVisible).ToList();

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.InProgress))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (sections.All(x => x.SectionStatusName == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.Complete;
            }

            if (sections.Any(x => x.SectionStatusName == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (sections.All(x => x.SectionStatusName == Constants.ApplicationResponseStatus.NotStarted))
            {
                return Constants.ApplicationResponseStatus.NotStarted;
            }

            return Constants.ApplicationResponseStatus.InProgress;
        }

        public static string SetStatusForAllSectionsForApplicant(List<ApplicationSectionResponse> items)
        {
            var sections = items.Where(x => x.IsVisible).ToList();

            return SetStatusFromChildrenForApplicant(null, sections);
        }

        public static string SetStatusForAllSectionsForReviewer(List<ApplicationSectionResponse> items)
        {
            var sections = items.Where(x => x.IsVisible).ToList();

            return SetStatusFromChildrenForReviewer(null, sections);
        }

        public static string SetStatusForAllApplicationsReviewer(List<AppResponse> apps)
        {
            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.ForReview))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (apps.All(x => string.IsNullOrEmpty(x.ApplicationTypeStatusName)))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.New))
            {
                return Constants.ApplicationResponseStatus.New;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.Reviewed))
            {
                return Constants.ApplicationResponseStatus.Reviewed;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.NotCompliant))
            {
                return Constants.ApplicationResponseStatus.NotCompliant;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.RfiFollowup))
            {
                return Constants.ApplicationResponseStatus.RfiFollowup;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.Compliant))
            {
                return Constants.ApplicationResponseStatus.Compliant;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.NA))
            {
                return Constants.ApplicationResponseStatus.NA;
            }

            return
                apps.Any(
                    x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.NoResponseRequested)
                    ? Constants.ApplicationResponseStatus.NoResponseRequested
                    : Constants.ApplicationResponseStatus.InProgress;
        }

        public static string SetStatusForAllApplicationsApplicant(List<AppResponse> apps)
        {
            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.InProgress))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (apps.All(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.Complete;
            }

            if (apps.Any(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (apps.All(x => x.ApplicationTypeStatusName == Constants.ApplicationResponseStatus.NotStarted))
            {
                return Constants.ApplicationResponseStatus.NotStarted;
            }

            return Constants.ApplicationResponseStatus.InProgress;
        }

        public static List<ApplicationSectionItem> SetColor(string appStatus, DateTime appSubmittedDate, List<ApplicationSectionItem> items, bool isReviewer)
        {
            foreach (var item in items)
            {
                item.IsVisible = true;

                if (item.Questions != null && item.Questions.Count > 0 &&
                    (item.Children == null || item.Children.Count == 0))
                {
                    if (item.Questions.All(x => x.IsHidden))
                    {
                        item.IsVisible = false;
                        
                    }
                }
                else if ((item.Questions == null || item.Questions.Count == 0) && (item.Children == null || item.Children.Count == 0))
                {
                    item.IsVisible = false;
                }

                if (item.Children != null && item.Children.Count > 0)
                {
                    SetColor(appStatus, appSubmittedDate, item.Children, isReviewer);

                    if (isReviewer)
                    {
                        if (appStatus == Constants.ApplicationStatus.InProgress)
                        {
                            var appColor = FindColorFromChildrenForApplicant(item);
                            item.Circle = appColor;
                        }
                        else
                        {
                            item.Circle = FindColorFromChildrenForReviewer(item);
                        }
                    }
                    else
                    {
                        var appColor = FindColorFromChildrenForApplicant(item);
                        item.Circle = appColor;
                    }

                    item.CircleStatusName = item.Circle?.Replace(" ", "");

                    item.IsVisible = HasVisibleChildren(item);
                }
                else
                {
                    if (isReviewer)
                    {
                        if (appStatus == Constants.ApplicationStatus.InProgress)
                        {
                            var appColor = FindColorFromQuestionsForApplicant(appStatus, appSubmittedDate, item);
                            item.Circle = appColor;
                        }
                        else
                        {
                            item.Circle = FindColorFromQuestionsForReviewer(item);
                        }
                    }
                    else
                    {
                        item.Circle = FindColorFromQuestionsForApplicant(appStatus, appSubmittedDate, item);
                    }
                    
                    item.CircleStatusName = item.Circle?.Replace(" ", "");
                }
            }

            return items;
        }

        private static bool HasVisibleChildren(ApplicationSectionItem item)
        {
            if ((item.Children == null || item.Children.Count == 0) && (item.Questions == null || item.Questions.Count == 0))
            {
                return false;
            }

            if (item.Children != null && item.Children.Count > 0)
            {
                foreach (var child in item.Children)
                {
                    if (HasVisibleChildren(child))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return item.IsVisible;
            }

            return false;
        }

        private static string FindColorFromChildrenForApplicant(ApplicationSectionItem item, List<ApplicationSectionItem> sections = null)
        {
            sections = sections ?? item.Children.Where(x => x.IsVisible).ToList();

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.InProgress))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (sections.All(x => x.Circle == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.Complete;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (sections.All(x => x.Circle == Constants.ApplicationResponseStatus.NotStarted))
            {
                return Constants.ApplicationResponseStatus.NotStarted;
            }

            return "";
        }

        private static string FindColorFromQuestionsForApplicant(string appStatus, DateTime appSubmittedDate, ApplicationSectionItem item)
        {
            if (!string.IsNullOrEmpty(item.Circle)) return item.Circle;

            var answeredCount = 0;
            var notAnswered = false;

            var questions = item.Questions.Where(x => !x.IsHidden).ToList();

            foreach (var question in questions)
            {
                var answered = false;

                if (question.Type == Constants.QuestionTypes.Checkboxes ||
                    question.Type == Constants.QuestionTypes.RadioButtons)
                {
                    if (question.Answers != null)
                    {
                        answered = question.Answers.Any(x => x.Selected);
                    }
                }
                else
                {
                    if (question.QuestionResponses != null && question.QuestionResponses.Count > 0)
                    {
                        answered = !string.IsNullOrEmpty(question.QuestionResponses[0].OtherText);

                        if (!answered)
                        {
                            answered = question.QuestionResponses[0].Document != null;
                        }

                        if (!answered)
                        {
                            answered = question.QuestionResponses[0].UserId.HasValue;
                        }

                        if (!answered)
                        {
                            answered = !string.IsNullOrEmpty(question.QuestionResponses[0].FromDate) &&
                                       !string.IsNullOrEmpty(question.QuestionResponses[0].ToDate);
                        }
                    }
                }

                if (answered)
                {
                    answeredCount++;
                }
                else
                {
                    notAnswered = true;
                }
            }

            if (notAnswered && appStatus == Constants.ApplicationStatus.RFI)
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            foreach (var q in questions.Where(x => string.IsNullOrEmpty(x.VisibleAnswerResponseStatusName)))
            {
                q.VisibleAnswerResponseStatusName = appStatus == Constants.ApplicationStatus.ForReview.Trim() ? Constants.ApplicationResponseStatus.Complete : q.AnswerResponseStatusName;
            }

            if (questions.Any(x => x.VisibleAnswerResponseStatusName == Constants.ApplicationResponseStatus.RFI && (appStatus == Constants.ApplicationStatus.RFI || x.QuestionResponses.Any(y=>y.UpdatedDate <= appSubmittedDate))))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }
            else if (questions.Any(x => x.VisibleAnswerResponseStatusName == Constants.ApplicationResponseStatus.RFI && 
                ((appStatus != Constants.ApplicationStatus.RFI && appStatus != Constants.ApplicationStatus.InProgress) || x.QuestionResponses.Any(y => y.UpdatedDate > appSubmittedDate))))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (questions.Any(x => x.VisibleAnswerResponseStatusName == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }


            return answeredCount > 0
                    ? notAnswered
                        ? Constants.ApplicationResponseStatus.InProgress
                        : Constants.ApplicationResponseStatus.Complete
                    : Constants.ApplicationResponseStatus.NotStarted;
        }

        private static string FindColorFromQuestionsForReviewer(ApplicationSectionItem item)
        {
            var questions = item.Questions.Where(x => !x.IsHidden).ToList();

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (questions.Any(x => string.IsNullOrEmpty(x.AnswerResponseStatusName) || x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.ForReview))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (questions.All(x => string.IsNullOrEmpty(x.AnswerResponseStatusName)))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.New))
            {
                return Constants.ApplicationResponseStatus.New;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.Reviewed))
            {
                return Constants.ApplicationResponseStatus.Reviewed;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.NotCompliant))
            {
                return Constants.ApplicationResponseStatus.NotCompliant;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.RfiFollowup))
            {
                return Constants.ApplicationResponseStatus.RfiFollowup;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.Compliant))
            {
                return Constants.ApplicationResponseStatus.Compliant;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.NA))
            {
                return Constants.ApplicationResponseStatus.NA;
            }

            if (questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.NoResponseRequested))
            {
                return Constants.ApplicationResponseStatus.NoResponseRequested;
            }


            return "";
        }

        public static string FindColorFromAllSectionsForReviewer(List<ApplicationSectionItem> items)
        {
            var sections = items.Where(x => x.IsVisible).ToList();

            return FindColorFromChildrenForReviewer(null, sections);
        }

        public static string FindColorFromAllSectionsForApplicant(List<ApplicationSectionItem> items)
        {
            var sections = items.Where(x => x.IsVisible).ToList();

            return FindColorFromChildrenForApplicant(null, sections);
        }

        public static string FindColorForAllApplicationsReviewer(List<ApplicationItem> apps)
        {
            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.ForReview))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (apps.All(x => string.IsNullOrEmpty(x.Circle)))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.New))
            {
                return Constants.ApplicationResponseStatus.New;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.Reviewed))
            {
                return Constants.ApplicationResponseStatus.Reviewed;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.NotCompliant))
            {
                return Constants.ApplicationResponseStatus.NotCompliant;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.RfiFollowup))
            {
                return Constants.ApplicationResponseStatus.RfiFollowup;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.Compliant))
            {
                return Constants.ApplicationResponseStatus.Compliant;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.NA))
            {
                return Constants.ApplicationResponseStatus.NA;
            }

            return
                apps.Any(
                    x => x.Circle == Constants.ApplicationResponseStatus.NoResponseRequested)
                    ? Constants.ApplicationResponseStatus.NoResponseRequested
                    : "";
        }

        public static string FindColorForAllApplicationsApplicant(List<ApplicationItem> apps)
        {
            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.InProgress))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (apps.All(x => x.Circle == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.Complete;
            }

            if (apps.Any(x => x.Circle == Constants.ApplicationResponseStatus.Complete))
            {
                return Constants.ApplicationResponseStatus.InProgress;
            }

            if (apps.All(x => x.Circle == Constants.ApplicationResponseStatus.NotStarted))
            {
                return Constants.ApplicationResponseStatus.NotStarted;
            }

            return "";
        }

        private static string FindColorFromChildrenForReviewer(ApplicationSectionItem item, List<ApplicationSectionItem> sections = null)
        {
            sections = sections ?? item.Children.Where(x => x.IsVisible).ToList();

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFI))
            {
                return Constants.ApplicationResponseStatus.RFI;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.RFICompleted))
            {
                return Constants.ApplicationResponseStatus.RFICompleted;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.ForReview))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (sections.All(x => string.IsNullOrEmpty(x.Circle)))
            {
                return Constants.ApplicationResponseStatus.ForReview;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.New))
            {
                return Constants.ApplicationResponseStatus.New;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.Reviewed))
            {
                return Constants.ApplicationResponseStatus.Reviewed;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.NotCompliant))
            {
                return Constants.ApplicationResponseStatus.NotCompliant;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.RfiFollowup))
            {
                return Constants.ApplicationResponseStatus.RfiFollowup;
            }
            
            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.Compliant))
            {
                return Constants.ApplicationResponseStatus.Compliant;
            }

            if (sections.Any(x => x.Circle == Constants.ApplicationResponseStatus.NA))
            {
                return Constants.ApplicationResponseStatus.NA;
            }

            return
                sections.Any(
                    x => x.Circle == Constants.ApplicationResponseStatus.NoResponseRequested)
                    ? Constants.ApplicationResponseStatus.NoResponseRequested
                    : "";
        }

        

        public static List<ApplicationResponseStatusItem> Convert(List<ApplicationResponseStatus> applicationResponseStatusList)
        {
            return applicationResponseStatusList == null ? null : applicationResponseStatusList.Select(Convert).ToList();
        }

        public static ApplicationResponseStatusItem Convert(ApplicationResponseStatus applicationResponseStatus)
        {
            if (applicationResponseStatus == null) return null;

            return new ApplicationResponseStatusItem
            {
                Id = applicationResponseStatus.Id,
                Name = applicationResponseStatus.Name,
                NameForApplicant = applicationResponseStatus.NameForApplicant,
                Description = applicationResponseStatus.Description
            };
        }

        public static Question Convert(ApplicationSectionQuestion question, bool includeChildData)
        {
            var q = new Question
            {
                Id = question.Id,
                Text = question.Text,
                Description = question.Description,
                Active = question.IsActive,
                Type = question.QuestionType == null ? "" : question.QuestionType.Name,
                ComplianceNumber = question.ComplianceNumber,
                Answers = question.Answers?.Where(x => x.IsActive).OrderBy(x => x.Order).Select(Convert).ToList() ?? new List<Answer>(),
                SectionId = question.ApplicationSectionId,
                SectionName = question.ApplicationSection?.Name ?? string.Empty,
                SectionUniqueIdentifier = question.ApplicationSection?.UniqueIdentifier ?? string.Empty,

                QuestionResponses = new List<QuestionResponse>()
            };

            if (includeChildData)
            {
                q.HiddenBy = question.HiddenBy?.Select(Convert).ToList() ?? new List<QuestionAnswerDisplay>();
                q.ScopeTypes = question.ScopeTypes.Select(Convert).ToList();
            }

            if (question.QuestionTypesFlag.HasValue)
            {
                q.QuestionTypeFlags = QuestionTypeFlags.Set(question.QuestionTypesFlag.Value);
            }

            return q;
        }

        public static Answer Convert(ApplicationSectionQuestionAnswer answer)
        {
            var an = new Answer
            {
                Id = answer.Id,
                Active = answer.IsActive,
                Text = answer.Text,
                Order = answer.Order,
                IsExpectedAnswer = answer.IsExpectedAnswer.GetValueOrDefault(),
            };

            if (answer.Displays != null)
            {
                an.HidesQuestions = answer.Displays.Select(Convert).ToList();
            }

            return an;
        }

        public static QuestionAnswerDisplay Convert(ApplicationSectionQuestionAnswerDisplay display)
        {
            if (display == null) return null;

            var ret = new QuestionAnswerDisplay
            {
                Id = display.Id,
                QuestionId = display.HidesQuestionId,
                RequirementNumber = display.ApplicationSectionQuestion?.ApplicationSection.UniqueIdentifier,
                ComplianceNumber = display.ApplicationSectionQuestion?.ComplianceNumber,
                QuestionText = display.ApplicationSectionQuestion != null ? display.ApplicationSectionQuestion.Text : string.Empty,
                AnswerId = display.ApplicationSectionQuestionAnswerId,
            };

            return ret;
        }

        public static ScopeTypeItem Convert(ApplicationSectionQuestionScopeType scopeType)
        {
            if (scopeType.ScopeType == null)
                return null;

            return new ScopeTypeItem
            {
                ImportName = scopeType.ScopeType.ImportName,
                Name = scopeType.ScopeType.Name,
                ScopeTypeId = scopeType.ScopeType.Id,
                IsActive = scopeType.ScopeType.IsActive
            };
        }

        public static List<InspectionScheduleItem> ConvertToInspectionSchedule(Application application)
        {
            var result = new List<InspectionScheduleItem>();

            foreach (var inspectionSchedule in application.InspectionSchedules.Where(x => x.IsActive && !x.IsArchive))
            {
                result.Add(new InspectionScheduleItem
                {
                    InspectionScheduleId = inspectionSchedule.Id.ToString(),
                    ApplicationId = application.Id.ToString(),
                    ApplicationTypeId = application.ApplicationTypeId.ToString(),
                    ApplicationTypeName = application.ApplicationType != null ? application.ApplicationType.Name : string.Empty,
                    OrganizationId = application.OrganizationId.ToString(),
                    OrganizationName = application.Organization != null ? application.Organization.Name : string.Empty,
                    //InspectionDate =inspectionSchedule != null ? inspectionSchedule.InspectionDate.ToShortDateString(): string.Empty,
                    StartDate = inspectionSchedule.StartDate.ToShortDateString(),
                    EndDate = inspectionSchedule.EndDate.ToShortDateString(),
                    //InspectionDate = inspectionSchedule != null ? inspectionSchedule.InspectionDate.ToString("g") : string.Empty,
                    UpdatedDate = inspectionSchedule.UpdatedDate?.ToString("g") ?? string.Empty,
                    UpdatedBy = inspectionSchedule.UpdatedBy,
                    CreatedBy = inspectionSchedule.CreatedBy,
                    CreatedDate = inspectionSchedule.CreatedDate?.ToString("g") ?? string.Empty,
                    CompletedDate = inspectionSchedule.CompletionDate.ToString("g"),
                    IsCompleted = inspectionSchedule.IsCompleted ? "Yes" : "No",
                    IsActive = inspectionSchedule.IsActive.ToString(),
                    ComplianceApplicationId = application.ComplianceApplicationId,
                    AppUniqueId = application.UniqueId
                });
            }

            if (result.Count > 0) return result;

            result.Add(new InspectionScheduleItem
            {
                InspectionScheduleId = "0",
                ApplicationId = application.Id.ToString(),
                ApplicationTypeId = application.ApplicationTypeId.ToString(),
                ApplicationTypeName = application.ApplicationType != null ? application.ApplicationType.Name : string.Empty,
                OrganizationId = application.OrganizationId.ToString(),
                OrganizationName = application.Organization != null ? application.Organization.Name : string.Empty,
                StartDate = string.Empty,
                EndDate = string.Empty,
                UpdatedDate = string.Empty,
                UpdatedBy = string.Empty,
                CreatedBy = string.Empty,
                CreatedDate = string.Empty,
                CompletedDate = string.Empty,
                IsCompleted = "No",
                IsActive = "False",
                ComplianceApplicationId = application.ComplianceApplicationId,
                AppUniqueId = application.UniqueId
            });

            return result;
        }

        public static JobFunctionItem Convert(JobFunction jobFunction)
        {
            if (jobFunction == null) return null;

            return new JobFunctionItem
            {
                Id = jobFunction.Id,
                Name = jobFunction.Name
            };
        }

        public static DocumentItem Convert(Document document, bool? hasResponses = null, bool includeAssociatationTypes = false)
        {
            if (document == null) return null;

            return new DocumentItem
            {
                Id = document.Id,
                Name = document.Name,
                OriginalName = document.OriginalName,
                RequestValues = document.RequestValues,
                HasResponses = hasResponses ?? document.ApplicationResponses?.Count > 0,
                CreatedDate = document.CreatedDate.GetValueOrDefault().ToString("G"),
                CreatedBy = document.CreatedBy,
                IncludeInReporting = document.IncludeInReporting.GetValueOrDefault(),
                IsLatestVersion = document.IsLatestVersion,
                ApplicationId = document.ApplicationId,
                AssociationTypes = includeAssociatationTypes ? document.AssociationTypes != null && document.AssociationTypes.Count > 0 ? string.Join(", ", document.AssociationTypes.Select(x => x.AssociationType?.Name).ToList()) : "None" : null,
                VaultId = document.OrganizationDocumentLibrary?.VaultId
            };
        }

        public static AssociationTypeItem Convert(AssociationType item)
        {
            if (item == null) return null;

            return new AssociationTypeItem
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        public static LanguageItem Convert(Language language)
        {
            if (language == null) return null;

            return new LanguageItem
            {
                Id = language.Id,
                Name = language.Name
            };
        }

        public static MembershipItem Convert(Membership membership)
        {
            if (membership == null) return null;
            return new MembershipItem
            {
                Id = membership.Id,
                Name = membership.Name
            };
        }

        public static List<CommentTypeItem> Convert(List<CommentType> commentTypeList)
        {
            return commentTypeList == null ? null : commentTypeList.Select(Convert).ToList();
        }

        public static CommentTypeItem Convert(CommentType commentType)
        {
            if (commentType == null)
                return null;

            return new CommentTypeItem
            {
                Id = commentType.Id,
                Name = commentType.Name
            };
        }

        public static List<ScopeTypeItem> Convert(List<ScopeType> scopeTypeList)
        {
            return scopeTypeList == null ? null : scopeTypeList.Select(Convert).ToList();
        }

        public static ScopeTypeItem Convert(ScopeType scopeType)
        {
            if (scopeType == null)
                return null;

            return new ScopeTypeItem
            {
                ScopeTypeId = scopeType.Id,
                ImportName = scopeType.ImportName,
                Name = scopeType.Name,
                IsActive = scopeType.IsActive,
                IsArchived = scopeType.IsArchived
            };
        }

        public static StateItem Convert(State state)
        {
            if (state == null)
                return null;

            return new StateItem
            {
                Id = state.Id,
                Name = state.Name
            };
        }

        public static CountryItem Convert(Country country)
        {
            if (country == null)
                return null;

            return new CountryItem
            {
                Id = country.Id,
                Name = country.Name
            };
        }

        public static AddressTypeItem Convert(AddressType addressType)
        {
            if (addressType == null)
                return null;

            return new AddressTypeItem
            {
                Id = addressType.Id,
                Name = addressType.Name
            };
        }

        public static ClinicalTypeItem Convert(ClinicalType clinicalType)
        {
            if (clinicalType == null)
                return null;

            return new ClinicalTypeItem
            {
                Id = clinicalType.Id,
                Name = clinicalType.Name
            };
        }

        public static ProcessingTypeItem Convert(ProcessingType processingType)
        {
            if (processingType == null)
                return null;

            return new ProcessingTypeItem
            {
                Id = processingType.Id,
                Name = processingType.Name
            };
        }

        public static CollectionProductTypeItem Convert(CollectionProductType collectionProductType)
        {
            if (collectionProductType == null)
                return null;

            return new CollectionProductTypeItem
            {
                Id = collectionProductType.Id,
                Name = collectionProductType.Name
            };
        }

        public static CBCollectionTypeItem Convert(CBCollectionType cbCollectionType)
        {
            if (cbCollectionType == null)
                return null;

            return new CBCollectionTypeItem
            {
                Id = cbCollectionType.Id,
                Name = cbCollectionType.Name
            };
        }

        public static CBUnitTypeItem Convert(CBUnitType cbUnitType)
        {
            if (cbUnitType == null)
                return null;

            return new CBUnitTypeItem
            {
                Id = cbUnitType.Id,
                Name = cbUnitType.Name
            };
        }

        public static TransplantTypeItem Convert(TransplantType transplantType)
        {
            if (transplantType == null)
                return null;

            return new TransplantTypeItem
            {
                Id = transplantType.Id,
                Name = transplantType.Name
            };
        }

        public static ClinicalPopulationTypeItem Convert(ClinicalPopulationType clinicalPopulationType)
        {
            if (clinicalPopulationType == null)
                return null;

            return new ClinicalPopulationTypeItem
            {
                Id = clinicalPopulationType.Id,
                Name = clinicalPopulationType.Name
            };
        }

        public static List<AccreditationStatusItem> Convert(List<AccreditationStatus> accreditationStatusList)
        {
            return accreditationStatusList == null ? null : accreditationStatusList.Select(Convert).ToList();
        }

        public static AccreditationStatusItem Convert(AccreditationStatus accreditationStatus)
        {
            if (accreditationStatus == null)
                return null;

            return new AccreditationStatusItem
            {
                Id = accreditationStatus.Id,
                Name = accreditationStatus.Name
            };
        }

        public static BAAOwnerItem Convert(BAAOwner baaOwner)
        {
            if (baaOwner == null)
                return null;

            return new BAAOwnerItem
            {
                Id = baaOwner.Id,
                Name = baaOwner.Name
            };
        }

        public static List<CredentialItem> Convert(List<Credential> credentialList)
        {
            return credentialList == null ? null : credentialList.Select(Convert).ToList();
        }

        public static CredentialItem Convert(Credential credential)
        {
            if (credential == null) return null;
            return new CredentialItem
            {
                Id = credential.Id,
                Name = credential.Name
            };
        }

        public static List<AccreditationOutcomeItem> Convert(List<AccreditationOutcome> accreditationOutcome)
        {
            return accreditationOutcome == null ? null : accreditationOutcome.Select(Convert).ToList();
        }

        public static AccreditationOutcomeItem Convert(AccreditationOutcome accreditationOutcome)
        {
            return new AccreditationOutcomeItem
            {
                Id = accreditationOutcome.Id,
                OrganizationId = accreditationOutcome.OrganizationId,
                OrganizationName = accreditationOutcome.Organization.Name,
                ApplicationId = accreditationOutcome.ApplicationId,
                ApplicationName = accreditationOutcome.Application.ApplicationType.Name,
                OutcomeStatusId = accreditationOutcome.OutcomeStatusId,
                OutcomeStatusName = accreditationOutcome.OutcomeStatus.Name,
                ReportReviewStatusId = accreditationOutcome.ReportReviewStatusId,
                ReportReviewStatusName = accreditationOutcome.ReportReviewStatus.Name,
                CommitteeDate = accreditationOutcome.CommitteeDate == null ? "" : accreditationOutcome.CommitteeDate.Value.ToShortDateString(),
                SendEmail = accreditationOutcome.SendEmail == null ? false : accreditationOutcome.SendEmail.GetValueOrDefault(),
                CreatedBy = accreditationOutcome.CreatedBy,
                CreatedDate = accreditationOutcome.CreatedDate == null ? "" : accreditationOutcome.CreatedDate.Value.ToShortDateString()
            };
        }

        public static List<OutcomeStatusItem> Convert(List<OutcomeStatus> outcomeStatusList)
        {
            return outcomeStatusList == null ? null : outcomeStatusList.Select(Convert).ToList();
        }

        public static OutcomeStatusItem Convert(OutcomeStatus outcomeStatus)
        {
            if (outcomeStatus == null)
                return null;

            return new OutcomeStatusItem
            {
                Id = outcomeStatus.Id,
                Name = outcomeStatus.Name
            };
        }

        public static List<ReportReviewStatusItem> Convert(List<ReportReviewStatus> reportReviewStatusList)
        {
            return reportReviewStatusList == null ? null : reportReviewStatusList.Select(Convert).ToList();
        }

        public static ReportReviewStatusItem Convert(ReportReviewStatus reportReviewStatus)
        {
            if (reportReviewStatus == null)
                return null;

            return new ReportReviewStatusItem
            {
                Id = reportReviewStatus.Id,
                Name = reportReviewStatus.Name
            };
        }

        //public static Personnel Convert(List<UserJobFunction> listUserJobFunction)
        //{
        //    if (listUserJobFunction.Count() == 0)
        //        return new Personnel();

        //    var personnelItem = new Personnel();
        //    UserJobFunction userJobFunction = null;

        //    userJobFunction = listUserJobFunction.Where(x => x.JobFunction.Name == Constants.JobFunctions.CordBloodBankDirector).FirstOrDefault();
        //    if (userJobFunction != null)
        //        personnelItem.BankDirector = userJobFunction.User.FirstName + " " + userJobFunction.User.LastName + ", " + userJobFunction.User.UserCredentials.FirstOrDefault().Credential.Name;

        //    userJobFunction = listUserJobFunction.Where(x => x.JobFunction.Name == Constants.JobFunctions.CordBloodBankMedicalDirector).FirstOrDefault();
        //    if (userJobFunction != null)
        //        personnelItem.BankMedicalDirector = userJobFunction.User.FirstName + " " + userJobFunction.User.LastName + ", " + userJobFunction.User.UserCredentials.FirstOrDefault().Credential.Name;

        //    userJobFunction = listUserJobFunction.Where(x => x.JobFunction.Name == Constants.JobFunctions.CordBloodCollectionSiteDirector).FirstOrDefault();
        //    if (userJobFunction != null)
        //        personnelItem.CollectionSiteDirector = userJobFunction.User.FirstName + " " + userJobFunction.User.LastName + ", " + userJobFunction.User.UserCredentials.FirstOrDefault().Credential.Name;

        //    userJobFunction = listUserJobFunction.Where(x => x.JobFunction.Name == Constants.JobFunctions.ProcessingFacilityDirector).FirstOrDefault();
        //    if (userJobFunction != null)
        //        personnelItem.ProcessingFacilityDirector = userJobFunction.User.FirstName + " " + userJobFunction.User.LastName + ", " + userJobFunction.User.UserCredentials.FirstOrDefault().Credential.Name;

        //    userJobFunction = listUserJobFunction.Where(x => x.JobFunction.Name == Constants.JobFunctions.QmSupervisor).FirstOrDefault();
        //    if (userJobFunction != null)
        //        personnelItem.QmSupervisor = userJobFunction.User.FirstName + " " + userJobFunction.User.LastName + ", " + userJobFunction.User.UserCredentials.FirstOrDefault().Credential.Name;

        //    return personnelItem;

        //}

        public static List<InspectionTeamMember> ConvertToInspectionTeamMember(List<InspectionSchedule> listInspectionSchedule)
        {
            if (listInspectionSchedule.Count() == 0)
                return new List<InspectionTeamMember>();

            var inspectionTeamMemberTeamList = new List<InspectionTeamMember>();

            foreach (var inspectionSchedule in listInspectionSchedule)
            {
                foreach (var inspectionScheduleDetail in inspectionSchedule.InspectionScheduleDetails.OrderBy(x => x.Site.Name))
                {
                    if (!inspectionScheduleDetail.IsActive && !inspectionScheduleDetail.IsArchive) continue;

                    var creds = string.Empty;

                    foreach (var cred in inspectionScheduleDetail.User.UserCredentials)
                    {
                        creds += cred.Credential.Name + ", ";
                    }

                    if (creds.Length > 0)
                    {
                        creds = creds.Substring(0, creds.Length - 2);
                    }

                    var inspectionTeamMemberItem = new InspectionTeamMember
                    {
                        Name = inspectionScheduleDetail.User.FirstName + " " + inspectionScheduleDetail.User.LastName + (creds.Length > 0 ? ", " + creds : ""),
                        Role = inspectionScheduleDetail.AccreditationRole.Name,
                        Site = inspectionScheduleDetail.Site.Name,
                        Email = inspectionScheduleDetail.User.EmailAddress
                    };

                    inspectionTeamMemberTeamList.Add(inspectionTeamMemberItem);
                }
            }

            return inspectionTeamMemberTeamList;
        }

        public static Overview ConvertToOverviewItem(ComplianceApplicationItem complianceApplication, List<InspectionSchedule> inspectionScheduleList)
        {
            if (complianceApplication == null)
                return new Overview();

            System.Collections.Generic.Dictionary<string, string> dicStandards = new Dictionary<string, string>();
            StringBuilder inspectionDates = new StringBuilder();
            StringBuilder standards = new StringBuilder();

            foreach (var application in complianceApplication.Applications)
            {
                var inspectionSchedule = inspectionScheduleList.Where(x => x.ApplicationId == application.ApplicationId).FirstOrDefault();
                if (inspectionSchedule != null)
                    inspectionDates.Append("," + inspectionSchedule.StartDate.ToShortDateString());

                if (!string.IsNullOrEmpty(application.ApplicationVersionTitle))
                    dicStandards[application.ApplicationVersionTitle] = application.ApplicationVersionTitle;                    
            }

            foreach (string std in dicStandards.Keys) {
                standards.Append(std);
            }

            Overview overviewItem = new Overview();
            overviewItem.Type = complianceApplication.ReportReviewStatus;
            overviewItem.TypeName = complianceApplication.ReportReviewStatusName;
            overviewItem.InspectionDate = inspectionDates.ToString().Trim().Trim(',');
            overviewItem.AccreditedSince = null;
            overviewItem.Standards = standards.ToString().Trim().Trim(',');
            overviewItem.AccreditationGoal = complianceApplication.AccreditationGoal;
            overviewItem.InspectionScope = complianceApplication.InspectionScope;

            return overviewItem;

        }

        public static InspectionItem Convert(Inspection inspection)
        {
            if (inspection == null) return null;

            var result = new InspectionItem
            {
                Id = inspection.Id,
                ApplicationId = inspection.ApplicationId.GetValueOrDefault(),
                CommendablePractices = inspection.CommendablePractices,
                OverallImpressions = inspection.OverallImpressions,
                SiteDescription = inspection.TraineeSiteDescription,
                User = new UserItem
                {
                    UserId = inspection.User.Id,
                    FirstName = inspection.User.FirstName,
                    LastName = inspection.User.LastName,
                    EmailAddress = inspection.User.EmailAddress,
                    Role = Convert(inspection.User.Role),
                    JobFunctions = inspection.User.UserJobFunctions.Select(x => Convert(x.JobFunction)).ToList(),
                    Credentials = inspection.User.UserCredentials.Select(x => Convert(x.Credential)).ToList()
                }
            };

            if (!inspection.IsTrainee.GetValueOrDefault() && inspection.Application != null && inspection.Application.Site != null)
            {
                result.SiteDescription = inspection.Application.Site.Description;

                result.AllSitesWithDetails = inspection.Application.ComplianceApplication.Applications.All(x => x.Inspections.Any());
            }

            return result;
        }

        public static CBCategoryItem Convert(CBCategory item)
        {
            return new CBCategoryItem
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        public static SiteCordBloodTransplantTotalItem Convert(SiteCordBloodTransplantTotal item)
        {
            if (item == null) return null;

            return new SiteCordBloodTransplantTotalItem
            {
                Id = item.Id,
                SiteId = item.SiteId,
                CbCategory = Convert(item.CbCategory),
                CbUnitType = Convert(item.CbUnitType),
                NumberOfUnits = item.NumberOfUnits,
                AsOfDate = item.AsOfDate.ToShortDateString()
            };
        }

        public static TransplantCellTypeItem Convert(TransplantCellType item)
        {
            if (item == null) return null;

            return new TransplantCellTypeItem
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        public static SiteTransplantTotalItem Convert(SiteTransplantTotal item)
        {
            if (item == null) return null;

            return new SiteTransplantTotalItem
            {
                Id = item.Id,
                SiteId = item.SiteId,
                TransplantCellType = Convert(item.TransplantCellType),
                ClinicalPopulationType = Convert(item.ClinicalPopulationType),
                TransplantType = Convert(item.TransplantType),
                IsHaploid = item.IsHaploid,
                NumberOfUnits = item.NumberOfUnits,
                StartDate = item.StartDate.ToShortDateString(),
                EndDate = item.EndDate.ToShortDateString()
            };
        }

        public static CollectionTypeItem Convert(CollectionType item)
        {
            if (item == null) return null;

            return new CollectionTypeItem
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        public static SiteCollectionTotalItem Convert(SiteCollectionTotal item)
        {
            if (item == null) return null;

            return new SiteCollectionTotalItem
            {
                Id = item.Id,
                SiteId = item.SiteId,
                CollectionType = Convert(item.CollectionType),
                ClinicalPopulationType = Convert(item.ClinicalPopulationType),
                NumberOfUnits = item.NumberOfUnits,
                StartDate = item.StartDate.ToShortDateString(),
                EndDate = item.EndDate.ToShortDateString()
            };
        }

        public static SiteProcessingTotalItem Convert(SiteProcessingTotal item)
        {
            if (item == null) return null;

            return new SiteProcessingTotalItem
            {
                Id = item.Id,
                SiteId = item.SiteId,
                NumberOfUnits = item.NumberOfUnits,
                StartDate = item.StartDate.ToShortDateString(),
                EndDate = item.EndDate.ToShortDateString(),
                SiteProcessingTotalTransplantCellTypes = item.SiteProcessingTotalTransplantCellTypes?.Select(Convert).ToList() ?? new List<SiteProcessingTotalTransplantCellTypeItem>()
            };
        }

        public static SiteProcessingMethodologyTotalItem Convert(SiteProcessingMethodologyTotal item)
        {
            if (item == null) return null;

            return new SiteProcessingMethodologyTotalItem
            {
                Id = item.Id,
                SiteId = item.SiteId,
                ProcessingType = Convert(item.ProcessingType),
                PlatformCount = item.PlatformCount,
                ProtocolCount = item.ProtocolCount,
                StartDate = item.StartDate.ToShortDateString(),
                EndDate = item.EndDate.ToShortDateString()
            };
        }

        public static SiteProcessingTotalTransplantCellTypeItem Convert(SiteProcessingTotalTransplantCellType item)
        {
            if (item == null) return null;

            return new SiteProcessingTotalTransplantCellTypeItem
            {
                Id = item.Id,
                SiteProcessingTotalId = item.SiteProcessingTotalId,
                TransplantCellType = Convert(item.TransplantCellType)
            };
        }

        public static CibmtrOutcomeAnalysis Convert(FacilityCibmtrOutcomeAnalysis item)
        {
            if (item == null) return null;

            return new CibmtrOutcomeAnalysis
            {
                Id = item.Id,
                CibmtrId = item.FacilityCibmtrId,
                ReportYear = item.ReportYear,
                SurvivalScore = item.SurvivalScore,
                SampleSize = item.SampleSize,
                ActualPercent = item.ActualPercent,
                PredictedPercent = item.PredictedPercent,
                LowerPercent = item.LowerPercent,
                UpperPercent = item.UpperPercent,
                ComparativeDataSource = item.ComparativeDataSource,
                PublishedOneYearSurvival = item.PublishedOneYearSurvival,
                ProgramOneYearSurvival = item.ProgramOneYearSurvival,
                Comments = item.Comments,
                ReportedCausesOfDeath = item.ReportedCausesOfDeath,
                CorrectiveActions = item.CorrectiveActions,
                FactImprovementPlan = item.FactImprovementPlan,
                AdditionalInformationRequested = item.AdditionalInformationRequested,
                ProgressOnImplementation = item.ProgressOnImplementation,
                InspectorCommendablePractices = item.InspectorCommendablePractices,
                InspectorInformation = item.InspectorInformation,
                Inspector100DaySurvival = item.Inspector100DaySurvival,
                Inspector1YearSurvival = item.Inspector1YearSurvival,
                IsNotRequired = item.IsNotRequired
            };
        }

        public static CibmtrDataMgmt Convert(FacilityCibmtrDataManagement item)
        {
            if (item == null) return null;

            return new CibmtrDataMgmt
            {
                Id = item.Id,
                CibmtrId = item.FacilityCibmtrId,
                AuditDate = item.AuditDate,
                CriticalFieldErrorRate = item.CriticalFieldErrorRate,
                RandomFieldErrorRate = item.RandomFieldErrorRate,
                OverallFieldErrorRate = item.OverallFieldErrorRate,
                IsCapIdentified = item.IsCapIdentified,
                AuditorComments = item.AuditorComments,
                CpiLetterDate = item.CpiLetterDate,
                CpiTypeId = item.CpiTypeId,
                CpiTypeName = item.CpiType?.Name,
                CpiComments = item.CpiComments,
                CorrectiveActions = item.CorrectiveActions,
                FactProgressDetermination = item.FactProgressDetermination,
                IsAuditAccuracyRequired = item.IsAuditAccuracyRequired,
                AdditionalInformation = item.AdditionalInformation,
                ProgressOnImplementation = item.ProgressOnImplementation,
                InspectorCommendablePractices = item.InspectorCommendablePractices,
                InspectorInformation = item.InspectorInformation,
                Inspector100DaySurvival = item.Inspector100DaySurvival,
                Inspector1YearSurvival = item.Inspector1YearSurvival
            };
        }

        public static CibmtrItem Convert(FacilityCibmtr item)
        {
            if (item == null) return null;

            return new CibmtrItem
            {
                Id = item.Id,
                CenterNumber = item.CenterNumber,
                CcnName = item.CcnName,
                TransplantSurvivalReportName = item.TransplantSurvivalReportName,
                DisplayName = item.DisplayName,
                IsNonCibmtr = item.IsNonCibmtr,
                IsActive = item.IsActive,
                CibmtrDataMgmts = item.FacilityCibmtrDataManagements.Select(Convert).ToList(),
                CibmtrOutcomeAnalyses = item.FacilityOutcomeAnalyses.Select(Convert).ToList()
            };
        }

        public static CompAppApproval Convert(ComplianceApplicationSubmitApproval item)
        {

            if (item == null) return null;

            return new CompAppApproval
            {
                FirstName = item.User.FirstName,
                LastName = item.User.LastName,
                EmailAddress = item.User.EmailAddress,
                IsApproved = item.IsApproved,
                ApprovalDate = item.UpdatedDate
            };
        }

        public static CompAppApproval Convert(ApplicationSubmitApproval item)
        {

            if (item == null) return null;

            return new CompAppApproval
            {
                FirstName = item.User.FirstName,
                LastName = item.User.LastName,
                EmailAddress = item.User.EmailAddress,
                IsApproved = item.IsApproved,
                ApprovalDate = item.UpdatedDate
            };
        }

        public static CompAppInspectionDetail Convert(ComplianceApplicationInspectionDetail item)
        {
            if (item == null) return null;

            return new CompAppInspectionDetail
            {
                Id = item.Id,
                ComplianceApplicationId = item.ComplianceApplicationId,
                InspectionScheduleId = item.InspectionScheduleId,
                InspectorsNeeded = item.InspectorsNeeded,
                ClinicalNeeded = item.ClinicalNeeded,
                AdultSimpleExperienceNeeded = item.AdultSimpleExperienceNeeded,
                AdultMediumExperienceNeeded = item.AdultMediumExperienceNeeded,
                AdultAnyExperienceNeeded = item.AdultAnyExperienceNeeded,
                PediatricSimpleExperienceNeeded = item.PediatricSimpleExperienceNeeded,
                PediatricMediumExperienceNeeded = item.PediatricMediumExperienceNeeded,
                PediatricAnyExperienceNeeded = item.PediatricAnyExperienceNeeded,
                Comments = item.Comments
            };
        }
    }
}















