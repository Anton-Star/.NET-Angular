using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class SiteAddressManager : BaseManager<SiteAddressManager, ISiteAddressRepository, SiteAddress>
    {
        public SiteAddressManager(ISiteAddressRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Get SiteAddress by Site Id 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<SiteAddress> GetBySiteId(int siteId)
        {
            LogMessage("GetBySiteId (SiteAddressManager)");

            return base.Repository.GetBySite(siteId);
        }
    }
}
