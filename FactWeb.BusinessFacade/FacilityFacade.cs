using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class FacilityFacade
    {
        private readonly Container container;

        public FacilityFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<Facility> GetAll()
        {
            var manager = this.container.GetInstance<FacilityManager>();

            return manager.GetAllFacilities();
        }

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<Facility>> GetAllAsync()
        {
            var manager = this.container.GetInstance<FacilityManager>();

            return manager.GetAllAsync();
        }

        /// <summary>
        /// Gets all active records asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<Facility>> GetAllActiveAsync()
        {
            var manager = this.container.GetInstance<FacilityManager>();

            return manager.GetAllActiveAsync();
        }

        /// <summary>
        /// Gets all active records
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<Facility> GetAllActive()
        {
            var manager = this.container.GetInstance<FacilityManager>();

            return manager.GetAllActive();
        }

        /// <summary>
        /// Gets facility by its ID
        /// </summary>
        /// <returns>Facility Object of entity objects</returns>
        public Facility GetByID(int facilityId)
        {
            var facilityManager = this.container.GetInstance<FacilityManager>();

            return facilityManager.GetById(facilityId);
        }

        /// <summary>
        /// Gets all Master ServiceType records
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<MasterServiceType> GetMasterServiceTypes()
        {
            var manager = this.container.GetInstance<MasterServiceTypeManager>();
            return manager.GetAll();
        }

        /// <summary>
        /// Gets all Master ServiceType records asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<MasterServiceType>> GetMasterServiceTypesAsync()
        {
            var manager = this.container.GetInstance<MasterServiceTypeManager>();
            return manager.GetAllAsync();
        }

        public Task<string> GetCBCollectionSiteTypes(int facilityId)
        {
            var manager = this.container.GetInstance<FacilityManager>();
            return manager.GetCBCollectionSiteTypes(facilityId);
        }

        /// <summary>
        /// Gets all ServiceType records
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<ServiceType> GetServiceTypes()
        {
            var manager = this.container.GetInstance<ServiceTypeManager>();
            return manager.GetAll();
        }

        /// <summary>
        /// Gets all ServiceType records asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<ServiceType>> GetServiceTypesAsync()
        {
            var manager = this.container.GetInstance<ServiceTypeManager>();
            return manager.GetAllAsync();
        }

        /// <summary>
        /// Gets all Facility Accredidations records
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<FacilityAccreditation> GetFacilityAccredidations()
        {
            var manager = this.container.GetInstance<FacilityAccreditationManager>();
            return manager.GetAll();
        }

        /// <summary>
        /// Gets all Facility Accredidations  records asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<FacilityAccreditation>> GetFacilityAccredidationsAsync()
        {
            var manager = this.container.GetInstance<FacilityAccreditationManager>();
            return manager.GetAllAsync();
        }

        /// <summary>
        /// Add or update facility 
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Facility Save(FacilityItems facilityItem, string updatedBy)
        {
            var manager = this.container.GetInstance<FacilityManager>();
            var facility = manager.AddOrUpdate(facilityItem, updatedBy);
            UpdateFacilityAccreditationMapping(facilityItem, updatedBy, facility.Id);
            return facility;
        }


        /// <summary>
        /// Add or update facility asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public async Task<Facility> SaveAsync(FacilityItems facilityItem, string updatedBy)
        {
            var manager = this.container.GetInstance<FacilityManager>();            
            var facility = manager.AddOrUpdate(facilityItem, updatedBy);
            await Task.WhenAll(Task.FromResult(facility));
            this.UpdateFacilityAccreditationMapping(facilityItem, updatedBy, facility.Id);
            await this.InvalidateCacheAsync(updatedBy);
            return facility;
        }

        public bool UpdateFacilityAccreditationMapping(FacilityItems facilityItem, string email, int facilityId)
        {
            var facilityAccreditationMappingManager = this.container.GetInstance<FacilityAccreditationMappingManager>();
            var existingfacilityAccreditation = facilityAccreditationMappingManager.GetByFacilityId(facilityItem.FacilityId);

            foreach (var item in existingfacilityAccreditation)// remove previous records
            {
                facilityAccreditationMappingManager.BatchRemove(item);
            }

            facilityAccreditationMappingManager.SaveChanges();

            foreach (var facilityAccreditation in facilityItem.FacilityAccreditation)
            {
                if (facilityAccreditation.isSelected.HasValue && facilityAccreditation.isSelected.Value == true)
                {
                    FacilityAccreditationMapping facilityAccreditationMapping = new FacilityAccreditationMapping();
                    facilityAccreditationMapping.FacilityId = facilityId;
                    facilityAccreditationMapping.FacilityAccreditationId = facilityAccreditation.Id;
                    facilityAccreditationMapping.CreatedBy = email;
                    facilityAccreditationMapping.CreatedDate = DateTime.Now;

                    facilityAccreditationMappingManager.Add(facilityAccreditationMapping);
                }
            }

            this.InvalidateCache(email);
            return true;
        }

        /// <summary>
        /// Delete facility asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public async Task<Facility> DeleteAsync(int facilityId, string updatedBy)
        {
            var manager = this.container.GetInstance<FacilityManager>();
            var fac = await manager.DeleteAsync(facilityId, updatedBy);

            await this.InvalidateCacheAsync(updatedBy);

            return fac;
        }

        /// <summary>
        /// Delete facility 
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Facility Delete(int facilityId, string updatedBy)
        {
            var manager = this.container.GetInstance<FacilityManager>();
            var fac = manager.Delete(facilityId, updatedBy);

            this.InvalidateCache(updatedBy);

            return fac;
        }

        public async Task InvalidateCacheAsync(string updatedBy)
        {
            var cacheStatusManager = this.container.GetInstance<CacheStatusManager>();
            await cacheStatusManager.UpdateOrgFacSiteCacheDateAsync(updatedBy);
        }

        public void InvalidateCache(string updatedBy)
        {
            var cacheStatusManager = this.container.GetInstance<CacheStatusManager>();
            cacheStatusManager.UpdateOrgFacSiteCacheDate(updatedBy);
        }

        public List<CibmtrItem> GetAllForOrg(string orgName)
        {
            var result = new List<CibmtrItem>();
            var facilityCibmtrManager = this.container.GetInstance<FacilityCibmtrManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var org = orgManager.GetByName(orgName);

            if (org == null)
            {
                return null;
            }

            foreach (var orgFac in org.OrganizationFacilities)
            {
                var items = facilityCibmtrManager.GetAllByFacility(orgFac.FacilityId);

                result.AddRange(items.Select(ModelConversions.Convert).ToList());
            }

            result = result.OrderBy(x => x.CenterNumber).ToList();

            return result;
        }

        public List<FacilityCibmtr> GetAllCibmtrForFacility(string facilityName)
        {
            var facilityCibmtrManager = this.container.GetInstance<FacilityCibmtrManager>();

            return facilityCibmtrManager.GetAllByFacility(facilityName);
        }

        private bool IsCibmtrNameUnique(string name)
        {
            var facilityCibmtrManager = this.container.GetInstance<FacilityCibmtrManager>();

            var record = facilityCibmtrManager.GetByName(name);

            return record == null;
        }

        public FacilityCibmtr SaveCibmtr(CibmtrItem item, string savedBy)
        {
            var facilityCibmtrManager = this.container.GetInstance<FacilityCibmtrManager>();
            var facilityManager = this.container.GetInstance<FacilityManager>();

            if (item.Id.HasValue)
            {
                var record = facilityCibmtrManager.GetById(item.Id.Value);

                if (record == null)
                {
                    throw new Exception("Cannot find CIBMTR record");
                }

                if (record.CenterNumber != item.CenterNumber && !this.IsCibmtrNameUnique(record.CenterNumber))
                {
                    throw new Exception($"{item.CenterNumber} is not unique. Please enter a unique name.");
                }

                record.CenterNumber = item.CenterNumber;
                record.CcnName = item.CcnName;
                record.TransplantSurvivalReportName = item.TransplantSurvivalReportName;
                record.DisplayName = item.DisplayName;
                record.IsNonCibmtr = item.IsNonCibmtr;
                record.IsActive = item.IsActive;
                record.UpdatedBy = savedBy;
                record.UpdatedDate = DateTime.Now;

                facilityCibmtrManager.Save(record);

                return record;
            }
            else
            {
                var facility = facilityManager.GetByName(item.FacilityName);

                if (facility == null)
                {
                    throw new Exception($"Cannot find Facility {item.FacilityName}");
                }

                if (!this.IsCibmtrNameUnique(item.CenterNumber))
                {
                    throw new Exception($"{item.CenterNumber} is not unique. Please enter a unique name.");
                }

                var record = new FacilityCibmtr
                {
                    Id = Guid.NewGuid(),
                    CenterNumber = item.CenterNumber,
                    CcnName = item.CcnName,
                    TransplantSurvivalReportName = item.TransplantSurvivalReportName,
                    DisplayName = item.DisplayName,
                    IsNonCibmtr = item.IsNonCibmtr,
                    FacilityId = facility.Id,
                    CreatedBy = savedBy,
                    CreatedDate = DateTime.Now,
                    IsActive = item.IsActive
                };

                facilityCibmtrManager.Add(record);

                return record;
            }
        }

        public FacilityCibmtrOutcomeAnalysis SaveCibmtrOutcome(CibmtrOutcomeAnalysis item, string savedBy)
        {
            var cibmtrOutcomeManager = this.container.GetInstance<FacilityCibmtrOutcomeAnalysisManager>();

            if (item.Id.HasValue)
            {
                var record = cibmtrOutcomeManager.GetById(item.Id.Value);

                if (record == null)
                {
                    throw new Exception("Cannot find Outcome Analysis Record. Please contact support.");
                }

                record.ReportYear = item.ReportYear;
                record.SurvivalScore = item.SurvivalScore;
                record.SampleSize = item.SampleSize;
                record.ActualPercent = item.ActualPercent;
                record.PredictedPercent = item.PredictedPercent;
                record.LowerPercent = item.LowerPercent;
                record.UpperPercent = item.UpperPercent;
                record.ComparativeDataSource = item.ComparativeDataSource;
                record.PublishedOneYearSurvival = item.PublishedOneYearSurvival;
                record.ProgramOneYearSurvival = item.ProgramOneYearSurvival;
                record.Comments = item.Comments;
                record.ReportedCausesOfDeath = item.ReportedCausesOfDeath;
                record.CorrectiveActions = item.CorrectiveActions;
                record.FactImprovementPlan = item.FactImprovementPlan;
                record.AdditionalInformationRequested = item.AdditionalInformationRequested;
                record.ProgressOnImplementation = item.ProgressOnImplementation;
                record.InspectorInformation = item.InspectorInformation;
                record.InspectorCommendablePractices = item.InspectorCommendablePractices;
                record.Inspector100DaySurvival = item.Inspector100DaySurvival;
                record.Inspector1YearSurvival = item.Inspector1YearSurvival;
                record.IsNotRequired = item.IsNotRequired;
                record.UpdatedBy = savedBy;
                record.UpdatedDate = DateTime.Now;

                cibmtrOutcomeManager.Save(record);

                return record;
            }
            else
            {
                var record = new FacilityCibmtrOutcomeAnalysis
                {
                    Id = Guid.NewGuid(),
                    FacilityCibmtrId = item.CibmtrId.GetValueOrDefault(),
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
                    InspectorCommendablePractices =  item.InspectorCommendablePractices,
                    InspectorInformation = item.InspectorInformation,
                    Inspector100DaySurvival = item.Inspector100DaySurvival,
                    Inspector1YearSurvival = item.Inspector1YearSurvival,
                    IsNotRequired = item.IsNotRequired,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy
                };

                cibmtrOutcomeManager.Add(record);

                return record;
            }
        }

        public FacilityCibmtrDataManagement SaveCibmtrData(CibmtrDataMgmt item, string savedBy)
        {
            var cibmtrDataManager = this.container.GetInstance<FacilityCibmtrDataManagementManager>();

            if (item.Id.HasValue)
            {
                var record = cibmtrDataManager.GetById(item.Id.Value);

                if (record == null)
                {
                    throw new Exception("Data Management record not found. Please contact support.");
                }

                record.AuditDate = item.AuditDate;
                record.CriticalFieldErrorRate = item.CriticalFieldErrorRate;
                record.RandomFieldErrorRate = item.RandomFieldErrorRate;
                record.OverallFieldErrorRate = item.OverallFieldErrorRate;
                record.IsCapIdentified = item.IsCapIdentified;
                record.AuditorComments = item.AuditorComments;
                record.CpiLetterDate = item.CpiLetterDate;
                record.CpiTypeId = item.CpiTypeId;
                record.CpiComments = item.CpiComments;
                record.CorrectiveActions = item.CorrectiveActions;
                record.FactProgressDetermination = item.FactProgressDetermination;
                record.IsAuditAccuracyRequired = item.IsAuditAccuracyRequired;
                record.AdditionalInformation = item.AdditionalInformation;
                record.ProgressOnImplementation = item.ProgressOnImplementation;
                record.InspectorCommendablePractices = item.InspectorCommendablePractices;
                record.Inspector100DaySurvival = item.Inspector100DaySurvival;
                record.Inspector1YearSurvival = item.Inspector1YearSurvival;
                record.InspectorInformation = item.InspectorInformation;
                record.UpdatedBy = savedBy;
                record.UpdatedDate = DateTime.Now;

                cibmtrDataManager.Save(record);

                return record;
            }
            else
            {
                var record = new FacilityCibmtrDataManagement
                {
                    Id = Guid.NewGuid(),
                    FacilityCibmtrId = item.CibmtrId,
                    AuditDate = item.AuditDate,
                    CriticalFieldErrorRate = item.CriticalFieldErrorRate,
                    RandomFieldErrorRate = item.RandomFieldErrorRate,
                    OverallFieldErrorRate = item.OverallFieldErrorRate,
                    IsCapIdentified = item.IsCapIdentified,
                    AuditorComments = item.AuditorComments,
                    CpiLetterDate = item.CpiLetterDate,
                    CpiTypeId = item.CpiTypeId,
                    CpiComments = item.CpiComments,
                    CorrectiveActions = item.CorrectiveActions,
                    FactProgressDetermination = item.FactProgressDetermination,
                    IsAuditAccuracyRequired = item.IsAuditAccuracyRequired,
                    AdditionalInformation = item.AdditionalInformation,
                    ProgressOnImplementation = item.ProgressOnImplementation,
                    InspectorCommendablePractices = item.InspectorCommendablePractices,
                    Inspector100DaySurvival = item.Inspector100DaySurvival,
                    Inspector1YearSurvival = item.Inspector1YearSurvival,
                    InspectorInformation = item.InspectorInformation,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy
                };

                cibmtrDataManager.Add(record);

                return record;
            }
        }
    }
}
