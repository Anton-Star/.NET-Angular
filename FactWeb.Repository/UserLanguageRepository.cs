using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class UserLanguageRepository : BaseRepository<UserLanguage>, IUserLanguageRepository
    {
        public UserLanguageRepository(FactWebContext context) : base(context)
        {
        }

        public List<UserLanguage> GetAllByUserId(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchMany(x => x.UserId == userId);
        }

        public Task<List<UserLanguage>> GetAllByUserIdAsync(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.UserId == userId);
        }
    }
}

