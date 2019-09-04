using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class SiteManager : BaseManager<SiteManager, ISiteRepository, Site>
    {
        public SiteManager(ISiteRepository repository) : base(repository)
        {
        }

        public List<Site> GetAllSites()
        {
            LogMessage("GetAllSites (SiteManager)");

            return base.Repository.GetAllSites();
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="siteName">Name of the site</param>
        /// <returns>Collection of site objects</returns>
        public Task<List<Site>> SearchAsync(string siteName)
        {
            LogMessage("SearchAsync Site Name (SiteManager)");

            return base.Repository.SearchAsync(siteName);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="siteName">Name of the site</param>
        /// <returns>Collection of site objects</returns>
        public List<Site> Search(string siteName)
        {
            LogMessage("Search Site Name (SiteManager)");

            return base.Repository.Search(siteName);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Collection of site objects</returns>
        public Task<List<Site>> SearchAsync(int? siteId)
        {
            LogMessage("SearchAsync Site Id (SiteManager)");

            if (siteId.HasValue)
            {
                return Task.FromResult(new List<Site> { base.Repository.GetById(siteId.Value) });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Collection of site objects</returns>
        public List<Site> Search(int? siteId)
        {
            LogMessage("Search Site Id (SiteManager)");

            if (siteId.HasValue)
            {
                return new List<Site> { base.Repository.GetById(siteId.Value) };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets an site by its name
        /// </summary>
        /// <param name="siteName">Name of the Site</param>
        /// <returns>Site entity object</returns>
        public Site GetByName(string siteName)
        {
            LogMessage("GetByName (SiteManager)");

            return base.Repository.GetByName(siteName);
        }

        /// <summary>
        /// Gets an site by its name asynchronously
        /// </summary>
        /// <param name="siteName">Name of the Site</param>
        /// <returns>Site entity object</returns>
        public Task<Site> GetByNameAsync(string siteName)
        {
            LogMessage("GetByNameAsync (SiteManager)");

            return base.Repository.GetByNameAsync(siteName);
        }

        public Site Save(SiteItems siteItem, string email)
        {
            LogMessage("Save (SiteManager)");

            Site site = null;
            if (siteItem.SiteId != 0)
            {
                site = this.Update(siteItem, email);
            }
            else
            {
                site = this.Add(siteItem, email);
            }

            return site;
        }

        /// <summary>
        /// Add new site record
        /// </summary>
        /// <param name="siteItem"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Site Add(SiteItems siteItem, string email)
        {
            Site site = new Site();

            site.Name = siteItem.SiteName;
            site.StartDate = Convert.ToDateTime(siteItem.SiteStartDate);
            
            site.IsPrimarySite = siteItem.SiteIsPrimarySite;
            
            site.ClinicalPopulationTypeId = siteItem.SiteClinicalPopulationType == null ? default(int?) : siteItem.SiteClinicalPopulationType.Id;

            site.CollectionProductTypeId = siteItem.SiteCollectionProductType == null ? default(int?) : siteItem.SiteCollectionProductType.Id;
            site.CBCollectionTypeId = siteItem.SiteCBCollectionType == null ? default(int?) : siteItem.SiteCBCollectionType.Id;
            site.CBUnitTypeId = siteItem.SiteCBUnitType == null ? default(int?) : siteItem.SiteCBUnitType.Id;
            //site.CBUnitsBanked = siteItem.SiteUnitsBanked;

            //if (!string.IsNullOrWhiteSpace(siteItem.SiteUnitsBankedDate))
            //{
            //    site.CBUnitsBankDate = Convert.ToDateTime(siteItem.SiteUnitsBankedDate);
            //}

            site.CreatedBy = email;
            site.CreatedDate = DateTime.Now;

            if (siteItem.SiteAddresses != null)
            {
                site.SiteAddresses = new List<SiteAddress>();

                foreach (var siteAddressItem in siteItem.SiteAddresses)
                {
                    Address address = new Address();
                    address.AddressTypeId = siteAddressItem.Address.AddressType.Id;
                    address.Street1 = siteAddressItem.Address.Street1;
                    address.Street2 = siteAddressItem.Address.Street2;
                    address.City = siteAddressItem.Address.City;
                    address.StateId = siteAddressItem.Address.State != null ? siteAddressItem.Address.State.Id : (int?)null;
                    address.Province = siteAddressItem.Address.Province;
                    address.ZipCode = siteAddressItem.Address.ZipCode;
                    address.CountryId = siteAddressItem.Address.Country.Id;
                    address.Phone = siteItem.SitePhone;
                    address.CreatedBy = email;
                    address.CreatedDate = DateTime.Now;

                    SiteAddress siteAddress = new SiteAddress();
                    siteAddress.Address = address;
                    siteAddress.CreatedBy = email;
                    siteAddress.IsPrimaryAddress = false;
                    siteAddress.CreatedDate = DateTime.Now;
                    
                    site.SiteAddresses.Add(siteAddress);
                }
            }

            base.Repository.Add(site);

            return site;
        }

        /// <summary>
        /// Update site record
        /// </summary>
        /// <param name="siteItem"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Site Update(SiteItems siteItem, string email)
        {
            var site = base.Repository.GetById(siteItem.SiteId);

            site.Name = siteItem.SiteName;
            site.StartDate = Convert.ToDateTime(siteItem.SiteStartDate);

            //site.StreetAddress1 = siteItem.SiteStreetAddress1;
            //site.StreetAddress2 = siteItem.SiteStreetAddress2;
            //site.City = siteItem.SiteCity;
            //site.Zip = siteItem.SiteZip;

            //site.StateId = siteItem.SiteState == null ? default(int?) : siteItem.SiteState.Id == 0 ? default(int?) : siteItem.SiteState.Id;
            //site.Province = siteItem.SiteProvince;
            //site.CountryId = siteItem.SiteCountry.Id;

            //site.Phone = siteItem.SitePhone;

            site.IsPrimarySite = siteItem.SiteIsPrimarySite;

            //site.ClinicalTypeId = siteItem.SiteClinicalType == null ? default(int?) : siteItem.SiteClinicalType.Id;
            //site.ProcessingTypeId = siteItem.SiteProcessingType == null ? default(int?) : siteItem.SiteProcessingType.Id;
            site.CollectionProductTypeId = siteItem.SiteCollectionProductType == null || siteItem.SiteCollectionProductType.Id == 0 ? default(int?) : siteItem.SiteCollectionProductType.Id;
            site.ClinicalPopulationTypeId = siteItem.SiteClinicalPopulationType == null || siteItem.SiteClinicalPopulationType.Id == 0 ? default(int?) : siteItem.SiteClinicalPopulationType.Id;
            //site.TransplantTypeId = siteItem.SiteTransplantType == null ? default(int?) : siteItem.SiteTransplantType.Id;

            site.CBCollectionTypeId = siteItem.SiteCBCollectionType == null || siteItem.SiteCBCollectionType.Id == 0 ? default(int?) : siteItem.SiteCBCollectionType.Id;
            site.CBUnitTypeId = siteItem.SiteCBUnitType == null || siteItem.SiteCBUnitType.Id == 0 ? default(int?) : siteItem.SiteCBUnitType.Id;
            site.CBUnitsBanked = siteItem.SiteUnitsBanked;

            if (!string.IsNullOrWhiteSpace(siteItem.SiteUnitsBankedDate))
            {
                site.CBUnitsBankDate = Convert.ToDateTime(siteItem.SiteUnitsBankedDate);
            }

            if (site.SiteAddresses != null)
            {
                var address = site.SiteAddresses.FirstOrDefault(x => x.IsPrimaryAddress.GetValueOrDefault());

                if (address != null)
                {
                    address.Address.Street1 = siteItem.SiteStreetAddress1;
                    address.Address.Street2 = siteItem.SiteStreetAddress2;
                    address.Address.City = siteItem.SiteCity;
                    address.Address.StateId = siteItem.SiteState == null ? default(int?) : siteItem.SiteState.Id == 0 ? default(int?) : siteItem.SiteState.Id;
                    address.Address.ZipCode = siteItem.SiteZip;
                    address.Address.CountryId = siteItem.SiteCountry.Id;
                    address.Address.Phone = siteItem.SitePhone;
                    address.Address.UpdatedBy = email;
                    address.Address.UpdatedDate = DateTime.Now;
                }
            }
            else
            {
                site.SiteAddresses = new List<SiteAddress>();
                foreach (var siteAddressItem in siteItem.SiteAddresses)
                {
                    var siteAddress = new SiteAddress
                    {
                        Address = new Address
                        {
                            AddressTypeId = siteAddressItem.Address.AddressType.Id,
                            Street1 = siteAddressItem.Address.Street1,
                            Street2 = siteAddressItem.Address.Street2,
                            City = siteAddressItem.Address.City,
                            StateId = siteAddressItem.Address.State?.Id,
                            Province = siteAddressItem.Address.Province,
                            ZipCode = siteAddressItem.Address.ZipCode,
                            CountryId = siteAddressItem.Address.Country.Id,
                            Phone = siteItem.SitePhone,
                            CreatedBy = email,
                            CreatedDate = DateTime.Now
                        },
                        CreatedBy = email,
                        IsPrimaryAddress = false,
                        CreatedDate = DateTime.Now
                    };

                    site.SiteAddresses.Add(siteAddress);
                }
            }

            site.UpdatedBy = email;
            site.UpdatedDate = DateTime.Now;

            base.Repository.Save(site);

            return site;
        }

        /// <summary>
        /// Gets site scope types of an organization by its id
        /// </summary>
        /// <param name="organizationID">Id of the Organization</param>
        public List<SiteScopeType> GetScopeTypes(long organizationId)
        {
            LogMessage("GetScopeTypes (SiteManager)");

            return base.Repository.GetScopeTypes(organizationId);
        }

        /// <summary>
        /// Gets site scope types of an organization by its id
        /// </summary>
        /// <param name="organizationID">Id of the Organization</param>
        public List<SiteScopeType> GetScopeTypesBySite(int siteId)
        {
            LogMessage("GetScopeTypesBySite (SiteManager)");

            return base.Repository.GetScopeTypesBySite(siteId);
        }

        public List<Site> GetSitesByOrganizationId(long organizationId)
        {
            LogMessage("GetSitesByOrganizationId (SiteManager)");

            return base.Repository.GetSitesByOrganizationId(organizationId);
        }

        public Task<Site> GetPrimarySiteAsync(long organizationId)
        {
            LogMessage("GetPrimarySiteAsync (SiteManager)");

            return Task.FromResult(base.Repository.GetPrimarySite(organizationId));
        }

        public List<FlatSite> GetFlatSites()
        {
            return base.Repository.GetFlatSites();
        }

        public List<Site> GetSitesByOrganizationInclusive(string orgName)
        {
            return base.Repository.GetSitesByOrganizationInclusive(orgName);
        }

        public List<Site> GetSitesByComplianceNoFacility(Guid complianceApplicationId)
        {
            return base.Repository.GetSitesByComplianceNoFacility(complianceApplicationId);
        }
    }
}
