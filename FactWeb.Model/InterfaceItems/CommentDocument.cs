using System;

namespace FactWeb.Model.InterfaceItems
{
    public class CommentDocument
    {
        public int CommentId { get; set; }
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string RequestValues { get; set; }
    }
}
