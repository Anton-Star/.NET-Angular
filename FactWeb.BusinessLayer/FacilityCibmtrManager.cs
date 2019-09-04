using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class FacilityCibmtrManager : BaseManager<FacilityCibmtrManager, IFacilityCibmtrRepository, FacilityCibmtr>
    {
        public FacilityCibmtrManager(IFacilityCibmtrRepository repository) : base(repository)
        {
        }

        public List<FacilityCibmtr> GetAllByFacility(int facilityId)
        {
            return base.Repository.GetAllByFacility(facilityId);
        }

        public List<FacilityCibmtr> GetAllByFacility(string facilityName)
        {
            return base.Repository.GetAllByFacility(facilityName);
        }

        public FacilityCibmtr GetByName(string centerName)
        {
            return base.Repository.GetByName(centerName);
        }
    }
}
