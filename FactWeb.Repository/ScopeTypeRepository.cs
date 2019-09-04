using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ScopeTypeRepository : BaseRepository<ScopeType>, IScopeTypeRepository
    {
        public ScopeTypeRepository(FactWebContext context) : base(context)
        {
        }

        public Task<List<ScopeType>> GetAllActiveAsync()
        {
            var scopeTypeList = base.FetchManyAsync(x => x.IsActive == true).Result.OrderBy(x => x.IsArchived).ToList();
            return Task.FromResult(scopeTypeList);
        }

        public List<ScopeType> GetAllActive()
        {
            return base.FetchMany(x => x.IsActive == true).OrderBy(x => x.IsArchived).ToList();
        }

        public Task<List<ScopeType>> GetAllActiveNonArchivedAsync()
        {
            return base.FetchManyAsync(x => x.IsActive == true && x.IsArchived == false);            
        }

        public string IsDuplicateScope(int scopeId, string  scopeTypeName, string importName)
        {
            var scopeTypeList = base.FetchMany(x => x.Name.ToLower() == scopeTypeName.Trim().ToLower());
            bool duplicateName = false;
            bool duplicateImportName = false;

            if (scopeId == 0)
            {
                duplicateName =  scopeTypeList.Count > 0 ? true : false;
            }
            else if (scopeTypeList.Count > 0)
            {
                duplicateName =  scopeTypeList[0].Id == scopeId ? false : true;
            }

            scopeTypeList = base.FetchMany(x => x.ImportName.ToLower() == importName.Trim().ToLower());

            if (scopeId == 0)
            {
                duplicateImportName = scopeTypeList.Count > 0 ? true : false;
            }
            else if (scopeTypeList.Count > 0)
            {
                duplicateImportName = scopeTypeList[0].Id == scopeId ? false : true;
            }

            if (duplicateName == true && duplicateImportName == true)
                return Constants.ScopeType.DuplicateScopeNameAndImportNames;
            if (duplicateName == true && duplicateImportName == false)
                return Constants.ScopeType.DuplicateScopeName;
            if (duplicateName == false && duplicateImportName == true)
                return Constants.ScopeType.DuplicateImportName;

            return string.Empty;


        }
    }
}
