using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class DistanceRepository : BaseRepository<Distance>, IDistanceRepository
    {
        public DistanceRepository(FactWebContext context) : base(context)
        {
        }

        public Task<List<Distance>> GetAllWithInRadiusAsync(int siteAddressId, int radius)
        {
            return base.FetchManyAsync(x => x.DistanceInMiles >= radius && x.SiteAddressId == siteAddressId);
        }

        public List<SiteAddressDistance> GetInspectorsWithoutDistance()
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var data = objectContext.ExecuteStoreQuery<SiteAddressDistance>(
                "EXEC usp_getInspectorsWithoutDistance").ToList();

            return data;
        }
    }
}
