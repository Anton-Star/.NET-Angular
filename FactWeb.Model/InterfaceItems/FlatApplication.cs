using System;

namespace FactWeb.Model.InterfaceItems
{
    public class FlatApplication
    {
        public Guid ApplicationVersionId { get; set; }
        public string ApplicationVersionTitle { get; set; }
        public string ApplicationVersionNumber { get; set; }
        public bool? ApplicationVersionIsActive { get; set; }
        public Guid ApplicationSectionId { get; set; }
        public Guid? ParentApplicationSectionId { get; set; }
        public string ApplicationSectionName { get; set; }
        public bool? ApplicationSectionIsActive { get; set; }
        public string ApplicationSectionHelpText { get; set; }
        public bool? ApplicationSectionIsVariance { get; set; }
        public string ApplicationSectionOrder { get; set; }
        public string ApplicationSectionUniqueIdentifier { get; set; }
        public Guid? ApplicationSectionQuestionId { get; set; }
        public string ApplicationSectionQuestionText { get; set; }
        public string ApplicationSectionQuestionDescription { get; set; }
        public bool? ApplicationSectionQuestionIsActive { get; set; }
        public string ApplicationSectionQuestionComplianceNumber { get; set; }
        public byte? QuestionTypesFlag { get; set; }
        public Guid? ApplicationSectionQuestionAnswerId { get; set; }
        public string ApplicationSectionQuestionAnswerText { get; set; }
        public bool? ApplicationSectionQuestionAnswerIsExpectedAnswer { get; set; }
        public int? QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public int? ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; }
        public Guid? ApplicationSectionQuestionAnswerDisplayId { get; set; }
        public Guid? HidesQuestionId { get; set; }
    }
}
