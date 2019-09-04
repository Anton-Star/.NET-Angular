using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FactWeb.Model.InterfaceItems;

namespace FactWeb.Mvc.Models
{
    public class InspectionScheduleDetailModels
    {
        public string InspectionScheduleId { get; set; }
        public string InspectionScheduleDetailId { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationTypeId { get; set; }
        public string OrganizationId { get; set; }
        public string SelectedUserId { get; set; }
        public string SelectedRoleId { get; set; }
        public string SelectedCategoryId { get; set; }
        public string SelectedFacilityId { get; set; } // todo remove
        public string SelectedSiteId { get; set; }
        public bool Lead { get; set; }
        public bool Mentor { get; set; }
        public string InspDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        
        public List<FacilitySiteItems> SelectedSiteList{ get; set; }
        


    }

}