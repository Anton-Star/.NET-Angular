using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IFacilityCibmtrDataManagementRepository : IRepository<FacilityCibmtrDataManagement>
    {
        List<FacilityCibmtrDataManagement> GetAllByFacility(int facilityId);
        List<FacilityCibmtrDataManagement> GetAllByFacility(string facilityName);
        List<FacilityCibmtrDataManagement> GetAllByCibmtr(Guid facilityCibmtrId);
    }
}
