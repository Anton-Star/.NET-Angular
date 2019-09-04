using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactWeb.Repository
{
    public class ApplicationStatusHistoryRepository : BaseRepository<ApplicationStatusHistory>, IApplicationStatusHistoryRepository
    {
        public ApplicationStatusHistoryRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationStatusHistory> GetAllByApplicationId(Guid applicationUniqueId)
        {
            return base.FetchMany(
                x =>
                    x.Application.UniqueId == applicationUniqueId).OrderByDescending(y => y.CreatedDate).ToList();
        }
    }
}

