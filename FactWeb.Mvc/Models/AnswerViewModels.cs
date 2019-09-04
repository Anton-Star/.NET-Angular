using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;

namespace FactWeb.Mvc.Models
{
    public class AnswerAddHidesViewModel
    {
        public Guid AnswerId { get; set; }
        public List<Question> Questions { get; set; }
    }
}