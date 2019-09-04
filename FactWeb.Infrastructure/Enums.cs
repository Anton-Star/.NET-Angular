namespace FactWeb.Infrastructure
{
    public enum ServiceTypeEnum
    {
        ClinicalProgramCT = 1,
        ProcessingCT = 2,
        ApheresisCollection = 3,
        MarrowCollectionCT = 4
    }

    public enum QuestionTypes
    {
        TextArea = 1,
        RadioButtons = 2,
        Checkboxes = 3,
        DocumentUpload = 4,
        Date = 5,
        DateRange = 6,
        PeoplePicker = 7,
        Textbox = 8,
        Multiple = 9
    }
}
