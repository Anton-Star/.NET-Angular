using System;

namespace FactWeb.Model
{
    public class QuestionAnswer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public int? ScopeTypeId { get; set; }
        public string ScopeTypeName { get; set; }
    }
}
