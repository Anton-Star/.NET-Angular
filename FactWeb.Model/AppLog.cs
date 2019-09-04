using System;
using System.ComponentModel.DataAnnotations;

namespace FactWeb.Model
{
    public class AppLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required, MaxLength(255)]
        public string Thread { get; set; }
        [Required, MaxLength(50)]
        public string Level1 { get; set; }
        [Required, MaxLength(255)]
        public string Logger { get; set; }
        [Required]
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
