using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ServiceTypeRepository : BaseRepository<ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(FactWebContext context) : base(context)
        {
        }

        public Task<List<ServiceType>> GetServiceTypeByMasterServiceTypeAsync(int masterServiceTypeId)
        {
            return base.FetchManyAsync(x => x.MasterServiceTypeId == masterServiceTypeId);
        }

        public List<ServiceType> GetServiceTypeByMasterServiceType(int masterServiceTypeId)
        {
            return base.FetchMany(x => x.MasterServiceTypeId == masterServiceTypeId);
        }
    }
}
