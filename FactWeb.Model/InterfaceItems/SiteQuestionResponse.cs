using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteQuestionResponse
    {
        public SiteItems Site { get; set; }       
        public List<QuestionResponse> QuestionResponse { get; set; }
    }
}
