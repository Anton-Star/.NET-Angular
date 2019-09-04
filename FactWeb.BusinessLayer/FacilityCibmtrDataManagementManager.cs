using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class FacilityCibmtrDataManagementManager : BaseManager<FacilityCibmtrDataManagementManager, IFacilityCibmtrDataManagementRepository, FacilityCibmtrDataManagement>
    {
        public FacilityCibmtrDataManagementManager(IFacilityCibmtrDataManagementRepository repository) : base(repository)
        {
        }

        public List<FacilityCibmtrDataManagement> GetAllByFacility(int facilityId)
        {
            return base.Repository.GetAllByFacility(facilityId);
        }

        public List<FacilityCibmtrDataManagement> GeetAllByFacility(string facilityName)
        {
            return base.Repository.GetAllByFacility(facilityName);
        }

        public List<FacilityCibmtrDataManagement> GetAllByCibmtr(Guid facilityCibmtrId)
        {
            return base.Repository.GetAllByCibmtr(facilityCibmtrId);
        }
    }
}
