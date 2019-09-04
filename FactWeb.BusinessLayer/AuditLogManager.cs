using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class AuditLogManager : BaseManager<AuditLogManager, IAuditLogRepository, AuditLog>
    {
        public AuditLogManager(IAuditLogRepository repository) : base(repository)
        {
        }
    }
}
