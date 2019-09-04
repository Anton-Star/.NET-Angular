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
    public class SiteFacade
    {
        private readonly Container container;

        public SiteFacade(Container container)
        {
            this.container = container;
        }

        public List<Site> GetAllSites()
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.GetAllSites();
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<Site> GetAll()
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.GetAll();
        }

        public Site GetByName(string name)
        {
            var manager = this.container.GetInstance<SiteManager>();

            return manager.GetByName(name);
        }

        public List<Site> GetSitesByCompliance(Guid complianceAppId)
        {
            var manager = this.container.GetInstance<SiteManager>();

            return manager.GetSitesByComplianceNoFacility(complianceAppId);
        }

        public List<Site> GetSitesByComplianceAppId(int appId)
        {
            var manager = this.container.GetInstance<SiteManager>();
            var appManager = this.container.GetInstance<ApplicationManager>();

            var app = appManager.GetById(appId);

            if (app == null || app.ComplianceApplicationId == null) return null;

            return this.GetSitesByCompliance(app.ComplianceApplicationId.Value);
        }

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<Site>> GetAllAsync()
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.GetAllAsync();
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Collection of site objects</returns>
        public Task<List<Site>> SearchAsync(int? siteId)
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.SearchAsync(siteId);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Collection of site objects</returns>
        public List<Site> Search(int? siteId)
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.Search(siteId);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="siteName">Name of the site</param>
        /// <returns>Collection of site objects</returns>
        public Task<List<Site>> SearchAsync(string siteName)
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.SearchAsync(siteName);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="siteName">Name of the organization</param>
        /// <returns>Collection of site objects</returns>
        public List<Site> Search(string siteName)
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.Search(siteName);
        }

        /// <summary>
        /// Saves new Site
        /// </summary>
        /// <param name="siteItem"></param>
        /// <returns></returns>
        public async Task<Site> SaveAsync(SiteItems siteItem, string email)
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            var site = siteManager.Save(siteItem, email);

            ManageSiteAddresses(siteItem, site, email);

            var siteScopeTypeManager = this.container.GetInstance<SiteScopeTypeManager>();

            siteScopeTypeManager.AddorUpdateSiteScopeType(siteItem, site.Id, email);

            var siteTransplantTypeManager = this.container.GetInstance<SiteTransplantTypeManager>();

            siteTransplantTypeManager.AddorUpdateSiteTransplantType(siteItem, site.Id, email);

            var siteClinicalTypeManager = this.container.GetInstance<SiteClinicalTypeManager>();

            siteClinicalTypeManager.AddorUpdateSiteClinicalType(siteItem, site.Id, email);

            var siteProcessingTypeManager = this.container.GetInstance<SiteProcessingTypeManager>();

            siteProcessingTypeManager.AddorUpdateSiteProcessingType(siteItem, site.Id, email);

            if (siteItem.SiteFacilityId != 0)
            {
                var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();

                var facilitySites = facilitySiteManager.GetBySiteId(site.Id);

                if (facilitySites != null)
                {
                    foreach (var facSite in facilitySites)
                    {
                        facilitySiteManager.Remove(facSite.Id);
                    }
                }

                facilitySiteManager.AddRelation(site.Id, siteItem.SiteFacilityId, email);
            }

            await this.InvalidateCacheAsync(email);

            //var temp = siteManager.GetById(site.Id);

            return site;
            
        }

        private void ManageSiteAddresses(SiteItems siteItem, Site site, string email)
        {
            var siteManager = this.container.GetInstance<SiteManager>();
            var siteAddressManager = this.container.GetInstance<SiteAddressManager>();
            var addressManager = this.container.GetInstance<AddressManager>();
            var distanceManager = this.container.GetInstance<DistanceManager>();

            if (siteItem.SiteId != 0) // site update case
            {
                var tempSite = siteManager.GetById(siteItem.SiteId);

                if (tempSite != null && tempSite.SiteAddresses != null)
                {
                    var primaryAddress = tempSite.SiteAddresses.FirstOrDefault(x => x.IsPrimaryAddress == true);

                    if (primaryAddress != null) // update primary address
                    {
                        var address = addressManager.GetById(primaryAddress.AddressId);
                        address.Street1 = siteItem.SiteStreetAddress1;
                        address.Street2 = siteItem.SiteStreetAddress2;
                        address.City = siteItem.SiteCity;
                        address.StateId = siteItem.SiteState?.Id;
                        address.Province = siteItem.SiteProvince;
                        address.Phone = siteItem.SitePhone;
                        address.ZipCode = siteItem.SiteZip;
                        address.CountryId = siteItem.SiteCountry?.Id ?? 0;
                        address.UpdatedBy = email;
                        address.UpdatedDate = DateTime.Now;
                        addressManager.Save(address);
                    }
                    else //add primary address
                    {
                        var address = new Address
                        {
                            AddressTypeId = (int) Constants.AddressType.Office,
                            Street1 = siteItem.SiteStreetAddress1,
                            Street2 = siteItem.SiteStreetAddress2,
                            City = siteItem.SiteCity,
                            StateId = siteItem.SiteState?.Id,
                            Province = siteItem.SiteProvince,
                            ZipCode = siteItem.SiteZip,
                            CountryId = siteItem.SiteCountry?.Id ?? 0,
                            Phone = siteItem.SitePhone,
                            CreatedBy = email,
                            CreatedDate = DateTime.Now
                        };
                        addressManager.Add(address);

                        var siteAddress = new SiteAddress
                        {
                            SiteId = siteItem.SiteId,
                            AddressId = address.Id,
                            IsPrimaryAddress = true,
                            CreatedBy = email,
                            CreatedDate = DateTime.Now
                        };

                        siteAddressManager.Add(siteAddress);
                    }

                    if (siteItem.SiteAddresses != null)
                    {
                        foreach (var siteAddressItem in siteItem.SiteAddresses)
                        {
                            if (siteAddressItem.Id == 0) //new site address
                            {
                                var address = new Address
                                {
                                    AddressTypeId = siteAddressItem.Address.AddressType.Id,
                                    Street1 = siteAddressItem.Address.Street1,
                                    Street2 = siteAddressItem.Address.Street2,
                                    City = siteAddressItem.Address.City,
                                    StateId =
                                        siteAddressItem.Address.State?.Id,
                                    Province = siteAddressItem.Address.Province,
                                    ZipCode = siteAddressItem.Address.ZipCode,
                                    CountryId = siteAddressItem.Address.Country.Id,
                                    Phone = siteAddressItem.Address.Phone,
                                    CreatedBy = email,
                                    CreatedDate = DateTime.Now
                                };
                                //address.Phone = siteAddressItem.Address.Phone;

                                addressManager.Add(address);

                                var siteAddress = new SiteAddress
                                {
                                    SiteId = siteItem.SiteId,
                                    AddressId = address.Id,
                                    IsPrimaryAddress = false,
                                    CreatedBy = email,
                                    CreatedDate = DateTime.Now
                                };

                                siteAddressManager.Add(siteAddress);
                            }
                            else  // update site address
                            {
                                var address = addressManager.GetById(siteAddressItem.AddressId);
                                address.Street1 = siteAddressItem.Address.Street1;
                                address.Street2 = siteAddressItem.Address.Street2;
                                address.City = siteAddressItem.Address.City;
                                address.StateId = siteAddressItem.Address.State?.Id;
                                address.Province = siteAddressItem.Address.Province;
                                address.Phone = siteAddressItem.Address.Phone;
                                address.ZipCode = siteAddressItem.Address.ZipCode;
                                address.CountryId = siteAddressItem.Address.Country?.Id;
                                address.UpdatedBy = email;
                                address.UpdatedDate = DateTime.Now;
                                addressManager.Save(address);
                            }

                        }

                        // delete site address 
                        foreach (var siteAddress in tempSite.SiteAddresses.Where(x => x.IsPrimaryAddress != true))
                        {
                            var found = siteItem.SiteAddresses.Any(x => x.Id == siteAddress.Id);

                            if (!found)
                            {
                                var distanceList = siteAddress.Distances;

                                if (distanceList != null)
                                {
                                    foreach (var distance in distanceList)
                                    {
                                        distanceManager.Remove(distance.Id);
                                    }
                                }                                

                                siteAddressManager.Remove(siteAddress.Id);
                            }
                        }
                    }

                    this.InvalidateCache(email);
                }
            }
            else // new addresses for new site
            {
                var address = new Address
                {
                    AddressTypeId = (int) Constants.AddressType.Office,
                    Street1 = siteItem.SiteStreetAddress1,
                    Street2 = siteItem.SiteStreetAddress2,
                    City = siteItem.SiteCity,
                    StateId = siteItem.SiteState?.Id,
                    Province = siteItem.SiteProvince,
                    ZipCode = siteItem.SiteZip,
                    CountryId = siteItem.SiteCountry?.Id ?? 0,
                    Phone = siteItem.SitePhone,
                    CreatedBy = email,
                    CreatedDate = DateTime.Now
                };
                //address.Phone = site.Phone;

                addressManager.Add(address);

                var siteAddress = new SiteAddress
                {
                    SiteId = site.Id,
                    AddressId = address.Id,
                    IsPrimaryAddress = true,
                    CreatedBy = email,
                    CreatedDate = DateTime.Now
                };

                siteAddressManager.Add(siteAddress);
            }
        }


        /// <summary>
        /// Get all states list
        /// </summary>
        /// <returns></returns>
        public Task<List<State>> GetStatesListAsync()
        {
            var stateManager = this.container.GetInstance<StateManager>();

            return stateManager.GetAllAsync();
        }

        /// <summary>
        /// Get all countries list
        /// </summary>
        /// <returns></returns>
        public Task<List<Country>> GetCountriesListAsync()
        {
            var countryManager = this.container.GetInstance<CountryManager>();

            return countryManager.GetAllAsync();
        }

        /// <summary>
        /// Get all Address Types list
        /// </summary>
        /// <returns></returns>
        public Task<List<AddressType>> GetAddressTypesListAsync()
        {
            var addressTypeManager = this.container.GetInstance<AddressTypeManager>();

            return addressTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all the clinical types
        /// </summary>
        /// <returns></returns>
        public Task<List<ClinicalType>> GetClinicalTypesAsync()
        {
            var clinicalTypeManager = this.container.GetInstance<ClinicalTypeManager>();

            return clinicalTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all the processing types
        /// </summary>
        /// <returns></returns>
        public Task<List<ProcessingType>> GetProcessingTypesAsync()
        {
            var processingTypeManager = this.container.GetInstance<ProcessingTypeManager>();

            return processingTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all the Collection Product types
        /// </summary>
        /// <returns></returns>
        public Task<List<CollectionProductType>> GetCollectionProductTypesAsync()
        {
            var collectionProductTypeManager = this.container.GetInstance<CollectionProductTypeManager>();

            return collectionProductTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all the CB Collection types
        /// </summary>
        /// <returns></returns>
        public Task<List<CBCollectionType>> GetCBCollectionTypesAsync()
        {
            var cbCollectionTypeManager = this.container.GetInstance<CBCollectionTypeManager>();

            return cbCollectionTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all the CB unit types
        /// </summary>
        /// <returns></returns>
        public Task<List<CBUnitType>> GetCBUnitTypesAsync()
        {
            var cbUnitTypeManager = this.container.GetInstance<CBUnitTypeManager>();

            return cbUnitTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all the Clinical Population types
        /// </summary>
        /// <returns></returns>
        public Task<List<ClinicalPopulationType>> GetClinicalPopulationTypesAsync()
        {
            var clinicalPopulationTypeManager = this.container.GetInstance<ClinicalPopulationTypeManager>();

            return clinicalPopulationTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all the Transplant types
        /// </summary>
        /// <returns></returns>
        public Task<List<TransplantType>> GetTransplantTypesAsync()
        {
            var transplantTypeManager = this.container.GetInstance<TransplantTypeManager>();

            return transplantTypeManager.GetAllAsync();
        }

        public List<CBCategory> GetAllCbCategories()
        {
            var cbCategoryManager = this.container.GetInstance<CBCategoryManager>();

            return cbCategoryManager.GetAll();
        }

        public Task<List<CBCategory>> GetAllCbCategoriesAsync()
        {
            var cbCategoryManager = this.container.GetInstance<CBCategoryManager>();

            return cbCategoryManager.GetAllAsync();
        }

        public Task<Site> GetPrimarySiteAsync(long organizationId)
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.GetPrimarySiteAsync(organizationId);
        }

        public List<Site> GetSitesByOrganizationId(long organizationId)
        {
            var siteManager = this.container.GetInstance<SiteManager>();

            return siteManager.GetSitesByOrganizationId(organizationId);
        }

        public SiteCordBloodTransplantTotal SaveSiteCordBloodTransplantTotal(SiteCordBloodTransplantTotalItem item, string savedBy)
        {
            var siteCordbloodTransplantTotalManager = this.container.GetInstance<SiteCordBloodTransplantTotalManager>();
            var cbUnitTypeManager = this.container.GetInstance<CBUnitTypeManager>();
            var cbCategoryManager = this.container.GetInstance<CBCategoryManager>();

            var unitType = cbUnitTypeManager.GetByName(item.CbUnitType.Name);
            var category = cbCategoryManager.GetByName(item.CbCategory.Name);

            if (unitType == null || category == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = siteCordbloodTransplantTotalManager.SaveSiteCordBloodTransplantTotal(unitType, category, item.Id,
                item.SiteId, item.NumberOfUnits, Convert.ToDateTime(item.AsOfDate), savedBy);

            this.InvalidateCache(savedBy);

            return result;
        }

        public async Task<SiteCordBloodTransplantTotal> SaveSiteCordBloodTransplantTotalAsync(SiteCordBloodTransplantTotalItem item, string savedBy)
        {
            var siteCordbloodTransplantTotalManager = this.container.GetInstance<SiteCordBloodTransplantTotalManager>();
            var cbUnitTypeManager = this.container.GetInstance<CBUnitTypeManager>();
            var cbCategoryManager = this.container.GetInstance<CBCategoryManager>();

            var unitTypeTask = cbUnitTypeManager.GetByNameAsync(item.CbUnitType.Name);
            var categoryTask = cbCategoryManager.GetByNameAsync(item.CbCategory.Name);

            await Task.WhenAll(unitTypeTask, categoryTask);

            if (unitTypeTask == null || categoryTask == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = await siteCordbloodTransplantTotalManager.SaveSiteCordBloodTransplantTotalAsync(unitTypeTask.Result, categoryTask.Result, item.Id,
                item.SiteId, item.NumberOfUnits, Convert.ToDateTime(item.AsOfDate), savedBy);

            await this.InvalidateCacheAsync(savedBy);

            return result;
        }

        public async Task<List<TransplantCellType>> GetAllTransplantCellTypesAsync()
        {
            var manager = this.container.GetInstance<TransplantCellTypeManager>();

            return await manager.GetAllAsync();
        }

        public List<TransplantCellType> GetAllTransplantCellTypes()
        {
            var manager = this.container.GetInstance<TransplantCellTypeManager>();

            return manager.GetAll();
        }

        public async Task<List<CollectionType>> GetAllCollectionTypesAsync()
        {
            var manager = this.container.GetInstance<CollectionTypeManager>();

            return await manager.GetAllAsync();
        }

        public List<CollectionType> GetAllCollectionTypes()
        {
            var manager = this.container.GetInstance<CollectionTypeManager>();

            return manager.GetAll();
        }

        public SiteTransplantTotal SaveSiteTransplantTotal(SiteTransplantTotalItem item, string savedBy)
        {
            var transplantCellTypeManager = this.container.GetInstance<TransplantCellTypeManager>();
            var clinicalPopulationTypeManager = this.container.GetInstance<ClinicalPopulationTypeManager>();
            var transplantTypeManager = this.container.GetInstance<TransplantTypeManager>();
            var siteTransplantTotalManager = this.container.GetInstance<SiteTransplantTotalManager>();

            var cellType = transplantCellTypeManager.GetByName(item.TransplantCellType.Name);
            var popType = clinicalPopulationTypeManager.GetByName(item.ClinicalPopulationType.Name);
            var transplantType = transplantTypeManager.GetByName(item.TransplantType.Name);

            if (cellType == null || popType == null || transplantType == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = siteTransplantTotalManager.SaveSiteTransplantTotal(cellType, popType, transplantType, item.Id,
                item.SiteId, item.IsHaploid, item.NumberOfUnits, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            this.InvalidateCache(savedBy);

            return result;
        }

        public async Task<SiteTransplantTotal> SaveSiteTransplantTotalAsync(SiteTransplantTotalItem item, string savedBy)
        {
            var transplantCellTypeManager = this.container.GetInstance<TransplantCellTypeManager>();
            var clinicalPopulationTypeManager = this.container.GetInstance<ClinicalPopulationTypeManager>();
            var transplantTypeManager = this.container.GetInstance<TransplantTypeManager>();
            var siteTransplantTotalManager = this.container.GetInstance<SiteTransplantTotalManager>();

            var cellTypeTask = transplantCellTypeManager.GetByNameAsync(item.TransplantCellType.Name);
            var popTypeTask = clinicalPopulationTypeManager.GetByNameAsync(item.ClinicalPopulationType.Name);
            var transplantTypeTask = transplantTypeManager.GetByNameAsync(item.TransplantType.Name);

            await Task.WhenAll(cellTypeTask, popTypeTask, transplantTypeTask);

            if (cellTypeTask.Result == null || popTypeTask.Result == null || transplantTypeTask.Result == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = await siteTransplantTotalManager.SaveSiteTransplantTotalAsync(cellTypeTask.Result, popTypeTask.Result, transplantTypeTask.Result, item.Id,
                item.SiteId, item.IsHaploid, item.NumberOfUnits, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            await this.InvalidateCacheAsync(savedBy);

            return result;
        }

        public SiteCollectionTotal SaveSiteCollectionTotal(SiteCollectionTotalItem item, string savedBy)
        {
            var collectionTypeManager = this.container.GetInstance<CollectionTypeManager>();
            var clinicalPopulationTypeManager = this.container.GetInstance<ClinicalPopulationTypeManager>();
            var siteCollectionTotalManager = this.container.GetInstance<SiteCollectionTotalManager>();

            var collectionType = collectionTypeManager.GetByName(item.CollectionType.Name);
            var popType = clinicalPopulationTypeManager.GetByName(item.ClinicalPopulationType.Name);

            if (collectionType == null || popType == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = siteCollectionTotalManager.SaveSiteCollectionTotal(collectionType, popType, item.Id,
                item.SiteId, item.NumberOfUnits, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            this.InvalidateCache(savedBy);

            return result;
        }

        public async Task<SiteCollectionTotal> SaveSiteCollectionTotalAsync(SiteCollectionTotalItem item, string savedBy)
        {
            var collectionTypeManager = this.container.GetInstance<CollectionTypeManager>();
            var clinicalPopulationTypeManager = this.container.GetInstance<ClinicalPopulationTypeManager>();
            var siteCollectionTotalManager = this.container.GetInstance<SiteCollectionTotalManager>();

            var collectionTypeTask = collectionTypeManager.GetByNameAsync(item.CollectionType.Name);
            var popTypeTask = clinicalPopulationTypeManager.GetByNameAsync(item.ClinicalPopulationType.Name);

            await Task.WhenAll(collectionTypeTask, popTypeTask);

            if (collectionTypeTask.Result == null || popTypeTask.Result == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = await siteCollectionTotalManager.SaveSiteCollectionTotalAsync(collectionTypeTask.Result, popTypeTask.Result, item.Id,
                item.SiteId, item.NumberOfUnits, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            await this.InvalidateCacheAsync(savedBy);

            return result;
        }

        public SiteProcessingTotalItem SaveSiteProcessingTotal(SiteProcessingTotalItem item, string savedBy)
        {
            var cellTypeManager = this.container.GetInstance<TransplantCellTypeManager>();
            var totalManager = this.container.GetInstance<SiteProcessingTotalManager>();
            var cellTypes = new List<TransplantCellType>();

            foreach (var cellType in item.SelectedTypes.Select(cellTypeName => cellTypeManager.GetByName(cellTypeName)))
            {
                if (cellType == null)
                {
                    throw new Exception("Cannot find data");
                }

                cellTypes.Add(cellType);
            }

            var row = totalManager.Save(cellTypes, item.Id,
                    item.SiteId, item.NumberOfUnits, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            this.InvalidateCache(savedBy);

            var conv = ModelConversions.Convert(row);

            conv.SiteProcessingTotalTransplantCellTypes = cellTypes.Select(x => new SiteProcessingTotalTransplantCellTypeItem
            {
                TransplantCellType = ModelConversions.Convert(x)
            })
            .ToList();

            return conv;
        }

        public async Task<SiteProcessingTotalItem> SaveSiteProcessingTotalAsync(SiteProcessingTotalItem item, string savedBy)
        {
            var cellTypeManager = this.container.GetInstance<TransplantCellTypeManager>();
            var totalManager = this.container.GetInstance<SiteProcessingTotalManager>();
            var cellTypes = new List<TransplantCellType>();

            foreach (var cellTypeName in item.SelectedTypes)
            {
                var cellType = await cellTypeManager.GetByNameAsync(cellTypeName);

                if (cellType == null)
                {
                    throw new Exception("Cannot find data");
                }

                cellTypes.Add(cellType);
            }

            var row = await totalManager.SaveAsync(cellTypes, item.Id,
                    item.SiteId, item.NumberOfUnits, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            await this.InvalidateCacheAsync(savedBy);

            var conv = ModelConversions.Convert(row);

            conv.SiteProcessingTotalTransplantCellTypes = cellTypes.Select(x => new SiteProcessingTotalTransplantCellTypeItem
            {
                TransplantCellType = ModelConversions.Convert(x)
            })
            .ToList();

            return conv;
        }

        public SiteProcessingMethodologyTotal SaveSiteProcessingMethodologyTotal(SiteProcessingMethodologyTotalItem item, string savedBy)
        {
            var processingTypeManager = this.container.GetInstance<ProcessingTypeManager>();
            var totalManager = this.container.GetInstance<SiteProcessingMethodologyTotalManager>();

            var processingType = processingTypeManager.GetByName(item.ProcessingType.Name);

            if (processingType == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = totalManager.Save(processingType, item.Id,
                item.SiteId, item.PlatformCount, item.ProtocolCount, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            this.InvalidateCache(savedBy);

            return result;
        }

        public async Task<SiteProcessingMethodologyTotal> SaveSiteProcessingMethodologyTotalAsync(SiteProcessingMethodologyTotalItem item, string savedBy)
        {
            var processingTypeManager = this.container.GetInstance<ProcessingTypeManager>();
            var totalManager = this.container.GetInstance<SiteProcessingMethodologyTotalManager>();

            var processingType = await processingTypeManager.GetByNameAsync(item.ProcessingType.Name);

            if (processingType == null)
            {
                throw new Exception("Cannot find data");
            }

            var result = await totalManager.SaveAsync(processingType, item.Id,
                item.SiteId, item.PlatformCount, item.ProtocolCount, Convert.ToDateTime(item.StartDate), Convert.ToDateTime(item.EndDate), savedBy);

            await this.InvalidateCacheAsync(savedBy);

            return result;
        }

        public void DeleteSiteTotal(Guid id, string type)
        {

            switch (type)
            {
                case "CB":
                    this.container.GetInstance<SiteCordBloodTransplantTotalManager>().Remove(id);
                    break;
                case "CO":
                    this.container.GetInstance<SiteCollectionTotalManager>().Remove(id);
                    break;
                case "MO":
                    this.container.GetInstance<SiteProcessingMethodologyTotalManager>().Remove(id);
                    break;
                case "TO":
                    this.container.GetInstance<SiteTransplantTotalManager>().Remove(id);
                    break;
                case "PO":
                    var mgr = this.container.GetInstance<SiteProcessingTotalTransplantCellTypeManager>();

                    var rec = mgr.GetByProcessingId(id);

                    foreach (var t in rec)
                    {
                        mgr.BatchRemove(t);
                    }

                    mgr.SaveChanges();

                    this.container.GetInstance<SiteProcessingTotalManager>().Remove(id);
                    break;
            }
            
        }

        public void DeleteSiteProcessingTotal(Guid id)
        {
            
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

        public List<FlatSite> GetFlatSites()
        {
            var manager = this.container.GetInstance<SiteManager>();

            return manager.GetFlatSites();
        }
    }
}

