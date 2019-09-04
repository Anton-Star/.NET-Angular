using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class ApplicationStatusHistoryManager : BaseManager<ApplicationStatusHistoryManager, IApplicationStatusHistoryRepository, ApplicationStatusHistory>
    {
        public ApplicationStatusHistoryManager(IApplicationStatusHistoryRepository repository) : base(repository)
        {
        }

        public List<ApplicationStatusHistory> GetAllByApplicationId(Guid applicationUniqueId)
        {
            return base.Repository.GetAllByApplicationId(applicationUniqueId);
        }
    }
}
