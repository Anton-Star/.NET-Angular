using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationStatusHistoryRepository : IRepository<ApplicationStatusHistory>
    {
        List<ApplicationStatusHistory> GetAllByApplicationId(Guid applicationUniqueId);
    }
}
