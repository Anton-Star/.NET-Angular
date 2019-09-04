using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class FacilityAccreditationRepository : BaseRepository<FacilityAccreditation>, IFacilityAccreditationRepository
    {
        public FacilityAccreditationRepository(FactWebContext context) : base(context)
        {
        }

       
    }
}
