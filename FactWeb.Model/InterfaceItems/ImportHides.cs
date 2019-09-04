using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ImportHides
    {
        public string StandardName { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public List<ImportHidesSection> ImportHidesSections { get; set; }

        public class ImportHidesSection
        {
            public string SectionNumber { get; set; }
            public int QuestionNumber { get; set; }
        }
    }
}
