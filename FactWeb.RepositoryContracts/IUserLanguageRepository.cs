using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IUserLanguageRepository : IRepository<UserLanguage>
    {
        List<UserLanguage> GetAllByUserId(Guid? id);

        Task<List<UserLanguage>> GetAllByUserIdAsync(Guid? id);

    }
}

