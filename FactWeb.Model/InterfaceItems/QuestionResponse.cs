using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public DocumentItem Document { get; set; }
        public Guid? UserId { get; set; }
        public UserItem User { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OtherText { get; set; }
        public string CoordinatorComment { get; set; }
        public UserItem UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ApplicationResponseCommentItem> ApplicationResponseComment { get; set; }
    }
}
