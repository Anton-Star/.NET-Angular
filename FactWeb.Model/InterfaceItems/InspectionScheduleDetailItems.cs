using System;

namespace FactWeb.Model.InterfaceItems
{
    public class InspectionScheduleDetailItems
    {
        public int InspectionScheduleDetailId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int InspectionCategoryId { get; set; }
        public string InspectionCategoryName { get; set; }
        public string InspectionDate { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public bool IsClinical { get; set; }
        public bool IsLead { get; set; }
        public bool IsMentor { get; set; }
        public bool IsArchive { get; set; }

        public bool IsInspectionComplete { get; set; }
        public bool ReviewedOutcomesData { get; set; }
        public DateTime? InspectionCompletionDate { get; set; }
        public string MentorFeedback { get; set; }
        public UserItem User { get; set; }


    }
}
