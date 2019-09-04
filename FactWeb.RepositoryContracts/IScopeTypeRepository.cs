using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IScopeTypeRepository : IRepository<ScopeType>
    {
        Task<List<ScopeType>> GetAllActiveAsync();
        List<ScopeType> GetAllActive();

        Task<List<ScopeType>> GetAllActiveNonArchivedAsync();        
        string IsDuplicateScope(int scopeId, string scopeTypeName, string importName);
    }
}
