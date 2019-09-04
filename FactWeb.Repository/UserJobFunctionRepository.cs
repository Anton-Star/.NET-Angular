using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class UserJobFunctionRepository : BaseRepository<UserJobFunction>, IUserJobFunctionRepository
    {
        public UserJobFunctionRepository(FactWebContext context) : base(context)
        {
        }

        public List<UserJobFunction> GetAllByUserId(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchMany(x => x.UserId == userId);
        }

        public Task<List<UserJobFunction>> GetAllByUserIdAsync(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.UserId == userId);
        }
    }
}

