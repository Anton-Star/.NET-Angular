using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class AuditLogRepository : BaseRepository<AuditLog> , IAuditLogRepository
    {
        public AuditLogRepository(FactWebContext context) : base(context)
        {
        }
    }
}
