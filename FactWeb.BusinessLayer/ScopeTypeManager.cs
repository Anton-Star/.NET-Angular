using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ScopeTypeManager : BaseManager<ScopeTypeManager, IScopeTypeRepository, ScopeType>
    {
        public ScopeTypeManager(IScopeTypeRepository repository) : base(repository)
        {
        }

        public Task<List<ScopeType>> GetAllActiveAsync()
        {
            LogMessage("GetAllActiveAsync (ScopeTypeManager)");

            return this.Repository.GetAllActiveAsync();
        }
        public List<ScopeType> GetAllActive()
        {
            LogMessage("GetAllActive (ScopeTypeManager)");

            return this.Repository.GetAllActive();
        }
        public Task<List<ScopeType>> GetAllActiveNonArchivedAsync()
        {
            LogMessage("GetAllActiveNonArchivedAsync (ScopeTypeManager)");

            return this.Repository.GetAllActiveNonArchivedAsync();
        }
        public string IsDuplicateScope(int scopeId, string scopeTypeName, string importName)
        {
            LogMessage("IsDuplicateScope (ScopeTypeManager)");

            return this.Repository.IsDuplicateScope(scopeId, scopeTypeName, importName);
        }
    }
}
