using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class FacilityManager : BaseManager<FacilityManager, IFacilityRepository, Facility>
    {
        public FacilityManager(IFacilityRepository repository) : base(repository)
        {
        }

        public List<Facility> GetAllFacilities()
        {
            return base.Repository.GetAllFacilities();
        }

        /// <summary>
        /// Add or updates the facility record asynchronously
        /// </summary>
        /// <param name="facilityItem"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Task<Facility> AddOrUpdateAsync(FacilityItems facilityItem, string updatedBy)
        {
            LogMessage("AddOrUpdate (FacilityManager)");
            Facility facility = null;
            if (facilityItem.FacilityId != 0)
            {
                facility = this.UpdateAsync(facilityItem, updatedBy).Result;
            }
            else
            {
                facility = this.AddAsync(facilityItem, updatedBy).Result;
            }

            return Task.FromResult(this.GetById(facility.Id));
        }

        /// <summary>
        /// Add or updates the facility record
        /// </summary>
        /// <param name="facilityItem"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Facility AddOrUpdate(FacilityItems facilityItem, string updatedBy)
        {
            LogMessage("AddOrUpdate (FacilityManager)");
            Facility  facility = null;
            if (facilityItem.FacilityId != 0)
            {
                facility = this.Update(facilityItem, updatedBy);
            }
            else
            {
                facility= this.Add(facilityItem, updatedBy);
            }
            return this.GetById(facility.Id);
        }

        public Task<List<Facility>> GetAllActiveAsync()
        {
            return base.Repository.GetAllActiveAsync();
        }

        public List<Facility> GetAllActive()
        {
            return base.Repository.GetAllActive();
        }

        public List<Facility> GetAllByOrg(int organizationId)
        {
            return base.Repository.GetAllByOrg(organizationId);
        }

        public Facility GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        /// <summary>
        /// Add new facility record
        /// </summary>
        /// <param name="facilityItem"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Facility Add(FacilityItems facilityItem, string updatedBy)
        {
            Facility facility = new Facility();

            facility.Name = facilityItem.FacilityName;
            facility.IsActive = true;
            facility.FacilityDirectorId = facilityItem.FacilityDirectorId;
            facility.OtherSiteAccreditationDetails = facilityItem.OtherSiteAccreditationDetails == null ? "" : facilityItem.OtherSiteAccreditationDetails;
            facility.QMRestrictions = facilityItem.QMRestrictions;
            facility.NetCordMembership = facilityItem.NetCordMembership;
            facility.HRSA = facilityItem.HRSA;
            facility.MasterServiceTypeId = facilityItem.MasterServiceTypeId;
            //facility.FacilityAccreditationId = facilityItem.FacilityAccreditationId;
            facility.ServiceTypeId = facilityItem.ServiceTypeId;
            facility.CreatedBy = updatedBy;
            facility.CreatedDate = DateTime.Now;
            facility.NetcordMembershipTypeId = facilityItem.NetcordMembershipTypeId;
            facility.ProvisionalMembershipDate = !string.IsNullOrEmpty(facilityItem.ProvisionalMembershipDate) ? Convert.ToDateTime(facilityItem.ProvisionalMembershipDate) : (DateTime?)null;
            facility.AssociateMembershipDate = !string.IsNullOrEmpty(facilityItem.AssociateMembershipDate) ? Convert.ToDateTime(facilityItem.AssociateMembershipDate) : (DateTime?)null;
            facility.FullMembershipDate = !string.IsNullOrEmpty(facilityItem.FullMembershipDate) ? Convert.ToDateTime(facilityItem.FullMembershipDate) : (DateTime?)null;


            base.Repository.Add(facility);

            return facility;
        }

        public Task<string> GetCBCollectionSiteTypes(int facilityId)
        {
            return base.Repository.GetCBCollectionSiteTypes(facilityId);
        }

        /// <summary>
        /// Add new facility record asynchronously
        /// </summary>
        /// <param name="facilityItem"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Task<Facility> AddAsync(FacilityItems facilityItem, string updatedBy)
        {
            Facility facility = new Facility();

            facility.Name = facilityItem.FacilityName;
            facility.IsActive = true;
            facility.FacilityDirectorId = facilityItem.FacilityDirectorId;
            facility.OtherSiteAccreditationDetails = facilityItem.OtherSiteAccreditationDetails;
            facility.QMRestrictions = facilityItem.QMRestrictions;
            facility.NetCordMembership = facilityItem.NetCordMembership;
            facility.HRSA = facilityItem.HRSA;
            facility.MasterServiceTypeId = facilityItem.MasterServiceTypeId;
            //facility.FacilityAccreditationId = facilityItem.FacilityAccreditationId;
            facility.ServiceTypeId = facilityItem.ServiceTypeId;
            facility.CreatedBy = updatedBy;
            facility.CreatedDate = DateTime.Now;
            facility.NetcordMembershipTypeId = facilityItem.NetcordMembershipTypeId;
            facility.ProvisionalMembershipDate = !string.IsNullOrEmpty(facilityItem.ProvisionalMembershipDate) ? Convert.ToDateTime(facilityItem.ProvisionalMembershipDate) : (DateTime?)null;
            facility.AssociateMembershipDate = !string.IsNullOrEmpty(facilityItem.AssociateMembershipDate) ? Convert.ToDateTime(facilityItem.AssociateMembershipDate) : (DateTime?)null;
            facility.FullMembershipDate = !string.IsNullOrEmpty(facilityItem.FullMembershipDate) ? Convert.ToDateTime(facilityItem.FullMembershipDate) : (DateTime?)null;

            base.Repository.AddAsync(facility);

            return Task.FromResult(facility);
        }

        /// <summary>
        /// Update facility record
        /// </summary>
        /// <param name="facilityItem"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Facility Update(FacilityItems facilityItem, string updatedBy)
        {
            var facility = base.Repository.GetById(facilityItem.FacilityId);

            facility.Name = facilityItem.FacilityName;
            facility.IsActive = true;
            facility.FacilityDirectorId = facilityItem.FacilityDirectorId;
            facility.PrimaryOrganizationId = facilityItem.PrimaryOrganizationId;
            facility.OtherSiteAccreditationDetails = facilityItem.OtherSiteAccreditationDetails;
            facility.QMRestrictions = facilityItem.QMRestrictions;
            facility.NetCordMembership = facilityItem.NetCordMembership;
            facility.HRSA = facilityItem.HRSA;
            facility.MasterServiceTypeId = facilityItem.MasterServiceTypeId;
            //facility.FacilityAccreditationId = facilityItem.FacilityAccreditationId;
            facility.ServiceTypeId = facilityItem.ServiceTypeId;
            facility.UpdatedBy = updatedBy;
            facility.UpdatedDate = DateTime.Now;
            facility.NetcordMembershipTypeId = facilityItem.NetcordMembershipTypeId;
            facility.ProvisionalMembershipDate = !string.IsNullOrEmpty(facilityItem.ProvisionalMembershipDate) ? Convert.ToDateTime(facilityItem.ProvisionalMembershipDate) : (DateTime?)null;
            facility.AssociateMembershipDate = !string.IsNullOrEmpty(facilityItem.AssociateMembershipDate) ? Convert.ToDateTime(facilityItem.AssociateMembershipDate) : (DateTime?)null;
            facility.FullMembershipDate = !string.IsNullOrEmpty(facilityItem.FullMembershipDate) ? Convert.ToDateTime(facilityItem.FullMembershipDate) : (DateTime?)null;

            base.Repository.Save(facility);

            return facility;
        }

        /// <summary>
        /// Update facility record asynchronously
        /// </summary>
        /// <param name="facilityItem"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Task<Facility> UpdateAsync(FacilityItems facilityItem, string updatedBy)
        {
            var facility = base.Repository.GetById(facilityItem.FacilityId);

            facility.Name = facilityItem.FacilityName;
            facility.IsActive = true;
            facility.FacilityDirectorId = facilityItem.FacilityDirectorId;
            facility.PrimaryOrganizationId = facilityItem.PrimaryOrganizationId;
            facility.OtherSiteAccreditationDetails = facilityItem.OtherSiteAccreditationDetails;
            facility.QMRestrictions = facilityItem.QMRestrictions;
            facility.NetCordMembership = facilityItem.NetCordMembership;
            facility.HRSA = facilityItem.HRSA;
            facility.MasterServiceTypeId = facilityItem.MasterServiceTypeId;
            //facility.FacilityAccreditationId = facilityItem.FacilityAccreditationId;
            facility.ServiceTypeId = facilityItem.ServiceTypeId;
            facility.UpdatedBy = updatedBy;
            facility.UpdatedDate = DateTime.Now;
            facility.NetcordMembershipTypeId = facilityItem.NetcordMembershipTypeId;
            facility.ProvisionalMembershipDate = !string.IsNullOrEmpty(facilityItem.ProvisionalMembershipDate) ? Convert.ToDateTime(facilityItem.ProvisionalMembershipDate) : (DateTime?)null;
            facility.AssociateMembershipDate = !string.IsNullOrEmpty(facilityItem.AssociateMembershipDate) ? Convert.ToDateTime(facilityItem.AssociateMembershipDate) : (DateTime?)null;
            facility.FullMembershipDate = !string.IsNullOrEmpty(facilityItem.FullMembershipDate) ? Convert.ToDateTime(facilityItem.FullMembershipDate) : (DateTime?)null;

            base.Repository.SaveAsync(facility);

            return Task.FromResult(facility);
        }

        /// <summary>
        /// Delete facility record 
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Facility Delete(int facilityId, string updatedBy)
        {
            LogMessage("Delete (FacilityManager)");

            var facility = base.Repository.GetById(facilityId);
            facility.IsActive = false;

            base.Repository.Save(facility);

            return facility;
        }

        /// <summary>
        /// Delete facility record asynchronously 
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Task<Facility> DeleteAsync(int facilityId, string updatedBy)
        {
            LogMessage("DeleteAsync (FacilityManager)");

            var facility = base.Repository.GetById(facilityId);
            facility.IsActive = false;

            base.Repository.SaveAsync(facility);

            return Task.FromResult(facility);
        }
    }
}
