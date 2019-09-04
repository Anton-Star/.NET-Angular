using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class AuditLog : BaseModel
    {
        [Key, Column("AuditLogId")]
        public int Id { get; set; }

        [Column("AuditLogUserName")]
        public string UserName { get; set; }

        [Column("AuditLogIPAddress")]
        public string IPAddress { get; set; }

        [Column("AuditLogDateTime")]
        public DateTimeOffset Date { get; set; }

        [Column("AuditLogDescription")]
        public string Description { get; set; }
    }
}
