using System;
using System.ComponentModel.DataAnnotations;

namespace FactWeb.Model
{
    public class Bug
    {
        [Key]
        public int BugId { get; set; }
        public string BugText { get; set; }

        public string ApplicationVersion { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string BugUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Guid? ApplicationUniqueId { get; set; }
    }
}
