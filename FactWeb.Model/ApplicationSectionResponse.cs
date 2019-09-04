using FactWeb.Model.InterfaceItems;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FactWeb.Model
{
    public class ApplicationSectionResponse
    {
        [JsonIgnore]
        public int ApplicationId { get; set; }
        [JsonIgnore]
        public Guid ApplicationUniqueId { get; set; }
        [JsonIgnore]
        public string ApplicationTypeName { get; set; }
        [JsonIgnore]
        public int SiteId { get; set; }
        [JsonIgnore]
        public string SiteName { get; set; }
        [JsonIgnore]
        public DateTime? SubmittedDate { get; set; }
        [JsonIgnore]
        public DateTime? DueDate { get; set; }
        [JsonIgnore]
        public DateTime? InspectionDate { get; set; }
        public Guid ApplicationSectionId { get; set; }
        [JsonIgnore]
        public Guid? ParentApplicationSectionId { get; set; }
        public string ApplicationSectionName { get; set; }
        public string ApplicationSectionHelpText { get; set; }
        public string ApplicationSectionUniqueIdentifier { get; set; }
        public bool HasQuestions { get; set; }
        public bool IsVisible { get; set; }
        public bool HasFlag { get; set; }
        public string SectionStatusName { get; set; }
        public bool HasRfiComment { get; set; }
        public bool HasCitationComment { get; set; }
        public bool HasFactOnlyComment { get; set; }
        public bool HasSuggestions { get; set; }
        public bool IsInspectionResponsesComplete { get; set; }
        [JsonIgnore]
        public string OrganizationName { get; set; }
        [JsonIgnore]
        public bool HasQmRestrictions { get; set; }
        [JsonIgnore]
        public bool IsReviewer { get; set; }

        public List<ApplicationSectionResponse> Children { get; set; }
        public List<Question> Questions { get; set; }
        public ApplicationSectionResponse NextSection { get; set; }
        public bool IgnoreChildren { get; set; }
        public string ScopeTypes { get; set; }
    }
}
