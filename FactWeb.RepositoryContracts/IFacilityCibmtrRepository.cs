using FactWeb.Model;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IFacilityCibmtrRepository : IRepository<FacilityCibmtr>
    {
        List<FacilityCibmtr> GetAllByFacility(int facilityId);
        List<FacilityCibmtr> GetAllByFacility(string facilityName);
        FacilityCibmtr GetByName(string centerName);
    }
}
