using FileHelpers;

namespace FactWeb.Model.InterfaceItems
{
    [DelimitedRecord(",")]
    public class RequirementImportItem
    {
        public string FullStandardName { get; set; }
        public string StandardText { get; set; }
        public string Guidance { get; set; }
        public string QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string QuestionNote { get; set; }
        public string OptionText { get; set; }
        public string OptionSkipsTheseQuestions { get; set; }
        public string AdultAllogeneic { get; set; }
        public string AdultAutologous { get; set; }
        public string PediatricAllogeneic { get; set; }
        public string PediatricAutologous { get; set; }
        public string OffSiteStorage { get; set; }
        public string UnrelatedFixed { get; set; }
        public string RelatedFixed { get; set; }
        public string UnrelatedNonfixed { get; set; }
        public string RelatedNonfixed { get; set; }
        public string PedsAlloAddon { get; set; }
        public string PedsAutoAddon { get; set; }
        public string AdultAutoAddon { get; set; }
        public string AdultAlloAddon { get; set; }
        public string MoreThanMinimalAddon { get; set; }
        public string MinimalAddon { get; set; }
        public string CollectionAddon { get; set; }
        public string ProcessingAddon { get; set; }
        public string UnrelatedAddon { get; set; }
        public string RelatedAddon { get; set; }
        public string NonFixedAddon { get; set; }
        public string FixedAddon { get; set; }
        public string Last { get; set; }
        public string ExpectedAnswer { get; set; }
    }
}