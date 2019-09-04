using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class FacilityAccreditationManager : BaseManager<FacilityAccreditationManager, IFacilityAccreditationRepository, FacilityAccreditation>
    {
        public FacilityAccreditationManager(IFacilityAccreditationRepository repository) : base(repository)
        {
        }

    }
}
