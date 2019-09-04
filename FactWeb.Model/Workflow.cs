using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Workflow : BaseModel
    {
        [Key, Column("WorkflowId")]
        public Guid Id { get; set; }
        [Column("WorkflowName")]
        public string Name { get; set; }
        [Column("WorkflowActivityName")]
        public string ActivityName { get; set; }
        public string K2SerialNumber { get; set; }
        public int K2ProcessInstanceId { get; set; }
        [Column("WorkflowActionName")]
        public string ActionName { get; set; }
        public Guid AssignedToUserId { get; set; }

        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }
        
    }
}
