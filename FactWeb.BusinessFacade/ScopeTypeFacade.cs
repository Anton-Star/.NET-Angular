using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class ScopeTypeFacade
    {
        private readonly Container container;

        public ScopeTypeFacade(Container container)
        {
            this.container = container;
        }

        public async Task<List<ScopeType>> GetAsync()
        {
            var scopeTypeManager = this.container.GetInstance<ScopeTypeManager>();

            return await scopeTypeManager.GetAllActiveAsync();
        }

        public async Task<List<ScopeType>> GetAllActiveNonArchivedAsync()
        {
            var scopeTypeManager = this.container.GetInstance<ScopeTypeManager>();

            return await scopeTypeManager.GetAllActiveNonArchivedAsync();
        }
        public ScopeType Save(ScopeTypeItem scopeTypeItem, string createdBy)
        {
            var scopeTypeManager = this.container.GetInstance<ScopeTypeManager>();
            ScopeType scopeType = null;

            if (scopeTypeItem.ScopeTypeId == 0) { // new case
                scopeType = new ScopeType();
                scopeType.Name = scopeTypeItem.Name;
                scopeType.ImportName = scopeTypeItem.ImportName;
                scopeType.IsActive = true;
                scopeType.IsArchived = false;
                scopeType.CreatedBy = createdBy;
                scopeType.CreatedDate = DateTime.Now;
                scopeTypeManager.Add(scopeType);
            }
            else //update 
            {
                scopeType = scopeTypeManager.GetById(scopeTypeItem.ScopeTypeId);
                scopeType.Name = scopeTypeItem.Name;
                scopeType.ImportName = scopeTypeItem.ImportName;
                scopeType.IsArchived = scopeTypeItem.IsArchived;
                scopeType.UpdatedBy = createdBy;
                scopeType.UpdatedDate = DateTime.Now;
                scopeTypeManager.Save(scopeType);
            }            

            return scopeType;
        }

        public string IsDuplicateScope(int scopeId, string scopeName, string importName)
        {
            var scopeTypeManager = this.container.GetInstance<ScopeTypeManager>();
            return  scopeTypeManager.IsDuplicateScope(scopeId, scopeName, importName);
        }

        public ScopeType Delete(int scopeTypeId, string updatedBy)
        {
            var scopeTypeManager = this.container.GetInstance<ScopeTypeManager>();

            var scopeType = scopeTypeManager.GetById(scopeTypeId);
            scopeType.IsActive = false;
            scopeType.UpdatedBy = updatedBy;
            scopeType.UpdatedDate = DateTime.Now;

            scopeTypeManager.Save(scopeType);

            return scopeType;
        }


    }
}
