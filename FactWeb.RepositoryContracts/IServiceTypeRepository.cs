using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IServiceTypeRepository :IRepository<ServiceType>
    {
        Task<List<ServiceType>> GetServiceTypeByMasterServiceTypeAsync(int masterServiceTypeId);
        List<ServiceType> GetServiceTypeByMasterServiceType(int masterServiceTypeId);
    }
    
}

