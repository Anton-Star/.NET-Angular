using System;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationSectionItemStatus
    {
        public Guid ApplicationSectionId { get; set; }
        public string UniqueIdentifier { get; set; }
        public string StatusName { get; set; }
        public bool HasRFIComments { get; set; }
        public bool HasCitationNotes { get; set; }
        public bool HasFACTOnlyComments{ get; set; }
        public bool HasAttachments { get; set; }
        public bool HasSuggestions { get; set; }
        public bool IsFlag { get; set; }
    }
}
