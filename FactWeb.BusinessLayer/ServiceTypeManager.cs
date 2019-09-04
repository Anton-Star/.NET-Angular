using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ServiceTypeManager : BaseManager<ServiceTypeManager, IServiceTypeRepository, ServiceType>
    {
        public ServiceTypeManager(IServiceTypeRepository repository) : base(repository)
        {
        }

        public async Task<List<ServiceType>> GetServiceTypeByMasterServiceTypeAsync(int masterServiceTypeId)
        {
            LogMessage("GetServiceTypeByMasterServiceTypeAsync (ServiceTypeManager)");

            return await this.Repository.GetServiceTypeByMasterServiceTypeAsync(masterServiceTypeId);
        }

        public List<ServiceType> GetServiceTypeByMasterServiceType(int masterServiceTypeId)
        {
            LogMessage("GetServiceTypeByMasterServiceType(ServiceTypeManager)");

            return this.Repository.GetServiceTypeByMasterServiceType(masterServiceTypeId);
        }

    }
}
